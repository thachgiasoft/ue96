using System;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Basic;

namespace Icson.DBAccess.Basic
{
    /// <summary>
    /// Summary description for ASPDac.
    /// </summary>
    public class ASPDac
    {

        public ASPDac()
        {
        }
        public int InsertArea(AreaInfo oParam)
        {
            string sql = @"INSERT INTO Area
                            (
                            SysNo, ProvinceSysNo, CitySysNo, 
                            ProvinceName, CityName, DistrictName, IsLocal, Status,LocalCode
                            )
                            VALUES (
                            @SysNo, @ProvinceSysNo, @CitySysNo, 
                            @ProvinceName, @CityName, @DistrictName, @IsLocal, @Status,@LocalCode
                            );
							set @sysno = SCOPE_IDENTITY();
						";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramProvinceSysNo = new SqlParameter("@ProvinceSysNo", SqlDbType.Int, 4);
            SqlParameter paramCitySysNo = new SqlParameter("@CitySysNo", SqlDbType.Int, 4);
            SqlParameter paramProvinceName = new SqlParameter("@ProvinceName", SqlDbType.NVarChar, 20);
            SqlParameter paramCityName = new SqlParameter("@CityName", SqlDbType.NVarChar, 20);
            SqlParameter paramDistrictName = new SqlParameter("@DistrictName", SqlDbType.NVarChar, 200);
            SqlParameter paramIsLocal = new SqlParameter("@IsLocal", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramLocalCode = new SqlParameter("@LocalCode", SqlDbType.Int, 4);

            paramSysNo.Value = oParam.SysNo;

            if (oParam.ProvinceSysNo != AppConst.IntNull)
                paramProvinceSysNo.Value = oParam.ProvinceSysNo;
            else
                paramProvinceSysNo.Value = System.DBNull.Value;

            if (oParam.CitySysNo != AppConst.IntNull)
                paramCitySysNo.Value = oParam.CitySysNo;
            else
                paramCitySysNo.Value = System.DBNull.Value;

            paramProvinceName.Value = oParam.ProvinceName;

            if (oParam.CityName != AppConst.StringNull)
                paramCityName.Value = oParam.CityName;
            else
                paramCityName.Value = System.DBNull.Value;

            if (oParam.DistrictName != AppConst.StringNull)
                paramDistrictName.Value = oParam.DistrictName;
            else
                paramDistrictName.Value = System.DBNull.Value;

            if (oParam.LocalCode != AppConst.IntNull)
                paramLocalCode.Value = oParam.LocalCode;
            else
                paramLocalCode.Value = System.DBNull.Value;

            paramIsLocal.Value = oParam.IsLocal;
            paramStatus.Value = oParam.Status;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramProvinceSysNo);
            cmd.Parameters.Add(paramCitySysNo);
            cmd.Parameters.Add(paramProvinceName);
            cmd.Parameters.Add(paramCityName);
            cmd.Parameters.Add(paramDistrictName);
            cmd.Parameters.Add(paramIsLocal);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramLocalCode);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int UpdateArea(AreaInfo oParam)
        {
            string sql = @"UPDATE Area SET 
                            ProvinceSysNo=@ProvinceSysNo, CitySysNo=@CitySysNo, 
                            ProvinceName=@ProvinceName, CityName=@CityName, 
                            DistrictName=@DistrictName, IsLocal=@IsLocal, Status=@Status,LocalCode=@LocalCode 
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramProvinceSysNo = new SqlParameter("@ProvinceSysNo", SqlDbType.Int, 4);
            SqlParameter paramCitySysNo = new SqlParameter("@CitySysNo", SqlDbType.Int, 4);
            SqlParameter paramProvinceName = new SqlParameter("@ProvinceName", SqlDbType.NVarChar, 20);
            SqlParameter paramCityName = new SqlParameter("@CityName", SqlDbType.NVarChar, 20);
            SqlParameter paramDistrictName = new SqlParameter("@DistrictName", SqlDbType.NVarChar, 200);
            SqlParameter paramIsLocal = new SqlParameter("@IsLocal", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramLocalCode = new SqlParameter("@LocalCode", SqlDbType.Int, 4);


            paramSysNo.Value = oParam.SysNo;
            paramProvinceSysNo.Value = oParam.ProvinceSysNo;
            paramCitySysNo.Value = oParam.CitySysNo;
            paramProvinceName.Value = oParam.ProvinceName;
            paramCityName.Value = oParam.CityName;
            paramDistrictName.Value = oParam.DistrictName;
            paramIsLocal.Value = oParam.IsLocal;
            paramStatus.Value = oParam.Status;
            paramLocalCode.Value = oParam.LocalCode;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramProvinceSysNo);
            cmd.Parameters.Add(paramCitySysNo);
            cmd.Parameters.Add(paramProvinceName);
            cmd.Parameters.Add(paramCityName);
            cmd.Parameters.Add(paramDistrictName);
            cmd.Parameters.Add(paramIsLocal);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramLocalCode);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int InsertPayType(PayTypeInfo oParam)
        {
            string sql = @"INSERT INTO PayType
                            (
                            SysNo, PayTypeID, PayTypeName, PayTypeDesc, 
                            Period, PaymentPage, PayRate, IsNet, 
                            IsPayWhenRecv, OrderNumber, IsOnlineShow
                            )
                            VALUES (
                            @SysNo, @PayTypeID, @PayTypeName, @PayTypeDesc, 
                            @Period, @PaymentPage, @PayRate, @IsNet, 
                            @IsPayWhenRecv, @OrderNumber, @IsOnlineShow
                            )";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramPayTypeID = new SqlParameter("@PayTypeID", SqlDbType.NVarChar, 10);
            SqlParameter paramPayTypeName = new SqlParameter("@PayTypeName", SqlDbType.NVarChar, 50);
            SqlParameter paramPayTypeDesc = new SqlParameter("@PayTypeDesc", SqlDbType.NVarChar, 500);
            SqlParameter paramPeriod = new SqlParameter("@Period", SqlDbType.NVarChar, 50);
            SqlParameter paramPaymentPage = new SqlParameter("@PaymentPage", SqlDbType.NVarChar, 100);
            SqlParameter paramPayRate = new SqlParameter("@PayRate", SqlDbType.Decimal, 9);
            SqlParameter paramIsNet = new SqlParameter("@IsNet", SqlDbType.Int, 4);
            SqlParameter paramIsPayWhenRecv = new SqlParameter("@IsPayWhenRecv", SqlDbType.Int, 4);
            SqlParameter paramOrderNumber = new SqlParameter("@OrderNumber", SqlDbType.NVarChar, 10);
            SqlParameter paramIsOnlineShow = new SqlParameter("@IsOnlineShow", SqlDbType.Int, 4);

            paramSysNo.Value = oParam.SysNo;
            paramPayTypeID.Value = oParam.PayTypeID;
            paramPayTypeName.Value = oParam.PayTypeName;
            paramPayTypeDesc.Value = oParam.PayTypeDesc;
            paramPeriod.Value = oParam.Period;
            paramPaymentPage.Value = oParam.PaymentPage;
            paramPayRate.Value = oParam.PayRate;
            paramIsNet.Value = oParam.IsNet;
            paramIsPayWhenRecv.Value = oParam.IsPayWhenRecv;
            paramOrderNumber.Value = oParam.OrderNumber;
            paramIsOnlineShow.Value = oParam.IsOnlineShow;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramPayTypeID);
            cmd.Parameters.Add(paramPayTypeName);
            cmd.Parameters.Add(paramPayTypeDesc);
            cmd.Parameters.Add(paramPeriod);
            cmd.Parameters.Add(paramPaymentPage);
            cmd.Parameters.Add(paramPayRate);
            cmd.Parameters.Add(paramIsNet);
            cmd.Parameters.Add(paramIsPayWhenRecv);
            cmd.Parameters.Add(paramOrderNumber);
            cmd.Parameters.Add(paramIsOnlineShow);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int UpdatePayType(PayTypeInfo oParam)
        {
            string sql = @"UPDATE PayType SET 
                            PayTypeID=@PayTypeID, 
                            PayTypeName=@PayTypeName, PayTypeDesc=@PayTypeDesc, 
                            Period=@Period, PaymentPage=@PaymentPage, 
                            PayRate=@PayRate, IsNet=@IsNet, 
                            IsPayWhenRecv=@IsPayWhenRecv, OrderNumber=@OrderNumber, 
                            IsOnlineShow=@IsOnlineShow
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramPayTypeID = new SqlParameter("@PayTypeID", SqlDbType.NVarChar, 10);
            SqlParameter paramPayTypeName = new SqlParameter("@PayTypeName", SqlDbType.NVarChar, 50);
            SqlParameter paramPayTypeDesc = new SqlParameter("@PayTypeDesc", SqlDbType.NVarChar, 500);
            SqlParameter paramPeriod = new SqlParameter("@Period", SqlDbType.NVarChar, 50);
            SqlParameter paramPaymentPage = new SqlParameter("@PaymentPage", SqlDbType.NVarChar, 100);
            SqlParameter paramPayRate = new SqlParameter("@PayRate", SqlDbType.Decimal, 9);
            SqlParameter paramIsNet = new SqlParameter("@IsNet", SqlDbType.Int, 4);
            SqlParameter paramIsPayWhenRecv = new SqlParameter("@IsPayWhenRecv", SqlDbType.Int, 4);
            SqlParameter paramOrderNumber = new SqlParameter("@OrderNumber", SqlDbType.NVarChar, 10);
            SqlParameter paramIsOnlineShow = new SqlParameter("@IsOnlineShow", SqlDbType.Int, 4);

            paramSysNo.Value = oParam.SysNo;
            paramPayTypeID.Value = oParam.PayTypeID;
            paramPayTypeName.Value = oParam.PayTypeName;
            paramPayTypeDesc.Value = oParam.PayTypeDesc;
            paramPeriod.Value = oParam.Period;
            paramPaymentPage.Value = oParam.PaymentPage;
            paramPayRate.Value = oParam.PayRate;
            paramIsNet.Value = oParam.IsNet;
            paramIsPayWhenRecv.Value = oParam.IsPayWhenRecv;
            paramOrderNumber.Value = oParam.OrderNumber;
            paramIsOnlineShow.Value = oParam.IsOnlineShow;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramPayTypeID);
            cmd.Parameters.Add(paramPayTypeName);
            cmd.Parameters.Add(paramPayTypeDesc);
            cmd.Parameters.Add(paramPeriod);
            cmd.Parameters.Add(paramPaymentPage);
            cmd.Parameters.Add(paramPayRate);
            cmd.Parameters.Add(paramIsNet);
            cmd.Parameters.Add(paramIsPayWhenRecv);
            cmd.Parameters.Add(paramOrderNumber);
            cmd.Parameters.Add(paramIsOnlineShow);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int InsertShipType(ShipTypeInfo oParam)
        {
            string sql = @"INSERT INTO ShipType
                            (
                            SysNo, ShipTypeID, ShipTypeName, ShipTypeDesc, 
                            Period, Provider, PremiumRate, PremiumBase, 
                            FreeShipBase, OrderNumber, IsOnlineShow, IsWithPackFee,
                            StatusQueryType,StatusQueryUrl
                            )
                            VALUES (
                            @SysNo, @ShipTypeID, @ShipTypeName, @ShipTypeDesc, 
                            @Period, @Provider, @PremiumRate, @PremiumBase, 
                            @FreeShipBase, @OrderNumber, @IsOnlineShow, @IsWithPackFee,
                            @StatusQueryType,@StatusQueryUrl
                            )";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramShipTypeID = new SqlParameter("@ShipTypeID", SqlDbType.NVarChar, 20);
            SqlParameter paramShipTypeName = new SqlParameter("@ShipTypeName", SqlDbType.NVarChar, 50);
            SqlParameter paramShipTypeDesc = new SqlParameter("@ShipTypeDesc", SqlDbType.NVarChar, 500);
            SqlParameter paramPeriod = new SqlParameter("@Period", SqlDbType.NVarChar, 50);
            SqlParameter paramProvider = new SqlParameter("@Provider", SqlDbType.NVarChar, 50);
            SqlParameter paramPremiumRate = new SqlParameter("@PremiumRate", SqlDbType.Decimal, 9);
            SqlParameter paramPremiumBase = new SqlParameter("@PremiumBase", SqlDbType.Decimal, 9);
            SqlParameter paramFreeShipBase = new SqlParameter("@FreeShipBase", SqlDbType.Decimal, 9);
            SqlParameter paramOrderNumber = new SqlParameter("@OrderNumber", SqlDbType.NVarChar, 10);
            SqlParameter paramIsOnlineShow = new SqlParameter("@IsOnlineShow", SqlDbType.Int, 4);
            SqlParameter paramIsWithPackFee = new SqlParameter("@IsWithPackFee", SqlDbType.Int, 4);
            SqlParameter paramStatusQueryType = new SqlParameter("@StatusQueryType", SqlDbType.Int, 4);
            SqlParameter paramStatusQueryUrl = new SqlParameter("@StatusQueryUrl", SqlDbType.NVarChar, 500);

            paramSysNo.Value = oParam.SysNo;
            paramShipTypeID.Value = oParam.ShipTypeID;
            paramShipTypeName.Value = oParam.ShipTypeName;
            paramShipTypeDesc.Value = oParam.ShipTypeDesc;
            paramPeriod.Value = oParam.Period;
            paramProvider.Value = oParam.Provider;
            paramPremiumRate.Value = oParam.PremiumRate;
            paramPremiumBase.Value = oParam.PremiumBase;
            paramFreeShipBase.Value = oParam.FreeShipBase;
            paramOrderNumber.Value = oParam.OrderNumber;
            paramIsOnlineShow.Value = oParam.IsOnlineShow;
            paramIsWithPackFee.Value = oParam.IsWithPackFee;
            paramStatusQueryType.Value = oParam.StatusQueryType;
            paramStatusQueryUrl.Value = oParam.StatusQueryUrl;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramShipTypeID);
            cmd.Parameters.Add(paramShipTypeName);
            cmd.Parameters.Add(paramShipTypeDesc);
            cmd.Parameters.Add(paramPeriod);
            cmd.Parameters.Add(paramProvider);
            cmd.Parameters.Add(paramPremiumRate);
            cmd.Parameters.Add(paramPremiumBase);
            cmd.Parameters.Add(paramFreeShipBase);
            cmd.Parameters.Add(paramOrderNumber);
            cmd.Parameters.Add(paramIsOnlineShow);
            cmd.Parameters.Add(paramIsWithPackFee);
            cmd.Parameters.Add(paramStatusQueryType);
            cmd.Parameters.Add(paramStatusQueryUrl);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
        public int UpdateShipType(ShipTypeInfo oParam)
        {
            string sql = @"UPDATE ShipType SET 
                            ShipTypeID=@ShipTypeID, 
                            ShipTypeName=@ShipTypeName, ShipTypeDesc=@ShipTypeDesc, 
                            Period=@Period, Provider=@Provider, 
                            PremiumRate=@PremiumRate, PremiumBase=@PremiumBase, 
                            FreeShipBase=@FreeShipBase, OrderNumber=@OrderNumber, 
                            IsOnlineShow=@IsOnlineShow, IsWithPackFee=@IsWithPackFee,
                            StatusQueryType=@StatusQueryType,StatusQueryUrl=@StatusQueryUrl 
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramShipTypeID = new SqlParameter("@ShipTypeID", SqlDbType.NVarChar, 20);
            SqlParameter paramShipTypeName = new SqlParameter("@ShipTypeName", SqlDbType.NVarChar, 50);
            SqlParameter paramShipTypeDesc = new SqlParameter("@ShipTypeDesc", SqlDbType.NVarChar, 500);
            SqlParameter paramPeriod = new SqlParameter("@Period", SqlDbType.NVarChar, 50);
            SqlParameter paramProvider = new SqlParameter("@Provider", SqlDbType.NVarChar, 50);
            SqlParameter paramPremiumRate = new SqlParameter("@PremiumRate", SqlDbType.Decimal, 9);
            SqlParameter paramPremiumBase = new SqlParameter("@PremiumBase", SqlDbType.Decimal, 9);
            SqlParameter paramFreeShipBase = new SqlParameter("@FreeShipBase", SqlDbType.Decimal, 9);
            SqlParameter paramOrderNumber = new SqlParameter("@OrderNumber", SqlDbType.NVarChar, 10);
            SqlParameter paramIsOnlineShow = new SqlParameter("@IsOnlineShow", SqlDbType.Int, 4);
            SqlParameter paramIsWithPackFee = new SqlParameter("@IsWithPackFee", SqlDbType.Int, 4);
            SqlParameter paramStatusQueryType = new SqlParameter("@StatusQueryType", SqlDbType.Int, 4);
            SqlParameter paramStatusQueryUrl = new SqlParameter("@StatusQueryUrl", SqlDbType.NVarChar, 500);

            paramSysNo.Value = oParam.SysNo;
            paramShipTypeID.Value = oParam.ShipTypeID;
            paramShipTypeName.Value = oParam.ShipTypeName;
            paramShipTypeDesc.Value = oParam.ShipTypeDesc;
            paramPeriod.Value = oParam.Period;
            paramProvider.Value = oParam.Provider;
            paramPremiumRate.Value = oParam.PremiumRate;
            paramPremiumBase.Value = oParam.PremiumBase;
            paramFreeShipBase.Value = oParam.FreeShipBase;
            paramOrderNumber.Value = oParam.OrderNumber;
            paramIsOnlineShow.Value = oParam.IsOnlineShow;
            paramIsWithPackFee.Value = oParam.IsWithPackFee;
            paramStatusQueryType.Value = oParam.StatusQueryType;
            paramStatusQueryUrl.Value = oParam.StatusQueryUrl;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramShipTypeID);
            cmd.Parameters.Add(paramShipTypeName);
            cmd.Parameters.Add(paramShipTypeDesc);
            cmd.Parameters.Add(paramPeriod);
            cmd.Parameters.Add(paramProvider);
            cmd.Parameters.Add(paramPremiumRate);
            cmd.Parameters.Add(paramPremiumBase);
            cmd.Parameters.Add(paramFreeShipBase);
            cmd.Parameters.Add(paramOrderNumber);
            cmd.Parameters.Add(paramIsOnlineShow);
            cmd.Parameters.Add(paramIsWithPackFee);
            cmd.Parameters.Add(paramStatusQueryType);
            cmd.Parameters.Add(paramStatusQueryUrl);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int InsertShipArea(ShipAreaInfo oParam)
        {
            string sql = @"INSERT INTO ShipType_Area_Un
                            (
                            ShipTypeSysNo, AreaSysNo
                            )
                            VALUES (
                            @ShipTypeSysNo, @AreaSysNo
                            );
							set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramShipTypeSysNo = new SqlParameter("@ShipTypeSysNo", SqlDbType.Int, 4);
            SqlParameter paramAreaSysNo = new SqlParameter("@AreaSysNo", SqlDbType.Int, 4);

            paramSysNo.Direction = ParameterDirection.Output;

            paramShipTypeSysNo.Value = oParam.ShipTypeSysNo;
            paramAreaSysNo.Value = oParam.AreaSysNo;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramShipTypeSysNo);
            cmd.Parameters.Add(paramAreaSysNo);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }
        public int DeleteShipArea(int paramSysNo)
        {
            string sql = "delete from ShipType_Area_Un where sysno =" + paramSysNo;
            SqlCommand cmd = new SqlCommand(sql);
            return SqlHelper.ExecuteNonQuery(cmd);
        }
        public int InsertShipPay(ShipPayInfo oParam)
        {
            string sql = @"INSERT INTO ShipType_PayType_Un
                            (
                            ShipTypeSysNo, PayTypeSysNo
                            )
                            VALUES (
                            @ShipTypeSysNo, @PayTypeSysNo
                            );
							set @SysNo = SCOPE_IDENTITY();";

            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramShipTypeSysNo = new SqlParameter("@ShipTypeSysNo", SqlDbType.Int, 4);
            SqlParameter paramPayTypeSysNo = new SqlParameter("@PayTypeSysNo", SqlDbType.Int, 4);

            paramSysNo.Direction = ParameterDirection.Output;
            paramShipTypeSysNo.Value = oParam.ShipTypeSysNo;
            paramPayTypeSysNo.Value = oParam.PayTypeSysNo;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramShipTypeSysNo);
            cmd.Parameters.Add(paramPayTypeSysNo);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);

        }
        public int DeleteShipPay(int paramSysNo)
        {
            string sql = "delete from ShipType_PayType_Un where sysno =" + paramSysNo;
            SqlCommand cmd = new SqlCommand(sql);
            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int InsertShipAreaPrice(ShipAreaPriceInfo oParam)
        {
            string sql = @"INSERT INTO ShipType_Area_Price
                            (
                            ShipTypeSysNo, AreaSysNo, BaseWeight, 
                            TopWeight, UnitWeight, UnitPrice, MaxPrice
                            )
                            VALUES (
                            @ShipTypeSysNo, @AreaSysNo, @BaseWeight, 
                            @TopWeight, @UnitWeight, @UnitPrice, @MaxPrice
                            );
							set @SysNo = SCOPE_IDENTITY();";

            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramShipTypeSysNo = new SqlParameter("@ShipTypeSysNo", SqlDbType.Int, 4);
            SqlParameter paramAreaSysNo = new SqlParameter("@AreaSysNo", SqlDbType.Int, 4);
            SqlParameter paramBaseWeight = new SqlParameter("@BaseWeight", SqlDbType.Int, 4);
            SqlParameter paramTopWeight = new SqlParameter("@TopWeight", SqlDbType.Int, 4);
            SqlParameter paramUnitWeight = new SqlParameter("@UnitWeight", SqlDbType.Int, 4);
            SqlParameter paramUnitPrice = new SqlParameter("@UnitPrice", SqlDbType.Decimal, 9);
            SqlParameter paramMaxPrice = new SqlParameter("@MaxPrice", SqlDbType.Decimal, 9);

            paramSysNo.Direction = ParameterDirection.Output;
            paramShipTypeSysNo.Value = oParam.ShipTypeSysNo;
            paramAreaSysNo.Value = oParam.AreaSysNo;
            paramBaseWeight.Value = oParam.BaseWeight;
            paramTopWeight.Value = oParam.TopWeight;
            paramUnitWeight.Value = oParam.UnitWeight;
            paramUnitPrice.Value = oParam.UnitPrice;
            paramMaxPrice.Value = oParam.MaxPrice;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramShipTypeSysNo);
            cmd.Parameters.Add(paramAreaSysNo);
            cmd.Parameters.Add(paramBaseWeight);
            cmd.Parameters.Add(paramTopWeight);
            cmd.Parameters.Add(paramUnitWeight);
            cmd.Parameters.Add(paramUnitPrice);
            cmd.Parameters.Add(paramMaxPrice);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);

        }
        public int DeleteShipAreaPrice(int paramSysNo)
        {
            string sql = "delete from ShipType_Area_Price where sysno =" + paramSysNo;
            SqlCommand cmd = new SqlCommand(sql);
            return SqlHelper.ExecuteNonQuery(cmd);

        }
        public int InsertFreightManArea(FreightManAreaInfo oParam)
        {
            string sql = @"INSERT INTO FreightMan_Area
                            (
                            FreightUserSysNo, AreaSysNo
                            )
                            VALUES (
                            @FreightUserSysNo, @AreaSysNo
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramFreightUserSysNo = new SqlParameter("@FreightUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramAreaSysNo = new SqlParameter("@AreaSysNo", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.FreightUserSysNo != AppConst.IntNull)
                paramFreightUserSysNo.Value = oParam.FreightUserSysNo;
            else
                paramFreightUserSysNo.Value = System.DBNull.Value;
            if (oParam.AreaSysNo != AppConst.IntNull)
                paramAreaSysNo.Value = oParam.AreaSysNo;
            else
                paramAreaSysNo.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramFreightUserSysNo);
            cmd.Parameters.Add(paramAreaSysNo);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }
        public int DeleteFreightManArea(int paramSysNo)
        {
            string sql = "delete from FreightMan_Area where sysno=" + paramSysNo;
            SqlCommand cmd = new SqlCommand(sql);
            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int InsertPayPlusCharge(PayPlusChargeInfo oParam)
        {
            string sql = @"INSERT INTO PayPlusCharge
                            (
                            SysNo, BankName, InstallmentNum, PlusRate, 
                            Status
                            )
                            VALUES (
                            @SysNo, @BankName, @InstallmentNum, @PlusRate, 
                            @Status
                            )";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramBankName = new SqlParameter("@BankName", SqlDbType.NVarChar, 50);
            SqlParameter paramInstallmentNum = new SqlParameter("@InstallmentNum", SqlDbType.Int, 4);
            SqlParameter paramPlusRate = new SqlParameter("@PlusRate", SqlDbType.Decimal, 9);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.BankName != AppConst.StringNull)
                paramBankName.Value = oParam.BankName;
            else
                paramBankName.Value = System.DBNull.Value;
            if (oParam.InstallmentNum != AppConst.IntNull)
                paramInstallmentNum.Value = oParam.InstallmentNum;
            else
                paramInstallmentNum.Value = System.DBNull.Value;
            if (oParam.PlusRate != AppConst.DecimalNull)
                paramPlusRate.Value = oParam.PlusRate;
            else
                paramPlusRate.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramBankName);
            cmd.Parameters.Add(paramInstallmentNum);
            cmd.Parameters.Add(paramPlusRate);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int UpdatePayPlusCharge(PayPlusChargeInfo oParam)
        {
            string sql = @"UPDATE PayPlusCharge SET 
                            BankName=@BankName, InstallmentNum=@InstallmentNum, 
                            PlusRate=@PlusRate, Status=@Status
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramBankName = new SqlParameter("@BankName", SqlDbType.NVarChar, 50);
            SqlParameter paramInstallmentNum = new SqlParameter("@InstallmentNum", SqlDbType.Int, 4);
            SqlParameter paramPlusRate = new SqlParameter("@PlusRate", SqlDbType.Decimal, 9);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.BankName != AppConst.StringNull)
                paramBankName.Value = oParam.BankName;
            else
                paramBankName.Value = System.DBNull.Value;
            if (oParam.InstallmentNum != AppConst.IntNull)
                paramInstallmentNum.Value = oParam.InstallmentNum;
            else
                paramInstallmentNum.Value = System.DBNull.Value;
            if (oParam.PlusRate != AppConst.DecimalNull)
                paramPlusRate.Value = oParam.PlusRate;
            else
                paramPlusRate.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramBankName);
            cmd.Parameters.Add(paramInstallmentNum);
            cmd.Parameters.Add(paramPlusRate);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}
