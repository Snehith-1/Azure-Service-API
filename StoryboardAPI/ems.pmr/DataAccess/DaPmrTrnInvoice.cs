﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ems.pmr.Models;
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
using System.Reflection.Emit;
using MySql.Data.MySqlClient;

namespace ems.pmr.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class DaPmrTrnInvoice
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        MySqlDataReader objMySqlDataReader;
        DataTable dt_datatable;
        string msGetGid;
        int mnResult, mnResult1;
        public void DaGetPmrTrnInvoiceAddSelectSummary(MdlPmrTrnInvoice values)
        {
            try { 
            msSQL = " select * from pbl_trn_tinvoiceaddsum_V order by date(purchaseorder_date) desc,purchaseorder_date asc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<invoice_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new invoice_list
                    {
                        vendor_gid = dt["vendor_gid"].ToString(),
                        purchaseorder_date = dt["purchaseorder_date"].ToString(),
                        purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        vendor_companyname = dt["vendor_companyname"].ToString(),
                        vendor_contact_person = dt["vendor_contact_person"].ToString(),
                        costcenter_name = dt["costcenter_name"].ToString(),
                        invoice_amount = dt["invoice_amount"].ToString(),
                        total_amount = dt["total_amount"].ToString(),
                        outstanding_amount = dt["outstanding_amount"].ToString(),

                    });
                    values.invoice_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading invoice!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetInvoiceSummary(MdlPmrTrnInvoice values, string user_gid)
        {
            try { 

            msSQL = " select /*+ MAX_EXECUTION_TIME(300000) */ distinct a.invoice_gid,a.agreement_gid, a.invoice_refno, case when a.invoice_status='IV Approved' then 'IV Completed' else a.invoice_status end as invoice_status, a.invoice_flag, " +
                " CASE when a.payment_flag <> 'PY Pending' then a.payment_flag " +
                " else a.invoice_flag end as 'overall_status', format(a.initialinvoice_amount,2) as initialinvoice_amount, " +
                " a.vendor_gid, format(a.invoice_amount,2) as invoice_amount,a.vendorinvoiceref_no, a.vendor_gid, i.costcenter_gid, " +
                " CASE WHEN i.costcenter_gid is NOT NULL THEN " +
                " (select costcenter_name from pmr_mst_tcostcenter x where h.costcenter_gid=x.costcenter_gid) " +
                " ELSE (select costcenter_name from pmr_mst_tcostcenter y where j.costcenter_gid=y.costcenter_gid) END as costcenter_name, " +
                "  DATE_FORMAT(a.invoice_date, '%d-%m-%Y') AS invoice_date," +
                " DATE_FORMAT(a.payment_date, '%d-%m-%Y') AS payment_date," +
                " a.payment_flag,a.invoice_type, " +
                " c.vendor_code, c.vendor_companyname,a.invoice_from,a.invoice_reference " +
                " from acp_trn_tinvoice a " +
                " left join acp_trn_tinvoicedtl b on a.invoice_gid = b.invoice_gid " +
                " left join acp_mst_tvendor c on a.vendor_gid = c.vendor_gid " +
                " left join adm_mst_tuser d on d.user_gid = a.user_gid " +
                " left join hrm_mst_temployee e on e.user_gid = d.user_gid " +
                " left join adm_mst_tmodule2employee  f on f.employee_gid = e.employee_gid " +
                " left join acp_trn_tpo2invoice g on g.invoice_gid=a.invoice_gid " +
                " left join pmr_trn_tpurchaseorder h on g.purchaseorder_gid=h.purchaseorder_gid " +
                " left join pmr_mst_tcostcenter i on h.costcenter_gid=i.costcenter_gid " +
                " left join pbl_trn_tserviceorder j on j.serviceorder_gid=a.invoice_reference " +
                " where a.invoice_type<>'Opening Invoice'  " +
                " order by date(a.invoice_date) desc,a.invoice_date asc, a.invoice_gid desc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<invoice_lista>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new invoice_lista
                    {
                        invoice_gid = dt["invoice_gid"].ToString(),
                        invoice_date = dt["invoice_date"].ToString(),
                        payment_date = dt["payment_date"].ToString(),
                        invoice_refno = dt["invoice_refno"].ToString(),
                        vendorinvoiceref_no = dt["vendorinvoiceref_no"].ToString(),
                        vendor_companyname = dt["vendor_companyname"].ToString(),
                        costcenter_name = dt["costcenter_name"].ToString(),
                        invoice_amount = dt["invoice_amount"].ToString(),
                        invoice_type = dt["invoice_type"].ToString(),
                        invoice_status = dt["invoice_status"].ToString(),
                        overall_status = dt["overall_status"].ToString(),

                    });
                    values.invoice_lista = getModuleList;
                }
            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading invoice!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        //purchasetype dropdowm
        public void DaGetPmrPurchaseDtl(MdlPmrTrnInvoice values)
        {
            try
            {

                msSQL = " select  a.account_gid, a.purchasetype_name " +
                        " from pmr_trn_tpurchasetype a ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetPurchaseTypeDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetPurchaseTypeDropdown
                        {
                            account_gid = dt["account_gid"].ToString(),
                            purchasetype_name = dt["purchasetype_name"].ToString(),
                        });
                        values.GetPmrPurchaseDtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading invoice!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetEditInvoiceSummary(string vendor_gid, MdlPmrTrnInvoice values)
        {
            try
            {
                msSQL = " SELECT a.serviceorder_gid,date_format(a.serviceorder_date, '%d/%m/%Y') as serviceorder_date," +
                    " b.vendor_companyname,b.vendor_gid,format(a.grand_total, 2) as grand_total,a.currency_code,sum(e.invoice_amount)" +
                    " as invoice_amount,f.tax_name,a.tax_amount, b.contactperson_name,b.email_id,b.vendor_code,c.exchange_rate,s.outstanding_amount," +
                    " format(a.addon_amount, 2) as addon_amount ,format(a.discount_amount, 2) as discount_amount,d.branch_name , " +
                    " g.product_gid, g.serviceorderdtl_gid,g.quantity,g.unit_price,format(g.amount, 2) as amount ,e.invoice_gid, e.invoice_date, e.invoice_remarks, " +
                    " format(g.tax_amount1, 2) as tax_amount1,g.tax1_gid,g.tax2_gid,g.tax3_gid, " + "format(g.tax_amount2, 2) as tax_amount2," +
                    " format(g.tax_amount3, 2) as tax_amount3, format(g.total_amount - g.tax_amount1 - tax_amount2 - tax_amount3, 2) as total_amount," +
                    " g.description, h.product_name, h.product_code, i.productgroup_name, i.productgroup_code,j.productuom_name,j.productuom_code,g.tax_name1, " +
                    " g.tax_name2,g.tax_name3,format(g.discount_amount, 2) as discount_amount, format((g.amount * quantity -if (g.discount_amount is null,'0.00', " +
                    " g.discount_amount)),2) as amount1,g.discount_percentage , e.invoice_refno,e.payment_date ,e.payment_term, m.purchasetype_name " +
                    " from pbl_trn_tserviceorder a  " +
                    " left join acp_mst_tvendor b on a.customer_gid = b.vendor_gid " +
                    " left join crm_trn_tcurrencyexchange c on a.currency_code = c.currency_code " +
                    " left join hrm_mst_tbranch d on a.branch_gid = d.branch_gid" +
                    " left join acp_trn_tinvoice e on a.serviceorder_gid = e.invoice_reference" +
                    " left join acp_mst_ttax f on a.tax_gid = f.tax_gid" +
                    " left join pbl_trn_tserviceorderdtl g on a.serviceorder_gid = g.serviceorder_gid " +
                    " left join pmr_mst_tproduct h on g.product_gid = h.product_gid " +
                    " left join pmr_mst_tproductgroup i on h.productgroup_gid = i.productgroup_gid " +
                    " left join pmr_mst_tproductuom j on h.productuom_gid = j.productuom_gid" +
                    " left join pmr_trn_tpurchaseorder l on b. vendor_gid=a.serviceorder_gid " +
                    " left join  pmr_trn_tpurchasetype m on m. purchasetype_gid=a.serviceorder_gid " +
                    " left join pbl_trn_tserviceorder k on g.serviceorder_gid = k.serviceorder_gid " +
                    " left join pbl_trn_tinvoiceaddsum_V s on b.vendor_companyname = s.vendor_companyname " +
                    " where a.customer_gid = '" + vendor_gid + "'";
                string msgetGID = objcmnfunctions.GetMasterGID("SIVP");
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetEditInvList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetEditInvList
                        {
                            vendor_gid = dt["vendor_gid"].ToString(),
                            serviceorder_gid = dt["serviceorder_gid"].ToString(),
                            serviceorder_date = dt["serviceorder_date"].ToString(),
                            invoice_refno = msgetGID,
                            payment_date = dt["payment_date"].ToString(),
                            payment_term = dt["payment_term"].ToString(),
                            invoice_gid = dt["invoice_gid"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            email_id = dt["email_id"].ToString(),
                            contactperson_name = dt["contactperson_name"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            invoice_remarks = dt["invoice_remarks"].ToString(),
                            purchasetype_name = dt["purchasetype_name"].ToString(),
                            //addon_amount = dt["addon_amount"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            description = dt["description"].ToString(),
                            quantity = dt["quantity"].ToString(),
                            unit_price = dt["unit_price"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            tax_amount1 = dt["tax_amount1"].ToString(),
                            tax_amount2 = dt["tax_amount2"].ToString(),
                            tax_amount3 = dt["tax_amount3"].ToString(),
                            tax_name1 = dt["tax_name1"].ToString(),
                            tax_name2 = dt["tax_name2"].ToString(),
                            tax_name3 = dt["tax_name3"].ToString(),
                            outstanding_amount = dt["outstanding_amount"].ToString()
                        });
                        values.invoiceaddcomfirm_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading invoice!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGettaxnamedropdown(MdlPmrTrnInvoice values)
        {
            try
            {
                msSQL = " select tax_gid,tax_name,percentage from acp_mst_ttax ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<taxnamedropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new taxnamedropdown
                        {
                            tax_name = dt["tax_name"].ToString(),
                            tax_gid = dt["tax_gid"].ToString(),
                            tax_percentage = dt["percentage"].ToString()

                        });
                        values.taxnamedropdown = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading invoice!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }


        public void DaGetPmrTrnInvoiceview(string invoice_gid, MdlPmrTrnInvoice values)
        {
            try
            {
                msSQL = "SELECT a.invoice_gid,a.invoice_refno, a.vendor_address,a.order_total,a.received_amount,a.received_year,k.contact_telephonenumber,k.contactperson_name," +
                    "a.vendor_contact_person,j.branch_name,k.vendor_companyname," +
                    "j.branch_name,date_format(a.invoice_date,'%d-%m-%Y') as invoice_date,i.costcenter_name,i.budget_available, " +
                    " date_format(a.payment_date,'%d-%m-%Y') as payment_date,a.round_off, a.payment_term, a.vendor_gid,format(a.extraadditional_amount, 2) as extraadditional_amount, " +
                    " case when a.currency_code is null then 'INR' else a.currency_code end as currency_code,format(a.extradiscount_amount, 2) as extradiscount_amount, " +
                    " case when a.exchange_rate is null then '1' else a.exchange_rate end as exchange_rate,CONCAT(ifnull(sum(e.product_total),'0.00') ) as price, " +
                    " a.invoice_remarks, a.reject_reason, a.invoice_status, format(a.invoice_amount,2) as invoice_amount, a.invoice_reference, " +
                    " format(a.freightcharges_amount, 2) as freightcharges_amount, format(a.additionalcharges_amount, 2) as additionalcharges_amount, " +
                    " format(a.discount_amount, 2) as discount_amount, format(a.total_amount, 2) as total_amount,  " +
                    " format(a.freightcharges,2) as freightcharges,format(a.buybackorscrap,2) as buybackorscrap,a.invoice_total,a.raised_amount,a.tax_amount,a.tax_name, " +
                    " e.product_gid, (e.qty_invoice) as qty_invoice, format(e.product_price,2) as product_price, " +
                    " format(e.discount_percentage,2) as discount_percentage, " +
                    " e.tax_name,e.tax_name2,e.tax_name3,format(e.tax_amount2,2)as tax_amount2,format(e.tax_amount3,2)as tax_amount3," +
                    " format(e.discount_amount,2) as discount_amount, format(e.tax_percentage,2) as tax_percentage, " +
                    " format(e.tax_amount,2) as tax_amount, e.product_remarks,  format((e.qty_invoice * e.product_price),2) as product_totalprice, " +
                    " format(e.excise_percentage,2) as excise_percentage, format(e.excise_amount,2) as excise_amount, " +
                    " format(e.product_total,2) as product_total, g.po2invoice_gid, e.invoice_gid, e.invoicedtl_gid, g.grn_gid, g.grndtl_gid, " +
                    " g.purchaseorder_gid, g.purchaseorderdtl_gid, g.product_gid, g.qty_invoice, " +
                    " e.product_name, e.product_code, " +
                    " e.productgroup_name, e.productuom_name " +
                    " FROM acp_trn_tinvoice a " +
                    " left join acp_trn_tinvoicedtl e on a.invoice_gid=e.invoice_gid " +
                    " left join acp_trn_tpo2invoice g on a.invoice_gid=g.invoice_gid " +
                    " left join pmr_trn_tpurchaseorder h on g.purchaseorder_gid=h.purchaseorder_gid " +
                    " left join pmr_mst_tcostcenter i on h.costcenter_gid=i.costcenter_gid " +
                    " left join hrm_mst_tbranch j on a.branch_gid=j.branch_gid " +
                    " left join acp_mst_tvendor k on a.vendor_gid=k.vendor_gid " +
                    " where a.invoice_gid = '" + invoice_gid + "' group by a.invoice_gid ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<invoice_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new invoice_lists
                        {
                            invoice_gid = dt["invoice_gid"].ToString(),

                            branch_name = dt["branch_name"].ToString(),
                            contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                            contactperson_name = dt["contactperson_name"].ToString(),
                            vendor_address = dt["vendor_address"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            invoice_remarks = dt["invoice_remarks"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            qty_invoice = dt["qty_invoice"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            product_totalprice = dt["product_totalprice"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            tax_name1 = dt["tax_name1"].ToString(),
                            tax_amount1 = dt["tax_amount1"].ToString(),
                            tax_name2 = dt["tax_name2"].ToString(),
                            tax_amount2 = dt["tax_amount2"].ToString(),
                            tax_name3 = dt["tax_name3"].ToString(),
                            tax_amount3 = dt["tax_amount3"].ToString(),
                            product_remarks = dt["product_remarks"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),
                        });
                        values.invoice_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading invoice!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void Dapblinvoicesubmit(string employee_gid, pblinvoice_list values)
        {
            try
            {



                string ls_referenceno = objcmnfunctions.GetMasterGID("SIVP");
                string lstype1 = "Services";

                msSQL = " select branch_gid from hrm_mst_tbranch where branch_name='" + values.branch_name + "'";
                string lsbranch_gid = objdbconn.GetExecuteScalar(msSQL);

                if (values.addon_amount == "" || values.addon_amount == null) { values.addon_amount = "0"; }
                if (values.discount_amount == "" || values.discount_amount == null) { values.discount_amount = "0"; }


                msSQL = " insert into acp_trn_tinvoice(" +
                    " invoice_gid," +
                    " invoice_date," +
                    " invoice_reference," +
                    " payment_date," +
                    " payment_term," +
                    " vendor_gid," +
                    " total_amount," +
                    " invoice_amount," +
                    " invoice_refno," +
                    " invoice_status, " +
                    " invoice_flag, " +
                    " user_gid," +
                    " created_date," +
                    " discount_amount," +
                    " additionalcharges_amount," +
                    " total_amount_L," +
                    " discount_amount_L," +
                    " additionalcharges_amount_L," +
                    " invoice_from," +
                    " invoice_type," +
                    " currency_code," +
                    " exchange_rate," +
                    " branch_gid," +
                    " invoice_remarks " +
                    ") values (" +
                     "'" + values.invoice_gid + "'," +
                     "'" + values.invoice_date.ToString("yyyy-MM-dd ") + "'," +
                     "'" + values.serviceorder_gid + "'," +
                     "'" + values.payment_date.ToString("yyyy-MM-dd ") + "'," +
                     "'" + values.payment_term + "'," +
                     "'" + values.vendor_gid + "'," +
                     "'" + values.grand_total + "'," +
                     "'" + values.invoice_amount + "'," +
                     "'" + values.invoice_gid + "'," +
                     "'IV Approved'," +
                     "'Payment Pending'," +
                     "'" + employee_gid + "'," +
                     "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                     "'" + values.discount_amount + "'," +
                     "'" + values.addon_amount + "'," +
                     "'" + values.grand_total + "'," +
                     "'" + values.discount_amount + "'," +
                     "'" + values.addon_amount + "'," +
                     "'" + values.invoice_from + "'," +
                     "'" + lstype1 + "'," +
                     "'" + values.currency_code + "'," +
                     "'" + values.exchange_rate + "'," +
                     "'" + lsbranch_gid + "'," +
                     "'" + values.invoice_remarks + "')";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    msSQL = " SELECT a.serviceorder_gid,date_format(a.serviceorder_date, '%d/%m/%Y') as serviceorder_date," +
                    " b.vendor_companyname,b.vendor_gid,format(a.grand_total, 2) as grand_total,a.currency_code,sum(e.invoice_amount)" +
                    " as invoice_amount,f.tax_name,a.tax_amount, b.contactperson_name,b.email_id,b.vendor_code,c.exchange_rate," +
                    " format(a.addon_amount, 2) as addon_amount ,format(a.discount_amount, 2) as discount_amount,d.branch_name , " +
                    " g.product_gid, g.serviceorderdtl_gid,g.quantity,g.unit_price,format(g.amount, 2) as amount ,e.invoice_gid, e.invoice_date, e.invoice_remarks, " +
                    " format(g.tax_amount1, 2) as tax_amount1,g.tax1_gid,g.tax2_gid,g.tax3_gid, " + "format(g.tax_amount2, 2) as tax_amount2," +
                    " format(g.tax_amount3, 2) as tax_amount3, format(g.total_amount - g.tax_amount1 - tax_amount2 - tax_amount3, 2) as total_amount," +
                    " g.description, h.product_name, h.product_code, i.productgroup_name, i.productgroup_code,j.productuom_name,j.productuom_code,g.tax_name1, " +
                    " g.tax_name2,g.tax_name3,format(g.discount_amount, 2) as discount_amount, format((g.amount * quantity -if (g.discount_amount is null,'0.00', " +
                    " g.discount_amount)),2) as amount1,g.discount_percentage , e.invoice_refno,e.payment_date ,e.payment_term, m.purchasetype_name " +
                    " from pbl_trn_tserviceorder a  " +
                    " left join acp_mst_tvendor b on a.customer_gid = b.vendor_gid " +
                    " left join crm_trn_tcurrencyexchange c on a.currency_code = c.currency_code " +
                    " left join hrm_mst_tbranch d on a.branch_gid = d.branch_gid" +
                    " left join acp_trn_tinvoice e on a.serviceorder_gid = e.invoice_reference" +
                    " left join acp_mst_ttax f on a.tax_gid = f.tax_gid" +
                    " left join pbl_trn_tserviceorderdtl g on a.serviceorder_gid = g.serviceorder_gid " +
                    " left join pmr_mst_tproduct h on g.product_gid = h.product_gid " +
                    " left join pmr_mst_tproductgroup i on h.productgroup_gid = i.productgroup_gid " +
                    " left join pmr_mst_tproductuom j on h.productuom_gid = j.productuom_gid" +
                    " left join pmr_trn_tpurchaseorder l on b. vendor_gid=a.serviceorder_gid " +
                    " left join  pmr_trn_tpurchasetype m on m. purchasetype_gid=a.serviceorder_gid " +
                    " left join pbl_trn_tserviceorder k on g.serviceorder_gid = k.serviceorder_gid " +
                    " where a.customer_gid = '" + values.vendor_gid + "'";

                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("SIVC");
                        msSQL = " insert into acp_trn_tinvoicedtl (" +
                            " invoicedtl_gid, " +
                            " invoice_gid, " +
                            " product_gid, " +
                            " product_price, " +
                            " product_total, " +
                            " discount_percentage, " +
                            " discount_amount, " +
                            " tax_name, " +
                            " tax_name2, " +
                            " tax_name3, " +
                            " tax_amount, " +
                            " tax_amount2, " +
                            " tax_amount3, " +
                            " tax1_gid, " +
                            " tax2_gid, " +
                            " tax3_gid, " +
                            " product_remarks, " +
                            " product_price_L," +
                            " discount_amount_L," +
                            " tax_amount1_L," +
                            " tax_amount2_L," +
                            " tax_amount3_L," +
                            " qty_invoice," +
                            " productgroup_code," +
                            " productgroup_name," +
                            " product_code," +
                            " product_name," +
                            " productuom_code," +
                            " productuom_name" +
                            " ) values ( " +
                            "'" + msGetGid + "'," +
                            "'" + values.invoice_gid + "'," +
                            "'" + dt["product_gid"].ToString() + "'," +
                            "'" + dt["unit_price"].ToString().Replace(",", "") + "'," +
                            "'" + dt["total_amount"].ToString().Replace(",", "") + "'," +
                            "'" + dt["discount_percentage"].ToString() + "'," +
                            "'" + dt["discount_amount"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax_name1"].ToString() + "'," +
                            "'" + dt["tax_name2"].ToString() + "'," +
                            "'" + dt["tax_name3"].ToString() + "'," +
                            "'" + dt["tax_amount1"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax_amount2"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax_amount3"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax1_gid"].ToString() + "'," +
                            "'" + dt["tax2_gid"].ToString() + "'," +
                            "'" + dt["tax3_gid"].ToString() + "'," +
                            "'" + dt["description"].ToString() + "'," +
                            "'" + dt["unit_price"].ToString().Replace(",", "") + "'," +
                            "'" + dt["discount_amount"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax_amount1"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax_amount2"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax_amount3"].ToString().Replace(",", "") + "'," +
                            "'" + dt["quantity"].ToString().Replace(",", "") + "'," +
                            "'" + dt["productgroup_code"].ToString() + "'," +
                            "'" + dt["productgroup_name"].ToString() + "'," +
                            "'" + dt["product_code"].ToString() + "'," +
                            "'" + dt["product_name"].ToString() + "'," +
                            "'" + dt["productuom_code"].ToString() + "'," +
                            "'" + dt["productuom_name"].ToString() + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }

                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Invoice Added Successfully";
                    }
                    else
                    {

                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading invoice!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
    }
}
