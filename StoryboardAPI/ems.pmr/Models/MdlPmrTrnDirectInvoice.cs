﻿using System;
using System.Collections.Generic;

namespace ems.pmr.Models
{
    public class MdlPmrTrnDirectInvoice :result
    {
        public List<GetBranchnamedropdown> GetBranchnamedropdown { get; set; }
        public List<GetVendornamedropdown> GetVendornamedropdown { get; set; }
        public List<GetOnChangeVendor> GetOnChangeVendor { get; set; }
        public List<Getcurrencycodedropdown> Getcurrencycodedropdown { get; set; }
        public List<GetPurchaseTypedropDown> GetPurchaseTypedropDown { get; set; }
        public List<Gettaxnamedropdown> Gettaxnamedropdown { get; set; }
        public List<GetExtraAddondropDown> GetExtraAddondropDown { get; set; }
        public List<GetExtraDeductiondropDown> GetExtraDeductiondropDown { get; set; }
        public List<directsalesinvoicelist> directsalesinvoicelist { get; set; }
    }
    public class GetBranchnamedropdown : result
    {
        public string branch_name { get; set; }
        public string branch_gid { get; set; }
    }
    public class GetVendornamedropdown : result
    {
        public string vendorgid { get; set; }
        public string vendorcompanyname { get; set; }
    }
    public class GetOnChangeVendor : result
    {
        public string address { get; set; }
        public string vendorcontact { get; set; }
        public string phone { get; set; }
    }
    public class Getcurrencycodedropdown : result
    {
        public string currency_code { get; set; }
        public string currencyexchange_gid { get; set; }
    }
    public class GetPurchaseTypedropDown : result
    {
        public string account_gid { get; set; }
        public string purchasetype_name { get; set; }
    }
    public class Gettaxnamedropdown : result
    {
        public string tax_name { get; set; }
        public string tax_gid { get; set; }
        public string tax_percentage { get; set; }
    }
    public class GetExtraAddondropDown : result
    {
        public string additional_gid { get; set; }
        public string additional_name { get; set; }
    }
    public class GetExtraDeductiondropDown : result
    {
        public string discount_gid { get; set; }
        public string discount_name { get; set; }
    }
    public class directsalesinvoicelist : result
    {
        public string direct_invoice_ven_name { get; set; }
        public string direct_invoice_refno { get; set; }
        public DateTime direct_invoice_date { get; set; }
        public DateTime direct_invoice_due_date { get; set; }
        public string vendor_gid { get; set; }
        public string direct_invoice_addon_amount { get; set; }
        public string direct_invoice_discount_amount { get; set; }
        public string direct_invoice_grand_total { get; set; }
        public double direct_invoice_amount { get; set; }
        public string direct_invoice_payterm { get; set; }
        public string direct_invoice_remarks { get; set; }
        public string direct_invoice_currencycode { get; set; }
        public string direct_invoice_exchange_rate { get; set; }
        public string direct_invoice_freight_charges { get; set; }
        public string direct_invoice_extra_addon { get; set; }
        public string direct_invoice_extra_deduction { get; set; }
        public string direct_invoice_buyback_scrap_charges { get; set; }
        public string direct_invoice_ven_ref_no { get; set; }
        public string direct_invoice_branchgid { get; set; }
        public string direct_invoice_round_off { get; set; }
        public string direct_invoice_taxname1 { get; set; }
        public string direct_invoice_taxname2 { get; set; }
        public string tax_gid1 { get; set; }
        public string tax_gid2 { get; set; }
        public string direct_invoice_ven_address { get; set; }
        public string direct_invoice_ven_contact_person { get; set; }
        public string direct_invoice_description { get; set; }
        public string vendor_contact_person { get; set; }
        public string direct_invoice_type { get; set; }
    }
}