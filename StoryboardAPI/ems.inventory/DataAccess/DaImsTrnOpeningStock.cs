﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ems.inventory.Models;
using ems.utilities.Functions;
using System.Data.Odbc;
using System.Data;
//using System.Web;
//using OfficeOpenXml;
using System.Configuration;
using System.IO;
//using OfficeOpenXml.Style;
using System.Drawing;
using System.Net.Mail;
using static System.Net.Mime.MediaTypeNames;
using System.Web.UI.WebControls;
using System.Text;
using OfficeOpenXml.Style;
using static OfficeOpenXml.ExcelErrorValue;
using System.Runtime.Remoting;
using MySql.Data.MySqlClient;

namespace ems.inventory.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class DaImsTrnOpeningStock
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        MySqlDataReader objMySqlDataReader;
        DataTable dt_datatable, objTbl;
        string msGetGid, lsbranch_gid, lsqtyrequested, msGetStockGID, lsproduct_price, lscustomer_gid;
        int mnResult, mnResult1;
        string issued_qty;
        int qtyrequested, finalQty;
        public void DaGetImsTrnOpeningstockSummary(MdlImsTrnOpeningStock values)
        {
            try
            {
              
                msSQL = " SELECT a.product_gid,d.stock_gid, a.product_code, a.product_name,  b.productgroup_name, c.productuom_name," +
                " d.stock_qty as opening_stock,g.branch_name,date_format(d.created_date,'%d-%m-%Y') as created_date,d.issued_qty," +
                " e.producttype_name as product_type,d.uom_gid,d.branch_gid,d.display_field,h.location_name " +
                " FROM ims_trn_tstock d " +
                " left join pmr_mst_tproduct a on d.product_gid = a.product_gid" +
                " left join pmr_mst_tproductgroup b on a.productgroup_gid = b.productgroup_gid" +
                " left join pmr_mst_tproductuom c on d.uom_gid = c.productuom_gid" +
                " left join pmr_mst_tproducttype e on a.producttype_gid = e.producttype_gid" +
                " left join ims_mst_tstocktype f on d.stocktype_gid = f.stocktype_gid" +
                " left join hrm_mst_tbranch g on d.branch_gid = g.branch_gid " +
                " left join ims_mst_tlocation h on d.location_gid=h.location_gid" +
                " order by created_date desc";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<stock_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new stock_list
                    {

                        created_date = dt["created_date"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        location_name = dt["location_name"].ToString(),
                        productgroup_name = dt["productgroup_name"].ToString(),
                        product_code = dt["product_code"].ToString(),
                        product_name = dt["product_name"].ToString(),
                        productuom_name = dt["productuom_name"].ToString(),
                        opening_stock = dt["opening_stock"].ToString(),
                        stock_gid = dt["stock_gid"].ToString(),
                        product_gid = dt["product_gid"].ToString(),
                        issued_qty = dt["issued_qty"].ToString(),

                    });
                    values.stock_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Open Stock Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }

        public void DaGetImsTrnOpeningstockAdd(MdlImsTrnOpeningStock values)
        {
            try
            {
                msSQL = " SELECT a.product_gid, a.product_code, a.product_name, a.product_desc, b.productgroup_name, c.productuom_name, a.created_date," +
                    " a.productuom_gid,a.productgroup_gid FROM pmr_mst_tproduct a" +
                    " left join pmr_mst_tproductgroup b on a.productgroup_gid = b.productgroup_gid" +
                    " left join pmr_mst_tproductuom c on a.productuom_gid = c.productuom_gid" +
                    " order by created_date desc";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<stockadd_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new stockadd_list
                    {

                        product_gid = dt["product_gid"].ToString(),
                        productgroup_gid = dt["productgroup_gid"].ToString(),
                        productuom_gid = dt["productuom_gid"].ToString(),
                        productgroup_name = dt["productgroup_name"].ToString(),
                        product_code = dt["product_code"].ToString(),
                        product_name = dt["product_name"].ToString(),
                        productuom_name = dt["productuom_name"].ToString(),
                        product_desc = dt["product_desc"].ToString(),


                    });
                    values.stockadd_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Adding Opening Stock !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        public void DaGetOnChangeLocation(MdlImsTrnOpeningStock values)
        {
            try
            {
                

                msSQL = " select location_gid,location_name,location_code " +
                " from  ims_mst_tlocation ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetLocation>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetLocation
                    {
                        location_name = dt["location_name"].ToString(),
                        location_gid = dt["location_gid"].ToString(),

                    });
                    values.GetLocation = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Location !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           

        }

        public void DaPostOpeningstock(string user_gid, Postopeningstock values)
        {
            try
            {
               
                msSQL = " SELECT product_gid,stock_qty FROM ims_trn_tstock " +
                " where product_gid = '" + values.product_gid + "' and" +
                " uom_gid = '" + values.uom_gid + "' and" +
                " stocktype_gid = 'SY0905270001' and" +
                " branch_gid = '" + values.branch_gid + "' and" +
                " display_field='" + values.display_field + "' and " +
                " location_gid='" + values.location_gid + "'";

            objMySqlDataReader = objdbconn.GetDataReader(msSQL);
            dt_datatable = objdbconn.GetDataTable(msSQL);
            if (objMySqlDataReader.HasRows == false)
            {
                objMySqlDataReader.Close();
            }

            if (dt_datatable.Rows.Count > 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    string lsstockQty = dt["stock_qty"].ToString();
                    int stockQty = int.Parse(lsstockQty);
                    string OpeningstockQty = values.stock_qty;
                    int Openingstock = int.Parse(OpeningstockQty);
                    qtyrequested = stockQty + Openingstock;
                }

                if (qtyrequested != 0)
                    finalQty = qtyrequested;
                else
                    finalQty = int.Parse(values.stock_qty);
                msSQL = " UPDATE ims_trn_tstock " +
                " SET stock_qty = '" + finalQty + "'," +
                " display_field='" + values.display_field + "', " +
                " created_by='" + user_gid + "'" +
                " WHERE product_gid = '" + values.product_gid + "' and " +
                " uom_gid= '" + values.uom_gid + "' and " +
                " branch_gid = '" + values.branch_gid + "' and " +
                " stocktype_gid = 'SY0905270001' and location_gid='" + values.location_gid + "'";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 0)
                {

                    values.status = false;
                    values.message = "Error Occured while adding Stock";
                }
                else
                {
                    values.status = true;
                    values.message = "Opening Stock Updated Successfully";
                }

            }
            else
            {
                msGetStockGID = objcmnfunctions.GetMasterGID("ISKP");
                if (msGetStockGID == "E")
                {
                    values.status = true;
                    values.message = "Create sequence code ISKP for temp sales enquiry";
                }


                msSQL = " Insert into ims_trn_tstock (" +
                                " stock_gid," +
                                " branch_gid, " +
                                " location_gid, " +
                                " product_gid," +
                                " display_field," +
                                " uom_gid," +
                                " stock_qty," +
                                " created_by," +
                                " created_date," +
                                " stocktype_gid, " +
                                " unit_price, " +
                                " stock_flag," +
                                " grn_qty," +
                                " rejected_qty," +
                                " issued_qty," +
                                " amend_qty," +
                                " damaged_qty," +
                                " adjusted_qty," +
                                " reference_gid," +
                                " remarks" +
                                " )values ( " +
                                " '" + msGetStockGID + "', " +
                                " '" + values.branch_gid + "'," +
                                " '" + values.location_gid + "'," +
                                " '" + values.product_gid + "'," +
                                " '" + values.display_field + "'," +
                                " '" + values.uom_gid + "'," +
                                " '" + values.stock_qty + "'," +
                                " '" + user_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                " 'SY0905270001'," +
                                 " '" + values.unit_price + "'," +
                                "'Y'," +
                                "'0.00'," +
                                "'0.00'," +
                                "'0.00'," +
                                "'0.00'," +
                                "'0.00'," +
                                "'0.00'," +
                                "'" + msGetStockGID + "'," +
                                "'From Opening Stock')";


                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {

                    values.status = true;
                    values.message = "Opening Stock Inserted Successfully";

                }
                else
                {
                    values.status = false;
                    values.message = "Opening Stock Inserted Failed";
                }

                //msSQL = " select issued_qty from ims_trn_tstock " +
                //    " WHERE product_gid = '" + values.product_gid + "' and " +
                //    " uom_gid= '" + values.uom_gid + "' and " +
                //    " branch_gid = '" + values.branch_gid + "' and " +
                //    " stocktype_gid = 'SY0905270001' and" +
                //    " display_field='" + values.display_field + "' and" +
                //    " location_gid='" + values.location_gid + "'";
                //objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                //if (objMySqlDataReader.HasRows == true)
                //{
                //    objMySqlDataReader.Read();

                //    foreach (DataRow dt in dt_datatable.Rows)
                //    {
                //         issued_qty = dt["issued_qty"].ToString();
                //    }
                //    if (issued_qty=="0.00")
                //    {
                //        objMySqlDataReader.Close();

                //    }
                //}

            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Submiting Opening Stock !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        public void DaGetEditOpeningStockSummary(string stock_gid, MdlImsTrnOpeningStock values)
        {
            try
            {
                
                msSQL = " SELECT a.product_gid, b.product_code, a.stock_gid,b.product_name,f.stocktype_name,b.status,a.display_field,g.location_name,g.location_gid," +
                    " c.productgroup_name,d.productuom_name,a.uom_gid,b.productgroup_gid," +
                    " (a.stock_qty) as opening_stock, a.unit_price as unit_price," +
                    " e.branch_name,a.branch_gid,(a.stock_qty * a.unit_price) as total_price" +
                    " from ims_trn_tstock a" +
                    " left join pmr_mst_tproduct b on a.product_gid=b.product_gid" +
                    " left join pmr_mst_tproductgroup c on b.productgroup_gid=c.productgroup_gid" +
                    " left join pmr_mst_tproductuom d on a.uom_gid=d.productuom_gid" +
                    " left join hrm_mst_tbranch e on a.branch_gid=e.branch_gid" +
                    " left join ims_mst_tstocktype f on a.stocktype_gid=f.stocktype_gid" +
                    " left join ims_mst_tlocation g on a.location_gid=g.location_gid" +
                    " where a.stock_gid='" + stock_gid + "' ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetEditOpeningStock>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetEditOpeningStock
                    {


                        product_gid = dt["product_gid"].ToString(),
                        product_code = dt["product_code"].ToString(),
                        product_name = dt["product_name"].ToString(),
                        location_name = dt["location_name"].ToString(),
                        location_gid = dt["location_gid"].ToString(),
                        productgroup_name = dt["productgroup_name"].ToString(),
                        productuom_name = dt["productuom_name"].ToString(),
                        productuom_gid = dt["uom_gid"].ToString(),
                        cost_price = dt["unit_price"].ToString(),
                        productgroup_gid = dt["productgroup_gid"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        branch_gid = dt["branch_gid"].ToString(),
                        product_desc = dt["display_field"].ToString(),
                        opening_stock = dt["opening_stock"].ToString(),
                        stock_gid = dt["stock_gid"].ToString(),
                        product_status = dt["status"].ToString(),
                        





                    });
                    values.GetEditOpeningStock = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Opening Stock !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
          
        }

        public void DaPostOpeningStockUpdate(string user_gid, stockedit_list values)
        {
            try
            {
                msGetStockGID = objcmnfunctions.GetMasterGID("IOSP");
            if (msGetStockGID == "E")
            {
                values.status = true;
                values.message = "Create sequence code IOSP for temp sales enquiry";
            }
            msSQL = " SELECT producttype_gid  FROM pmr_mst_tproduct  WHERE product_gid='" + values.product_gid + "' ";
            string lsproducttype_gid = objdbconn.GetExecuteScalar(msSQL);
         

            msSQL = " INSERT INTO ims_trn_topeningstockedit" +
                            " (stockedit_gid," +
                            " branch_gid," +
                            " product_gid," +
                            " stockedit_quantity," +
                            " producttype_gid," +
                            " uom_gid," +
                            " display_field," +
                            " reference_gid," +
                            " product_status," +
                            " created_by," +
                            " created_date)" +
                            " values (" +
                            " '" + msGetStockGID + "', " +
                            " '" + values.branch_gid + "'," +
                            " '" + values.product_gid + "'," +
                            " '" + values.stock_qty + "'," +
                            " '" + lsproducttype_gid + "'," +
                            " '" + values.productuom_gid + "'," +
                            " '" + values.product_desc + "'," +
                            " '" + values.stock_gid + "'," +
                            " '" + values.product_status + "'," +
                            " '" + user_gid + "'," +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";


            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            //if (mnResult == 1)
            //{

            //    values.status = true;
            //    values.message = "Opening Stock Inserted Successfully";

            //}
            //else
            //{
            //    values.status = false;
            //    values.message = "Opening Stock Inserted Failed";
            //}


            msSQL = " UPDATE ims_trn_tstock " +
                " SET stock_qty = '" + values.stock_qty + "'," +
                " display_field='" + values.product_desc + "'," +
                " unit_price='" + values.cost_price + "'" +
                " WHERE stock_gid ='" + values.stock_gid + "'" ;

            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult != 0)
            {

                values.status = true;
                values.message = "Stock Updated Successfully";

            }
            else
            {
                values.status = false;
                values.message = "Error While Updating Stock";
            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Opening Stock !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            




        }

        public void DaGetOnChangeproductName(string product_gid, MdlImsTrnOpeningStock values)
        {
            try
            {
                objdbconn.OpenConn();
                msSQL = " Select a.product_name, a.product_code,a.mrp_price as cost_price," +
                        " b.productuom_gid,b.productuom_name,c.productgroup_name,c.productgroup_gid,a.productuom_gid  from pmr_mst_tproduct a  " +
                         " left join pmr_mst_tproductuom b on a.productuom_gid = b.productuom_gid  " +
                        " left join pmr_mst_tproductgroup c on a.productgroup_gid = c.productgroup_gid " +
                    " where a.product_gid='" + product_gid + "' ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    var getModuleList = new List<GetproductsCode>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetproductsCode
                            {
                                product_name = dt["product_name"].ToString(),
                                product_code = dt["product_code"].ToString(),
                                productuom_name = dt["productuom_name"].ToString(),
                                productgroup_name = dt["productgroup_name"].ToString(),
                                productuom_gid = dt["productuom_gid"].ToString(),
                                productgroup_gid = dt["productgroup_gid"].ToString(),
                                unitprice = dt["cost_price"].ToString(),

                            });
                            values.ProductsCode = getModuleList;
                        }
                    }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Product Name !";
            }
          
        }


    }
}