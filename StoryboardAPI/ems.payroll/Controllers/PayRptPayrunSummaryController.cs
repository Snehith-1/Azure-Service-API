﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ems.payroll.DataAccess;
using ems.payroll.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
namespace ems.payroll.Controllers

{
    [Authorize]
    [RoutePrefix("api/PayRptPayrunSummary")]

    public class PayRptPayrunSummaryController : ApiController
    {

        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaRptPayrunSummary objDaRptPayrunSummary = new DaRptPayrunSummary();


        [ActionName("GetPayrunSummary")]
        [HttpGet]
        public HttpResponseMessage GetPayrunSummary(string branch_gid, string department_gid, string year, string month)
        {
            MdlRptPayrunSummary values = new MdlRptPayrunSummary();
            objDaRptPayrunSummary.DaGetPayrunSummary(branch_gid, department_gid, month, year, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetBranchDtl")]
        [HttpGet]
        public HttpResponseMessage GetBranchDtl()
        {
            MdlRptPayrunSummary values = new MdlRptPayrunSummary();
            objDaRptPayrunSummary.DaGetBranchDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetDepartmentDtl")]
        [HttpGet]
        public HttpResponseMessage GetDepartmentDtl()
        {
            MdlRptPayrunSummary values = new MdlRptPayrunSummary();
            objDaRptPayrunSummary.DaGetDepartmentDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }



        [ActionName("additionalsummary")]
        [HttpGet]
        public HttpResponseMessage additionalsummary(string salary_gid)
        {
            MdlRptPayrunSummary values = new MdlRptPayrunSummary();
            objDaRptPayrunSummary.Daadditionalsummary(salary_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("deductsummary")]
        [HttpGet]
        public HttpResponseMessage deductsummary(string salary_gid)
        {
            MdlRptPayrunSummary values = new MdlRptPayrunSummary();
            objDaRptPayrunSummary.Dadeductsummary(salary_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("othersummary")]
        [HttpGet]
        public HttpResponseMessage othersummary(string salary_gid)
        {
            MdlRptPayrunSummary values = new MdlRptPayrunSummary();
            objDaRptPayrunSummary.Daothersummary(salary_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}