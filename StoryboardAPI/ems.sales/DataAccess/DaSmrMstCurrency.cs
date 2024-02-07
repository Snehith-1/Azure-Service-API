﻿using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using ems.sales.Models;
using static ems.sales.Models.MdlSmrMstCurrency;

namespace ems.sales.DataAccess
{
    public class DaSmrMstCurrency

    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        MySqlDataReader objMySqlDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        public void DaGetSmrCurrencySummary(MdlSmrMstCurrency values)
        {
            try {
               
                msSQL = " select  currencyexchange_gid,currency_code,exchange_rate,country as country_name , CONCAT(b.user_firstname,' ',b.user_lastname) as created_by,date_format(a.created_date,'%d-%m-%Y')  as created_date " +
                    " from crm_trn_tcurrencyexchange a " +
                    " left join adm_mst_tuser b on b.user_gid=a.created_by order by a.created_date desc";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<Getsales_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new Getsales_list
                    {
                        currencyexchange_gid = dt["currencyexchange_gid"].ToString(),
                        currency_code = dt["currency_code"].ToString(),
                        exchange_rate = dt["exchange_rate"].ToString(),
                        created_by = dt["created_by"].ToString(),
                        created_date = dt["created_date"].ToString(),
                        country_name = dt["country_name"].ToString()
                    });
                    values.salescurrency_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Currency Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
        public void DaGetSmrCountryDtl(MdlSmrMstCurrency values)
        {
            try {
               
                msSQL = " select  country_gid, country_code, country_name " +
                    " from adm_mst_tcountry ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<Getcountrydropdown>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new Getcountrydropdown
                    {
                        country_gid = dt["country_gid"].ToString(),
                        country_name = dt["country_name"].ToString(),
                    });
                    values.GetSmrCountryDtl = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Country Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
        public void DaPostSmrCurrency(string user_gid, Getsales_list values)
        {
            try { 
               
            msSQL = " select * from crm_trn_tcurrencyexchange where country_gid= '" + values.country_name + "'";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            if (dt_datatable.Rows.Count != 0)
            {
                values.status = false;
                values.message = "Country Name Already Exist";
            }
            

                msSQL = " select * from crm_trn_tcurrencyexchange where currency_code= '" + values.currency_code + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    values.status = false;
                    values.message = "Currency Code Already Exist";
                }

                else
                {
                    msGetGid = objcmnfunctions.GetMasterGID("CUR");
                    msSQL = " Select country_name from adm_mst_tcountry where country_gid= '" + values.country_name + "'";
                    string lscountry_name = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " insert into crm_trn_tcurrencyexchange(" +
                            " currencyexchange_gid," +
                            " currency_code," +
                            " country_gid," +
                            " exchange_rate," +
                            " country," +
                            " created_by, " +
                            " created_date)" +
                            " values(" +
                            " '" + msGetGid + "'," +
                            " '" + values.currency_code.Replace("'", "\\'") + "'," +
                            " '" + values.country_name + "'," +
                            " '" + values.exchange_rate.Replace("'", "\\'") + "',";
                    if (lscountry_name == null || lscountry_name == "")
                    {
                        msSQL += "'',";
                    }
                    else
                    {
                        msSQL += "'" + lscountry_name.Replace("'", "") + "',";
                    }
                    msSQL += "'" + user_gid + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Currency Added Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Currency";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Submiting  Currency !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            

        }
        public void DaSmrCurrencyUpdate(string user_gid, Getsales_list values)
        {
            try {
               
                msSQL = " Select country_gid from adm_mst_tcountry where country_name= '" + values.country_nameedit + "'";
            string lscountry_gid = objdbconn.GetExecuteScalar(msSQL);
            msSQL = " insert into crm_trn_tcurrencyexchangehistory (" +
                   " currency_code," +
                   " exchange_rate," +
                   " country," +
                   " updated_by, " +
                   " updated_date)" +
                   " values(" +
                   " '" + values.currency_codeedit.Replace("'", "\\'") + "'," +
                   "'" + values.exchange_rateedit.Replace("'", "\\'") + "',";
            if (values.country_nameedit == null || values.country_nameedit == "")
            {
                msSQL += "'',";
            }
            else
            {
                msSQL += "'" + values.country_nameedit.Replace("'", "") + "',";
            }
            msSQL += "'" + user_gid + "'," +
                     "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            if (mnResult != 0)

            {
                msSQL = " update  crm_trn_tcurrencyexchange set " +
             " currency_code = '" + values.currency_codeedit + "'," +
             " exchange_rate = '" + values.exchange_rateedit + "'," +
             " country = '" + values.country_nameedit + "'," +
             " country_gid = '" + lscountry_gid + "'," +
             " updated_by = '" + user_gid + "'," +
             " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where currencyexchange_gid='" + values.currencyexchange_gid + "'  ";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Currency Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Currency";
                }
            }
            else
            {
                values.status = false;
                values.message = "Error While Adding Currency";
            }


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updatuin Currency !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            

        }
        public void DaSmrCurrencySummaryDelete(string currencyexchange_gid, Getsales_list values)
        {
            try {
                
                msSQL = "  delete from crm_trn_tcurrencyexchange where currencyexchange_gid='" + currencyexchange_gid + "'  ";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult != 0)
            {
                values.status = true;
                values.message = "Currency Deleted Successfully";
            }
            else
            {
                values.status = false;
                values.message = "Error While Deleting Currency";
            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Currency Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }



    }
}