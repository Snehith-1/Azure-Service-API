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
using MySql.Data.MySqlClient;

namespace ems.pmr.DataAccess
{
    public class DaPblTrnPaymentRpt
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        MySqlDataReader objMySqlDataReader, objMySqlDataReader1;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        string lsoutstanding, lsadvance;

        public void DaGetPaymentRptSummary(MdlPblTrnPaymentRpt values)
        {
            try
            {
                msSQL = " select a.payment_gid,h.invoice_gid,date_format(a.payment_date,'%d-%m-%y') as payment_date,a.vendor_gid,a.payment_remarks,a.payment_total,a.payment_status,a.user_gid, " +
                        " date_format(a.created_date,'%d-%m-%y') as created_date,a.payment_reference,a.purchaseorder_gid,a.advance_total,a.payment_mode,a.bank_name,a.branch_name, " +
                        " concat(a.cheque_no,a.dd_no)as cheque_no,a.city_name,a.currency_code,a.exchange_rate,a.tds_amount,a.tdscalculated_finalamount," +
                        " a.priority,a.priority_remarks,a.approved_by,a.approved_date,a.reject_reason,a.bank_gid,a.payment_from, " +
                        " a.addon_amount,a.additional_discount,a.additional_gid,a.discount_gid,b.*,c.* " +
                        " from acp_trn_tpayment a " +
                        " left join acp_trn_tinvoice2payment h on h.payment_gid=a.payment_gid " +
                        " left join acp_trn_tpaymentdtl c on a.payment_gid=c.payment_gid " +
                        " left join acp_mst_tvendor b on b.vendor_gid=a.vendor_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<paymentrpt_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new paymentrpt_list
                        {
                            payment_gid = dt["payment_gid"].ToString(),
                            invoice_gid = dt["invoice_gid"].ToString(),
                            payment_date = dt["payment_date"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            payment_remarks = dt["payment_remarks"].ToString(),
                            payment_total = dt["payment_total"].ToString(),
                            payment_status = dt["payment_status"].ToString(),
                            user_gid = dt["user_gid"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            payment_reference = dt["payment_reference"].ToString(),
                            purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                            advance_total = dt["advance_total"].ToString(),
                            payment_mode = dt["payment_mode"].ToString(),
                            bank_name = dt["bank_name"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            cheque_no = dt["cheque_no"].ToString(),
                            city_name = dt["city_name"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                            tds_amount = dt["tds_amount"].ToString(),
                            tdscalculated_finalamount = dt["tdscalculated_finalamount"].ToString(),
                            priority = dt["priority"].ToString(),
                            priority_remarks = dt["priority_remarks"].ToString(),
                            approved_by = dt["approved_by"].ToString(),
                            approved_date = dt["approved_date"].ToString(),
                            reject_reason = dt["reject_reason"].ToString(),
                            bank_gid = dt["bank_gid"].ToString(),
                            addon_amount = dt["addon_amount"].ToString(),
                            additional_discount = dt["additional_discount"].ToString(),
                            additional_gid = dt["additional_gid"].ToString(),
                            discount_gid = dt["discount_gid"].ToString(),

                        });
                        values.paymentrptlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetInvoicedetails(string invoice_gid, MdlPblTrnPaymentRpt values)
        {
            try
            {
                msSQL = "select invoice_refno,date_format(invoice_date,'%d-%m-%y') as invoice_date,invoice_amount from acp_trn_tinvoice where invoice_gid = '" + invoice_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetInvoice>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetInvoice
                        {

                            invoice_refno = dt["invoice_refno"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),

                        });
                        values.getinvoice = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetPaymentview(string payment_gid, string user_gid, MdlPblTrnPaymentRpt values)
        {
            try
            {
                msSQL = " SELECT a.payment_gid,c.address1,d.country, a.payment_date, a.vendor_gid, a.payment_remarks, a.payment_status," +
                        " a.payment_mode, a.bank_name, a.branch_name, a.cheque_no, a.city_name, a.dd_no, a.currency_code," +
                        " a.exchange_rate, format(a.payment_total, 2) as payment_total," +
                        " a.payment_reference,b.vendor_companyname,b.contactperson_name,b.contact_telephonenumber," +
                        " b.email_id,concat(c.address1, c.city, c.postal_code, c.state, d.country) as vendoraddress,c.fax" +
                        " FROM acp_trn_tpayment a left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid " +
                        " left join adm_mst_taddress c on b.address_gid = c.address_gid " +
                        " left join crm_trn_Tcurrencyexchange d on d.country_gid = c.country_gid where a.payment_gid = '" + payment_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                msSQL = " select concat(a.user_firstname,' - ',c.department_name) as employee_name, " +
                        " b.employee_emailid, b.employee_phoneno, c.department_name " +
                        " from adm_mst_tuser a " +
                        " left join hrm_mst_temployee b on a.user_gid = b.user_gid " +
                        " left join hrm_mst_tdepartment c on b.department_gid = c.department_gid " +
                        " where a.user_gid = '" + user_gid + "'";
                string name = objdbconn.GetExecuteScalar(msSQL);

                var getModuleList = new List<payment_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new payment_lists
                        {
                            payment_gid = dt["payment_gid"].ToString(),
                            payment_date = dt["payment_date"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            payment_remarks = dt["payment_remarks"].ToString(),
                            payment_mode = dt["payment_mode"].ToString(),
                            payment_status = dt["payment_status"].ToString(),
                            payment_total = dt["payment_total"].ToString(),
                            payment_reference = dt["payment_reference"].ToString(),
                            city_name = dt["city_name"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                            address1 = dt["address1"].ToString(),
                            country = dt["country"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            contactperson_name = dt["contactperson_name"].ToString(),
                            fax = dt["fax"].ToString(),
                            vendoraddress = dt["vendoraddress"].ToString(),
                            email_id = dt["email_id"].ToString(),
                            contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                            employee_name = name,



                        });
                        values.paymentlists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }


        public void DagetPaymenamount(string payment_gid, MdlPblTrnPaymentRpt values)
        {
            try
            {
                msSQL = " Select a.invoice2payment_gid, a.payment_gid, a.paymentdtl_gid, a.invoice_gid, " +
                        " format(a.invoice_amount, 2) as invoice_amount, format(a.advance_amount, 2) as po_advance, " +
                        " format(a.payment_amount, 2) as payment_amount, b.invoice_remarks ,c.payment_reference " +
                        " FROM acp_trn_tinvoice2payment a " +
                        " left join acp_trn_tpaymentdtl b on a.paymentdtl_gid = b.paymentdtl_gid " +
                        " left join acp_trn_tpayment c on a.payment_gid = c.payment_gid " +
                        " where a.payment_gid = '" + payment_gid + "' order by a.payment_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<paymentamount_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new paymentamount_list
                        {
                            invoice2payment_gid = dt["invoice2payment_gid"].ToString(),
                            payment_gid = dt["payment_gid"].ToString(),
                            paymentdtl_gid = dt["paymentdtl_gid"].ToString(),
                            invoice_gid = dt["invoice_gid"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            po_advance = dt["po_advance"].ToString(),
                            payment_amount = dt["payment_amount"].ToString(),
                            invoice_remarks = dt["invoice_remarks"].ToString(),
                            payment_reference = dt["payment_reference"].ToString(),
                        });

                        values.paymentamount_list = getModuleList;

                    }

                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetPaymentAddproceed(MdlPblTrnPaymentRpt values, string vendorname)
        {
            try
            {
                if (vendorname != null)
                {


                    msSQL = " select a.invoice_gid,b.vendor_companyname,b.vendor_gid,a.invoice_status,format(sum(a.invoice_amount),2)as invoice_amount," +
                    " format(sum(a.payment_amount),2) as payment_amount,format(sum(a.invoice_amount-a.payment_amount),2) as outstanding,a.invoice_from, " +
                    " case when b.contact_telephonenumber is null then  concat(b.contactperson_name,'/',b.email_id) " +
                    " when b.contact_telephonenumber is not null then concat(b.contactperson_name,'/',b.contact_telephonenumber,'/',b.email_id) end as contact " +
                    " from acp_mst_tvendor b" +
                    " left join acp_trn_tinvoice a on b.vendor_gid=a.vendor_gid where " +
                    " a.invoice_amount <> ABS((a.payment_amount+a.advance_amount+a.debit_note)) and " +
                    " ((a.invoice_flag = 'Payment Pending' and a.payment_flag ='PY Pending') or " +
                    " (a.invoice_flag = 'Payment Pending' and a.payment_flag = 'Payment Done Partial')) and  " +
                    " a.invoice_from <> 'Expenses' and b.vendor_companyname like '%" + vendorname + "%' " +
                    " group by b.vendor_gid order by a.invoice_gid desc ";

                }

                else
                {
                    msSQL = " select a.invoice_gid,b.vendor_companyname,b.vendor_gid,a.invoice_status,format(sum(a.invoice_amount),2)as invoice_amount," +
                    " format(sum(a.payment_amount),2) as payment_amount,format(sum(a.invoice_amount-a.payment_amount),2) as outstanding,a.invoice_from, " +
                    " case when b.contact_telephonenumber is null then  concat(b.contactperson_name,'/',b.email_id) " +
                    " when b.contact_telephonenumber is not null then concat(b.contactperson_name,'/',b.contact_telephonenumber,'/',b.email_id) end as contact " +
                    " from acp_mst_tvendor b" +
                    " left join acp_trn_tinvoice a on b.vendor_gid=a.vendor_gid where " +
                    " a.invoice_amount <> ABS((a.payment_amount+a.advance_amount+a.debit_note)) and " +
                    " ((a.invoice_flag = 'Payment Pending' and a.payment_flag ='PY Pending') or " +
                    " (a.invoice_flag = 'Payment Pending' and a.payment_flag = 'Payment Done Partial')) and a.invoice_from <> 'Expenses' " +
                    " group by b.vendor_gid order by a.invoice_gid desc ";

                }

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<paymentadd>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new paymentadd
                        {
                            invoice_gid = dt["invoice_gid"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            invoice_status = dt["invoice_status"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            payment_amount = dt["payment_amount"].ToString(),
                            outstanding = dt["outstanding"].ToString(),
                            invoice_from = dt["invoice_from"].ToString(),
                            contact = dt["contact"].ToString(),

                        });
                        values.paymentadd = getModuleList;
                    }

                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetMakepaymentExpand(string vendor_gid, MdlPblTrnPaymentRpt values)
        {
            try
            {
                msSQL = " select distinct a.invoice_gid,a.invoice_refno, a.invoice_date, a.invoice_from,a.vendor_gid, a.vendor_contact_person, a.vendor_address, a.invoice_remarks, " +
                    " CASE when a.payment_flag <> 'PY Pending' then a.payment_flag " +
                    " else a.invoice_flag end as 'overall_status', " +
                    " format(a.invoice_amount, 2) as invoice_amount, a.invoice_status, a.user_gid, a.created_date, a.invoice_reference,a.agreement_gid, " +
                    " format(ABS(a.payment_amount + a.advance_amount+a.debit_note),2) as payed_amount,format(ABS(a.invoice_amount-(a.payment_amount+a.advance_amount+a.debit_note)),2) as outstanding," +
                    " a.payment_date,a.invoice_flag,case when d.purchaseorder_gid is null then a.invoice_reference else d.purchaseorder_gid end as purchaseorder_gid , " +
                    " c.vendor_companyname,date(a.payment_date) as due_date,b.producttype_gid " +
                    " from acp_trn_tinvoice a " +
                    " left join acp_trn_tinvoicedtl b on a.invoice_gid = b.invoice_gid " +
                    " left join acp_mst_tvendor c on a.vendor_gid = c.vendor_gid " +
                    " left join acp_trn_tpo2invoice d on a.invoice_gid = d.invoice_gid  " +
                    " where a.invoice_amount <> ABS((a.payment_amount+a.advance_amount+a.debit_note)) and" +
                    " ((a.invoice_flag = 'Payment Pending' and a.payment_flag ='PY Pending') or " +
                    " (a.invoice_flag = 'Payment Pending' and a.payment_flag = 'Payment Done Partial')) and a.invoice_from <> 'Expenses' and a.vendor_gid='" + vendor_gid + "' " +
                    "group by a.invoice_gid  order by a.invoice_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<paymentExpand>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new paymentExpand
                        {
                            invoice_gid = dt["invoice_gid"].ToString(),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            invoice_from = dt["invoice_from"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            vendor_contact_person = dt["vendor_contact_person"].ToString(),
                            vendor_address = dt["vendor_address"].ToString(),
                            invoice_remarks = dt["invoice_remarks"].ToString(),
                            overall_status = dt["overall_status"].ToString(),
                            payed_amount = dt["payed_amount"].ToString(),
                            outstanding = dt["outstanding"].ToString(),
                            due_date = dt["due_date"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),

                        });
                        values.paymentExpand = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaProductdetails(string invoice_gid, MdlPblTrnPaymentRpt values)
        {
            try
            {
                msSQL = " SELECT a.invoice_gid,a.invoicedtl_gid, a.product_gid,b.product_code,c.invoice_refno,format(a.product_total,2) as product_total , " +
                                " a.qty_invoice, b.product_name " +
                                " FROM acp_trn_tinvoicedtl a " +
                                " left join pmr_mst_tproduct b on b.product_gid = a.product_gid " +
                                " left join acp_trn_tinvoice c on a.invoice_gid=c.invoice_gid" +
                                " where a.invoice_gid = '" + invoice_gid + "'" +
                                " order by a.invoicedtl_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<productdetail_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new productdetail_list
                        {
                            invoice_gid = dt["invoice_gid"].ToString(),
                            invoicedtl_gid = dt["invoicedtl_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            product_total = dt["product_total"].ToString(),
                            qty_invoice = dt["qty_invoice"].ToString(),
                            product_name = dt["product_name"].ToString(),

                        });
                        values.productdetail_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetSinglePaymentSummary(string vendor_gid, MdlPblTrnPaymentRpt values)
        {
            try
            {
                msSQL = " select distinct a.invoice_gid, a.invoice_status, format(a.invoice_amount,2) as invoice_amount, a.vendor_gid, a.invoice_remarks,a.invoice_from," +
                        " case when d.purchaseorder_gid is null then a.invoice_reference else d.purchaseorder_gid end as purchaseorder_gid, " +
                        " date_format(a.invoice_date,'%d-%m-%Y') as invoice_date, format(a.payment_amount + a.advance_amount+a.debit_note,2) as payed_amount, b.vendor_companyname,  " +
                        " format(ABS(a.invoice_amount-(a.payment_amount+a.advance_amount+a.debit_note)),2) as outstanding ,date_format(a.payment_date,'%d-%m-%Y') as due_date from acp_trn_tinvoice a " +
                        " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid " +
                        " left join acp_trn_tpo2invoice d on a.invoice_gid = d.invoice_gid " +
                        " where a.invoice_amount<> ABS((a.payment_amount+a.advance_amount + a.debit_note)) and" +
                        " ((a.invoice_flag = 'Payment Pending' and a.payment_flag = 'PY Pending') or" +
                        " (a.invoice_flag = 'Payment Pending' and a.payment_flag = 'Payment Done Partial')) and a.invoice_from<> 'Expenses' and b.vendor_gid = '" + vendor_gid + "' " +
                        " order by a.invoice_date desc, a.invoice_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<singlepayment_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {

                        getModuleList.Add(new singlepayment_list
                        {
                            invoice_gid = dt["invoice_gid"].ToString(),
                            invoice_status = dt["invoice_status"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            invoice_remarks = dt["invoice_remarks"].ToString(),
                            invoice_from = dt["invoice_from"].ToString(),
                            purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            payed_amount = dt["payed_amount"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            outstanding = dt["outstanding"].ToString(),


                        });
                        values.singlepaymentlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void Dapaymentcancelsubmit(string payment_gid, MdlPblTrnPaymentRpt values)
        {
            try
            {
                msSQL = "select invoice_gid from acp_trn_Tinvoice2payment where payment_gid='" + payment_gid + "'";
                msSQL = " delete from acp_trn_tinvoice2payment " +
                    " where payment_gid = '" + payment_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                {
                    msSQL = " Update acp_trn_tpayment set " +
                        " payment_status = 'Payment Cancelled'" +
                        " where payment_gid = '" + payment_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Cancel Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While add Branch !!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetpaymentCancel(string payment_gid, MdlPblTrnPaymentRpt values,string user_gid)
        {
            try
            {
                msSQL = msSQL = " SELECT a.payment_gid,c.address1,d.country, a.payment_date, a.vendor_gid, a.payment_remarks, " +
                    " a.payment_mode, a.bank_name, a.branch_name, a.cheque_no, a.city_name, a.dd_no,  " +
                    " format(a.payment_total, 2) as payment_total, a.payment_reference,b.vendor_companyname,b.contactperson_name,b.contact_telephonenumber, " +
                     " b.email_id,concat(c.address1, c.city, c.postal_code, c.state, d.country) as vendoraddress,c.fax " +
                     "FROM acp_trn_tpayment a" +
                     " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid " +
                    " left join adm_mst_taddress c on b.address_gid = c.address_gid " +
                     " left join crm_trn_Tcurrencyexchange d on d.country_gid = c.country_gid where a.payment_gid = '" + payment_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                dt_datatable = objdbconn.GetDataTable(msSQL);

                msSQL = " select concat (a.user_firstname,' ',c.department_name) as Name from adm_mst_Tuser a " +
                        " left join hrm_mst_Temployee b on a.user_gid=b.user_gid left join   c " +
                        " on b.department_gid=c.department_gid where a.user_gid= '" + user_gid + "' ";
                string Name = objdbconn.GetExecuteScalar(msSQL);

                var getModuleList = new List<paymentcancel>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)

                        getModuleList.Add(new paymentcancel
                        {
                            payment_gid = dt["payment_gid"].ToString(),
                            payment_date = dt["payment_date"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            payment_remarks = dt["payment_remarks"].ToString(),
                            payment_mode = dt["payment_mode"].ToString(),
                            payment_total = dt["payment_total"].ToString(),
                            payment_reference = dt["payment_reference"].ToString(),
                            city_name = dt["city_name"].ToString(),
                            address1 = dt["address1"].ToString(),
                            country = dt["country"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            contactpersonname = dt["contactperson_name"].ToString(),
                            fax = dt["fax"].ToString(),
                            vendoraddress = dt["vendoraddress"].ToString(),
                            email_id = dt["email_id"].ToString(),
                            contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                            name = Name,

                        });
                    values.paymentcancel = getModuleList;
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetBankDetail(MdlPblTrnPaymentRpt values)
        {
            try
            {
                msSQL = "select bank_name From acc_mst_tallbank";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetBankdtldropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetBankdtldropdown
                        {

                            bank_name = dt["bank_name"].ToString(),
                        });
                        values.GetBankNameVle = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetmultipleinvoice2employeedtl(string user_gid, string vendor_gid, MdlPblTrnPaymentRpt values)
        {
            try
            {
                msSQL = " select a.vendor_gid, a.email_id,a.contact_telephonenumber,a.vendor_companyname,b.city,concat(a.contactperson_name, '', a.contact_telephonenumber) as vendorcontactdetails, " +
                        " concat(b.address1, ' ', b.address2, '', b.city, '', b.postal_code, '', b.state, '', c.country) as vendoraddress,c.currency_code,c.exchange_rate " +
                        " ,b.fax from acp_mst_tvendor a " +
                        " left join adm_mst_taddress b on a.address_gid = b.address_gid " +
                        " left join crm_trn_Tcurrencyexchange c on b.country_gid = c.country_gid where a.vendor_gid = '" + vendor_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                msSQL = " select concat (a.user_firstname,' ',c.department_name) as Name from adm_mst_Tuser a " +
                        " left join hrm_mst_Temployee b on a.user_gid=b.user_gid left join hrm_mst_Tdepartment c " +
                        " on b.department_gid=c.department_gid where a.user_gid= '" + user_gid + "' ";
                string Name = objdbconn.GetExecuteScalar(msSQL);

                var getModuleList = new List<Getmultipleinvoice2employeedtl>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getmultipleinvoice2employeedtl
                        {
                            email_id = dt["email_id"].ToString(),
                            contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            vendorcontactdetails = dt["vendorcontactdetails"].ToString(),
                            vendoraddress = dt["vendoraddress"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                            fax = dt["fax"].ToString(),
                            name = Name,
                            payment_date = "",
                            payment_remarks = "",
                            paymentnotes = "",
                            vendor_gid = dt["vendor_gid"].ToString(),
                            city = dt["city"].ToString(),

                        });
                        values.Getmultipleinvoice2employeedtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }
        public void DaGetpaymentInvoiceSummary(string vendor_gid, MdlPblTrnPaymentRpt values)
        {
            try
            {
                msSQL = " select distinct a.invoice_refno, a.invoice_gid, a.invoice_status, format(a.invoice_amount,2) as invoice_amount, a.vendor_gid, a.invoice_remarks,a.invoice_from," +
                        " case when d.purchaseorder_gid is null then a.invoice_reference else d.purchaseorder_gid end as purchaseorder_gid, " +
                        " date_format(a.invoice_date,'%d-%m-%Y') as invoice_date, format(a.payment_amount + a.advance_amount+a.debit_note,2) as payed_amount, b.vendor_companyname,  " +
                        " format(ABS(a.invoice_amount-(a.payment_amount+a.advance_amount+a.debit_note)),2) as outstanding ,date_format(a.payment_date,'%d-%m-%Y') as due_date from acp_trn_tinvoice a " +
                        " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid " + " left join acp_trn_tpo2invoice d on a.invoice_gid = d.invoice_gid " +
                        " where a.invoice_amount <> ABS((a.payment_amount+a.advance_amount+a.debit_note)) and" + " ((a.invoice_flag = 'Payment Pending' ) or " +
                        " (a.invoice_status = 'IV Completed')) and b.vendor_gid='" + vendor_gid + "' and a.invoice_from <> 'Expenses'";

                msSQL += " order by a.invoice_date desc, a.invoice_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetMultipleInvoiceSummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        if (dt["invoice_from"].ToString() == "Services")
                        {
                            msSQL = " select format(a.advance_amount,2) as advance_amount, " +
                                " format(a.advance_amount - a.advance_amount_utilized,2) outstanding_advance,b.exchange_rate,b.currency_code " +
                                " from pbl_trn_tserviceorder a " +
                                " left join acp_trn_tinvoice b on a.serviceorder_gid= b.invoice_reference " +
                                " where serviceorder_gid='" + dt["purchaseorder_gid"].ToString() + "'";
                            objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                        }
                        else
                        {
                            msSQL = " select format(purchaseorder_advance,2) as purchaseorder_advance, " +
                                " format(purchaseorder_advance - purchaseorder_advance_utilized,2) outstanding_advance, " +
                                " case when currency_code is null then 'INR' else currency_code end as currency_code," +
                                " case when exchange_rate is null then '1' else exchange_rate end as exchange_rate " +
                                " from pmr_trn_tpurchaseorder " + " where purchaseorder_gid = '" + dt["purchaseorder_gid"].ToString() + "'";
                            objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                        }
                        if (objMySqlDataReader.HasRows == true)
                        {
                            objdbconn.OpenConn();
                            lsadvance = objMySqlDataReader["advance_amount"].ToString();
                            lsoutstanding = objMySqlDataReader["outstanding_advance"].ToString();



                        }
                        if (lsadvance == null) { lsadvance = "0.0"; }
                        if (lsoutstanding == null) { lsoutstanding = "0.0"; }
                        getModuleList.Add(new GetMultipleInvoiceSummary
                        {

                            invoice_gid = dt["invoice_gid"].ToString(),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                            invoice_amount = double.Parse(dt["invoice_amount"].ToString()),
                            invoice_status = dt["invoice_status"].ToString(),
                            outstanding = double.Parse(dt["outstanding"].ToString()),
                            payed_amount = double.Parse(dt["payed_amount"].ToString()),
                            advance = double.Parse(lsadvance),
                            payment_amount = 0.00,
                            balancepo_advance = 0.00,
                            grand_total = 0.00,
                            tds_amount = 0.00,
                            final_amount = "0.0",
                            totalpo_advance = 0.00,
                            remark = "",
                        });

                        values.GetMultipleInvoiceSummary = getModuleList;
                    }


                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetCardDetail(MdlPblTrnPaymentRpt values)
        {
            try
            {
                msSQL = " select bank_gid,concat(bank_name,'/',cardholder_name) as bank_name From acc_mst_tcreditcard ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetCarddtldropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetCarddtldropdown
                        {
                            bank_gid = dt["bank_gid"].ToString(),
                            bank_name = dt["bank_name"].ToString(),
                        });
                        values.GetCardNameVle = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaPostmultipleinvoice2singlepayment(multipleinvoice2singlepayment values,string user_gid)
        {
            try
            {
                string invoice_date;
                foreach (var data in values.GetMultipleInvoiceSummary)
                {
                    double lspayment_amount = data.payment_amount;
                    double outstanding_amount = data.outstanding;
                    invoice_date = data.invoice_date;
                    double total = data.grand_total + data.advance + data.payment_amount + data.tds_amount + data.balancepo_advance;
                    if (data.invoice_amount < total)
                    {
                        values.message = "Payment amount cannot be more than invoice amount ";
                    }
                    else if (lspayment_amount > outstanding_amount)
                    {
                        values.message = "Payment amount cannot be more than outstanding amount";
                    }
                    else if (data.payment_amount == 0)
                    {
                        values.message = "Payment amount must be numeric";
                    }
                    //else if (data.balancepo_advance!=0)
                    //{
                    //    if (data.totalpo_advance==0)
                    //    {
                    //        values.message = "Please check the balance po advance amount";

                    //    }

                    //}
                    else
                    {






                        string msGetGID = objcmnfunctions.GetMasterGID("SPYC");
                        string msPYGetGID = objcmnfunctions.GetMasterGID("SPYC");



                        double lsInvoice_Amount = data.invoice_amount;
                        string lsInvoice_remarks = data.remark;
                        double lsAdvance_amount = data.advance;
                        double lspayment_amount_currency = data.payment_amount;
                        double lsinvoice_amount_currency = data.invoice_amount;
                        double lsadvance_amount_currency = data.advance;


                        msSQL = " insert into acp_trn_tpaymentdtl (" +
                               " paymentdtl_gid, " +
                               " payment_gid, " +
                               " payment_amount, " +
                               " invoice_amount, " +
                               " advance_amount, " +
                               " invoice_remarks," +
                               " payment_amount_L," +
                               " invoice_Amount_L," +
                               " advance_amount_L" + " )values ( " +
                               "'" + msGetGID + "', " +
                               "'" + msPYGetGID + "', " +
                               "'" + lspayment_amount + "', " +
                               "'" + lsInvoice_Amount + "', " +
                               "'" + lsAdvance_amount + "', " +
                               "'" + lsInvoice_remarks +
                               "'," + "'" + lspayment_amount_currency + "'," +
                               "'" + lsinvoice_amount_currency + "'," +
                               "'" + lsadvance_amount_currency + "'" + ")";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        string mspGetGID = objcmnfunctions.GetMasterGID("SIPC");

                        msSQL = " insert into acp_trn_tinvoice2payment (" +
                                " invoice2payment_gid, " +
                                " payment_gid, " +
                                " paymentdtl_gid, " +
                                " invoice_gid, " +
                                " invoice_amount, " +
                                " advance_amount, " +
                                " payment_amount," +
                                " payment_amount_L," +
                                " invoice_Amount_L," +
                                " advance_amount_L" + " )values ( " +
                                "'" + mspGetGID + "'," +
                                "'" + msPYGetGID + "'," +
                                "'" + msGetGID + "'," +
                                "'" + data.invoice_gid + "'," +
                                "'" + lsInvoice_Amount + "'," +
                                "'" + lsAdvance_amount + "'," +
                                "'" + lspayment_amount + "'," +
                                "'" + lspayment_amount_currency + "'," +
                                "'" + lsinvoice_amount_currency + "'," + "" +
                                "'" + lsadvance_amount_currency + "'" + ")";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        double lspay = data.tds_amount + lspayment_amount;
                        msSQL = "select payment_amount  from acp_trn_tinvoice  where invoice_gid = '" + data.invoice_gid + "'";
                        string isamount = objdbconn.GetExecuteScalar(msSQL);
                        if (isamount == "") { isamount = "0"; }
                        if (isamount != null)
                        {
                            double isamount1 = double.Parse(isamount);
                            lspay = lspay + isamount1;
                        }
                        msSQL = " Update acp_trn_tinvoice " +
                            " Set payment_amount = '" + lspay + "'," +
                            " advance_amount = '" + lsAdvance_amount + "'";
                        if (lspayment_amount == outstanding_amount)
                        {
                            msSQL += " ,invoice_status = 'IV Completed'";
                        }
                        else
                        {
                            msSQL += " ,invoice_status = 'IV Work In Progress'";
                        }

                        msSQL += " where invoice_gid = '" + data.invoice_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        foreach (var data1 in values.Getmultipleinvoice2employeedtl)
                        {
                            {



                                msSQL = " insert into acp_trn_tpayment (" +
                                    " payment_gid, " +
                                    " user_gid, " +
                                    " payment_date," +
                                    " created_date," +
                                    " vendor_gid, " +
                                    " payment_remarks, " +
                                    " payment_reference, " +
                                    " purchaseorder_gid, " +
                                    " payment_mode, " +
                                    " bank_name, " +
                                    " branch_name, " +
                                    " cheque_no, " +
                                    " city_name, " +
                                    " dd_no, " +
                                    " advance_total, " +
                                    " payment_total, " +
                                    " payment_status, " +
                                    " currency_code," +
                                    " exchange_rate," +
                                    " tds_amount," +
                                    " priority, " +
                                    " priority_remarks," +
                                    " bank_gid," +
                                    " payment_from," +
                                    " cheque_date," +
                                    " tdscalculated_finalamount" + " ) values (" +
                                    "'" + msPYGetGID + "'," +
                                    "'" + user_gid + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                     "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                     "'" + data1.vendor_gid + "'," +
                                     "'" + data1.payment_remarks + "'," +
                                     "'" + data1.paymentnotes + "'," +
                                     "'" + data.purchaseorder_gid + "'," +
                                     "'" + values.payment_mode + "'," +
                                     "'" + values.bankname + "'," +
                                     "'" + values.branch_name + "'," +
                                     "'" + values.cheque_no + "'," +
                                     "'" + data1.city + "'," +
                                     "'" + values.dd_no + "'," +
                                     "'" + data.advance + "'," +
                                     "'" + lspayment_amount + "'," +
                                     "'Payment Done'," +
                                     "'" + data1.currency_code + "'," +
                                     "'" + data1.exchange_rate + "'," +
                                     "'" + data.tds_amount + "'," +
                                     "'" + values.priority + "', " +
                                     "'" + values.textbox + "', " +
                                     "'" + values.bankname + "', " +
                                     "'invoice completed', " +
                                     "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                     "'" + data.tds_amount + "')";

                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                        }



                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "Payment Done Sucessfully";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error occured while Payment";
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading payment!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/pbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

    }

}