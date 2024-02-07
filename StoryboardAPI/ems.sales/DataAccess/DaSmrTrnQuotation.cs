﻿using ems.sales.Models;
using ems.utilities.Functions;
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
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using MySql.Data.MySqlClient;
using static OfficeOpenXml.ExcelErrorValue;


namespace ems.sales.DataAccess
{
    public class DaSmrTrnQuotation
    {

        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        string lspop_server, lspop_mail, lspop_password, lscompany, lscompany_code;
        string msINGetGID, msGet_att_Gid, msenquiryloggid;
        string lspath, lspath1, lspath2, mail_path, mail_filepath, pdf_name = "";
        MySqlDataReader objMySqlDataReader, objMySqlDataReader1;
        MySqlDataReader ds_tsalesorderadd;
        int lsamendcount;
        DataTable mail_datatable, dt_datatable;

        string msEmployeeGID, lsemployee_gid, lsenquiry_type, start_date, end_date, lsentity_code, lsquotationgid, lsproductgid1, lstaxname1, TempSOGID, SalesOrderGID, msGetSalesOrderGID, lstaxname, lstaxamount, lspercentage1, lscustomer_gid, lsproduct_price, msGetTempGID, lsquotation_type, lsdesignation_code, lstaxname2, lstaxname3, lsamount2, lsamount3, lspercentage2, lspercentage3, lscustomer_code, pricingsheet_refno, roundoff, mssalesorderGID, lsCode, msGetGid, msGetGid1, msgetGid2, msgetGid4, lstype1, lshierarchy_flag, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5, lspop_port;
        string msGetCustomergid, msconGetGID;
        string lscustomer_name;
        string lscontact_person, lscustomercontact_gid, lscustomerbranch_name, lscustomercontact_names;
        string lstmpquotationgid;
        string lsproductgroup_gid;
        string lsproductgroup;
        string lsproductname_gid;
        string lsproductname;
        string lsuom_gid;
        string lsvendor_gid;
        string lsuom;
        string lsunitprice;
        string lsquantity;
        string lsdiscountpercentage;
        string lsdiscountamount;
        string lstax_name1;
        string lscustomerproduct_code;
        string lstax_name2;
        string lstax_name3;
        string lstaxamount_1;
        string lstaxamount_2;
        string lstaxamount_3;
        string lstotalamount;
        string lssono, lsprice;
        string lsdisplay_field, lslocalmarginpercentage, lslocalsellingprice, lsuom_name, lsreqdate_remarks, lsrequired_date;
        MailMessage message = new MailMessage();
        public void DaGetSmrTrnQuotation(MdlSmrTrnQuotation values)
        {
            try
            {
               
                string currency = "INR";

                msSQL = " select distinct d.leadbank_gid,a.quotation_gid, a.customer_gid,a.quotation_referenceno1,ifnull(concat(f.enquiry_gid , '|' , a.quotation_type ),'Direct Quotation') as enquiry_gid,f.enquiry_referencenumber,s.source_name,DATE_FORMAT(a.quotation_date, '%d-%m-%Y') as quotation_date,c.user_firstname,a.quotation_type,a.currency_code, " +
                  " case when a.grandtotal_l ='0.00' then format(a.Grandtotal,2) else format(a.grandtotal_l,2) end as Grandtotal," +
                  " case when a.currency_code = '" + currency + "' then a.customer_name " +
                  " when a.currency_code is null then a.customer_name " +
                  " when a.currency_code is not null and a.currency_code <> '" + currency + "' then (a.customer_name) end as customer_name, " +
                  " a.customer_contact_person, a.quotation_status,a.enquiry_gid, " +
                  " case when a.contact_mail is null then concat(e.leadbankcontact_name,'/',e.mobile,'/',e.email) " +
                  " when a.contact_mail is not null then concat(a.customer_contact_person,' / ',a.contact_no,' / ',a.contact_mail) end as contact, " +
                  " a.customer_address " +
                  " from smr_trn_treceivequotation a " +
                  " left join hrm_mst_temployee b on b.employee_gid=a.created_by " +
                  " left join adm_mst_tuser c on b.user_gid= c.user_gid " +
                  " left join crm_trn_tleadbank d on d.leadbank_gid=a.customer_gid " +
                  " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code " +
                  " left join crm_trn_tleadbankcontact e on e.leadbank_gid=d.leadbank_gid " +
                  " left join crm_mst_tsource s on s.source_gid=d.source_gid" +
                  " left join acp_trn_tenquiry f on a.enquiry_gid=f.enquiry_gid " +
                  " where 1=1 order by a.quotation_date desc";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<quotation_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new quotation_list
                        {
                            quotation_gid = dt["quotation_gid"].ToString(),
                            quotation_date = dt["quotation_date"].ToString(),
                            quotation_referenceno1 = dt["quotation_gid"].ToString(),
                            enquiry_gid = dt["enquiry_gid"].ToString(),
                            enquiry_referencenumber = dt["enquiry_referencenumber"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            contact = dt["contact"].ToString(),
                            quotation_type = dt["quotation_type"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            quotation_status = dt["quotation_status"].ToString(),
                            Grandtotal = dt["Grandtotal"].ToString(),


                        });
                        values.quotation_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Quotation Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        // Sales Person

        public void DaGetSalesDtl(MdlSmrTrnQuotation values)
        {
            try
            {
                
                msSQL = "select concat(c.department_name,' ','/',' ',a.user_firstname,' ',a.user_lastname) as user_name,a.user_gid from adm_mst_tuser a " +
                " left join hrm_mst_temployee b on a.user_gid=b.user_gid " +
                " left join hrm_mst_tdepartment c on b.department_gid=c.department_gid where a.user_status='Y' and " +
                " department_name in('Marketing','Management','Finance') order by a.user_code  asc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetSalesDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetSalesDropdown

                        {
                            user_gid = dt["user_gid"].ToString(),
                            user_name = dt["user_name"].ToString(),

                        });
                        values.GetSalesDtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading User Name !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        // Currency
        public void DaGetCurrencyCodeDtl(MdlSmrTrnQuotation values)
        {
            try
            {
                


                msSQL = "select currencyexchange_gid,currency_code from crm_trn_tcurrencyexchange order by currency_code asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetCurrencyCodeDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetCurrencyCodeDropdown

                        {
                            currencyexchange_gid = dt["currencyexchange_gid"].ToString(),
                            currency_code = dt["currency_code"].ToString(),

                        });
                        values.GetCurrencyCodeDtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Currency Code !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }


        // Tax 1
        public void DaGetTaxOnceDtl(MdlSmrTrnQuotation values)
        {

            try
            {
               

                msSQL = " select tax_name,tax_gid,percentage from acp_mst_ttax where active_flag='Y' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetTaxOnceDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetTaxOnceDropdown

                        {
                            tax_gid = dt["tax_gid"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            percentage = dt["percentage"].ToString()

                        });
                        values.GetTaxOnceDtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }

        // Tax 2
        public void DaGetTaxTwiceDtl(MdlSmrTrnQuotation values)
        {
            try
            {


                msSQL = " select tax_name,tax_gid,percentage from acp_mst_ttax where active_flag='Y' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetTaxTwiceDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetTaxTwiceDropdown

                        {
                            tax_gid = dt["tax_gid"].ToString(),
                            tax_name2 = dt["tax_name"].ToString(),
                            percentage = dt["percentage"].ToString()

                        });
                        values.GetTaxTwiceDtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }

        // Tax 3
        public void DaGetTaxThriceDtl(MdlSmrTrnQuotation values)
        {
            try
            {


                msSQL = " select tax_name,tax_gid,percentage from acp_mst_ttax where active_flag='Y' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetTaxThriceDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetTaxThriceDropdown

                        {
                            tax_gid = dt["tax_gid"].ToString(),
                            tax_name3 = dt["tax_name"].ToString(),
                            percentage = dt["percentage"].ToString()

                        });
                        values.GetTaxThriceDtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }



        // Product

        public void DaGetProductNamesDtl(MdlSmrTrnQuotation values)
        {
            try
            {


                msSQL = "Select product_gid, product_name from pmr_mst_tproduct";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProductNamesDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProductNamesDropdown

                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),

                        });
                        values.GetProductNamesDtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Product  Name  !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }

        //CRM product dropdown
        public void DaGetProductNamesDtlCRM(MdlSmrTrnQuotation values)
        {

            try
            {
                
                msSQL = "Select product_gid, product_name from pmr_mst_tproduct";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProductNamesDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProductNamesDropdown

                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),

                        });
                        values.GetProductNamesDtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {

                values.message = "Exception occured while loading Product  !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }

        // Tax 3
        public void DaGetTaxFourSDtl(MdlSmrTrnQuotation values)
        {
            try
            {
               


                msSQL = "  select tax_name,tax_gid,percentage from acp_mst_ttax where active_flag='Y' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetTaxFourSDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetTaxFourSDropdown

                        {
                            tax_gid = dt["tax_gid"].ToString(),
                            tax_name4 = dt["tax_name"].ToString(),
                            percentage = dt["percentage"].ToString()

                        });
                        values.GetTaxFourSDtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Tax Percentage !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }

        //on change CRM
        public void DaGetOnChangeProductsNameCRM(string product_gid, MdlSmrTrnQuotation values)
        {
            try
            {

                if (product_gid != null)
                {
                    msSQL = " Select a.product_name, a.product_code, b.productuom_gid,b.productuom_name,c.productgroup_name,c.productgroup_gid,a.productuom_gid  from pmr_mst_tproduct a  " +
                         " left join pmr_mst_tproductuom b on a.productuom_gid = b.productuom_gid  " +
                        " left join pmr_mst_tproductgroup c on a.productgroup_gid = c.productgroup_gid  " +
                    " where a.product_gid='" + product_gid + "' ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    var getModuleList = new List<GetproductsCodes>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetproductsCodes
                            {
                                product_name = dt["product_name"].ToString(),
                                product_code = dt["product_code"].ToString(),
                                productuom_name = dt["productuom_name"].ToString(),
                                productgroup_name = dt["productgroup_name"].ToString(),
                                productuom_gid = dt["productuom_gid"].ToString(),
                                productgroup_gid = dt["productgroup_gid"].ToString(),

                            });
                            values.ProductsCode = getModuleList;
                        }
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting  Product Detailes !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }

        // On change


        public void DaGetOnChangeProductsName(string product_gid, string customercontact_gid, MdlSmrTrnQuotation values)

        {
            try
            {
                
                if (customercontact_gid != null)
                {
                    msSQL = "select customer_gid from crm_trn_tleadbank where leadbank_gid ='" + customercontact_gid + "'";
                    lscustomer_gid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = "  select a.product_price from smr_trn_tpricesegment2product a    left join smr_trn_tpricesegment2customer b on a.pricesegment_gid= b.pricesegment_gid " +
                        "  left join pmr_mst_tproduct c on a.product_gid=c.product_gid where b.customer_gid='" + lscustomer_gid + "'   and a.product_gid='" + product_gid + "'";
                    lsproduct_price = objdbconn.GetExecuteScalar(msSQL);
                    if (lsproduct_price != "")
                    {

                        msSQL = " Select a.product_name, a.product_code,case when f.customer_gid is not null then(select a.product_price from smr_trn_tpricesegment2product a " +
                        " left join smr_trn_tpricesegment2customer b on a.pricesegment_gid= b.pricesegment_gid where b.customer_gid='" + lscustomer_gid + "'" +
                        " and a.product_gid='" + product_gid + "') else (a.mrp_price)end as cost_price,  b.productuom_gid,b.productuom_name,c.productgroup_name," +
                        "c.productgroup_gid,a.productuom_gid  from pmr_mst_tproduct a  left join pmr_mst_tproductuom b on a.productuom_gid = b.productuom_gid " +
                        "  left join pmr_mst_tproductgroup c on  a.productgroup_gid = c.productgroup_gid  left join smr_trn_tpricesegment2product e" +
                        " on a.product_gid=e.product_gid left join smr_trn_tpricesegment2customer f on e.pricesegment_gid=f.pricesegment_gid " +
                        " where a.product_gid='" + product_gid + "'";
                        dt_datatable = objdbconn.GetDataTable(msSQL);

                        var getModuleList = new List<GetproductsCodes>();
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                getModuleList.Add(new GetproductsCodes
                                {
                                    product_name = dt["product_name"].ToString(),
                                    product_code = dt["product_code"].ToString(),
                                    productuom_name = dt["productuom_name"].ToString(),
                                    productgroup_name = dt["productgroup_name"].ToString(),
                                    productuom_gid = dt["productuom_gid"].ToString(),
                                    productgroup_gid = dt["productgroup_gid"].ToString(),
                                    selling_price = dt["cost_price"].ToString(),

                                });
                                values.ProductsCode = getModuleList;
                            }
                        }
                    }
                    else
                    {
                        msSQL = " Select a.product_name, a.product_code,a.mrp_price as cost_price," +
                            " b.productuom_gid,b.productuom_name,c.productgroup_name,c.productgroup_gid,a.productuom_gid  from pmr_mst_tproduct a  " +
                             " left join pmr_mst_tproductuom b on a.productuom_gid = b.productuom_gid  " +
                            " left join pmr_mst_tproductgroup c on a.productgroup_gid = c.productgroup_gid " +
                        " where a.product_gid='" + product_gid + "' ";
                        dt_datatable = objdbconn.GetDataTable(msSQL);

                        var getModuleList = new List<GetproductsCodes>();
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                getModuleList.Add(new GetproductsCodes
                                {
                                    product_name = dt["product_name"].ToString(),
                                    product_code = dt["product_code"].ToString(),
                                    productuom_name = dt["productuom_name"].ToString(),
                                    productgroup_name = dt["productgroup_name"].ToString(),
                                    productuom_gid = dt["productuom_gid"].ToString(),
                                    productgroup_gid = dt["productgroup_gid"].ToString(),
                                    selling_price = dt["cost_price"].ToString(),

                                });
                                values.ProductsCode = getModuleList;
                            }
                        }
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Kindly Select Customer Before Adding Product";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while  Adding Product !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           

        }

        // qoute to order onchange order

        public void GetOnChangeProductsNameQTO(string product_gid, MdlSmrTrnQuotation values)
        {
            try
            {
             
                msSQL = " Select a.product_gid, a.product_name, a.product_code, a.cost_price," +
                       " b.productuom_gid,b.productuom_name,c.productgroup_name,c.productgroup_gid,a.productuom_gid  from pmr_mst_tproduct a  " +
                        " left join pmr_mst_tproductuom b on a.productuom_gid = b.productuom_gid  " +
                       " left join pmr_mst_tproductgroup c on a.productgroup_gid = c.productgroup_gid " +
                   " where a.product_gid='" + product_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetproductsCodes>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetproductsCodes
                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            selling_price = dt["cost_price"].ToString(),

                        });
                        values.ProductsCode = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting  Product Detailes !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }

        public void DaGetOnChangeProductsNames(string product_gid, MdlSmrTrnQuotation values)
        {
            try
            {
                
                msSQL = " Select a.product_gid, a.product_name, a.product_code," +
                       " b.productuom_gid,b.productuom_name,c.productgroup_name,c.productgroup_gid,a.productuom_gid  from pmr_mst_tproduct a  " +
                        " left join pmr_mst_tproductuom b on a.productuom_gid = b.productuom_gid  " +
                       " left join pmr_mst_tproductgroup c on a.productgroup_gid = c.productgroup_gid " +
                   " where a.product_gid='" + product_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetproductsCodes>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetproductsCodes
                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            //selling_price = dt["cost_price"].ToString(),

                        });
                        values.ProductsCode = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while  Product  Detailes !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }

