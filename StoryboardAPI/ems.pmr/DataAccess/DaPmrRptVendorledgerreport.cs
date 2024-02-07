﻿using ems.pmr.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace ems.pmr.DataAccess
{
    public class DaPmrRptVendorledgerreport
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        MySqlDataReader objMySqlDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, maGetGID, lsvendor_code, msUserGid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;

        public void DaGetVendorledgerReportSummary(MdlPmrRptVendorledgerreport values)
        {
            try
            {
                 
                msSQL = "select  a.purchaseorder_gid,a.vendor_gid,c.vendor_companyname, c.contactperson_name,c.vendor_code,concat(f.address1,f.address2) as vendor_address, " +
                " concat(c.contactperson_name,'/',c.contact_telephonenumber,'/',c.email_id) as details, " +
                " group_concat(distinct e.product_name,' ') as product_name, " +
                " sum(b.product_price) as amount, " +
                " (select case when x.total_amount_L='0.00' then format(sum(x.total_amount),2) " +
                " when x.total_amount_L<>'0.00' then format(sum(x.total_amount_L),2) end as total " +
                " from pmr_trn_tpurchaseorder x " +
                " where c.vendor_gid=x.vendor_gid) as total  from pmr_trn_tpurchaseorder a " +
                " left join pmr_trn_tpurchaseorderdtl b on a.purchaseorder_gid=b.purchaseorder_gid " +
                " left join acp_mst_tvendor c on a.vendor_gid=c.vendor_gid  " +
                " left join pmr_mst_tproduct e on b.product_gid=e.product_gid " +
                " left join adm_mst_taddress f on c.address_gid = f.address_gid " +
                " where  a.purchaseorder_status not in ('Approve Pending','PO Amended','Rejected') " +
                " group by a.vendor_gid";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<vendorledger_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new vendorledger_list
                        {

                            vendor_refno = dt["vendor_gid"].ToString(),
                            vendor = dt["vendor_companyname"].ToString(),
                            vendor_code = dt["vendor_code"].ToString(),
                            vendor_address = dt["vendor_address"].ToString(),
                            contact_details = dt["details"].ToString(),
                            products = dt["product_name"].ToString(),
                            order_value = dt["amount"].ToString()



                        });
                        values.vendorledger_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Purchaseorder detailed report Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                    ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                    msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                    DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }
    }

}