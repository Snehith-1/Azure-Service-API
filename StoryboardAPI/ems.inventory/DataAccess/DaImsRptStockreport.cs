﻿using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.inventory.Models;
using OfficeOpenXml.FormulaParsing.Excel.Operators;
using MySql.Data.MySqlClient;

namespace ems.inventory.DataAccess
{
    public class DaImsRptStockreport
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        MySqlDataReader objMySqlDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsuom_gid, lsbranch_gid, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;

        public void DaGetImsRptStockreport(string branch_gid ,MdlImsRptStockreport values)

        {
            try
            {                
                msSQL = " SELECT distinct a.product_gid,b.productgroup_name,a.branch_gid,j.branch_name, d.product_code, d.product_name," +
                "c.productuom_name, round(sum(a.stock_qty+a.amend_qty-a.damaged_qty-a.issued_qty-a.transfer_qty),2)" +
                " as Stock_Balance, a.unit_price as Product_Price ,i.bin_number,a.display_field, " +
                "format(((sum(a.stock_qty+a.amend_qty-a.damaged_qty-a.issued_qty-a.transfer_qty)) * a.unit_price),2) as Stock_Value," +
                " h.location_name FROM  ims_trn_tstock a left join " +
                "pmr_mst_tproduct d on a.product_gid = d.product_gid left join pmr_mst_tproductgroup b on d.productgroup_gid=b.productgroup_gid" +
                " left join pmr_mst_tproductuom c on a.uom_gid = c.productuom_gid " +
                "left join ims_mst_tstocktype f on f.stocktype_gid = a.stocktype_gid " +
                "left join ims_mst_tlocation h on h.location_gid=a.location_gid " +
                "left join ims_mst_tbin i on a.bin_gid=i.bin_gid " +
               " left join hrm_mst_tbranch j on j.branch_gid = a.branch_gid " +
                " WHERE 0 = 0 and a.branch_gid = '" + branch_gid + "' and a.stock_flag = 'Y' " +
                " group by d.product_gid,d.productuom_gid,a.branch_gid Order by d.product_name asc "; 

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<stockreport_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new stockreport_list
                    {

                        bin_number = dt["bin_number"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        location_name = dt["location_name"].ToString(),
                        product_code = dt["product_code"].ToString(),
                        product_name = dt["product_name"].ToString(),
                        productuom_name = dt["productuom_name"].ToString(),
                        stock_balance = dt["stock_balance"].ToString(),
                        stock_value = dt["stock_value"].ToString(),
                        product_price = dt["product_price"].ToString(),
                        display_field = dt["display_field"].ToString(),


                    });
                    values.stockreport_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Stock Report!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +  ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }


        public void DaGetBranch(MdlImsRptStockreport values)
        {
            try
            {
                objdbconn.OpenConn();
                msSQL = " Select branch_name,branch_gid " +
                    " from hrm_mst_tbranch ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<branch_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new branch_list
                    {
                        branch_name = dt["branch_name"].ToString(),
                        branch_gid = dt["branch_gid"].ToString(),
                    });
                    values.branch_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Branch!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +  ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }

    }
}