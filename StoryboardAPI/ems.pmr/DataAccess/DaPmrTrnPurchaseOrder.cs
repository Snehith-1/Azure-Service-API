﻿using ems.utilities.Functions;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Web;
using System;
using System.Net.Mail;
using System.IO;
using System.Linq;
using System.Net;
using System.Configuration;
using System.Globalization;
using Newtonsoft.Json;
using ems.pmr.Models;
using MySql.Data.MySqlClient;




namespace ems.pmr.DataAccess
{
    public class DaPmrTrnPurchaseOrder
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string lspop_server, lspop_mail, lspop_password, lscompany, lscompany_code;
        string msINGetGID, msGet_att_Gid, msenquiryloggid;
        string lspath, lspath1, lspath2, mail_path, mail_filepath, pdf_name = "";
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        MySqlDataReader objMySqlDataReader;
        DataTable mail_datatable, dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msPOGetGID, msPO1GetGID, msGetGID, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5, lspop_port;
        MailMessage message = new MailMessage();

        public void DaGetPurchaseOrderSummary(MdlPmrTrnPurchaseOrder values)
        {
            try
            {
                

                msSQL = " select /*+ MAX_EXECUTION_TIME(600000) */ distinct h.costcenter_name,a.purchaseorder_gid, " +
                " if(a.manualporef_no is null,a.purchaseorder_gid, " +
                " if(a.manualporef_no='',a.purchaseorder_gid, " +
                " if(a.manualporef_no=a.purchaseorder_gid,a.purchaseorder_gid,concat(a.purchaseorder_gid,'/',a.manualporef_no)))) " +
                " as porefno, " +
                " a.purchaseorder_remarks,a.purchaseorder_status, format(a.total_amount,2) as total_amount,Date_add(a.purchaseorder_date,Interval delivery_days day) as ExpectedPODeliveryDate ," +
                " format(a.total_amount,2) as paymentamount, " +
                " DATE_FORMAT(a.purchaseorder_date , '%d-%b-%Y') as purchaseorder_date ,a.vendor_status," +
                " CASE when a.invoice_flag <> 'IV Pending' then a.invoice_flag" +
                " when a.grn_flag <> 'GRN Pending' then a.grn_flag" +
                " else a.purchaseorder_flag end as 'overall_status'," +
                " a.purchaseorder_flag, a.grn_flag, a.invoice_flag," +
                " (case when k.currency_code is null then b.vendor_companyname" +
                " when k.currency_code is not null then" +
                " concat(b.vendor_companyname,'/',k.currency_code) end)  as vendor_companyname, c.branch_name, " +
                " case when group_concat(distinct purchaserequisition_referencenumber)=',' then '' " +
                " when group_concat(distinct purchaserequisition_referencenumber) <> ',' then group_concat(distinct purchaserequisition_referencenumber) end  as refrence_no, " +
                " bscc_flag,po_type from pmr_trn_tpurchaseorder a" +
                " left join  acp_mst_tvendor b on b.vendor_gid = a.vendor_gid" +
                " left join hrm_mst_tbranch c on c.branch_gid = a.branch_gid" +
                " left join adm_mst_tuser d on d.user_gid = a.created_by" +
                " left join hrm_mst_temployee e on e.user_gid = d.user_gid" +
                " left join adm_mst_tmodule2employee  f on f.employee_gid = e.employee_gid" +
                " left join pmr_mst_tcostcenter h on h.costcenter_gid=a.costcenter_gid" +
                " left join pmr_Trn_tpr2po i on i.purchaseorder_gid=a.purchaseorder_gid" +
                " left join pmr_Trn_tpurchaserequisition j on j.purchaserequisition_gid=i.purchaserequisition_gid " +
                " left join crm_trn_tcurrencyexchange k on k.currencyexchange_gid =a.currency_code" +
                " where 1=1 and a.workorderpo_flag='N'  and a.purchaseorder_status <>'PO Amended'group by a.purchaseorder_gid  ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetPurchaseOrder_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetPurchaseOrder_lists
                        {

                            purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                            purchaseorder_date = dt["purchaseorder_date"].ToString(),
                            ExpectedPODeliveryDate = dt["ExpectedPODeliveryDate"].ToString(),
                            porefno = dt["porefno"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            costcenter_name = dt["costcenter_name"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            remarks = dt["purchaseorder_remarks"].ToString(),
                            total_amount = dt["total_amount"].ToString(),
                            purchaseorder_status = dt["purchaseorder_status"].ToString(),
                            vendor_status = dt["vendor_status"].ToString(),
                            paymentamount = dt["paymentamount"].ToString(),

                        });
                        values.GetPurchaseOrder_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {

                values.message = "Exception occured while getting PO summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            


        }
        public void DaGetBranch(MdlPmrTrnPurchaseOrder values)
        {
            try
            {
                 
                msSQL = " Select branch_name,branch_gid " +
                " from hrm_mst_tbranch ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetBranch>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetBranch
                        {
                            branch_name = dt["branch_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                        });
                        values.GetBranch = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting branch!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
             
        }
        public void DaGetVendor(MdlPmrTrnPurchaseOrder values)
        {
            try
            {
                 
                msSQL = " Select vendor_companyname,vendor_gid " +
            " from acp_mst_tvendor where blacklist_flag<>'Y'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetVendor>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetVendor
                        {
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                        });
                        values.GetVendor = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting vendor!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }

        public void DaGetDispatchToBranch(MdlPmrTrnPurchaseOrder values)
        {
            try
            {
                
                msSQL = " Select branch_name,branch_gid " +
        " from hrm_mst_tbranch ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetDispatchToBranch>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetDispatchToBranch
                        {
                            dispatch_name = dt["branch_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                        });
                        values.GetDispatchToBranch = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting despatch to branch!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        public void DaGetCurrency(MdlPmrTrnPurchaseOrder values)
        {
            try
            {
                 
                msSQL = " select distinct a.currencyexchange_gid,a.currency_code from  crm_trn_tcurrencyexchange a " +
   " left join acp_mst_tvendor b on a.currencyexchange_gid = b.currencyexchange_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL, "crm_trn_tcurrencyexchange");

                var getModuleList = new List<GetCurrency>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetCurrency
                        {
                            currency_code = dt["currency_code"].ToString(),
                            currencyexchange_gid = dt["currencyexchange_gid"].ToString(),
                        });
                        values.GetCurrency = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting currecny!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        public void DaGetTax(MdlPmrTrnPurchaseOrder values)
        {
            try
            {
                
                msSQL = " select tax_name,tax_gid,percentage from acp_mst_ttax where active_flag='Y' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetTax>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetTax
                        {
                            tax_gid = dt["tax_gid"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_name1 = dt["tax_name"].ToString(),
                            tax_name2 = dt["tax_name"].ToString(),
                            tax_name3 = dt["tax_name"].ToString(),
                            percentage = dt["percentage"].ToString(),
                        });
                        values.GetTax = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting PO !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
             
        }

        public void DaGetProductCode(MdlPmrTrnPurchaseOrder values)
        {
            try
            {
                

                msSQL = " Select product_code,product_gid,product_name" +
" from pmr_mst_tproduct ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProductCode>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProductCode
                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                        });
                        values.GetProductCode = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Product code!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
             

        }
        public void DaGetViewPurchaseOrderSummary(string purchaseorder_gid, MdlPmrTrnPurchaseOrder values)
        {
            try
            {
                
                msSQL = " select a.purchaseorder_gid, a.purchaserequisition_gid,a.purchaseorder_remarks,y.tax_name, " +
" case when a.exchange_rate is null then '1' else a.exchange_rate end as exchange_rate, " +
" a.purchaserequisition_gid, a.quotation_gid, a.branch_gid,a.ship_via,a.payment_terms,a.delivery_location,a.freight_terms, " +
" date_format(a.purchaseorder_date, '%d-%m-%y') as purchaseorder_date , " +
" a.vendor_address,a.shipping_address, a.vendor_contact_person,a.created_by,a.priority,a.priority_remarks, " + " case when a.priority = 'Y' then 'High' else 'Low' end as priority_n, " +
" CASE when a.invoice_flag<> 'IV Pending' then a.invoice_flag " +
" when a.grn_flag<> 'GRN Pending' then a.grn_flag " +
" else a.purchaseorder_flag end as 'overall_status', a.approver_remarks, " +
" a.purchaseorder_reference,format(a.total_amount, 2) as total_amount , " +
" a.vendor_emailid, a.vendor_faxnumber, a.vendor_contactnumber, " +
" a.termsandconditions, a.payment_term, " +
" b.vendor_companyname, g.user_firstname as approved_by, " +
" concat(c.user_firstname, ' - ', e.department_name) as user_firstname, " +
" d.employee_emailid, d.employee_mobileno, f.branch_name," +
" format(a.discount_amount, 2) as discount_amount, format(a.tax_percentage, 2) as tax_percentage, " +
" format(a.addon_amount, 2) as addon_amount,format(a.roundoff, 2) as roundoff, " +
" concat_ws('-', h.costcenter_name, h.costcenter_gid) as costcenter_name,format(h.budget_available, 2) as budget_available,h.costcenter_gid, " +
" a.payment_days,a.tax_gid,a.delivery_days,a.freightcharges,a.buybackorscrap,a.manualporef_no ,z.branch_name as deliverytobranch," +
" format(a.packing_charges, 2) as packing_charges, format(a.insurance_charges, 2) as insurance_charges ," +
" i.purchaseorderdtl_gid, i.product_gid, " +
" i.product_price, i.qty_ordered,i.needby_date," +
" format(i.discount_percentage, 2) as discount_percentage , " +
" format(i.discount_amount, 2) as discount_amount , " +
" format(i.tax_percentage, 2) as tax_percentage, " +
" format(i.tax_percentage2, 2) as tax_percentage2, " +
" format(i.tax_percentage3, 2) as tax_percentage3, " +
" format(i.tax_amount, 2) as tax_amount, " +
" format(i.tax_amount2, 2) as tax_amount2, " +
" format(i.tax_amount3, 2) as tax_amount3, " +
" i.qty_Received, i.qty_grnadjusted, " +
" i.product_remarks, format((qty_ordered * i.product_price), 2) as product_totalprice, " +
" format((((qty_ordered * i.product_price) - i.discount_amount) + i.tax_amount + i.tax_amount2 + i.tax_amount3), 2) " +
" as product_total, i.product_code, (i.product_name) as product_name," +
" k.productgroup_name, i.productuom_name,i.purchaseorder_gid,i.display_field_name, m.currency_code,a.poref_no,i.tax1_gid,y.tax_name, " +
"(SELECT SUM(tax_amount) FROM pmr_trn_tpurchaseorderdtl where purchaseorder_gid = '" + purchaseorder_gid + "') AS overall_tax" +
" from pmr_trn_tpurchaseorder a " +
" left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid " +
" left join adm_mst_tuser c on c.user_gid = a.created_by " +
" left join hrm_mst_temployee d on d.user_gid = c.user_gid " +
" left join hrm_mst_tdepartment e on e.department_gid = d.department_gid " +
" left join hrm_mst_tbranch f on d.branch_gid = f.branch_gid " +
" left join hrm_mst_tbranch z on a.deliverytobranch_gid = z.branch_gid " +
" left join adm_mst_tuser g on g.user_gid = a.approved_by " +
" left join pmr_mst_tcostcenter h on h.costcenter_gid = a.costcenter_gid " +
" left join pmr_trn_tpurchaseorderdtl i ON a.purchaseorder_gid = i.purchaseorder_gid " +
" left join acp_mst_ttax y on y.tax_gid = i.tax1_gid " +
" left join pmr_mst_tproduct j on i.product_gid = j.product_gid " +
" left join pmr_mst_tproductgroup k on j.productgroup_gid = k.productgroup_gid " +
" left join pmr_mst_tproductuom l on i.uom_gid = l.productuom_gid " +
" left join crm_trn_tcurrencyexchange m on m.currencyexchange_gid = a.currency_code   " +
" where a.purchaseorder_gid = '" + purchaseorder_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetViewPurchaseOrder>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetViewPurchaseOrder
                        {
                            purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                            manualporef_no = dt["manualporef_no"].ToString(),
                            purchaseorder_date = dt["purchaseorder_date"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            deliverytobranch = dt["deliverytobranch"].ToString(),
                            vendor_contactnumber = dt["vendor_contactnumber"].ToString(),
                            vendor_contact_person = dt["vendor_contact_person"].ToString(),
                            vendor_faxnumber = dt["vendor_faxnumber"].ToString(),
                            vendor_emailid = dt["vendor_emailid"].ToString(),
                            vendor_address = dt["vendor_address"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                            ship_via = dt["ship_via"].ToString(),
                            payment_terms = dt["payment_terms"].ToString(),
                            freight_terms = dt["freight_terms"].ToString(),
                            delivery_location = dt["delivery_location"].ToString(),
                            priority_n = dt["priority_n"].ToString(),
                            priority_remarks = dt["priority_remarks"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            shipping_address = dt["shipping_address"].ToString(),
                            purchaseorder_reference = dt["purchaseorder_reference"].ToString(),
                            purchaseorder_remarks = dt["purchaseorder_remarks"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),
                            tax_gid = dt["tax_gid"].ToString(),
                            packing_charges = dt["packing_charges"].ToString(),
                            insurance_charges = dt["insurance_charges"].ToString(),
                            payment_days = dt["payment_days"].ToString(),
                            addon_amount = dt["addon_amount"].ToString(),
                            buybackorscrap = dt["buybackorscrap"].ToString(),
                            roundoff = dt["roundoff"].ToString(),
                            total_amount = dt["total_amount"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            termsandconditions = dt["termsandconditions"].ToString(),
                            delivery_days = dt["delivery_days"].ToString(),
                            tax_amount2 = dt["tax_amount2"].ToString(),
                            tax_amount3 = dt["tax_amount3"].ToString(),
                            approver_remarks = dt["approver_remarks"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            qty_ordered = dt["qty_ordered"].ToString(),
                            qty_Received = dt["qty_ordered"].ToString(),
                            qty_grnadjusted = dt["qty_ordered"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            po_no = dt["poref_no"].ToString(),
                            tax_percentage = dt["tax_percentage"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            product_total = dt["product_total"].ToString(),
                            overall_tax = dt["overall_tax"].ToString(),



                        });

                        values.GetViewPurchaseOrder = getModuleList;

                    }

                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting PO summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
             
        }

        public void DaGetOnchangeCurrency(string currencyexchange_gid, MdlPmrTrnPurchaseOrder values)
        {
            try
            {
                
                msSQL = " select currencyexchange_gid,currency_code,exchange_rate from crm_trn_tcurrencyexchange " +
" where currencyexchange_gid='" + currencyexchange_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetOnchangeCurrency>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetOnchangeCurrency
                        {

                            exchange_rate = dt["exchange_rate"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                        });
                        values.GetOnchangeCurrency = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while on changing currecy!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
             
        }



        public void DaGetOnChangeBranch(string branch_gid, MdlPmrTrnPurchaseOrder values)
        {
            try
            {
                
                msSQL = " select branch_name,concat(address1,'\n',postal_code) as address1,city,state, postal_code from hrm_mst_tbranch  " +
" where branch_gid  ='" + branch_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetBranch>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetBranch
                        {
                            branch_name = dt["branch_name"].ToString(),
                            address1 = dt["address1"].ToString(),
                            city = dt["city"].ToString(),
                            state = dt["state"].ToString(),
                            postal_code = dt["postal_code"].ToString(),


                        });
                        values.GetBranch = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while gonchanging branch!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
             
        }

        public void DaGetOnChangeVendor(string vendor_gid, MdlPmrTrnPurchaseOrder values)
        {
            try
            {
                
                msSQL = "  select b.address1, b.address2, b.city, b.state, b.postal_code, b.fax,a.contactperson_name, a.vendor_companyname,d.payment_terms, " +
" a.contact_telephonenumber, c.country_name,a.email_id,a.currencyexchange_gid from acp_mst_tvendor a " +
" left join adm_mst_taddress b on b.address_gid = a.address_gid " +
" left join adm_mst_tcountry c on c.country_gid = b.country_gid" +
"  left join acp_mst_tvendor d on d.vendor_gid = a.vendor_gid  " +
" where a.vendor_gid  ='" + vendor_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetVendor>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetVendor
                        {

                            address1 = dt["address1"].ToString(),
                            address2 = dt["address2"].ToString(),
                            city = dt["city"].ToString(),
                            state = dt["state"].ToString(),
                            postal_code = dt["postal_code"].ToString(),
                            fax = dt["fax"].ToString(),
                            contactperson_name = dt["contactperson_name"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                            country_name = dt["country_name"].ToString(),
                            email_id = dt["email_id"].ToString(),
                            currencyexchange_gid = dt["currencyexchange_gid"].ToString(),


                        });
                        values.GetVendor = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while on changing vendor!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
        public void DaGetOnChangeProductCode(string product_code, MdlPmrTrnPurchaseOrder values)
        {
            try
            {
                
                if (product_code != null)
                {
                    msSQL = " Select a.product_name, a.product_code, b.productuom_gid,b.productuom_name,c.productgroup_name  from pmr_mst_tproduct a  " +
                     " left join pmr_mst_tproductuom b on a.productuom_gid = b.productuom_gid  " +
                    " left join pmr_mst_tproductgroup c on a.productgroup_gid = c.productgroup_gid  " +
                " where a.product_code='" + product_code + "' ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    var getModuleList = new List<GetProductCode>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetProductCode
                            {
                                product_name = dt["product_name"].ToString(),
                                product_code = dt["product_code"].ToString(),
                                productuom_name = dt["productuom_name"].ToString(),
                                productgroup_name = dt["productgroup_name"].ToString(),

                            });
                            values.GetProductCode = getModuleList;
                        }
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while  on changing product code!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
             
        }
        public void DaGetOnChangeProductName(string product_gid, MdlPmrTrnPurchaseOrder values)
        {
            try
            {
                

                if (product_gid != null)
                {
                    msSQL = " Select a.product_name, a.product_code, a.cost_price,b.productuom_gid,b.productuom_name,c.productgroup_name,c.productgroup_gid,a.productuom_gid  from pmr_mst_tproduct a  " +
                         " left join pmr_mst_tproductuom b on a.productuom_gid = b.productuom_gid  " +
                        " left join pmr_mst_tproductgroup c on a.productgroup_gid = c.productgroup_gid  " +
                    " where a.product_gid='" + product_gid + "' ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    var getModuleList = new List<GetProductCode>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetProductCode
                            {
                                product_name = dt["product_name"].ToString(),
                                product_code = dt["product_code"].ToString(),
                                productuom_name = dt["productuom_name"].ToString(),
                                productgroup_name = dt["productgroup_name"].ToString(),
                                productuom_gid = dt["productuom_gid"].ToString(),
                                productgroup_gid = dt["productgroup_gid"].ToString(),
                                unitprice = dt["cost_price"].ToString()

                            });
                            values.GetProductCode = getModuleList;
                        }
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while on changing product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }



        public void DaGetProduct(MdlPmrTrnPurchaseOrder values)
        {
            try
            {
                 
                msSQL = " Select product_gid, product_name from pmr_mst_tproduct  " +
" order by product_name asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProduct>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProduct
                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                        });
                        values.GetProduct = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
             
        }

        public void DaGetOnAdd(string user_gid, productlist values)
        {
            try
            {
                
                msGetGid = objcmnfunctions.GetMasterGID("PODC");
                string lsrefno = "";
                string lspercentage1, lspercentage2;
                string lstax1, lstax2;
                msSQL = "select product_name from pmr_mst_tproduct where product_gid='" + values.product_gid + "'";
                string lsproductName = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select productuom_gid from pmr_mst_tproductuom where productuom_name='" + values.productuom_name + "'";
                string lsproductuomgid = objdbconn.GetExecuteScalar(msSQL);
                if (values.tax_name1 == "")
                {
                    lspercentage1 = "0";
                }
                else
                {
                    msSQL = "select percentage from acp_mst_ttax  where tax_gid='" + values.tax_name1 + "'";
                    lspercentage1 = objdbconn.GetExecuteScalar(msSQL);
                }
                if (values.tax_name2 == "")
                {
                    lspercentage2 = "0";
                }
                else
                {
                    msSQL = "select percentage from acp_mst_ttax  where tax_gid='" + values.tax_name2 + "'";
                    lspercentage2 = objdbconn.GetExecuteScalar(msSQL);
                }

                if (values.tax_name1 == "")
                {
                    lstax1 = "0";
                }
                else
                {
                    msSQL = "select tax_name from acp_mst_ttax  where tax_gid='" + values.tax_name1 + "'";
                    lstax1 = objdbconn.GetExecuteScalar(msSQL);
                }

                if (values.tax_name3 == "")
                {
                    lstax2 = "0";
                }
                else
                {
                    msSQL = "select tax_name from acp_mst_ttax  where tax_gid='" + values.tax_name3 + "'";
                    lstax2 = objdbconn.GetExecuteScalar(msSQL);
                }
                {
                    msSQL = " insert into pmr_tmp_tpurchaseorder ( " +
                            " tmppurchaseorderdtl_gid," +
                            " tmppurchaseorder_gid," +
                            " qty_poadjusted," +
                            " product_gid," +
                            " product_code," +
                            " product_name," +
                            " productuom_name," +
                            " uom_gid," +
                            " qty_ordered," +
                            " product_price," +
                            " discount_percentage," +
                            " discount_amount," +
                            " tax_name," +
                            " tax_name2," +
                            " tax_name3," +
                            " tax_percentage," +
                            " tax_percentage2," +
                            " tax_percentage3," +
                            " tax1_gid," +
                            " tax2_gid," +
                            " tax_amount, " +
                            " tax_amount2, " +
                            " tax_amount3," +
                            " created_by," +
                            " producttotal_price " +
                            " ) values ( " +
                            "'" + msGetGid + "'," +
                            "''," +
                             "'0.00'," +
                            "'" + values.product_gid + "'," +
                            "'" + values.productcode + "'," +
                            "'" + lsproductName + "'," +
                            "'" + values.productuom_name + "'," +
                            "'" + lsproductuomgid + "'," +
                            "'" + values.quantity + "'," +
                            "'" + values.unitprice + "'," +
                            "'" + values.discountpercentage + "'," +
                            "'" + values.discountamount + "'," +
                            "'" + lstax1 + "'," +
                            "'" + values.tax_name2 + "'," +
                            "'" + values.tax_name3 + "'," +
                            "'" + values.taxamount1 + "'," +
                            "'0.00'," +
                            "'0.00'," +
                            "'" + values.tax_name1 + "'," +
                            "'" + lstax2 + "'," +
                            "'" + values.taxamount1 + "'," +
                            "'0.00'," +
                            "'0.00'," +
                            "'" + user_gid + "'," +
                            "'" + values.totalamount + "')";
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
                    values.message = "Error While Adding Product";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
             
        }
        public void DaProductSummary(string user_gid, MdlPmrTrnPurchaseOrder values)
        {
            try
            {
                
                double grand_total = 0.00;
                double grandtotal = 0.00;
                msSQL = " select a.tmppurchaseorderdtl_gid, a.tmppurchaseorder_gid,a.product_gid,a.product_name,a.product_code,b.productgroup_gid," +
                    " format(a.qty_ordered,2)As qty_ordered ,c.productgroup_name,a.productuom_name,b.producttype_gid," +
                    " a.created_by,a.product_price, format(a.producttotal_price,2)as producttotal_price," +
                    " a.discount_percentage, format(a.discount_amount,2)As discount_amount ," +
                    " a.tax_percentage, format(a.tax_amount,2)as tax_amount , format(a.product_total,2)As product_total ,a.needby_date," +
                    " a.type, a.uom_gid, a.display_field," +
                    " a.purchaserequisitiondtl_gid, a.tax_name, a.tax_name2, a.tax_name3, a.tax_percentage2," +
                    " a.tax_percentage3, format(a.tax_amount2,2)AS tax_amount2 ,format(a.tax_amount3,2)As tax_amount3 ," +
                    " a.tax1_gid,a.tax2_gid,a.tax3_gid from pmr_tmp_tpurchaseorder a" +
                    " left join pmr_mst_tproduct b on a.product_gid=b.product_gid" +
                    " left join pmr_mst_tproductgroup c on b.productgroup_gid=c.productgroup_gid" +
                    " left join pmr_mst_tproductuom d on a.uom_gid=d.productuom_gid" +
                    " where a.created_by='" + user_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<productsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        grand_total += double.Parse(dt["producttotal_price"].ToString());
                        grandtotal += double.Parse(dt["producttotal_price"].ToString());
                        getModuleList.Add(new productsummary_list
                        {
                            tmppurchaseorderdtl_gid = dt["tmppurchaseorderdtl_gid"].ToString(),

                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            unitprice = dt["product_price"].ToString(),
                            qty = dt["qty_ordered"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            taxamount1 = dt["tax_amount"].ToString(),
                            tax_name1 = dt["tax_name"].ToString(),
                            product_total = dt["producttotal_price"].ToString(),


                        });
                        values.productsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
                values.grand_total = grand_total;
                values.grandtotal = grandtotal;
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
             
        }

        //Submit
        public void DaProductSubmit(string user_gid, GetViewPurchaseOrder values)
        {
            try
            {
                 
                string uiDateStr = values.po_date;
                DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string po_date = uiDate.ToString("yyyy-MM-dd");

                msGetGID = objcmnfunctions.GetMasterGID("PPRP");
                msPO1GetGID = objcmnfunctions.GetMasterGID("PPOP");


                msSQL = " insert into pmr_trn_tpurchaseorder (" +
                     " purchaseorder_gid, " +
                     " purchaserequisition_gid, " +
                     " quote_referenceno, " +
                     " quotation_gid, " +
                     " branch_gid, " +
                     " deliverytobranch_gid, " +
                     " created_by, " +
                     " purchaseorder_date," +
                     " created_date," +
                     " vendor_gid, " +
                     " vendor_address, " +
                     " shipping_address, " +
                     " vendor_contact_person, " +
                     " purchaseorder_reference, " +
                     " total_amount, " +
                     " termsandconditions, " +
                     " purchaseorder_status, " +
                     " purchaseorder_flag, " +
                     " purchaseorder_remarks, " +
                     " vendor_contactnumber, " +
                     " vendor_emailid, " +
                     " purchaseorder_from, " +
                     " currency_code," +
                     " exchange_rate," +
                     " payment_days," +
                     " poref_no, " +
                     " delivery_days," +
                     " freightcharges," +
                     " priority, " +
                     " priority_remarks," +
                     " tax_amount," +
                     " buybackorscrap," +
                     " packing_charges," +
                     " insurance_charges," +
                     " ship_via, " +
                     " freight_terms, " +
                     " delivery_location, " +
                     " roundoff, " +
                     "  payment_terms " +
                     " ) values (" +
                     "'" + msPO1GetGID + "'," +
                     "'" + msGetGID + "'," +
                     "''," +
                     "''," +
                     "'" + values.branch_name + "'," +
                     "'" + values.dispatch_name + "'," +
                     "'" + user_gid + "'," +
                    "'" + po_date + "', " +
                     "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                     "'" + values.vendor_companyname + "'," +
                     "'" + values.vendor_address + "'," +
                     "'" + values.Shipping_address + "'," +
                     "'" + values.contact_person + "'," +
                     "'" + values.pocovernote_address + "', " +
                     "'" + values.totalamount + "'," +
                     "'" + values.template_content + "'," +
                     "'" + "Approved" + "'," +
                     "'" + "PO Approved" + "'," +
                     "'" + values.remarks + "'," +
                     "'" + values.contact_number + "'," +
                     "'" + values.email_address + "'," +
                     "'Purchase'," +
                     "'" + values.currency_code + "'," +
                     "'" + values.exchange_rate + "'," +
                    "'" + values.payment_days + "'," +
                      "'" + values.po_no + "'," +
                     "'" + values.delivery_days + "',";
                if (values.freighttax_amount == null)
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.freighttax_amount + "',";
                }
                msSQL += "'" + values.priority_n + "', " +
                     "'" + values.priority_remarks + "', ";
                if (values.tax_amount4 == null)
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.tax_amount4 + "',";
                }
                if (values.buybackorscrap == null && values.buybackorscrap == "")
                {
                    msSQL += "'" + values.buybackorscrap + "',";
                }
                else
                {

                    msSQL += "'0.00',";
                }
                if (values.packing_charges == null && values.packing_charges == "")
                {
                    msSQL += "'" + values.packing_charges + "',";
                }
                else
                {

                    msSQL += "'0.00',";
                }
                if (values.insurance_charges == null)
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.insurance_charges + "',";
                }

                msSQL += "'" + values.ship_via + "'," +
                 "'" + values.freight_terms + "'," +
                 "'" + values.delivery_location + "',";
                if (values.roundoff == null)
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.roundoff + "',";
                }
                msSQL += "'" + values.payment_terms + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    msSQL = " select a.tmppurchaseorderdtl_gid, a.tmppurchaseorder_gid,a.product_gid,a.product_name,a.product_code,b.productgroup_gid," +
                             " format(a.qty_ordered,2)As qty_ordered ,c.productgroup_name,a.productuom_name,b.producttype_gid," +
                             " a.created_by,a.product_price, format(a.producttotal_price,2)as producttotal_price," +
                             " a.discount_percentage, format(a.discount_amount,2)As discount_amount ," +
                             " a.tax_percentage, format(a.tax_amount,2)as tax_amount , format(a.product_total,2)As product_total ,a.needby_date," +
                             " a.type, a.uom_gid, a.display_field," +
                             " a.purchaserequisitiondtl_gid, a.tax_name, a.tax_name2, a.tax_name3, a.tax_percentage2," +
                             " a.tax_percentage3, format(a.tax_amount2,2)AS tax_amount2 ,format(a.tax_amount3,2)As tax_amount3 ," +
                             " a.tax1_gid,a.tax2_gid,a.tax3_gid from pmr_tmp_tpurchaseorder a" +
                             " left join pmr_mst_tproduct b on a.product_gid=b.product_gid" +
                             " left join pmr_mst_tproductgroup c on b.productgroup_gid=c.productgroup_gid" +
                             " left join pmr_mst_tproductuom d on a.uom_gid=d.productuom_gid" +
                             " where a.created_by='" + user_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            msPOGetGID = objcmnfunctions.GetMasterGID("PODC");
                            msSQL = " insert into pmr_trn_tpurchaseorderdtl (" +
                                        " purchaseorderdtl_gid, " +
                                        " purchaseorder_gid, " +
                                        " product_gid, " +
                                        " product_code, " +
                                        " product_name, " +
                                        " productuom_name, " +
                                        " uom_gid, " +
                                        " producttype_gid, " +
                                        " product_price, " +
                                        " discount_percentage, " +
                                        " discount_amount, " +
                                        " tax_name, " +
                                        " tax1_gid, " +
                                        " tax_amount, " +
                                        " qty_ordered, " +
                                        " display_field_name," +
                                        " needby_date" +
                                        " )values ( " +
                                        "'" + msPOGetGID + "', " +
                                        "'" + msPO1GetGID + "'," +
                                        "'" + dt["product_gid"].ToString() + "', " +
                                        "'" + dt["product_code"].ToString() + "', " +
                                        "'" + dt["product_name"].ToString() + "', " +
                                        "'" + dt["productuom_name"].ToString() + "', " +
                                        "'" + dt["uom_gid"].ToString() + "', " +
                                        "'" + dt["type"].ToString() + "', " +
                                        "'" + dt["product_price"].ToString() + "', " +
                                        "'" + dt["discount_percentage"].ToString() + "', " +
                                        "'" + dt["discount_amount"].ToString() + "', " +
                                        "'" + dt["tax_name"].ToString() + "', " +
                                        "'" + dt["tax1_gid"].ToString() + "', " +
                                        "'" + dt["tax_amount"].ToString().Replace(",", "") + "', " +
                                        "'" + dt["qty_ordered"].ToString() + "', " +
                                        "'" + dt["display_field"].ToString() + "'," +
                                        "'" + dt["needby_date"].ToString() + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                    }


                    if (mnResult != 0)

                    {
                        values.status = true;
                        values.message = "Purchase Order Raised Successfully!";



                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding product in Purchase Order!";


                    }

                    msSQL = "  delete from pmr_tmp_tpurchaseorder where created_by='" + user_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding product in Purchase Order!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
             
        }
        public void DaDeleteProductSummary(string tmppurchaseorderdtl_gid, productlist values)
        {
            try
            {
                 
                msSQL = "  delete from pmr_tmp_tpurchaseorder where tmppurchaseorderdtl_gid='" + tmppurchaseorderdtl_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while deleting product in PO!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            

        }
        //mailfunction

        public void DaGetTemplatelist(MdlPmrTrnPurchaseOrder values)
        {
            try
            {
                 
                msSQL = " select a.template_gid, c.template_name, c.template_content from adm_trn_ttemplate2module a " +
" left join adm_mst_tmodule b on a.module_gid = b.module_gid " +
" left join adm_mst_ttemplate c on a.template_gid = c.template_gid " +
" left join adm_mst_ttemplatetype d on c.templatetype_gid = d.templatetype_gid " +
" where a.module_gid = 'MKT' and c.templatetype_gid='2' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<templatelist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new templatelist
                        {
                            template_gid = dt["template_gid"].ToString(),
                            template_name = dt["template_name"].ToString(),
                            template_content = dt["template_content"].ToString(),
                        });
                        values.templatelist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting template list!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
        public void DaGetTemplatet(string template_gid, MdlPmrTrnPurchaseOrder values)
        {
            try
            {
                


                msSQL = " select a.template_gid, c.template_name, c.template_content from adm_trn_ttemplate2module a " +
             " left join adm_mst_tmodule b on a.module_gid = b.module_gid " +
             " left join adm_mst_ttemplate c on a.template_gid = c.template_gid " +
             " left join adm_mst_ttemplatetype d on c.templatetype_gid = d.templatetype_gid " +
             " where a.module_gid = 'MKT' and c.template_gid='" + template_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<templatelist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new templatelist
                        {
                            template_gid = dt["template_gid"].ToString(),
                            template_name = dt["template_name"].ToString(),
                            template_content = dt["template_content"].ToString(),
                        });
                        values.templatelist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting template!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
        public void DaPostMail(HttpRequest httpRequest, string user_gid, result objResult)
        {
            {

                msSQL = " select pop_server,pop_port,pop_username,pop_password,company_name,company_code from adm_mst_tcompany where company_gid='1'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    objMySqlDataReader.Read();

                    lspop_server = objMySqlDataReader["pop_server"].ToString();
                    lspop_port = Convert.ToInt32(objMySqlDataReader["pop_port"]);
                    lspop_mail = objMySqlDataReader["pop_username"].ToString();
                    lspop_password = objMySqlDataReader["pop_password"].ToString();
                    lscompany = objMySqlDataReader["company_name"].ToString();
                    lscompany_code = objMySqlDataReader["company_code"].ToString();
                    objMySqlDataReader.Close();
                }

                // attachment get function

                HttpFileCollection httpFileCollection;
                string lsfilepath = string.Empty;
                string lsdocument_gif = string.Empty;
                MemoryStream ms_stream = new MemoryStream();

                //split function

                string mail_from = httpRequest.Form[1];
                string sub = httpRequest.Form[2];
                string to = httpRequest.Form[3];
                string body = httpRequest.Form[4];
                string bcc = httpRequest.Form[5];
                string cc = httpRequest.Form[6];

                HttpPostedFile httpPostedFile;

                // save path

                string lsPath = string.Empty;
                lsPath = ConfigurationManager.AppSettings["Doc_upload_file"] + "/erp_documents" + "/" + lscompany_code + "/" + "Mail/Post/" + DateTime.Now.Year + "/" + DateTime.Now.Month;
                {
                    if ((!System.IO.Directory.Exists(lsPath)))
                        System.IO.Directory.CreateDirectory(lsPath);
                }
                try
                {
                    if (httpRequest.Files.Count > 0)
                    {
                        string file_path = string.Empty;
                        httpFileCollection = httpRequest.Files;
                        for (int i = 0; i < httpFileCollection.Count; i++)
                        {
                            string document_gid = objcmnfunctions.GetMasterGID("UPLF");
                            httpPostedFile = httpFileCollection[i];
                            string FileExtension = httpPostedFile.FileName;
                            pdf_name = httpPostedFile.FileName;
                            string lsfilepath_gid = document_gid;
                            FileExtension = Path.GetExtension(FileExtension).ToLower();
                            string lsfilepaths_gid = lsfilepath_gid + FileExtension;
                            Stream ls_stream;
                            ls_stream = httpPostedFile.InputStream;
                            ls_stream.CopyTo(ms_stream);

                            // upload file
                            lspath = "/erp_documents" + "/" + lscompany_code + "/" + "Mail/Post/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                            //lsPath = ConfigurationManager.AppSettings["Doc_upload_file"] + "/erp_documents" + "/" + lscompany_code + "/" + "Mail/Post/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                            string return_path;
                            return_path = objcmnfunctions.uploadFile(lsPath + "/" + document_gid, FileExtension);
                            ms_stream.Close();
                            //lspath = "/erp_documents" + "/" + lscompany_code + "/" + "Mail/Post/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                            lspath1 = "erp_documents" + "/" + lscompany_code + "/" + "Mail/Post/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + lsfilepath_gid + FileExtension;
                            mail_path = lspath1;

                            // Get file attachment from path

                            //mail_filepath =  System.IO.Path.GetFileName(document_gid);
                            msGet_att_Gid = objcmnfunctions.GetMasterGID("BEAC");
                            msenquiryloggid = objcmnfunctions.GetMasterGID("BELP");
                            msSQL = " insert into acc_trn_temailattachments (" +
                                     " emailattachment_gid, " +
                                     " email_gid, " +
                                     " attachment_systemname, " +
                                     " attachment_path, " +
                                     " inbuild_attachment, " +
                                     " attachment_type " +
                                     " ) values ( " +
                                     "'" + msGet_att_Gid + "'," +
                                     "'" + msenquiryloggid + "'," +
                                     "'" + pdf_name + "'," +
                                     "'" + lspath1 + "', " +
                                     "'" + lspath1 + "', " +
                                     "'" + FileExtension + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                    }

                }
                catch (Exception errormessege)
                {

                }
               

                msSQL = " select inbuild_attachment from acc_trn_temailattachments where email_gid='" + msenquiryloggid + "'";
                mail_datatable = objdbconn.GetDataTable(msSQL);

                //  message of mail

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                message.From = new MailAddress(mail_from);
                message.To.Add(new MailAddress(to));
                message.Body = body;
                message.Subject = sub;
                message.IsBodyHtml = true; // convert into html
                message.Priority = MailPriority.Normal;

                foreach (DataRow dt in mail_datatable.Rows)
                {
                    if (mail_datatable.Rows.Count > 0)
                    {
                        message.Attachments.Add(new Attachment(HttpContext.Current.Server.MapPath("../../../" + dt["inbuild_attachment"].ToString())));
                    }
                    else
                    {

                    }
                }

                // mail send 

                SmtpClient client = new SmtpClient();
                client.Host = lspop_server;
                client.Port = lspop_port;
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                client.Credentials = new NetworkCredential(lspop_mail, lspop_password);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                try
                {
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                objResult.status = true;
                objResult.message = "Mail Sent Successfully !!";
            }
        }

    }
}




