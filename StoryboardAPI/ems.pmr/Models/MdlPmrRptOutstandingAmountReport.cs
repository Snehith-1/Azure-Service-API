﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.pmr.Models
{
    public class MdlPmrRptOutstandingAmountReport : result
    {
        public List<Getoutstandingamountreport_list> Getoutstandingamountreport_list { get; set; }
    }
    public class Getoutstandingamountreport_list : result
    {
        public string purchaseorder_gid { get; set; }
        public string invoice_date { get; set; }
        public string invoice_refno { get; set; }
        public string vendor_refnodate { get; set; }
        public string branch_name { get; set; }
        public string companydetails { get; set; }

        public string invoice_amount { get; set; }
        public string payment_amount { get; set; }
        public string outstanding_amount { get; set; }
        public string due_date { get; set; }
        public string pending_days { get; set; }
        public string invoice_status { get; set; }
    }
}