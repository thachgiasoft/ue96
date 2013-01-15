using System;
using System.Data;
using System.Collections;

using Icson.Utils;
using System.Transactions;
using Icson.Objects;
using Icson.Objects.Basic;
using Icson.Objects.ImportData;
using Icson.DBAccess;
using Icson.DBAccess.ImportData;
using Icson.DBAccess.Basic;

namespace Icson.BLL.Basic
{
	/// <summary>
	/// Category 将放在内存中
	/// </summary>
	public class CategoryManager : IInitializable
	{
		private CategoryManager()
		{
		}
		private static CategoryManager _instance;
		public static CategoryManager GetInstance()
		{
			if ( _instance == null)
			{
				_instance = new CategoryManager();
				_instance.Init();
				SyncManager.GetInstance().RegisterLastVersion((int)AppEnum.Sync.Category);
			}
			return _instance;
		}
		private int attributeCapacity = 30;

		private static object categoryLocker = new object();
		private static Hashtable C1Hash = new Hashtable(10);
		private static Hashtable C2Hash = new Hashtable(50);
		private static Hashtable C3Hash = new Hashtable(100);

        private static Hashtable Color1Hash = new Hashtable(10);
        private static Hashtable Color2Hash = new Hashtable(100);

        private static Hashtable Size1Hash = new Hashtable(10);
        private static Hashtable Size2Hash = new Hashtable(100);

		public Hashtable GetC1Hash() { return C1Hash; }
		public Hashtable GetC2Hash() { return C2Hash; }
		public Hashtable GetC3Hash() { return C3Hash; }

        public Hashtable GetColor1Hash() { return Color1Hash; }
        public Hashtable GetColor2Hash() { return Color2Hash; }

        public Hashtable GetSize1Hash() { return Size1Hash; }
        public Hashtable GetSize2Hash() { return Size2Hash; }

