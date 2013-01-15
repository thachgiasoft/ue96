using System;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Basic;

namespace Icson.DBAccess.Basic
{
	/// <summary>
	/// Summary description for CategoryDac.
	/// </summary>
	public class CategoryDac
	{
		
		public CategoryDac()
		{
		}

		public int Insert(Category1Info oParam)
		{

			string sql = @"INSERT INTO Category1
                            (
                            SysNo, C1ID, C1Name, Status
                            )
                            VALUES (
                            @SysNo, @C1ID, @C1Name, @Status
                            )";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramC1ID = new SqlParameter("@C1ID", SqlDbType.NVarChar,20);
			SqlParameter paramC1Name = new SqlParameter("@C1Name", SqlDbType.NVarChar,200);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);

			paramSysNo.Value = oParam.SysNo;
            paramC1ID.Value = oParam.C1ID;
			paramC1Name.Value = oParam.C1Name;
			paramStatus.Value = oParam.Status;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramC1ID);
			cmd.Parameters.Add(paramC1Name);
			cmd.Parameters.Add(paramStatus);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

        public int DeleteCategory1(int category1SysNo)
        {
            //删除评论项
            string sql = "delete from Category3_ReviewItem where C3SysNo in (select SysNo from Category3 where C2SysNo in (select SysNo from Category2 where C1SysNo="+category1SysNo.ToString()+"))";
            SqlHelper.ExecuteNonQuery(sql);
            //删除二级分类选项
            sql = "delete from Category_Attribute2_Option where Attribute2SysNo in (select SysNo from Category_Attribute2 where A1SysNo in (select SysNo from Category_Attribute1 where C3SysNo in (select SysNo from Category3 where C2SysNo in (select SysNo from Category2 where C1SysNo="+category1SysNo.ToString()+"))))";
            SqlHelper.ExecuteNonQuery(sql);
            //删除二级分类
            sql = "delete from Category_Attribute2 where A1SysNo in (select SysNo from Category_Attribute1 where C3SysNo in (select SysNo from Category3 where C2SysNo in (select SysNo from Category2 where C1SysNo="+category1SysNo.ToString()+")))";
            SqlHelper.ExecuteNonQuery(sql);
            //删除一级分类
            sql = "delete from Category_Attribute1 where C3SysNo in (select SysNo from Category3 where C2SysNo in (select SysNo from Category2 where C1SysNo="+category1SysNo.ToString()+"))";
            SqlHelper.ExecuteNonQuery(sql);
            //删除小类
            sql = "delete from Category3 where C2SysNo in (select SysNo from Category2 where C1SysNo="+category1SysNo.ToString()+")";
            SqlHelper.ExecuteNonQuery(sql);
            //删除中类
            sql = "delete from Category2 where C1SysNo="+category1SysNo.ToString();
            SqlHelper.ExecuteNonQuery(sql);
            //删除大类
            sql = "delete from Category1 where SysNo=" + category1SysNo.ToString();
            return SqlHelper.ExecuteNonQuery(sql);
        }

		public int Insert(Category2Info oParam)
		{
			string sql = @"INSERT INTO Category2
                            (
                            SysNo, C1SysNo, C2ID, C2Name, 
                            Status
                            )
                            VALUES (
                            @SysNo, @C1SysNo, @C2ID, @C2Name, 
                            @Status
                            )";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramC1SysNo = new SqlParameter("@C1SysNo", SqlDbType.Int,4);
			SqlParameter paramC2ID = new SqlParameter("@C2ID", SqlDbType.NVarChar,20);
			SqlParameter paramC2Name = new SqlParameter("@C2Name", SqlDbType.NVarChar,200);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);

