﻿using ems.sales.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Configuration;
using System.IO;
using OfficeOpenXml;
using System.Data.OleDb;
using OfficeOpenXml.Style;
using System.Drawing;
using MySql.Data.MySqlClient;

namespace ems.sales.DataAccess
{
    public class DaSmrMstProduct
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        private MySqlDataReader objMySqlDataReader;
        
        DataTable dt_datatable;
        string exclproducttypecode;
        string msEmployeeGID, lsemployee_gid, lsuser_gid, lsentity_code, mcGetGID, msGetGID,maGetGID, mrGetGID, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, status;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;

        
        public void DaGetSalesProductSummary(MdlSmrMstProduct values)
        {
            try {
                
                msSQL = " SELECT d.producttype_name,b.productgroup_name,b.productgroup_code,a.product_gid, a.product_price, a.cost_price, a.product_code, CONCAT_WS('|',a.product_name,a.size, a.width, a.length) as product_name,  CONCAT(f.user_firstname,' ',f.user_lastname) as created_by,date_format(a.created_date,'%d-%m-%Y')  as created_date, " +
                    " c.productuomclass_code,e.productuom_code,c.productuomclass_name,(case when a.stockable ='Y' then 'Yes' else 'No ' end) as stockable,e.productuom_name,d.producttype_name as product_type, a.status," +
                    " (case when a.serial_flag ='Y' then 'Yes' else 'No' end)as serial_flag,(case when a.avg_lead_time is null then '0 days' else concat(a.avg_lead_time,'  ', 'days') end)as lead_time  from pmr_mst_tproduct a " +
                    " left join pmr_mst_tproductgroup b on a.productgroup_gid = b.productgroup_gid " +
                    " left join pmr_mst_tproductuomclass c on a.productuomclass_gid = c.productuomclass_gid " +
                    " left join pmr_mst_tproducttype d on a.producttype_gid = d.producttype_gid " +
                    " left join pmr_mst_tproductuom e on a.productuom_gid = e.productuom_gid " +
                    " left join adm_mst_tuser f on f.user_gid=a.created_by order by a.created_date desc";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<product_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new product_list
                    {
                        product_gid = dt["product_gid"].ToString(),
                        product_name = dt["product_name"].ToString(),
                        product_code = dt["product_code"].ToString(),
                        created_by = dt["created_by"].ToString(),
                        created_date = dt["created_date"].ToString(),

                        producttype_name = dt["producttype_name"].ToString(),
                        productgroup_name = dt["productgroup_name"].ToString(),
                        productgroup_code = dt["productgroup_code"].ToString(),
                        product_price = dt["product_price"].ToString(),
                        cost_price = dt["cost_price"].ToString(),
                        productuomclass_code = dt["productuomclass_code"].ToString(),
                        productuom_code = dt["productuom_code"].ToString(),
                        productuomclass_name = dt["productuomclass_name"].ToString(),
                        stockable = dt["stockable"].ToString(),

                        productuom_name = dt["productuom_name"].ToString(),
                        product_type = dt["product_type"].ToString(),
                        Status = dt["status"].ToString(),
                        serial_flag = dt["serial_flag"].ToString(),
                        lead_time = dt["lead_time"].ToString(),


                    });
                    values.product_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Loading Product Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        // Add Event

        public void DaPostSalesProduct(string user_gid, product_list values)
        {
            try {
               


                msGetGid = objcmnfunctions.GetMasterGID("PPTM");
            string msGetGid2 = objcmnfunctions.GetMasterGID("VPDC");


            msSQL = " insert into pmr_mst_tproduct (" +
                    " product_gid," +
                    " product_code," +
                    " product_name," +
                    " product_desc, " +
                    " productgroup_gid, " +
                    " productuomclass_gid, " +
                    " productuom_gid, " +
                    " mrp_price, " +
                    " cost_price, " +
                    " avg_lead_time, " +
                    " stockable, " +
                    " status, " +
                    " producttype_gid, " +
                    " purchasewarrenty_flag, " +
                    " expirytracking_flag, " +
                    " batch_flag," +
                    " serial_flag," +
                    " created_by, " +
                    " created_date)" +
                    " values(" +
                    " '" + msGetGid + "'," +
                    "'" + values.product_code + "',";
            if (values.product_name == null || values.product_name == "")
            {
                msSQL += "'',";
            }
            else
            {
                msSQL += "'" + values.product_name.Replace("'", "\\'") + "',";
            }
            msSQL += "'" + values.product_desc + "'," +
                     "'" + values.productgroup_name + "'," +
                     "'" + values.productuomclass_name + "'," +
                     "'" + values.productuom_name + "'," +
                    "'" + values.mrp_price + "'," +
                     "'" + values.cost_price + "'," +
                     "'" + values.avg_lead_time + "'," +
                     "'" + "Y" + "'," +
                     "'" + "Active" + "'," +
                     "'" + values.producttype_name + "'," +
                     "'" + values.purchasewarrenty_flag + "'," +
                     "'" + values.expirytracking_flag + "'," +
                     "'" + values.batch_flag + "'," +
                     "'" + values.serial_flag + "'," +
                     "'" + user_gid + "'," +
                     "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            msSQL = " SELECT productuom_gid FROM pmr_mst_tproductuom WHERE productuom_name='" + values.productuom_name + "' ";
            string lsproductuomgid = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " SELECT productuomclass_gid FROM pmr_mst_tproductuom WHERE productuomclass_name='" + values.productuomclass_name + "' ";
            string lsproductuomclass_gid = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " SELECT productgroup_gid FROM pmr_mst_tproductgroup WHERE productgroup_name='" + values.productgroup_name + "' ";
            string lsproductgroupgid = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " SELECT product_gid FROM pmr_mst_tproduct WHERE product_name='" + values.product_name + "' ";
            string lsproductgid = objdbconn.GetExecuteScalar(msSQL);

            if (mnResult != 0)
            {
                msSQL = " insert into smr_trn_tpricelistdtl (" +
               " pricelist_gid, " +
               " branch_gid, " +
               " product_code, " +
               " productuom_gid, " +
               " productuom_name, " +
               " productuomclass_gid," +
               " productgroup_gid," +
               " productgroup_name," +
               " product_price," +
               " product_gid, " +
                " created_by, " +
                " created_date,  " +
               " product_name " +
                ")" +
               " values ( " +
               "'" + msGetGid2 + "', " +
               "'" + values.branch_gid + "'," +
               "'" + values.product_code + "'," +
               "'" + lsproductuomgid + "'," +
               "'" + values.productuom_name + "'," +
               "'" + lsproductuomclass_gid + "'," +
               "'" + lsproductgroupgid + "', " +
               "'" + values.productgroup_name + "', " +
                "'" + values.cost_price + "', " +
               "'" + lsproductgid + "', " +
               "'" + user_gid + "', " +
               "'" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
               "'" + values.product_name + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }

            if (mnResult != 0)
            {
                values.status = true;
                values.message = "Product Added Successfully";
            }
            else
            {
                values.status = false;
                values.message = "Please Enter All Mandatory Fields";
            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Submitting Product  !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           

        }

        //product type dropdown
        public void DaGetProducttype(MdlSmrMstProduct values)
        {
            try {
               
                msSQL = " Select producttype_name,producttype_gid  " +
                    " from pmr_mst_tproducttype ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetProducttype>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetProducttype
                    {
                        producttype_name = dt["producttype_name"].ToString(),
                        producttype_gid = dt["producttype_gid"].ToString(),
                    });
                    values.GetProducttype = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product Type !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }

        //product group dropdown
        public void DaGetProductGroup(MdlSmrMstProduct values)
        {
            try {

                msSQL = " Select productgroup_gid, productgroup_name from pmr_mst_tproductgroup  " +
                    " order by productgroup_name asc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetProductGroup>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetProductGroup
                    {
                        productgroup_gid = dt["productgroup_gid"].ToString(),
                        productgroup_name = dt["productgroup_name"].ToString(),
                    });
                    values.GetProductGroup = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Prodcut Name !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        //product unit class
        public void DaGetProductUnitclass(MdlSmrMstProduct values)
        {
            try {
               
                msSQL = " Select productuomclass_gid, productuomclass_code, productuomclass_name  " +
                    " from pmr_mst_tproductuomclass order by productuomclass_name asc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetProductUnitclass>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetProductUnitclass
                    {
                        productuomclass_gid = dt["productuomclass_gid"].ToString(),
                        productuomclass_code = dt["productuomclass_code"].ToString(),
                        productuomclass_name = dt["productuomclass_name"].ToString(),
                    });
                    values.GetProductUnitclass = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product UOM Class Name!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }

        //product unit
        public void DaGetProductUnit(MdlSmrMstProduct values)
        {
            try {
               
                msSQL = " select productuom_name,productuom_gid from pmr_mst_tproductuom a left join pmr_mst_tproductuomclass b on b.productuomclass_gid= a.productuomclass_gid  " +
                    " order by a.sequence_level ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetProductUnit>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetProductUnit
                    {
                        productuom_name = dt["productuom_name"].ToString(),
                        productuom_gid = dt["productuom_gid"].ToString(),

                    });
                    values.GetProductUnit = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product UOM Name !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }

        //on change
        public void DaGetOnChangeProductUnitClass(string productuomclass_gid, MdlSmrMstProduct values)
        {
            try {
               
                msSQL = " select productuom_name,productuom_gid from pmr_mst_tproductuom a left join pmr_mst_tproductuomclass b on b.productuomclass_gid= a.productuomclass_gid  " +
                     " where b.productuomclass_gid ='" + productuomclass_gid + "' order by a.sequence_level  ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetProductUnit>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetProductUnit
                    {
                        productuom_name = dt["productuom_name"].ToString(),
                        productuom_gid = dt["productuom_gid"].ToString(),

                    });
                    values.GetProductUnit = getModuleList;
                }
            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product Unit Class Name  !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        public void DaSmrMstProductUpdate(string user_gid, product_list values)
        {
            try {
               
                msSQL = " SELECT productgroup_gid FROM pmr_mst_tproductgroup WHERE productgroup_name='" + values.productgroupname + "' ";
            string lsproductgroup_gid = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " SELECT producttype_gid FROM pmr_mst_tproducttype WHERE producttype_name='" + values.producttypename + "' ";
            string lsproducttype_gid = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " SELECT productuomclass_gid FROM pmr_mst_tproductuomclass WHERE productuomclass_name='" + values.productuomclassname + "' ";
            string lsproductuomclass_gid = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " SELECT productuom_gid FROM pmr_mst_tproductuom WHERE productuom_name='" + values.productuomname + "' ";
            string lsproductuom_gid = objdbconn.GetExecuteScalar(msSQL);


            msSQL = " update  pmr_mst_tproduct  set " +
          " product_name = '" + values.product_name + "'," +
          " product_code = '" + values.product_code + "'," +
          " product_desc = '" + values.product_desc + "'," +
          " currency_code = '" + values.currency_code + "'," +
          " productgroup_gid = '" + lsproductgroup_gid + "'," +
          " producttype_gid = '" + lsproducttype_gid + "'," +
          " productuomclass_gid = '" + lsproductuomclass_gid + "'," +
          " productuom_gid = '" + lsproductuom_gid + "'," +
          " mrp_price = '" + values.mrp_price + "'," +
          " cost_price = '" + values.cost_price + "'," +
          " avg_lead_time = '" + values.avg_lead_time + "'," +
          " purchasewarrenty_flag = '" + values.purchasewarrenty_flag + "'," +
          " expirytracking_flag = '" + values.expirytracking_flag + "'," +
          " batch_flag = '" + values.batch_flag + "'," +
          " serial_flag = '" + values.serial_flag + "'," +
          " updated_by = '" + user_gid + "'," +
          " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where product_gid='" + values.product_gid + "'  ";

            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult != 0)
            {

                values.status = true;
                values.message = "Product Updated Successfully";

            }
            else
            {
                values.status = false;
                values.message = "Error While Updating Product";
            }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Product !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            


        }

        //Edit
        public void DaGetEditProductSummary(string product_gid, MdlSmrMstProduct values)
        {
            try {
               
                msSQL = "  select  a.batch_flag,a.serial_flag, a.purchasewarrenty_flag,a.expirytracking_flag,a.product_desc,a.avg_lead_time," +
              "  a.mrp_price,a.cost_price,a.product_gid,a.product_name,a.product_code,b.productgroup_gid,b.productgroup_name,c.productuomclass_gid,c.productuomclass_name," +
              "  d.producttype_gid,d.producttype_name,e.productuom_gid,e.productuom_name from pmr_mst_tproduct a " +
              " left join pmr_mst_tproductgroup b on a.productgroup_gid = b.productgroup_gid" +
              " left join pmr_mst_tproductuomclass c on a.productuomclass_gid = c.productuomclass_gid" +
              " left join pmr_mst_tproducttype d on a.producttype_gid = d.producttype_gid " +
              " left join pmr_mst_tproductuom e on a.productuom_gid = e.productuom_gid" +
              " where a.product_gid='" + product_gid + "' ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetEditProductSummary>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetEditProductSummary
                    {


                        productgroup_name = dt["productgroup_name"].ToString(),
                        productgroup_gid = dt["productgroup_gid"].ToString(),
                        product_name = dt["product_name"].ToString(),
                        product_gid = dt["product_gid"].ToString(),
                        product_code = dt["product_code"].ToString(),
                        productuomclass_name = dt["productuomclass_name"].ToString(),
                        productuomclass_gid = dt["productuomclass_gid"].ToString(),
                        productuom_name = dt["productuom_name"].ToString(),
                        productuom_gid = dt["productuom_gid"].ToString(),
                        producttype_name = dt["producttype_name"].ToString(),
                        producttype_gid = dt["producttype_gid"].ToString(),
                        batch_flag = dt["batch_flag"].ToString(),
                        serial_flag = dt["serial_flag"].ToString(),
                        expirytracking_flag = dt["expirytracking_flag"].ToString(),
                        purchasewarrenty_flag = dt["purchasewarrenty_flag"].ToString(),
                        cost_price = dt["cost_price"].ToString(),
                        mrp_price = dt["mrp_price"].ToString(),
                        product_desc = dt["product_desc"].ToString(),
                        avg_lead_time = dt["avg_lead_time"].ToString(),



                    });
                    values.GetEditProductSummary = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Prodcut Detailes !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        //View

        public void DaGetViewProductSummary(string product_gid, MdlSmrMstProduct values)
        {
            try {
               

                msSQL = "  select CASE WHEN a.batch_flag = 'N' THEN 'NO' ELSE 'YES' END AS batch_flag,CASE WHEN a.serial_flag = 'N' THEN 'NO' ELSE 'YES' END AS serial_flag," +
                    " CASE WHEN a.purchasewarrenty_flag = 'N' THEN 'NO' ELSE 'YES' END AS purchasewarrenty_flag,CASE WHEN a.expirytracking_flag = 'N' THEN 'NO' ELSE 'YES' END AS expirytracking_flag," +
                    "  a.product_desc,a.avg_lead_time,a.mrp_price,e.producttype_name,a.cost_price,b.currency_code,a.product_gid,c.productgroup_name,a.product_name,f.productuomclass_name,a.product_code,d.productuom_name " +
                    "  from pmr_mst_tproduct a " +
                    "  left join crm_trn_tcurrencyexchange b on b.currency_code=a.currency_code" +
                    "  left  join pmr_mst_tproductgroup c on a.productgroup_gid=c.productgroup_gid" +
                    "  left join pmr_mst_tproductuom d on a.productuom_gid=d.productuom_gid" +
                    "  left  join pmr_mst_tproducttype e on a.producttype_gid=e.producttype_gid" +
                    "  left  join pmr_mst_tproductuomclass f on a.productuomclass_gid=f.productuomclass_gid" +
                    "  where a.product_gid='" + product_gid + "' group by a.product_gid";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetViewProductSummary>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetViewProductSummary
                    {


                        productgroup_name = dt["productgroup_name"].ToString(),
                        product_name = dt["product_name"].ToString(),
                        product_code = dt["product_code"].ToString(),
                        productuomclass_name = dt["productuomclass_name"].ToString(),
                        productuom_name = dt["productuom_name"].ToString(),
                        producttype_name = dt["producttype_name"].ToString(),
                        batch_flag = dt["batch_flag"].ToString(),
                        serial_flag = dt["serial_flag"].ToString(),
                        expirytracking_flag = dt["expirytracking_flag"].ToString(),
                        purchasewarrenty_flag = dt["purchasewarrenty_flag"].ToString(),
                        cost_price = dt["cost_price"].ToString(),
                        mrp_price = dt["mrp_price"].ToString(),
                        product_desc = dt["product_desc"].ToString(),
                        avg_lead_time = dt["avg_lead_time"].ToString(),
                        product_gid = dt["product_gid"].ToString(),
                        currency_code = dt["currency_code"].ToString(),

                    });
                    values.GetViewProductSummary = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Prodcut  View !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        public void DaGetDeleteSalesProductdetails(string product_gid, product_list values)
        {
            try {
               
                msSQL = "  delete from pmr_mst_tproduct where product_gid='" + product_gid + "'  ";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult != 0)
            {
                values.status = true;
                values.message = "Product Deleted Successfully";
            }
            else
            {
                values.status = false;
                values.message = "Error While Deleting Product";
            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured whileDeleting Product !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        public void DaProductImportExcel(HttpRequest httpRequest, string user_gid, result productresult, product_list values)
        {
            try {
               
                string lscompany_code;
            try
            {
                HttpFileCollection httpFileCollection;
                string lspath, lsfilePath;

                msSQL = " select company_code from adm_mst_tcompany";
                lscompany_code = objdbconn.GetExecuteScalar(msSQL);

                // Create Directory
                lsfilePath = ConfigurationManager.AppSettings["importexcelfile1"];

                if (!Directory.Exists(lsfilePath))
                    Directory.CreateDirectory(lsfilePath);

                httpFileCollection = httpRequest.Files;
                for (int i = 0; i < httpFileCollection.Count; i++)
                {
                    httpPostedFile = httpFileCollection[i];
                }
                string FileExtension = httpPostedFile.FileName;

                string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                string lsfile_gid = msdocument_gid;
                FileExtension = Path.GetExtension(FileExtension).ToLower();
                lsfile_gid = lsfile_gid + FileExtension;
                FileInfo fileinfo = new FileInfo(lsfilePath);
                Stream ls_readStream;
                ls_readStream = httpPostedFile.InputStream;
                MemoryStream ms = new MemoryStream();
                ls_readStream.CopyTo(ms);

                //path creation        
                lspath = lsfilePath + "/";
                FileStream file = new FileStream(lspath + lsfile_gid, FileMode.Create, FileAccess.Write);
                ms.WriteTo(file);
                try
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;                    
                    string status;
                    status = objcmnfunctions.uploadFile(lsfilePath, FileExtension);
                    file.Close();
                    ms.Close();                    
                }
                catch (Exception ex)
                {
                    productresult.status = false;
                    productresult.message = ex.ToString();
                    return;
                }

                //Excel To DataTable
                try
                {
                    DataTable dataTable = new DataTable();
                    int totalSheet = 1;
                    string connectionString = string.Empty;
                    string fileExtension = Path.GetExtension(lspath);

                    //lsfilePath = @"" + lsfilePath.Replace("/", "\\") + "\\" + lsfile_gid + "";
                    //string correctedPath = Regex.Replace(lsfilePath, @"\\+", @"\");
                    //excelRange = "A1:" + endRange + rowCount.ToString();
                    //dt = objcmnfunctions.ExcelToDataTable(correctedPath, excelRange);
                    //dt = dt.Rows.Cast<DataRow>().Where(r => string.Join("", r.ItemArray).Trim() != string.Empty).CopyToDataTable();
                    lsfilePath = @"" + lsfilePath.Replace("/", "\\") + "\\" + lsfile_gid + "";

                    string correctedPath = Regex.Replace(lsfilePath, @"\\+", @"\");
                    
                    try
                    {
                        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + correctedPath + ";Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1;MAXSCANROWS=0';";
                    }
                    catch (Exception ex)
                    {

                    }

                    using (OleDbConnection connection = new OleDbConnection(connectionString))
                    {
                        connection.Open();
                        DataTable schemaTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                        if (schemaTable != null)
                        {
                            var tempDataTable = (from dataRow in schemaTable.AsEnumerable()
                                                 where !dataRow["TABLE_NAME"].ToString().Contains("FilterDatabase")
                                                 select dataRow).CopyToDataTable();

                            schemaTable = tempDataTable;
                            totalSheet = schemaTable.Rows.Count;
                            using (OleDbCommand command = new OleDbCommand())
                            {                                
                                command.Connection = connection;
                                command.CommandText = "select * from [Sheet1$]";

                                using (OleDbDataReader reader = command.ExecuteReader())
                                {
                                    dataTable.Load(reader);
                                }                             

                                foreach (DataRow dt_product in dataTable.Rows)
                                {
                                    string exproducttype = dt_product["PRODUCT TYPE"].ToString();
                                    string exproductgroup = dt_product["PRODUCT GROUP"].ToString();
                                    string exproductcode = dt_product["PRODUCT CODE"].ToString();
                                    string exproducthsncode = dt_product["HSN CODE"].ToString();
                                    string exproduct = dt_product["PRODUCT"].ToString();
                                    string exproductdescription = dt_product["PRODUCT DESCRIPTION"].ToString();
                                    string exproductunit = dt_product["UNITS"].ToString();
                                    string exproductserialno = dt_product["SERIAL NUMBER TRACKER"].ToString();
                                    string exproductwarrenty = dt_product["WARRANTY TRACKER"].ToString();
                                    string exproductExpirydate = dt_product["EXPIRY DATE TRACKER"].ToString();
                                    string exproductcostprice = dt_product["COST PRICE"].ToString();
                                    string lsstatus = "Y", lsserialflag = "1", lsserialtracking_flag = "", lsstockflag = "";
                                    msSQL = "Select producttype_gid from pmr_mst_tproducttype " +
                                            "where producttype_name = '" + exproducttype + "'";
                                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                                    if (objMySqlDataReader.HasRows == true)
                                    {
                                        objMySqlDataReader.Read();
                                        exclproducttypecode = objMySqlDataReader["producttype_gid"].ToString();
                                        objMySqlDataReader.Close();
                                    }
                                    msSQL = " Select productgroup_gid from pmr_mst_tproductgroup " +
                                    " where productgroup_name = '" + exproductgroup + "'";
                                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                                    if (objMySqlDataReader.HasRows == true)
                                    {
                                        objMySqlDataReader.Read();
                                        mcGetGID = objMySqlDataReader["productgroup_gid"].ToString();
                                        objMySqlDataReader.Close();
                                    }
                                    else
                                    {
                                        mcGetGID = objcmnfunctions.GetMasterGID("PPGM");
                                    }                                   
                                    msSQL = "insert into pmr_mst_tproductgroup (" +
                                                    "productgroup_gid, " +
                                                    "productgroup_code, " +
                                                    "productgroup_name) " +
                                                    "values (" +
                                                    "'" + mcGetGID + "', " +
                                                    "'" + exproductgroup + "', " +
                                                    "'" + exproductgroup + "')";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult != 0)
                                    {
                                        msSQL = " Select productuomclass_gid from pmr_mst_tproductuomclass " +
                                                " where productuomclass_name = '" + exproductunit + "'";
                                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                                        if (objMySqlDataReader.HasRows == true)
                                        {
                                            objMySqlDataReader.Read();
                                            maGetGID = objMySqlDataReader["productuomclass_gid"].ToString();
                                            objMySqlDataReader.Close();
                                        }
                                        else
                                        {
                                            maGetGID = objcmnfunctions.GetMasterGID("PUCM");
                                        }
                                        msSQL = "insert into pmr_mst_tproductuomclass (" +
                                                "productuomclass_gid, " +
                                                "productuomclass_code, " +
                                                "productuomclass_name) " +
                                                "values(" +
                                                "'" + maGetGID + "', " +
                                                "'" + exproductunit + "', " +
                                                "'" + exproductunit + "')";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult != 0)
                                        {
                                            msSQL = " Select productuom_gid from pmr_mst_tproductuom " +
                                                    " where productuom_name = '" + exproductunit + "'";
                                            objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                                            if (objMySqlDataReader.HasRows == true)
                                            {
                                                objMySqlDataReader.Read();
                                                msGetGID = objMySqlDataReader["productuom_gid"].ToString();
                                                objMySqlDataReader.Close();
                                            }
                                            else
                                            {
                                                msGetGID = objcmnfunctions.GetMasterGID("PPMM");
                                            }
                                            msSQL = "insert into pmr_mst_tproductuom (" +
                                                    "productuom_gid, " +
                                                    "productuom_code, " +
                                                    "productuom_name, " +
                                                    "productuomclass_gid) " +
                                                    "values(" +
                                                    "'" + msGetGID + "', " +
                                                    "'" + exproductunit + "', " +
                                                    "'" + exproductunit + "', " +
                                                    "'" + maGetGID + "')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            if (mnResult != 0)
                                            {
                                                msSQL = " Select product_gid from pmr_mst_tproduct " +
                                                        " where product_code =  '" + exproductcode + "' and product_name = '" + exproduct + "'";
                                                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                                                if (objMySqlDataReader.HasRows == true)
                                                {
                                                    objMySqlDataReader.Close();
                                                }
                                                else
                                                {
                                                    mrGetGID = objcmnfunctions.GetMasterGID("PPTM");
                                                }

                                                msSQL = "insert into pmr_mst_tproduct (" +
                                                      "product_gid, " +
                                                      "productgroup_gid, " +
                                                                "productuom_gid, " +
                                                               "productuomclass_gid, " +
                                                                "product_code, " +
                                                                "product_name, " +
                                                                "producttype_gid, " +
                                                                "status, " +
                                                                        "serial_flag, " +
                                                                         "serialtracking_flag, " +
                                                                         "warrentytracking_flag, " +
                                                                         "expirytracking_flag, " +
                                                                        " created_by, " +
                                                                         " created_date) " +
                                                                         "values (" +
                                                                       "'" + mrGetGID + "', " +
                                                                         "'" + mcGetGID + "', " +
                                                                         "'" + msGetGID + "'," +
                                                                         "'" + maGetGID + "'," +
                                                                        "'" + exproductcode + "', " +
                                                                         "'" + exproduct + "', " +
                                                                         "'" + exclproducttypecode + "', " +
                                                                        "'" + lsstatus + "', " +
                                                                         "'" + lsserialflag + "', " +
                                                                         "'" + lsserialtracking_flag + "', " +
                                                                        "'" + exproductwarrenty + "', " +
                                                                         "'" + exproductwarrenty + "', " +
                                                                         "'" + user_gid + "'," +
                                                                         "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }               
                catch (Exception ex)
                {
                    productresult.status = false;
                    productresult.message = ex.ToString();
                    return;
                }
            }
            catch (Exception ex)
            {
                productresult.status = false;
                productresult.message = ex.ToString();
            }
            if (mnResult != 0)
            {
                values.status = true;
                values.message = "Product Added Successfully";
            }
            else
            {
                values.status = false;
                values.message = "Please Enter All Mandatory Fields";
            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Importin Product !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }




        //Product Import Excel
        public void DaproductImportExcel(HttpRequest httpRequest, string user_gid, result productresult, product_list values)
        {
            try {
               
                string lscompany_code;
            try
            {
                HttpFileCollection httpFileCollection;
                string lspath, lsfilePath;

                msSQL = " select company_code from adm_mst_tcompany";
                lscompany_code = objdbconn.GetExecuteScalar(msSQL);

                // Create Directory
                lsfilePath = ConfigurationManager.AppSettings["importexcelfile1"];

                if (!Directory.Exists(lsfilePath))
                    Directory.CreateDirectory(lsfilePath);

                httpFileCollection = httpRequest.Files;
                for (int i = 0; i < httpFileCollection.Count; i++)
                {
                    httpPostedFile = httpFileCollection[i];
                }
                string FileExtension = httpPostedFile.FileName;

                string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                string lsfile_gid = msdocument_gid;
                FileExtension = Path.GetExtension(FileExtension).ToLower();
                lsfile_gid = lsfile_gid + FileExtension;
                FileInfo fileinfo = new FileInfo(lsfilePath);
                Stream ls_readStream;
                ls_readStream = httpPostedFile.InputStream;
                MemoryStream ms = new MemoryStream();
                ls_readStream.CopyTo(ms);

                //path creation        
                lspath = lsfilePath + "/";
                FileStream file = new FileStream(lspath + lsfile_gid, FileMode.Create, FileAccess.Write);
                ms.WriteTo(file);
                try
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    string status;
                    status = objcmnfunctions.uploadFile(lsfilePath, FileExtension);
                    file.Close();
                    ms.Close();
                }
                catch (Exception ex)
                {
                    productresult.status = false;
                    productresult.message = ex.ToString();
                    return;
                }

                //Excel To DataTable
                try
                {
                    DataTable dataTable = new DataTable();
                    int totalSheet = 1;
                    string connectionString = string.Empty;
                    string fileExtension = Path.GetExtension(lspath);

                    //lsfilePath = @"" + lsfilePath.Replace("/", "\\") + "\\" + lsfile_gid + "";
                    //string correctedPath = Regex.Replace(lsfilePath, @"\\+", @"\");
                    //excelRange = "A1:" + endRange + rowCount.ToString();
                    //dt = objcmnfunctions.ExcelToDataTable(correctedPath, excelRange);
                    //dt = dt.Rows.Cast<DataRow>().Where(r => string.Join("", r.ItemArray).Trim() != string.Empty).CopyToDataTable();
                    lsfilePath = @"" + lsfilePath.Replace("/", "\\") + "\\" + lsfile_gid + "";

                    string correctedPath = Regex.Replace(lsfilePath, @"\\+", @"\");

                    try
                    {
                        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + correctedPath + ";Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1;MAXSCANROWS=0';";
                    }
                    catch (Exception ex)
                    {

                    }

                    using (OleDbConnection connection = new OleDbConnection(connectionString))
                    {
                        connection.Open();
                        DataTable schemaTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                        if (schemaTable != null)
                        {
                            var tempDataTable = (from dataRow in schemaTable.AsEnumerable()
                                                 where !dataRow["TABLE_NAME"].ToString().Contains("FilterDatabase")
                                                 select dataRow).CopyToDataTable();

                            schemaTable = tempDataTable;
                            totalSheet = schemaTable.Rows.Count;
                            using (OleDbCommand command = new OleDbCommand())
                            {
                                command.Connection = connection;
                                command.CommandText = "select * from [Sheet1$]";

                                using (OleDbDataReader reader = command.ExecuteReader())
                                {
                                    dataTable.Load(reader);
                                }

                                foreach (DataRow dt_product in dataTable.Rows)
                                {
                                    string exproducttype = dt_product["PRODUCT TYPE"].ToString();
                                    string exproductgroup = dt_product["PRODUCT GROUP"].ToString();
                                    string exproductcode = dt_product["PRODUCT CODE"].ToString();
                                    string exproducthsncode = dt_product["HSN CODE"].ToString();
                                    string exproduct = dt_product["PRODUCT"].ToString();
                                    string exproductdescription = dt_product["PRODUCT DESCRIPTION"].ToString();
                                    string exproductunit = dt_product["UNITS"].ToString();
                                    string exproductserialno = dt_product["SERIAL NUMBER TRACKER"].ToString();
                                    string exproductwarrenty = dt_product["WARRANTY TRACKER"].ToString();
                                    string exproductExpirydate = dt_product["EXPIRY DATE TRACKER"].ToString();
                                    string exproductcostprice = dt_product["COST PRICE"].ToString();
                                    string lsstatus = "1", lsserialflag = "", lsserialtracking_flag = "", lsstockflag = "";
                                    msSQL = "Select producttype_gid from pmr_mst_tproducttype " +
                                            "where producttype_name = '" + exproducttype + "'";
                                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                                    if (objMySqlDataReader.HasRows == true)
                                    {
                                        objMySqlDataReader.Read();
                                        exclproducttypecode = objMySqlDataReader["producttype_gid"].ToString();
                                        objMySqlDataReader.Close();
                                    }
                                    msSQL = " Select productgroup_gid from pmr_mst_tproductgroup " +
                                    " where productgroup_name = '" + exproductgroup + "'";
                                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                                    if (objMySqlDataReader.HasRows == true)
                                    {
                                        objMySqlDataReader.Read();
                                        mcGetGID = objMySqlDataReader["productgroup_gid"].ToString();
                                        objMySqlDataReader.Close();
                                    }
                                    else
                                    {
                                        mcGetGID = objcmnfunctions.GetMasterGID("PPGM");
                                    }
                                    msSQL = "insert into pmr_mst_tproductgroup (" +
                                                    "productgroup_gid, " +
                                                    "productgroup_code, " +
                                                    "productgroup_name) " +
                                                    "values (" +
                                                    "'" + mcGetGID + "', " +
                                                    "'" + exproductgroup + "', " +
                                                    "'" + exproductgroup + "')";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                   
                                    {
                                        msSQL = " Select productuomclass_gid from pmr_mst_tproductuomclass " +
                                                " where productuomclass_name = '" + exproductunit + "'";
                                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                                        if (objMySqlDataReader.HasRows == true)
                                        {
                                            objMySqlDataReader.Read();
                                            maGetGID = objMySqlDataReader["productuomclass_gid"].ToString();
                                            objMySqlDataReader.Close();
                                        }
                                        else
                                        {
                                            maGetGID = objcmnfunctions.GetMasterGID("PUCM");
                                        }
                                        msSQL = "insert into pmr_mst_tproductuomclass (" +
                                                "productuomclass_gid, " +
                                                "productuomclass_code, " +
                                                "productuomclass_name) " +
                                                "values(" +
                                                "'" + maGetGID + "', " +
                                                "'" + exproductunit + "', " +
                                                "'" + exproductunit + "')";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        
                                        {
                                            msSQL = " Select productuom_gid from pmr_mst_tproductuom " +
                                                    " where productuom_name = '" + exproductunit + "'";
                                            objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                                            if (objMySqlDataReader.HasRows == true)
                                            {
                                                objMySqlDataReader.Read();
                                                msGetGID = objMySqlDataReader["productuom_gid"].ToString();
                                                objMySqlDataReader.Close();
                                            }
                                            else
                                            {
                                                msGetGID = objcmnfunctions.GetMasterGID("PPMM");
                                            }
                                            msSQL = "insert into pmr_mst_tproductuom (" +
                                                    "productuom_gid, " +
                                                    "productuom_code, " +
                                                    "productuom_name, " +
                                                    "productuomclass_gid) " +
                                                    "values(" +
                                                    "'" + msGetGID + "', " +
                                                    "'" + exproductunit + "', " +
                                                    "'" + exproductunit + "', " +
                                                    "'" + maGetGID + "')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            
                                            {
                                                msSQL = " Select product_gid from pmr_mst_tproduct " +
                                                        " where product_code =  '" + exproductcode + "' and product_name = '" + exproduct + "'";
                                                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                                                if (objMySqlDataReader.HasRows == true)
                                                {
                                                    objMySqlDataReader.Close();
                                                }
                                                else
                                                {
                                                    mrGetGID = objcmnfunctions.GetMasterGID("PPTM");
                                                }

                                                msSQL = "insert into pmr_mst_tproduct (" +
                                                      "product_gid, " +
                                                      "productgroup_gid, " +
                                                                "productuom_gid, " +
                                                               "productuomclass_gid, " +
                                                                "product_code, " +
                                                                "product_name, " +
                                                                "producttype_gid, " +
                                                                "status, " +
                                                                "cost_price, "+
                                                                        "serial_flag, " +
                                                                         "serialtracking_flag, " +
                                                                         "warrentytracking_flag, " +
                                                                         "expirytracking_flag, " +
                                                                        " created_by, " +
                                                                         " created_date) " +
                                                                         "values (" +
                                                                       "'" + mrGetGID + "', " +
                                                                         "'" + mcGetGID + "', " +
                                                                         "'" + msGetGID + "'," +
                                                                         "'" + maGetGID + "'," +
                                                                        "'" + exproductcode + "', " +
                                                                         "'" + exproduct + "', " +
                                                                         "'" + exclproducttypecode + "', " +
                                                                        "'" + lsstatus + "', " +
                                                                        "'" + exproductcostprice + "', " +
                                                                         "'" + lsserialflag + "', " +
                                                                         "'" + lsserialtracking_flag + "', " +
                                                                        "'" + exproductwarrenty + "', " +
                                                                         "'" + exproductwarrenty + "', " +
                                                                         "'" + user_gid + "'," +
                                                                         "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    productresult.status = false;
                    productresult.message = ex.ToString();
                    return;
                }
            }
            catch (Exception ex)
            {
                productresult.status = false;
                productresult.message = ex.ToString();
            }
            if (mnResult != 0)
            {
                values.status = true;
                values.message = "Product Added Successfully";
            }
            else
            {
                values.status = false;
                values.message = "Please Enter All Mandatory Fields";
            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Importing Product !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }


        // Product Active - Inactive
        public void DaGetcustomerInactive(string product_gid, MdlSmrTrnCustomerSummary values)
        {
            try {
                
                msSQL = " update pmr_mst_tproduct set" +
                        " status='Inactive'" +
                        " where product_gid = '" + product_gid + "'";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult != 0)
            {
                values.status = true;
                values.message = "Product Inactivated Successfully";
            }
            else
            {
                {
                    values.status = false;
                    values.message = "Error While Customer Inactivated";
                }
            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while   Updating Customer Inactivated !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }

        public void DaGetcustomerActive(string product_gid, MdlSmrTrnCustomerSummary values)
        {
            try {
               
                msSQL = " update pmr_mst_tproduct set" +
                        " status='Active'" +
                        " where product_gid = '" + product_gid + "'";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult != 0)
            {
                values.status = true;
                values.message = "Product Activated Successfully";
            }
            else
            {
                {
                    values.status = false;
                    values.message = "Error While Customer Activated";
                }
            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while  Updating Customer Activated!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        public void DaGetProductReportExport(MdlSmrMstProduct values)
        {
            try {
               
                msSQL = " SELECT b.productgroup_name as ProductGroupName, CONCAT_WS('|',a.product_name,a.size, a.width, a.length) as ProductName,a.product_code as ProductCode, " +
                    " e.productuom_name as Unit,d.producttype_name as ProductType   from pmr_mst_tproduct a " +
                    " left join pmr_mst_tproductgroup b on a.productgroup_gid = b.productgroup_gid " +
                    " left join pmr_mst_tproductuomclass c on a.productuomclass_gid = c.productuomclass_gid " +
                    " left join pmr_mst_tproducttype d on a.producttype_gid = d.producttype_gid " +
                    " left join pmr_mst_tproductuom e on a.productuom_gid = e.productuom_gid " +
                    " left join adm_mst_tuser f on f.user_gid=a.created_by order by a.created_date desc";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            string lscompany_code = string.Empty;
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Product Report");
            try
            {
                msSQL = " select company_code from adm_mst_tcompany";

                lscompany_code = objdbconn.GetExecuteScalar(msSQL);
                string lspath = ConfigurationManager.AppSettings["exportexcelfile"] + "/prodcut/export" + "/" + lscompany_code + "/" + "Export/" + DateTime.Now.Year + "/" + DateTime.Now.Month;
                //values.lspath = ConfigurationManager.AppSettings["file_path"] + "/erp_documents" + "/" + lscompany_code + "/" + "SDC/TestReport/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                {
                    if ((!System.IO.Directory.Exists(lspath)))
                        System.IO.Directory.CreateDirectory(lspath);
                }

                string lsname2 = "Product_Report" + DateTime.Now.ToString("(dd-MM-yyyy HH-mm-ss)") + ".xlsx";
                string lspath1 = ConfigurationManager.AppSettings["exportexcelfile"] + "/prodcut/export" + "/" + lscompany_code + "/" + "Export/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + lsname2;

                workSheet.Cells[1, 1].LoadFromDataTable(dt_datatable, true);
                FileInfo file = new FileInfo(lspath1);
                using (var range = workSheet.Cells[1, 1, 1, 8])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                    range.Style.Font.Color.SetColor(Color.White);
                }
                excel.SaveAs(file);

                var getModuleList = new List<productexport_list>();
                if (dt_datatable.Rows.Count != 0)
                {

                    getModuleList.Add(new productexport_list
                    {
                        lsname2 = lsname2,
                        lspath1 = lspath1,
                    });
                    values.productexport_list = getModuleList;

                }
                dt_datatable.Dispose();
                values.status = true;
                values.message = "Success";
            }
            catch (Exception ex)
            {
                values.status = false;
                values.message = "Failure";
            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Exporting Prodcut  !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }

    }
}