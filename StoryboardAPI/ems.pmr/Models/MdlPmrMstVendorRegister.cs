﻿using StoryboardAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.pmr.Models
{
    public class MdlPmrMstVendorRegister : result
    {
        public List<editvendorregistersummary_list> editvendorregistersummary_list { get; set; }
        public List<GetDocument_list> GetDocument_list { get; set; }
        public List<Getvendor_lists> Getvendor_lists { get; set; }
        public List<vendor_listaddinfo> vendor_listaddinfo { get; set; }
        public List<vendorexport_list> vendorexport_list { get; set; }
        public List<Getcountry> Getcountry { get; set; }
        public List<Getcurency> Getcurency { get; set; }
        public List<Gettax> Gettax { get; set; }
        public List<vendor_list> vendor_list { get; set; }
        public List<GetDocumentType> GetDocumentType { get; set; }
        public List<ActiveStatus_list> ActiveStatus_list { get; set; }

    }

    public class Getvendor_lists : result
    {
        public string vendorregister_gid { get; set; }
        public string vendor_code { get; set; }
        public string vendor_companyname { get; set; }
        public string contactperson_name { get; set; }
        public string contact_telephonenumber { get; set; }
        public string vendor_status { get; set; }
        public string active_flag { get; set; }

    }
    public class Getcountry : result
    {
        public string country_name { get; set; }
        public string country_gid { get; set; }

    }
    public class GetDocumentType : result
    {
        public string documenttype_gid { get; set; }
        public string documenttype_name { get; set; }





    }
    public class Getcurency : result
    {
        public string currency_code { get; set; }
        public string currencyexchange_gid { get; set; }

    }
    public class Gettax : result
    {
        public string tax_name { get; set; }
        public string tax_gid { get; set; }

    }
    public class vendor_list : result
    {
        public string vendor_code { get; set; }
        public string vendor_companyname { get; set; }
        public string contactperson_name { get; set; }
        public string contact_telephonenumber { get; set; }
        public string city { get; set; }
        public string state_name { get; set; }
        public string postal_code { get; set; }
        public string currencyname { get; set; }
        public string countryname { get; set; }
        public string taxname { get; set; }
        public string email_address { get; set; }
        public string tin_number { get; set; }
        public string excise_details { get; set; }
        public string pan_number { get; set; }
        public string servicetax_number { get; set; }
        public string cst_number { get; set; }
        public string bank_details { get; set; }
        public string ifsc_code { get; set; }
        public string rtgs_code { get; set; }
        public string fax_name { get; set; }
        public string address { get; set; }
        public string address2 { get; set; }

        public getmobile mobile { get; set; }
    }

    public class getmobile
    {
        public string e164Number { get; set; }

    }

    public class editvendorregistersummary_list : result
    {

        public string vendorregister_gid { get; set; }
        public string vendor_code { get; set; }
        public string vendor_companyname { get; set; }
        public string contactperson_name { get; set; }
        public string contact_telephonenumber { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postal_code { get; set; }
        public string currency_code { get; set; }
        public string country_name { get; set; }
        public string fax { get; set; }
        public string tax_name { get; set; }
        public string vendor_status { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string email_id { get; set; }
        public string tin_number { get; set; }
        public string excise_details { get; set; }
        public string pan_number { get; set; }
        public string servicetax_number { get; set; }
        public string cst_number { get; set; }
        public string bank_details { get; set; }
        public string ifsc_code { get; set; }
        public string rtgs_code { get; set; }
        public string address_gid { get; set; }
        public string tax_gid { get; set; }
        public string currencyexchange_gid { get; set; }
        public string country_gid { get; set; }

        public string address1 { get; set; }
        public string address2 { get; set; }
        public string pincode { get; set; }
        public string country { get; set; }
        public string blacklist_date { get; set; }
        public string blacklist_flag { get; set; }
        public string blacklist_remarks { get; set; }

        public string blacklist_by { get; set; }

        public string document_gid { get; set; }
        public string document_type { get; set; }
        public string document_name { get; set; }
        public string user_firstname { get; set; }
        public string documenttype_name { get; set; }

        public string file_path { get; set; }
        public byte[] file_data { get; set; }

    }
    public class vendor_listaddinfo : result
    {
        public string vendorregister_gid { get; set; }
        public string vendor_code { get; set; }
        public string vendor_companyname { get; set; }
        public string contactperson_name { get; set; }
        public string contact_telephonenumber { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postal_code { get; set; }
        public string currency_code { get; set; }
        public string country_name { get; set; }
        public string tax_name { get; set; }
        public string vendor_status { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string email_id { get; set; }
        public string tin_number { get; set; }
        public string excise_details { get; set; }
        public string pan_number { get; set; }
        public string servicetax_number { get; set; }
        public string cst_number { get; set; }
        public string bank_details { get; set; }
        public string ifsc_code { get; set; }
        public string rtgs_code { get; set; }
        public string address_gid { get; set; }
        public string tax_gid { get; set; }
        public string currencyexchange_gid { get; set; }
        public string country_gid { get; set; }
        public string fax { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string pincode { get; set; }
        public string country { get; set; }
        public string blacklist_date { get; set; }
        public string blacklist_flag { get; set; }
        public string blacklist_remarks { get; set; }
        public string blacklist_by { get; set; }

        public string document_gid { get; set; }
        public string document_type { get; set; }
        public string document_name { get; set; }
        public string user_firstname { get; set; }
        public string file_path { get; set; }
        public byte[] file_data { get; set; }
        public string documenttype_name { get; set; }
    }

    public class ActiveStatus_list : result
    {
        public string active_flag { get; set; }
        public string vendorregister_gid { get; set; }
        public string product_desc { get; set; }
    }

    public class vendorexport_list : result
    {
        public string lspath1 { get; set; }
        public string lsname2 { get; set; }
    }
    public class GetDocument_list : result
    {
        public string user_firstname { get; set; }
        public string vendorregister_gid { get; set; }
        public string document_name { get; set; }
        public string document_type { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string document_gid { get; set; }
        public string file_path{ get; set; }
    }

}
