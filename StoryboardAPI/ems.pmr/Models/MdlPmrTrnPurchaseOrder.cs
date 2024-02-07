﻿//using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;




namespace ems.pmr.Models
{
    public class MdlPmrTrnPurchaseOrder : result
    {
        public List<GetPurchaseOrder_lists> GetPurchaseOrder_lists { get; set; }
        public List<GetViewPurchaseOrder> GetViewPurchaseOrder { get; set; }
        public List<GetBranch> GetBranch { get; set; }
        public List<templatelist> templatelist { get; set; }
        public List<GetDispatchToBranch> GetDispatchToBranch { get; set; }
        public List<GetCurrency> GetCurrency { get; set; }
        public List<GetTax> GetTax { get; set; }
        public List<GetVendor> GetVendor { get; set; }
        public List<GetProductCode> GetProductCode { get; set; }
        public List<GetProduct> GetProduct { get; set; }
        public List<GetOnchangeCurrency> GetOnchangeCurrency { get; set; }
        public List<productsummary_list> productsummary_list { get; set; }
        public double grand_total { get; set; }
        public double grandtotal { get; set; }
    }
    public class GetPurchaseOrder_lists : result
    {

        public string purchaseorder_gid { get; set; }
        public string purchaseorder_date { get; set; }
        public string ExpectedPODeliveryDate { get; set; }
        public string porefno { get; set; }
        public string branch_name { get; set; }
        public string costcenter_name { get; set; }

        public string vendor_companyname { get; set; }
        public string total_amount { get; set; }
        public string remarks { get; set; }
        public string purchaseorder_status { get; set; }
        public string vendor_status { get; set; }
        public string paymentamount { get; set; }
        public string currency_code { get; set; }

    }
    public class GetBranch
    {
        public string branch_gid { get; set; }
        public string branch_name { get; set; }
        public string address1 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postal_code { get; set; }
    }
    public class GetVendor
    {
        public string vendor_gid { get; set; }
        public string vendor_companyname { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postal_code { get; set; }
        public string currencyexchange_gid { get; set; }
        public string fax { get; set; }
        public string contactperson_name { get; set; }
        public string country_name { get; set; }
        public string email_id { get; set; }
        public string contact_telephonenumber { get; set; }

    }
    public class GetDispatchToBranch
    {
        public string branch_gid { get; set; }
        public string dispatch_name { get; set; }

    }
    public class GetCurrency
    {
        public string currency_code { get; set; }
        public string currencyexchange_gid { get; set; }

    }
    public class GetProduct
    {
        public string product_gid { get; set; }
        public string product_name { get; set; }

    }

    public class GetOnchangeCurrency
    {

        public string exchange_rate { get; set; }
        public string currency_code { get; set; }

    }
    public class GetTax
    {
        public string tax_gid { get; set; }
        public string tax_name { get; set; }
        public string tax_name1 { get; set; }
        public string tax_name2 { get; set; }
        public string tax_name3 { get; set; }
        public string percentage { get; set; }



    }
    public class GetProductCode
    {
        public string product_gid { get; set; }
        public string product_code { get; set; }
        public string productgroup_name { get; set; }
        public string productuom_name { get; set; }
        public string productuom_gid { get; set; }
        public string productgroup_gid { get; set; }
        public string product_name { get; set; }
        public string unitprice { get; set; }

    }
    public class GetViewPurchaseOrder : result
    {
        public string vendor_fax { get; set; }
        public string email_address { get; set; }
        public string productuom_gid { get; set; }
        public string overalltax { get; set; }
        public string additional_discount { get; set; }
        public string tax_percentage { get; set; }
        public string tax_name { get; set; }
        public string product_total { get; set; }
        public string overall_tax { get; set; }
        public string  po_date { get; set; }
        public string discountamount { get; set; }
        public string tax_amount4 { get; set; }

        public string dispatch_name { get; set; }
        public string grandtotal { get; set; }

        public string Shipping_address { get; set; }

        public string contact_person { get; set; }

        public string pocovernote_address { get; set; }

        public string totalamount { get; set; }

        public string template_content { get; set; }
        public string contact_number { get; set; }
        public string remarks { get; set; }

        public string purchaseorder_gid { get; set; }
        public string purchaseorder_date { get; set; }
        public string manualporef_no { get; set; }
        public string deliverytobranch { get; set; }
        public string branch_name { get; set; }
        public string vendor_contactnumber { get; set; }
        public string vendor_contact_person { get; set; }
        public string vendor_faxnumber { get; set; }
        public string vendor_companyname { get; set; }
        public string vendor_emailid { get; set; }
        public string vendor_address { get; set; }
        public string exchange_rate { get; set; }
        public string po_no { get; set; }

