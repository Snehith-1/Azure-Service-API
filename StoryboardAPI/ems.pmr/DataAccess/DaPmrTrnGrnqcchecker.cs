﻿using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.pmr.Models;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;


namespace ems.pmr.DataAccess
{
    public class DaPmrTrnGrnQcchecker
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        MySqlDataReader objMySqlDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lssplit_flag, lsentity_code, lsbaseuom_gid, lsordereduom_sequencelevel, lsordereduom_conversionrate, lsreceiveduom_sequencelevel,
            lsreceiveduom_conversionrate, lsShortageSUM, lsSUM, dggrnchecker, FindControl, blnStatus, lsgrn_qty, lsPO_Sum_Received, lsTotal, lsqty_billed, lsproduct_gid, lsgrn_gid,
            lsreceiveduom_gid, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetGid2, msGetPrivilege_gid, msGetModule2employee_gid, maGetGID, lsvendor_code, msUserGid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        double qtyDelivered, qtyRejected, qtyShortage;
        string lspurchaseorder_status;
        string lstPO_GRN_flag;

        public void DaGetPmrTrnGrnQcchecker(string grn_gid, MdlPmrTrnGrnQcchecker values)
        {
            try
            {
                
                msSQL = msSQL = " select a.grn_gid, a.dc_no,a.purchaseorder_gid, date_format(a.grn_date,'%d-%m-%Y') as grn_date,a.vendor_gid, " +
                    " a.checkeruser_gid,a.vendor_contact_person, " +
                    " a.purchaseorder_list,a.grn_remarks,a.grn_reference, " +
                    " concat(b.user_firstname,' - ',d.department_name) as user_firstname, " +
                    " e.vendor_gid, e.vendor_companyname, e.contactperson_name,CONCAT(p.user_firstname, ' ', p.user_lastname) AS user_checkername, " +
                    " e.contact_telephonenumber,e.email_id,concat(f.address1,'',f.address2) as address " +
                    " from pmr_trn_tgrn a " +
                    " left join adm_mst_tuser b on a.user_gid=b.user_gid " +
                    " left join hrm_mst_temployee c on c.user_gid = b.user_gid " +
                    " left join hrm_mst_tdepartment d on c.department_gid=d.department_gid " +
                    " left join acp_mst_tvendor e on e.vendor_gid = a.vendor_gid " +
                    " left join adm_mst_taddress f on e.address_gid=f.address_gid " +
                    " left join adm_mst_tuser p on a.user_gid=p.user_gid " +
                     " where a.grn_gid = '" + grn_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetGrnQcChecker_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetGrnQcChecker_list
                        {
                            grn_gid = dt["grn_gid"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            grn_date = dt["grn_date"].ToString(),
                            vendor_contact_person = dt["vendor_contact_person"].ToString(),
                            contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                            email_id = dt["email_id"].ToString(),
                            address = dt["address"].ToString(),
                            purchaseorder_list = dt["purchaseorder_list"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            grn_remarks = dt["grn_remarks"].ToString(),
                            grn_reference = dt["grn_reference"].ToString(),
                            user_checkername = dt["user_checkername"].ToString(),
                            dc_no = dt["dc_no"].ToString(),
                            purchaseorder_gid = dt["purchaseorder_gid"].ToString(),

                        });

                        values.GetGrnQcChecker_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting GRN QC details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
               ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
               DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        public void DaGetPmrTrnGrnQccheckerpo(string grn_gid, MdlPmrTrnGrnQcchecker values)
        {
            try
            {
                
                msSQL = " select a.grn_gid, b.grndtl_gid,b.product_gid, b.product_remarks, b.qty_delivered,b.qty_shortage,b.display_field, " +
                " c.product_name, c.product_code, d.productgroup_name, e.productuom_name,e.productuom_gid,f.product_price,b.purchaseorderdtl_gid, " +
                " h.location_name,k.bin_number from pmr_trn_tgrn a " +
                " left join pmr_trn_tgrndtl b on a.grn_gid = b.grn_gid " +
                " left join pmr_mst_tproduct c on b.product_gid = c.product_gid " +
                " left join pmr_mst_tproductgroup d on c.productgroup_gid = d.productgroup_gid " +
                " left join pmr_mst_tproductuom e on e.productuom_gid = b.uom_gid " +
                " left join pmr_trn_tpurchaseorderdtl f on f.purchaseorderdtl_gid=b.purchaseorderdtl_gid" +
                " left join ims_mst_tlocation h on b.location_gid=h.location_gid " +
                " left join ims_mst_tbin k on b.bin_gid=k.bin_gid" +
                " where a.grn_gid = '" + grn_gid + "' group by grndtl_gid";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetGrnQcChecker_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetGrnQcChecker_lists
                        {
                            product_gid = dt["product_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            qty_delivered = dt["qty_delivered"].ToString(),
                            qty_shortage = dt["qty_shortage"].ToString(),
                            display_field = dt["product_remarks"].ToString(),
                            location_name = dt["location_name"].ToString(),
                            bin_number = dt["bin_number"].ToString(),
                            purchaseorderdtl_gid = dt["purchaseorderdtl_gid"].ToString(),
                            grndtl_gid = dt["grndtl_gid"].ToString(),

                        });
                        //double qtydelivered = double.Parse("qty_delivered");
                        //double qtyshortage = double.Parse("qty_shortage");

                        values.GetGrnQcChecker_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting QC checker details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
               ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
               DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
          
        }

        public void DaPostPmrTrnGrnQcchecker(string user_gid, PostGrnQcChecker_lists values)

        {
            try
            {
                
                foreach (var data in values.GetGrnQcChecker_lists)
                {
                    msSQL = " select uom_gid, split_flag,receiveduom_gid from pmr_trn_tgrndtl " +
                            " where grn_gid = '" + values.grn_gid + "'and" +
                            " product_gid = '" + data.product_gid + "'";

                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                    if (objMySqlDataReader.HasRows)
                    {
                        lsreceiveduom_gid = objMySqlDataReader["receiveduom_gid"].ToString();
                        lsbaseuom_gid = objMySqlDataReader["uom_gid"].ToString();
                        lssplit_flag = objMySqlDataReader["split_flag"].ToString();
                    }


                    if (lssplit_flag == "Y")
                    {
                        msSQL = " select convertion_rate, sequence_level from pmr_mst_tproductuom " +
                                    " where productuom_gid = '" + lsbaseuom_gid + "'";

                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                        lsordereduom_conversionrate = objMySqlDataReader["convertion_rate"].ToString();
                        lsordereduom_sequencelevel = objMySqlDataReader["sequence_level"].ToString();
                        double lsordereduomsequencelevel = double.Parse(lsordereduom_sequencelevel);
                        double lsordereduomconversionrate = double.Parse(lsordereduom_conversionrate);


                        msSQL = " select convertion_rate, sequence_level from pmr_mst_tproductuom " +
                                        " where productuom_gid = '" + values.productuom_gid + "'";

                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                        if (objMySqlDataReader.HasRows)
                        {
                            lsreceiveduom_conversionrate = objMySqlDataReader["convertion_rate"].ToString();
                            lsreceiveduom_sequencelevel = objMySqlDataReader["sequence_level"].ToString();
                            double lsreceiveduomsequencelevel = double.Parse(lsreceiveduom_sequencelevel);
                            double lsreceiveduomconversionrate = double.Parse(lsreceiveduom_conversionrate);


                            //if (lsordereduom_sequencelevel > lsreceiveduom_sequencelevel)
                            //{
                            //    lsSUM = double.Parse(((TextBox)dggrnchecker.FindControl("txtQtyRejected")).Text) * lsreceiveduom_conversionrate;
                            //    lsShortageSUM = double.Parse(((TextBox)dggrnchecker.FindControl("txtQtyShortage")).Text) * lsreceiveduom_conversionrate;
                            //}
                            //else
                            //{
                            //    lsSUM = double.Parse(((TextBox)dggrnchecker.FindControl("txtQtyRejected")).Text) / lsordereduom_conversionrate;
                            //    lsShortageSUM = double.Parse(((TextBox)dggrnchecker.FindControl("txtQtyShortage")).Text) / lsordereduom_conversionrate;
                            //}

                            //    if (double.TryParse(values.rejected_qty.ToString(), out double parsedRejectedQty) &&
                            //        double.TryParse(values.qty_shortage.ToString(), out double parsedQtyShortage))
                            //    {

                            //        if (double.TryParse(values.qty_delivered.ToString(), out qtyDelivered) &&
                            //              double.TryParse(values.rejected_qty.ToString(), out qtyRejected) &&
                            //              double.TryParse(values.qty_shortage.ToString(), out qtyShortage))
                            //            if (lsordereduomsequencelevel > lsreceiveduomsequencelevel)
                            //            {
                            //                lsSUM = parsedRejectedQty * lsreceiveduomconversionrate;
                            //                lsShortageSUM = parsedQtyShortage * lsreceiveduomconversionrate;
                            //            }
                            //            else
                            //            {
                            //                lsSUM = parsedRejectedQty / lsordereduomconversionrate;
                            //                lsShortageSUM = parsedQtyShortage / lsordereduomconversionrate;
                            //            }
                            //    }
                        }
                        if (blnStatus == "GRN QC Rejected")
                        {
                            msSQL = msSQL = "select grn_qty from ims_trn_tstock where reference_gid='" + values.grn_gid + "'";

                            objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                        }
                        if (objMySqlDataReader.HasRows == true)
                        {
                            objMySqlDataReader.Read();
                            lsgrn_qty = objMySqlDataReader["grn_qty"].ToString();
                            objMySqlDataReader.Close();
                        }


                        msSQL = " update ims_trn_tstock" +
                                        " set rejected_qty='" + lsSUM + "'," +
                                        " shortage_qty ='" + lsShortageSUM + "'," +
                                        " stock_qty='" + values.stock_qty + "'" +
                                        " Where reference_gid = '" + values.grn_gid + "'and" +
                                        " product_gid = '" + data.product_gid + "' and uom_gid='" + lsreceiveduom_gid + "'";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                        if (double.TryParse(values.qty_delivered.ToString(), out qtyDelivered) &&
                              double.TryParse(values.rejected_qty.ToString(), out qtyRejected) &&
                              double.TryParse(values.qty_shortage.ToString(), out qtyShortage))
                        {

                            double amountPercentage = (100 / qtyDelivered) * qtyRejected;



                            msGetGid1 = objcmnfunctions.GetMasterGID("PMVR");


                            msSQL = "insert into pmr_trn_tvendordamage ( " +
                            " vendordamage_gid, " +
                            " vendor_gid, " +
                            " grn_gid, " +
                            " grn_date, " +
                            " damage_percentage," +
                            " created_by, " +
                            " delivered_qty, " +
                            " rejected_qty, " +
                            " shortage_qty " +
                            " ) values ( " +
                            " '" + msGetGid1 + "'," +
                            "'" + values.vendor_gid + "'," +
                            "'" + values.grn_gid + "', " +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                            "'" + amountPercentage + "', " +
                            "'" + user_gid + "', " +
                            "'" + data.qty_delivered + "', " +
                            "'" + data.rejected_qty + "', " +
                            "'" + data.qty_shortage + "') ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                        }

                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Checker Approved Successfully";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Occuring Checker  Approved ";
                        }
                    }
                    else
                    {
                        double qtyRejected = Convert.ToInt32(data.rejected_qty);
                        double qtyShortage = Convert.ToInt32(data.qty_shortage);
                        double qtyDelivered = Convert.ToInt32(data.qty_delivered);

                        msSQL = " update ims_trn_tstock" +
                                   " set rejected_qty='" + lsSUM + "'," +
                                    " shortage_qty = '" + lsShortageSUM + "'," +
                                    " remarks = 'From Purchase'" +
                                    " where product_gid = '" + data.product_gid + "'and" +
                                    " uom_gid = '" + lsreceiveduom_gid + "'and" +
                                    " reference_gid = '" + values.grn_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = " update ims_trn_tstock set" +
                                " stock_qty=grn_qty-rejected_qty-shortage_qty" +
                                " where product_gid = '" + values.product_gid + "'and" +
                                " uom_gid = '" + lsreceiveduom_gid + "'and" +
                                " reference_gid = '" + values.grn_gid + "'";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (Convert.ToInt32(data.qty_delivered) != 0 &&
                            Convert.ToInt32(data.rejected_qty) == 0 &&
                            Convert.ToInt32(data.qty_shortage) == 0)
                        {

                            double amountPercentage = (100 / qtyDelivered) * qtyRejected;

                            msGetGid1 = objcmnfunctions.GetMasterGID("PMVR");
                            msSQL = "insert into pmr_trn_tvendordamage ( " +
                          " vendordamage_gid, " +
                          " vendor_gid, " +
                          " grn_gid, " +
                          " grn_date, " +
                          " damage_percentage," +
                          " created_by, " +
                          " delivered_qty, " +
                          " rejected_qty, " +
                          " shortage_qty " +
                          " ) values ( " +
                          " '" + msGetGid1 + "'," +
                          "'" + values.vendor_gid + "'," +
                          "'" + values.grn_gid + "', " +
                          "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                          "'" + amountPercentage + "', " +
                          "'" + user_gid + "', " +
                          "'" + data.qty_delivered + "', " +
                          "'" + data.rejected_qty + "', " +
                          "'" + data.qty_shortage + "') ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (Convert.ToDouble(data.qty_delivered) != 0.0)
                            {




                                msSQL = " update ims_trn_tstock set " +
                                        " rejected_qty = '0'," +
                                        " shortage_qty = '" + data.qty_shortage + "'," +
                                        " remarks = 'From Purchase'" +
                                        " where product_gid = '" + data.product_gid + "'and" +
                                        " uom_gid = '" + data.productuom_gid + "'and" +
                                        " reference_gid = '" + values.grn_gid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                            else
                            {
                                double lsSUM = 0.0;
                                double lsShortageSUM = 0.0;
                                blnStatus = "GRN QC Approved";
                                msSQL = " update ims_trn_tstock set " +
                                        " rejected_qty = '0'," +
                                        " shortage_qty = '0'," +
                                        " remarks = 'From Purchase'" +
                                        " where product_gid = '" + data.product_gid + "'and" +
                                        " uom_gid = '" + data.productuom_gid + "'and" +
                                        " reference_gid = '" + values.grn_gid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                            msSQL = " update ims_trn_tstock set" +
                              " stock_qty=grn_qty-rejected_qty-shortage_qty" +
                              " where product_gid = '" + data.product_gid + "'and" +
                              " uom_gid = '" + data.productuom_gid + "'and" +
                              " reference_gid = '" + data.grn_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            lsTotal = lsTotal + lsSUM;

                            msSQL = " Update pmr_trn_tgrndtl " +
                                    " Set qty_rejected = qty_rejected + '" + lsSUM + "', " +
                                    " qty_shortage = qty_shortage + '" + lsShortageSUM + "', " +
                                    " qc_remarks = '" + data.display_field + "', " +
                                    " qc_status = '" + blnStatus + "' " +
                                    " Where grn_gid = '" + values.grn_gid + "'and" +
                                    " grndtl_gid = '" + values.grndtl_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            int i = 0;
                            i = i + 1;

                            msSQL = "select qty_received from pmr_trn_tpurchaseorderdtl where purchaseorderdtl_gid='" + values.purchaseorderdtl_gid + "'";
                            objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                            if (objMySqlDataReader.HasRows)
                            {
                                lsPO_Sum_Received = objMySqlDataReader["qty_received"].ToString();
                            }

                            msSQL = " update pmr_trn_tpurchaseorderdtl " +
                                    " Set qty_received = '" + lsPO_Sum_Received + "'" +
                                    " where  purchaseorderdtl_gid = '" + values.purchaseorderdtl_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);



                            msSQL = " SELECT qty_received, qty_grnadjusted, qty_ordered" +
                                   "  FROM pmr_trn_tpurchaseorderdtl " +
                                   "  WHERE purchaseorder_gid = '" + values.purchaseorder_gid + "' AND(qty_received + qty_grnadjusted) < qty_ordered";

                            objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                            if (objMySqlDataReader.HasRows)
                            {
                                lspurchaseorder_status = "PO Work In Progress";
                                lstPO_GRN_flag = "Goods Received Partial";
                            }
                            else
                            {
                                lspurchaseorder_status = "PO Completed";
                                lstPO_GRN_flag = "Goods Received";
                            }
                            if (i != 0)
                            {
                                msSQL = " Update pmr_trn_tpurchaseorder " +
                                        " Set purchaseorder_status = '" + lspurchaseorder_status + "'," +
                                        " grn_flag = '" + lstPO_GRN_flag + "'" +
                                        " where purchaseorder_gid = '" + values.purchaseorder_gid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult == 0)
                                {

                                    objcmnfunctions.LogForAudit("Error occurred while updating into Purchaseorder table" + msSQL);
                                }
                            }
                            //if (objMySqlDataReader.HasRows)
                            //{
                            //    lsPO_Sum_Received = objMySqlDataReader["qty_received"].ToString();
                            //}
                            //lsfilepath = objcmnfunctions.popupload(fileuploadadd, "grn_upload", "purchase")
                            //msSQL = " Update pmr_trn_tgrn Set" +
                            //        " attachment ='" + lsfilename + "'," +
                            //        " attachment_file ='" + lsfilepath + "'" +
                            //        " Where grn_gid = '" + rqgrn_gid + "'";
                            //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            msSQL = " Update pmr_trn_tgrn Set" +
                                    " qc_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                    " grn_status = 'GRNLinkPO Completed', " +
                                    " grn_flag = 'GRN Approved' " +
                                    " Where grn_gid = '" + values.grn_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                            msSQL = "select approval_flag from pmr_trn_tapproval where submodule_gid='PMRSTKGRA' and grnapproval_gid='" + values.grn_gid + "'";
                            objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                            msSQL = " Update pmr_trn_tgrn " +
                                    " Set " +
                                    " grn_status = 'GRN Approved', " +
                                    " grn_flag = 'Invoice Pending', " +
                                    " invoice_status = 'IV Pending', " +
                                    " approved_by = '" + user_gid + "', " +
                                    " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                    " where grn_gid = '" + values.grn_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            msSQL = " select a.grn_gid, b.product_gid, b.product_remarks, " +
                                    " (b.qty_delivered - b.qty_rejected-b.qty_shortage) as qty_accepted, " +
                                    " b.qty_delivered, b.qty_billed, b.qty_excess " +
                                    " from pmr_trn_tgrn a " +
                                    " left join pmr_trn_tgrndtl b on a.grn_gid = b.grn_gid " +
                                    " where a.grn_gid = '" + values.grn_gid + "'";

                            objMySqlDataReader = objdbconn.GetDataReader(msSQL);



                            if (objMySqlDataReader.HasRows)
                            {
                                lsgrn_gid = objMySqlDataReader["grn_gid"].ToString();
                                lsproduct_gid = objMySqlDataReader["product_gid"].ToString();
                                lsqty_billed = objMySqlDataReader["qty_accepted"].ToString();
                            }

                            msSQL = " update ims_trn_tstock set " +
                                        " stock_flag = 'Y'" +
                                        " where reference_gid = '" + lsgrn_gid + "' and " +
                                        " product_gid = '" + lsproduct_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            msSQL = " update ims_trn_tstocktracker set " +
                                    " stock_flag = 'Y'" +
                                    " where reference_gid = '" + lsgrn_gid + "' and " +
                                    " product_gid = '" + lsproduct_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult != 0)
                            {
                                values.status = true;
                                values.message = "Checker Approved Successfully";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Occuring Checker  Approved ";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding GRN QC!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
               ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
               DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
    }
}