        // Summary bind

        public void DaGetRaiseSOSummary(string quotation_gid, MdlSmrTrnQuotation values)
        {
            try
            {
               
                msSQL = " Select quotation_gid,quotationdtl_gid, product_gid, product_name, productgroup_gid, productgroup_name, product_price, product_code, " +
                    " qty_quoted, discount_percentage, discount_amount, uom_gid, uom_name, tax_amount, tax_name, created_by, price from smr_trn_treceivequotationdtl" +
                    " where quotation_gid = '" + quotation_gid + "' AND quotationdtl_gid  NOT IN(SELECT quotationdtl_gid FROM smr_tmp_tsalesorderdtl WHERE quotation_gid ='" + quotation_gid +"')";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        SalesOrderGID = objcmnfunctions.GetMasterGID("VSOP");
                        TempSOGID = objcmnfunctions.GetMasterGID("VSDT");

                        msSQL = " Insert into smr_tmp_tsalesorderdtl(" +
                                " tmpsalesorderdtl_gid, " +
                                " salesorder_gid, " +
                                " quotation_gid, " +
                                " quotationdtl_gid, " +
                                " product_gid," +
                                " product_code," +
                                " product_name," +
                                " productgroup_name," +
                                " productgroup_gid," +
                                " uom_gid," +
                                " uom_name," +
                                " qty_quoted," +
                                " discount_percentage," +
                                " discount_amount," +
                                " tax_name," +
                                " tax_amount," +
                                " product_price," +
                                " employee_gid," +
                                " price" +
                                ") values (" +
                                "'" + TempSOGID + "'," +
                                "'" + SalesOrderGID + "'," +
                                "'" + quotation_gid + "'," +
                                 "'" + dt["quotationdtl_gid"] + "', " +
                                "'" + dt["product_gid"] + "', " +
                                "'" + dt["product_code"] + "', " +
                                "'" + dt["product_name"] + "', " +
                                "'" + dt["productgroup_name"] + "', " +
                                "'" + dt["productgroup_gid"] + "', " +
                                "'" + dt["uom_gid"] + "', " +
                                "'" + dt["uom_name"] + "', " +
                                "'" + dt["qty_quoted"] + "', " +
                                "'" + dt["discount_percentage"] + "', " +
                                "'" + dt["discount_amount"] + "', " +
                                "'" + dt["tax_name"] + "', " +
                                "'" + dt["tax_amount"] + "', " +
                                "'" + dt["product_price"] + "', " +
                                "'" + dt["created_by"] + "', " +
                                "'" + dt["price"] + "') ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }
                if (mnResult != 0)
                {
                    msSQL = " select a.quotation_gid,l.quotation_gid,g.customer_gid,a.currency_code, a.termsandconditions,DATE_FORMAT(a.quotation, '%d-%m-%Y') as quotation_date, " +
                            " a.quotation_referencenumber,a.quotation_remarks,a.branch_gid,  a.quotation_referenceno1,a.payment_days,l.product_code," +
                            " e.branch_name,a.customerbranch_gid,a.freight_terms, a.payment_terms,h.currency_code as code,a.exchange_rate,h.currencyexchange_gid," +
                            " h.exchange_rate as code,  a.contact_mail,a.contact_no,concat(a.customer_contact_person) as contact_person, " +
                            " a.customer_address,format(a.Grandtotal_l, 2) as Grandtotal_l,a.currency_gid,a.exchange_rate as rate, format(a.Grandtotal, 2) as Grandtotal ," +
                            " format(a.addon_charge, 2) as addon_charge,format(a.additional_discount, 2) as additional_discount, format(sum(l.price), 2) as total_value, " +
                            " a.customer_name, format(a.gst_percentage, 2) as gst_percentage, a.tax_gid,format(a.total_amount, 2) as total_amount," +
                            " format(a.total_price, 2) as total_price, a.payment_days,a.delivery_days,a.enquiry_refno,a.salesperson_gid,   c.customercontact_name,c.mobile,c.email," +
                            " format(a.freight_charges, 2) as freight_charges, l.product_name, l.productgroup_name,l.productgroup_gid,  l.uom_gid,l.uom_name," +
                            " l.tax_amount,l.product_price,l.qty_quoted,l.price,l.tax_name, format(a.buyback_charges, 2) as buyback_charges," +
                            " format(a.packing_charges, 2) as packing_charges,l.discount_amount,l.tmpsalesorderdtl_gid, l.discount_percentage, format(a.roundoff, 2) as roundoff , " +
                            " format(a.insurance_charges, 2) as insurance_charges from smr_trn_treceivequotation a " +
                            " left join crm_mst_tcustomer g on a.customer_gid = g.customer_gid " +
                            " left join crm_trn_tcurrencyexchange h on h.currencyexchange_gid = g.currency_gid " +
                            " left join crm_mst_tcustomercontact c on g.customer_gid = c.customer_gid " +
                            " left join smr_tmp_tsalesorderdtl l on a.quotation_gid = l.quotation_gid " +
                            " left join hrm_mst_tbranch e on e.branch_gid = a.branch_gid " +
                            " where a.quotation_gid = '" + quotation_gid + "' group by a.quotation_gid,l.product_gid";

                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<GetSummaryList>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetSummaryList
                            {
                                quotation_gid = dt["quotation_gid"].ToString(),
                                quotation_date = dt["quotation_date"].ToString(),
                                branch_name = dt["branch_name"].ToString(),
                                quotation_referenceno1 = dt["quotation_gid"].ToString(),
                                customer_name = dt["customer_name"].ToString(),
                                so_referencenumber = dt["enquiry_refno"].ToString(),
                                customercontact_names = dt["contact_person"].ToString(),
                                customer_mobile = dt["contact_no"].ToString(),
                                customer_email = dt["contact_mail"].ToString(),
                                so_remarks = dt["quotation_remarks"].ToString(),
                                customer_address = dt["customer_address"].ToString(),
                                payment_terms = dt["payment_terms"].ToString(),
                                freight_terms = dt["freight_terms"].ToString(),
                                exchange_rate = dt["rate"].ToString(),
                                currency_code = dt["currency_code"].ToString(),
                                user_name = dt["salesperson_gid"].ToString(),
                                addon_charge = dt["addon_charge"].ToString(),
                                additional_discount = dt["additional_discount"].ToString(),
                                product_name = dt["product_name"].ToString(),
                                buyback_charges = dt["buyback_charges"].ToString(),
                                quantity = dt["qty_quoted"].ToString(),
                                discountpercentage = dt["discount_percentage"].ToString(),
                                discountamount = dt["discount_amount"].ToString(),
                                product_price = dt["product_price"].ToString(),
                                tax_amount = dt["tax_amount"].ToString(),
                                tax_name = dt["tax_name"].ToString(),
                                totalamount = dt["price"].ToString(),
                                insurance_charges = dt["insurance_charges"].ToString(),
                                freight_charges = dt["freight_charges"].ToString(),
                                packing_charges = dt["packing_charges"].ToString(),
                                roundoff = dt["roundoff"].ToString(),
                                price = dt["price"].ToString(),
                                Grandtotal = dt["Grandtotal"].ToString(),
                                product_code = dt["product_code"].ToString(),
                                productuom_name = dt["uom_name"].ToString(),
                                payment_days = dt["payment_days"].ToString(),
                                delivery_days = dt["delivery_days"].ToString(),
                                total_price = dt["price"].ToString(),

                            });
                            values.SO_list = getModuleList;
                        }

                    }
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Raise Sales Order !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }


        // branch

