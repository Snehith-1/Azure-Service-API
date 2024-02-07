﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ems.sales.DataAccess;
using ems.sales.Models;
using ems.utilities.Functions;
using ems.utilities.Models;

namespace ems.sales.Controllers
{
    [Authorize]
    [RoutePrefix("api/SmrRptOrderReport")]
    public class SmrRptOrderReportController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmrRptOrderReport objDaSmrRptOrderReport = new DaSmrRptOrderReport();

        // GetOrderForLastSixMonths

        [ActionName("GetOrderForLastSixMonths")]
        [HttpGet]
        public HttpResponseMessage GetOrderForLastSixMonths()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrRptOrderReport values = new MdlSmrRptOrderReport();
            objDaSmrRptOrderReport.DaGetOrderForLastSixMonths(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOrderReportForLastSixMonthsSearch")]
        [HttpGet]
        public HttpResponseMessage GetOrderReportForLastSixMonthsSearch(string from_date, string to_date)
        {
            //string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            //getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrRptOrderReport values = new MdlSmrRptOrderReport();
            objDaSmrRptOrderReport.DaGetOrderForLastSixMonthsSearch(values, from_date, to_date);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // GetOrderSummary

        [ActionName("GetOrderSummary")]
        [HttpGet]
        public HttpResponseMessage GetOrderSummary(string salesorder_gid )
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrRptOrderReport values = new MdlSmrRptOrderReport();
            objDaSmrRptOrderReport.DaGetOrderSummary(getsessionvalues.employee_gid, salesorder_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // GetOrderDetailSummary

        [ActionName("GetOrderDetailSummary")]
        [HttpGet]
        public HttpResponseMessage GetOrderDetailSummary(string month,string year)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrRptOrderReport values = new MdlSmrRptOrderReport();
            objDaSmrRptOrderReport.DaGetOrderDetailSummary(getsessionvalues.employee_gid,month, year, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // GetMonthwiseOrderReport

        [ActionName("GetMonthwiseOrderReport")]
        [HttpGet]
        public HttpResponseMessage GetMonthwiseOrderReport()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrRptOrderReport values = new MdlSmrRptOrderReport();
            objDaSmrRptOrderReport.DaGetMonthwiseOrderReport(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // GetOrderwiseOrderReport

        [ActionName("GetOrderwiseOrderReport")]
        [HttpGet]
        public HttpResponseMessage GetOrderwiseOrderReport()
       {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrRptOrderReport values = new MdlSmrRptOrderReport();
            objDaSmrRptOrderReport.DaGetOrderWiseOrderReport(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
         }
    }
}