using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

using Icson.Utils;
using Icson.Objects.Basic;
using Icson.BLL;
using Icson.BLL.Basic;
using Icson.Objects;

namespace YoeJoyWeb.Controls
{
	/// <summary>
	/// Summary description for Area.
	/// </summary>
	public class Area : Control, INamingContainer
	{
		private DropDownList ddlProvince = new DropDownList();
		private DropDownList ddlCity = new DropDownList();
		private DropDownList ddlDistrict = new DropDownList();
		/// <summary>
		/// ��ȡ��ǰ�����е�ʡ���
		/// </summary>
		public int ProvinceSysNo
		{
			get
			{
				return Convert.ToInt32(ddlProvince.SelectedItem.Value);
			}
		}
		/// <summary>
		/// ��ȡ��ǰ�����е��б��
		/// </summary>
		public int CitySysNo
		{
			get
			{
				return Convert.ToInt32(ddlCity.SelectedItem.Value);
			}
		}
		/// <summary>
		/// ��ȡ��ǰ�����е����ر��
		/// </summary>
		public int DistrictSysNo
		{
			get
			{
				return Convert.ToInt32(ddlDistrict.SelectedItem.Value);
			}
		}
		/// <summary>
		/// ��ȡ��ǰ������ײ�ı�ţ����ز�Ϊ�շ������أ�����Ϊ�տ�����������Ϊ�տ�ʡ����Ϊ�շ���ϵͳ�Ŀա�
		/// </summary>
		public int AreaSysNo
		{
			get
			{
				if ( this.DistrictSysNo != AppConst.IntNull )
					return this.DistrictSysNo;
				else if ( this.CitySysNo != AppConst.IntNull )
					return this.CitySysNo;
				else if ( this.ProvinceSysNo != AppConst.IntNull )
					return this.ProvinceSysNo;
				else
					return AppConst.IntNull;
			}
			set
			{
				AreaInfo oParam = ASPManager.GetInstance().GetAreaHash()[value] as AreaInfo;
				if ( oParam != null )
				{
					switch(ASPManager.GetInstance().GetAreaType(oParam)) 
					{
						case (int)AppEnum.AreaType.District:
							BindProvince();
							ddlProvince.SelectedIndex = ddlProvince.Items.IndexOf( ddlProvince.Items.FindByValue(oParam.ProvinceSysNo.ToString() ) );
							BindCity();
							ddlCity.SelectedIndex = ddlCity.Items.IndexOf(ddlCity.Items.FindByValue(oParam.CitySysNo.ToString()) );
							BindDistrict();
							ddlDistrict.SelectedIndex = ddlDistrict.Items.IndexOf(ddlDistrict.Items.FindByValue(oParam.SysNo.ToString()) );
							break;
						case (int)AppEnum.AreaType.City:
							BindProvince();
							ddlProvince.SelectedIndex = ddlProvince.Items.IndexOf( ddlProvince.Items.FindByValue(oParam.ProvinceSysNo.ToString() ) );
							BindCity();
							ddlCity.SelectedIndex = ddlCity.Items.IndexOf(ddlCity.Items.FindByValue(oParam.SysNo.ToString()) );
							BindDistrict();
							break;
						case (int)AppEnum.AreaType.Province:
							BindProvince();
							ddlProvince.SelectedIndex = ddlProvince.Items.IndexOf( ddlProvince.Items.FindByValue(oParam.SysNo.ToString() ) );
							BindCity();
							BindDistrict();
							break;
						default:
							break;
					}
				}
				else
					BindArea();
			}
		}

		/// <summary>
		/// ��ʼ���ؼ�
		/// </summary>
		public void BindArea()
		{
			BindProvince();
			BindCity();
			BindDistrict();
		}


		#region private�Ĺ��ߺ���
		private void BindProvince()
		{
			SortedList sl = ASPManager.GetInstance().GetProvinceList(true);
			if ( sl != null )
			{
				ddlProvince.Items.Clear();
				foreach(AreaInfo item in sl.Keys)
				{
					ddlProvince.Items.Add(
						new ListItem( item.ProvinceName, item.SysNo.ToString() ) );
				}
			}
			ddlProvince.Items.Insert(0, new ListItem( AppConst.AllSelectString, AppConst.IntNull.ToString() ));

		}
		private void BindCity()
		{
			ddlCity.Items.Clear();
			int provinceSysNo = Convert.ToInt32(ddlProvince.SelectedItem.Value);
			SortedList sl = ASPManager.GetInstance().GetCityList( provinceSysNo, true);
			if ( sl != null )
			{
				foreach(AreaInfo item in sl.Keys)
				{
					ddlCity.Items.Add(
						new ListItem( item.CityName, item.SysNo.ToString() ) );
				}
			}
			ddlCity.Items.Insert(0, new ListItem( AppConst.AllSelectString, AppConst.IntNull.ToString() ));
		}
		private void BindDistrict()
		{
			ddlDistrict.Items.Clear();
			int citySysNo = Convert.ToInt32(ddlCity.SelectedItem.Value);
			SortedList sl = ASPManager.GetInstance().GetDistrictList(citySysNo, true);
			if ( sl != null )
			{
				foreach(AreaInfo item in sl.Keys)
				{
					ddlDistrict.Items.Add(
						new ListItem( item.DistrictName, item.SysNo.ToString() ) );
				}
			}
			ddlDistrict.Items.Insert(0, new ListItem( AppConst.AllSelectString, AppConst.IntNull.ToString() ));
		}
		#endregion

		private void ddlProvince_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			BindCity();
			BindDistrict();
		}

		private void ddlCity_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			BindDistrict();
		}


		private bool _autoPostBack;
		public bool AutoPostBack
		{
			get
			{
				return _autoPostBack;
			}
			set
			{
				_autoPostBack = value;
			}

		}

		protected override void OnInit(EventArgs e)
		{
			ddlProvince.AutoPostBack = true;
			ddlProvince.SelectedIndexChanged += new System.EventHandler(ddlProvince_SelectedIndexChanged);
			
			ddlCity.AutoPostBack = true;
			ddlCity.SelectedIndexChanged += new System.EventHandler(ddlCity_SelectedIndexChanged);

			if ( AutoPostBack)
			{
				ddlDistrict.AutoPostBack = true;
				ddlProvince.SelectedIndexChanged += new System.EventHandler(ddl_SelectedIndexChanged);
				ddlCity.SelectedIndexChanged += new System.EventHandler(ddl_SelectedIndexChanged);
				ddlDistrict.SelectedIndexChanged += new System.EventHandler(ddl_SelectedIndexChanged);
			}

            Controls.Add(new LiteralControl("<lable>ʡ�ݣ�</lable>"));
			Controls.Add(ddlProvince);
            Controls.Add(new LiteralControl("<lable>���У�</lable>"));
			Controls.Add(ddlCity);
            Controls.Add(new LiteralControl("<lable>���أ�</lable>"));
			Controls.Add(ddlDistrict);

			base.OnInit(e);
		}

		public void ddl_SelectedIndexChanged(object sender, System.EventArgs e)
		{

			if (SelectedIndexChanged != null)
				SelectedIndexChanged(this, EventArgs.Empty);
		}

		public bool Enabled
		{
			set
			{
				ddlProvince.Enabled = value;
				ddlCity.Enabled = value;
				ddlProvince.Enabled = value;
			}
		}

		public event EventHandler SelectedIndexChanged;

	}
}