        public void DaGetBranchDt(MdlSmrTrnQuotation values)
        {
            try
            {
               

                msSQL = "select branch_gid,branch_name from hrm_mst_tbranch";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetBranchDropdowns>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetBranchDropdowns

                        {
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),

                        });
                        values.GetBranchDt = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Branch !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
          
        }

        //Customer
        public void DaGetCustomerDtl(MdlSmrTrnQuotation values)
        {
            try
            {
                

                msSQL = "Select a.customer_gid, a.customer_name " +
                " from crm_mst_tcustomer a " +
                "where a.status='Active'";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetCustomerDropdowns>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetCustomerDropdowns

                        {
                            customer_gid = dt["customer_gid"].ToString(),
                            customer_name = dt["customer_name"].ToString(),

                        });
                        values.GetCustomerDt = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Customer Name !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }

        // Contact
        public void DaGetPersonDtl(MdlSmrTrnQuotation values)
        {
            try
            {
                

                msSQL = "select concat(c.department_name,' ','/',' ',a.user_firstname,' ',a.user_lastname) as user_name,a.user_gid from adm_mst_tuser a " +
                " left join hrm_mst_temployee b on a.user_gid=b.user_gid " +
                " left join hrm_mst_tdepartment c on b.department_gid=c.department_gid where a.user_status='Y' and " +
                " c.department_name in('Sales') order by a.user_code  asc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetPersonDropdowns>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetPersonDropdowns

                        {
                            user_gid = dt["user_gid"].ToString(),
                            user_name = dt["user_name"].ToString(),

                        });
                        values.GetPersonDt = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting User Name !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }

        public void DaGetCurrencyDtl(MdlSmrTrnQuotation values)
        {
            try
            {
                

                msSQL = "select currencyexchange_gid,currency_code from crm_trn_tcurrencyexchange order by currency_code asc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetCurrencyDropdowns>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetCurrencyDropdowns

                        {
                            currencyexchange_gid = dt["currencyexchange_gid"].ToString(),
                            currency_code = dt["currency_code"].ToString(),

                        });
                        values.GetCurrencyDt = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Currency!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }

        //Product
        public void DaGetProductDtl(MdlSmrTrnQuotation values)
        {
            try
            {
               

                msSQL = "Select product_gid, product_name from pmr_mst_tproduct";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProductDropdowns>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProductDropdowns

                        {
                            product_gid = dt["product_gid"].ToString(),
                            productname = dt["product_name"].ToString(),

                        });
                        values.GetProductDt = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting  Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }

        // Tax 1
        public void DaGetTax1Dtl(MdlSmrTrnQuotation values)
        {

            try
            {
               
                msSQL = " select tax_name,tax_gid,percentage from acp_mst_ttax where active_flag='Y' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetTax1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetTax1

                        {
                            tax_gid = dt["tax_gid"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            percentage = dt["percentage"].ToString()
                        });
                        values.GetTax1Dtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Tax!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }

        // Tax 2
        public void DaGetTax2Dtl(MdlSmrTrnQuotation values)
        {
            try
            {
               

                msSQL = " select tax_name,tax_gid,percentage from acp_mst_ttax where active_flag='Y' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetTax2>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetTax2

                        {
                            tax_gid2 = dt["tax_gid"].ToString(),
                            tax_name2 = dt["tax_name"].ToString(),
                            percentage = dt["percentage"].ToString()

                        });
                        values.GetTax2Dtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting  Tax !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }

        // Tax 3
        public void DaGetTax3Dtl(MdlSmrTrnQuotation values)
        {

            try
            {
                

                msSQL = " select tax_name,tax_gid,percentage from acp_mst_ttax where active_flag='Y' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetTax3>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetTax3

                        {
                            tax_gid3 = dt["tax_gid"].ToString(),
                            tax_name3 = dt["tax_name"].ToString(),
                            percentage = dt["percentage"].ToString()

                        });
                        values.GetTax3Dtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Tax !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }



        //Customer dropdown CRM
        public void DaGetCustomerDtlCRM(string leadbank_gid, MdlSmrTrnQuotation values)
        {
            try
            {
                
                msSQL = "Select customer_gid from crm_trn_tleadbank where leadbank_gid='" + leadbank_gid + "' ";
                string lscustomer_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "Select a.customer_gid, a.customer_name " +
                " from crm_mst_tcustomer a where customer_gid='" + lscustomer_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetCustomerDropdowns>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetCustomerDropdowns

                        {
                            customer_gid = dt["customer_gid"].ToString(),
                            customer_name = dt["customer_name"].ToString(),

                        });
                        values.GetCustomerDt = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Customer Name !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }

        // Temp Summary for quotation to order

        public void DaGetTemporarySummary(string employee_gid, string quotation_gid, MdlSmrTrnQuotation values)
        {

            try
            {
                double grand_total = 0.00;
                double grandtotal = 0.00;
                double total_price = 0.00;

               
                msSQL = " select " +
                        " tmpsalesorderdtl_gid," +
                        " salesorder_gid," +
                        " product_gid," +
                        " quotation_gid," +
                        " productgroup_gid," +
                        " productgroup_name," +
                        " product_name," +
                        " product_price," +
                        " qty_quoted," +
                        " discount_percentage," +
                        " discount_amount," +
                        " uom_gid," +
                        " uom_name," +
                        " FORMAT(price,2) as price," +
                        " tax_name," +
                        " tax_amount," +
                        " slno," +
                        "product_code," +
                        " order_type " +
                        " from smr_tmp_tsalesorderdtl" +
                        " where quotation_gid='" + quotation_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetTemporarysummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        //grand_total += double.Parse(dt["price"].ToString());
                        grandtotal += double.Parse(dt["price"].ToString());
                        total_price += double.Parse(dt["price"].ToString());
                        getModuleList.Add(new GetTemporarysummary

                        {
                            tmpsalesorderdtl_gid = dt["tmpsalesorderdtl_gid"].ToString(),
                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            quantity = dt["qty_quoted"].ToString(),
                            discountpercentage = dt["discount_percentage"].ToString(),
                            discountamount = dt["discount_amount"].ToString(),
                            productuom_gid = dt["uom_gid"].ToString(),
                            productuom_name = dt["uom_name"].ToString(),
                            price = dt["price"].ToString(),
                            producttotalamount = dt["price"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            slno = dt["slno"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            grand_total = dt["price"].ToString(),

                        });
                        values.temp_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
                values.grand_total = grand_total;
                values.grandtotal = grandtotal;
                values.total_price = total_price;
            }




            catch (Exception ex)
            {
                values.message = "Exception occured while Temproary Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }
        public void DaGetProductAdd(string employee_gid, summarys_lists values)

        {
            try
            {
               
                msGetGid = objcmnfunctions.GetMasterGID("VSDT");
                msSQL = "select product_gid from pmr_mst_tproduct where product_name='" + values.product_name + "'";
                string lsproductgid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select productuom_gid from pmr_mst_tproductuom where productuom_name='" + values.productuom_name + "'";
                string lsproductuomgid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "Select productgroup_gid from pmr_mst_tproductgroup where productgroup_name='" + values.productgroup_name + "'";
                string lsproductgroupgid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select tax_name from acp_mst_ttax where tax_gid='" + values.tax_name + "'";
                string lstaxname = objdbconn.GetExecuteScalar(msSQL);

                if (values.tax_name2 == null || values.tax_name2 == "")
                {
                    lstaxname2 = "0.00";
                }
                else
                {
                    msSQL = "select tax_name from acp_mst_ttax where tax_gid='" + values.tax_name2 + "'";
                    lstaxname2 = objdbconn.GetExecuteScalar(msSQL);
                }
                if (values.tax_name3 == null || values.tax_name3 == "")
                {
                    lstaxname3 = "0.00";
                }
                else
                {
                    msSQL = "select tax_name from acp_mst_ttax where tax_gid='" + values.tax_name3 + "'";
                    lstaxname3 = objdbconn.GetExecuteScalar(msSQL);
                }
                msSQL = "select percentage from acp_mst_ttax where tax_gid='" + values.tax_name + "'";
                string lspercentage1 = objdbconn.GetExecuteScalar(msSQL);
                if (values.tax_name2 == null || values.tax_name2 == "")
                {
                    lspercentage2 = "0.00";
                }
                else
                {
                    msSQL = "select percentage from acp_mst_ttax where tax_gid='" + values.tax_name2 + "'";
                    lspercentage2 = objdbconn.GetExecuteScalar(msSQL);
                }
                if (values.tax_name3 == null || values.tax_name3 == "")
                {
                    lspercentage3 = "0.00";
                }
                else
                {

                    msSQL = "select percentage from acp_mst_ttax where tax_gid='" + values.tax_name3 + "'";
                    lspercentage3 = objdbconn.GetExecuteScalar(msSQL);

                }

                if (values.tax_amount2 == null || values.tax_amount2 == "")
                {
                    lsamount2 = "0.00";
                }
                else
                {
                    lsamount2 = "'" + values.tax_amount2 + "',";
                }

                if (values.tax_amount3 == null || values.tax_amount3 == "")
                {
                    lsamount3 = "0.00";
                }
                else
                {
                    lsamount3 = "'" + values.tax_amount3 + "',";
                }
                //int i = 0;

                msSQL = " SELECT a.producttype_name FROM pmr_mst_tproducttype a " +
                  " INNER JOIN pmr_mst_tproduct b ON a.producttype_gid=b.producttype_gid  " +
                  " WHERE product_gid='" + lsproductgid + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    if (objMySqlDataReader["producttype_name"].ToString() != "Services")
                    {
                        lsquotation_type = "Sales";
                    }
                    else
                    {
                        lsquotation_type = "Services";
                    }

                }

                msSQL = " insert into smr_tmp_tsalesorderdtl( " +
                   " tmpsalesorderdtl_gid," +
                   " quotation_gid," +
                   " employee_gid," +
                   " product_gid," +
                   " productgroup_gid," +
                   " productgroup_name," +
                   " customerproduct_code," +
                   " product_name," +
                   " display_field," +
                   " product_price," +
                   " qty_quoted," +
                   " uom_gid," +
                   " uom_name," +
                   " price," +
                   " tax_name," +
                   " tax_name2," +
                   " tax_name3," +
                   " tax_amount," +
                   " tax_amount2," +
                   " tax_amount3," +
                   " tax_percentage,tax_percentage2,tax_percentage3, " +
                   " tax1_gid,tax2_gid,tax3_gid, " +
                   " margin_percentage," +
                   " margin_amount," +
                   " selling_price," +
                   " vendor_gid," +
                   " product_requireddate , " +
                   " product_requireddateremarks, " +
                   " product_code, " +
                   
                    " created_by " +
                   ")values(" +
                   "'" + msGetGid + "'," +
                   "'" + values.quotation_gid + "'," +
                   "'" + employee_gid + "'," +
                   "'" + lsproductgid + "'," +
                   "'" + lsproductgroupgid + "'," +
                   "'" + values.productgroup_name + "'," +
                   "'" + values.customerproduct_code + "'," +
                   "'" + values.product_name + "'," +
                   "'" + values.display_field + "'," +
                   "'" + values.selling_price + "'," +
                   "'" + values.quantity + "'," +
                   "'" + lsproductuomgid + "'," +
                   "'" + values.productuom_name + "'," +
                   "'" + values.totalamount + "'," +
                   "'" + lstaxname + "'," +
                   "'" + lstaxname2 + "'," +
                   "'" + lstaxname3 + "'," +
                   "'" + values.tax_amount + "',";
                if (values.tax_amount2 == "" || values.tax_amount2 == null)
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.tax_amount2 + "',";
                }
                if (values.tax_amount3 == "" || values.tax_amount3 == null)
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.tax_amount3 + "',";
                }
                msSQL += "'" + lspercentage1 + "',";
                if (lspercentage2 == "" || lspercentage2 == null)
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + lspercentage2 + "',";
                }
                if (lspercentage3 == "" || lspercentage3 == null)
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + lspercentage3 + "',";
                }
                msSQL += "'" + values.tax_gid + "'," +
                   "'" + values.tax_gid2 + "'," +
                   "'" + values.tax_gid3 + "'," +
                   "'" + values.discountpercentage + "'," +
                   "'" + values.discountamount + "'," +
                   "'" + "0.00" + "'," +
                   "'" + values.vendor_gid + "'," +
                 " '" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                "'" + values.product_requireddateremarks + "', " +
                 " '" + values.product_code + "'," +
                  
                  " '" + employee_gid + "' )";


                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
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
                values.message = "Exception occured while Adding Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           

        }

        //Terms And Condition Dropdown

        public void DaGetTermsandConditions(MdlSmrTrnQuotation values)
        {
            try
            {
               
                msSQL = "  select c.template_gid, c.template_name, c.template_content from adm_mst_ttemplate c ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetTandCDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetTandCDropdown
                        {
                            template_gid = dt["template_gid"].ToString(),
                            template_name = dt["template_name"].ToString(),
                            termsandconditions = dt["template_content"].ToString()
                        });
                        values.GetTermsandConditions = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Template Name!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }
        public void DaGetOnChangeTerms(string template_gid, MdlSmrTrnQuotation values)
        {
            try
            {
                
                if (template_gid != null)
                {
                    msSQL = " select template_gid, template_name, template_content from adm_mst_ttemplate where template_gid='" + template_gid + "' ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<GetTermDropdown>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetTermDropdown
                            {
                                template_gid = dt["template_gid"].ToString(),
                                template_name = dt["template_name"].ToString(),
                                termsandconditions = dt["template_content"].ToString(),
                            });
                            values.terms_list = getModuleList;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Template Content!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }
        // on change
        public void DaGetOnChangeCustomerDtls(string customercontact_gid, MdlSmrTrnQuotation values)
        {
            try
            {
                

                if (customercontact_gid != null)
                {
                    msSQL = " select a.customercontact_gid,concat(a.address1,'   ',a.city,'   ',a.state) as address1,ifnull(a.address2,'') as address2,ifnull(a.city,'') as city, " +
                    " ifnull(a.state,'') as state,ifnull(a.country_gid,'') as country_gid,ifnull(a.zip_code,'') as zip_code, " +
                    " ifnull(a.mobile,'') as mobile,a.email,ifnull(b.country_name,'') as country_name,a.customerbranch_name,concat(a.customercontact_name) as " +
                    " customercontact_names, c.customer_gid " +
                    " from crm_mst_tcustomercontact a " +
                    " left join crm_mst_tcustomer c on a.customer_gid=c.customer_gid " +
                    " left join adm_mst_tcountry b on a.country_gid=b.country_gid " +
                    " where c.customer_gid='" + customercontact_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    var getModuleList = new List<GetCustomerDetl>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetCustomerDetl
                            {
                                customercontact_names = dt["customercontact_names"].ToString(),
                                branch_name = dt["customerbranch_name"].ToString(),
                                country_name = dt["country_name"].ToString(),
                                email = dt["email"].ToString(),
                                mobile = dt["mobile"].ToString(),
                                zip_code = dt["zip_code"].ToString(),
                                country_gid = dt["country_gid"].ToString(),
                                state = dt["state"].ToString(),
                                city = dt["city"].ToString(),
                                address2 = dt["address2"].ToString(),
                                address1 = dt["address1"].ToString(),
                                customercontact_gid = dt["customercontact_gid"].ToString(),
                                customer_gid = dt["customer_gid"].ToString(),

                            });
                            values.GetCustomerdetls = getModuleList;
                        }
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Update Customer Detailes !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }

        // Temp Summary
        public void DaGetTempProductsSummary(string employee_gid, MdlSmrTrnQuotation values)
        {
            try
            {
                double total_amount = 0.00;
                double ltotalamount = 0.00;
              
                msSQL = " Select a.tmpquotationdtl_gid, a.slno,a.tax_name,a.enquiry_gid,a.tax1_gid,a.tax_amount, " +
                    " d.productuom_name,a.quotationdtl_gid,a.quotation_gid,a.product_gid,a.product_name,  " +
                    " format(a.product_price,2) as product_price ,a.product_code,a.qty_quoted,a.product_remarks,a.uom_gid,  " +
                    " a.uom_name,format(a.price,2) as price , format(a.discount_percentage,2)as discount_percentage," +
                    " format(a.discount_amount,2)as discount_amount from smr_tmp_treceivequotationdtl a  " +
                    " left join pmr_mst_tproduct b on a.product_gid=b.product_gid " +
                    " left join pmr_mst_tproductgroup c on a.productgroup_gid=c.productgroup_gid  " +
                    " left join pmr_mst_tproductuom d on b.productuom_gid=d.productuom_gid " +
                    " left join acp_mst_tvendor e on a.vendor_gid=e.vendor_gid  " +
                    " where a.created_by='" + employee_gid + "' and b.delete_flag='N'and a.enquiry_gid is null order by a.slno asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<tempsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        total_amount += double.Parse(dt["price"].ToString());
                        ltotalamount += double.Parse(dt["price"].ToString());
                        getModuleList.Add(new tempsummary_list
                        {
                            tmpquotationdtl_gid = dt["tmpquotationdtl_gid"].ToString(),
                            qty_requested = dt["qty_quoted"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            tax_gid = dt["tax1_gid"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            slno = dt["slno"].ToString(),
                            margin_percentage = dt["discount_percentage"].ToString(),
                            margin_amount = dt["discount_amount"].ToString(),
                            price = dt["price"].ToString()
                        });
                        values.prodsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
                values.total_amount = total_amount;
                values.ltotalamount = ltotalamount;



            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Temproary Product !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }


        // Product Submit for Add Quotation

        public void DaPostAddProduct(string employee_gid, summaryprod_list values)
        {
            try
            {
                

                msGetGid1 = objcmnfunctions.GetMasterGID("VQDT");
                msGetGid = objcmnfunctions.GetMasterGID("VQNP");

                msSQL = "select product_gid from pmr_mst_tproduct where product_name='" + values.product_name + "'";
                string lsproductgid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select productuom_gid from pmr_mst_tproductuom where productuom_name='" + values.productuom_name + "'";
                string lsproductuomgid = objdbconn.GetExecuteScalar(msSQL);


                if (values.discountpercentage == null || values.discountpercentage == "")
                {
                    lsdiscountpercentage = "0.00";
                }
                else
                {
                    lsdiscountpercentage = values.discountpercentage;
                }

                if (values.discountamount == null || values.discountamount == "")
                {
                    lsdiscountamount = "0.00";
                }
                else
                {
                    lsdiscountamount = values.discountamount;
                }


                if (values.tax_name == null || values.tax_name == "")
                {
                    lstaxname = "0.00";
                }
                else
                {
                    msSQL = "select tax_name from acp_mst_ttax where tax_gid='" + values.tax_name + "'";
                    lstaxname = objdbconn.GetExecuteScalar(msSQL);
                }

                if (values.tax_name == null || values.tax_name == "")
                {
                    lspercentage1 = "0.00";
                }
                else
                {
                    msSQL = "select percentage from acp_mst_ttax where tax_gid='" + values.tax_name + "'";
                    lspercentage1 = objdbconn.GetExecuteScalar(msSQL);
                }


                if (values.tax_amount == null || values.tax_amount == "")
                {
                    lstaxamount = "0.00";
                }
                else
                {
                    lstaxamount = values.tax_amount;
                }

                int i = 0;

                msSQL = " SELECT a.producttype_name FROM pmr_mst_tproducttype a " +
                  " INNER JOIN pmr_mst_tproduct b ON a.producttype_gid=b.producttype_gid  " +
                  " WHERE product_gid='" + lsproductgid + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    if (objMySqlDataReader["producttype_name"].ToString() != "Services")
                    {
                        lsquotation_type = "Sales";
                    }
                    else
                    {
                        lsquotation_type = "Services";
                    }

                }

                msSQL = " insert into smr_tmp_treceivequotationdtl( " +
                        " tmpquotationdtl_gid," +
                        " quotation_gid," +
                        " product_gid," +
                        " product_code," +
                        " product_name," +
                        " product_price," +
                        " qty_quoted," +
                        " discount_percentage," +
                        " discount_amount," +
                        " uom_gid," +
                        " uom_name," +
                        " price," +
                        " created_by," +
                        " tax_name, " +
                        " tax1_gid, " +
                        " slno, " +
                        " quotation_type, " +
                        " tax_percentage," +
                        " tax_amount " +
                        " ) values( " +
                        "'" + msGetGid1 + "'," +
                        "'" + msGetGid + "'," +
                        "'" + lsproductgid + "'," +
                        "'" + values.product_code + "'," +
                        "'" + values.product_name + "', " +
                        "'" + values.selling_price + "', " +
                        "'" + values.quantity + "', " +
                        "'" + lsdiscountpercentage + "', " +
                        "'" + lsdiscountamount + "', " +
                        "'" + lsproductuomgid + "', " +
                        "'" + values.productuom_name + "', " +
                        "'" + values.totalamount + "', " +
                        "'" + employee_gid + "', " +
                        "'" + lstaxname + "', " +
                        "'" + values.tax_name + "', " +
                        "'" + i + 1 + "', " +
                        "'" + lsquotation_type + "'," +
                        "'" + lspercentage1 + "'," +
                        "'" + lstaxamount + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
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
                values.message = "Exception occured while Adding Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }

        public void DaGetDeleteQuotationProductSummary(string tmpquotationdtl_gid, summaryprod_list values)
        {
            try
            {
               


                msSQL = "select price from smr_tmp_treceivequotationdtl " +
                    " where tmpquotationdtl_gid='" + tmpquotationdtl_gid + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)

                {
                    lsprice = objMySqlDataReader["price"].ToString();
                }

                msSQL = " delete from smr_tmp_treceivequotationdtl " +
                        " where tmpquotationdtl_gid='" + tmpquotationdtl_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)

                {
                    values.status = true;
                    values.message = "Product Deleted Successfully!";

                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting The Product!";


                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while  Deleting The Product !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }
        string lscustomergid;
        public void DaPostQuotationToOrder(string employee_gid, string user_gid, MdlSmrTrnQuotation values)
        {
            try
            {

                

                msSQL = " select customer_gid from crm_mst_tcustomer where customer_name='" + values.customer_name + "'";
                lscustomergid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select branch_gid from hrm_mst_tbranch where branch_name='" + values.branch_name + "'";
                string lsbranchgid = objdbconn.GetExecuteScalar(msSQL);

                mssalesorderGID = objcmnfunctions.GetMasterGID("VSOP");

                if (values.salesorder_gid != "New Ref.No")
                {
                    values.salesorder_gid = values.salesorder_gid;
                }
                else
                {
                    values.salesorder_gid = objcmnfunctions.GetMasterGID("SO");
                }
                string ls_referenceno = values.salesorder_gid;


                msSQL = " select customercontact_gid,customer_contact_person,customerbranch_gid from smr_trn_treceivequotation where quotation_gid='" + values.quotation_gid + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    lscustomercontact_gid = objMySqlDataReader["customercontact_gid"].ToString();
                    lscontact_person = objMySqlDataReader["customer_contact_person"].ToString();
                    lscustomerbranch_name = objMySqlDataReader["customerbranch_gid"].ToString();
                }
                //string formattedDate = values.salesorder_date.ToString();
                string uiDateStr = values.salesorder_date;
                DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string salesorder_date = uiDate.ToString("yyyy-MM-dd");

                if (values.start_date == null || values.start_date == "")
                {
                    start_date = "0000-00-00";
                }
                else
                {
                    string uiDateStr2 = values.start_date;
                    DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    start_date = uiDate2.ToString("yyyy-MM-dd");
                }

                if (values.end_date == null || values.end_date == "")
                {
                    end_date = "0000-00-00";
                }
                else
                {
                    string uiDateStr2 = values.end_date;
                    DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    end_date = uiDate2.ToString("yyyy-MM-dd");
                }

                msSQL = " insert  into smr_trn_tsalesorder (" +
                    " salesorder_gid ," +
                    " branch_gid ," +
                    " salesorder_date," +
                    " customer_gid," +
                    " customer_name," +
                    " customer_contact_gid," +
                    " customer_contact_person," +
                    " customer_address," +
                    " customer_email, " +
                    " customer_mobile, " +
                    " customerbranch_gid," +
                    " created_by," +
                    " so_referencenumber," +
                    " so_remarks," +
                    " so_referenceno1, " +
                    " payment_days, " +
                    " delivery_days, " +
                    " Grandtotal, " +
                    " termsandconditions, " +
                    " salesorder_status, " +
                    " addon_charge, " +
                    " additional_discount, " +
                    " currency_code, " +
                    " currency_gid, " +
                    " exchange_rate, " +
                    " shipping_to, " +
                    " freight_terms, " +
                    " payment_terms, " +
                    " gst_amount," +
                    " tax_gid," +
                    " vessel_name," +
                    " salesperson_gid," +
                    " quotation_gid, " +
                    " roundoff, " +
                    " start_date, " +
                    " end_date ," +
                    " freight_charges," +
                    " buyback_charges," +
                    " packing_charges," +
                    " insurance_charges " +
                    ")values(" +
                    " '" + mssalesorderGID + "'," +
                    " '" + lsbranchgid + "'," +
                    " '" + salesorder_date + "'," +
                    " '" + lscustomergid + "'," +
                    " '" + values.customer_name + "'," +
                    " '" + lscustomercontact_gid + "'," +
                    " '" + lscontact_person + "'," +
                    " '" + values.customer_address + "'," +
                    " '" + values.customer_email + "'," +
                    " '" + values.customer_mobile + "'," +
                    "'" + lscustomerbranch_name + "'," +
                    " '" + employee_gid + "'," +
                    " '" + values.quotation_gid + "'," +
                    " '" + values.remarks + "'," +
                    " '" + ls_referenceno + "'," +
                    " '" + values.payment_days + "'," +
                    " '" + values.delivery_days + "'," +
                    " '" + values.Grandtotal + "'," +
                    " '" + values.termsandcondition + "'," +
                    " 'Approved',";
                if (values.addon_charge == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.addon_charge + "',";
                }
                if (values.additional_discount == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.additional_discount + "',";
                }
                msSQL += " '" + values.currency_code + "'," +
                     " '" + values.currencyexchange_gid + "'," +
                     " '" + values.exchange_rate + "'," +
                     " '" + values.shipping_to + "'," +
                     "'" + values.freight_terms + "'," +
                     "'" + values.payment_terms + "',";
                if (values.tax_amount == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.tax_amount + "',";
                }
                msSQL += " '" + values.tax_gid + "'," +
                    " '" + values.vessel + "'," +
                    " '" + values.user_name + "'," +
                    " '" + values.quotation_gid + "',";
                if (values.roundoff == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.roundoff + "',";
                }

                msSQL += " '" + start_date + "'," +
                          " '" + end_date + "',";

                if (values.frieght_charges == "" | values.frieght_charges == "0")
                {
                    msSQL += "'" + values.frieght_charges + "',";
                }
                else
                {
                    msSQL += "'0.00',";

                }
                if (values.buyback_charges == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.buyback_charges + "',";
                }
                if (values.packing_charges == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.packing_charges + "',";
                }
                if (values.insurance_charges == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.insurance_charges + "')";
                }
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 0)
                {
                    values.status = false;
                    values.message = "Error occurred while inserting Salesorder";

                }
                else
                {
                    msSQL = " insert  into acp_trn_torder (" +
                            " salesorder_gid ," +
                            " branch_gid ," +
                            " salesorder_date," +
                            " customer_gid," +
                            " customer_name," +
                            " customer_contact_person," +
                            " customer_contact_gid," +
                            " customerbranch_gid," +
                            " customer_address," +
                            " customer_email, " +
                            " customer_mobile, " +
                            " created_by," +
                            " so_referencenumber," +
                            " so_remarks," +
                            " so_referenceno1, " +
                            " payment_days, " +
                            " delivery_days, " +
                            " Grandtotal, " +
                            " termsandconditions, " +
                            " salesorder_status, " +
                            " addon_charge, " +
                            " additional_discount, " +
                            " currency_code, " +
                            " currency_gid, " +
                            " exchange_rate, " +
                            " shipping_to, " +
                            " vessel_name," +
                            " quotation_gid, " +
                            " roundoff, " +
                            " salesperson_gid ," +
                            " freight_charges," +
                            " buyback_charges," +
                            " packing_charges," +
                            " insurance_charges" +
                            " )values(" +
                            " '" + mssalesorderGID + "'," +
                            " '" + lsbranchgid + "'," +
                            " '" + salesorder_date + "'," +
                            " '" + values.customer_gid + "'," +
                            " '" + values.customer_name + "'," +
                            " '" + lscustomercontact_gid + "'," +
                            " '" + lscontact_person + "'," +
                            " '" + values.customerbranch_gid + "'," +
                            " '" + values.customer_address + "'," +
                            " '" + values.customer_email + "'," +
                            " '" + values.customer_mobile + "'," +
                            " '" + employee_gid + "'," +
                            " '" + values.quotation_gid + "'," +
                            " '" + values.remarks + "'," +
                            " '" + ls_referenceno + "'," +
                            " '" + values.payment_days + "'," +
                            " '" + values.delivery_days + "'," +
                            " '" + values.Grandtotal + "'," +
                            " '" + values.termsandcondition + "'," +
                            " 'Approved',";
                    if (values.addon_charge == "" | values.addon_charge == "0")
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.addon_charge + "',";
                    }

                    if (values.additional_discount == "" | values.additional_discount == "0")
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.additional_discount + "',";
                    }


                    msSQL += " '" + values.currency_code + "'," +
                    " '" + values.currencyexchange_gid + "'," +
                    " '" + values.exchange_rate + "'," +
                    " '" + values.shipping_to + "'," +
                    " '" + values.vessel + "'," +
                    " '" + values.quotation_gid + "',";
                    if (values.roundoff == "" | values.roundoff == "0")
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.roundoff + "',";
                    }
                    msSQL += " '" + values.user_name + "',";
                    if (values.frieght_charges == "" | values.frieght_charges == "0")
                    {
                        msSQL += "'" + values.frieght_charges + "',";
                    }
                    else
                    {
                        msSQL += "'0.00',";



                    }
                    if (values.buyback_charges == "" | values.buyback_charges == "0")
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.buyback_charges + "',";
                    }
                    if (values.packing_charges == "" | values.packing_charges == "0")
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.packing_charges + "',";
                    }
                    if (values.insurance_charges == "")
                    {
                        msSQL += "'" + values.insurance_charges + "',";
                    }
                    else
                    {
                        msSQL += "'0.00')";
                    }


                    mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (mnResult2 == 0)
                {
                    values.status = false;
                    values.message = "Error While Raise Sales order";
                }

                if (mnResult2 == 1)
                {

                    msSQL = " select " +
                        " tmpsalesorderdtl_gid," +
                        " salesorder_gid," +
                        " product_gid," +
                        " product_name," +
                        " product_price," +
                        " qty_quoted," +
                        " margin_percentage," +
                        " margin_amount," +
                        " uom_gid," +
                        " uom_name," +
                        " price," +
                        " tax_name," +
                        " tax_amount," +
                        " margin_percentage," +
                        " margin_amount," +
                        " slno," +
                        " selling_price," +
                        " order_type " +
                        " from smr_tmp_tsalesorderdtl" +
                        " where employee_gid='" + employee_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<summarys_lists>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new summarys_lists
                            {
                                quotation_gid = dt["tmpsalesorderdtl_gid"].ToString(),
                                product_gid = dt["product_gid"].ToString(),
                                product_name = dt["product_name"].ToString(),
                                productuom_gid = dt["uom_gid"].ToString(),
                                productuom_name = dt["uom_name"].ToString(),
                                price = dt["product_price"].ToString(),
                                quantity = dt["qty_quoted"].ToString(),
                                discountpercentage = dt["margin_percentage"].ToString(),
                                discountamount = dt["margin_amount"].ToString(),
                                tax_name = dt["tax_name"].ToString(),
                                tax_amount = dt["tax_amount"].ToString(),
                                totalamount = dt["price"].ToString(),
                                selling_price = dt["selling_price"].ToString(),
                                slno = dt["slno"].ToString(),

                            });
                            values.summarys_lists = getModuleList;

                            msSQL = "select type from smr_trn_tsalesorderdtl where salesorder_gid='" + mssalesorderGID + "' ";
                            string lsquotation_type = objdbconn.GetExecuteScalar(msSQL);

                            string mssalesorderGID1 = objcmnfunctions.GetMasterGID("VSDC");
                            if (mssalesorderGID1 == "E")
                            {
                                values.status = false;
                                values.message = "Create sequence code for VSDC";
                            }
                            int i = 0;
                            msSQL = " insert into smr_trn_tsalesorderdtl (" +
                                " salesorderdtl_gid ," +
                                " salesorder_gid," +
                                " product_gid ," +
                                " product_name," +
                                " product_price," +
                                " type," +
                                " qty_quoted," +
                                " discount_percentage," +
                                " discount_amount," +
                                " tax_amount ," +
                                " uom_gid," +
                                " uom_name," +
                                " price," +
                                " tax_name," +
                                " salesorder_refno," +
                                " slno" +
                                ")values(" +
                                " '" + mssalesorderGID1 + "'," +
                                " '" + mssalesorderGID + "'," +
                                " '" + dt["product_gid"] + "'," +
                                " '" + dt["product_name"] + "'," +
                                " '" + dt["product_price"] + "'," +
                                " '" + lsquotation_type + "'," +
                                " '" + dt["qty_quoted"] + "'," +
                                " '" + dt["margin_percentage"] + "'," +
                                " '" + dt["margin_amount"] + "'," +
                                " '" + dt["tax_amount"] + "'," +
                                " '" + dt["uom_gid"] + "'," +
                                " '" + dt["uom_name"] + "'," +
                                " '" + dt["price"] + "'," +
                                " '" + dt["tax_name"] + "'," +
                                " '" + ls_referenceno + "'," +
                                " '" + i + 1 + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 0)
                            {

                                values.status = true;
                                values.message = "Error occurred while Insertion Product Details";
                            }

                            else
                            {
                                msSQL = " insert into acp_trn_torderdtl (" +
                                    " salesorderdtl_gid ," +
                                    " salesorder_gid," +
                                    " product_gid ," +
                                    " product_name," +
                                    " product_price," +
                                    " qty_quoted," +
                                    " discount_percentage," +
                                    " discount_amount," +
                                    " tax_amount ," +
                                    " uom_gid," +
                                    " uom_name," +
                                    " price," +
                                    " tax_name," +
                                    " slno, " +
                                    " salesorder_refno" +
                                    ")values(" +
                                    " '" + mssalesorderGID1 + "'," +
                                    " '" + mssalesorderGID + "'," +
                                     " '" + dt["product_gid"] + "'," +
                                     " '" + dt["product_name"] + "'," +
                                    " '" + dt["product_price"] + "'," +
                                    " '" + dt["qty_quoted"] + "'," +
                                    " '" + dt["margin_percentage"] + "'," +
                                    " '" + dt["margin_amount"] + "'," +
                                    " '" + dt["tax_amount"] + "'," +
                                    " '" + dt["uom_gid"] + "'," +
                                    " '" + dt["uom_name"] + "'," +
                                    " '" + dt["price"] + "'," +
                                    " '" + dt["tax_name"] + "'," +
                                    " '" + dt["slno"] + "'," +
                                    " '" + ls_referenceno + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                        }
                    }
                }

                if (mnResult == 1)
                {
                    msSQL = "select type from smr_trn_tsalesorderdtl where salesorder_gid='" + mssalesorderGID + "' ";
                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                    if (objMySqlDataReader.HasRows == true)
                    {
                        lstype1 = "Sales";
                    }
                    else
                    {
                        lstype1 = "Services";
                    }


                    msSQL = " update smr_trn_tsalesorder set so_type='" + lstype1 + "' where salesorder_gid='" + mssalesorderGID + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    msSQL = " update acp_trn_torder set so_type='" + lstype1 + "' where salesorder_gid='" + mssalesorderGID + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }


                if (mnResult != 0)

                {
                    values.status = true;
                    values.message = "Sales Order Raised Successfully!";

                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Sales Order!";


                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Adding Sales Order!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }
        // Overall submit for Direct Quotation

        public void DaPostDirectQuotation(string employee_gid, Post_List values)
        {
            try
            {
                
                msSQL = " select * from smr_tmp_treceivequotationdtl " +
                     " where created_by='" + employee_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 0)
                {
                    values.status = false;
                    values.message = "Select one Product to Raise Quotation";
                }

                msGetGid = objcmnfunctions.GetMasterGID("VQDC");
                if (msGetGid == "E")
                {
                    values.status = false;
                    values.message = "Create Sequence Code VQDC for Raise Enquiry";
                }
                if (values.tax_amount == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.tax_amount + "',";
                }


                if (msGetGid == "New Ref.No")
                {
                    msGetGid = ("quotation_gid");
                }
                else
                {
                    msGetGid = objcmnfunctions.GetMasterGID("VQDC");

                }

                msSQL = " select customerbranch_name from crm_mst_tcustomercontact where customercontact_gid=  '" + values.cuscontact_gid + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                lscustomercontact_names = values.customercontact_names;
                string lsquotation_status = "Approved";
                string lsadditional_discount_l = "0.00";
                string lsaddon_charge_l = "0.00";
                string lsgrandtotal_l = "0.00";
                string lsgst_percentage = "0.00";
                if (objMySqlDataReader.HasRows == true)
                {
                    lscustomerbranch_name = objMySqlDataReader["customerbranch_name"].ToString();
                }

                string uiDateStr = values.quotation_date;
                DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string quotation_date = uiDate.ToString("yyyy-MM-dd");

                msSQL = " insert  into smr_trn_treceivequotation (" +
                         " quotation_gid ," +
                         " quotation_referencenumber ," +
                         " branch_gid ," +
                         " quotation_date," +
                         " customer_gid," +
                         " customer_name," +
                         " customerbranch_gid," +
                         " customercontact_gid," +
                         " customer_contact_person," +
                         " created_by," +
                         " quotation_remarks," +
                         " quotation_referenceno1, " +
                         " payment_days, " +
                         " delivery_days, " +
                         " Grandtotal, " +
                         " termsandconditions, " +
                         " quotation_status, " +
                         " contact_no, " +
                         " customer_address, " +
                         " contact_mail, " +
                         " addon_charge, " +
                         " additional_discount, " +
                         " addon_charge_l, " +
                         " additional_discount_l, " +
                         " grandtotal_l, " +
                         " currency_code, " +
                         " exchange_rate, " +
                         " currency_gid, " +
                         " total_amount," +
                         " gst_percentage," +
                         " tax_gid," +
                         " salesperson_gid," +
                         " vessel_name, " +
                         " freight_terms, " +
                         " payment_terms," +
                         " tax_name," +
                         " pricingsheet_gid, " +
                         " pricingsheet_refno, " +
                         " roundoff, " +
                         " total_price, " +
                         " freight_charges," +
                         " buyback_charges," +
                         " packing_charges," +
                         " created_date ," +
                         " insurance_charges " +
                         ") values ( " +
                         " '" + msGetGid + "'," +
                         " '" + msGetGid + "'," +
                         " '" + values.branch_name + "'," +
                         " '" + quotation_date + "'," +
                         " '" + values.customer_gid + "'," +
                         " '" + values.customer_name + "'," +
                         " '" + lscustomerbranch_name + "'," +
                         " '" + values.cuscontact_gid + "'," +
                         " '" + lscustomercontact_names + "'," +
                         " '" + employee_gid + "'," +
                         " '" + values.quotation_remarks + "'," +
                         " '" + values.quotation_referenceno1 + "'," +
                         " '" + values.payment_days + "'," +
                         " '" + values.delivery_days + "'," +
                         "'" + values.grandtotal + "', " +
                         " '" + values.termsandconditions + "'," +
                         " '" + lsquotation_status + "'," +
                         " '" + values.mobile + "'," +
                         " '" + values.address1 + "'," +
                         " '" + values.email + "'," +
                         "'" + values.addoncharge + "'," +
                         "'" + values.additional_discount + "'," +
                         "'" + lsaddon_charge_l + "'," +
                         "'" + lsadditional_discount_l + "'," +
                         "'" + lsgrandtotal_l + "', " +
                         "'" + values.currencyexchange_gid + "'," +
                         "'" + values.exchange_rate + "'," +
                         "'" + values.currency_code + "',";
                if (values.total_amount == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.total_amount + "',";
                }
                msSQL += "'" + lsgst_percentage + "', " +
                "'" + values.tax4_gid + "'," +
                "'" + values.user_name + "'," +
                "'" + values.vessel_name + "'," +
                "'" + values.freight_terms + "'," +
                "'" + values.payment_terms + "'," +
                "'" + values.tax_name4 + "', " +
                "'" + values.pricingsheet_gid + "', ";

                if (values.pricingsheet_refno == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.pricingsheet_refno + "',";
                }
                if (values.roundoff == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.roundoff + "',";
                }
                msSQL += "'" + values.producttotalamount + "',";

                if (values.freightcharges == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.freightcharges + "',";
                }
                if (values.buybackcharges == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.buybackcharges + "',";
                }
                if (values.packing_charges == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.packing_charges + "',";
                }
                msSQL += "'" + DateTime.Now.ToString("yyyy-MM-dd") + "',";
                if (values.insurance_charges == "")
                {
                    msSQL += "'0.00')";
                }
                else
                {
                    msSQL += "'" + values.insurance_charges + "')";
                }
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 0)
                {
                    values.status = false;
                    values.message = "Error Occured while inserting Quotation";
                }

                else
                {
                    msSQL = " select " +
                          " tmpquotationdtl_gid," +
                          " quotation_gid," +
                          " product_gid," +
                          " productgroup_gid," +
                          " productgroup_name," +
                          " product_name," +
                          " product_code," +
                          " product_price," +
                          " qty_quoted," +
                          " format(discount_percentage,2) as discount_percentage," +
                          " format(discount_amount,2) as discount_amount, " +
                          " uom_gid," +
                          " uom_name," +
                          " format(price,2) as price," +
                          " tax_name, " +
                          " tax_name2, " +
                          " tax_name3, " +
                          " slno, " +
                          " productrequireddate_remarks, " +
                          " tax_amount " +
                           " tax_amount, " +
                           " customerproduct_code " +

                           " from smr_tmp_treceivequotationdtl  where created_by='" + employee_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<Post_List>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new Post_List
                            {

                                tmpquotationdtl_gid = dt["tmpquotationdtl_gid"].ToString(),
                                quotation_gid = dt["quotation_gid"].ToString(),
                                product_gid = dt["product_gid"].ToString(),
                                productgroup_gid = dt["productgroup_gid"].ToString(),
                                customerproduct_code = dt["customerproduct_code"].ToString(),
                                product_name = dt["product_name"].ToString(),
                                product_price = dt["product_price"].ToString(),
                                quantity = dt["qty_quoted"].ToString(),
                                discountpercentage = dt["discount_amount"].ToString(),
                                discountamount = dt["discount_amount"].ToString(),
                                productuom_gid = dt["uom_gid"].ToString(),
                                productuom_name = dt["uom_gid"].ToString(),
                                tax_name = dt["tax_name"].ToString(),
                                slno = dt["slno"].ToString(),
                                tax_amount = dt["tax_amount"].ToString(),
                                price = dt["price"].ToString(),
                            });

                            msgetGid2 = objcmnfunctions.GetMasterGID("VQDC");
                            if (msgetGid2 == "E")
                            {
                                values.status = true;
                                values.message = "Create Sequence Code PPDC for Sales Enquiry Details";
                            }


                            msSQL = "insert into smr_trn_treceivequotationdtl (" +
                                    " quotationdtl_gid ," +
                                    " quotation_gid," +
                                    " product_gid ," +
                                    " productgroup_gid," +
                                    " productgroup_name," +
                                    " product_name," +
                                    " product_code," +
                                    " product_price," +
                                    " qty_quoted," +
                                    " discount_percentage," +
                                    " discount_amount," +
                                    " uom_gid," +
                                    " uom_name," +
                                    " price," +
                                    " tax_name," +
                                    " slno," +
                                    " tax_amount " +
                                    ")values(" +
                                    " '" + msgetGid2 + "'," +
                                    " '" + msGetGid + "'," +
                                    " '" + dt["product_gid"].ToString() + "'," +
                                    " '" + dt["productgroup_gid"].ToString() + "'," +
                                    " '" + dt["productgroup_name"].ToString() + "'," +
                                    " '" + dt["product_name"].ToString() + "'," +
                                    " '" + dt["product_code"].ToString() + "'," +
                                    " '" + dt["product_price"].ToString() + "'," +
                                    " '" + dt["qty_quoted"].ToString() + "'," +
                                    " '" + dt["discount_percentage"].ToString() + "'," +
                                    " '" + dt["discount_amount"].ToString().Replace(",", "").Trim() + "'," +
                                    " '" + dt["uom_gid"].ToString() + "'," +
                                    " '" + dt["uom_name"].ToString() + "'," +
                                    " '" + dt["price"].ToString().Replace(",", "").Trim() + "'," +
                                    " '" + dt["tax_name"].ToString() + "'," +
                                    " '" + dt["slno"].ToString() + "', ";
                            msSQL += " '" + dt["tax_amount"].ToString() + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 0)
                            {
                                values.status = false;
                                values.message = "Error occured while Inserting into Quotationdtl";
                            }
                        }
                    }


                    msSQL = "select distinct quotation_type from smr_tmp_treceivequotationdtl where created_by='" + employee_gid + "' ";
                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                    if (objMySqlDataReader.HasRows == true)
                    {
                        lsquotation_type = "sales";


                    }

                    else
                    {
                        lsquotation_type = "Service";
                    }

                    msSQL = " update smr_trn_treceivequotation set quotation_type='" + lsquotation_type + "' where quotation_gid='" + msGetGid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = " update smr_trn_treceivequotation Set " +
                " leadbank_gid = '" + values.customer_gid + "' " + "," +
                " leadbankcontact_gid = '" + lscustomercontact_gid + "' " +
                " where quotation_gid='" + msGetGid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }


                msgetGid4 = objcmnfunctions.GetMasterGID("PODC");
                {
                    msSQL = " insert into smr_trn_tapproval ( " +
                            " approval_gid, " +
                            " approved_by, " +
                            " approved_date, " +
                            " submodule_gid, " +
                            " qoapproval_gid " +
                            " ) values ( " +
                            "'" + msgetGid4 + "'," +
                            " '" + employee_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                            "'SMRSMRQAP'," +
                            "'" + msGetGid + "') ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = "select approval_flag from smr_trn_tapproval where submodule_gid='SMRSMRQAP' and qoapproval_gid='" + msGetGid + "' ";
                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                    if (objMySqlDataReader.HasRows == false)
                    {
                        msSQL = " Update smr_trn_treceivequotation Set " +
                               " quotation_status = 'Approved', " +
                               " approved_by = '" + employee_gid + "', " +
                               " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                               " where quotation_gid = '" + msGetGid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    else
                    {
                        msSQL = "select approved_by from smr_trn_tapproval where submodule_gid='SMRSMRQAP' and qoapproval_gid='" + msGetGid + "'";
                        objMySqlDataReader1 = objdbconn.GetDataReader(msSQL);
                    }
                    if (objMySqlDataReader1.RecordsAffected == 1)
                    {
                        msSQL = " update smr_trn_tapproval set " +
                       " approval_flag = 'Y', " +
                       " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                       " where approved_by = '" + employee_gid + "'" +
                       " and qoapproval_gid = '" + msGetGid + "' and submodule_gid='SMRSMRQAP'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        msSQL = " Update smr_trn_treceivequotation Set " +
                               " quotation_status = 'Approved', " +
                       " approved_by = '" + employee_gid + "', " +
                       " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                       " where quotation_gid = '" + msGetGid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    else if (objMySqlDataReader1.RecordsAffected > 1)
                    {
                        msSQL = " update smr_trn_tapproval set " +
                               " approval_flag = 'Y', " +
                               " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                               " where approved_by = '" + employee_gid + "'" +
                               " and quotation_gid = '" + msGetGid + "' and submodule_gid='SMRSMRQAP'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }

                if (mnResult == 0 || mnResult != 0)
                {
                    msSQL = " delete from smr_tmp_treceivequotationdtl " +
                         " where created_by='" + employee_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }


                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Quotation Raised Successfully!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Raising Quotation!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Submiting  Quotation !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }
        public void DaGetViewQuotationSummary(string quotation_gid, MdlSmrTrnQuotation values)
        {
            try
            {
                
                msSQL = " select b.leadbank_gid, k.lead2campaign_gid,a.quotation_gid,l.currency_code,a.vessel_name,a.freight_terms,a.payment_terms,concat(a.customerbranch_gid,' | ',a.customer_contact_person)as contact_person,a.salesperson_gid,a.termsandconditions,date_format(a.quotation_date,'%d-%m-%Y') as quotation_date,a.customerenquiryref_number,a.quotation_remarks, " +
                    " a.quotation_referenceno1,a.payment_days,d.product_code,e.branch_name,h.currency_code as code,a.exchange_rate," +
                    " a.contact_mail,a.contact_no,a.customer_address,format(a.Grandtotal_l,2) as Grandtotal_l," +
                    " a.delivery_days,format(a.Grandtotal,2) as Grandtotal ,format(a.addon_charge,2) as addon_charge,format(a.additional_discount,2) as additional_discount," +
                    " format(sum(d.price),2) as total_value,a.customer_name, format(a.gst_percentage,2)as gst_percentage,a.tax_gid,i.tax_name,format(a.total_amount,2)as total_amount,format(a.total_price,2) as total_price, " +
                    " b.leadbank_address1,b.leadbank_address2,b.leadbank_city,b.leadbank_state,b.leadbank_pin,a.payment_days,a.delivery_days,d.slno,d.product_price, " +
                    " c.leadbankcontact_name,c.mobile,c.email,a.pricingsheet_refno,concat(j.user_code,' ','/',' ',j.user_firstname,' ',j.user_lastname) as user_firstname, " +
                    " format(a.freight_charges,2)as freight_charges,format(a.buyback_charges,2)as buyback_charges, " +
                    " format(a.packing_charges,2)as packing_charges,format(a.insurance_charges,2)as insurance_charges,d.margin_percentage, " +
                    " d.slno,d.productgroup_name,d.customerproduct_code,d.display_field,d.product_name,d.uom_name,d.margin_amount,d.tax_amount,d.tax_amount2,d.tax_amount3," +
                    " d.uom_name,d.qty_quoted,d.selling_price,d.discount_percentage,d.discount_amount,d.tax_name,d.tax_name2,d.tax_name3,d.price,cast(date_format(d.product_requireddate,'%d-%m-%Y')as char) as product_requireddate, " +
                    " format(a.roundoff,2)as roundoff from smr_trn_treceivequotation a " +
                    " left join crm_trn_tleadbank b on b.leadbank_gid=a.customer_gid" +
                    " left join crm_trn_tlead2campaign k on k.leadbank_gid=b.leadbank_gid" +
                    " left join crm_mst_tcustomer g on g.customer_gid=b.customer_gid" +
                    " left join crm_trn_tcurrencyexchange h on h.currencyexchange_gid=g.currency_gid" +
                    " left join crm_trn_tleadbankcontact c on c.leadbank_gid=b.leadbank_gid " +
                    " left join smr_trn_treceivequotationdtl d on d.quotation_gid=a.quotation_gid " +
                    " left join pmr_mst_tproduct f on d.product_gid=f.product_gid " +
                    " left join hrm_mst_tbranch e on e.branch_gid=a.branch_gid" +
                    " left join acp_mst_ttax i on i.tax_gid = a.tax_gid " +
                    " left join crm_trn_tcurrencyexchange l on a.currency_gid = l.currencyexchange_gid " +
                    " left join adm_mst_tuser j on j.user_gid  = a.salesperson_gid " +
                    " where a.quotation_gid='" + quotation_gid + "' group by a.quotation_gid , d.product_gid";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetSummaryList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetSummaryList
                        {


                            quotation_gid = dt["quotation_gid"].ToString(),
                            quotation_date = dt["quotation_date"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            contact_person = dt["contact_person"].ToString(),
                            contact_no = dt["contact_no"].ToString(),
                            contact_mail = dt["contact_mail"].ToString(),
                            customer_address = dt["customer_address"].ToString(),
                            quotation_remarks = dt["quotation_remarks"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                            freight_terms = dt["freight_terms"].ToString(),
                            payment_terms = dt["payment_terms"].ToString(),
                            slno = dt["slno"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            //customerproduct_code = dt["customerproduct_code"].ToString(),
                            //display_field = dt["display_field"].ToString(),
                            productuom_name = dt["uom_name"].ToString(),
                            quantity = dt["qty_quoted"].ToString(),
                            selling_price = dt["selling_price"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            //tax_name2 = dt["tax_name2"].ToString(),
                            //tax_name3 = dt["tax_name3"].ToString(),
                            product_requireddate = dt["product_requireddate"].ToString(),
                            payment_days = dt["payment_days"].ToString(),
                            delivery_days = dt["delivery_days"].ToString(),
                            total_price = dt["total_price"].ToString(),
                            total_amount = dt["total_amount"].ToString(),
                            addon_charge = dt["addon_charge"].ToString(),
                            additional_discount = dt["additional_discount"].ToString(),
                            freight_charges = dt["freight_charges"].ToString(),
                            buyback_charges = dt["buyback_charges"].ToString(),
                            packing_charges = dt["packing_charges"].ToString(),
                            insurance_charges = dt["insurance_charges"].ToString(),
                            roundoff = dt["roundoff"].ToString(),
                            Grandtotal = dt["Grandtotal"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            //discountpercentage = dt["discount_percentage"].ToString(),
                            discountamount = dt["discount_amount"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            //tax_amount2 = dt["tax_amount2"].ToString(),
                            //tax_amount3 = dt["tax_amount3"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            price = dt["price"].ToString(),
                            lead2campaign_gid = dt["lead2campaign_gid"].ToString()
                        });
                        values.SO_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Loading Quootation View !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }

        public void DaGetOnchangeCurrency(string currencyexchange_gid, MdlSmrTrnQuotation values)
        {
            try
            {
               
                msSQL = " select currencyexchange_gid,currency_code,exchange_rate from crm_trn_tcurrencyexchange " +
                " where currencyexchange_gid='" + currencyexchange_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetOnchangecurrency>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetOnchangecurrency
                        {

                            exchange_rate = dt["exchange_rate"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                        });
                        values.GetOnchangecurrency = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured whileGetting Currency Code !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }

        public void DaGetProductdetails(string quotation_gid, MdlSmrTrnQuotation values)
        {
            try
            {
               
                msSQL = "select customerproduct_code,product_code,product_name,qty_quoted from smr_trn_treceivequotationdtl where quotation_gid='" + quotation_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<productlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new productlist
                        {

                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            qty_quoted = dt["qty_quoted"].ToString(),
                        });
                        values.product_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product Detailes !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }
        public Quotationlist DaGetQuotationamend(string quotation_gid)

        {
            try { 
            Quotationlist objQuotationamendlist = new Quotationlist();

            msSQL = "select a.quotation_gid," + "date_format(a.quotation_date, '%d-%m-%y') as quotation_date, " +
                        "a.quotation_referencenumber," + "a.customer_gid," + "a.customer_address," +
                        "concat(customer_contact_person)as contact_person," +
                        "a.quotation_remarks," + "a.created_by," + "format(a.total_amount,2) as total_amount," +
                        "a.payment_days," + "a.delivery_days ," + "a.quotation_status," + "a.contact_no," + "a.contact_mail," + "a.customer_name," +
                        "format(a.Grandtotal,2) as Grandtotal, " + "format(a.Grandtotal_l,2) as Grandtotal_l, " +
                        "format(a.addon_charge,2) as addon_charge, " + "a.quotation_referenceno1," +
                        "format(a.additional_discount,2) as additional_discount," + "a.currency_code, " +
                        "a.exchange_rate, " + "a.currency_gid , " + "a.termsandconditions, " + "a.created_date," +
                        "a.branch_gid," + " b.branch_name," + " format(a.gst_percentage,2)as gst_percentage," +
                        " a.tax_gid," + " a.tax_name," + " a.salesperson_gid," + " a.vessel_name," + " a.enquiry_refno," +
                        " format(a.total_price,2)as total_price,a.pricingsheet_gid,a.pricingsheet_refno,a.freight_terms,a.payment_terms,a.customerenquiryref_number,a.roundoff, " +
                        " format(ifnull(a.freight_charges,0.00),2)as freight_charges," +
                        " format(ifnull(a.buyback_charges,0.00),2)as buyback_charges," + " format(ifnull(a.packing_charges,0.00),2)as packing_charges," +
                        " format(ifnull(a.insurance_charges,0.00),2)as insurance_charges,a.enquiry_gid " +
                        " from smr_trn_treceivequotation a " + " left join hrm_mst_tbranch b on b.branch_gid=a.branch_gid " +
                        " where quotation_gid ='" + quotation_gid + "'";
            objMySqlDataReader = objdbconn.GetDataReader(msSQL);
            if (objMySqlDataReader.HasRows == true)
            {
                objMySqlDataReader.Read();
                objQuotationamendlist.quotation_gid = objMySqlDataReader["quotation_gid"].ToString();
                objQuotationamendlist.quotation_date = objMySqlDataReader["quotation_date"].ToString();
                objQuotationamendlist.quotation_referencenumber = objMySqlDataReader["quotation_referencenumber"].ToString();
                objQuotationamendlist.customer_gid = objMySqlDataReader["customer_gid"].ToString();
                objQuotationamendlist.quotation_remarks = objMySqlDataReader["quotation_remarks"].ToString();
                objQuotationamendlist.customer_name = objMySqlDataReader["customer_name"].ToString();
                objQuotationamendlist.customer_address = objMySqlDataReader["customer_address"].ToString();
                objQuotationamendlist.contact_person = objMySqlDataReader["contact_person"].ToString();
                objQuotationamendlist.total_amount = objMySqlDataReader["total_amount"].ToString();
                objQuotationamendlist.payment_days = objMySqlDataReader["payment_days"].ToString();
                objQuotationamendlist.delivery_days = objMySqlDataReader["delivery_days"].ToString();
                objQuotationamendlist.quotation_status = objMySqlDataReader["quotation_status"].ToString();
                objQuotationamendlist.contact_no = objMySqlDataReader["contact_no"].ToString();
                objQuotationamendlist.contact_mail = objMySqlDataReader["contact_mail"].ToString();
                objQuotationamendlist.Grandtotal = objMySqlDataReader["Grandtotal"].ToString();
                objQuotationamendlist.Grandtotal_l = objMySqlDataReader["Grandtotal_l"].ToString();
                objQuotationamendlist.addon_charge = objMySqlDataReader["addon_charge"].ToString();
                objQuotationamendlist.quotation_referenceno1 = objMySqlDataReader["quotation_referenceno1"].ToString();
                objQuotationamendlist.additional_discount = objMySqlDataReader["additional_discount"].ToString();
                objQuotationamendlist.currency_gid = objMySqlDataReader["currency_code"].ToString();
                objQuotationamendlist.exchange_rate = objMySqlDataReader["exchange_rate"].ToString();
                objQuotationamendlist.currency_gid = objMySqlDataReader["currency_gid"].ToString();
                objQuotationamendlist.termsandconditions = objMySqlDataReader["termsandconditions"].ToString();
                objQuotationamendlist.created_date = objMySqlDataReader["created_date"].ToString();
                objQuotationamendlist.branch_gid = objMySqlDataReader["branch_gid"].ToString();
                objQuotationamendlist.gst_percentage = objMySqlDataReader["gst_percentage"].ToString();
                objQuotationamendlist.tax_gid = objMySqlDataReader["tax_gid"].ToString();
                objQuotationamendlist.tax_name = objMySqlDataReader["tax_name"].ToString();
                objQuotationamendlist.salesperson_gid = objMySqlDataReader["salesperson_gid"].ToString();
                objQuotationamendlist.vessel_name = objMySqlDataReader["vessel_name"].ToString();
                objQuotationamendlist.enquiry_refno = objMySqlDataReader["enquiry_refno"].ToString();
                objQuotationamendlist.total_price = objMySqlDataReader["total_price"].ToString();
                objQuotationamendlist.pricingsheet_gid = objMySqlDataReader["pricingsheet_gid"].ToString();
                objQuotationamendlist.pricingsheet_refno = objMySqlDataReader["pricingsheet_refno"].ToString();
                objQuotationamendlist.freight_terms = objMySqlDataReader["freight_terms"].ToString();
                objQuotationamendlist.payment_terms = objMySqlDataReader["payment_terms"].ToString();
                objQuotationamendlist.customerenquiryref_number = objMySqlDataReader["customerenquiryref_number"].ToString();
                objQuotationamendlist.roundoff = objMySqlDataReader["roundoff"].ToString();
                objQuotationamendlist.freight_charges = objMySqlDataReader["freight_charges"].ToString();
                objQuotationamendlist.buyback_charges = objMySqlDataReader["buyback_charges"].ToString();
                objQuotationamendlist.packing_charges = objMySqlDataReader["packing_charges"].ToString();
                objQuotationamendlist.insurance_charges = objMySqlDataReader["insurance_charges"].ToString();
                objQuotationamendlist.enquiry_gid = objMySqlDataReader["enquiry_gid"].ToString();
                objQuotationamendlist.branch_name = objMySqlDataReader["branch_name"].ToString();

            }
            return objQuotationamendlist;
            }
            catch (Exception ex)
            {
                ex.ToString();
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                return null;
            }
        }


        public void DaGetQuotattionproductSummary(MdlSmrTrnQuotation values, string quotation_gid)
        {
            try
            {
                msSQL = "select a.tmpquotationdtl_gid as quotationdtl_gid,a.quotation_gid,a.customerproduct_code,a.productgroup_gid,a.productgroup_name,a.product_gid,a.product_name,a.product_code,a.qty_quoted,a.slno, " +
                        " a.product_price,a.margin_percentage,a.selling_price,tax_percentage, tax_amount,tax_name,tax_name2,tax_name3,tax_percentage2,tax_percentage3, " +
                        " a.margin_amount ,a.product_remarks,tax_amount2,tax_amount3,tax1_gid,tax2_gid,tax3_gid,a.discount_percentage,a.discount_amount,a.mrp,concat(tax_name,'-',tax_amount) as tax, " +
                        " a.uom_gid,a.uom_name,a.payment_days,a.delivery_period,a.price,a.display_field,c.vendor_companyname,a.vendor_gid,a.vendor_name, " +
                        " a.product_status, date_format(a.product_requireddate,'%Y-%m-%d') as product_requireddate,a.productrequireddate_remarks " +
                        " from smr_tmp_treceivequotationdtl a " +
                        " left join pmr_mst_tproduct b on a.product_gid=b.product_gid " +
                        " left join acp_mst_tvendor c on a.vendor_gid=c.vendor_gid " +
                        " union" +
                        " select a.quotationdtl_gid,a.quotation_gid,a.customerproduct_code,a.productgroup_gid,a.productgroup_name,a.product_gid,a.product_name,a.product_code,a.qty_quoted,a.slno, " +
                        " a.product_price,a.margin_percentage,a.selling_price,tax_percentage, tax_amount,tax_name,tax_name2,tax_name3,tax_percentage2,tax_percentage3, " +
                        " a.margin_amount ,a.product_remarks,tax_amount2,tax_amount3,tax1_gid,tax2_gid,tax3_gid,a.discount_percentage,a.discount_amount,a.mrp,concat(tax_name,'-',tax_amount) as tax,  " +
                        " a.uom_gid,a.uom_name,a.payment_days,a.delivery_period,a.price,a.display_field,c.vendor_companyname,a.vendor_gid,a.vendor_name, " +
                        " a.product_status, date_format(a.product_requireddate,'%Y-%m-%d') as product_requireddate,a.productrequireddate_remarks " +
                        " from smr_trn_treceivequotationdtl a " +
                        " left join pmr_mst_tproduct b on a.product_gid=b.product_gid " +
                        " left join acp_mst_tvendor c on a.vendor_gid=c.vendor_gid " +
                        " where a.quotation_gid='" + quotation_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                double grand_total = 0.00;
                var getModuleList = new List<Productsummaryamend_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        grand_total += double.Parse(dt["price"].ToString());
                        getModuleList.Add(new Productsummaryamend_list
                        {
                            quotation_gid = dt["quotation_gid"].ToString(),
                            tmpquotationdtl_gid = dt["quotationdtl_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            customerproduct_code = dt["customerproduct_code"].ToString(),
                            uom_name = dt["uom_name"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            //Details = dt["Details"].ToString(),
                            discount = dt["discount_amount"].ToString(),
                            selling_price = dt["selling_price"].ToString(),
                            product_total = dt["price"].ToString(),
                            tax = dt["tax"].ToString(),
                            qty_quoted = dt["qty_quoted"].ToString(),
                            mrp = dt["selling_price"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),
                            grand_total = grand_total


                        });
                        values.Productsummaryamend_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product Detailes !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DagetDeleteQuotation(string quotation_gid, string employee_gid, MdlSmrTrnQuotation values)
        {
            try
            {
                
                msSQL = " insert into smr_trn_tsalesdelete ( " +
                    " record_gid, " +
                    " deleted_by, " +
                    " deleted_date, " +
                    " record_reference " +
                   " ) values ( " +
              " '" + quotation_gid + "', " +
              " '" + employee_gid + "', " +
              "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
              " 'Quotation') ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    msSQL = " update smr_trn_treceivequotation set delete_flag='Y' where quotation_gid='" + quotation_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Quotation Deleted Successfully";
                }
                else
                {
                    {
                        values.status = false;
                        values.message = "Error While Deleting Quotation";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Quotation!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }

        public quotationhistorylist DaGetquotationhistorydata(string quotation_gid)
        {
            try
            {
                quotationhistorylist objquotationhistorylist = new quotationhistorylist();
                {

                    msSQL = " select quotation_gid,date_format(quotation_date,'%d-%m-%Y') as quotation_date,customer_name, " +
                            " quotation_remarks,quotation_referenceno1 " +
                            " from smr_trn_treceivequotation where quotation_gid='" + quotation_gid + "'";
                }

                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows)
                {
                    objMySqlDataReader.Read();
                    objquotationhistorylist.quotation_gid = objMySqlDataReader["quotation_gid"].ToString();
                    objquotationhistorylist.quotation_referenceno1 = objMySqlDataReader["quotation_referenceno1"].ToString();
                    objquotationhistorylist.quotation_date = objMySqlDataReader["quotation_date"].ToString();
                    objquotationhistorylist.customer_name = objMySqlDataReader["customer_name"].ToString();
                    objquotationhistorylist.quotation_remarks = objMySqlDataReader["quotation_remarks"].ToString();

                    objMySqlDataReader.Close();
                }
                return objquotationhistorylist;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return null;
            }
        }



        public void DaGetquotationhistorysummarydata(MdlSmrTrnQuotation values, string quotation_gid)
        {
            try
            {

                msSQL = "select currency_code from crm_trn_tcurrencyexchange where default_currency='Y'";
                string currency = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select distinct a.quotation_gid, a.quotation_referenceno1,date_format(a.quotation_date,'%d-%m-%Y') as quotation_date,a.customer_name,c.user_firstname, " +
                        " case when a.grandtotal_l ='0.00' then format(a.Grandtotal,2) else format(a.grandtotal_l,2) end as Grandtotal," +
                        " case when a.currency_code = '" + currency + "' then a.customer_name " +
                        "  when a.currency_code is null then a.customer_name " +
                        "  when a.currency_code is not null and a.currency_code <> '" + currency +
                        "' then concat(a.customer_name,' / ',h.country) end as customer_name, " +
                        "  a.customer_contact_person, a.quotation_status,concat(e.customercontact_name,'/',e.mobile,'/',e.email) as contact,a.enquiry_gid " +
                        " from smr_trn_treceivequotation a " +
                        " left join hrm_mst_temployee b on b.employee_gid=a.created_by " +
                        " left join adm_mst_tuser c on b.user_gid= c.user_gid " +
                        " left join crm_mst_tcustomer d on d.customer_gid=a.customer_gid " +
                        " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code " +
                        " left join crm_mst_tcustomercontact e on e.customer_gid=d.customer_gid " +
                        " where 1=1 and a.quotation_status='Quotation Amended' and a.quotation_gid like '" + quotation_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<quotationhistorysummarylist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new quotationhistorysummarylist
                        {
                            quotation_date = dt["quotation_date"].ToString(),
                            quotation_referenceno1 = dt["quotation_referenceno1"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            customer_contact_person = dt["customer_contact_person"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            Grandtotal = dt["Grandtotal"].ToString(),
                            quotation_status = dt["quotation_status"].ToString(),
                            quotation_gid = dt["quotation_gid"].ToString(),


                        });
                        values.quotationhistorysummarylist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Quotation history!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }

        public void DaGetquotationproductdetails(string quotation_gid, MdlSmrTrnQuotation values)
        {
            try
            {
               
                msSQL = " select a.qty_quoted,a.product_name from smr_trn_treceivequotationdtl a " +
                    " left join pmr_mst_tproduct b on a.product_gid = b.product_gid where a.quotation_gid='" + quotation_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<quotationproduct_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new quotationproduct_list
                        {

                            product_name = dt["product_name"].ToString(),
                            qty_quoted = dt["qty_quoted"].ToString(),


                        });
                        values.quotationproduct_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }


        public void DapostsubmitQuotation(string employee_gid, Post_List values)
        {
            try
            {

                string msSQL = "select * from smr_trn_treceivequotation " +
                    "where quotation_gid  like '%" + values.quotation_gid + "%'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows)
                {
                    lsamendcount = objMySqlDataReader.RecordsAffected;

                    {
                        msSQL = " update smr_trn_treceivequotation set " + " quotation_gid = '" + values.quotation_gid + "NHA" + lsamendcount + "'," + " quotation_status='Quotation Amended'" + " where quotation_gid = '" + values.quotation_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        //msSQL = " update smr_trn_treceivequotationdtl set quotationdtl_gid=concat(quotationdtl_gid,'" + "NHA" + lsamendcount + "') ," + " quotation_gid = '" + values.quotation_gid + "NHA" + lsamendcount + "'" + " where quotation_gid = '" + values.quotation_gid + "'";
                        //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Select one Product to Raise Quotation";
                    }
                }

                msGetGid = objcmnfunctions.GetMasterGID("VQDC");
                if (msGetGid == "E")
                {
                    values.status = true;
                    values.message = "Create Sequence Code VQDC for Raise Enquiry";
                }
                if (values.tax_amount == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.tax_amount + "',";
                }


                if (msGetGid == "New Ref.No")
                {
                    msGetGid = ("quotation_gid");
                }
                else
                {
                    //msGetGid = objcmnfunctions.GetMasterGID("VQDC");

                }


                msSQL = "select customercontact_gid from crm_mst_tcustomercontact where customercontact_name = '" + values.customercontact_names + "'";
                string lscustomercontactgid = objdbconn.GetExecuteScalar(msSQL);


                msSQL = " select customerbranch_name from crm_mst_tcustomercontact where customercontact_gid=  '" + lscustomercontactgid + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                lscustomercontact_names = values.customercontact_names;
                string lsquotation_status = "Approved";
                string lsadditional_discount_l = "0.00";
                string lsaddon_charge_l = "0.00";
                string lsgrandtotal_l = "0.00";
                string lsproducttotalamount = "0.00";
                string lsgst_percentage = "0.00";
                if (objMySqlDataReader.HasRows == true)
                {
                    //  lscustomercontact_names = objMySqlDataReader["customercontact_name"].ToString();
                    lscustomerbranch_name = objMySqlDataReader["customerbranch_name"].ToString();
                }
                //string uiDateStr = values.quotation_date;
                //DateTime quotation_date = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                //string quotationorder_date = quotation_date.ToString("yyyy-MM-dd");



                msSQL = " insert  into smr_trn_treceivequotation (" +
                         " quotation_gid ," +
                         " quotation_referencenumber ," +
                         " branch_gid ," +
                         " quotation_date," +
                         " customer_name," +
                         " customerbranch_gid," +
                         " created_by," +
                         " quotation_remarks," +
                         " quotation_referenceno1, " +
                         " payment_days, " +
                         " delivery_days, " +
                         " Grandtotal, " +
                         " termsandconditions, " +
                         " quotation_status, " +
                         " contact_no, " +
                         " customer_address, " +
                         " contact_mail, " +
                         " addon_charge, " +
                         " additional_discount, " +
                         " addon_charge_l, " +
                         " additional_discount_l, " +
                         " grandtotal_l, " +
                         " currency_code, " +
                         " exchange_rate, " +
                         " total_amount," +
                         " gst_percentage," +
                         " salesperson_gid," +
                         " freight_terms, " +
                         " payment_terms," +
                         " roundoff, " +
                         " total_price, " +
                         " freight_charges," +
                         " buyback_charges," +
                         " packing_charges," +
                         " created_date ," +
                         " insurance_charges " +
                         ") values ( " +
                         " '" + values.quotation_gid + "'," +
                          " '" + values.quotation_referencenumber + "'," +
                         " '" + values.branch_name + "'," +
                         "'" + values.quotation_date + "'," +
                         " '" + values.customer_name + "'," +
                         " '" + lscustomerbranch_name + "'," +
                         " '" + employee_gid + "'," +
                         " '" + values.quotation_remarks + "'," +
                         " '" + values.quotation_referenceno1 + "'," +
                         " '" + values.payment_days + "'," +
                         " '" + values.delivery_days + "'," +
                         "'" + values.grandtotal + "', " +
                         " '" + values.termsandconditions + "'," +
                         " '" + lsquotation_status + "'," +
                         " '" + values.customer_mobile + "'," +
                         " '" + values.customer_address + "'," +
                         " '" + values.customer_email + "'," +
                         "'" + values.addon_charge + "'," +
                         "'" + values.additional_discount + "'," +
                         "'" + lsaddon_charge_l + "'," +
                         "'" + lsadditional_discount_l + "'," +
                         "'" + lsgrandtotal_l + "', " +
                         "'" + values.currency_code + "'," +
                         "'" + values.exchange_rate + "'," +
                         "'" + values.producttotalamount.Replace(",", "") + "'," +
                         "'" + lsgst_percentage + "', " +
                         "'" + values.user_name + "'," +
                         "'" + values.freight_terms + "'," +
                         "'" + values.payment_terms + "',";

                if (values.roundoff == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.roundoff + "',";
                }
                msSQL += "'" + lsproducttotalamount + "'," +
                          "'" + values.freight_charges + "'," +
                          "'" + values.buyback_charges + "'," +
                          "'" + values.packing_charges + "'," +
                           "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                          "'" + values.insurance_charges + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 0)
                {
                    values.status = false;
                    values.message = "Error Occured while inserting Quotation";
                }

                else
                {
                    msSQL = "select a.tmpquotationdtl_gid as quotationdtl_gid,a.quotation_gid,a.customerproduct_code,a.productgroup_gid,a.productgroup_name,a.product_gid,a.product_name,a.product_code,a.qty_quoted,a.slno, " +
                        " a.product_price,a.margin_percentage,a.selling_price,tax_percentage, tax_amount,tax_name,tax_name2,tax_name3,tax_percentage2,tax_percentage3, " +
                        " a.margin_amount ,a.product_remarks,tax_amount2,tax_amount3,tax1_gid,tax2_gid,tax3_gid,a.discount_percentage,a.discount_amount,a.mrp,concat(tax_name,'-',tax_amount) as tax, " +
                        " a.uom_gid,a.uom_name,a.payment_days,a.delivery_period,a.price,a.display_field,c.vendor_companyname,a.vendor_gid,a.vendor_name, " +
                        " a.product_status, date_format(a.product_requireddate,'%Y-%m-%d') as product_requireddate,a.productrequireddate_remarks " +
                        " from smr_tmp_treceivequotationdtl a " +
                        " left join pmr_mst_tproduct b on a.product_gid=b.product_gid " +
                        " left join acp_mst_tvendor c on a.vendor_gid=c.vendor_gid " +
                        " union" +
                        " select a.quotationdtl_gid,a.quotation_gid,a.customerproduct_code,a.productgroup_gid,a.productgroup_name,a.product_gid,a.product_name,a.product_code,a.qty_quoted,a.slno, " +
                        " a.product_price,a.margin_percentage,a.selling_price,tax_percentage, tax_amount,tax_name,tax_name2,tax_name3,tax_percentage2,tax_percentage3, " +
                        " a.margin_amount ,a.product_remarks,tax_amount2,tax_amount3,tax1_gid,tax2_gid,tax3_gid,a.discount_percentage,a.discount_amount,a.mrp,concat(tax_name,'-',tax_amount) as tax,  " +
                        " a.uom_gid,a.uom_name,a.payment_days,a.delivery_period,a.price,a.display_field,c.vendor_companyname,a.vendor_gid,a.vendor_name, " +
                        " a.product_status, date_format(a.product_requireddate,'%Y-%m-%d') as product_requireddate,a.productrequireddate_remarks " +
                        " from smr_trn_treceivequotationdtl a " +
                        " left join pmr_mst_tproduct b on a.product_gid=b.product_gid " +
                        " left join acp_mst_tvendor c on a.vendor_gid=c.vendor_gid " +
                        " where a.quotation_gid='" + values.quotation_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count != 0)
                    {

                        msSQL = " update smr_trn_treceivequotationdtl set quotationdtl_gid=concat(quotationdtl_gid,'" + "NHA" + lsamendcount + "') ," + " quotation_gid = '" + values.quotation_gid + "NHA" + lsamendcount + "'" + " where quotation_gid = '" + values.quotation_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        foreach (DataRow dt in dt_datatable.Rows)

                        {
                            msgetGid2 = objcmnfunctions.GetMasterGID("VQDC");

                            int i = 0;
                            msSQL = "insert into smr_trn_treceivequotationdtl (" +
                                   " quotationdtl_gid ," +
                                   " quotation_gid," +
                                   " product_gid ," +
                                   " customerproduct_code," +
                                   " productgroup_gid," +
                                   " productgroup_name," +
                                   " product_name," +
                                   " product_code," +
                                   " display_field," +
                                   " product_price," +
                                   " qty_quoted," +
                                   " discount_percentage," +
                                   " discount_amount," +
                                   " selling_price," +
                                   " uom_gid," +
                                   " uom_name," +
                                   " price," +
                                   " tax_name," +
                                   " tax_name2, " +
                                   " tax_name3, " +
                                   " tax1_gid, " +
                                   " tax2_gid, " +
                                   " tax3_gid, " +
                                   " tax_percentage," +
                                   " tax_percentage2," +
                                   " tax_percentage3," +
                                   " vendor_gid , " +
                                   " slno," +
                                   " productrequireddate_remarks, " +
                                   " tax_amount, tax_amount2, tax_amount3 " +
                                   ")values(" +
                                   " '" + msgetGid2 + "'," +
                                   " '" + values.quotation_gid + "'," +
                                   " '" + dt["product_gid"].ToString() + "'," +
                                   " '" + dt["customerproduct_code"].ToString() + "'," +
                                   " '" + dt["productgroup_gid"].ToString() + "'," +
                                   " '" + dt["productgroup_name"].ToString() + "'," +
                                   " '" + dt["product_name"].ToString() + "'," +
                                   " '" + dt["product_code"].ToString() + "'," +
                                   " '" + dt["display_field"].ToString() + "'," +
                                   " '" + dt["product_price"].ToString() + "'," +
                                   " '" + dt["qty_quoted"].ToString() + "'," +
                                   " '" + dt["margin_percentage"].ToString() + "'," +
                                   " '" + dt["margin_amount"].ToString().Replace(",", "").Trim() + "'," +
                                   " '" + dt["selling_price"].ToString() + "'," +
                                   " '" + dt["uom_gid"].ToString() + "'," +
                                   " '" + values.producttotalamount + "'," +
                                   " '" + values.producttotalamount + "'," +
                                   " '" + dt["tax_name"].ToString() + "'," +
                                   " '" + dt["tax_name2"].ToString() + "'," +
                                   " '" + dt["tax_name3"].ToString() + "'," +
                                   " '" + dt["tax1_gid"].ToString() + "'," +
                                   " '" + dt["tax2_gid"].ToString() + "'," +
                                   " '" + dt["tax3_gid"].ToString() + "'," +
                                   " '" + dt["tax_percentage"].ToString() + "'," +
                                   " '" + dt["tax_percentage2"].ToString() + "'," +
                                   " '" + dt["tax_percentage3"].ToString() + "'," +
                                   " '" + dt["vendor_gid"].ToString() + "'," +
                                   " '" + i + 1 + "', ";


                            msSQL += " '" + dt["productrequireddate_remarks"].ToString() + "'," +
                                     " '" + dt["tax_amount"].ToString() + "'," +
                                     " '" + dt["tax_amount2"].ToString() + "'," +
                                     " '" + dt["tax_amount3"].ToString() + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 0)
                            {
                                values.status = false;
                                values.message = "Error occured while Inserting into Quotationdtl";
                            }
                            else
                            {
                                msSQL = "  delete from smr_trn_treceivequotationdtl where employee_gid='" + employee_gid + "'  ";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                        }
                    }


                    msSQL = "select distinct quotation_type from smr_tmp_treceivequotationdtl where created_by='" + employee_gid + "' ";
                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                    if (objMySqlDataReader.HasRows == true)
                    {
                        lsquotation_type = "sales";


                    }

                    else
                    {
                        lsquotation_type = "Service";
                    }




                    msSQL = " update smr_trn_treceivequotation set quotation_type='" + lsquotation_type + "' where quotation_gid='" + values.quotation_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = " update smr_trn_treceivequotation Set " +
                " leadbank_gid = '" + values.customer_gid + "' " + "," +
                " leadbankcontact_gid = '" + lscustomercontact_gid + "' " +
                " where quotation_gid='" + values.quotation_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }


                msSQL = " delete from smr_tmp_treceivequotationdtl " +
                        " where created_by='" + employee_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 0)
                {
                    values.status = false;
                    values.message = "Error occured while Inserting into Temp Data";
                }


                msgetGid4 = objcmnfunctions.GetMasterGID("PODC");
                {
                    msSQL = " insert into smr_trn_tapproval ( " +
                            " approval_gid, " +
                            " approved_by, " +
                            " approved_date, " +
                            " submodule_gid, " +
                            " qoapproval_gid " +
                            " ) values ( " +
                            "'" + msgetGid4 + "'," +
                            " '" + employee_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                            "'SMRSMRQAP'," +
                            "'" + values.quotation_gid + "') ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 0)
                    {
                        values.status = false;
                    }



                    msSQL = "select approval_flag from smr_trn_tapproval where submodule_gid='SMRSMRQAP' and qoapproval_gid='" + values.quotation_gid + "' ";
                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                    if (objMySqlDataReader.HasRows == false)
                    {
                        msSQL = " Update smr_trn_treceivequotation Set " +
                               " quotation_status = 'Approved', " +
                               " approved_by = '" + employee_gid + "', " +
                               " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                               " where quotation_gid = '" + values.quotation_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    else
                    {
                        msSQL = "select approved_by from smr_trn_tapproval where submodule_gid='SMRSMRQAP' and qoapproval_gid='" + values.quotation_gid + "'";
                        objMySqlDataReader1 = objdbconn.GetDataReader(msSQL);
                        if (objMySqlDataReader1.RecordsAffected == 1)
                        {
                            msSQL = " update smr_trn_tapproval set " +
                           " approval_flag = 'Y', " +
                           " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                           " where approved_by = '" + employee_gid + "'" +
                           " and qoapproval_gid = '" + values.quotation_gid + "' and submodule_gid='SMRSMRQAP'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            msSQL = " Update smr_trn_treceivequotation Set " +
                                   " quotation_status = 'Approved', " +
                           " approved_by = '" + employee_gid + "', " +
                           " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                           " where quotation_gid = '" + values.quotation_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                        else if (objMySqlDataReader1.RecordsAffected > 1)
                        {
                            msSQL = " update smr_trn_tapproval set " +
                                   " approval_flag = 'Y', " +
                                   " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                                   " where approved_by = '" + employee_gid + "'" +
                                   " and quotation_gid = '" + values.quotation_gid + "' and submodule_gid='SMRSMRQAP'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                    }
                }

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Quotation Amend Successfully!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Amend Quotation!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }
        public void GetdeleteamendProductSummary(string tmpquotationdtl_gid, summaryprod_list values)
        {

            try
            {

                msSQL = "select price from smr_tmp_treceivequotationdtl " +
                    " where tmpquotationdtl_gid='" + tmpquotationdtl_gid + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)

                {
                    lsprice = objMySqlDataReader["price"].ToString();
                }

                msSQL = " delete from smr_tmp_treceivequotationdtl " +
                        " where tmpquotationdtl_gid='" + tmpquotationdtl_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)

                {
                    values.status = true;
                    values.message = "Product Deleted Successfully!";

                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting The Product!";


                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting The Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
          
        }

        //mail function

        public void DaGetTemplatelist(MdlSmrTrnQuotation values)
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
                values.message = "Exception occured while Getting Template Name!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }
        public void DaGetTemplatet(string template_gid, MdlSmrTrnQuotation values)
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
                values.message = "Exception occured while Getting Template Type !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }
        public void DaPostMail(HttpRequest httpRequest, string user_gid, result objResult)
        {
            {
                try
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
                    objResult.message = "Mail Send Successfully !!";
                }
                catch (Exception ex)
                {
                    objResult.message = "Exception occured while UpdateEnquirytoQuotation Product!";
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                objResult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                }
               
            }

        }

        // DIRECT QUOTATION PRODUCT SUMMARY

        public void DaGetDirectQuotationEditProductSummary(string tmpquotationdtl_gid, MdlSmrTrnQuotation values)
        {
            try
            {
                
                msSQL = " Select tmpquotationdtl_gid,quotation_gid,product_gid,productgroup_gid,tax1_gid," +
                    " productgroup_name,product_name,qty_quoted,discount_percentage,discount_amount,product_price," +
                    " tax_percentage,tax_amount,uom_gid,uom_name,price,tax_name,product_code from smr_tmp_treceivequotationdtl" +
                    " where tmpquotationdtl_gid = '" + tmpquotationdtl_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<DirecteditQuotationList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new DirecteditQuotationList
                        {
                            tmpquotationdtl_gid = dt["tmpquotationdtl_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["uom_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            quantity = dt["qty_quoted"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            selling_price = dt["product_price"].ToString(),
                            discountpercentage = dt["discount_percentage"].ToString(),
                            discountamount = dt["discount_amount"].ToString(),
                            totalamount = dt["price"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_gid = dt["tax1_gid"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),

                        });
                        values.directeditquotation_list = getModuleList;
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting  DirectQuotationEditProductSummary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }

        public void DaGetRaiseQuotedetail(string product_gid, MdlSmrTrnQuotation values)
        {
            try
            {
               
                msSQL = " select a.quotation_gid,a.quotationdtl_gid,a.customerproduct_code,a.product_gid,b.currency_code,a.product_requireddate as product_requireddate," +
                    " d.product_name,date_format(b.quotation_date,'%d-%m-%Y') as quotation_date," +
                    " b.customer_gid,b.customer_name,a.qty_quoted,format(a.product_price,2) as product_price,c.leadbank_name " +
                    " from smr_trn_treceivequotationdtl a left join smr_trn_treceivequotation b on a.quotation_gid=b.quotation_gid " +
                    " left join pmr_mst_tproduct d on a.product_gid = d.product_gid " +
                    " left join crm_trn_tleadbank c on b.customer_gid=c.leadbank_gid " +
                    " where a.product_gid='" + product_gid + "' group by a.product_price " +
                    " order by b.quotation_date desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Directeddetailslist1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Directeddetailslist1
                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            quotation_date = dt["quotation_date"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            qty_quoted = dt["qty_quoted"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            currency_code = dt["currency_code"].ToString(),

                        });
                        values.Directeddetailslist1 = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting RaiseQuotedetail !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
               values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }


        //  PRODUCT UPDATE -- DIRECT QUOTATION PRODUCT SUMMARY

        public void DaPostUpdateDirectQuotationProduct(string employee_gid, DirecteditQuotationList values)
        {
            try
            {
               

                if (values.product_gid != null)
                {
                    lsproductgid1 = values.product_gid;
                    msSQL = "Select product_name from pmr_mst_tproduct where product_gid='" + lsproductgid1 + "'";
                    values.product_name = objdbconn.GetExecuteScalar(msSQL);
                }
                else
                {
                    msSQL = " Select product_gid from pmr_mst_tproduct where product_name = '" + values.product_name + "'";
                    lsproductgid1 = objdbconn.GetExecuteScalar(msSQL);
                }
                if (values.tax_gid == null)
                {
                    msSQL = "Select percentage from acp_mst_ttax where tax_name='" + values.tax_name + "'";
                    lspercentage1 = objdbconn.GetExecuteScalar(msSQL);
                }
                else
                {
                    msSQL = "Select tax_name from acp_mst_ttax where tax_gid='" + values.tax_gid + "'";
                    values.tax_name = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = "Select percentage from acp_mst_ttax where tax_name='" + values.tax_name + "'";
                    lspercentage1 = objdbconn.GetExecuteScalar(msSQL);
                }

                msSQL = "Select productuom_gid from pmr_mst_tproductuom where productuom_name='" + values.productuom_name + "'";
                string lsproductuomgid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " SELECT a.producttype_name FROM pmr_mst_tproducttype a " +
                        " INNER JOIN pmr_mst_tproduct b ON a.producttype_gid=b.producttype_gid  " +
                        " WHERE product_gid='" + lsproductgid1 + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    lsenquiry_type = "Sales";
                }

                else
                {
                    lsenquiry_type = "Service";
                }

                msSQL = " select * from smr_tmp_treceivequotationdtl where product_gid='" + lsproductgid1 + "' and uom_gid='" + lsproductuomgid + "'" +
                        " and product_price='" + values.selling_price + "'" +
                        "  and created_by='" + employee_gid + "' " +
                        " and discount_percentage='" + values.discountpercentage + "' ";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    msSQL = " update smr_tmp_treceivequotationdtl set qty_quoted='" + Convert.ToDouble(values.quantity) + Convert.ToDouble(objMySqlDataReader["qty_quoted"].ToString()) + "', " +
                            " price='" + Convert.ToDouble(values.totalamount) + Convert.ToDouble(objMySqlDataReader["price"].ToString()) + "' " +
                            " where tmpquotationdtl_gid='" + values.tmpquotationdtl_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = " delete from smr_tmp_treceivequotationdtl where tmpquotationdtl_gid='" + values.tmpquotationdtl_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                else
                {
                    msSQL = " update smr_tmp_treceivequotationdtl set " +
                           " product_gid = '" + lsproductgid1 + "'," +
                           " product_code='" + values.product_code + "' ," +
                           " product_name= '" + values.product_name + "'," +
                           " product_price='" + values.selling_price + "'," +
                           " qty_quoted='" + values.quantity + "'," +
                           " discount_percentage='" + values.discountpercentage + "'," +
                           " discount_amount='" + values.discountamount + "'," +
                           " uom_gid = '" + lsproductuomgid + "', " +
                           " uom_name='" + values.productuom_name + "'," +
                           " price='" + values.totalamount + "'," +
                           " created_by='" + employee_gid + "'," +
                           " tax_name= '" + values.tax_name + "'," +
                           " tax_amount='" + values.tax_amount + "'," +
                           " tax_percentage='" + lspercentage1 + "'" +
                           " where tmpquotationdtl_gid='" + values.tmpquotationdtl_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = " Product Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = " Error While Updating Product";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
               values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }

        // PRODUCT DELETE EVENT FOT QUOTATION TO ORDER
        public void DaGetDeleteQuotetoOrderProductSummary(string tmpsalesorderdtl_gid, GetSummaryList values)
        {
            try
            {
                
                msSQL = " delete from smr_tmp_tsalesorderdtl " +
                    " where tmpsalesorderdtl_gid='" + tmpsalesorderdtl_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Product  Deleted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting Product";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
               values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }


        // PRODUCT EDIT FOR QUOTATION TO SALES ORDER
        public void DaGetQuotetoOrderProductEditSummary(string tmpsalesorderdtl_gid, MdlSmrTrnQuotation values)
        {
            try
            {
               
                msSQL = " select a.tmpsalesorderdtl_gid,a.salesorder_gid, a.selling_price, a.employee_gid, a.product_gid, b.product_code," +
                " a.slno, a.productgroup_gid,a.productgroup_name, a.product_name,a.product_price,a.qty_quoted,a.discount_percentage," +
                    " a.discount_amount,a.uom_gid,a.uom_name,a.price,a.tax_name,a.tax1_gid, a.tax_amount, " +
                    " format(a.tax_percentage,2)as tax_percentage " +
                    " from smr_tmp_tsalesorderdtl a " +
                    " left join pmr_mst_tproduct b on a.product_gid = b.product_gid " +
                    " where tmpsalesorderdtl_gid='" + tmpsalesorderdtl_gid + "'  ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<DirecteditQuotationList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new DirecteditQuotationList
                        {
                            tmpsalesorderdtl_gid = dt["tmpsalesorderdtl_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["uom_name"].ToString(),
                            quantity = dt["qty_quoted"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            selling_price = dt["product_price"].ToString(),
                            discountpercentage = dt["discount_percentage"].ToString(),
                            discountamount = dt["discount_amount"].ToString(),
                            totalamount = dt["price"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_gid = dt["tax1_gid"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),

                        });
                        values.directeditquotation_list = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while QuotetoOrderProductEditSummary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
               values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }
        // PRODUCT UPDATE FOR  QUOTATION TO ORDER 
        public void DaPostUpdateQuotationtoOrderProductSummary(string employee_gid, DirecteditQuotationList values)
        {
            try
            {
               
                if (values.product_gid != null)
                {

                    msSQL = "Select product_gid from pmr_mst_tproduct where product_name='" + values.product_name + "'";
                    lsproductgid1 = objdbconn.GetExecuteScalar(msSQL);
                }
                else
                {
                    msSQL = " Select product_name from pmr_mst_tproduct where product_gid = '" + values.product_gid + "'";
                    lsproductgid1 = objdbconn.GetExecuteScalar(msSQL);
                }
                if (values.tax_gid == null)
                {
                    msSQL = "Select percentage from acp_mst_ttax where tax_name='" + values.tax_name + "'";
                    lspercentage1 = objdbconn.GetExecuteScalar(msSQL);
                }
                else
                {
                    msSQL = "Select tax_name from acp_mst_ttax where tax_gid='" + values.tax_gid + "'";
                    values.tax_name = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = "Select percentage from acp_mst_ttax where tax_name='" + values.tax_name + "'";
                    lspercentage1 = objdbconn.GetExecuteScalar(msSQL);
                }
                msSQL = " select productuom_gid from pmr_mst_tproductuom where productuom_name='" + values.productuom_name + "'";
                string lsproductuomgid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " SELECT a.producttype_name FROM pmr_mst_tproducttype a " +
                      " INNER JOIN pmr_mst_tproduct b ON a.producttype_gid=b.producttype_gid  " +
                      " WHERE product_gid='" + lsproductgid1 + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    lsenquiry_type = "Sales";
                }

                else
                {
                    lsenquiry_type = "Service";
                }

                msSQL = " select * from smr_tmp_tsalesorderdtl where product_gid='" + lsproductgid1 + "' and uom_gid='" + lsproductuomgid + "' " +
              " and selling_price='" + values.product_price +
              "  and employee_gid='" + employee_gid + "' " +
              " and discount_percentage='" + values.discountpercentage + "' " +
                " and tmpsalesorderdtl_gid = '" + values.tmpsalesorderdtl_gid + "' ";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    msSQL = " update smr_tmp_tsalesorderdtl set qty_quoted='" + Convert.ToDouble(values.quantity) + Convert.ToDouble(objMySqlDataReader["qty_quoted"].ToString()) + "'," +
                          " price='" + Convert.ToDouble(values.price) + Convert.ToDouble(objMySqlDataReader["price"].ToString()) + "' " +
                          " where tmpsalesorderdtl_gid='" + objMySqlDataReader["tmpsalesorderdtl_gid"] + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = " delete from smr_tmp_tsalesorderdtl where tmpsalesorderdtl_gid='" + values.tmpsalesorderdtl_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                else
                {
                    msSQL = " update smr_tmp_tsalesorderdtl set " +
                  " product_gid = '" + lsproductgid1 + "'," +
                  " product_name= '" + values.product_name + "'," +
                  " product_price='" + values.product_price + "'," +
                  " qty_quoted='" + values.quantity + "'," +
                  " disocunt_percentage='" + values.discountpercentage + "'," +
                  " discount_amount='" + values.discountamount + "'," +
                  " uom_gid = '" + lsproductuomgid + "', " +
                  " uom_name='" + values.productuom_name + "'," +
                  " price='" + values.price + "'," +
                  " employee_gid='" + employee_gid + "'," +
                  " tax_name= '" + values.tax_name + "'," +
                  " tax_amount='" + values.tax_amount + "'," +
                  " tax_percentage='" + lspercentage1 + "'," +
                  " order_type='" + lsenquiry_type + "', " +
                 " where tmpsalesorderdtl_gid='" + values.tmpsalesorderdtl_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Update QuotationtoOrderProduct!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
               values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }

    }
}


    
