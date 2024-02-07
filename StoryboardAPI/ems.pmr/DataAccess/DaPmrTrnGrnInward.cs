﻿using ems.pmr.Models;
using ems.utilities.Functions;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Web;
using MySql.Data.MySqlClient;
using System;

namespace ems.pmr.DataAccess
{
    public class DaPmrTrnGrnInward
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
        public void DaGetGrnInwardSummary(MdlPmrTrnGrnInward values)
        {
            try
            {
                
                msSQL = " select distinct a.grn_gid,a.grn_gid as grnrefno,a.dc_no, a.grn_status, a.vendor_gid, a.grn_flag, a.invoice_flag,f.costcenter_name, " +
                    " CASE when a.invoice_flag <> 'IV Pending' then a.invoice_flag " +
                    " else a.grn_flag end as 'overall_status', " +
                    " date(a.grn_date)  as grn_date, c.vendor_companyname,a.purchaseorder_gid,format(d.total_amount,2) as po_amount,a.created_date, " +
                    " case when group_concat(distinct e.purchaserequisition_referencenumber)=',' then '' " +
                    " when group_concat(distinct e.purchaserequisition_referencenumber) <> ',' then group_concat(distinct e.purchaserequisition_referencenumber) end  as refrence_no " +
                    " from pmr_trn_tgrn a " +
                    " left join pmr_trn_tgrndtl b on a.grn_gid = b.grn_gid   " +
                    " left join acp_mst_tvendor c on a.vendor_gid = c.vendor_gid " +
                    " left join pmr_trn_tpurchaseorder d on d.purchaseorder_gid=a.purchaseorder_gid" +
                    " left join pmr_trn_tpurchaserequisition e on e.purchaserequisition_gid=d.purchaserequisition_gid" +
                    " left join pmr_mst_tcostcenter f on d.costcenter_gid=f.costcenter_gid " +
                    " where 0 = 0  group by a.grn_gid order by date(a.grn_date) desc";
            
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetGrnInward_lists>();
            
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetGrnInward_lists
                    {
                        grn_gid = dt["grn_gid"].ToString(),
                        grn_date = dt["grn_date"].ToString(),
                        grnrefno = dt["purchaseorder_gid"].ToString(),
                        refrence_no = dt["refrence_no"].ToString(),
                        vendor_companyname = dt["vendor_companyname"].ToString(),
                        costcenter_name = dt["costcenter_name"].ToString(),
                        po_amount = dt["po_amount"].ToString(),
                        created_date = dt["created_date"].ToString(),
                        invoice_flag = dt["grn_flag"].ToString(),
                        dc_no = dt["dc_no"].ToString(),
                    });
                    values.GetGrnInward_lists = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting GRN summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
             
        }
        public void DaGetEditGrnInward(string grn_gid, MdlPmrTrnGrnInward values)
            {
                try
                {
                    
                    msSQL = " select a.grn_gid,a.purchaseorder_gid,date_format(a.grn_date,'%d-%m-%Y') as grn_date,a.vendor_contact_person, " +
                    " a.checkeruser_gid,a.purchaseorder_list,a.grn_remarks,a.grn_reference,a.grn_receipt,a.grn_status, " +
                    " date_format(a.dc_date,'%d-%m-%Y') as dc_date,date_format(a.invoice_date,'%d-%m-%Y') as invoice_date,a.invoice_refno,a.dc_no," +
                    " CASE when a.invoice_flag <> 'IV Pending' then a.invoice_flag " +
                    " else a.grn_flag end as 'overall_status', a.reject_reason, " +
                    " concat(b.user_firstname,' - ',d.department_name) as user_firstname, " +
                    " i.vendor_gid,i.vendor_companyname," +
                    " i.contact_telephonenumber,i.email_id,concat(f.address1,'',f.address2) as address,concat(y.user_firstname,'  ',y.user_lastname) as user_checkername,a.priority," +
                    " a.checkeruser_gid, a.approved_by, y.user_gid,concat(z.user_firstname,'  ',z.user_lastname) as user_approvedby,CASE WHEN a.priority = 'N' THEN 'Low' ELSE 'High' END AS priority_n " +
                    " from pmr_trn_tgrn a " +
                    " left join adm_mst_tuser b on a.user_gid=b.user_gid " +
                    " left join hrm_mst_temployee c on c.user_gid = b.user_gid  " +
                    " left join adm_mst_tuser y on y.user_gid = a.checkeruser_gid " +
                    "  left join adm_mst_tuser z on z.user_gid = a.approved_by " +
                    " left join hrm_mst_tdepartment d on c.department_gid=d.department_gid " +
                    " left join acp_mst_tvendor i on i.vendor_gid = a.vendor_gid " +
                    " left join adm_mst_taddress f on i.address_gid=f.address_gid " +
                    " where a.grn_gid = '" + grn_gid + "'";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetEditGrnInward_lists>();

            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetEditGrnInward_lists
                    {
                        grn_gid = dt["grn_gid"].ToString(),
                        vendor_companyname = dt["vendor_companyname"].ToString(),
                        grn_date = dt["grn_date"].ToString(),
                        vendor_contact_person = dt["vendor_contact_person"].ToString(),
                        contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                        email_id = dt["email_id"].ToString(),
                        address = dt["address"].ToString(), 
                        purchaseorder_list = dt["purchaseorder_list"].ToString(),
                        reject_reason = dt["reject_reason"].ToString(),
                        grn_remarks = dt["grn_remarks"].ToString(),
                        dc_date = dt["dc_date"].ToString(),
                        grn_reference = dt["grn_reference"].ToString(),
                        dc_no = dt["dc_no"].ToString(),
                        invoice_refno = dt["invoice_refno"].ToString(),
                        invoice_date = dt["invoice_date"].ToString(),
                        user_checkername = dt["user_checkername"].ToString(),
                        user_approvedby = dt["user_approvedby"].ToString(),
                        priority_n = dt["priority_n"].ToString(),
                        purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                    });

                   values.GetEditGrnInward_lists = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating GRN!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
             
        }
        public void DaGetEditGrnInwardproduct(string grn_gid, MdlPmrTrnGrnInward values)
                {
                    try
                    {
                        
                        msSQL = " SELECT distinct b.grndtl_gid, a.grn_gid, a.purchaseorder_gid, format(b.qty_rejected,2) as qty_rejected, b.qc_remarks, b.product_remarks, " +
                " format(b.qty_delivered,2) as qty_received, format(b.qty_grnadjusted,2) as qty_grnadjusted, " +
                " date_format( e.purchaseorder_date,'%d-%m-%Y') as purchaseorder_date,b.product_remarks,a.priority, " +
                " b.product_gid, b.product_code, b.product_name, d.productgroup_name,b.productuom_name, format(f.qty_ordered,2) as qty_ordered, " +
                " h.location_name,k.bin_number FROM pmr_trn_tgrn a " +
                " left join pmr_trn_tgrndtl b on a.grn_gid = b.grn_gid " +
                " left join pmr_mst_tproduct c on b.product_gid = c.product_gid " +
                " left join pmr_mst_tproductgroup d on d.productgroup_gid = c.productgroup_gid " +
                " left join pmr_trn_tpurchaseorder e on a.purchaseorder_gid = e.purchaseorder_gid " +
                " left join pmr_trn_tpurchaseorderdtl f on f.purchaseorderdtl_gid = b.purchaseorderdtl_gid " +
                " left join pmr_mst_tproductuom g on b.uom_gid = g.productuom_gid " +
                " left join ims_mst_tlocation h on b.location_gid=h.location_gid " +
                " left join ims_mst_tbin k on b.bin_gid=k.bin_gid" +
                " where a.grn_gid = '" + grn_gid + "'";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetEditGrnInwardproduct_lists>();

            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetEditGrnInwardproduct_lists
                    {
                        productgroup_name = dt["productgroup_name"].ToString(),
                        productuom_gid = dt["productuom_name"].ToString(),
                        product_code = dt["product_code"].ToString(),
                        product_name = dt["product_name"].ToString(),
                        qty_ordered = dt["qty_ordered"].ToString(),
                        qty_received = dt["qty_received"].ToString(),
                        qty_grnadjusted = dt["qty_grnadjusted"].ToString(),
                        qty_rejected = dt["qty_rejected"].ToString(),
                        location_name = dt["location_name"].ToString(),
                        bin_number = dt["bin_number"].ToString(),
                        product_remarks = dt["product_remarks"].ToString(),
                        qc_remarks = dt["qc_remarks"].ToString(),
                      
                    });

                    values.GetEditGrnInwardproduct_lists = getModuleList;
                }
            }
            dt_datatable.Dispose();
                }
                catch (Exception ex)
                {
                    values.message = "Exception occured while updating product in GRN!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
               
            }
        public void DaGetPurchaseOrderDetails(string purchaseorder_gid, MdlPmrTrnGrnInward values)
                    {
                        try
                        { 
                            msSQL = " select a.purchaseorderdtl_gid, a.product_gid, a.purchaseorder_gid, c.productgroup_name, b.product_code, b.product_name, d.productuom_name, a.qty_ordered, " +
                    " format(a.product_price,2) as product_price, format(a.discount_percentage,2) as discount_percentage , format(a.discount_amount,2) as discount_amount, " +
                    " tax_name, format(a.tax_percentage,2) as tax_percentage, format(a.tax_amount,2) as tax_amount, " +
                    " tax_name2, format(a.tax_percentage2,2) as tax_percentage2, format(a.tax_amount2,2) as tax_amount2, concat(a.tax_name,' - ', a.tax_amount, ' / ', a.tax_name2,' - ', a.tax_amount2) as tax, " +
                    " format((((qty_ordered * a.product_price) - a.discount_amount) + a.tax_amount + a.tax_amount2),2) as product_total, " +
                    " e.payment_days, e.delivery_days, format(e.total_amount,2) as total_amount, format((a.tax_amount) + (a.tax_amount2),2) as total_tax, e.discount_amount, e.addon_amount, e.freight_charges, e.buybackorscrap, format(e.total_amount,2) as grand_total, e.currency_code from pmr_trn_tpurchaseorderdtl a " +
                    " left join pmr_mst_tproduct b on a.product_gid = b.product_gid " +
                    " left join pmr_mst_tproductgroup c on b.productgroup_gid = c.productgroup_gid " +
                    " left join pmr_mst_tproductuom d on a.uom_gid = d.productuom_gid " +
                    " left join pmr_trn_tpurchaseorder e on a.purchaseorder_gid = e.purchaseorder_gid " +
                    " where a.purchaseorder_gid = '" + purchaseorder_gid + "'" + " " +
                    " group by a.product_gid, a.uom_gid, a.display_field_name order by b.product_name ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<Getpurchaseorder_list>();

            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new Getpurchaseorder_list
                    {
                        purchaseorderdtl_gid = dt["purchaseorderdtl_gid"].ToString(),
                        product_gid = dt["product_gid"].ToString(),
                        productgroup_name = dt["productgroup_name"].ToString(),
                        product_code = dt["product_code"].ToString(),
                        product_name = dt["product_name"].ToString(),
                        productuom_name = dt["productuom_name"].ToString(),
                        qty_ordered = dt["qty_ordered"].ToString(),
                        product_price = dt["product_price"].ToString(),
                        discount_percentage = dt["discount_percentage"].ToString(),
                        discount_amount = dt["discount_amount"].ToString(),
                        tax = dt["tax"].ToString(),                        
                        product_total = dt["product_total"].ToString(),
                        payment_days = dt["payment_days"].ToString(),
                        delivery_days = dt["delivery_days"].ToString(),
                        total_amount = dt["total_amount"].ToString(),
                        total_tax = dt["total_tax"].ToString(),
                        total_discount_amount = dt["discount_amount"].ToString(),
                        addon_amount = dt["addon_amount"].ToString(),
                        freight_charges = dt["freight_charges"].ToString(),
                        buybackorscrap = dt["buybackorscrap"].ToString(),
                        grand_total = dt["grand_total"].ToString(),
                        currency_code = dt["currency_code"].ToString(),
                    });
                    values.Getpurchaseorder_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting PO details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
    }
}
