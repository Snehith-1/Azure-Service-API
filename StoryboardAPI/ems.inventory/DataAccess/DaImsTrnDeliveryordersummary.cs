﻿using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.inventory.Models;
using OfficeOpenXml.FormulaParsing.Excel.Operators;
using System.Windows.Media.Media3D;
using System.Runtime.Remoting;
using System.Globalization;
using MySql.Data.MySqlClient;

namespace ems.inventory.DataAccess
{

    public class DaImsTrnDeliveryordersummary
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        MySqlDataReader objMySqlDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsuom_gid, lsbranch_gid, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, msstockdtlGid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        string mssalesorderGID;

        public void DaGetImsTrnDeliveryorderSummary(MdlImsTrnDeliveryordersummary values)
        {
            try
            {                
                msSQL = " select distinct a.directorder_gid,s.branch_name,cast(concat(c.so_referenceno1," +
                " if(c.so_referencenumber<>'',concat(' | ',c.so_referencenumber),'') ) as char)as so_referenceno1," +
                " directorder_refno, date_format(directorder_date, '%d-%m-%Y') as directorder_date, n.user_firstname, a.dc_no,a.salesorder_gid, " +
                " a.customer_name, customer_branchname, customer_contactperson, directorder_status,delivery_status, " +
                " concat(CAST(date_format(delivered_date,'%d-%m-%Y') as CHAR),'/',delivered_to) as delivery_details, " +
                " case when a.customer_contactnumber is null then concat(e.customercontact_name,'/',e.mobile,'/',e.email) " +
                " when a.customer_contactnumber is not null then concat(a.customer_contactperson,' / ',a.customer_contactnumber,' / ',a.customer_emailid) end as contact" +
                " from smr_trn_tdeliveryorder a " +
                " left join crm_mst_tcustomercontact e on e.customer_gid = a.customer_gid " +
                " left join hrm_mst_temployee m on m.employee_gid=a.created_name " +
                " left join hrm_mst_tbranch s on s.branch_gid=a.customerbranch_gid " +
                " left join adm_mst_tuser n on n.user_gid= m.user_gid " +
                " left join smr_trn_tdeliveryorderdtl b on a.directorder_gid =b.directorder_gid " +
                " left join smr_trn_tsalesorder c on a.salesorder_gid=c.salesorder_gid " +
                " where dc_type<>'Direct DC' " +
                " order by a.directorder_date DESC,a.directorder_gid desc ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<deliveryorder_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new deliveryorder_list
                    {

                        directorder_date = dt["directorder_date"].ToString(),
                        directorder_refno = dt["directorder_refno"].ToString(),
                        so_referenceno1 = dt["so_referenceno1"].ToString(),
                        customer_name = dt["customer_name"].ToString(),
                        contact = dt["contact"].ToString(),
                        branch_name = dt["customer_branchname"].ToString(),
                        delivery_status = dt["delivery_status"].ToString(),
                        user_firstname = dt["user_firstname"].ToString(),
                        dc_no = dt["dc_no"].ToString(),
                        directorder_gid = dt["directorder_gid"].ToString()

                    });
                    values.deliveryorder_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Delivery Order Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        public void DaGetImsTrnAddDeliveryorderSummary(MdlImsTrnDeliveryordersummary values)
        {
            try
            {               
                msSQL = " select distinct a.salesorder_gid,y.branch_name, cast(concat(a.so_referenceno1," +
               " if(a.so_referencenumber<>'',concat(' | ',a.so_referencenumber),'') ) as char)as so_referenceno1, date_format(a.salesorder_date,'%d-%m-%Y') as salesorder_date, " +
                " sum(b.qty_quoted) as qty_quoted,sum(b.product_delivered) as product_delivered," +
                " a.customer_name,  a.customer_contact_person, a.salesorder_status,c.mobile, " +
                " a.despatch_status, " +
                " case when a.customer_email is null then concat(c.customercontact_name,'/',c.mobile,'/',c.email) " +
                " when a.customer_email is not null then concat(a.customer_contact_person,' / ',a.customer_mobile,' / ',a.customer_email) end as contact " +
                " from smr_trn_tsalesorder a " +
                " left join smr_trn_tsalesorderdtl b on b.salesorder_gid = a.salesorder_gid " +
                " left join crm_mst_tcustomercontact c on c.customer_gid=a.customer_gid " +
                " left join hrm_mst_tbranch y on y.branch_gid=a.branch_gid" +
                 " group by salesorder_gid " +
                 " having(qty_quoted <> product_delivered)  order by a.salesorder_date desc, a.customer_name desc ";


            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<adddeliveryorder_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new adddeliveryorder_list
                    {
                        salesorder_date = dt["salesorder_date"].ToString(),
                        salesorder_gid = dt["salesorder_gid"].ToString(),
                        so_referenceno1 = dt["so_referenceno1"].ToString(),
                        customer_name = dt["customer_name"].ToString(),
                        contact = dt["contact"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        salesorder_status = dt["salesorder_status"].ToString(),

                    });
                    values.adddeliveryorder_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Adding Delivery Order !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        public void DaGetRaiseDeliveryorderSummary(string salesorder_gid, MdlImsTrnDeliveryordersummary values)
        {
            try
            {
               
                msSQL = " select a.salesorder_gid,concat(a.so_referenceno1,case when so_referencenumber='' then '' else concat(' ','-',' ',so_referencenumber) end )as so_reference," +
                    " DATE_FORMAT(a.salesorder_date, '%d-%m-%Y') as salesorder_date,a.termsandconditions,b.customer_gid,b.customer_code,format(a.grandtotal,2) as grandtotal, a.customer_name, c.customerbranch_name ," +
                    " concat(b.customer_address,b.customer_address2,b.customer_city,b.customer_state,b.customer_pin) as customer_address," +
                    " c.designation,c.customercontact_name,c.email,c.mobile,a.currency_code,a.shipping_to, " +
                    "  a.customer_mobile,a.customer_email,a.customer_address as customer_address_so, " +
                    " a.customer_contact_person,a.shipping_to from smr_trn_tsalesorder a" +
                    " left join crm_mst_tcustomer b on b.customer_gid=a.customer_gid " +
                    " left join crm_mst_tcustomercontact c on c.customer_gid=a.customer_gid " +
                    " where a.salesorder_gid='" + salesorder_gid + "' ";


            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<raisedelivery_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new raisedelivery_list
                    {

                        salesorder_gid = dt["salesorder_gid"].ToString(),
                        salesorder_date = dt["salesorder_date"].ToString(),
                        customer_mobile = dt["customer_mobile"].ToString(),
                        customer_name = dt["customer_name"].ToString(),
                        customer_branchname = dt["customer_name"].ToString(),
                        customer_branch = dt["customerbranch_name"].ToString(),
                        customercontact_names = dt["customercontact_name"].ToString(),
                        customer_email = dt["customer_email"].ToString(),
                        customer_address_so = dt["customer_address_so"].ToString(),
                        customer_address = dt["customer_address_so"].ToString(),
                        so_referencenumber = dt["so_reference"].ToString(),


                    });
                    values.raisedelivery_list = getModuleList;
                }
            }

            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Raising Deilvery Order !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        public void DaGetProductdelivery(string salesorder_gid, MdlImsTrnDeliveryordersummary values)
        {
            try
            {

                msSQL = "select distinct y.branch_name from smr_trn_tsalesorder a " +
                  "left join crm_mst_tcustomercontact c on c.customer_gid = a.customer_gid " +
                  "left join hrm_mst_tbranch y on y.branch_gid = a.branch_gid " +
                    " where salesorder_gid = '" + salesorder_gid + "' ";
            string branch_name = objdbconn.GetExecuteScalar(msSQL);

            msSQL = "select branch_gid from hrm_mst_tbranch where branch_name = '" + branch_name + "' ";
            string branch_gid = objdbconn.GetExecuteScalar(msSQL);


            msSQL = " select a.salesorderdtl_gid,c.design_no,c.color_name,a.salesorder_gid,a.product_gid,date_format(a.product_requireddate, '%d-%m-%Y') as product_requireddate,z.productgroup_gid,z.productgroup_name,a.product_name,a.uom_gid,a.uom_name,a.qty_quoted," +
                    " a.display_field,a.product_delivered,format(a.product_price,2) as product_price, a.discount_percentage,format(a.discount_amount,2) as discount_amount," +
                    " format(a.tax_amount,2) as tax_amount,format(a.tax_amount2,2) as tax_amount2,format(a.tax_amount3,2) as tax_amount3, " +
                    " a.tax_name,a.tax_name2,a.tax_name3,format(a.price,2) as price,b.stockable,a.customerproduct_code, " +
                    " (select ifnull(sum(m.stock_qty)+sum(m.amend_qty)-sum(m.damaged_qty)-sum(m.issued_qty)-sum(m.transfer_qty),0) as available_quantity from " +
                    "  ims_trn_tstock m where m.stock_flag='Y' and m.product_gid=a.product_gid and m.branch_gid='" + branch_gid + " ' and " +
                    "  m.uom_gid=a.uom_gid) as available_quantity,b.serial_flag,b.producttype_gid, " +
                    " a.tax1_gid,a.tax2_gid,a.tax3_gid,b.product_code " +
                    " from smr_trn_tsalesorderdtl a " +
                    " left join pmr_mst_tproduct b on a.product_gid=b.product_gid " +
                    " left join pmr_mst_tproductgroup z on b.productgroup_gid=z.productgroup_gid"+
                    " left join acp_trn_torderdtl c on c.salesorderdtl_gid=a.salesorderdtl_gid " +
                    " where a.salesorder_gid = '" + salesorder_gid + "'" +
                    " group by a.salesorderdtl_gid order by a.salesorderdtl_gid asc  ";


            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<raisedelivery_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new raisedelivery_list
                    {

                        salesorder_gid = dt["salesorder_gid"].ToString(),
                        productgroup_name = dt["productgroup_name"].ToString(),
                        product_gid = dt["product_gid"].ToString(),
                        customerproduct_code = dt["customerproduct_code"].ToString(),
                        product_code = dt["product_code"].ToString(),
                        product_name = dt["product_name"].ToString(),
                        display_field = dt["display_field"].ToString(),
                        uom_name = dt["uom_name"].ToString(),
                        uom_gid = dt["uom_gid"].ToString(),
                        available_quantity = dt["available_quantity"].ToString(),
                        qty_quoted = dt["qty_quoted"].ToString(),
                        product_delivered = dt["product_delivered"].ToString(),
                        product_requireddate = dt["product_requireddate"].ToString(),


                    });
                    values.raisedelivery_list = getModuleList;
                }
            }

            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product Delivery !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
        public void DaGetOutstandingQty(string salesorderdtl_gid, MdlImsTrnDeliveryordersummary values)
        {
            try
            {
             
                msSQL = "select salesorderdtl_gid from smr_trn_tsalesorderdtl where salesorder_gid = '" + salesorderdtl_gid + "' ";
            string lssalesorderdtl_gid = objdbconn.GetExecuteScalar(msSQL);


            msSQL = "select (a.qty_quoted-a.product_delivered) as outstanding_qty,a.product_gid,a.uom_gid," +
                " a.display_field,b.product_name,c.productuom_name,b.branch_gid from smr_trn_tsalesorderdtl a" +
                " left join pmr_mst_tproduct b on a.product_gid=b.product_gid" +
                " left join pmr_mst_tproductuom c on a.uom_gid=c.productuom_gid" +
                " where  a.salesorderdtl_gid = '" + lssalesorderdtl_gid + "' group by a.salesorderdtl_gid ";


            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<OutstandingQty_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new OutstandingQty_list
                    {

                        outstanding_qty = dt["outstanding_qty"].ToString(),
                        product_name = dt["product_name"].ToString(),
                        uom_name = dt["productuom_name"].ToString(),
                        display_field = dt["display_field"].ToString(),
                        product_gid = dt["product_gid"].ToString(),
                        uom_gid = dt["uom_gid"].ToString(),
                        branch_gid = dt["branch_gid"].ToString(),


                    });
                    values.OutstandingQty_list = getModuleList;
                }
            }

            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Outstanding Quantity !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
          
        }
        public void DaIssueFromStock(string product_gid, string salesorder_gid, MdlImsTrnDeliveryordersummary values)
        {
            try
            {
                
                msSQL = "select salesorderdtl_gid from smr_trn_tsalesorderdtl where salesorder_gid = '" + salesorder_gid + "' ";
            string lssalesorderdtl_gid = objdbconn.GetExecuteScalar(msSQL);

            msSQL = "select y.branch_gid ,a.uom_gid from smr_trn_tsalesorderdtl a " +
                    "left join smr_trn_tsalesorder b on a.salesorder_gid = b.salesorder_gid " +
                    "left join crm_mst_tcustomercontact c on c.customer_gid = b.customer_gid " +
                    "left join hrm_mst_tbranch y on y.branch_gid = b.branch_gid " +
                    "where salesorderdtl_gid = '" + lssalesorderdtl_gid + "'";
            objMySqlDataReader = objdbconn.GetDataReader(msSQL);
            if (objMySqlDataReader.HasRows == true)
            {
                objMySqlDataReader.Read();
                lsuom_gid = objMySqlDataReader["uom_gid"].ToString();
                lsbranch_gid = objMySqlDataReader["branch_gid"].ToString();
                objMySqlDataReader.Close();
            }

            msSQL = " select a.created_date,a.stock_gid,a.product_gid,a.display_field,a.uom_gid,a.reference_gid," +
                      " sum(a.stock_qty+amend_qty-a.issued_qty-damaged_qty-transfer_qty)as stock_qty,b.product_name,c.productuom_name" +
                      " from ims_trn_tstock a" +
                      " left join pmr_mst_tproduct b on a.product_gid=b.product_gid" +
                      " left join pmr_mst_tproductuom c on a.uom_gid=c.productuom_gid" +
                        " where a.product_gid='" + product_gid + "'" +
                        " and a.uom_gid='" + lsuom_gid + "' and a.stock_flag='Y'" +
                        " and a.branch_gid='" + lsbranch_gid + "' " +
                        " order by date(a.created_date) asc,a.created_date desc ";


            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<IssuedQty_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new IssuedQty_list
                    {

                        created_date = dt["created_date"].ToString(),
                        stock_gid = dt["stock_gid"].ToString(),
                        product_gid = dt["product_gid"].ToString(),
                        display = dt["display_field"].ToString(),
                        uom_gid = dt["uom_gid"].ToString(),
                        reference_gid = dt["reference_gid"].ToString(),
                        stock_qty = dt["stock_qty"].ToString(),
                        product_name = dt["product_name"].ToString(),
                        uom_name = dt["productuom_name"].ToString(),


                    });
                    values.IssuedQty_list = getModuleList;
                }
            }

            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Issue From Stock !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
        public void DaPostSelectIssueQtySubmit(string employee_gid, IssuedQty_list values)
        {
            try
            {
                

                msSQL = " select display_field from smr_trn_tsalesorderdtl where salesorder_gid='" + values.salesorder_gid + "' and product_gid='" + values.product_gid + "'";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            {
                if (Convert.ToDouble(values.total_amount) <= Convert.ToDouble(values.outstanding_qty))
                {
                    if (Convert.ToDouble(values.total_amount) <= Convert.ToDouble(values.stock_qty))
                    {
                       

                        msSQL = " insert into ims_tmp_tstock(" +
                            " stock_gid," +
                            " salesorderdtl_gid, " +
                            " product_gid," +
                            " stock_quantity," +
                            " created_by," +
                            " created_date," +
                            " branch_gid," +
                            " productuom_gid," +
                            " mrdtl_gid," +
                            " display_field" + ") " +
                            "values (" +
                            "'" + values.stock_gid + "'," +
                             "'" + values.salesorder_gid + "'," +
                            "'" + values.product_gid + "'," +
                            "'" + values.total_amount + "'," +
                            "'" + employee_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                            "'" + values.branch_gid + "'," +
                            "'" + values.uom_gid + "'," +
                            "'" + values.mrdtl_gid + "'," +
                            "'" + values.display_field + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    else
                    {
                        values.message = "Issue Quantity must be Less than or equal to Actual  Quantity";
                    }

                }
                else
                {
                    values.message = "Issue Quantity must be Less than or equal to Outstanding Quantity";

                }

                values.txtstocktotal = "0.00";
            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Submitting Issue Quantity !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           

        }

        // Overall Submit 

        string xDate, createDate, lssendername, lssenderdesignation, lssender_contactnumber, lsbranch, lsproductcode, lsproduct, lsuomname, lsstockgid, lsproductgid, lsproductuomgid, lsstockquantity;
        public void DaPostDeliveryOrderSubmit(MdlImsTrnDeliveryorder values, string employee_gid)
        {
            try
            {
                
                msSQL = " select * from hrm_mst_temployee where employee_gid='" + employee_gid + "' ";
            objMySqlDataReader = objdbconn.GetDataReader(msSQL);
            if (objMySqlDataReader.HasRows)
            {
                objMySqlDataReader.Read();
                lssendername = objMySqlDataReader["employee_gid"].ToString();
                lssenderdesignation = objMySqlDataReader["designation_gid"].ToString();
                lssender_contactnumber = objMySqlDataReader["employee_mobileno"].ToString();
                lsbranch = objMySqlDataReader["branch_gid"].ToString();
                objMySqlDataReader.Close();
            }

                string uiDateStr2 = values.salesorder_date;
                DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string salesorder_date = uiDate2.ToString("yyyy-MM-dd");

                mssalesorderGID = objcmnfunctions.GetMasterGID("VDOP");

            msSQL = " insert into smr_trn_tdeliveryorder (" +
                " directorder_gid, " +
                " directorder_date," +
                " directorder_refno, " +
                " salesorder_gid, " +
                " customer_gid, " +
                " customer_name , " +
                " customerbranch_gid, " +
                " customer_branchname, " +
                " customer_contactperson, " +
                " customer_contactnumber, " +
                " customer_address, " +
                " directorder_status, " +
                " terms_condition, " +
                " created_date, " +
                " created_name, " +
                " sender_name," +
                " delivered_by," +
                " dc_no," +
                " mode_of_despatch, " +
                " tracker_id, " +
                " sender_designation," +
                " sender_contactnumber, " +
                " grandtotal_amount, " +
                " delivered_date," +
                " shipping_to, " +
                " customer_emailid " +
                " ) values (" +
                "'" + mssalesorderGID + "'," +
                "'" + salesorder_date + "'," +
                "'" + mssalesorderGID + "'," +
                "'" + values.salesorder_gid + "'," +
                "'" + values.customer_gid + "'," +
                "'" + values.customer_name + "'," +
                "'" + values.branch_gid + "'," +
                "'" + values.customer_name + "'," +
                "'" + values.customercontact_names + "'," +
                "'" + values.customer_mobile + "'," +
                " '" + values.customer_address + "'," +
                "'Despatch Done'," +
                "'" + values.template_content + "', " +
                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                "'" + employee_gid + "'," +
                "'" + lssendername + "'," +
                "'" + employee_gid + "'," +
                "'" + values.dc_no + "'," +
                "'" + values.customer_mode + "'," +
                "'" + values.customer_city + "'," +
                "'" + lssenderdesignation + "'," +
                "'" + lssender_contactnumber + "',";
            if (values.grandtotalamount == null || values.grandtotalamount == "")
            {
                msSQL += "'0.00',";
            }
            else
            {
                msSQL += "'" + values.grandtotalamount + "',";
            }


            msSQL += "'" +salesorder_date + "'," +
            "'" + values.customer_address_so + "'," +
            "'" + values.customer_email + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            if (mnResult == 0)
            {
                values.message = "Error occurred while inserting Records!";
            }


            msGetGid = objcmnfunctions.GetMasterGID("VDDC");

            msSQL = " update smr_trn_tsalesorderdtl set product_delivered='" + values.product_delivered + "' " +
                             " where salesorderdtl_gid='" + values.salesorderdtl_gid + "'";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult == 0)
            {
                values.status = false;
            }

            msSQL = " SELECT, a.salesorderdtl_gid, a.product_name, a.product_code, a.uom_name, " +
                    " a.uom_gid, a.product_gid, a.productgroup_gid,a.delivery_quantity, a.price, a.display_field, " +
                    " a.product_price, " +
                    " (SELECT IFNULL(SUM(b.stock_qty) - SUM(b.amend_qty) - SUM(b.damaged_qty) - SUM(b.issued_qty) - SUM(b.transfer_qty), 0) " +
                    " FROM ims_trn_tstock b " +
                    " WHERE a.product_gid = b.product_gid) AS available_quantity,"  +
                    " a.qty_quoted " +
                    " FROM smr_trn_tsalesorderdtl a " +
                    " WHERE a.product_gid = '" + values.product_gid + "'";


                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
            if (objMySqlDataReader.HasRows)
            {
                objMySqlDataReader.Read();
                lsproductcode = objMySqlDataReader["product_code"].ToString();
                lsproduct = objMySqlDataReader["product_name"].ToString();
                lsuomname = objMySqlDataReader["uom_name"].ToString();
                lsuom_gid = objMySqlDataReader["uom_gid"].ToString();
                objMySqlDataReader.Close();
            }
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {

                        msSQL = " insert into smr_trn_tdeliveryorderdtl (" +
                           " directorderdtl_gid, " +
                           " directorder_gid, " +                          
                           " productgroup_gid, " +
                           " productgroup_name, " +
                           " product_gid," +
                           " product_name, " +
                           " product_code, " +
                           " product_uom_gid, " +
                           " productuom_name, " +
                           " product_qty, " +
                           " product_description, " +
                           " product_price, " +
                           " product_total, " +
                           " salesorderdtl_gid," +
                           " product_qtydelivered " +
                           "  ) " +
                           " values ( " +
                           "'" + msGetGid + "', " +
                           "'" + mssalesorderGID + "'," +                           
                           "'" + dt["productgroup_gid"] + "', " +
                           "'" + dt["productgroup_name"] + "', " +
                           "'" + dt["product_gid"] + "', " +
                           "'" + lsproduct + "', " +
                           "'" + lsproductcode + "', " +
                           "'" + lsuom_gid + "', " +
                           "'" + lsuomname + "'," +
                           "'" + dt["available_quantity"] + "', " +
                           "'" + dt["display_field"] + "',";
                        if (dt["price"].ToString() == null || DBNull.Value.Equals(dt["price"].ToString()))
                        {
                            msSQL += "null,";
                        }
                        else
                        {
                            msSQL += "'" + dt["price"] + "',";


                        }

                        if (dt["product_price"].ToString() == null || DBNull.Value.Equals(dt["product_price"].ToString()))
                        {
                            msSQL += "null,";
                        }
                        else
                        {
                            msSQL += "'" + dt["product_price"] + "',";
                        }


                        msSQL += "'" + dt["salesorderdtl_gid"] + "'," +
                                 "'" + dt["product_delivered"] + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }
            if (mnResult == 0)
            {
                values.status = false;
            }

            msSQL = " select * from ims_tmp_tstock where created_by='" + employee_gid + "'";
            objMySqlDataReader = objdbconn.GetDataReader(msSQL);
            if (objMySqlDataReader.HasRows)
            {
                objMySqlDataReader.Read();
                lsstockgid = objMySqlDataReader["stock_gid"].ToString();
                lsproductgid = objMySqlDataReader["product_gid"].ToString();
                lsproductuomgid = objMySqlDataReader["productuom_gid"].ToString();
                lsstockquantity = objMySqlDataReader["stock_quantity"].ToString();
                objMySqlDataReader.Close();
            }



            msstockdtlGid = objcmnfunctions.GetMasterGID("ISTP");

            msSQL = "insert into ims_trn_tstockdtl(" +
                       "stockdtl_gid," +
                       "stock_gid," +
                       "branch_gid," +
                       "product_gid," +
                       "uom_gid," +
                       "issued_qty," +
                       "amend_qty," +
                       "damaged_qty," +
                       "adjusted_qty," +
                       "transfer_qty," +
                       "return_qty," +
                       "reference_gid," +
                       "stock_type," +
                       "remarks," +
                       "created_by," +
                       "created_date," +
                       "display_field" +
                       ") values ( " +
                        "'" + msstockdtlGid + "'," +
                       "'" + lsstockgid + "'," +
                       "'" + lsbranch + "'," +
                       "'" + lsproductgid + "'," +
                       "'" + lsproductuomgid + "',";
                        if (lsstockquantity == null || lsstockquantity == "")
                        {
                            msSQL += "'0.00',";
                        }
                        else
                        {
                            msSQL += "'" + lsstockquantity + "',";

                        }
            msSQL += "'0.00'," +
                       "'0.00'," +
                       "'0.00'," +
                       "'0.00'," +
                       "'0.00'," +
                       "'" + mssalesorderGID + "'," +
                       "'Delivery'," +
                       "''," +
                       "'" + employee_gid + "'," +
                       "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                       "'')";

            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


            msSQL = " update ims_trn_tstock set " +
                    " issued_qty= issued_qty + '" + lsstockquantity + "' " +
                    " where stock_gid='" + lsstockgid + "'";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);



            msSQL = "select distinct  " +
                " sum(qty_quoted) as qty_quoted,sum(product_delivered) as product_delivered " +
                " from smr_trn_tsalesorderdtl where salesorder_gid='" + values.salesorder_gid + "' group by salesorder_gid " +
                " having(qty_quoted <> product_delivered) ";
            objMySqlDataReader = objdbconn.GetDataReader(msSQL);

            if (objMySqlDataReader.HasRows == true)
            {
                msSQL = " update smr_trn_tsalesorder " +
                    " set salesorder_status= 'Delivery Done Partial' where " +
                    " salesorder_gid = '" + values.salesorder_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            else
            {
                msSQL = " update smr_trn_tsalesorder " +
                    " set salesorder_status= 'Delivery Completed' where " +
                    " salesorder_gid = '" + values.salesorder_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            if(mnResult == 0)
                {
                    values.status = false;
                    values.message = "Error While Raising Delivery Order";

                }
                else
                {
                    values.status = true;
                    values.message = "Delivery Order Raised Successfully.";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Submitting Delivery Order !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           

        }
        public void DaGetImsTrnDeliveryorderSummaryView(string directorder_gid, MdlImsTrnDeliveryordersummary values)
        {
            try
            {
               
                msSQL = "select a.directorder_refno," +
                    " date_format(a.directorder_date,'%d/%m/%Y') as directorder_date, " +
                    "a.customer_name," +
                    "a.customer_branchname ," +
                    "a.customer_contactperson," +
                    " a.customer_address,  " +
                    " a.directorder_remarks," +
                    " a.terms_condition, " +
                    " a.Landline_no, " +
                    " a.customer_department,  " +
                    " a.grandtotal_amount,  " +
                    " a.addon_amount,  " +
                    " a.shipping_to, " +
                    " a.dc_no, " +
                    " a.mode_of_despatch, " +
                    " a.tracker_id, " +
                    " case when a.customer_contactnumber is null then b.mobile when a.customer_contactnumber is not null then a.customer_contactnumber end as mobile," +
                    " a.customer_emailid,b.designation " +
                    " from smr_trn_tdeliveryorder a " +
                    " left join smr_trn_tdeliveryorderdtl c on c.directorder_gid=a.directorder_gid " +
                    " left join crm_mst_tcustomercontact b on b.customer_gid=a.customer_gid " +
                    " where a.directorder_gid = '" + directorder_gid + "' group by a.directorder_gid ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<deliveryorderview_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new deliveryorderview_list
                    {

                        directorder_date = dt["directorder_date"].ToString(),
                        directorder_refno = dt["directorder_refno"].ToString(),
                        customer_name = dt["customer_name"].ToString(),
                        mobile = dt["mobile"].ToString(),
                        customer_address = dt["customer_address"].ToString(),
                        shipping_to = dt["shipping_to"].ToString(),
                        customer_emailid = dt["customer_emailid"].ToString(),
                        dc_no = dt["dc_no"].ToString(),
                        tracker_id = dt["tracker_id"].ToString(),
                        directorder_remarks = dt["directorder_remarks"].ToString(),
                        customer_contactperson = dt["customer_contactperson"].ToString(),
                        mode_of_despatch = dt["mode_of_despatch"].ToString(),
                        terms_condition = dt["terms_condition"].ToString()

                    });
                    values.deliveryorderview_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Delivery Order View !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
        public void DaGetImsTrnDeliveryorderProductView(string directorder_gid, MdlImsTrnDeliveryordersummary values)
        {
            try
            {
                
                msSQL = "select a.productgroup_name,d.design_no,d.color_name," +
                " concat(a.product_code,' ','-',' ',a.product_name) as product_code, " +
                " a.product_name," +
                " a.product_description," +
                " a.productuom_name," +
                " a.product_qty as product_qty, " +
                " (a.product_qtydelivered-a.qty_returned) as product_qtydelivered, " +
                " (a.qty_returned) as qty_return, " +
                " (a.product_qtydelivered) as total_qty_delivered, " +
                " format(a.discount_amount,2) as discount_amount, " +
                " format(a.tax_amount,2) as tax_amount, " +
                " format(a.product_total,2) as product_total,e.customerproduct_code as customerproduct_code " +
                " from smr_trn_tdeliveryorderdtl a " +
                " left join pmr_mst_tproduct b on a.product_gid= b.product_gid " +
                " left join smr_trn_tdeliveryorder c on a.directorder_gid=c.directorder_gid " +
                " left join acp_trn_torderdtl d on d.salesorder_gid=c.salesorder_gid " +
                " inner join smr_trn_tsalesorderdtl e on a.salesorderdtl_gid=e.salesorderdtl_gid " +
                " where a.directorder_gid = '" + directorder_gid + "' group by directorderdtl_gid order by directorderdtl_gid asc ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<deliveryorderview_list1>();
            if (dt_datatable.Rows.Count > 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new deliveryorderview_list1
                    {

                        product_name = dt["product_name"].ToString(),
                        product_code = dt["product_code"].ToString(),
                        productuom_name = dt["productuom_name"].ToString(),
                        total_qty_delivered = dt["total_qty_delivered"].ToString(),
                        product_qty = dt["product_qty"].ToString(),
                        product_qtydelivered = dt["product_qtydelivered"].ToString(),
                        qty_return = dt["qty_return"].ToString(),


                    });
                    values.deliveryorderview_list1 = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Delivery Order Product View!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
    }
}