		public void ImportCategory()
		{
			if ( !AppConfig.IsImportable)
				throw new BizException("Is Importable is false");

			/*  do not  use the following code after Data Pour in */
			string sql = " select top 1 * from category1 ";
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds) )
				throw new BizException("the table category is not empty");

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                try
                {
				    string sql1 = @"select cf.*, cfl.firstcategoryname from ipp2003..category_first cf, ipp2003..category_first_language cfl
								    where cf.sysno = cfl.firstcategorysysno and languageid = 'cn'";
				    DataSet ds1 = SqlHelper.ExecuteDataSet(sql1);
				    foreach(DataRow dr1 in ds1.Tables[0].Rows )
				    {
					    Category1Info oC1 = new Category1Info();
					    oC1.C1ID = Util.TrimNull(dr1["FirstCategoryID"]);
					    oC1.C1Name = Util.TrimNull(dr1["FirstCategoryName"]);
					    oC1.Status = Util.TrimIntNull(dr1["Status"]);
    					
					    this.Insert(oC1);

					    //insert into convert table
					    ImportInfo oC1Convert = new ImportInfo();
					    oC1Convert.OldSysNo = Util.TrimIntNull(dr1["SysNo"]);
					    oC1Convert.NewSysNo = oC1.SysNo;
					    new ImportDac().Insert(oC1Convert, "Category1");

					    string sql2 = @"select a.*, b.secondcategoryname from ipp2003..category_second a, ipp2003..category_second_language b
									    where a.sysno = b.secondcategorysysno and languageid = 'cn'
									    and firstcategorysysno = " + Util.TrimIntNull(dr1["SysNo"]);
					    DataSet ds2 = SqlHelper.ExecuteDataSet(sql2);
					    foreach( DataRow dr2 in ds2.Tables[0].Rows )
					    {
						    Category2Info oC2 = new Category2Info();
						    oC2.C1SysNo = oC1.SysNo;
						    oC2.C2ID = Util.TrimNull(dr2["SecondCategoryID"]);
						    oC2.C2Name = Util.TrimNull(dr2["SecondCategoryName"]);
						    oC2.Status = Util.TrimIntNull(dr2["Status"]);

						    this.Insert(oC2);

						    //insert into convert table
						    ImportInfo oC2Convert = new ImportInfo();
						    oC2Convert.OldSysNo = Util.TrimIntNull(dr2["SysNo"]);
						    oC2Convert.NewSysNo = oC2.SysNo;
						    new ImportDac().Insert(oC2Convert,"Category2");

						    string sql3 = @"select a.*, b.thirdcategoryname from ipp2003..category_third a, ipp2003..category_third_language b
										    where a.sysno = b.thirdcategorysysno and languageid = 'cn'
										    and secondcategorysysno = "+ Util.TrimIntNull(dr2["SysNo"]);
						    DataSet ds3 = SqlHelper.ExecuteDataSet(sql3);
						    foreach( DataRow dr3 in ds3.Tables[0].Rows )
						    {
							    Category3Info oC3 = new Category3Info();
							    oC3.C2SysNo = oC2.SysNo;
							    oC3.C3ID = Util.TrimNull(dr3["ThirdCategoryID"]);
							    oC3.C3Name = Util.TrimNull(dr3["ThirdCategoryName"]);
							    oC3.Status = Util.TrimIntNull(dr3["Status"]);

							    this.Insert(oC3);

							    //insert into convert table
							    ImportInfo oC3Convert = new ImportInfo();
							    oC3Convert.OldSysNo = Util.TrimIntNull(dr3["SysNo"]);
							    oC3Convert.NewSysNo = oC3.SysNo;
							    new ImportDac().Insert(oC3Convert, "Category3");

							    string sql4 = @"select a.*,b.AttributeName from ipp2003..category_attribute as a, ipp2003..category_attribute_language as b
										    where 
										    a.ThirdCategorySysNo = b.ThirdCategorySysNo
										    and a.AttributeID = b.AttributeID
										    and languageid = 'cn' and AttributeName <>''
										    and a.thirdcategorysysno = "+ Util.TrimIntNull(dr3["SysNo"]);
							    DataSet ds4 = SqlHelper.ExecuteDataSet(sql4);

							    int count = 0;
							    foreach( DataRow dr4 in ds4.Tables[0].Rows )
							    {
								    if ( count == 0)
								    {
									    InitAttribute(Util.TrimIntNull(oC3.SysNo));
									    count ++;
								    }

								    CategoryAttributeInfo oCA = new CategoryAttributeInfo();
								    oCA.C3SysNo = oC3.SysNo;
								    oCA.AttributeID = Util.TrimNull(dr4["AttributeID"]);
								    oCA.AttributeName = Util.TrimNull(dr4["AttributeName"]);
								    oCA.OrderNum = Util.TrimIntNull(dr4["SequenceNo"]);
								    oCA.Status = Util.TrimIntNull(dr4["Status"]);

								    new CategoryDac().UpdateAttributeByC3andAID(oCA);
							    }
						    }
					    }
				    }
                }
                catch(Exception ex) 
			    {
				    C1Hash.Clear();
				    C2Hash.Clear();
				    C3Hash.Clear();
				    throw ex;
			    }
                scope.Complete();
				
			}
			
		}

		#region Map
		private void Map(Category1Info oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.C1ID = Util.TrimNull(tempdr["C1ID"]);
			oParam.C1Name = Util.TrimNull(tempdr["C1Name"]);
			oParam.Status = Util.TrimIntNull(tempdr["Status"]);
		}
		private void Map(Category2Info oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.C1SysNo = Util.TrimIntNull(tempdr["C1SysNo"]);
			oParam.C2ID = Util.TrimNull(tempdr["C2ID"]);
			oParam.C2Name = Util.TrimNull(tempdr["C2Name"]);
			oParam.Status = Util.TrimIntNull(tempdr["Status"]);
		}
		private void Map(Category3Info oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.C2SysNo = Util.TrimIntNull(tempdr["C2SysNo"]);
			oParam.C3ID = Util.TrimNull(tempdr["C3ID"]);
			oParam.C3Name = Util.TrimNull(tempdr["C3Name"]);
			oParam.Status = Util.TrimIntNull(tempdr["Status"]);
			oParam.Online = Util.TrimIntNull(tempdr["Online"]);
            oParam.C3InventoryCycleTime = Util.TrimIntNull(tempdr["C3InventoryCycleTime"]);
            oParam.C3DMSWeight = Util.TrimDecimalNull(tempdr["C3DMSWeight"]);
		}


        private void Map(Color1Info oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.Color1ID = Util.TrimNull(tempdr["Color1ID"]);
            oParam.Color1Name = Util.TrimNull(tempdr["Color1Name"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
        }


        private void Map(Color2Info oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.Color1SysNo = Util.TrimIntNull(tempdr["Color1SysNo"]);
            oParam.Color2ID = Util.TrimNull(tempdr["Color2ID"]);
            oParam.Color2Name = Util.TrimNull(tempdr["Color2Name"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
        }


        private void Map(Size1Info oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.Size1ID = Util.TrimNull(tempdr["Size1ID"]);
            oParam.Size1Name = Util.TrimNull(tempdr["Size1Name"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
        }


        private void Map(Size2Info oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.Size1SysNo = Util.TrimIntNull(tempdr["Size1SysNo"]);
            oParam.Size2ID = Util.TrimNull(tempdr["Size2ID"]);
            oParam.Size2Name = Util.TrimNull(tempdr["Size2Name"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
        }

		#endregion

		#region Init
		public void Init()
		{
			InitC1();
			InitC2();
			InitC3();

            InitColor1();
            InitColor2();

            InitSize1();
            InitSize2();
		}
		private void InitC1()
		{
			lock ( categoryLocker )
			{
				C1Hash.Clear();

				string sql = " select * from category1 order by sysno";
				DataSet ds = SqlHelper.ExecuteDataSet(sql);
				if ( !Util.HasMoreRow(ds))
					return;
				foreach( DataRow dr in ds.Tables[0].Rows )
				{
					Category1Info item = new Category1Info();
					Map(item, dr);
					C1Hash.Add(item.SysNo, item);
				}
			}
		}
		private void InitC2()
		{
			lock ( categoryLocker )
			{
				C2Hash.Clear();
                string sql = " select * from category2 order by sysno";
				DataSet ds = SqlHelper.ExecuteDataSet(sql);
				if ( !Util.HasMoreRow(ds))
					return;
				foreach( DataRow dr in ds.Tables[0].Rows )
				{
					Category2Info item = new Category2Info();
					Map(item, dr);
					C2Hash.Add(item.SysNo, item);
				}
			}
		}
		private void InitC3()
		{
			lock( categoryLocker )
			{
				C3Hash.Clear();
                string sql = " select * from category3 order by sysno";
				DataSet ds = SqlHelper.ExecuteDataSet(sql);
				if ( !Util.HasMoreRow(ds))
					return;
				foreach( DataRow dr in ds.Tables[0].Rows )
				{
					Category3Info item = new Category3Info();
					Map(item, dr);
					C3Hash.Add(item.SysNo, item);
				}
			}
		}

        private void InitColor1()
        {
            lock (categoryLocker)
            {
                Color1Hash.Clear();

                string sql = " select * from color1 ";
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                    return;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Color1Info item = new Color1Info();
                    Map(item, dr);
                    Color1Hash.Add(item.SysNo, item);
                }
            }
        }
        private void InitColor2()
        {
            lock (categoryLocker)
            {
                Color2Hash.Clear();
                string sql = " select * from color2 ";
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                    return;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Color2Info item = new Color2Info();
                    Map(item, dr);
                    Color2Hash.Add(item.SysNo, item);
                }
            }
        }

        private void InitSize1()
        {
            lock (categoryLocker)
            {
                Size1Hash.Clear();

                string sql = " select * from Size1 ";
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                    return;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Size1Info item = new Size1Info();
                    Map(item, dr);
                    Size1Hash.Add(item.SysNo, item);
                }
            }
        }
        private void InitSize2()
        {
            lock (categoryLocker)
            {
                Size2Hash.Clear();
                string sql = " select * from Size2 ";
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                    return;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Size2Info item = new Size2Info();
                    Map(item, dr);
                    Size2Hash.Add(item.SysNo, item);
                }
            }
        }

		#endregion

        //判断是否有重复的大类名
        public bool IsExistC1Name(string paramName,int paramSysNo)
        {
            bool _return = false;
            foreach (Category1Info item in C1Hash.Values)
            {
                if (item.C1Name == paramName&&item.SysNo!=paramSysNo)
                {
                    _return = true;
                    return true;
                }
            }
            return _return;
        }

        //判断是否有重复的中类名
        public bool IsExistC2Name(string paramName, int paramSysNo, int paramC1SysNo)
        {
            bool _return = false;

            foreach (Category2Info item in C2Hash.Values)
            {
                if (item.C2Name == paramName && item.SysNo != paramSysNo && item.C1SysNo == paramC1SysNo)
                {
                    _return = true;
                    return true;
                }
            }
            return _return;
        }

        //判断是否有重复的小类名
        public bool IsExistC3Name(string paramName, int paramSysNo,int paramC2SysNo,int paramC1SysNo)
        {
            bool _return = false;
            int _c1sysno = -1;
            int _c2sysno = -1;
            bool _tmp=false;

            foreach (Category3Info item in C3Hash.Values)
            {
                if (item.SysNo == paramSysNo)
                {
                    _c2sysno = item.C2SysNo;
                    break;
                }
            }
            foreach (Category2Info item in C2Hash.Values)
            {
                if (item.SysNo == _c2sysno)
                {
                    _c1sysno = item.C1SysNo;
                    break;
                }
            }
            foreach (Category3Info item in C3Hash.Values)
            {
                if (item.C3Name == paramName && item.SysNo != paramSysNo&item.C2SysNo==paramC2SysNo)
                {
                    _tmp=false;
                    foreach (Category2Info item1 in C2Hash.Values)
                    {
                        if(item1.C1SysNo==paramC1SysNo)
                        {
                            _tmp = true;
                            break;
                        }
                    }
                    if (_tmp)
                    {
                        _return = true;
                        return true;
                    }
                }
            }
            return _return;
        }

        public void Insert(Category1Info oParam)
        {
            //foreach( Category1Info item in C1Hash.Values )
            //{
            //    if ( item.C1ID == oParam.C1ID )
            //        throw new BizException("the same first category ID exists");
            //}

            //自动生成ID号
            string _c1ID = "";
            int _tmpInt = -1;
            string _tmpStr = "";
            string sql = "select max(C1ID) from Category1";
            _tmpStr = SqlHelper.ExecuteScalar(sql).ToString();

            if (_tmpStr.Trim() == "")
                _tmpStr = "0";

            if (!Util.IsInteger(_tmpStr))
                throw new BizException("对不起，编号生成错误，不能添加");

            _tmpInt = Convert.ToInt32(_tmpStr);

            if (_tmpInt >= 99)//两位编号
            {
                throw new BizException("对不起，编号越界，不能再添加");
            }
            _tmpInt++;
            if (_tmpInt < 10)
                _c1ID = "0" + _tmpInt.ToString();
            else
                _c1ID = _tmpInt.ToString();
            oParam.C1ID = _c1ID;

            oParam.SysNo = SequenceDac.GetInstance().Create("Category_Sequence");
            new CategoryDac().Insert(oParam);
            SyncManager.GetInstance().SetDbLastVersion((int)AppEnum.Sync.Category);

            C1Hash.Add(oParam.SysNo, oParam);
        }

        public void InsertCate1(Category1Info oParam)
        {
            //foreach( Category1Info item in C1Hash.Values )
            //{
            //    if ( item.C1ID == oParam.C1ID )
            //        throw new BizException("the same first category ID exists");
            //}

            //自动生成ID号
            string _c1ID = "";
            int _tmpInt = -1;
            string _tmpStr = "";
            string sql = "select max(C1ID) from Category1";
            _tmpStr = SqlHelper.ExecuteScalar(sql).ToString();

            if (_tmpStr.Trim() == "")
                _tmpStr = "0";

            if (!Util.IsInteger(_tmpStr))
                throw new BizException("对不起，编号生成错误，不能添加");

            _tmpInt = Convert.ToInt32(_tmpStr);

            if (_tmpInt >= 99)//两位编号
            {
                throw new BizException("对不起，编号越界，不能再添加");
            }
            _tmpInt++;
            if (_tmpInt < 10)
                _c1ID = "0" + _tmpInt.ToString();
            else
                _c1ID = _tmpInt.ToString();
            oParam.C1ID = _c1ID;
            oParam.SysNo = oParam.SysNo;
            //oParam.SysNo = SequenceDac.GetInstance().Create("Category_Sequence");
            new CategoryDac().Insert(oParam);
            SyncManager.GetInstance().SetDbLastVersion((int)AppEnum.Sync.Category);

            C1Hash.Add(oParam.SysNo, oParam);
        }

        public void DeleteCategory1(int category1SysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                new CategoryDac().DeleteCategory1(category1SysNo);
                scope.Complete();
            }
        }

		public void Insert(Category2Info oParam)
		{
            //foreach( Category2Info item in C2Hash.Values )
            //{
            //    if ( item.C2ID == oParam.C2ID )
            //        throw new BizException("the same second category ID exists");
            //}

            string _c1ID = "";
            string _c2ID="";
            int _tmpInt = -1;
            string _tmpStr = "";

            Category1Info _oC1 = CategoryManager.GetInstance().GetC1Hash()[oParam.C1SysNo] as Category1Info;
            if(_oC1.C1ID.Trim()=="")
                throw new BizException("对不起，所属大类编号错误，无法添加");
            _c1ID=_oC1.C1ID.Trim();


            string sql = "select max(C2ID) from Category2 where c2id like '"+_c1ID+"%'";
            _tmpStr = SqlHelper.ExecuteScalar(sql).ToString();
            if (_tmpStr.Trim() == "")
                _tmpStr = _c1ID+"00";

            if(_tmpStr.Length!=4)
                throw new BizException("对不起，生成中类编号错误，无法添加");

            _tmpStr = _tmpStr.Substring(2,2);

            if(!Util.IsInteger(_tmpStr))
                throw new BizException("对不起，生成中类编号错误，无法添加");

            _tmpInt = Convert.ToInt32(_tmpStr);

            if (_tmpInt >= 99)//两位编号
            {
                throw new BizException("对不起，编号越界，不能再添加");
            }
            _tmpInt++;
            if (_tmpInt < 10)
                _c2ID = _c1ID+"0" + _tmpInt.ToString();
            else
                _c2ID = _c1ID+ _tmpInt.ToString();
            oParam.C2ID = _c2ID;

			oParam.SysNo = SequenceDac.GetInstance().Create("Category_Sequence");
			new CategoryDac().Insert(oParam);
			SyncManager.GetInstance().SetDbLastVersion( (int)AppEnum.Sync.Category );
			C2Hash.Add(oParam.SysNo, oParam);
		}

        //删除中类
        public void DeleteCategory2(int category2SysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                new CategoryDac().DeleteCategory2(category2SysNo);
                scope.Complete();
            }
        }

		public void Insert(Category3Info oParam)
		{
            //foreach( Category3Info item in C3Hash.Values )
            //{
            //    if ( item.C3ID == oParam.C3ID )
            //        throw new BizException("the same third category ID exists");
            //}

            string _c2ID = "";
            string _c3ID = "";
            int _tmpInt = -1;
            string _tmpStr = "";

            Category2Info _oC2 = CategoryManager.GetInstance().GetC2Hash()[oParam.C2SysNo] as Category2Info;
            if (_oC2.C2ID.Trim() == "")
                throw new BizException("对不起，所属中类编号错误，无法添加");
            _c2ID = _oC2.C2ID.Trim();

            string sql = "select max(C3ID) from Category3 where c3id like '" + _c2ID + "%'";
            _tmpStr = SqlHelper.ExecuteScalar(sql).ToString();
            if (_tmpStr.Trim() == "")
                _tmpStr = _c2ID + "00";
            if (_tmpStr.Length != 6)
                throw new BizException("对不起，生成小类编号错误，无法添加");

            _tmpStr = _tmpStr.Substring(4, 2);

            if (!Util.IsInteger(_tmpStr))
                throw new BizException("对不起，生成小类编号错误，无法添加");

            _tmpInt = Convert.ToInt32(_tmpStr);

            if (_tmpInt >= 99)//两位编号
            {
                throw new BizException("对不起，编号越界，不能再添加");
            }
            _tmpInt++;
            if (_tmpInt < 10)
                _c3ID = _c2ID + "0" + _tmpInt.ToString();
            else
                _c3ID = _c2ID + _tmpInt.ToString();
            oParam.C3ID = _c3ID;

			oParam.SysNo = SequenceDac.GetInstance().Create("Category_Sequence");
			new CategoryDac().Insert(oParam);
			SyncManager.GetInstance().SetDbLastVersion( (int)AppEnum.Sync.Category );
			C3Hash.Add(oParam.SysNo, oParam);
		}

        public void DeleteCategory3(int category3SysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                new CategoryDac().DeleteCategory3(category3SysNo);
                scope.Complete();
            }
        }

        public void Insert(Color1Info oParam)
        {
            foreach (Color1Info item in Color1Hash.Values)
            {
                if (item.Color1ID == oParam.Color1ID)
                    throw new BizException("the same first color ID exists");
            }
            oParam.SysNo = SequenceDac.GetInstance().Create("Color_Sequence");
            new ColorDac().Insert(oParam);
            SyncManager.GetInstance().SetDbLastVersion((int)AppEnum.Sync.Category);

            Color1Hash.Add(oParam.SysNo, oParam);
        }
        public void Insert(Color2Info oParam)
        {
            foreach (Color2Info item in Color2Hash.Values)
            {
                if (item.Color2ID == oParam.Color2ID)
                    throw new BizException("the same second color ID exists");
            }
            oParam.SysNo = SequenceDac.GetInstance().Create("Color_Sequence");
            new ColorDac().Insert(oParam);
            SyncManager.GetInstance().SetDbLastVersion((int)AppEnum.Sync.Category);
            Color2Hash.Add(oParam.SysNo, oParam);
        }

        public void Insert(Size1Info oParam)
        {
            foreach (Size1Info item in Size1Hash.Values)
            {
                if (item.Size1ID == oParam.Size1ID)
                    throw new BizException("the same first Size ID exists");
            }
            oParam.SysNo = SequenceDac.GetInstance().Create("Size_Sequence");
            new SizeDac().Insert(oParam);
            SyncManager.GetInstance().SetDbLastVersion((int)AppEnum.Sync.Category);

            Size1Hash.Add(oParam.SysNo, oParam);
        }
        public void Insert(Size2Info oParam)
        {
            foreach (Size2Info item in Size2Hash.Values)
            {
                if (item.Size2ID == oParam.Size2ID)
                    throw new BizException("the same second Size ID exists");
            }
            oParam.SysNo = SequenceDac.GetInstance().Create("Size_Sequence");
            new SizeDac().Insert(oParam);
            SyncManager.GetInstance().SetDbLastVersion((int)AppEnum.Sync.Category);
            Size2Hash.Add(oParam.SysNo, oParam);
        }

		public void Update(Category1Info oParam)
		{
            //foreach(Category1Info item in C1Hash.Values)
            //{
            //    if ( item.SysNo!=oParam.SysNo && item.C1ID == oParam.C1ID)
            //        throw new BizException("the same third category ID exists");
            //}
			
			new CategoryDac().Update(oParam);
			SyncManager.GetInstance().SetDbLastVersion( (int)AppEnum.Sync.Category );

			if ( C1Hash.ContainsKey(oParam.SysNo))
			{
				C1Hash.Remove(oParam.SysNo);
				C1Hash.Add(oParam.SysNo, oParam);
			}
		}
		public void Update(Category2Info oParam)
		{
            //foreach(Category2Info item in C2Hash.Values)
            //{
            //    if ( item.SysNo!=oParam.SysNo && item.C2ID == oParam.C2ID)
            //        throw new BizException("the same second category ID exists");
            //}
			new CategoryDac().Update(oParam);
			SyncManager.GetInstance().SetDbLastVersion( (int)AppEnum.Sync.Category );

			if ( C2Hash.ContainsKey(oParam.SysNo))
			{
				C2Hash.Remove(oParam.SysNo);
				C2Hash.Add(oParam.SysNo, oParam);
			}
		}
		public void Update(Category3Info oParam)
		{
            //foreach(Category3Info item in C3Hash.Values)
            //{
            //    if ( item.SysNo!=oParam.SysNo && item.C3ID == oParam.C3ID)
            //        throw new BizException("the same third category ID exists");
            //}
			new CategoryDac().Update(oParam);
			SyncManager.GetInstance().SetDbLastVersion( (int)AppEnum.Sync.Category );

			if ( C3Hash.ContainsKey(oParam.SysNo))
			{
				C3Hash.Remove(oParam.SysNo);
				C3Hash.Add(oParam.SysNo, oParam);
			}
		}

        public void Update(Color1Info oParam)
        {
            foreach (Color1Info item in Color1Hash.Values)
            {
                if (item.SysNo != oParam.SysNo && item.Color1ID == oParam.Color1ID)
                    throw new BizException("the same Color ID exists");
            }

            new ColorDac().Update(oParam);
            SyncManager.GetInstance().SetDbLastVersion((int)AppEnum.Sync.Category);

            if (Color1Hash.ContainsKey(oParam.SysNo))
            {
                Color1Hash.Remove(oParam.SysNo);
                Color1Hash.Add(oParam.SysNo, oParam);
            }
        }
        public void Update(Color2Info oParam)
        {
            foreach (Color2Info item in Color2Hash.Values)
            {
                if (item.SysNo != oParam.SysNo && item.Color2ID == oParam.Color2ID)
                    throw new BizException("the same Color ID exists");
            }
            new ColorDac().Update(oParam);
            SyncManager.GetInstance().SetDbLastVersion((int)AppEnum.Sync.Category);

            if (Color2Hash.ContainsKey(oParam.SysNo))
            {
                Color2Hash.Remove(oParam.SysNo);
                Color2Hash.Add(oParam.SysNo, oParam);
            }
        }

        public void Update(Size1Info oParam)
        {
            foreach (Size1Info item in Size1Hash.Values)
            {
                if (item.SysNo != oParam.SysNo && item.Size1ID == oParam.Size1ID)
                    throw new BizException("the same Size ID exists");
            }

            new SizeDac().Update(oParam);
            SyncManager.GetInstance().SetDbLastVersion((int)AppEnum.Sync.Category);

            if (Size1Hash.ContainsKey(oParam.SysNo))
            {
                Size1Hash.Remove(oParam.SysNo);
                Size1Hash.Add(oParam.SysNo, oParam);
            }
        }
        public void Update(Size2Info oParam)
        {
            foreach (Size2Info item in Size2Hash.Values)
            {
                if (item.SysNo != oParam.SysNo && item.Size2ID == oParam.Size2ID)
                    throw new BizException("the same Size ID exists");
            }
            new SizeDac().Update(oParam);
            SyncManager.GetInstance().SetDbLastVersion((int)AppEnum.Sync.Category);

            if (Size2Hash.ContainsKey(oParam.SysNo))
            {
                Size2Hash.Remove(oParam.SysNo);
                Size2Hash.Add(oParam.SysNo, oParam);
            }
        }

		public SortedList GetC1List()
		{
			if ( C1Hash.Count == 0 )
				return null;
			SortedList sl = new SortedList(10);
			foreach(Category1Info item in C1Hash.Values)
			{
				sl.Add(item,null);
			}
			return sl;
		}

        //按SysNo排序
        public SortedList GetC1List_DBC()
        {
            if (C1Hash.Count == 0)
                return null;
            SortedList sl = new SortedList(10);
            foreach (Category1Info item in C1Hash.Values)
            {
                sl.Add(item.SysNo,item);
            }
            return sl;
        }

		public SortedList GetC2List()
		{
			if ( C2Hash.Count == 0 )
				return null;
			SortedList sl = new SortedList(10);
			foreach(Category2Info item in C2Hash.Values)
			{
				sl.Add(item, null);
			}
			return sl;
		}

        //按SysNo排序
        public SortedList GetC2List_DBC()
        {
            if (C2Hash.Count == 0)
                return null;
            SortedList sl = new SortedList(10);
            foreach (Category2Info item in C2Hash.Values)
            {
                sl.Add(item.SysNo,item);
            }
            return sl;
        }

		public SortedList GetC3List()
		{
			if ( C3Hash.Count == 0 )
				return null;
			SortedList sl = new SortedList(10);
			foreach(Category3Info item in C3Hash.Values)
			{
				sl.Add(item, null);
			}
			return sl;
		}

        //按SysNo排序
        public SortedList GetC3List_DBC()
        {
            if (C3Hash.Count == 0)
                return null;
            SortedList sl = new SortedList(10);
            foreach (Category3Info item in C3Hash.Values)
            {
                sl.Add(item.SysNo,item);
            }
            return sl;
        }

        public SortedList GetColor1List()
        {
            if (Color1Hash.Count == 0)
                return null;
            SortedList sl = new SortedList(10);
            foreach (Color1Info item in Color1Hash.Values)
            {
                sl.Add(item, null);
            }
            return sl;
        }
        public SortedList GetColor2List()
        {
            if (Color2Hash.Count == 0)
                return null;
            SortedList sl = new SortedList(10);
            foreach (Color2Info item in Color2Hash.Values)
            {
                sl.Add(item, null);
            }
            return sl;
        }

        public SortedList GetSize1List()
        {
            if (Size1Hash.Count == 0)
                return null;
            SortedList sl = new SortedList(10);
            foreach (Size1Info item in Size1Hash.Values)
            {
                sl.Add(item, null);
            }
            return sl;
        }
        public SortedList GetSize2List()
        {
            if (Size2Hash.Count == 0)
                return null;
            SortedList sl = new SortedList(10);
            foreach (Size2Info item in Size2Hash.Values)
            {
                sl.Add(item, null);
            }
            return sl;
        }

		//////////////////////////////////////////////////////////////////////////
		public CategoryAttributeInfo Load(int paramAttributeSysNo)
		{
			string sql = "select * from category_attribute where sysno=" + paramAttributeSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( !Util.HasMoreRow(ds))
				return null;
			CategoryAttributeInfo item = new CategoryAttributeInfo();
			Map( item, ds.Tables[0].Rows[0]);
			return item;
		}
		public bool IsAttribute(int paramC3SysNo)
		{
			string sql = "select top 1 sysno from category_attribute where c3sysno = " + paramC3SysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds))
				return true;
			else
				return false;
		}
		public SortedList GetAttributeList(int paramC3SysNo)
		{
			string sql = " select * from category_attribute where c3sysno = " + paramC3SysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( !Util.HasMoreRow(ds))
				return null;
			SortedList sl = new SortedList(attributeCapacity);
			foreach( DataRow dr in ds.Tables[0].Rows )
			{
				CategoryAttributeInfo item = new CategoryAttributeInfo();
				Map(item, dr);
				sl.Add(item, null);
			}
			return sl;
		}

		private void Map(CategoryAttributeInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.C3SysNo = Util.TrimIntNull(tempdr["C3SysNo"]);
			oParam.AttributeID = Util.TrimNull(tempdr["AttributeID"]);
			oParam.AttributeName = Util.TrimNull(tempdr["AttributeName"]);
			oParam.OrderNum = Util.TrimIntNull(tempdr["OrderNum"]);
			oParam.Status = Util.TrimIntNull(tempdr["Status"]);
			oParam.AttributeType  = Util.TrimIntNull(tempdr["AttributeType"]);
		}
		public void InitAttribute(int paramC3SysNo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				CategoryDac o = new CategoryDac();

				CategoryAttributeInfo oItem = new CategoryAttributeInfo();
				for(int i=1; i<=attributeCapacity; i++ )
				{
					oItem.C3SysNo = paramC3SysNo;
					oItem.AttributeID = "A" + i.ToString();
					oItem.AttributeName = "" + i.ToString();
					oItem.OrderNum = i;
					oItem.Status = (int)AppEnum.BiStatus.InValid;
					oItem.AttributeType = (int)AppEnum.AttributeType.Text;
					o.InsertAttribute(oItem);
				}

				scope.Complete();
            }
		}
		public int UpdateAttribute(CategoryAttributeInfo oParam)
		{
			return new CategoryDac().UpdateAttribute(oParam);
		}

		public void MoveTop(CategoryAttributeInfo oParam)
		{
			if ( oParam.OrderNum == 1 )
			{
				throw new BizException("it's the top one already");
			}
			SortedList sl = GetAttributeList(oParam.C3SysNo);
			if ( sl == null )
			{
				throw new BizException("no attribute for this third category");
			}

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				CategoryDac o = new CategoryDac();

				foreach(CategoryAttributeInfo item  in sl.Keys)
				{
					if ( item.OrderNum < oParam.OrderNum )
					{
						item.OrderNum = item.OrderNum+1;
						o.SetOrderNum(item);
					}
				}
				oParam.OrderNum = 1;
				o.SetOrderNum(oParam);

				scope.Complete();
            }
		}
		public void MoveUp(CategoryAttributeInfo oParam)
		{
			if ( oParam.OrderNum == 1 )
			{
				throw new BizException("it's the first one, can't be moved up");
			}
			SortedList sl = GetAttributeList(oParam.C3SysNo);
			if ( sl == null )
			{
				throw new BizException("no attributes");
			}

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				CategoryDac o = new CategoryDac();

				foreach(CategoryAttributeInfo item  in sl.Keys)
				{
					if ( item.OrderNum == oParam.OrderNum-1 )
					{
						item.OrderNum += 1;
						o.SetOrderNum(item);
					}
				}
				oParam.OrderNum -= 1;
				o.SetOrderNum(oParam);

				scope.Complete();
            }
		}
		public void MoveDown(CategoryAttributeInfo oParam)
		{
			if ( oParam.OrderNum == attributeCapacity )
			{
				throw new BizException("it's the last one, can't be moved down");
			}
			SortedList sl = GetAttributeList(oParam.C3SysNo);
			if ( sl == null )
			{
				throw new BizException("no attributes");
			}

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				CategoryDac o = new CategoryDac();

				foreach(CategoryAttributeInfo item  in sl.Keys)
				{
					if ( item.OrderNum == oParam.OrderNum+1 )
					{
						item.OrderNum -= 1;
						o.SetOrderNum(item);
					}
				}
				oParam.OrderNum += 1;
				o.SetOrderNum(oParam);

				scope.Complete();
            }
		}
		public void MoveBottom(CategoryAttributeInfo oParam)
		{
			if ( oParam.OrderNum == attributeCapacity )
			{
				throw new BizException("it's the bottom one already");
			}
			SortedList sl = GetAttributeList(oParam.C3SysNo);
			if ( sl == null )
			{
				throw new BizException("no attributes");
			}

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				CategoryDac o = new CategoryDac();

				foreach(CategoryAttributeInfo item  in sl.Keys)
				{
					if ( item.OrderNum > oParam.OrderNum )
					{
						item.OrderNum = item.OrderNum-1;
						o.SetOrderNum(item);
					}
				}
				oParam.OrderNum = attributeCapacity;
				o.SetOrderNum(oParam);

				scope.Complete();
            }
		}

		//old version for category attribute option
		/*
		//---------------------------CategoryAttributeOption------------------------
		public CategoryAttributeOptionInfo LoadAttributeOption(int paramAttributeOptionSysNo)
		{
			string sql = "select * from category_attribute_option where sysno=" + paramAttributeOptionSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( !Util.HasMoreRow(ds))
				return null;
			CategoryAttributeOptionInfo item = new CategoryAttributeOptionInfo();
			Map( item, ds.Tables[0].Rows[0]);
			return item;
		}
		public bool IsAttributeOption(int paramAttributeSysNo)
		{
			string sql = "select top 1 sysno from category_attribute_option where AttributeSysNo = " + paramAttributeSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds))
				return true;
			else
				return false;
		}

		public bool IsExistAttributeOption(CategoryAttributeOptionInfo oParam)
		{
			return new CategoryDac().IsExistAttributeOption(oParam);
		}

		public SortedList GetAttributeOptionList(int paramAttributeSysNo)
		{
			string sql = " select * from category_attribute_option where attributesysno = " + paramAttributeSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( !Util.HasMoreRow(ds))
				return null;
			SortedList sl = new SortedList(attributeCapacity);
			foreach( DataRow dr in ds.Tables[0].Rows )
			{
				CategoryAttributeOptionInfo item = new CategoryAttributeOptionInfo();
				Map(item, dr);
				sl.Add(item, null);
			}
			return sl;
		}
		private void Map(CategoryAttributeOptionInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.AttributeSysNo = Util.TrimIntNull(tempdr["AttributeSysNo"]);
			oParam.AttributeOptionName = Util.TrimNull(tempdr["AttributeOptionName"]);
			oParam.OrderNum = Util.TrimIntNull(tempdr["OrderNum"]);
			oParam.IsRecommend  = Util.TrimIntNull(tempdr["IsRecommend"]);
			oParam.Status = Util.TrimIntNull(tempdr["Status"]);
		}
		public void InitAttributeOption(int paramAttributeSysNo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				CategoryDac o = new CategoryDac();

				CategoryAttributeOptionInfo oItem = new CategoryAttributeOptionInfo();
				for(int i=1; i<=attributeCapacity; i++ )
				{
					oItem.AttributeSysNo = paramAttributeSysNo;
					oItem.AttributeOptionName = "";
					oItem.OrderNum = i;
					oItem.IsRecommend = (int)AppEnum.YNStatus.No;
					oItem.Status = (int)AppEnum.BiStatus.InValid;
					o.InsertAttributeOption(oItem);
				}

				scope.Complete();
            }
		}
	
		public int GetCatetoryAttributeOptionNewOrderNum(CategoryAttributeOptionInfo oParam)
		{
			return new CategoryDac().GetCatetoryAttributeOptionNewOrderNum(oParam);
		}
	
		public int InsertAttributeOption(CategoryAttributeOptionInfo oParam)
		{
			return new CategoryDac().InsertAttributeOption(oParam);
		}

		public int UpdateAttributeOption(CategoryAttributeOptionInfo oParam)
		{
			return new CategoryDac().UpdateAttributeOption(oParam);
		}

		public void MoveTop(CategoryAttributeOptionInfo oParam)
		{
			if ( oParam.OrderNum == 1 )
			{
				throw new BizException("it's the top one already");
			}
			SortedList sl = GetAttributeOptionList(oParam.AttributeSysNo);
			if ( sl == null )
			{
				throw new BizException("no attribute for this third category");
			}

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				CategoryDac o = new CategoryDac();

				foreach(CategoryAttributeOptionInfo item  in sl.Keys)
				{
					if ( item.OrderNum < oParam.OrderNum )
					{
						item.OrderNum = item.OrderNum+1;
						o.SetOrderNum(item);
					}
				}
				oParam.OrderNum = 1;
				o.SetOrderNum(oParam);

				scope.Complete();
            }
		}
		public void MoveUp(CategoryAttributeOptionInfo oParam)
		{
			if ( oParam.OrderNum == 1 )
			{
				throw new BizException("it's the first one, can't be moved up");
			}
			SortedList sl = GetAttributeOptionList(oParam.AttributeSysNo);
			if ( sl == null )
			{
				throw new BizException("no attributes");
			}

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				CategoryDac o = new CategoryDac();

				foreach(CategoryAttributeOptionInfo item  in sl.Keys)
				{
					if ( item.OrderNum == oParam.OrderNum-1 )
					{
						item.OrderNum += 1;
						o.SetOrderNum(item);
					}
				}
				oParam.OrderNum -= 1;
				o.SetOrderNum(oParam);

				scope.Complete();
            }
		}
		public void MoveDown(CategoryAttributeOptionInfo oParam)
		{
			SortedList sl = GetAttributeOptionList(oParam.AttributeSysNo);
			if ( sl == null )
			{
				throw new BizException("no attributes");
			}

			if ( oParam.OrderNum == sl.Count )
			{
				throw new BizException("it's the last one, can't be moved down");
			}

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				CategoryDac o = new CategoryDac();

				foreach(CategoryAttributeOptionInfo item  in sl.Keys)
				{
					if ( item.OrderNum == oParam.OrderNum+1 )
					{
						item.OrderNum -= 1;
						o.SetOrderNum(item);
					}
				}
				oParam.OrderNum += 1;
				o.SetOrderNum(oParam);

				scope.Complete();
            }
		}
		public void MoveBottom(CategoryAttributeOptionInfo oParam)
		{
			SortedList sl = GetAttributeOptionList(oParam.AttributeSysNo);
			if ( sl == null )
			{
				throw new BizException("no attributes");
			}

			if ( oParam.OrderNum == sl.Count )
			{
				throw new BizException("it's the last one, can't be moved down");
			}

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				CategoryDac o = new CategoryDac();

				foreach(CategoryAttributeOptionInfo item  in sl.Keys)
				{
					if ( item.OrderNum > oParam.OrderNum )
					{
						item.OrderNum = item.OrderNum-1;
						o.SetOrderNum(item);
					}
				}
				oParam.OrderNum = attributeCapacity;
				o.SetOrderNum(oParam);

				scope.Complete();
            }
		}
		//---------------------------CategoryAttributeOption------------------------
		*/

		//---------------------------Category_Attribute2_Option------------------------
		public CategoryAttribute2OptionInfo LoadAttribute2Option(int paramAttribute2OptionSysNo)
		{
			string sql = "select * from category_attribute2_option where sysno=" + paramAttribute2OptionSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( !Util.HasMoreRow(ds))
				return null;
			CategoryAttribute2OptionInfo item = new CategoryAttribute2OptionInfo();
			Map( item, ds.Tables[0].Rows[0]);
			return item;
		}
		public bool IsAttribute2Option(int paramAttribute2SysNo)
		{
			string sql = "select top 1 sysno from category_attribute2_option where Attribute2SysNo = " + paramAttribute2SysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds))
				return true;
			else
				return false;
		}

		public bool IsExistAttribute2Option(CategoryAttribute2OptionInfo oParam)
		{
			return new CategoryDac().IsExistAttribute2Option(oParam);
		}

		public SortedList GetAttribute2OptionList(int paramAttribute2SysNo)
		{
			string sql = " select * from category_attribute2_option where attribute2sysno = " + paramAttribute2SysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( !Util.HasMoreRow(ds))
				return null;
			SortedList sl = new SortedList(ds.Tables[0].Rows.Count);
			foreach( DataRow dr in ds.Tables[0].Rows )
			{
				CategoryAttribute2OptionInfo item = new CategoryAttribute2OptionInfo();
				Map(item, dr);
				sl.Add(item, null);
			}
			return sl;
		}
		private void Map(CategoryAttribute2OptionInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.Attribute2SysNo = Util.TrimIntNull(tempdr["Attribute2SysNo"]);
			oParam.Attribute2OptionName = Util.TrimNull(tempdr["Attribute2OptionName"]);
			oParam.OrderNum = Util.TrimIntNull(tempdr["OrderNum"]);
			oParam.IsRecommend  = Util.TrimIntNull(tempdr["IsRecommend"]);
			oParam.Status = Util.TrimIntNull(tempdr["Status"]);
		}		
	
		public int GetCatetoryAttribute2OptionNewOrderNum(CategoryAttribute2OptionInfo oParam)
		{
			return new CategoryDac().GetCatetoryAttribute2OptionNewOrderNum(oParam);
		}

        public int GetOptionMaxSysNo(int Attribute1SysNo)
        {
            return new CategoryDac().GetMaxSysNo(Attribute1SysNo);
        }

		public int InsertAttribute2Option(CategoryAttribute2OptionInfo oParam)
		{
			return new CategoryDac().InsertAttribute2Option(oParam);
		}

        //删除二级分类选项
        public int DeleteAttribute2Option(int optionSysNo)
        {
            return new CategoryDac().DeleteAttribute2Option(optionSysNo);
        }

		public int UpdateAttribute2Option(CategoryAttribute2OptionInfo oParam)
		{
			return new CategoryDac().UpdateAttribute2Option(oParam);
		}

		public void MoveTop(CategoryAttribute2OptionInfo oParam)
		{
			if ( oParam.OrderNum == 1 )
			{
				throw new BizException("it's the top one already");
			}
			SortedList sl = GetAttribute2OptionList(oParam.Attribute2SysNo);
			if ( sl == null )
			{
				throw new BizException("no attribute for this third category");
			}

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				CategoryDac o = new CategoryDac();

				foreach(CategoryAttribute2OptionInfo item  in sl.Keys)
				{
					if ( item.OrderNum < oParam.OrderNum )
					{
						item.OrderNum = item.OrderNum+1;
						o.SetOrderNum(item);
					}
				}
				oParam.OrderNum = 1;
				o.SetOrderNum(oParam);

				scope.Complete();
            }
		}
		public void MoveUp(CategoryAttribute2OptionInfo oParam)
		{
			if ( oParam.OrderNum == 1 )
			{
				throw new BizException("it's the first one, can't be moved up");
			}
			SortedList sl = GetAttribute2OptionList(oParam.Attribute2SysNo);
			if ( sl == null )
			{
				throw new BizException("no attributes");
			}

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				CategoryDac o = new CategoryDac();

				foreach(CategoryAttribute2OptionInfo item  in sl.Keys)
				{
					if ( item.OrderNum == oParam.OrderNum-1 )
					{
						item.OrderNum += 1;
						o.SetOrderNum(item);
					}
				}
				oParam.OrderNum -= 1;
				o.SetOrderNum(oParam);

				scope.Complete();
            }
		}
		public void MoveDown(CategoryAttribute2OptionInfo oParam)
		{
			SortedList sl = GetAttribute2OptionList(oParam.Attribute2SysNo);
			if ( sl == null )
			{
				throw new BizException("no attributes");
			}

			if ( oParam.OrderNum == sl.Count )
			{
				throw new BizException("it's the last one, can't be moved down");
			}

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				CategoryDac o = new CategoryDac();

				foreach(CategoryAttribute2OptionInfo item  in sl.Keys)
				{
					if ( item.OrderNum == oParam.OrderNum+1 )
					{
						item.OrderNum -= 1;
						o.SetOrderNum(item);
					}
				}
				oParam.OrderNum += 1;
				o.SetOrderNum(oParam);

				scope.Complete();
            }
		}
		public void MoveBottom(CategoryAttribute2OptionInfo oParam)
		{
			SortedList sl = GetAttribute2OptionList(oParam.Attribute2SysNo);
			if ( sl == null )
			{
				throw new BizException("no attributes");
			}

			if ( oParam.OrderNum == sl.Count )
			{
				throw new BizException("it's the last one, can't be moved down");
			}

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				CategoryDac o = new CategoryDac();

				foreach(CategoryAttribute2OptionInfo item  in sl.Keys)
				{
					if ( item.OrderNum > oParam.OrderNum )
					{
						item.OrderNum = item.OrderNum-1;
						o.SetOrderNum(item);
					}
				}
				oParam.OrderNum = attributeCapacity;
				o.SetOrderNum(oParam);

				scope.Complete();
            }
		}
		//---------------------------Category_Attribute2_Option------------------------

		//---------------------------Category_Attribute1-------------------------
		public bool IsExistAttribute1(CategoryAttribute1Info oParam)
		{
			return new CategoryDac().IsExistAttribute1(oParam);
		}

		public int GetCatetoryAttribute1NewOrderNum(CategoryAttribute1Info oParam)
		{
			return new CategoryDac().GetCatetoryAttribute1NewOrderNum(oParam);
		}

		public CategoryAttribute1Info LoadCategoryAttribute1(int paramAttribute1SysNo)
		{
			string sql = "select * from category_attribute1 where sysno=" + paramAttribute1SysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( !Util.HasMoreRow(ds))
				return null;
			CategoryAttribute1Info item = new CategoryAttribute1Info();
			Map( item, ds.Tables[0].Rows[0]);
			return item;
		}
		public bool IsAttribute1(int paramC3SysNo)
		{
			string sql = "select top 1 sysno from category_attribute1 where c3sysno = " + paramC3SysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds))
				return true;
			else
				return false;
		}
		public SortedList GetAttribute1List(int paramC3SysNo)
		{
			string sql = " select * from category_attribute1 where c3sysno = " + paramC3SysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( !Util.HasMoreRow(ds))
				return null;
			SortedList sl = new SortedList(ds.Tables[0].Rows.Count);
			foreach( DataRow dr in ds.Tables[0].Rows )
			{
				CategoryAttribute1Info item = new CategoryAttribute1Info();
				Map(item, dr);
				sl.Add(item, null);
			}
			return sl;
		}

		private void Map(CategoryAttribute1Info oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.C3SysNo = Util.TrimIntNull(tempdr["C3SysNo"]);
			oParam.Attribute1ID = Util.TrimNull(tempdr["Attribute1ID"]);
			oParam.Attribute1Name = Util.TrimNull(tempdr["Attribute1Name"]);
			oParam.OrderNum = Util.TrimIntNull(tempdr["OrderNum"]);
			oParam.Status = Util.TrimIntNull(tempdr["Status"]);
			oParam.Attribute1Type  = Util.TrimIntNull(tempdr["Attribute1Type"]);
		}
		
		public int InsertAttribute1(CategoryAttribute1Info oParam)
		{
            string _atrribute1ID = "";
            string _tmpStr = "";
            int _tmpInt = -1;
            string sql = "select max(Attribute1ID) from Category_Attribute1 where C3SysNo=" + oParam.C3SysNo.ToString();
            _tmpStr = SqlHelper.ExecuteScalar(sql).ToString();
            if (_tmpStr.Trim() == "")
                _tmpStr = "0";
            if (!Util.IsInteger(_tmpStr))
                throw new BizException("对不起，生成编号错误，不能添加");
            _tmpInt = Convert.ToInt32(_tmpStr);
            _tmpInt++;

            _atrribute1ID = _tmpInt.ToString();

            oParam.Attribute1ID = _atrribute1ID;

			return new CategoryDac().InsertAttribute1(oParam);
		}

		public int UpdateAttribute1(CategoryAttribute1Info oParam)
		{
			return new CategoryDac().UpdateAttribute1(oParam);
		}

        //删除商品属性一级分类
        public int DeleteAttribute1(int attribute1SysNo)
        {
            int _return = 0;
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                _return = new CategoryDac().DeleteAttribute1(attribute1SysNo);
                scope.Complete();
            }
            return _return;
        }

		public void MoveTop(CategoryAttribute1Info oParam)
		{
			if ( oParam.OrderNum == 1 )
			{
				throw new BizException("it's the top one already");
			}
			SortedList sl = GetAttribute1List(oParam.C3SysNo);
			if ( sl == null )
			{
				throw new BizException("no attribute for this third category");
			}

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				CategoryDac o = new CategoryDac();

				foreach(CategoryAttribute1Info item  in sl.Keys)
				{
					if ( item.OrderNum < oParam.OrderNum )
					{
						item.OrderNum = item.OrderNum+1;
						o.SetOrderNum(item);
					}
				}
				oParam.OrderNum = 1;
				o.SetOrderNum(oParam);

				scope.Complete();
            }
		}
		public void MoveUp(CategoryAttribute1Info oParam)
		{
			if ( oParam.OrderNum == 1 )
			{
				throw new BizException("it's the first one, can't be moved up");
			}
			SortedList sl = GetAttribute1List(oParam.C3SysNo);
			if ( sl == null )
			{
				throw new BizException("no attributes");
			}

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				CategoryDac o = new CategoryDac();

				foreach(CategoryAttribute1Info item  in sl.Keys)
				{
					if ( item.OrderNum == oParam.OrderNum-1 )
					{
						item.OrderNum += 1;
						o.SetOrderNum(item);
					}
				}
				oParam.OrderNum -= 1;
				o.SetOrderNum(oParam);

				scope.Complete();
            }
		}
		public void MoveDown(CategoryAttribute1Info oParam)
		{
			SortedList sl = GetAttribute1List(oParam.C3SysNo);
			if ( sl == null )
			{
				throw new BizException("no attributes");
			}

			if ( oParam.OrderNum == sl.Count )
			{
				throw new BizException("it's the last one, can't be moved down");
			}

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				CategoryDac o = new CategoryDac();

				foreach(CategoryAttribute1Info item  in sl.Keys)
				{
					if ( item.OrderNum == oParam.OrderNum+1 )
					{
						item.OrderNum -= 1;
						o.SetOrderNum(item);
					}
				}
				oParam.OrderNum += 1;
				o.SetOrderNum(oParam);

				scope.Complete();
            }
		}
		public void MoveBottom(CategoryAttribute1Info oParam)
		{			
			SortedList sl = GetAttribute1List(oParam.C3SysNo);
			if ( sl == null )
			{
				throw new BizException("no attributes");
			}

			if ( oParam.OrderNum == sl.Count )
			{
				throw new BizException("it's the last one, can't be moved down");
			}

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				CategoryDac o = new CategoryDac();

				foreach(CategoryAttribute1Info item  in sl.Keys)
				{
					if ( item.OrderNum > oParam.OrderNum )
					{
						item.OrderNum = item.OrderNum-1;
						o.SetOrderNum(item);
					}
				}
				oParam.OrderNum = sl.Count;
				o.SetOrderNum(oParam);

				scope.Complete();
            }
		}
		//---------------------------Category_Attribute1-------------------------

		//---------------------------Category_Attribute2-------------------------
		public bool IsExistAttribute2(CategoryAttribute2Info oParam)
		{
			return new CategoryDac().IsExistAttribute2(oParam);
		}

		public int GetCatetoryAttribute2NewOrderNum(CategoryAttribute2Info oParam)
		{
			return new CategoryDac().GetCatetoryAttribute2NewOrderNum(oParam);
		}

		public CategoryAttribute2Info LoadCategoryAttribute2(int paramAttribute2SysNo)
		{
			string sql = "select * from category_attribute2 where sysno=" + paramAttribute2SysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( !Util.HasMoreRow(ds))
				return null;
			CategoryAttribute2Info item = new CategoryAttribute2Info();
			Map( item, ds.Tables[0].Rows[0]);
			return item;
		}
		public bool IsAttribute2(int paramA1SysNo)
		{
			string sql = "select top 1 sysno from category_attribute2 where a1sysno = " + paramA1SysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds))
				return true;
			else
				return false;
		}
		public SortedList GetAttribute2List(int paramA1SysNo)
		{
			string sql = " select * from category_attribute2 where a1sysno = " + paramA1SysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( !Util.HasMoreRow(ds))
				return null;
			SortedList sl = new SortedList(ds.Tables[0].Rows.Count);
			foreach( DataRow dr in ds.Tables[0].Rows )
			{
				CategoryAttribute2Info item = new CategoryAttribute2Info();
				Map(item, dr);
				sl.Add(item, null);
			}
			return sl;
		}

        public DataSet GetAttribute2DsByC3SysNo(int paramC3SysNo, bool IsOptionType)
        {
            string sql = @"select category_attribute2.sysno as attribute2sysno,category_attribute2.attribute2name ,category_attribute2.Attribute2Type
                           from category_attribute1 inner join category_attribute2 on category_attribute1.sysno = category_attribute2.a1sysno where category_attribute1.status=0 
                           and category_attribute2.status=0 and c3sysno=@c3sysno";
            if(IsOptionType)
            {
                sql += " and category_attribute2.attribute2type>0";
            }
            sql += " order by category_attribute2.attribute2type,category_attribute2.ordernum";
            sql = sql.Replace("@c3sysno",paramC3SysNo.ToString());

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                return ds;
            }
            else
            {
                return null;
            }
        }

        //按Sysno排序
        public DataSet GetAttribute2DsByC3SysNo_OrderBySysNo(int paramC3SysNo, bool IsOptionType)
        {
            string sql = @"select category_attribute2.sysno as attribute2sysno,category_attribute2.attribute2name ,category_attribute2.Attribute2Type
                           from category_attribute1 inner join category_attribute2 on category_attribute1.sysno = category_attribute2.a1sysno where category_attribute1.status=0 
                           and category_attribute2.status=0 and c3sysno=@c3sysno";
            if (IsOptionType)
            {
                sql += " and category_attribute2.attribute2type>0";
            }
            sql += " order by category_attribute2.sysno,category_attribute2.ordernum";
            sql = sql.Replace("@c3sysno", paramC3SysNo.ToString());

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                return ds;
            }
            else
            {
                return null;
            }
        }

		private void Map(CategoryAttribute2Info oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.A1SysNo = Util.TrimIntNull(tempdr["A1SysNo"]);
			oParam.Attribute2ID = Util.TrimNull(tempdr["Attribute2ID"]);
			oParam.Attribute2Name = Util.TrimNull(tempdr["Attribute2Name"]);
			oParam.OrderNum = Util.TrimIntNull(tempdr["OrderNum"]);
			oParam.Status = Util.TrimIntNull(tempdr["Status"]);
			oParam.Attribute2Type  = Util.TrimIntNull(tempdr["Attribute2Type"]);
		}

		public int InsertAttribute2(CategoryAttribute2Info oParam)
		{
            string _atrribute2ID = "";
            string _tmpStr = "";
            int _tmpInt = -1;
            string sql = "select max(Attribute2ID) from Category_Attribute2 where A1SysNo=" + oParam.A1SysNo.ToString();
            _tmpStr = SqlHelper.ExecuteScalar(sql).ToString();
            if (_tmpStr.Trim() == "")
                _tmpStr = "0";
            if (!Util.IsInteger(_tmpStr))
                throw new BizException("对不起，生成编号错误，不能添加");
            _tmpInt = Convert.ToInt32(_tmpStr);
            _tmpInt++;

            _atrribute2ID = _tmpInt.ToString();

            oParam.Attribute2ID = _atrribute2ID;

			return new CategoryDac().InsertAttribute2(oParam);
		}

		public int UpdateAttribute2(CategoryAttribute2Info oParam)
		{
			return new CategoryDac().UpdateAttribute2(oParam);
		}

        //删除商品属性二级分类
        public int DeleteAttribute2(int attribute2SysNo)
        {
            int _return = 0;
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                _return=new CategoryDac().DeleteAttribute2(attribute2SysNo);
                scope.Complete();
            }
            return _return;
        }

        //设置属性为搜索索引
        public void SetAttributeTypeOptionFirst(string AttributeSysNos,string  AttributeSysNosElse)
        {
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                if (AttributeSysNosElse!="") new CategoryDac().SetOptionSecond(AttributeSysNosElse);
                if (AttributeSysNos!="") new CategoryDac().SetOptionFirst(AttributeSysNos);

                scope.Complete();
            }
        }

		public void MoveTop(CategoryAttribute2Info oParam)
		{
			if ( oParam.OrderNum == 1 )
			{
				throw new BizException("it's the top one already");
			}
			SortedList sl = GetAttribute2List(oParam.A1SysNo);
			if ( sl == null )
			{
				throw new BizException("no attribute for this third category");
			}

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				CategoryDac o = new CategoryDac();

				foreach(CategoryAttribute2Info item  in sl.Keys)
				{
					if ( item.OrderNum < oParam.OrderNum )
					{
						item.OrderNum = item.OrderNum+1;
						o.SetOrderNum(item);
					}
				}
				oParam.OrderNum = 1;
				o.SetOrderNum(oParam);

				scope.Complete();
            }
		}
		public void MoveUp(CategoryAttribute2Info oParam)
		{
			if ( oParam.OrderNum == 1 )
			{
				throw new BizException("it's the first one, can't be moved up");
			}
			SortedList sl = GetAttribute2List(oParam.A1SysNo);
			if ( sl == null )
			{
				throw new BizException("no attributes");
			}

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				CategoryDac o = new CategoryDac();

				foreach(CategoryAttribute2Info item  in sl.Keys)
				{
					if ( item.OrderNum == oParam.OrderNum-1 )
					{
						item.OrderNum += 1;
						o.SetOrderNum(item);
					}
				}
				oParam.OrderNum -= 1;
				o.SetOrderNum(oParam);

				scope.Complete();
            }
		}
		public void MoveDown(CategoryAttribute2Info oParam)
		{			
			SortedList sl = GetAttribute2List(oParam.A1SysNo);
			if ( sl == null )
			{
				throw new BizException("no attributes");
			}

			if ( oParam.OrderNum == sl.Count )
			{
				throw new BizException("it's the last one, can't be moved down");
			}

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				CategoryDac o = new CategoryDac();

				foreach(CategoryAttribute2Info item  in sl.Keys)
				{
					if ( item.OrderNum == oParam.OrderNum+1 )
					{
						item.OrderNum -= 1;
						o.SetOrderNum(item);
					}
				}
				oParam.OrderNum += 1;
				o.SetOrderNum(oParam);

				scope.Complete();
            }
		}
		public void MoveBottom(CategoryAttribute2Info oParam)
		{			
			SortedList sl = GetAttribute2List(oParam.A1SysNo);
			if ( sl == null )
			{
				throw new BizException("no attributes");
			}

			if ( oParam.OrderNum == sl.Count )
			{
				throw new BizException("it's the last one, can't be moved down");
			}

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				CategoryDac o = new CategoryDac();

				foreach(CategoryAttribute2Info item  in sl.Keys)
				{
					if ( item.OrderNum > oParam.OrderNum )
					{
						item.OrderNum = item.OrderNum-1;
						o.SetOrderNum(item);
					}
				}
				oParam.OrderNum = sl.Count;
				o.SetOrderNum(oParam);

				scope.Complete();
            }
		}
		//---------------------------Category_Attribute2-------------------------
	}
}