			paramSysNo.Value = oParam.SysNo;
			paramC1SysNo.Value = oParam.C1SysNo;
			paramC2ID.Value = oParam.C2ID;
			paramC2Name.Value = oParam.C2Name;
			paramStatus.Value = oParam.Status;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramC1SysNo);
			cmd.Parameters.Add(paramC2ID);
			cmd.Parameters.Add(paramC2Name);
			cmd.Parameters.Add(paramStatus);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

        public int DeleteCategory2(int category2SysNo)
        {
            //删除中类包含的评论项
            string sql = "delete from Category3_ReviewItem where C3SysNo in (select SysNo from Category3 where C2SysNo=" + category2SysNo.ToString() + ")";
            SqlHelper.ExecuteNonQuery(sql);

            //删除中类包含的选项
            sql = "delete from Category_Attribute2_Option where Attribute2SysNo in (select SysNo from Category_Attribute2 where A1SysNo in (select SysNo from Category_Attribute1 where C3SysNo in (select SysNo from Category3 where C2SysNo="+category2SysNo.ToString()+")))";
            SqlHelper.ExecuteNonQuery(sql);

            //删除二级分类
            sql = "delete from Category_Attribute2 where A1SysNo in (select SysNo from Category_Attribute1 where C3SysNo in (select SysNo from Category3 where C2SysNo="+category2SysNo.ToString()+"))";
            SqlHelper.ExecuteNonQuery(sql);

            //删除一级分类
            sql = "delete from Category_Attribute1 where C3SysNo in (select SysNo from Category3 where C2SysNo="+category2SysNo.ToString()+")";
            SqlHelper.ExecuteNonQuery(sql);

            //删除小类
            sql = "delete from Category3 where C2SysNo="+category2SysNo.ToString();
            SqlHelper.ExecuteNonQuery(sql);

            //删除中类
            sql = "delete from Category2 where SysNo=" + category2SysNo.ToString();
            SqlHelper.ExecuteNonQuery(sql);

            return SqlHelper.ExecuteNonQuery(sql);
        }

		public int Insert(Category3Info oParam)
		{
			string sql = @"INSERT INTO Category3
                            (
                            SysNo, C2SysNo, C3ID, C3Name, 
                            Status,Online,
                            C3InventoryCycleTime,C3DMSWeight
                            )
                            VALUES (
                            @SysNo, @C2SysNo, @C3ID, @C3Name, 
                            @Status,@Online,
                            @C3InventoryCycleTime,@C3DMSWeight
                            )";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramC2SysNo = new SqlParameter("@C2SysNo", SqlDbType.Int,4);
			SqlParameter paramC3ID = new SqlParameter("@C3ID", SqlDbType.NVarChar,20);
			SqlParameter paramC3Name = new SqlParameter("@C3Name", SqlDbType.NVarChar,200);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);
			SqlParameter paramOnline = new SqlParameter("@Online", SqlDbType.Int,4);
            SqlParameter paramC3InventoryCycleTime = new SqlParameter("@C3InventoryCycleTime", SqlDbType.Int, 4);
            SqlParameter paramC3DMSWeight = new SqlParameter("@C3DMSWeight", SqlDbType.Decimal, 2);

			paramSysNo.Value = oParam.SysNo;
			paramC2SysNo.Value = oParam.C2SysNo;
			paramC3ID.Value = oParam.C3ID;
			paramC3Name.Value = oParam.C3Name;
			paramStatus.Value = oParam.Status;
			paramOnline.Value = oParam.Online;
            paramC3InventoryCycleTime.Value = oParam.C3InventoryCycleTime;
            paramC3DMSWeight.Value = oParam.C3DMSWeight;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramC2SysNo);
			cmd.Parameters.Add(paramC3ID);
			cmd.Parameters.Add(paramC3Name);
			cmd.Parameters.Add(paramStatus);
			cmd.Parameters.Add(paramOnline);
            cmd.Parameters.Add(paramC3InventoryCycleTime);
            cmd.Parameters.Add(paramC3DMSWeight);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

        //删除小类
        public int DeleteCategory3(int category3SysNo)
        {
            //删除小类包含的评论项
            string sql = "delete from Category3_ReviewItem where C3SysNo=" + category3SysNo.ToString();
            SqlHelper.ExecuteNonQuery(sql);
            //删除包含的选项
            sql = "delete from Category_Attribute2_Option where Attribute2SysNo in (select SysNo from Category_Attribute2 where A1SysNo in (select SysNo from Category_Attribute1 where C3SysNo=" + category3SysNo.ToString() + "))";
            SqlHelper.ExecuteNonQuery(sql);
            //删除包含的二级分类
            sql = "delete from Category_Attribute2 where A1SysNo in (select SysNo from Category_Attribute1 where C3SysNo=" + category3SysNo.ToString() + ")";
            SqlHelper.ExecuteNonQuery(sql);
            //删除包含的一级分类
            sql = "delete from Category_Attribute1 where C3SysNo="+category3SysNo.ToString();
            SqlHelper.ExecuteNonQuery(sql);
            //删除小类
            sql = "delete from Category3 where SysNo=" + category3SysNo.ToString();
            return SqlHelper.ExecuteNonQuery(sql);
        }

		public int Update(Category1Info oParam)
		{
			string sql = @"UPDATE Category1 SET 
                            C1ID=@C1ID, 
                            C1Name=@C1Name, Status=@Status
                            WHERE SysNo=@SysNo";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramC1ID = new SqlParameter("@C1ID", SqlDbType.NVarChar,20);
			SqlParameter paramC1Name = new SqlParameter("@C1Name", SqlDbType.NVarChar,200);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);

			paramSysNo.Value = oParam.SysNo;
			paramC1ID.Value = oParam.C1ID;
			paramC1Name.Value = oParam.C1Name;
			paramStatus.Value = oParam.Status;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramC1ID);
			cmd.Parameters.Add(paramC1Name);
			cmd.Parameters.Add(paramStatus);

			return SqlHelper.ExecuteNonQuery(cmd);
		}
		public int Update(Category2Info oParam)
		{
			string sql = @"UPDATE Category2 SET 
                            C1SysNo=@C1SysNo, 
                            C2ID=@C2ID, C2Name=@C2Name, 
                            Status=@Status
                            WHERE SysNo=@SysNo";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramC1SysNo = new SqlParameter("@C1SysNo", SqlDbType.Int,4);
			SqlParameter paramC2ID = new SqlParameter("@C2ID", SqlDbType.NVarChar,20);
			SqlParameter paramC2Name = new SqlParameter("@C2Name", SqlDbType.NVarChar,200);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);

			paramSysNo.Value = oParam.SysNo;
			paramC1SysNo.Value = oParam.C1SysNo;
			paramC2ID.Value = oParam.C2ID;
			paramC2Name.Value = oParam.C2Name;
			paramStatus.Value = oParam.Status;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramC1SysNo);
			cmd.Parameters.Add(paramC2ID);
			cmd.Parameters.Add(paramC2Name);
			cmd.Parameters.Add(paramStatus);

			return SqlHelper.ExecuteNonQuery(cmd);
		}
		public int Update(Category3Info oParam)
		{
			string sql = @"UPDATE Category3 SET 
                            C2SysNo=@C2SysNo, 
                            C3ID=@C3ID, C3Name=@C3Name, 
                            Status=@Status,Online=@Online,
                            C3InventoryCycleTime=@C3InventoryCycleTime,C3DMSWeight=@C3DMSWeight 
                            WHERE SysNo=@SysNo";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramC2SysNo = new SqlParameter("@C2SysNo", SqlDbType.Int,4);
			SqlParameter paramC3ID = new SqlParameter("@C3ID", SqlDbType.NVarChar,20);
			SqlParameter paramC3Name = new SqlParameter("@C3Name", SqlDbType.NVarChar,200);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);
			SqlParameter paramOnline = new SqlParameter("@Online", SqlDbType.Int,4);
            SqlParameter paramC3InventoryCycleTime = new SqlParameter("@C3InventoryCycleTime", SqlDbType.Int, 4);
            SqlParameter paramC3DMSWeight = new SqlParameter("@C3DMSWeight", SqlDbType.Decimal, 2);

			paramSysNo.Value = oParam.SysNo;
			paramC2SysNo.Value = oParam.C2SysNo;
			paramC3ID.Value = oParam.C3ID;
			paramC3Name.Value = oParam.C3Name;
			paramStatus.Value = oParam.Status;
			paramOnline.Value = oParam.Online;
            paramC3InventoryCycleTime.Value = oParam.C3InventoryCycleTime;
            paramC3DMSWeight.Value = oParam.C3DMSWeight;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramC2SysNo);
			cmd.Parameters.Add(paramC3ID);
			cmd.Parameters.Add(paramC3Name);
			cmd.Parameters.Add(paramStatus);
			cmd.Parameters.Add(paramOnline);
            cmd.Parameters.Add(paramC3InventoryCycleTime);
            cmd.Parameters.Add(paramC3DMSWeight);

			return SqlHelper.ExecuteNonQuery(cmd);
		}
		public int InsertAttribute(CategoryAttributeInfo oParam)
		{
			string sql = @"INSERT INTO Category_Attribute
                            (
                            C3SysNo, AttributeID, AttributeName, 
                            OrderNum, Status, AttributeType
                            )
                            VALUES (
                            @C3SysNo, @AttributeID, @AttributeName, 
                            @OrderNum, @Status, @AttributeType
                            )";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramC3SysNo = new SqlParameter("@C3SysNo", SqlDbType.Int,4);
			SqlParameter paramAttributeID = new SqlParameter("@AttributeID", SqlDbType.NVarChar,20);
			SqlParameter paramAttributeName = new SqlParameter("@AttributeName", SqlDbType.NVarChar,50);
			SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int,4);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);
			SqlParameter paramAttributeType = new SqlParameter("@AttributeType",SqlDbType.Int,4);

			paramC3SysNo.Value = oParam.C3SysNo;
			paramAttributeID.Value = oParam.AttributeID;
			paramAttributeName.Value = oParam.AttributeName;
			paramOrderNum.Value = oParam.OrderNum;
			paramStatus.Value = oParam.Status;
			paramAttributeType.Value = oParam.AttributeType;

			cmd.Parameters.Add(paramC3SysNo);
			cmd.Parameters.Add(paramAttributeID);
			cmd.Parameters.Add(paramAttributeName);
			cmd.Parameters.Add(paramOrderNum);
			cmd.Parameters.Add(paramStatus);
			cmd.Parameters.Add(paramAttributeType);

			return SqlHelper.ExecuteNonQuery(cmd);
		}
		public int UpdateAttribute(CategoryAttributeInfo oParam)
		{
			string sql = @"UPDATE Category_Attribute SET 
                            C3SysNo=@C3SysNo, 
                            AttributeID=@AttributeID, AttributeName=@AttributeName, 
                            OrderNum=@OrderNum, Status=@Status,AttributeType=@AttributeType 
                            WHERE SysNo=@SysNo";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramC3SysNo = new SqlParameter("@C3SysNo", SqlDbType.Int,4);
			SqlParameter paramAttributeID = new SqlParameter("@AttributeID", SqlDbType.NVarChar,20);
			SqlParameter paramAttributeName = new SqlParameter("@AttributeName", SqlDbType.NVarChar,50);
			SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int,4);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);
			SqlParameter paramAttributeType = new SqlParameter("@AttributeType", SqlDbType.Int,4);

			paramSysNo.Value = oParam.SysNo;
			paramC3SysNo.Value = oParam.C3SysNo;
			paramAttributeID.Value = oParam.AttributeID;
			paramAttributeName.Value = oParam.AttributeName;
			paramOrderNum.Value = oParam.OrderNum;
			paramStatus.Value = oParam.Status;
			paramAttributeType.Value = oParam.AttributeType;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramC3SysNo);
			cmd.Parameters.Add(paramAttributeID);
			cmd.Parameters.Add(paramAttributeName);
			cmd.Parameters.Add(paramOrderNum);
			cmd.Parameters.Add(paramStatus);
			cmd.Parameters.Add(paramAttributeType);

			return SqlHelper.ExecuteNonQuery(cmd);
		}
		public int UpdateAttributeByC3andAID(CategoryAttributeInfo oParam)
		{
			string sql = @"UPDATE Category_Attribute SET 
                             AttributeName=@AttributeName, 
                            OrderNum=@OrderNum, Status=@Status
                            WHERE C3SysNo=@C3SysNo and AttributeID=@AttributeID ";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramC3SysNo = new SqlParameter("@C3SysNo", SqlDbType.Int,4);
			SqlParameter paramAttributeID = new SqlParameter("@AttributeID", SqlDbType.NVarChar,20);
			SqlParameter paramAttributeName = new SqlParameter("@AttributeName", SqlDbType.NVarChar,50);
			SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int,4);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);
			SqlParameter paramAttributeType = new SqlParameter("@AttributeType",SqlDbType.Int,4);

			paramC3SysNo.Value = oParam.C3SysNo;
			paramAttributeID.Value = oParam.AttributeID;
			paramAttributeName.Value = oParam.AttributeName;
			paramOrderNum.Value = oParam.OrderNum;
			paramStatus.Value = oParam.Status;
			paramAttributeType.Value = oParam.AttributeType;

			cmd.Parameters.Add(paramC3SysNo);
			cmd.Parameters.Add(paramAttributeID);
			cmd.Parameters.Add(paramAttributeName);
			cmd.Parameters.Add(paramOrderNum);
			cmd.Parameters.Add(paramStatus);
			cmd.Parameters.Add(paramAttributeType);

			return SqlHelper.ExecuteNonQuery(cmd);
		}
		public int SetOrderNum(CategoryAttributeInfo oParam)
		{
			string sql = "update category_attribute set ordernum = " + oParam.OrderNum + " where sysno = " + oParam.SysNo;
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int SetOrderNum(CategoryAttribute1Info oParam)
		{
			string sql = "update category_attribute1 set ordernum = " + oParam.OrderNum + " where sysno = " + oParam.SysNo;
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int SetOrderNum(CategoryAttribute2Info oParam)
		{
			string sql = "update category_attribute2 set ordernum = " + oParam.OrderNum + " where sysno = " + oParam.SysNo;
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}
		
		//old for attribute opiton
		public int GetCatetoryAttributeOptionNewOrderNum(CategoryAttributeOptionInfo oParam)
		{
			string sql = "select isnull(max(OrderNum),0)+1 as newOrderNum from Category_Attribute_Option where AttributeSysNo=" + oParam.AttributeSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if(Util.HasMoreRow(ds))
			{
				return Int32.Parse(ds.Tables[0].Rows[0][0].ToString());
			}
			else
			{
				return 1;
			}
		}

		public int GetCatetoryAttribute2OptionNewOrderNum(CategoryAttribute2OptionInfo oParam)
		{
			string sql = "select isnull(max(OrderNum),0)+1 as newOrderNum from Category_Attribute2_Option where Attribute2SysNo=" + oParam.Attribute2SysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if(Util.HasMoreRow(ds))
			{
				return Int32.Parse(ds.Tables[0].Rows[0][0].ToString());
			}
			else
			{
				return 1;
			}
		}

		public int GetCatetoryAttribute1NewOrderNum(CategoryAttribute1Info oParam)
		{
			string sql = "select isnull(max(OrderNum),0)+1 as newOrderNum from Category_Attribute1 where C3SysNo=" + oParam.C3SysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if(Util.HasMoreRow(ds))
			{
				return Int32.Parse(ds.Tables[0].Rows[0][0].ToString());
			}
			else
			{
				return 1;
			}
		}

		public int GetCatetoryAttribute2NewOrderNum(CategoryAttribute2Info oParam)
		{
			string sql = "select isnull(max(OrderNum),0)+1 as newOrderNum from Category_Attribute2 where A1SysNo=" + oParam.A1SysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if(Util.HasMoreRow(ds))
			{
				return Int32.Parse(ds.Tables[0].Rows[0][0].ToString());
			}
			else
			{
				return 1;
			}
		}
/*
		public int InsertAttributeOption(CategoryAttributeOptionInfo oParam)
		{
			string sql = @"INSERT INTO Category_Attribute_Option
                            (
                            AttributeSysNo, AttributeOptionName, 
                            OrderNum, IsRecommend, Status
                            )
                            VALUES (
                            @AttributeSysNo, @AttributeOptionName, 
                            @OrderNum, @IsRecommend, @Status
                            )";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramAttributeSysNo = new SqlParameter("@AttributeSysNo", SqlDbType.Int,4);
			SqlParameter paramAttributeOptionName = new SqlParameter("@AttributeOptionName", SqlDbType.NVarChar,500);
			SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int,4);
			SqlParameter paramIsRecommend = new SqlParameter("@IsRecommend", SqlDbType.Int,4);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);

			paramAttributeSysNo.Value = oParam.AttributeSysNo;
			paramAttributeOptionName.Value = oParam.AttributeOptionName;			
			paramOrderNum.Value = oParam.OrderNum;
			paramIsRecommend.Value = oParam.IsRecommend;
			paramStatus.Value = oParam.Status;

			cmd.Parameters.Add(paramAttributeSysNo);
			cmd.Parameters.Add(paramAttributeOptionName);
			cmd.Parameters.Add(paramOrderNum);
			cmd.Parameters.Add(paramIsRecommend);
			cmd.Parameters.Add(paramStatus);

			return SqlHelper.ExecuteNonQuery(cmd);
		}
		public int UpdateAttributeOption(CategoryAttributeOptionInfo oParam)
		{
			string sql = @"UPDATE Category_Attribute_Option SET 
                            AttributeSysNo=@AttributeSysNo, AttributeOptionName=@AttributeOptionName, 
                            OrderNum=@OrderNum, IsRecommend=@IsRecommend, Status=@Status
                            WHERE SysNo=@SysNo";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramAttributeSysNo = new SqlParameter("@AttributeSysNo", SqlDbType.Int,4);
			SqlParameter paramAttributeOptionName = new SqlParameter("@AttributeOptionName", SqlDbType.NVarChar,500);
			SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int,4);
			SqlParameter paramIsRecommend = new SqlParameter("@IsRecommend", SqlDbType.Int,4);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);

			paramSysNo.Value = oParam.SysNo;
			paramAttributeSysNo.Value = oParam.AttributeSysNo;
			paramAttributeOptionName.Value = oParam.AttributeOptionName;
			paramOrderNum.Value = oParam.OrderNum;
			paramIsRecommend.Value = oParam.IsRecommend;
			paramStatus.Value = oParam.Status;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramAttributeSysNo);
			cmd.Parameters.Add(paramAttributeOptionName);
			cmd.Parameters.Add(paramOrderNum);
			cmd.Parameters.Add(paramIsRecommend);
			cmd.Parameters.Add(paramStatus);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public bool IsExistAttributeOption(CategoryAttributeOptionInfo oParam)
		{
			string sql = "select top 1 sysno from category_attribute_option where AttributeSysNo=@AttributeSysNo and AttributeOptionName=@AttributeOptionName";
			SqlCommand cmd = new SqlCommand(sql);
			SqlParameter paramAttributeSysNo = new SqlParameter("@AttributeSysNo", SqlDbType.Int,4);
			SqlParameter paramAttributeOptionName = new SqlParameter("@AttributeOptionName", SqlDbType.NVarChar,500);

			paramAttributeSysNo.Value = oParam.AttributeSysNo;
			paramAttributeOptionName.Value = oParam.AttributeOptionName;

			cmd.Parameters.Add(paramAttributeSysNo);
			cmd.Parameters.Add(paramAttributeOptionName);

			DataSet ds = SqlHelper.ExecuteDataSet(cmd);;
			if ( Util.HasMoreRow(ds))
				return true;
			else
				return false;
		}

		public int SetOrderNum(CategoryAttributeOptionInfo oParam)
		{
			string sql = "update category_attribute_option set ordernum = " + oParam.OrderNum + " where sysno = " + oParam.SysNo;
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}
		*/
		public int InsertAttribute2Option(CategoryAttribute2OptionInfo oParam)
		{
			string sql = @"INSERT INTO Category_Attribute2_Option
                            (
                            Attribute2SysNo, Attribute2OptionName, 
                            OrderNum, IsRecommend, Status
                            )
                            VALUES (
                            @Attribute2SysNo, @Attribute2OptionName, 
                            @OrderNum, @IsRecommend, @Status
                            )";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramAttribute2SysNo = new SqlParameter("@Attribute2SysNo", SqlDbType.Int,4);
			SqlParameter paramAttribute2OptionName = new SqlParameter("@Attribute2OptionName", SqlDbType.NVarChar,500);
			SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int,4);
			SqlParameter paramIsRecommend = new SqlParameter("@IsRecommend", SqlDbType.Int,4);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);

			paramAttribute2SysNo.Value = oParam.Attribute2SysNo;
			paramAttribute2OptionName.Value = oParam.Attribute2OptionName;			
			paramOrderNum.Value = oParam.OrderNum;
			paramIsRecommend.Value = oParam.IsRecommend;
			paramStatus.Value = oParam.Status;

			cmd.Parameters.Add(paramAttribute2SysNo);
			cmd.Parameters.Add(paramAttribute2OptionName);
			cmd.Parameters.Add(paramOrderNum);
			cmd.Parameters.Add(paramIsRecommend);
			cmd.Parameters.Add(paramStatus);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

        public int DeleteAttribute2Option(int optionSysNo)
        {
            string sql = "delete from Category_Attribute2_Option where SysNo=" + optionSysNo.ToString();
            return SqlHelper.ExecuteNonQuery(sql);
        }

        public int GetMaxSysNo(int Attribute1SysNo)
        {
            string sql = "select max(SysNo) as max from Category_Attribute2 where A1SysNo=" + Attribute1SysNo.ToString();
            DataSet dt;
            dt = SqlHelper.ExecuteDataSet(sql);
            if (dt.Tables[0].Rows.Count > 0)
                return Convert.ToInt32(dt.Tables[0].Rows[0]["max"].ToString());
            else
                return -1;
        }

		public int UpdateAttribute2Option(CategoryAttribute2OptionInfo oParam)
		{
			string sql = @"UPDATE Category_Attribute2_Option SET 
                            Attribute2SysNo=@Attribute2SysNo, Attribute2OptionName=@Attribute2OptionName, 
                            OrderNum=@OrderNum, IsRecommend=@IsRecommend, Status=@Status
                            WHERE SysNo=@SysNo";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramAttribute2SysNo = new SqlParameter("@Attribute2SysNo", SqlDbType.Int,4);
			SqlParameter paramAttribute2OptionName = new SqlParameter("@Attribute2OptionName", SqlDbType.NVarChar,500);
			SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int,4);
			SqlParameter paramIsRecommend = new SqlParameter("@IsRecommend", SqlDbType.Int,4);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);

			paramSysNo.Value = oParam.SysNo;
			paramAttribute2SysNo.Value = oParam.Attribute2SysNo;
			paramAttribute2OptionName.Value = oParam.Attribute2OptionName;
			paramOrderNum.Value = oParam.OrderNum;
			paramIsRecommend.Value = oParam.IsRecommend;
			paramStatus.Value = oParam.Status;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramAttribute2SysNo);
			cmd.Parameters.Add(paramAttribute2OptionName);
			cmd.Parameters.Add(paramOrderNum);
			cmd.Parameters.Add(paramIsRecommend);
			cmd.Parameters.Add(paramStatus);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public bool IsExistAttribute2Option(CategoryAttribute2OptionInfo oParam)
		{
			string sql = "select top 1 sysno from category_attribute2_option where Attribute2SysNo=@Attribute2SysNo and Attribute2OptionName=@Attribute2OptionName and SysNo!=@SysNo";
			SqlCommand cmd = new SqlCommand(sql);
			SqlParameter paramAttribute2SysNo = new SqlParameter("@Attribute2SysNo", SqlDbType.Int,4);
            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
			SqlParameter paramAttribute2OptionName = new SqlParameter("@Attribute2OptionName", SqlDbType.NVarChar,500);

			paramAttribute2SysNo.Value = oParam.Attribute2SysNo;
			paramAttribute2OptionName.Value = oParam.Attribute2OptionName;
            paramSysNo.Value = oParam.SysNo;

			cmd.Parameters.Add(paramAttribute2SysNo);
			cmd.Parameters.Add(paramAttribute2OptionName);
            cmd.Parameters.Add(paramSysNo);

			DataSet ds = SqlHelper.ExecuteDataSet(cmd);
			if ( Util.HasMoreRow(ds))
				return true;
			else
				return false;
		}

		public int SetOrderNum(CategoryAttribute2OptionInfo oParam)
		{
			string sql = "update category_attribute2_option set ordernum = " + oParam.OrderNum + " where sysno = " + oParam.SysNo;
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public bool IsExistAttribute1(CategoryAttribute1Info oParam)
		{
            string sql = "select top 1 sysno from category_attribute1 where c3sysno=@c3sysno and Attribute1Name=@Attribute1Name and SysNo!=@SysNo";
			SqlCommand cmd = new SqlCommand(sql);
			
			SqlParameter paramC3SysNo = new SqlParameter("@C3SysNo", SqlDbType.Int,4);
			SqlParameter paramAttribute1Name = new SqlParameter("@Attribute1Name", SqlDbType.NVarChar,50);
            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
			paramC3SysNo.Value = oParam.C3SysNo;
			paramAttribute1Name.Value = oParam.Attribute1Name;
            paramSysNo.Value = oParam.SysNo;

			cmd.Parameters.Add(paramC3SysNo);
			cmd.Parameters.Add(paramAttribute1Name);
            cmd.Parameters.Add(paramSysNo);

			DataSet ds = SqlHelper.ExecuteDataSet(cmd);;
			if ( Util.HasMoreRow(ds))
				return true;
			else
				return false;
		}

		public int InsertAttribute1(CategoryAttribute1Info oParam)
		{
			string sql = @"INSERT INTO Category_Attribute1
                            (
                            C3SysNo, Attribute1ID, Attribute1Name, 
                            OrderNum, Status, Attribute1Type
                            )
                            VALUES (
                            @C3SysNo, @Attribute1ID, @Attribute1Name, 
                            @OrderNum, @Status, @Attribute1Type
                            )";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramC3SysNo = new SqlParameter("@C3SysNo", SqlDbType.Int,4);
			SqlParameter paramAttribute1ID = new SqlParameter("@Attribute1ID", SqlDbType.NVarChar,20);
			SqlParameter paramAttribute1Name = new SqlParameter("@Attribute1Name", SqlDbType.NVarChar,50);
			SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int,4);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);
			SqlParameter paramAttribute1Type = new SqlParameter("@Attribute1Type",SqlDbType.Int,4);

			paramC3SysNo.Value = oParam.C3SysNo;
			paramAttribute1ID.Value = oParam.Attribute1ID;
			paramAttribute1Name.Value = oParam.Attribute1Name;
			paramOrderNum.Value = oParam.OrderNum;
			paramStatus.Value = oParam.Status;
			paramAttribute1Type.Value = oParam.Attribute1Type;

			cmd.Parameters.Add(paramC3SysNo);
			cmd.Parameters.Add(paramAttribute1ID);
			cmd.Parameters.Add(paramAttribute1Name);
			cmd.Parameters.Add(paramOrderNum);
			cmd.Parameters.Add(paramStatus);
			cmd.Parameters.Add(paramAttribute1Type);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int UpdateAttribute1(CategoryAttribute1Info oParam)
		{
			string sql = @"UPDATE Category_Attribute1 SET 
                            C3SysNo=@C3SysNo, 
                            Attribute1ID=@Attribute1ID, Attribute1Name=@Attribute1Name, 
                            OrderNum=@OrderNum, Status=@Status,Attribute1Type=@Attribute1Type 
                            WHERE SysNo=@SysNo";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramC3SysNo = new SqlParameter("@C3SysNo", SqlDbType.Int,4);
			SqlParameter paramAttribute1ID = new SqlParameter("@Attribute1ID", SqlDbType.NVarChar,20);
			SqlParameter paramAttribute1Name = new SqlParameter("@Attribute1Name", SqlDbType.NVarChar,50);
			SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int,4);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);
			SqlParameter paramAttribute1Type = new SqlParameter("@Attribute1Type", SqlDbType.Int,4);

			paramSysNo.Value = oParam.SysNo;
			paramC3SysNo.Value = oParam.C3SysNo;
			paramAttribute1ID.Value = oParam.Attribute1ID;
			paramAttribute1Name.Value = oParam.Attribute1Name;
			paramOrderNum.Value = oParam.OrderNum;
			paramStatus.Value = oParam.Status;
			paramAttribute1Type.Value = oParam.Attribute1Type;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramC3SysNo);
			cmd.Parameters.Add(paramAttribute1ID);
			cmd.Parameters.Add(paramAttribute1Name);
			cmd.Parameters.Add(paramOrderNum);
			cmd.Parameters.Add(paramStatus);
			cmd.Parameters.Add(paramAttribute1Type);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

        public int DeleteAttribute1(int attribute1SysNo)
        {
            string sql = "delete from Category_Attribute2_Option where Attribute2SysNo in (select SysNo from Category_Attribute2 where A1SysNo=" + attribute1SysNo.ToString() + ")";
            SqlHelper.ExecuteNonQuery(sql);

            sql = "delete from Category_Attribute2 where A1SysNo=" + attribute1SysNo.ToString();
            SqlHelper.ExecuteNonQuery(sql);

            sql = "delete from Category_Attribute1 where SysNo=" + attribute1SysNo.ToString();
            return SqlHelper.ExecuteNonQuery(sql);
        }

		public bool IsExistAttribute2(CategoryAttribute2Info oParam)
		{
            string sql = "select top 1 sysno from category_attribute2 where a1sysno=@a1sysno and Attribute2Name=@Attribute2Name and SysNo!=@SysNo";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramA1SysNo = new SqlParameter("@A1SysNo", SqlDbType.Int,4);
            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
			SqlParameter paramAttribute2Name = new SqlParameter("@Attribute2Name", SqlDbType.NVarChar,50);

			paramA1SysNo.Value = oParam.A1SysNo;
			paramAttribute2Name.Value = oParam.Attribute2Name;
            paramSysNo.Value = oParam.SysNo;

			cmd.Parameters.Add(paramA1SysNo);
			cmd.Parameters.Add(paramAttribute2Name);
            cmd.Parameters.Add(paramSysNo);

			DataSet ds = SqlHelper.ExecuteDataSet(cmd);;
			if ( Util.HasMoreRow(ds))
				return true;
			else
				return false;
		}

		public int InsertAttribute2(CategoryAttribute2Info oParam)
		{
			string sql = @"INSERT INTO Category_Attribute2
                            (
                            A1SysNo, Attribute2ID, Attribute2Name, 
                            OrderNum, Status, Attribute2Type
                            )
                            VALUES (
                            @A1SysNo, @Attribute2ID, @Attribute2Name, 
                            @OrderNum, @Status, @Attribute2Type
                            )";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramA1SysNo = new SqlParameter("@A1SysNo", SqlDbType.Int,4);
			SqlParameter paramAttribute2ID = new SqlParameter("@Attribute2ID", SqlDbType.NVarChar,20);
			SqlParameter paramAttribute2Name = new SqlParameter("@Attribute2Name", SqlDbType.NVarChar,50);
			SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int,4);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);
			SqlParameter paramAttribute2Type = new SqlParameter("@Attribute2Type",SqlDbType.Int,4);

			paramA1SysNo.Value = oParam.A1SysNo;
			paramAttribute2ID.Value = oParam.Attribute2ID;
			paramAttribute2Name.Value = oParam.Attribute2Name;
			paramOrderNum.Value = oParam.OrderNum;
			paramStatus.Value = oParam.Status;
			paramAttribute2Type.Value = oParam.Attribute2Type;

			cmd.Parameters.Add(paramA1SysNo);
			cmd.Parameters.Add(paramAttribute2ID);
			cmd.Parameters.Add(paramAttribute2Name);
			cmd.Parameters.Add(paramOrderNum);
			cmd.Parameters.Add(paramStatus);
			cmd.Parameters.Add(paramAttribute2Type);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int UpdateAttribute2(CategoryAttribute2Info oParam)
		{
			string sql = @"UPDATE Category_Attribute2 SET 
                            A1SysNo=@A1SysNo, 
                            Attribute2ID=@Attribute2ID, Attribute2Name=@Attribute2Name, 
                            OrderNum=@OrderNum, Status=@Status,Attribute2Type=@Attribute2Type 
                            WHERE SysNo=@SysNo";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramA1SysNo = new SqlParameter("@A1SysNo", SqlDbType.Int,4);
			SqlParameter paramAttribute2ID = new SqlParameter("@Attribute2ID", SqlDbType.NVarChar,20);
			SqlParameter paramAttribute2Name = new SqlParameter("@Attribute2Name", SqlDbType.NVarChar,50);
			SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int,4);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);
			SqlParameter paramAttribute2Type = new SqlParameter("@Attribute2Type", SqlDbType.Int,4);

			paramSysNo.Value = oParam.SysNo;
			paramA1SysNo.Value = oParam.A1SysNo;
			paramAttribute2ID.Value = oParam.Attribute2ID;
			paramAttribute2Name.Value = oParam.Attribute2Name;
			paramOrderNum.Value = oParam.OrderNum;
			paramStatus.Value = oParam.Status;
			paramAttribute2Type.Value = oParam.Attribute2Type;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramA1SysNo);
			cmd.Parameters.Add(paramAttribute2ID);
			cmd.Parameters.Add(paramAttribute2Name);
			cmd.Parameters.Add(paramOrderNum);
			cmd.Parameters.Add(paramStatus);
			cmd.Parameters.Add(paramAttribute2Type);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

        //删除商品属性二级分类
        public int DeleteAttribute2(int attribute2SysNo)
        {
            string sql = "delete from Category_Attribute2_Option where Attribute2SysNo=" + attribute2SysNo.ToString();
            SqlHelper.ExecuteNonQuery(sql);

            sql = "delete from Category_Attribute2 where SysNo=" + attribute2SysNo.ToString();
            return SqlHelper.ExecuteNonQuery(sql);
        }

        //设置为搜索索引
        public int SetOptionFirst(string AttributeSysNos)
        {
            string sql = "update Category_Attribute2 set Attribute2Type=1 where SysNo in (" + AttributeSysNos + ")";
            return SqlHelper.ExecuteNonQuery(sql);
        }

        //设置为非搜索索引
        public int SetOptionSecond(string AttributeSysNos)
        {
            string sql = "update Category_Attribute2 set Attribute2Type=2 where SysNo in (" + AttributeSysNos + ")";
            return SqlHelper.ExecuteNonQuery(sql);
        }
	}
}
