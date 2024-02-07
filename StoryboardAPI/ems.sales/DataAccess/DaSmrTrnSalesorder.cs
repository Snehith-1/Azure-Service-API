﻿using ems.sales.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using Microsoft.SqlServer.Server;
using System.Diagnostics;
using OfficeOpenXml.Style;
using System.Globalization;
using MySql.Data.MySqlClient;

namespace ems.sales.DataAccess
{
    public class DaSmrTrnSalesorder
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL, msINGetGID, msGetinGid, msSQL1, msSQL2 = string.Empty;
        MySqlDataReader objMySqlDataReader, objMySqlDataReader1;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lspercentage,start_date, end_date, lspercentage1, lsdesignation_code, lstaxname2, lsorder_type, lsproduct_type, lsproductgid1,lstaxname1, lsdiscountpercentage, lsdiscountamount, lsprice, lstype1, lsproduct_price, mssalesorderGID, mssalesorderGID1, mscusconGetGID, lscustomer_name, msGetCustomergid, lscustomer_gid, msGetGid2, msGetGid3, lsCode, msPOGetGID, msGetGID, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;

        public void DaGetSmrTrnSalesordersummary(MdlSmrTrnSalesorder values)
        {
            try
            {

                string currency = "INR";

                msSQL = " select distinct a.salesorder_gid, cast(concat(a.so_referenceno1," +
                   " if(a.so_referencenumber<>'',concat(a.so_referencenumber),'') ) as char)as so_referenceno1," +
                   " DATE_FORMAT(a.salesorder_date, '%d-%m-%Y') as salesorder_date,c.user_firstname,a.so_type,a.currency_code," +
                    " a.customer_contact_person, a.salesorder_status,a.currency_code,s.source_name,d.customer_code,i.branch_name, " +
                    " case when a.grandtotal_l ='0.00' then format(a.Grandtotal,2) else format(a.grandtotal_l,2) end as Grandtotal, " +
                    " case when a.currency_code = '" + currency + "' then a.customer_name " +
                    " when a.currency_code is null then a.customer_name " +
                    " when a.currency_code is not null and a.currency_code <> '" + currency + "' then (a.customer_name) end as customer_name, " +
                    " case when a.customer_email is null then concat(e.customercontact_name,'/',e.mobile,'/',e.email) " +
                    " when a.customer_email is not null then concat(a.customer_contact_person,' / ',a.customer_mobile,' / ',a.customer_email) end as contact,a.invoice_flag " +
                    " from smr_trn_tsalesorder a " +
                    " left join crm_mst_tcustomer d on a.customer_gid=d.customer_gid " +
                    " left join crm_mst_tcustomercontact e on d.customer_gid=e.customer_gid " +
                    " left join hrm_mst_temployee b on b.employee_gid=a.created_by " +
                    " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code " +
                    " left join adm_mst_tuser c on b.user_gid= c.user_gid" +
                    " left join hrm_mst_tbranch i on a.branch_gid= i.branch_gid" +
                    " left join crm_trn_tleadbank l on l.customer_gid=a.customer_gid" +
                    " left join crm_mst_tsource s on s.source_gid=l.source_gid" +
                    " where 1=1 order by a.salesorder_date desc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<salesorder_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new salesorder_list
                        {
                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            salesorder_date = dt["salesorder_date"].ToString(),
                            so_referenceno1 = dt["so_referenceno1"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            contact = dt["contact"].ToString(),
                            so_type = dt["so_type"].ToString(),
                            Grandtotal = dt["Grandtotal"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            salesorder_status = dt["salesorder_status"].ToString()
                        });
                        values.salesorder_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Order Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }


        public void DaGetViewsalesorderSummary(string salesorder_gid, MdlSmrTrnSalesorder values)
        {
            try
            {
                
                msSQL = " SELECT a.salesorder_gid, h.currency_code, a.customerbranch_gid, a.exchange_rate, " +
                    " DATE_FORMAT(a.salesorder_date, '%d-%m-%Y') AS salesorder_date, a.salesperson_gid, " +
                    " CONCAT(g.user_code, ' / ', g.user_firstname, ' ', g.user_lastname) AS salesperson_name, " +
                    " FORMAT(a.Grandtotal, 2) AS Grandtotal, a.termsandconditions, FORMAT(a.addon_charge, 2) AS addon_charge, " +
                    " FORMAT(a.additional_discount_l, 2) AS additional_discount, a.payment_days, FORMAT(a.gst_amount, 2) AS gst_amount, " +
                    " a.delivery_days, a.so_referenceno1, a.so_referencenumber, a.payment_terms, a.freight_terms, " +
                    " FORMAT(a.roundoff, 2) AS roundoff, a.so_remarks, FORMAT(SUM(d.price), 2) AS total_value, a.shipping_to, " +
                    " a.customer_address, a.customer_name, CONCAT(a.customerbranch_gid, '|', a.customer_contact_person) AS customer_contact_person, " +
                    " DATE_FORMAT(a.start_date, '%d-%m-%Y') AS start_date, DATE_FORMAT(a.end_date, '%d-%m-%Y') AS end_date, " +
                    " a.termsandconditions, a.shipping_to, a.customer_mobile, a.customer_email, e.branch_name, " +
                    " FORMAT(a.total_amount, 2) AS total_amount, FORMAT(a.freight_charges, 2) AS freight_charges, " +
                    " FORMAT(a.packing_charges, 2) AS packing_charges, FORMAT(a.buyback_charges, 2) AS buyback_charges, " +
                    " FORMAT(a.insurance_charges, 2) AS insurance_charges,product_code, d.product_name, d.uom_name," +
                    " d.qty_quoted, d.margin_percentage, FORMAT(d.product_price, 2) AS product_price, " +
                    " d.selling_price, FORMAT(d.price, 2) AS price, d.tax_amount,  d.tax_name  FROM smr_trn_tsalesorder a " +
                    " LEFT JOIN smr_trn_tsalesorderdtl d ON d.salesorder_gid = a.salesorder_gid " +
                    " LEFT JOIN hrm_mst_tbranch e ON e.branch_gid = a.branch_gid " +
                    " LEFT JOIN acp_mst_ttax f ON f.tax_gid = a.tax_gid " +
                    " LEFT JOIN adm_mst_tuser g ON g.user_gid = a.salesperson_gid " +
                    " LEFT JOIN crm_trn_tcurrencyexchange h ON a.currency_gid = h.currencyexchange_gid " +
                    " WHERE a.salesorder_gid = '" + salesorder_gid + "' GROUP BY a.salesorder_gid, d.product_gid ORDER BY a.salesorder_gid ASC";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<postsalesorder_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new postsalesorder_list
                    {


                        salesorder_gid = dt["salesorder_gid"].ToString(),
                        salesorder_date = dt["salesorder_date"].ToString(),
                        customer_name = dt["customer_name"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        customer_contact_person = dt["customer_contact_person"].ToString(),
                        customer_email = dt["customer_email"].ToString(),
                        customer_mobile = dt["customer_mobile"].ToString(),
                        customer_address = dt["customer_address"].ToString(),
                        start_date = dt["start_date"].ToString(),
                        end_date = dt["end_date"].ToString(),
                        currency_code = dt["currency_code"].ToString(),
                        exchange_rate = dt["exchange_rate"].ToString(),
                        freight_terms = dt["freight_terms"].ToString(),
                        payment_terms = dt["payment_terms"].ToString(),
                        payment_days = dt["payment_days"].ToString(),
                        so_referencenumber = dt["so_referencenumber"].ToString(),
                        shipping_to = dt["shipping_to"].ToString(),
                        delivery_days = dt["delivery_days"].ToString(),
                        so_remarks = dt["so_remarks"].ToString(),
                        salesperson_name = dt["salesperson_name"].ToString(),                       
                        product_code = dt["product_code"].ToString(),
                        product_name = dt["product_name"].ToString(),
                        uom_name = dt["uom_name"].ToString(),
                        qty_quoted = dt["qty_quoted"].ToString(),
                        selling_price = dt["selling_price"].ToString(),
                        price = dt["price"].ToString(),                        
                        product_price = dt["product_price"].ToString(),
                        addon_charge = dt["addon_charge"].ToString(),
                        additional_discount = dt["additional_discount"].ToString(),
                        freight_charges = dt["freight_charges"].ToString(),
                        buyback_charges = dt["buyback_charges"].ToString(),
                        packing_charges = dt["packing_charges"].ToString(),
                        insurance_charges = dt["insurance_charges"].ToString(),
                        roundoff = dt["roundoff"].ToString(),
                        Grandtotal = dt["Grandtotal"].ToString(),
                        termsandconditions = dt["termsandconditions"].ToString(),
                        margin_percentage = dt["margin_percentage"].ToString(),
                        tax_name = dt["tax_name"].ToString(),                        
                        tax_amount = dt["tax_amount"].ToString(),                        
                        totax = dt["totax"].ToString(),
                        
                    });
                    values.postsalesorder_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Order Summary !";
               
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }


        // branch

        public void DaGetBranchDtl(MdlSmrTrnSalesorder values)
        {
            try
            {
                


                msSQL = "select branch_gid,branch_name from hrm_mst_tbranch";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetBranchDropdown>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetBranchDropdown

                    {
                        branch_gid = dt["branch_gid"].ToString(),
                        branch_name = dt["branch_name"].ToString(),

                    });
                    values.GetBranchDtl = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Branch Dropdown !";
                
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        // Customer 

        public void DaGetCustomerDtl(MdlSmrTrnSalesorder values)
        {
            try
            {
                


                msSQL = " select customer_name,customer_gid from crm_mst_tcustomer where status='Active'";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetCustomerDropdown>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetCustomerDropdown

                    {
                        customer_gid = dt["customer_gid"].ToString(),
                        customer_name = dt["customer_name"].ToString(),

                    });
                    values.GetCustomerDtl = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Customer Dropdown !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }


        // Customer dropdown 360

        public void DaGetCustomerDtlCRM(MdlSmrTrnSalesorder values, string leadbank_gid)
        {
            try
            {
                


                msSQL = "Select customer_gid from crm_trn_tleadbank where leadbank_gid='" + leadbank_gid + "' ";
            string lscustomer_gid = objdbconn.GetExecuteScalar(msSQL);

            msSQL = "Select a.customer_gid, a.customer_name " +
            " from crm_mst_tcustomer a where customer_gid='" + lscustomer_gid + "' ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetCustomerDropdown>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetCustomerDropdown

                    {
                        customer_gid = dt["customer_gid"].ToString(),
                        customer_name = dt["customer_name"].ToString(),

                    });
                    values.GetCustomerDtl = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Customer dropdown CRM !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }

        // Contact
        public void DaGetPersonDtl(MdlSmrTrnSalesorder values)
        {
            try
            {
               


                msSQL = "select concat(c.department_name,' ','/',' ',a.user_firstname,' ',a.user_lastname) as user_name,a.user_gid from adm_mst_tuser a " +
                " left join hrm_mst_temployee b on a.user_gid=b.user_gid " +
                " left join hrm_mst_tdepartment c on b.department_gid=c.department_gid where a.user_status='Y' and " +
                " c.department_name in('Technical') order by a.user_code  asc ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetPersonDropdown>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetPersonDropdown

                    {
                        user_gid = dt["user_gid"].ToString(),
                        user_name = dt["user_name"].ToString(),

                    });
                    values.GetPersonDtl = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Person Dropdown !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }

        // Currency
        public void DaGetCurrencyDtl(MdlSmrTrnSalesorder values)
        {
            try
            {
                


                msSQL = "select currencyexchange_gid,currency_code from crm_trn_tcurrencyexchange order by currency_code asc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetCurrencyDropdown>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetCurrencyDropdown

                    {
                        currencyexchange_gid = dt["currencyexchange_gid"].ToString(),
                        currency_code = dt["currency_code"].ToString(),

                    });
                    values.GetCurrencyDtl = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Currenct Dropdown !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        // on change
        public void DaGetOnChangeCustomer(string customer_gid, MdlSmrTrnSalesorder values)
        {
            try
            {
                

                if (customer_gid != null)
            {
                msSQL = " select a.customercontact_gid,concat(a.address1,'   ',a.city,'   ',a.state) as address1,ifnull(a.address2,'') as address2,ifnull(a.city,'') as city, " +
                " ifnull(a.state,'') as state,ifnull(a.country_gid,'') as country_gid,ifnull(a.zip_code,'') as zip_code, " +
                " ifnull(a.mobile,'') as mobile,a.email,ifnull(b.country_name,'') as country_name,a.customerbranch_name,concat(customerbranch_name,' | ',a.customercontact_name) as " +
                " customercontact_names " +
                " from crm_mst_tcustomercontact a " +
                " left join crm_mst_tcustomer c on a.customer_gid=c.customer_gid " +
                " left join adm_mst_tcountry b on a.country_gid=b.country_gid " +
                " where c.customer_gid='" + customer_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetCustomerDet>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetCustomerDet
                        {
                            customercontact_names = dt["customercontact_names"].ToString(),
                            branch_name = dt["customerbranch_name"].ToString(),
                            country_name = dt["country_name"].ToString(),
                            customer_email = dt["email"].ToString(),
                            customer_mobile = dt["mobile"].ToString(),
                            zip_code = dt["zip_code"].ToString(),
                            country_gid = dt["country_gid"].ToString(),
                            state = dt["state"].ToString(),
                            city = dt["city"].ToString(),
                            address2 = dt["address2"].ToString(),
                            customer_address = dt["address1"].ToString(),
                            customercontact_gid = dt["customercontact_gid"].ToString(),

                        });
                        values.GetCustomer = getModuleList;
                    }
                }
            }
            else
            {

            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Changing Customer !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }


        public void DaGetCustomerOnchangeCRM(string customercontact_gid, MdlSmrTrnSalesorder values)
        {
            try
            {
                

                if (customercontact_gid != null)
            {
                msSQL = " select a.customercontact_gid,concat(a.address1,'   ',a.city,'   ',a.state) as address1,ifnull(a.address2,'') as address2,ifnull(a.city,'') as city, " +
                " ifnull(a.state,'') as state,ifnull(a.country_gid,'') as country_gid,ifnull(a.zip_code,'') as zip_code, " +
                " ifnull(a.mobile,'') as mobile,a.email,ifnull(b.country_name,'') as country_name,a.customerbranch_name,concat(customerbranch_name,' | ',a.customercontact_name) as " +
                " customercontact_names, c.customer_gid " +
                " from crm_mst_tcustomercontact a " +
                " left join crm_mst_tcustomer c on a.customer_gid=c.customer_gid " +
                " left join adm_mst_tcountry b on a.country_gid=b.country_gid " +
                " where c.customer_gid='" + customercontact_gid + "'";

                var getModuleList = new List<GetCustomerDet>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetCustomerDet
                        {
                            customercontact_names = dt["customercontact_names"].ToString(),
                            branch_name = dt["customerbranch_name"].ToString(),
                            country_name = dt["country_name"].ToString(),
                            customer_email = dt["email"].ToString(),
                            customer_mobile = dt["mobile"].ToString(),
                            zip_code = dt["zip_code"].ToString(),
                            country_gid = dt["country_gid"].ToString(),
                            state = dt["state"].ToString(),
                            city = dt["city"].ToString(),
                            address2 = dt["address2"].ToString(),
                            customer_address = dt["address1"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),

                        });
                        values.GetCustomer = getModuleList;
                    }
                }
            }
            else
            {

            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Customer Onchange CRM  !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }

        // CRM customer on change 360 
        public void DaGetOnChangeCustomerCRM(string leadbank_gid, MdlSmrTrnSalesorder values)
        {
            try
            {
                

                if (leadbank_gid != null)
            {
                msSQL = " select a.leadbankcontact_gid,concat(a.address1,'   ',a.city,'   ',a.state) as address1,ifnull(a.address2,'') as address2,ifnull(a.city,'') as city, " +
                " ifnull(a.state,'') as state,ifnull(a.country_gid,'') as country_gid,ifnull(a.zip_code,'') as zip_code, " +
                " ifnull(a.mobile,'') as mobile,a.email,ifnull(b.country_name,'') as country_name,a.customerbranch_name,concat(customerbranch_name,' | ',a.customercontact_name) as " +
                " customercontact_names " +
                " from crm_mst_tleadbankcontact a " +
                " left join crm_mst_tleadbank c on a.leadbank_gid=c.leadbank_gid " +
                " left join adm_mst_tcountry b on a.country_gid=b.country_gid " +
                " where c.customer_gid='" + leadbank_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetCustomerDet>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetCustomerDet
                        {
                            customercontact_names = dt["customercontact_names"].ToString(),
                            branch_name = dt["customerbranch_name"].ToString(),
                            country_name = dt["country_name"].ToString(),
                            customer_email = dt["email"].ToString(),
                            customer_mobile = dt["mobile"].ToString(),
                            zip_code = dt["zip_code"].ToString(),
                            country_gid = dt["country_gid"].ToString(),
                            state = dt["state"].ToString(),
                            city = dt["city"].ToString(),
                            address2 = dt["address2"].ToString(),
                            customer_address = dt["address1"].ToString(),
                            customercontact_gid = dt["customercontact_gid"].ToString(),

                        });
                        values.GetCustomer = getModuleList;
                    }
                }
            }
            else
            {

            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading  onchange Cusotmer CRM!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        // Tax 1
        public void DaGetTax1Dtl(MdlSmrTrnSalesorder values)
        {
            try
            {
                


                msSQL = " select tax_name,tax_gid,percentage from acp_mst_ttax where active_flag='Y' ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetTaxoneDropdown>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetTaxoneDropdown

                    {
                        tax_gid = dt["tax_gid"].ToString(),
                        tax1_gid = dt["tax_gid"].ToString(),
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
                values.message = "Exception occured while loading Tax Dropdown !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        // Tax 2
        public void DaGetTax2Dtl(MdlSmrTrnSalesorder values)
        {
            try
            {
               

                msSQL = " select tax_name,tax_gid,percentage from acp_mst_ttax where active_flag='Y' ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetTaxTwoDropdown>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetTaxTwoDropdown

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
                values.message = "Exception occured while loading Tax dropdown !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        // Tax 3
        public void DaGetTax3Dtl(MdlSmrTrnSalesorder values)
        {
            try
            {
               


                msSQL = " select tax_name,tax_gid,percentage from acp_mst_ttax where active_flag='Y' ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetTaxThreeDropdown>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetTaxThreeDropdown

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
                values.message = "Exception occured while loading Tax Dropdown !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }



        // Product

        public void DaGetProductNamDtl(MdlSmrTrnSalesorder values)
        {
            try
            {
              


                msSQL = "Select product_gid, product_name from pmr_mst_tproduct";


            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetProductNamDropdown>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetProductNamDropdown

                    {
                        product_gid = dt["product_gid"].ToString(),
                        product_name = dt["product_name"].ToString(),

                    });
                    values.GetProductNamDtl = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Product name dropdown !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        // Product Drop down CRM
        public void DaGetProductNamDtlCRM(MdlSmrTrnSalesorder values)
        {
            try
            {
                


                msSQL = "Select product_gid, product_name from pmr_mst_tproduct";


            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetProductNamDropdown>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetProductNamDropdown

                    {
                        product_gid = dt["product_gid"].ToString(),
                        product_name = dt["product_name"].ToString(),

                    });
                    values.GetProductNamDtl = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Product Name dropdown CRM !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        // Tax 3
        public void DaGetTax4Dtl(MdlSmrTrnSalesorder values)
        {
            try
            {
                


                msSQL = " select tax_name,tax_gid,percentage from acp_mst_ttax where active_flag='Y' ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetTaxFourDropdown>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetTaxFourDropdown

                    {
                        tax_gid = dt["tax_gid"].ToString(),
                        tax_name4 = dt["tax_name"].ToString(),
                        percentage = dt["percentage"].ToString()

                    });
                    values.GetTax4Dtl = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax Dropdown !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        //onchange CRM
        public void DaGetOnChangeProductsNameCRM(string product_gid, MdlSmrTrnSalesorder values)
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

                var getModuleList = new List<getproductsCode>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new getproductsCode
                        {
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),

                        });
                        values.ProductsCodes = getModuleList;
                    }
                }
            }
            else
            {

            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Product Name !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        public void DaGetOnChangeProductsNameAmend(string product_gid, MdlSmrTrnSalesorder values)
        {
            try
            {
                


                msSQL = " Select a.product_name, a.product_code, a.cost_price,a.product_gid, b.productuom_gid,b.productuom_name,c.productgroup_name," +
                " c.productgroup_gid,a.productuom_gid  from pmr_mst_tproduct a  " +
                " left join pmr_mst_tproductuom b on a.productuom_gid = b.productuom_gid  " +
                " left join pmr_mst_tproductgroup c on a.productgroup_gid = c.productgroup_gid  " +
                " where a.product_gid='" + product_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<getproductsCode>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new getproductsCode
                        {
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            unitprice = dt["cost_price"].ToString()

                        });
                        values.ProductsCodes = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Changing Product Name Amend!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            

        }
        public void DaGetOnChangeProductsName(string product_gid,string customercontact_gid, MdlSmrTrnSalesorder values)
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
                else 
                {
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
            }
            else
            {
                values.status = false;
                values.message = "Kindly Select Customer Before Adding Product";
            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Product Name !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        // Overall Submit for sales order Amend 
        public void DaAmendSalesOrder(string employee_gid, Getamendsalesorderdtl1 values)
        {
            try
            {
               
                string lstmpquotationgid = null;
            string lsproductgroup_gid = null;
            string lsproductgroup = null;
            string lscustomerproduct_code = null;
            string lsproductname_gid = null;
            string lsproductname = null;
            string lsuom_gid = null;
            string lsuom = null;
            string lsunitprice = null;
            string lsquantity = null;
            string lsdiscountpercentage = null;
            string lsdiscountamount = null;
            string lstax_name1 = null;
            string lstax_name2 = null;
            string lstax_name3 = null;
            string lstaxamount_1 = null;
            string lstaxamount_2 = null;
            string lstaxamount_3 = null;
            string lstotalamount = null;
            string lsdisplay_field = null;
            string lsvendor_gid = null;
            string lsproduct_delivered = null, lscurrency_code;
            string lsslno = null;
            DataTable objtbl;
            double lslocaltaxamount1;
            double lslocaltaxamount2;
            double lslocaltaxamount3;
            double lslocalproductprice;
            double lslocalproductdiscount;
            double lslocalproduct_totalprice;
            double lslocalselling_price;
            double lslocalgrandtotal;
            double lslocaladdon;
            double lslocaladditionaldiscount;
            double grandtotal;
            double ADDON;
            double ADD_DISCIOUNT;
            double Total_Price;
            double Total_Amount;
            string lssalesref_no = null;
            double lslocalsellingprice;
            double lsroundoff = 0.0;
            double lsfreight_charge = 0.0;
            double buyback_charge = 0.0;
            double packing_charge = 0.0;
            double insurance_charge = 0.0;
            bool blncommit;
            string lscustomer_gid, lscustomer_name = "", msconGetGID = "";
            
            msSQL = "select * from crm_trn_tleadbank a " +
                " left  join crm_mst_tcustomer b on a.customer_gid=b.customer_gid " +
                "  where a.leadbank_gid='" + values.customer_gid + "' and a.customer_gid is not null ";
            objMySqlDataReader = objdbconn.GetDataReader(msSQL);
            if (objMySqlDataReader.HasRows == true)
            {
                lscustomer_gid = objMySqlDataReader["customer_gid"].ToString();
            }
            else
            {
                string lscustomer_code = "";
                msGetCustomergid = objcmnfunctions.GetMasterGID("BCRM");

                msSQL = " Select * from crm_trn_tleadbank " +
                  " where leadbank_gid = '" + values.customer_gid + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    lscustomer_name = objMySqlDataReader["leadbank_name"].ToString();
                    lscustomer_code = objMySqlDataReader["leadbank_id"].ToString();
                    msSQL = " INSERT INTO crm_mst_tcustomer " +
                        " (customer_gid, " +
                        " customer_id, " +
                        " customer_name, " +
                        " company_website, " +
                        " customer_code," +
                        " customer_address," +
                        " customer_address2," +
                        " customer_city," +
                        " customer_state," +
                        " customer_pin," +
                        " main_branch," +
                        " created_by, " +
                        " created_date, " +
                        " customer ," +
                        " status, " +
                        " created_flag)" +
                        " values ( " +
                        "'" + msGetCustomergid + "'," +
                        "'" + objMySqlDataReader["leadbank_id"].ToString() + "'," +
                        "'" + objMySqlDataReader["leadbank_name"].ToString() + "'," +
                        "'" + objMySqlDataReader["company_website"].ToString() + "'," +
                        "'" + objMySqlDataReader["leadbank_code"].ToString() + "'," +
                        "'" + objMySqlDataReader["leadbank_address1"].ToString() + "'," +
                        "'" + objMySqlDataReader["leadbank_address2"].ToString() + "'," +
                        "'" + objMySqlDataReader["leadbank_city"].ToString() + "'," +
                        "'" + objMySqlDataReader["leadbank_state"].ToString() + "'," +
                        "'" + objMySqlDataReader["leadbank_pin"].ToString() + "'," +
                        "'" + objMySqlDataReader["main_branch"].ToString() + "'," +
                        "'" + employee_gid + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                        "'N'," +
                        " 'Active', " +
                        "'" + objMySqlDataReader["created_flag"] + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (mnResult == 1)
                {
                    msSQL = " update crm_trn_tleadbank set" +
                        " customer_gid = '" + msGetCustomergid + "'" +
                        " where leadbank_gid = '" + values.customer_gid + "'";
                    mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult3 == 0)
                    {
                        blncommit = false;
                    }
                    lscustomer_gid = msGetCustomergid;
                }
                if (mnResult3 == 1)
                {
                    msSQL = " Select * from crm_trn_tleadbankcontact " +
                        " where leadbank_gid = '" + values.customer_gid + "'";
                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                    if (objMySqlDataReader.HasRows == true)
                    {
                        while (objMySqlDataReader.Read())
                        {
                            msconGetGID = objcmnfunctions.GetMasterGID("BCCM");
                            msSQL = " INSERT INTO crm_mst_tcustomercontact " +
                                    " (customercontact_gid," +
                                    " customer_gid," +
                                    " customercontact_name," +
                                    " email," +
                                    " mobile," +
                                    " designation," +
                                    " did_number," +
                                    " created_date," +
                                    " created_by," +
                                    " address1, ," +
                                    " address2, ," +
                                    " state, ," +
                                    " country_gid, ," +
                                    " city, ," +
                                    " region, ," +
                                    " zip_code, ," +
                                    " customerbranch_name, ," +
                                    " main_contact)," +
                                    " values( ," +
                                    "'" + msconGetGID + "'," +
                                    "'" + msGetCustomergid + "'," +
                                    "'" + objMySqlDataReader["leadbankcontact_name"].ToString() + "'," +
                                    "'" + objMySqlDataReader["email"].ToString() + "'," +
                                    "'" + objMySqlDataReader["mobile"].ToString() + "'," +
                                    "'" + objMySqlDataReader["designation"].ToString() + "'," +
                                    "'" + objMySqlDataReader["did_number"].ToString() + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                    "'" + employee_gid + "'," +
                                    "'" + objMySqlDataReader["address1"] + "'," +
                                    "'" + objMySqlDataReader["address2"] + "'," +
                                    "'" + objMySqlDataReader["state"] + "'," +
                                    "'" + objMySqlDataReader["country_gid"] + "'," +
                                    "'" + objMySqlDataReader["city"] + "'," +
                                    "'" + objMySqlDataReader["region_name"] + "',," +
                                    "'" + objMySqlDataReader["pincode"] + "'," +
                                    "'" + objMySqlDataReader["leadbankbranch_name"].ToString() + "'," +
                                    "'" + objMySqlDataReader["main_contact"].ToString() + "')";
                            mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult2 == 1)
                            {
                                msSQL = " update crm_trn_tleadbankcontact set," +
                                    " customercontact_gid = '" + msconGetGID + "'," +
                                    " where leadbankcontact_gid = '" + objMySqlDataReader["leadbankcontact_gid"]+ "'";
                                mnResult5 = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                            if (mnResult5 == 0)
                            {
                                blncommit = false;
                            }

                        }
                    }
                }

            }
            int lsamend_count = 0;
            string lsstatus, lsbranch_gid = "";
            msSQL = " select branch_gid from smr_trn_tsalesorder where salesorder_gid='" + values.salesorder_gid + "'";
            lsbranch_gid = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " select currency_code from crm_trn_tcurrencyexchange where currencyexchange_gid ='" + values.currency_code + "'";
            lscurrency_code = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " select * from smr_trn_tsalesorder " +
                   " where salesorder_gid like '" + values.salesorder_gid + "%'";
            objMySqlDataReader = objdbconn.GetDataReader(msSQL);
            lsamend_count = objMySqlDataReader.RecordsAffected;
            objMySqlDataReader.Close();
            msSQL = " select salesorder_status,so_referencenumber from smr_trn_tsalesorder where salesorder_gid='" + values.salesorder_gid + "'";
            objMySqlDataReader = objdbconn.GetDataReader(msSQL);
            string hsnso_referencenumber = "";
            if (objMySqlDataReader.HasRows == true)
            {
                lsstatus = objMySqlDataReader["salesorder_status"].ToString();
                hsnso_referencenumber = objMySqlDataReader["so_referencenumber"].ToString();
            }
            objMySqlDataReader.Close();
            msSQL = " update smr_trn_tsalesorder set " +
                " salesorder_gid = '" + values.salesorder_gid + "NHA" + lsamend_count + "'," +
                " salesorder_status = 'SO Amended' " +
                      " where salesorder_gid = '" + values.salesorder_gid + "'";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            msSQL = " update smr_trn_tsalesorderdtl set salesorderdtl_gid=concat(salesorderdtl_gid,'" + "NHA" + lsamend_count + "') ," +
                      " salesorder_gid = '" + values.salesorder_gid + "NHA" + lsamend_count + "'" +
                      " where salesorder_gid = '" + values.salesorder_gid + "'";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult == 0)
            {
                blncommit = false;
            }
            //double lslocalgrandtotal, lslocaladdon, lslocaladditionaldiscount, Total_Price, Total_Amount = 0.00;
            lslocalgrandtotal = double.Parse(values.total_amount) * double.Parse(values.exchange_rate);
            lslocaladdon = double.Parse(values.addon_charge) * double.Parse(values.exchange_rate);
            lslocaladditionaldiscount = double.Parse(values.additional_discount);
            Total_Price = double.Parse(values.total_price);
            Total_Amount = double.Parse(values.Grandtotal);
            if (values.so_referencenumber != hsnso_referencenumber)
            {
                values.so_referencenumber = values.so_referencenumber;
            }
            else
            {
                values.so_referencenumber = hsnso_referencenumber;
            }
            if (values.shipping_to == null)
            {
                values.shipping_to = "";
            }
            if (values.so_referenceno1 == null)
            {
                values.so_referenceno1 = "";
            }
            if (values.vessel_name == null)
            {
                values.vessel_name = "";
            }

            string uiDateStr = values.salesorder_date;
            DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            string salesorder_date = uiDate.ToString("yyyy-MM-dd");

            msSQL = " insert  into smr_trn_tsalesorder (" +
                    " salesorder_gid ," +
                    " branch_gid ," +
                    " salesorder_date," +
                    " customer_gid," +
                    " customer_name," +
                    " customer_contact_person," +
                    " customer_address," +
                    " shipping_to," +
                    " freight_terms, " +
                    " payment_terms," +
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
                    " addon_charge_l, " +
                    " additional_discount_l, " +
                    " grandtotal_l, " +
                    " currency_code, " +
                    " currency_gid, " +
                    " exchange_rate, " +
                    " updated_addon_charge, " +
                    " customer_contact_gid, " +
                    " gst_amount," +
                    " total_price," +
                    " total_amount," +
                    " vessel_name," +
                    " salesperson_gid," +
                    " start_date, " +
                    " end_date, " +
                    " roundoff, " +
                    " updated_additional_discount, " +
                    " freight_charges," +
                    " buyback_charges," +
                    " packing_charges," +
                    " insurance_charges, " +
                    " customerbranch_gid " +
                    ")values(" +
                    " '" + values.salesorder_gid + "'," +
                    " '" + lsbranch_gid + "'," +
                    " '" + salesorder_date + "'," +
                    " '" + values.customer_gid + "'," +
                    " '" + (values.customer_name).Replace("'", "\'").Trim() + "'," +
                    " '" + (values.customer_contact_person).Replace("'", "\'").Trim() + "'," +
                    " '" + (values.customer_address).Replace("'", "\'").Trim() + "'," +
                    " '" + (values.shipping_to).Replace("'", "\'").Trim() + "'," +
                    " '" + (values.freight_terms).Replace("'", "\'").Trim() + "'," +
                    " '" + (values.payment_terms).Replace("'", "\'").Trim() + "'," +
                    " '" + values.customer_email + "'," +
                    " '" + values.customer_mobile + "'," +
                    " '" + employee_gid + "'," +
                    " '" + values.so_referencenumber + "'," +
                    " '" + (values.so_remarks).Replace("'", "\'").Trim() + "'," +
                    " '" + values.so_referenceno1 + "'," +
                    " '" + values.payment_days + "'," +
                    " '" + values.delivery_days + "'," +
                    " '" + values.total_amount + "'," +
                    " '" + values.termsandconditions.Trim().Replace("<br />", "<br>").Replace("'", "\'") + "'," +
                    " 'Approved'," +
                    " '" + Convert.ToDouble(values.addon_charge) + "'," +
                    " '" + values.additional_discount + "'," +
                    " '" + lslocaladdon + "'," +
                    " '" + lslocaladditionaldiscount + "'," +
                    " '" + lslocalgrandtotal + "'," +
                    " '" + lscurrency_code + "'," +
                    " '" + values.currency_code + "'," +
                    " '" + values.exchange_rate + "'," +
                    " '" + Convert.ToDouble(values.addon_charge) + "'," +
                    " '" + values.customer_gid + "'," +
                    " '" + values.total_amount + "'," +
                    " '" + Total_Price + "'," +
                    " '" + Total_Amount + "'," +
                    " '" + values.vessel_name + "'," +
                    " '" + employee_gid + "'," +
                    " '" + values.start_date + "'," +
                    " '" + values.end_date + "'," +
                    " '" + values.roundoff + "'," +
                    " '" + values.additional_discount + "'," +
                    " '" + values.freight_charges + "'," +
                    " '" + values.buyback_charges + "'," +
                    " '" + values.packing_charges + "'," +
                    " '" + values.insurance_charges + "'," +
                    " '" + values.customer_branch + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult == 0)
            {
                values.message = "Error Occurred while adding Salesorder";
                values.status = false;
            }
            else
            {
                string msrenewalGID = "";
                msSQL = " update acp_trn_torder set " +
                        " salesorder_gid = '" + values.salesorder_gid + "NHA" + lsamend_count + "'," +
                        " salesorder_status = 'SO Amended' " +
                        " where salesorder_gid = '" + values.salesorder_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                msSQL = " update acp_trn_torderdtl set salesorderdtl_gid=concat(salesorderdtl_gid,'" + "NHA" + lsamend_count + "') ," +
                        " salesorder_gid = '" + values.salesorder_gid + "NHA" + lsamend_count + "'" +
                        " where salesorder_gid = '" + values.salesorder_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                msSQL = " insert  into acp_trn_torder (" +
                " salesorder_gid ," +
                " branch_gid ," +
                " salesorder_date," +
                " customer_gid," +
                " customer_name," +
                " customer_contact_person," +
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
                " addon_charge_l, " +
                " additional_discount_l, " +
                " grandtotal_l, " +
                " currency_code, " +
                " currency_gid, " +
                " exchange_rate, " +
                " updated_addon_charge, " +
                " updated_additional_discount, " +
                " renewal_gid, " +
                " customer_contact_gid, " +
                " vessel_name," +
                " salesperson_gid," +
                " roundoff, " +
                " shipping_to," +
                " freight_charges," +
                " buyback_charges," +
                " packing_charges," +
                " insurance_charges, " +
                " customerbranch_gid " +
                ")values(" +
                " '" + values.salesorder_gid + "'," +
                " '" + lsbranch_gid + "'," +
                " '" + salesorder_date + "'," +
                " '" + values.customer_gid + "'," +
                " '" + lscustomer_name.Replace("'", "\'").Trim() + "'," +
                " '" + values.customer_contact_person.Replace("'", "\'").Trim() + "'," +
                " '" + values.customer_address.Replace("'", "\'").Trim() + "'," +
                " '" + values.customer_email.Replace("'", "\'") + "'," +
                " '" + values.customer_mobile + "'," +
                " '" + employee_gid + "'," +
                " '" + values.quotation_refno + "'," +
                " '" + values.so_remarks + "'," +
                " '" + values.quotation_refno + "'," +
                " '" + values.payment_days + "'," +
                " '" + values.delivery_days + "'," +
                " '" + values.Grandtotal + "'," +
                " '" + (values.termsandconditions).Replace("'", "\'").Trim() + "'," +
                " 'Approved'," +
                " '" + values.addon_charge + "'," +
                " '" + values.additional_discount + "'," +
                " '" + lslocaladdon + "'," +
                " '" + lslocaladditionaldiscount + "'," +
                " '" + lslocalgrandtotal + "'," +
                " '" + values.currency_code + "'," +
                " '" + values.currency_gid + "'," +
                " '" + values.exchange_rate + "'," +
                " '" + values.addon_charge + "'," +
                " '" + values.additional_discount + "'," +
                " '" + msrenewalGID + "'," +
                " '" + values.customer_gid + "'," +
                " '" + values.vessel_name.Replace("'", "\'").Trim() + "'," +
                " '" + values.salesperson_gid + "'," +
                " '" + values.roundoff + "'," +
                " '" + values.shipping_to.Replace("'", "\'").Trim() + "'," +
                " '" + values.freight_charges + "'," +
                " '" + values.buyback_charges + "'," +
                " '" + values.packing_charges + "'," +
                " '" + values.insurance_charges + "'," +
                " '" + values.customer_branch + "')";

                mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            DataSet ds_tsalesorderadd;
            //DataTable objtbl;
            if (mnResult == 1)
            {
                msSQL = "delete from smr_trn_tsalesorderdtl where salesorder_gid='" + values.salesorder_gid + "'";
                mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult3 == 1)
                {
                    msSQL = " select  tmpsalesorderdtl_gid, salesorder_gid, product_gid, productgroup_gid," +
                           " productgroup_name, customerproduct_code,  product_name, display_field, product_price, qty_quoted, product_delivered, format(margin_percentage,2) as margin_percentage," +
                           " format(margin_amount,2) as margin_amount,  uom_gid, uom_name, format(price,2) as price, vendor_gid, slno, tax_name, tax_name2, " +
                           " tax_name3, tax1_gid,  tax2_gid,  tax3_gid,  tax_amount, tax_amount2, tax_amount3,  tax_percentage, tax_percentage2, tax_percentage3,salesorder_refno, " +
                           " product_requireddateremarks, selling_price,product_requireddate,order_type" +
                           " from smr_tmp_tsalesorderdtl where salesorder_gid='" + values.salesorder_gid + "'" +
                           " order by tmpsalesorderdtl_gid asc ";
                    ds_tsalesorderadd = objdbconn.GetDataSet(msSQL, "smr_tmp_tsalesorderdtl");
                    objtbl = ds_tsalesorderadd.Tables["smr_tmp_tsalesorderdtl"];
                    foreach (DataRow dt in objtbl.Rows)
                    {
                        lstmpquotationgid = dt["tmpsalesorderdtl_gid"].ToString();
                        lsproductgroup_gid = dt["productgroup_gid"].ToString();
                        lsproductgroup = dt["productgroup_name"].ToString();
                        lscustomerproduct_code = dt["customerproduct_code"].ToString();
                        lsproductname_gid = dt["product_gid"].ToString();
                        lsproductname = dt["product_name"].ToString();
                        lsuom_gid = dt["uom_gid"].ToString();
                        lsuom = dt["uom_name"].ToString();
                        lsunitprice = dt["product_price"].ToString();
                        lsquantity = dt["qty_quoted"].ToString();
                        lsproduct_delivered = dt["product_delivered"].ToString();
                        lsdiscountpercentage = dt["margin_percentage"].ToString();
                        lsdiscountamount = dt["margin_amount"].ToString();
                        lstotalamount = dt["price"].ToString();
                        lsdisplay_field = dt["display_field"].ToString();
                        lslocalsellingprice = double.Parse(dt["selling_price"].ToString());
                        lsvendor_gid = dt["vendor_gid"].ToString();
                        lsslno = dt["slno"].ToString();
                        lstaxamount_1 = dt["tax_amount"].ToString();
                        lstaxamount_2 = dt["tax_amount2"].ToString();
                        lstaxamount_3 = dt["tax_amount3"].ToString();
                        lstax_name1 = dt["tax_name"].ToString();
                        lstax_name2 = dt["tax_name2"].ToString();
                        lstax_name3 = dt["tax_name3"].ToString();
                        //-----Currency multiplication starts here-----
                        lslocaltaxamount1 = Convert.ToDouble(lstaxamount_1) * double.Parse(values.exchange_rate);
                        lslocaltaxamount2 = Convert.ToDouble(lstaxamount_2) * double.Parse(values.exchange_rate);
                        lslocaltaxamount3 = Convert.ToDouble(lstaxamount_3) * double.Parse(values.exchange_rate);
                        lslocalproductdiscount = Convert.ToDouble(lsdiscountamount) * double.Parse(values.exchange_rate);
                        lslocalproductprice = Convert.ToDouble(lsunitprice) * double.Parse(values.exchange_rate);
                        lslocalproduct_totalprice = Convert.ToDouble(lstotalamount) * double.Parse(values.exchange_rate);
                        lslocalselling_price = Convert.ToDouble(lslocalsellingprice) * double.Parse(values.exchange_rate);
                        
                        msSQL1 = "insert into smr_trn_tsalesorderdtl (" +
                            " salesorderdtl_gid ," +
                            " salesorder_gid," +
                            " product_gid ," +
                            " productgroup_gid," +
                            " productgroup_name," +
                            " customerproduct_code," +
                            " product_name," +
                            " display_field," +
                            " product_price," +
                            " qty_quoted," +
                            " product_delivered," +
                            " discount_percentage," +
                            " discount_amount," +
                            " margin_percentage," +
                            " margin_amount," +
                            " uom_gid," +
                            " uom_name," +
                            " price," +
                            " selling_price," +
                            " discount_amount_l, " +
                            " product_price_l, " +
                            " price_l, " +
                            " vendor_gid," +
                            " slno," +
                            " vendor_price," +
                            " tax_name," +
                            " tax_name2," +
                            " tax_name3," +
                            " tax1_gid," +
                            " tax2_gid," +
                            " tax3_gid," +
                            " tax_amount ," +
                            " tax_amount2," +
                            " tax_amount3," +
                            " tax_percentage," +
                            " tax_percentage2," +
                            " tax_percentage3," +
                            " type, " +
                            " salesorder_refno" +
                            ")values(" +
                            " '" + lstmpquotationgid + "'," +
                            " '" + values.salesorder_gid + "'," +
                            " '" + lsproductname_gid + "'," +
                            " '" + lsproductgroup_gid + "'," +
                            " '" + lsproductgroup + "'," +
                            " '" + lscustomerproduct_code + "'," +
                            " '" + lsproductname + "'," +
                            " '" + lsdisplay_field + "'," +
                            " '" + lslocalsellingprice + "'," +
                            " '" + lsquantity + "'," +
                            " '" + lsproduct_delivered + "'," +
                             " '" + lsdiscountpercentage + "'," +
                            " '" + lsdiscountamount + "'," +
                            " '" + lsdiscountpercentage + "'," +
                            " '" + lsdiscountamount + "'," +
                            " '" + lsuom_gid + "'," +
                            " '" + lsuom + "'," +
                            " '" + lstotalamount + "'," +
                            " '" + lslocalsellingprice + "'," +
                            " '" + lslocalproductdiscount + "'," +
                            " '" + lslocalselling_price + "'," +
                            " '" + lslocalproduct_totalprice + "'," +
                            " '" + lsvendor_gid + "'," +
                            " '" + lsslno + "'," +
                            " '" + lsunitprice + "'," +
                            " '" + lstax_name1 + "'," +
                            " '" + lstax_name2 + "'," +
                            " '" + lstax_name3 + "'," +
                            " '" + dt["tax1_gid"].ToString() + "'," +
                            " '" + dt["tax2_gid"].ToString() + "'," +
                            " '" + dt["tax3_gid"].ToString() + "'," +
                            " '" + lstaxamount_1 + "'," +
                            " '" + lstaxamount_2 + "'," +
                            " '" + lstaxamount_3 + "'," +
                            " '" + dt["tax_percentage"].ToString() + "'," +
                            " '" + dt["tax_percentage2"].ToString() + "'," +
                            " '" + dt["tax_percentage3"].ToString() + "'," +
                            " '" + dt["order_type"].ToString() + "', " +
                            " '" + dt["salesorder_refno"].ToString() + "' " +
                            " '')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Sales order Amended Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error Occured While Amend Sales order";
                    }

                }

            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Submitting  Amend Sales order !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           



        }

        // Overall Submit

        public void DaPostSalesOrder(string employee_gid, postsales_list values)
        {
            try
            {
                



                string lscustomerbranch = "H.Q";
            string lscampaign_gid = "NO CAMPAIGN";

            msSQL = " select * from smr_tmp_tsalesorderdtl " +
                " where employee_gid='" + employee_gid + "'";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            if (mnResult != 0)
            {
                values.status = true;
                values.message = "Select one Product to Raise Enquiry";
            }
            string inputDate = values.salesorder_date;
            DateTime uiDate = DateTime.ParseExact(inputDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
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

            string lslocaladdon = "0.00";
            string lslocaladditionaldiscount = "0.00";
            string lslocalgrandtotal = " 0.00";
            string lsgst = "0.00";
            //string lsproducttotalamount = "0.00";
           
            mssalesorderGID = objcmnfunctions.GetMasterGID("VSOP");

            msSQL = " insert  into smr_trn_tsalesorder (" +
                     " salesorder_gid ," +
                     " branch_gid ," +
                     " salesorder_date," +
                     " customer_gid," +
                     " customer_name," +
                     " customer_contact_gid," +
                     " customerbranch_gid," +
                     " customer_contact_person," +
                     " customer_address," +
                     " customer_email, " +
                     " customer_mobile, " +
                     " created_by," +
                     " so_referencenumber," +
                     " so_referenceno1 ," +
                     " so_remarks," +
                     " payment_days, " +
                     " delivery_days, " +
                     " Grandtotal, " +
                     " termsandconditions, " +
                     " salesorder_status, " +
                     " addon_charge, " +
                     " additional_discount, " +
                     " addon_charge_l, " +
                     " additional_discount_l, " +
                     " grandtotal_l, " +
                     " currency_code, " +
                     " currency_gid, " +
                     " exchange_rate, " +
                     " shipping_to, " +
                     " freight_terms, " +
                     " payment_terms," +
                     " tax_gid," +
                     " tax_name, " +
                     " gst_amount," +
                     " total_price," +
                     " total_amount," +
                     " vessel_name," +
                     " salesperson_gid," +
                     " start_date, " +
                     " end_date, " +
                     " roundoff, " +
                     " updated_addon_charge, " +
                     " updated_additional_discount, " +
                     " freight_charges," +
                     " buyback_charges," +
                     " packing_charges," +
                     " insurance_charges " +
                     " )values(" +
                     " '" + mssalesorderGID + "'," +
                     " '" + values.branch_name + "'," +
                     " '" + salesorder_date + "'," +
                     " '" + values.customer_gid + "'," +
                     " '" + values.customer_name + "'," +
                     " '" + values.customercontact_gid + "'," +
                     " '" + lscustomerbranch + "'," +
                     " '" + values.customercontact_names + "'," +
                     " '" + values.customer_address + "'," +
                     " '" + values.customer_email + "'," +
                     " '" + values.customer_mobile + "'," +
                     " '" + employee_gid + "'," +
                     " '" + values.so_referencenumber + "'," +
                     " '" + values.salesorder_refno + "'," +
                     " '" + values.so_remarks + "'," +
                     " '" + values.payment_days + "'," +
                     " '" + values.delivery_days + "'," +
                     " '" + values.grandtotal + "'," +
                     " '" + values.termsandcondition + "'," +
                     " 'Approved'," +
                     " '" + lslocaladdon + "'," +
                     " '" + lslocaladditionaldiscount + "'," +
                     " '" + lslocaladdon + "'," +
                     " '" + lslocaladditionaldiscount + "'," +
                     " '" + lslocalgrandtotal + "'," +
                     " '" + values.currencyexchange_gid + "'," +
                     " '" + values.currency_code + "'," +
                     " '" + values.exchange_rate + "'," +
                     " '" + values.shipping_to + "'," +
                     "'" + values.freight_terms + "'," +
                     "'" + values.payment_terms + "'," +
                     " '" + values.tax_name4 + "'," +
                     " '" + values.txttaxamount_1 + "', " +
                     " '" + lsgst + "'," +
                     " '" + values.producttotalamount + "'," +
                     " '" + values.total_price + "'," +
                     " '" + values.vessel_name + "'," +
                     " '" + values.user_name + "'," +
                     " '" + start_date + "'," +
                     " '" + end_date + "',";           
            if (values.roundoff == "")
            {
                msSQL += "'0.00',";
            }
            else
            {
                msSQL += "'" + values.roundoff + "',";
            }
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
            if (values.freight_charges == "")
            {
                msSQL += "'0.00',";
            }
            else
            {
                msSQL += "'" + values.freight_charges + "',";
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
                values.message = " Some Error Occurred While Inserting Salesorder Details";
            }
            else
            {
                msSQL = " insert  into acp_trn_torder (" +
                       " salesorder_gid ," +
                       " branch_gid ," +
                       " salesorder_date," +
                       " customer_gid," +
                       " customer_name," +
                       " customer_contact_gid," +
                       " customerbranch_gid," +
                       " customer_contact_person," +
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
                       " addon_charge_l, " +
                       " additional_discount_l, " +
                       " grandtotal_l, " +
                       " currency_code, " +
                       " currency_gid, " +
                       " exchange_rate, " +
                       " updated_addon_charge, " +
                       " updated_additional_discount, " +
                       " shipping_to, " +
                       " campaign_gid, " +
                       " vessel_name," +
                       " roundoff," +
                       " salesperson_gid, " +
                       " freight_charges," +
                       " buyback_charges," +
                       " packing_charges," +
                       " insurance_charges " +
                       ") values(" +
                       " '" + mssalesorderGID + "'," +
                       " '" + values.branch_name + "'," +
                       " '" + salesorder_date + "'," +
                       " '" + lscustomer_gid + "'," +
                       " '" + lscustomer_name + "'," +
                       " '" + values.customercontact_gid + "'," +
                       " '" + lscustomerbranch + "'," +
                       " '" + values.customercontact_names + "'," +
                       " '" + values.customer_address + "'," +
                       " '" + values.customer_email + "'," +
                       " '" + values.customer_mobile + "'," +
                       " '" + employee_gid + "'," +
                       " '" + values.so_referencenumber + "'," +
                       " '" + values.so_remarks + "'," +
                       " '" + values.so_referenceno1 + "'," +
                       " '" + values.payment_days + "'," +
                       " '" + values.delivery_days + "'," +
                       " '" + values.grandtotal + "'," +
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
                msSQL += "'" + lslocaladdon + "'," +
                       " '" + lslocaladditionaldiscount + "'," +
                       " '" + lslocalgrandtotal + "'," +
                       " '" + values.currencyexchange_gid + "'," +
                       " '" + values.currency_code + "'," +
                       " '" + values.exchange_rate + "',";
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
                msSQL += "'" + values.shipping_to + "'," +
                       " '" + lscampaign_gid + "'," +
                       " '" + values.vessel_name + "',";
                       if (values.roundoff == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.roundoff + "',";
                }
                msSQL += "'"+ values.user_name + "',";
                       if (values.freight_charges == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.freight_charges + "',";
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
                mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult2 == 1)
                {
                    values.status = true;
                }

            }

            msSQL = " select " +
                    " tmpsalesorderdtl_gid," +
                    " salesorder_gid," +
                    " product_gid," +
                    " productgroup_gid," +
                    " productgroup_name," +
                    " customerproduct_code," +
                    " product_name," +
                     " product_code," +
                    " display_field, " +
                    " product_price," +
                    " qty_quoted," +
                    " discount_percentage," +
                    " discount_amount," +
                    " uom_gid," +
                    " uom_name," +
                    " price," +
                    " tax_name," +
                    " tax_name2," +
                    " tax_name3," +
                    " tax1_gid, " +
                    " tax2_gid, " +
                    " tax3_gid, " +
                    " tax_amount," +
                    " tax_amount2," +
                    " tax_amount3, " +                  
                    " vendor_gid," +
                    " slno," +
                    " product_requireddate, " +
                    " product_requireddateremarks, " +
                    " tax_percentage," +
                    " tax_percentage2," +
                    " tax_percentage3," +
                    " selling_price,order_type " +
                    " from smr_tmp_tsalesorderdtl" +
                    " where employee_gid='" + employee_gid + "' ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<postsales_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new postsales_list
                    {
                        salesorder_gid = dt["salesorder_gid"].ToString(),
                        tmpsalesorderdtl_gid = dt["tmpsalesorderdtl_gid"].ToString(),
                        productgroup_gid = dt["productgroup_gid"].ToString(),
                        productgroup_name = dt["productgroup_name"].ToString(),
                        customerproduct_code = dt["customerproduct_code"].ToString(),
                        product_gid = dt["product_gid"].ToString(),
                        product_name = dt["product_name"].ToString(),
                        product_code = dt["product_code"].ToString(),
                        // productuom_gid = dt["productuom_gid"].ToString(),
                        productuom_name = dt["customerproduct_code"].ToString(),
                        unitprice = dt["product_price"].ToString(),
                        quantity = dt["qty_quoted"].ToString(),
                        discountpercentage = dt["discount_percentage"].ToString(),
                        discountamount = dt["discount_amount"].ToString(),
                        tax_name = dt["tax_name"].ToString(),
                        //tax_name2 = dt["tax_name2"].ToString(),
                        //tax_name3 = dt["tax_name3"].ToString(),
                        tax_amount = dt["tax_amount"].ToString(),
                        //tax_amount2 = dt["tax_amount2"].ToString(),
                        //tax_amount3 = dt["tax_amount3"].ToString(),
                        totalamount = dt["price"].ToString(),
                        display_field = dt["display_field"].ToString(),
                        vendor_gid = dt["vendor_gid"].ToString(),
                        order_type = dt["order_type"].ToString(),
                        slno = dt["slno"].ToString(),
                        //selling_price = dt["selling_price"].ToString()

                    });


                   
                    int i =0;

                    mssalesorderGID1 = objcmnfunctions.GetMasterGID("VSDC");
                    if (mssalesorderGID1 == "E")
                    {
                        values.message = "Create Sequence code for VSDC ";
                    }



                    msSQL = " insert into smr_trn_tsalesorderdtl (" +
                         " salesorderdtl_gid ," +
                         " salesorder_gid," +
                         " product_gid ," +
                         //" productgroup_gid," +
                         //" productgroup_name," +
                         //" customerproduct_code," +
                         " product_name," +
                         " product_code," +
                         " display_field," +
                         " product_price," +
                         " qty_quoted," +
                         " discount_percentage," +
                         " discount_amount," +
                         " tax_amount ," +
                         " uom_gid," +
                         " uom_name," +
                         " price," +
                         " tax_name," +
                         " tax_name2," +
                         " tax_name3," +
                         " tax1_gid," +
                         " tax2_gid," +
                         " tax3_gid," +
                         " tax_amount2," +
                         " tax_amount3," +
                         //" tax_amount_l ," +
                         //" tax_amount2_l," +
                         //" tax_amount3_l," +
                         //" discount_amount, " +
                         //" product_price_l, " +
                         //" price_l, " +
                         //" salesorder_refno," +
                         //" vendor_gid," +
                         " slno," +
                        // " product_requireddate, " +
                         " product_requireddateremarks," +
                         //" vendor_price ," +
                         " tax_percentage," +
                         " tax_percentage2," +
                         " tax_percentage3," +
                         " type, " +
                         " selling_price" +
                         ")values(" +
                         " '" + mssalesorderGID1 + "'," +
                         " '" + mssalesorderGID + "'," +
                         " '" + dt["product_gid"].ToString() + "'," +
                         //" '" + dt["productgroup_gid"].ToString() + "'," +
                         //" '" + dt["productgroup_name"].ToString() + "'," +
                         //" '" + dt["customerproduct_code"].ToString() + "'," +
                         " '" + dt["product_name"].ToString() + "'," +
                         " '" + dt["product_code"].ToString() + "'," +
                         " '" + dt["display_field"].ToString() + "'," +
                         " '" + dt["product_price"].ToString() + "'," +
                         " '" + dt["qty_quoted"].ToString() + "'," +
                         " '" + dt["discount_percentage"].ToString() + "'," +
                         " '" + dt["discount_amount"].ToString() + "'," +
                         " '" + dt["tax_amount"].ToString() + "'," +
                         " '" + dt["uom_gid"].ToString() + "'," +
                         " '" + dt["uom_name"].ToString() + "'," +
                         " '" + dt["price"].ToString() + "'," +
                         " '" + dt["tax_name"].ToString() + "'," +
                         " '" + dt["tax_name2"].ToString() + "'," +
                         " '" + dt["tax_name3"].ToString() + "'," +
                         " '" + dt["tax1_gid"].ToString() + "'," +
                         " '" + dt["tax2_gid"].ToString() + "'," +
                         " '" + dt["tax3_gid"].ToString() + "'," +
                         " '" + dt["tax_amount2"].ToString() + "'," +
                         " '" + dt["tax_amount3"].ToString() + "'," +
                         //" '" + dt["taxamount_l"].ToString() + "'," +
                         //" '" + dt["taxamount2_l"].ToString() + "'," +
                         //" '" + dt["taxamount3_l"].ToString() + "'," +
                         //" '" + dt["discount_amount"].ToString() + "'," +
                         //" '" + dt["product_price_l"].ToString() + "'," +
                         //" '" + dt["price_l"].ToString() + "'," +
                         //" '" + dt["salesorder_refno"].ToString() + "'," +
                         //" '" + dt["vendor_gid"].ToString() + "'," +
                         " '" +  i+1 + "',";
                    //if (dt["product_requireddate"].ToString() == null || DBNull.Value.Equals(dt["product_requireddate"].ToString()))
                    //{
                    //    msSQL += "null,";
                    //}
                    //else
                    //{
                    //    string formattedDate = ((DateTime)dt["product_requireddate"]).ToString("yyyy-MM-dd");
                    //    msSQL += "'" + formattedDate + "',";
                    //}
                    msSQL += "'" + dt["product_requireddateremarks"].ToString() + "'," +
                     //" '" + dt["unitprice"].ToString() + "'," +
                    " '" + dt["tax_percentage"].ToString() + "'," +
                    " '" + dt["tax_percentage2"].ToString() + "'," +
                    " '" + dt["tax_percentage3"].ToString() + "'," +
                    " '" + dt["order_type"].ToString() + "', " +
                    " '" + dt["selling_price"].ToString() + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 0)
                    {
                        values.status = false;
                        values.message = "Error occurred while Insertion";
                    }

                    //msSQL = " delete from smr_trn_tsalesorder where salesorder_gid='" + mssalesorderGID + "' ";
                    //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    //msSQL = " delete from acp_trn_torder where salesorder_gid='" + mssalesorderGID + "' ";
                    //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                    msSQL = " insert into acp_trn_torderdtl (" +
                     " salesorderdtl_gid ," +
                     " salesorder_gid," +
                     " product_gid ," +
                     //" productgroup_gid," +
                     //" productgroup_name," +
                     " product_name," +
                     //" display_field," +
                     " product_price," +
                     " qty_quoted," +
                     " discount_percentage," +
                     " discount_amount," +
                     " tax_amount ," +
                     " uom_gid," +
                     " uom_name," +
                     " price," +
                     " tax_name," +
                     " tax_name2," +
                     " tax_name3," +
                     " tax1_gid," +
                     " tax2_gid," +
                     " tax3_gid," +
                     " tax_amount2," +
                     " tax_amount3," +
                     //" tax_amount_l ," +
                     //" tax_amount2_l," +
                     //" tax_amount3_l," +
                     //" discount_amount_l, " +
                     //" product_price_l, " +
                     //" price_l, " +
                     " vendor_gid," +
                     " slno," +
                   //  " product_requireddate, " +
                     " product_requireddateremarks, " +
                     " tax_percentage," +
                     " tax_percentage2," +
                     " tax_percentage3," +
                     " type, " +
                     " salesorder_refno" +
                     ")values(" +
                     " '" + mssalesorderGID1 + "'," +
                     " '" + mssalesorderGID + "'," +
                     " '" + dt["product_gid"].ToString() + "'," +
                     //" '" + dt["productgroup_gid"].ToString() + "'," +
                     //" '" + dt["productgroup_name"].ToString() + "'," +
                     " '" + dt["product_name"].ToString() + "'," +
                     //" '" + dt["display_field"].ToString() + "'," +
                     " '" + dt["product_price"].ToString() + "'," +
                     " '" + dt["qty_quoted"].ToString() + "'," +
                     " '" + dt["discount_percentage"].ToString() + "'," +
                     " '" + dt["discount_amount"].ToString() + "'," +
                     " '" + dt["tax_amount"].ToString() + "'," +
                     " '" + dt["uom_gid"].ToString() + "'," +
                     " '" + dt["uom_name"].ToString() + "'," +
                     " '" + dt["price"].ToString() + "'," +
                     " '" + dt["tax_name"].ToString() + "'," +
                     " '" + dt["tax_name2"].ToString() + "'," +
                     " '" + dt["tax_name3"].ToString() + "'," +
                     " '" + dt["tax1_gid"].ToString() + "'," +
                     " '" + dt["tax2_gid"].ToString() + "'," +
                     " '" + dt["tax3_gid"].ToString() + "'," +
                     " '" + dt["tax_amount2"].ToString() + "'," +
                     " '" + dt["tax_amount3"].ToString() + "'," +
                     //" '" + dt["tax_amount_l"].ToString() + "'," +
                     //" '" + dt["tax_amount2_l"].ToString() + "'," +
                     //" '" + dt["tax_amount3_l"].ToString() + "'," +
                     //" '" + dt["discount_amount_l"].ToString() + "'," +
                     //" '" + dt["product_price_l"].ToString() + "'," +
                     //" '" + dt["price_l"].ToString() + "'," +
                     " '" + dt["vendor_gid"] + "'," +
                     " '" + values.slno + "',";
                    //if (dt["product_requireddate"].ToString() == null || DBNull.Value.Equals(dt["product_requireddate"].ToString()))
                    //{
                    //    msSQL += "null,";
                    //}
                    //else
                    //{
                    //    string formattedDate = ((DateTime)dt["product_requireddate"]).ToString("yyyy-MM-dd");
                    //    msSQL += "'" + formattedDate + "',";
                    //}
                    msSQL += "'" + dt["product_requireddateremarks"].ToString() + "'," +

               " '" + dt["tax_percentage"].ToString() + "'," +
               " '" + dt["tax_percentage2"].ToString() + "'," +
               " '" + dt["tax_percentage3"].ToString() + "'," +
              " '" + dt["order_type"].ToString() + "', " +
               " '" + values.salesorder_refno + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                }
            }




            msSQL = "select distinct type from smr_trn_tsalesorderdtl where salesorder_gid='" + mssalesorderGID + "' ";
            objMySqlDataReader = objdbconn.GetDataReader(msSQL);
            if (objMySqlDataReader.HasRows == true)
            {

                  lsorder_type = "Sales";

                 
                    }

                    else
                    {
                        lsorder_type = "Service";
                    }
                

            


            msSQL = " update smr_trn_tsalesorder set so_type='" + lsorder_type + "' where salesorder_gid='" + mssalesorderGID + "' ";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            msSQL = " update acp_trn_torder set so_type='" + lsorder_type + "' where salesorder_gid='" + mssalesorderGID + "' ";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


            msSQL = "select hierarchy_flag from adm_mst_tcompany where hierarchy_flag ='Y'";
            objMySqlDataReader = objdbconn.GetDataReader(msSQL);
            if (objMySqlDataReader.HasRows == true)
            {
                msGetGID = objcmnfunctions.GetMasterGID("PODC");
                msSQL = " insert into smr_trn_tapproval ( " +
                " approval_gid, " +
                " approved_by, " +
                " approved_date, " +
                " submodule_gid, " +
                " soapproval_gid " +
                " ) values ( " +
                "'" + msGetGID + "'," +
                " '" + employee_gid + "'," +
                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                "'SMRSROSOA'," +
                "'" + mssalesorderGID + "') ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                if (mnResult == 0)
                {
                    values.status = false;
                }


                msSQL = "select approval_flag from smr_trn_tapproval where submodule_gid='SMRSROSOA' and soapproval_gid='" + mssalesorderGID + "' ";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == false)
                {
                    msSQL = "update smr_trn_tsalesorder set salesorder_status='Approved',salesorder_remarks='" + values.so_remarks + "', " +
                           " approved_by='" + employee_gid + "', approved_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "' where salesorder_gid='" + mssalesorderGID + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = "update acp_trn_torder set salesorder_status='Approved',salesorder_remarks='" + values.so_remarks + "', " +
                          " approved_by='" + employee_gid + "', approved_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "' where salesorder_gid='" + mssalesorderGID + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                else
                {
                    msSQL = "select approved_by from smr_trn_tapproval where submodule_gid='SMRSROSOA' and soapproval_gid='" + mssalesorderGID + "'";
                    objMySqlDataReader1 = objdbconn.GetDataReader(msSQL);
                }
                if (objMySqlDataReader1.RecordsAffected == 1)
                {

                    msSQL = " update smr_trn_tapproval set " +
                   " approval_flag = 'Y', " +
                   " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                   " where approved_by = '" + employee_gid + "'" +
                   " and soapproval_gid = '" + mssalesorderGID + "' and submodule_gid='SMRSROSOA'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    msSQL = "update smr_trn_tsalesorder set salesorder_status='Approved',salesorder_remarks='" + values.so_remarks + "', " +
                       " approved_by='" + employee_gid + "', approved_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "' where salesorder_gid='" + mssalesorderGID + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = "update acp_trn_torder set salesorder_status='Approved',salesorder_remarks='" + values.so_remarks + "', " +
                          " approved_by='" + employee_gid + "', approved_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "' where salesorder_gid='" + mssalesorderGID + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                else
                {
                    msSQL = " update smr_trn_tapproval set " +
                               " approval_flag = 'Y', " +
                               " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                               " where approved_by = '" + employee_gid + "'" +
                               " and soapproval_gid = '" + mssalesorderGID + "' and submodule_gid='SMRSROSOA'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if(mnResult !=0)
                {
                    msSQL = " select leadbank_gid from crm_trn_tleadbank where customer_gid='" + values.customer_gid + "'";
                    string lsleadbank_gid = objdbconn.GetExecuteScalar(msSQL);
      
                    msSQL = " update crm_trn_tlead2campaign  set " +
                                        " leadstage_gid=' 6 '" +
                                        " where leadbank_gid='" + lsleadbank_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                    if(mnResult !=0 || mnResult == 0)
                    {

                        msSQL = " delete from smr_tmp_tsalesorderdtl " +
                                " where employee_gid='" + employee_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }

                
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Sales Order Successfully Raised";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Raising Sales Order";
                }

            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Submitting Sales Order !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           

        }

        //product summary//
        public void DaGetSalesOrdersummary(string employee_gid, MdlSmrTrnSalesorder values)
        {
            try
            {
                

                double grand_total = 0.00;
            //double grandtotal = 0.00;

             msSQL = "SELECT a.tmpsalesorderdtl_gid,a.quotation_gid, a.tax_name, a.tax_amount, a.salesorderdtl_gid, a.salesorder_gid, a.product_gid, a.productgroup_gid, " +
                " a.productgroup_name, a.product_name, FORMAT(a.product_price, 2) AS product_price, b.product_code, a.qty_quoted, a.product_remarks, " +
                " a.uom_gid, a.vendor_gid, a.slno, a.uom_name, FORMAT(a.price, 2) AS price, " +
                " FORMAT(a.discount_percentage,2) AS discount_percentage, FORMAT(a.discount_amount,2) AS discount_amount, " +
                " FORMAT(a.selling_price, '0.00') AS selling_price " +
                " FROM smr_tmp_tsalesorderdtl a " +
                " LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                " LEFT JOIN acp_mst_tvendor e ON a.vendor_gid = e.vendor_gid " +
                " WHERE a.employee_gid = '" + employee_gid + "' group by a.product_gid, b.delete_flag = 'N' and a.quotation_gid is null" +
                " ORDER BY a.slno ASC";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<salesorders_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    grand_total += double.Parse(dt["price"].ToString());
                    //grandtotal += double.Parse(dt["price"].ToString());
                    getModuleList.Add(new salesorders_list
                    {
                        tmpsalesorderdtl_gid = dt["tmpsalesorderdtl_gid"].ToString(),
                        salesorder_gid = dt["salesorder_gid"].ToString(),
                        product_name = dt["product_name"].ToString(),
                        product_gid = dt["product_gid"].ToString(),
                        product_code = dt["product_code"].ToString(),
                        slno = dt["slno"].ToString(),
                        discountamount = dt["discount_amount"].ToString(),
                        discountpercentage = double.Parse(dt["discount_percentage"].ToString()),
                        //productgroup_name = dt["productgroup_name"].ToString(),
                        product_price = dt["product_price"].ToString(),                        
                        quantity = double.Parse(dt["qty_quoted"].ToString()),
                        uom_gid = dt["uom_gid"].ToString(),
                        productuom_name = dt["uom_name"].ToString(),
                        producttotalamount = dt["price"].ToString(),
                        totalamount = dt["price"].ToString(),
                        tax_name = dt["tax_name"].ToString(),                       
                        tax_amount = dt["tax_amount"].ToString(),                        
                        grand_total= grand_total,
                        // grandtotal = grandtotal



                    });
                    values.salesorders_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            values.grand_total = grand_total;
                //values.grandtotal = grandtotal;
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Order Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           

        }
        public void GetDeleteDirectSOProductSummary(string tmpsalesorderdtl_gid, salesorders_list values)
        {
            try
            {
               


                msSQL = "select price from smr_tmp_tsalesorderdtl " +
                    " where tmpsalesorderdtl_gid='" + tmpsalesorderdtl_gid + "'";
            objMySqlDataReader = objdbconn.GetDataReader(msSQL);
            if (objMySqlDataReader.HasRows == true)

            {
                lsprice = objMySqlDataReader["price"].ToString();
            }

            msSQL = " delete from smr_tmp_tsalesorderdtl " +
                    " where tmpsalesorderdtl_gid='" + tmpsalesorderdtl_gid + "'";
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
                values.message = "Exception occured while deleting DirectSO product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }

        public void DaupdateSalesOrderedit(string employee_gid, salesorders_list values)
        {
            try
            {
               


                string lstax_name;
            string lstax_gid;
            string lstax_amount;

            msSQL = "select product_gid from pmr_mst_tproduct where product_name='" + values.product_name + "'";
            string lsproductgid = objdbconn.GetExecuteScalar(msSQL);

            msSQL = "select productuom_gid from pmr_mst_tproductuom where productuom_name='" + values.unit + "'";
            string lsproductuomgid = objdbconn.GetExecuteScalar(msSQL);

            msSQL = "select productgroup_gid from pmr_mst_tproductgroup where productgroup_name='" + values.productgroup_name + "'";
            string lsproductgroupgid = objdbconn.GetExecuteScalar(msSQL);

            if (values.tax_name == "" || values.tax_name == null)
            {
                lstax_name = "No tax";
            }
            else
            {
                lstax_name = values.tax_name;
                //msSQL = "select tax_name from acp_mst_ttax where tax_gid='" + values.tax_name + "'";
                //lstax_name = objdbconn.GetExecuteScalar(msSQL);
            }
            if (values.tax_amount == null)
            {
                lstax_amount = "0";
            }
            else
            {

                lstax_amount = values.tax_amount;
            }
            if (values.tax_gid == null)
            {
                lstax_gid = "";
            }
            else
            {

                lstax_gid = values.tax_gid;
            }

            msSQL = "update smr_tmp_tsalesorderdtl set " +
                " product_gid='" + lsproductgid + "', " +
                " product_name='" + values.product_name + "', " +
                " product_code='" + values.product_code + "', " +
                " productgroup_gid='" + lsproductgroupgid + "', " +
                " productgroup_name='" + values.productgroup_name + "', " +
                " uom_gid='" + lsproductuomgid + "', " +
                " uom_name='" + values.unit + "', " +
                " discount_amount='" + values.discountamount + "', " +
                " discount_percentage='" + values.discount_percentage + "', " +
                " qty_quoted='" + values.quantity + "', " +
                " tax1_gid='" + lstax_gid + "', " +
                " tax_name='" + lstax_name + "',"+
                " price='" + values.total_amount + "'," +
                " tax_amount='" + lstax_amount + "'"+
                " where tmpsalesorderdtl_gid='" + values.tmpsalesorderdtl_gid + "' ";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Sales Order !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            


        }

            //product add//

            public void DaPostOnAdds(string employee_gid, salesorders_list values)
        {
            try
            {
               

                msGetGid = objcmnfunctions.GetMasterGID("VSDT");
            
            //tmpsalesorderdtl_gid = msGetGid;

            msSQL = "select product_gid from pmr_mst_tproduct where product_name='" + values.product_name + "'";
            string lsproductgid = objdbconn.GetExecuteScalar(msSQL);

            msSQL = "select productuom_gid from pmr_mst_tproductuom where productuom_name='" + values.productuom_name + "'";
            string lsproductuomgid = objdbconn.GetExecuteScalar(msSQL);

           
            if (values.discount_percentage == null || values.discount_percentage == "")
            {
                lsdiscountpercentage = "0.00";
            }
            else
            {
                lsdiscountpercentage = values.discount_percentage;
            }

            if (values.discountamount == null || values.discountamount == "")
            {
                lsdiscountamount = "0.00";
            }
            else
            {
                lsdiscountamount = values.discountamount;
            }


            msSQL = "select tax_name from acp_mst_ttax where tax_gid='" + values.tax_name + "'";
            string lstaxname = objdbconn.GetExecuteScalar(msSQL);
            msSQL = "select percentage from acp_mst_ttax where tax_name='" + lstaxname + "'";
            string lspercentage1 = objdbconn.GetExecuteScalar(msSQL);


            int i = 0;
            msSQL = " SELECT a.producttype_name FROM pmr_mst_tproducttype a " +
             " INNER JOIN pmr_mst_tproduct b ON a.producttype_gid=b.producttype_gid  " +
             " WHERE product_gid='" + lsproductgid + "'";
            objMySqlDataReader = objdbconn.GetDataReader(msSQL);
            if (objMySqlDataReader.HasRows == true)
            {
                if (objMySqlDataReader["producttype_name"].ToString() != "Services")
                {
                    lsorder_type = "Sales";
                }
                else
                {
                    lsorder_type = "Services";
                }

            }

            msSQL = " insert into smr_tmp_tsalesorderdtl( " +
               " tmpsalesorderdtl_gid," +
               " salesorder_gid," +
               " employee_gid," +
               " product_gid," +              
               " product_code," +
               " product_name," +              
               " product_price," +
               " qty_quoted," +
               " uom_gid," +
               " uom_name," +
               " price," +             
               " order_type," +
               " tax_name," +               
               " tax_amount," +              
               " discount_amount, " +
               " discount_percentage, " +
               " tax_percentage" +               
               ")values(" +
               "'" + msGetGid + "'," +
               "'" + values.salesorder_gid + "'," +
               "'" + employee_gid + "'," +
               "'" + lsproductgid + "'," +                          
               "'" + values.product_code + "'," +
               "'" + values.product_name + "'," +               
               "'" + values.unitprice + "'," +
               "'" + values.quantity + "'," +
               "'" + lsproductuomgid + "'," +
               "'" + values.productuom_name + "'," +
               "'" + values.totalamount + "'," +               
               " '" + lsorder_type + "', " +
               "'" + lstaxname + "'," +               
               "'" + values.tax_amount + "'," +            
               "'" + lsdiscountamount + "'," +
             "'" + lsdiscountpercentage + "'," +
             "'" + lspercentage1 + "')";           
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
                values.message = "Exception occured while Adding Product !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        public void DaGetOnchangeCurrency(string currencyexchange_gid, MdlSmrTrnSalesorder values)
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
                values.message = "Exception occured while loading Currency !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        public void DaGetamendsalesorderdetails(string employee_gid, string salesorder_gid, MdlSmrTrnSalesorder values)
        {
            try
            {
               
                msSQL = " select a.tmpsalesorderdtl_gid,a.margin_percentage,a.margin_amount,a.salesorder_gid, " +
             " a.selling_price,a.productgroup_gid,a.productgroup_name, " +
             " if(a.customerproduct_code='+nbsp;',' ',a.customerproduct_code) as customerproduct_code," +
             " a.discount_percentage, format(a.discount_amount,2) as discount_amount, " +
             " a.tax_percentage,format(a.tax_amount,2) as tax_amount,a.vendor_gid,a.payment_days,a.delivery_period,  " +
             " a.product_remarks,format(a.price,2) as price,a.display_field, " +
             " a.tax_name,a.tax_name2,a.tax_name3,a.tax_percentage2,a.tax_percentage3, " +
             " format(a.tax_amount2,2) as tax_amount2,format(a.tax_amount3,2) as tax_amount3, " +
             " a.product_gid,a.product_name,a.uom_gid,a.uom_name,a.qty_quoted, " +
             " a.product_delivered,a.employee_gid,a.vendor_gid,e.vendor_companyname, " +
             " b.product_code,a.slno,a.tax1_gid,a.tax1_gid,a.tax3_gid,a.tax2_gid, " +
             " a.tax3_gid,a.product_requireddateremarks, " +
             " format(a.product_price,2) as product_price,date_format(product_requireddate,'%d-%m-%Y') " +
             " as product_requireddate " +
             " from smr_tmp_tsalesorderdtl a" +
             " left join pmr_mst_tproduct b on a.product_gid= b.product_gid " +
             " left join acp_mst_tvendor e on a.vendor_gid=e.vendor_gid " +
             "  where salesorder_gid='" + salesorder_gid + "' order by salesorder_gid asc ";

            DataTable dt_datatable = objdbconn.GetDataTable(msSQL);

            double grand_total = 0.00;
            var getModuleList = new List<summarydtl_list>();

            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt1 in dt_datatable.Rows)
                {
                    grand_total += double.Parse(dt1["price"].ToString());

                    getModuleList.Add(new summarydtl_list
                    {
                        tmpsalesorderdtl_gid = dt1["tmpsalesorderdtl_gid"].ToString(),
                        margin_percentage = dt1["margin_percentage"].ToString(),
                        margin_amount = dt1["margin_amount"].ToString(),
                        salesorder_gid = dt1["salesorder_gid"].ToString(),
                        selling_price = dt1["selling_price"].ToString(),
                        productgroup_gid = dt1["productgroup_gid"].ToString(),
                        productgroup_name = dt1["productgroup_name"].ToString(),
                        customerproduct_code = dt1["customerproduct_code"].ToString(),
                        discount_percentage = dt1["discount_percentage"].ToString(),
                        discount_amount = dt1["discount_amount"].ToString(),
                        tax_percentage = dt1["tax_percentage"].ToString(),
                        tax_amount = dt1["tax_amount"].ToString(),
                        vendor_gid = dt1["vendor_gid"].ToString(),
                        payment_days = dt1["payment_days"].ToString(),
                        delivery_period = dt1["delivery_period"].ToString(),
                        product_remarks = dt1["product_remarks"].ToString(),
                        price = dt1["price"].ToString(),
                        totalamount= dt1["price"].ToString(),
                        display_field = dt1["display_field"].ToString(),
                        tax_name = dt1["tax_name"].ToString(),
                        product_gid = dt1["product_gid"].ToString(),
                        product_name = dt1["product_name"].ToString(),
                        uom_gid = dt1["uom_gid"].ToString(),
                        uom_name = dt1["uom_name"].ToString(),
                        qty_quoted = dt1["qty_quoted"].ToString(),
                        product_delivered = dt1["product_delivered"].ToString(),
                        employee_gid = dt1["employee_gid"].ToString(),
                        vendor_companyname = dt1["vendor_companyname"].ToString(),
                        product_code = dt1["product_code"].ToString(),
                        slno = dt1["slno"].ToString(),
                        tax1_gid = dt1["tax1_gid"].ToString(),
                        tax_gid = dt1["tax1_gid"].ToString(),
                        product_requireddateremarks = dt1["product_requireddateremarks"].ToString(),
                        product_price = dt1["product_price"].ToString(),
                        product_requireddate = dt1["product_requireddate"].ToString(),
                        grand_total= grand_total
                    });
                }
                values.summarydtl_list = getModuleList;
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Order Details !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

            public void DaGetamendsalesorderdtl(string employee_gid, string salesorder_gid, MdlSmrTrnSalesorder values)
        {
            try
            {
                
                msSQL = " delete from smr_tmp_tsalesorderdtl " +
                 " where employee_gid='" + employee_gid + "'";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            string leadbank_address1, leadbank_name;

            msSQL = " select a.salesorder_gid,date_format(salesorder_date,'%Y-%m-%d') as salesorder_date,date_format(start_date,'%Y-%m-%d') as start_date,date_format(end_date,'%Y-%m-%d') as end_date, "+ 
                    " order_note,a.so_referencenumber,a.customer_gid, " +
                    " concat(a.customerbranch_gid,'|',a.customer_contact_person) as customer_contact_person," +
                    " a.customer_address,a.so_remarks,a.created_by,format(a.total_price,2) as total_price, " +
                    " format(total_amount,2) as total_amount,format(a.roundoff,2) as roundoff,a.payment_days, " +
                    " a.delivery_days ,a.salesorder_status,format(Grandtotal,2) as Grandtotal,format(addon_charge,2) as addon_charge, " +
                    " a.so_referenceno1,format(additional_discount,2) as additional_discount, " +
                    " a.currency_code,format(a.gst_amount,2) as gst_amount,a.exchange_rate,a.customer_contact_gid, " +
                    " a.termsandconditions,a.renewal_date,a.renewal_description ,a.shipping_to ,a.freight_terms, " +
                    " a.payment_terms,a.currency_gid,a.tax_gid,a.campaign_gid, " +
                    " a.vessel_name,a.salesperson_gid,customerbranch_gid,a.customer_email, " +
                    " b.branch_name,a.freight_charges,a.buyback_charges,a.packing_charges,a.insurance_charges,a.customer_mobile,a.customer_name,so_referenceno1  " +
                    " from smr_trn_tsalesorder a " +
                    " left join hrm_mst_tbranch b on a.branch_gid=b.branch_gid " +
                    " where salesorder_gid ='" + salesorder_gid + "'";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getModuleList = new List<Getamendsalesorderdtl>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    msSQL1 = "select leadbank_gid,leadbank_name,leadbank_address1  from crm_trn_tleadbank  where customer_gid='" + dt["customer_gid"].ToString() + "'";
                    if (dt["customerbranch_gid"].ToString() == null || dt["customerbranch_gid"].ToString() == "")
                    {
                        leadbank_name = objdbconn.GetExecuteScalar(msSQL1);
                        leadbank_address1 = objdbconn.GetExecuteScalar(msSQL1);
                    }
                    msSQL = " select concat(c.department_name,' ','/',' ',a.user_firstname,' ',a.user_lastname) as name1 from hrm_mst_temployee a" +
                            " left join adm_mst_tuser b on a.user_gid = b.user_gid "+
                            " left join hrm_mst_tdepartment c on c.department_gid = a.department_gid "+
                            " where a.user_gid='" + dt["salesperson_gid"].ToString() + "'";
                   string  name1 = objdbconn.GetExecuteScalar(msSQL);

                    getModuleList.Add(new Getamendsalesorderdtl
                    {
                        customer_name = dt["customer_name"].ToString(),
                        customer_address = dt["customer_address"].ToString(),
                        roundoff = dt["roundoff"].ToString(),
                        salesorder_gid = dt["salesorder_gid"].ToString(),
                        salesorder_date = dt["salesorder_date"].ToString(),
                        customer_gid = dt["customer_gid"].ToString(),
                        freight_terms = dt["freight_terms"].ToString(),
                        payment_terms = dt["payment_terms"].ToString(),
                        customer_mobile = dt["customer_mobile"].ToString(),
                        tax_gid = dt["tax_gid"].ToString(),
                        gst_amount = dt["gst_amount"].ToString(),
                        customer_contact_person = dt["customer_contact_person"].ToString(),
                        total_price = dt["total_price"].ToString(),
                        shipping_to = dt["shipping_to"].ToString(),
                        termsandconditions = dt["termsandconditions"].ToString(),
                        so_remarks = dt["so_remarks"].ToString(),
                        so_referencenumber = dt["so_referencenumber"].ToString(),
                        Grandtotal = dt["Grandtotal"].ToString(),
                        delivery_days = dt["delivery_days"].ToString(),
                        payment_days = dt["payment_days"].ToString(),
                        additional_discount = dt["additional_discount"].ToString(),
                        addon_charge = dt["addon_charge"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        campaign_gid = dt["campaign_gid"].ToString(),
                        so_referenceno1 = dt["so_referenceno1"].ToString(),
                        total_amount = dt["total_amount"].ToString(),
                        vessel_name = dt["vessel_name"].ToString(),
                        start_date = dt["start_date"].ToString(),
                        end_date = dt["end_date"].ToString(),
                        salesperson_gid = dt["salesperson_gid"].ToString(),
                        user_name = name1,
                        freight_charges = dt["freight_charges"].ToString(),
                        packing_charges = dt["packing_charges"].ToString(),
                        buyback_charges = dt["buyback_charges"].ToString(),
                        currency_code = dt["currency_code"].ToString(),
                        exchange_rate = dt["exchange_rate"].ToString(),
                        currency_gid = dt["currency_gid"].ToString(),
                        insurance_charges = dt["insurance_charges"].ToString(),
                        customer_email = dt["customer_email"].ToString(),
                        salesorder_refno = dt["so_referenceno1"].ToString(),



                    });
                }
                values.Getamendsalesorderdtl = getModuleList;
            }
            


            msSQL = " select salesorderdtl_gid,salesorder_gid,productgroup_gid,productgroup_name,slno," +
                       " product_gid,product_name,qty_quoted,product_price,vendor_price,discount_percentage,vendor_gid, " +
                       " discount_amount,tax_percentage,tax_amount,product_remarks,uom_gid,margin_percentage,margin_amount," +
                       " uom_name,payment_days,delivery_period,price,display_field,selling_price, " +
                       " tax_name,tax_name2,tax_name3,tax_percentage2,tax_percentage3,tax1_gid,tax2_gid,tax3_gid,product_delivered,salesorder_refno," +
                       " tax_amount2,tax_amount3,product_requireddate,type,customerproduct_code,product_requireddateremarks from smr_trn_tsalesorderdtl " +
                       " where salesorder_gid='" + salesorder_gid + "'";
            DataTable dt_datatable3 = objdbconn.GetDataTable(msSQL);
            foreach (DataRow dt1 in dt_datatable3.Rows)
            {
              
                msSQL2 = " insert into smr_tmp_tsalesorderdtl(" +
                           " tmpsalesorderdtl_gid," +
                           " salesorder_gid," +
                           " customerproduct_code, " +
                           " productgroup_gid," +
                           " productgroup_name," +
                           " discount_percentage," +
                           " discount_amount," +
                           " tax_percentage," +
                           " tax_amount," +
                           " product_remarks," +
                           " payment_days," +
                           " delivery_period," +
                           " price," +
                           " display_field," +
                           " tax_name," +
                           " tax_name2," +
                           " tax_name3," +
                           " tax1_gid," +
                           " tax2_gid," +
                           " tax3_gid," +
                           " tax_percentage2," +
                           " tax_percentage3," +
                           " tax_amount2," +
                           " tax_amount3," +
                           " product_gid," +
                           " product_name," +
                           " uom_gid," +
                           " uom_name," +
                           " qty_quoted," +
                           " product_delivered," +
                           " employee_gid," +
                           " selling_price, " +
                           " slno," +
                           " product_requireddate , " +
                           " product_requireddateremarks, " +
                           " vendor_gid," +
                           " margin_percentage," +
                           " margin_amount, " +
                           " product_price," +
                           " order_type ," +
                           " salesorder_refno" +
                           ") " +
                           " values ( " +
                           "'" + dt1["salesorderdtl_gid"].ToString() + "', " +
                           "'" + dt1["salesorder_gid"].ToString() + "', " +
                           "'" + dt1["customerproduct_code"].ToString() + "', " +
                           "'" + dt1["productgroup_gid"].ToString() + "', " +
                           "'" + dt1["productgroup_name"].ToString() + "'," +
                           "'" + dt1["discount_percentage"].ToString() + "', " +
                           "'" + dt1["discount_amount"].ToString() + "', " +
                           "'" + dt1["tax_percentage"].ToString() + "', " +
                           "'" + dt1["tax_amount"].ToString() + "', " +
                           "'" + dt1["product_remarks"].ToString() + "', " +
                           "'" + dt1["payment_days"].ToString() + "', " +
                           "'" + dt1["delivery_period"].ToString() + "', " +
                           "'" + dt1["price"].ToString() + "', " +
                           "'" + dt1["display_field"].ToString() + "', " +
                           "'" + dt1["tax_name"].ToString() + "', " +
                           "'" + dt1["tax_name2"].ToString() + "', " +
                           "'" + dt1["tax_name3"].ToString() + "', " +
                           "'" + dt1["tax1_gid"].ToString() + "', " +
                           "'" + dt1["tax2_gid"].ToString() + "', " +
                           "'" + dt1["tax3_gid"].ToString() + "', " +
                           "'" + dt1["tax_percentage2"].ToString() + "', " +
                           "'" + dt1["tax_percentage3"].ToString() + "', " +
                           "'" + dt1["tax_amount2"].ToString() + "', " +
                           "'" + dt1["tax_amount3"].ToString() + "', " +
                           "'" + dt1["product_gid"].ToString() + "', " +
                           "'" + dt1["product_name"].ToString() + "', " +
                           "'" + dt1["uom_gid"].ToString() + "', " +
                           "'" + dt1["uom_name"].ToString() + "', " +
                           "'" + dt1["qty_quoted"].ToString() + "'," +
                           "'" + dt1["product_delivered"].ToString() + "'," +
                           "'" + employee_gid + "'," +
                           "'" + dt1["product_price"].ToString() + "',";
                if (dt1["slno"].ToString() == null || dt1["slno"].ToString() == "")
                {
                    msSQL2 += "null,";
                }
                else
                {
                    msSQL2 += "'" + dt1["slno"].ToString() + "',";
                }

                if (dt1["product_requireddate"].ToString() == null || dt1["product_requireddate"].ToString() == "")
                {
                    msSQL2 += "null,";
                }
                else
                {
                    msSQL2 += "'" + dt1["product_requireddate"].ToString() + "',";
                }


                msSQL2 += "'" + dt1["product_requireddateremarks"].ToString().Replace("'", "''") + "'," +
                      "'" + dt1["vendor_gid"].ToString() + "'," +
                      "'" + dt1["margin_percentage"].ToString() + "'," +
                      "'" + dt1["margin_amount"].ToString() + "',";
                      

                if (dt1["product_price"] is DBNull)
                {
                    msSQL2 += null + ",";
                }
                else
                {
                    msSQL2 += "'" + dt1["product_price"].ToString() + "',";
                }
                if (dt1["type"] is DBNull)
                {
                    msSQL2 += null + ",";
                }
                else
                {
                    msSQL2 += "'" + dt1["type"] + "',";
                }
                if (dt1["salesorder_gid"] is DBNull)
                {
                    msSQL2 += null + ",";
                }
                else
                {
                    msSQL2 += "'" + dt1["salesorder_gid"].ToString() + "')";
                }

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL2);



            }

    



            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Order dropdown !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           

        }
    


            public void DaGetSalesProductdetails(string salesorder_gid, MdlSmrTrnSalesorder values)
            {
            try
            {
               
                msSQL ="select customerproduct_code,product_code,product_name,qty_quoted from smr_trn_tsalesorderdtl where salesorder_gid ='" + salesorder_gid +" '";
            
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<salesproductlist>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new salesproductlist
                    {
                        product_code = dt["product_code"].ToString(),
                        product_name = dt["product_name"].ToString(),
                        qty_quoted = dt["qty_quoted"].ToString(),

                    });
                    values.salesproduct_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Product Detail !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }

        public void DagetCancelSalesOrder(string salesorder_gid, MdlSmrTrnSalesorder values)
        {
            try
            {
                
                msSQL = "SELECT * FROM smr_trn_tsalesorder WHERE salesorder_gid = '" + salesorder_gid + "' and salesorder_status in('Approved', 'Approve Pending')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult != 0)
            {
                msSQL = "update smr_trn_tsalesorder set salesorder_status='Cancelled' where salesorder_gid='" + salesorder_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            if (mnResult != 0)
            {
                values.status = true;
                values.message = "Order Cancelled Successfully";
            }
            else
            {
                {
                    values.status = false;
                    values.message = "Error While Cancelling Order";
                }
            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while canceling Sales Order!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           

        }

        // DIRECT SALES ORDER PRODCUCT SUMMARY EDIT
        public void DaGetDirectSalesOrderEditProductSummary(string tmpsalesorderdtl_gid, MdlSmrTrnSalesorder values)
        {
            try
            {
                msSQL = " Select tmpsalesorderdtl_gid, salesorder_gid, product_gid,product_name, product_price, qty_quoted,tax1_gid, discount_percentage," +
                    " discount_amount, tax_amount, tax_name, uom_gid, uom_name, price, product_code from smr_tmp_tsalesorderdtl" +
                    " where tmpsalesorderdtl_gid = '" + tmpsalesorderdtl_gid + "'";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<DirecteditSalesorderList>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new DirecteditSalesorderList
                    {
                        tmpsalesorderdtl_gid = dt["tmpsalesorderdtl_gid"].ToString(),
                        product_gid = dt["product_gid"].ToString(),
                        product_name = dt["product_name"].ToString(),
                        productuom_name = dt["uom_name"].ToString(),                      
                        quantity = dt["qty_quoted"].ToString(),
                        product_code = dt["product_code"].ToString(),
                        unitprice = dt["product_price"].ToString(),
                        discount_percentage = dt["discount_percentage"].ToString(),
                        discountamount = dt["discount_amount"].ToString(),
                        totalamount = dt["price"].ToString(),
                        tax_name = dt["tax_name"].ToString(),
                        tax_gid = dt["tax1_gid"].ToString(),
                        tax_amount = dt["tax_amount"].ToString(),

                    });
                    values.directeditsalesorder_list = getModuleList;
                }
            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Order Product Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }

        public void DaGetSalesorderdetail(string product_gid, MdlSmrTrnSalesorder values)
        {
            try
            {
              
                msSQL = " select a.salesorder_gid,a.salesorderdtl_gid,a.product_gid,b.currency_code,a.product_requireddate as product_requireddate, "+
                    " d.product_name,date_format(c.salesorder_date, '%d-%m-%Y') as salesorder_date, "+
                    " b.customer_gid,b.customer_name,a.qty_quoted,format(a.price, 2) as product_price"+
                    " from smr_trn_tsalesorderdtl a left join smr_trn_tsalesorder b on a.salesorder_gid = b.salesorder_gid"+
                    " left join pmr_mst_tproduct d on a.product_gid = d.product_gid" +
                    " left join acp_trn_torder c on a.salesorder_gid = c.salesorder_gid" +
                    " where a.product_gid = '" + product_gid + "' group by a.price  order by c.salesorder_date desc";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<Directeddetailslist2>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new Directeddetailslist2
                    {
                        product_gid = dt["product_gid"].ToString(),
                        product_name = dt["product_name"].ToString(),
                        salesorder_date = dt["salesorder_date"].ToString(),
                        customer_name = dt["customer_name"].ToString(),
                        qty_quoted = dt["qty_quoted"].ToString(),
                        product_price = dt["product_price"].ToString(),
                        currency_code = dt["currency_code"].ToString(),

                    });
                    values.Directeddetailslist2 = getModuleList;
                }
            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Order Detail !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }


        // PRODUCT SUMMARY UPDATE FOR DIRECT SALES ORDER

        public void DaPostUpdateDirectSOProduct (string employee_gid, DirecteditSalesorderList values)
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

            msSQL = "Select productuom_gid from pmr_mst_tproductuom where productuom_name='" + values.productuom_name + "'";
            string lsproductuomgid = objdbconn.GetExecuteScalar(msSQL);
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
            msSQL = " SELECT a.producttype_name FROM pmr_mst_tproducttype a " +
             " INNER JOIN pmr_mst_tproduct b ON a.producttype_gid=b.producttype_gid  " +
             " WHERE product_gid='" + values.product_gid + "'";
            objMySqlDataReader = objdbconn.GetDataReader(msSQL);
            if (objMySqlDataReader.HasRows == true)
            {
                lsproduct_type = "Sales";
    
           }
            else
            {
                lsproduct_type = "Services";
       }

            msSQL = " select * from smr_tmp_tsalesorderdtl where product_gid='" + values.product_gid + "' and uom_gid='" + lsproductuomgid + "' " +
                    " and product_price='" + values.unitprice + "' and tax1_gid='" + values.tax_name + "'  and employee_gid='" + employee_gid + "' " +
                    " and tmpsalesorderdtl_gid = '" + values.tmpsalesorderdtl_gid + "' ";
            objMySqlDataReader = objdbconn.GetDataReader(msSQL);
            if (objMySqlDataReader.HasRows == true)
            {
                msSQL = " update smr_tmp_tsalesorderdtl set qty_quoted='" + Convert.ToDouble(values.quantity) + Convert.ToDouble(objMySqlDataReader["qty_quoted"].ToString()) + "' " +
                        " price='" + Convert.ToDouble(values.totalamount) + Convert.ToDouble(objMySqlDataReader["price"].ToString()) + "' " +
                        " where tmpsalesorderdtl_gid='" + values.tmpsalesorderdtl_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
               msSQL = " delete from smr_tmp_tsalesorderdtl where tmpsalesorderdtl_gid='" + values.tmpsalesorderdtl_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            else
            {
                msSQL = " update smr_tmp_tsalesorderdtl set " +
                       " product_gid = '" + values.product_gid + "'," +
                       " product_name= '" + values.product_name + "'," +
                       " product_price='" + values.unitprice + "'," +
                       " qty_quoted='" + values.quantity + "'," +
                       " margin_percentage='" + values.discount_percentage + "'," +
                       " margin_amount='" + values.discountamount + "'," +
                       " uom_gid = '" + lsproductuomgid + "', " +
                       " uom_name='" + values.productuom_name + "'," +
                       " price='" + values.totalamount + "'," +
                       " employee_gid='" + employee_gid + "'," +
                       " tax_name= '" + values.tax_name + "'," +
                       " tax1_gid= '" + values.tax_gid + "'," +
                       " tax_amount='" + values.tax_amount + "'," +
                       " order_type='" + lsproduct_type + "', " +
                       " tax_percentage='" + lspercentage1 + "'" +
                       " where tmpsalesorderdtl_gid='" + values.tmpsalesorderdtl_gid + "'";
                       mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }

            if (mnResult != 0)
            {
                values.status = true;
                values.message = " Product Updated Successfully! ";

                    }
            else
            {
                values.status = false;
                values.message = " Error While Updating Product! ";

                    }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating DirectSO Product !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }

        public void DaGetsalesonupdate(string employee_gid, MdlSmrTrnSalesorder values)
        {
            try
            {
               


                msSQL = "  select distinct a.salesorder_gid, so_referenceno1, a.shopify_orderid,a.shopifyorder_number,a.shopifycustomer_id,DATE_FORMAT(a.salesorder_date, '%Y-%m-%d') as salesorder_date," +
                    " a.so_referencenumber,d.customer_gid,d.customer_name,d.customer_address, a.customer_contact_person as customer_contactperson,e.mobile,c.user_firstname, " +
                    " a.so_type, a.currency_code, a.salesorder_status,s.source_name,d.customer_code," +
                    " i.branch_name, a.grandtotal_l ,i.branch_gid, a.currency_code,a.so_type, a.created_by,a.created_date, " +
                    " f.product_gid,f.productgroup_gid,f.product_code, f.productgroup_name,f.product_name,f.product_price,f.qty_quoted," +
                    "f.discount_percentage,f.discount_amount,f.tax_percentage,f.tax_amount,f.uom_gid,f.uom_name,f.tax_name," +
                    "f.tax1_gid,f.display_field,f.selling_price,f.product_price_l,f.customerproduct_code,c.user_gid from smr_trn_tsalesorder a " +
                    " left join crm_mst_tcustomer d on a.customer_gid=d.customer_gid  " +
                    " left join crm_mst_tcustomercontact e on d.customer_gid=e.customer_gid   " +
                    " left join hrm_mst_temployee b on b.employee_gid=a.created_by  " +
                    " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code  " +
                    " left join adm_mst_tuser c on b.user_gid= c.user_gid   " +
                    " left join hrm_mst_tbranch i on a.branch_gid= i.branch_gid" +
                    " left join crm_trn_tleadbank l on l.customer_gid=a.customer_gid" +
                    " left join crm_mst_tsource s on s.source_gid=l.source_gid " +
                    " left join smr_trn_tsalesorderdtl f on f.salesorder_gid=a.salesorder_gid   " +
                    " where a.salesorder_status='paid' group by a.shopify_orderid ";

            DataTable dt_datatable = objdbconn.GetDataTable(msSQL);
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt1 in dt_datatable.Rows)
                {


                    msINGetGID = objcmnfunctions.GetMasterGID("SIVT");

                    msSQL = " insert into rbl_trn_tinvoice(" +
                    " invoice_gid," +
                    " invoice_date," +
                    " invoice_type," +
                    " invoice_status," +
                    " customer_gid," +
                    " customer_name," +
                    " customer_contactperson," +
                    " customer_contactnumber," +
                    " customer_address," +
                    " total_amount," +
                    " user_gid," +
                    " currency_code," +
                    " branch_gid " +
                    " ) values (" +
                    "'" + msINGetGID + "'," +
                    "'" + dt1["salesorder_date"].ToString() + "'," +
                    "'" + dt1["so_type"].ToString() + "'," +
                    " 'Payment Done'," +
                    "'" + dt1["customer_gid"].ToString() + "'," +
                    "'" + dt1["customer_name"].ToString() + "'," +
                    "'" + dt1["customer_contactperson"].ToString() + "'," +
                    "'" + dt1["mobile"].ToString() + "'," +
                    "'" + dt1["customer_address"].ToString() + "'," +
                    "'" + dt1["grandtotal_l"].ToString() + "'," +
                    "'" + dt1["user_gid"].ToString() + "'," +
                    "'" + dt1["currency_code"].ToString() + "'," +
                    "'" + dt1["branch_gid"].ToString() + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 1)
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("SIVC");
                        msSQL = " insert into rbl_trn_tinvoicedtl ( " +
                                " invoicedtl_gid," +
                                " invoice_gid," +
                                " product_gid," +
                                " product_price," +
                                " discount_percentage," +
                                " discount_amount," +
                                " uom_gid," +
                                " tax_amount," +
                                " tax_name," +
                                " display_field," +
                                " tax1_gid," +
                                " product_name," +
                                " product_code," +
                                " customerproduct_code, " +
                                " uom_name," +
                                " productgroup_gid, " +
                                " productgroup_name," +
                                " selling_price," +
                                " product_price_L " +
                                " ) values ( " +
                                "'" + msGetGid + "'," +
                                "'" + msINGetGID + "'," +
                                "'" + dt1["product_gid"].ToString() + "'," +
                                "'" + dt1["product_price"].ToString() + "'," +
                                "'" + dt1["discount_percentage"].ToString() + "'," +
                                "'" + dt1["discount_amount"].ToString() + "'," +
                                "'" + dt1["uom_gid"].ToString() + "'," +
                                "'" + dt1["tax_amount"].ToString() + "'," +
                                "'" + dt1["tax_name"].ToString() + "'," +
                                "'" + dt1["display_field"].ToString() + "'," +
                                "'" + dt1["tax1_gid"].ToString() + "'," +
                                "'" + dt1["product_name"].ToString() + "'," +
                                "'" + dt1["product_code"].ToString() + "'," +
                                "'" + dt1["customerproduct_code"].ToString() + "'," +
                                "'" + dt1["uom_name"].ToString() + "'," +
                                "'" + dt1["productgroup_gid"].ToString() + "'," +
                                "'" + dt1["productgroup_name"].ToString() + "'," +
                                "'" + dt1["selling_price"].ToString() + "'," +
                                "'" + dt1["product_price_L"].ToString() + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    if (mnResult == 1)
                    {
                        msGetinGid = objcmnfunctions.GetMasterGID("BPTP");

                        msSQL = " insert into rbl_trn_tpayment (" +
                            " payment_gid, " +
                            " payment_date," +
                            " invoice_gid," +
                            " total_amount," +
                            " branch," +
                            " created_by," +
                            " currency_code " + ") values (" +
                            "'" + msGetinGid + "'," +
                             "'" + dt1["salesorder_date"].ToString() + "'," +
                             "'" + msINGetGID + "'," +
                            "'" + dt1["grandtotal_l"].ToString() + "'," +
                            "'" + dt1["branch_name"].ToString() + "'," +
                            "'" + dt1["created_by"].ToString() + "'," +
                            "'" + dt1["currency_code"].ToString() + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }
            }
            if (mnResult == 1)
            {
                msSQL = "update smr_trn_tsalesorder set salesorder_status='Payment Done' where salesorder_status='paid' and shopify_orderid";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            }
            if (mnResult != 0)

            {
                values.status = true;
                values.message = "Status Updated Successfully!";

            }
            else
            {
                values.status = false;
                values.message = "Error While Upadating Status!";

            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Sales Order !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           




        }
        public void DaGetupdate(string employee_gid, MdlSmrTrnSalesorder values)
        {
            try
            {
              

                msSQL = "SELECT salesorder_status,shopify_orderid FROM smr_trn_tsalesorder WHERE shopify_orderid and salesorder_status in('Pending')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult != 0)
            {

                msSQL = "update smr_trn_tsalesorder set salesorder_status='Approved' where shopify_orderid ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }

            if (mnResult != 0)
            {
                values.status = true;
                values.message = "Status Updated Successfully!";
            }
            else
            {
                {
                    values.status = false;
                    values.message = "Error While Upadating Status!";
                }
            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Status!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }




    }
}