        public string ship_via { get; set; }
        public string addoncharge { get; set; }

        public string payment_terms { get; set; }
        public string freight_terms { get; set; }
        public string delivery_location { get; set; }
        public string currency_code { get; set; }
        public string shipping_address { get; set; }
        public string purchaseorder_reference { get; set; }
        public string purchaseorder_remarks { get; set; }
        public string discount_percentage { get; set; }
        public string tax_gid { get; set; }
        public string packing_charges { get; set; }
        public string insurance_charges { get; set; }
        public string payment_days { get; set; }
        public string termsandconditions { get; set; }
        public string buybackorscrap { get; set; }
        public string roundoff { get; set; }
        public string discount_amount { get; set; }
        public string total_amount { get; set; }
        public string freighttax_amount { get; set; }
        public string tax_amount { get; set; }
        public string approver_remarks { get; set; }

        public string delivery_days { get; set; }

        public string productgroup_name { get; set; }

        public string product_code { get; set; }

        public string product_name { get; set; }

        public string productuom_name { get; set; }

        public string product_price { get; set; }

        public string priority_n { get; set; }

        public string qty_ordered { get; set; }
        public string qty_Received { get; set; }

        public string qty_grnadjusted { get; set; }
        public string priority_remarks { get; set; }

        public string tax_amount2 { get; set; }

        public string tax_amount3 { get; set; }

        public string addon_amount { get; set; }
        public string product_gid { get; set; }

    }
    public class productlist : result
    {
        public string tax_gid3 { get; set; }
        public string tax_gid { get; set; }

        public string quantity { get; set; }
        public string productname { get; set; }
        public string productuom { get; set; }
        public string product_gid { get; set; }
        public string tax_name1 { get; set; }
        public string totalamount { get; set; }
        public string tax_name2 { get; set; }
        public string unitprice { get; set; }
        public string discountpercentage { get; set; }
        public string discountamount { get; set; }
        public string taxamount1 { get; set; }
        public string taxamount2 { get; set; }
        public string taxamount3 { get; set; }

        public string tax_name3 { get; set; }
        public string totalamount1 { get; set; }
        public string productuom_gid { get; set; }
        public string productuom_name { get; set; }
        public string created_by { get; set; }
        public string taxpercentage3 { get; set; }

        public string needby_date { get; set; }

        public string taxname1 { get; set; }
        public string taxname2 { get; set; }
        public string productdescription { get; set; }
        public string tax_gid1 { get; set; }
        public string tax_gid2 { get; set; }
        public string product_name { get; set; }
        public string productgroup_gid { get; set; }
        public string productgroup { get; set; }
        public string costprice { get; set; }
        public string productcode { get; set; }
        public string customerproductcode { get; set; }
        public string taxpercentage1 { get; set; }
        public string taxpercentage2 { get; set; }
        public string productgid { get; set; }

        public string vendorprice { get; set; }
    }
    public class productsummary_list : result
    {
        public string productuom_name { get; set; }
        public string taxamount1 { get; set; }
        public string unitprice { get; set; }

        public string product_name { get; set; }
        public string tmppurchaseorderdtl_gid { get; set; }
        public string discount_percentage { get; set; }

        public string tax { get; set; }
        public string totalamount { get; set; }
        public string hsn { get; set; }
        public string product_code { get; set; }
        public string productgroup_name { get; set; }
        public string customerproduct_code { get; set; }
        public string uom_name { get; set; }
        public string qty { get; set; }
        public string product_price { get; set; }
        public string discount_amount { get; set; }

        public string product_total { get; set; }
        public string tax_name1 { get; set; }
        public string tax_name2 { get; set; }

        public string tax_name3 { get; set; }
        public string tax_percentage3 { get; set; }
        public string tax_percentage2 { get; set; }
        public string tax_percentage { get; set; }
        public string purchaseorder_gid { get; set; }
        public string product_gid { get; set; }
    }
    public class templatelist : result
    {
        public string template_gid { get; set; }
        public string template_name { get; set; }
        public string template_content { get; set; }
    }
    public class mailsend_list : result
    {
        public result results { get; set; }
        public string AutoID_Key { get; set; }
    }
    public class MailsendAttachmentbase
    {
        public string name { get; set; }
        public string type { get; set; }
        public string data { get; set; }
    }
    public class DbsendAttachmentPath
    {
        public string path { get; set; }
    }
    public class attachment : result
    {
        public string filename { get; set; }
        public string typedata { get; set; }
    }
    public class file : result
    {
        public string File { get; set; }
    }


}