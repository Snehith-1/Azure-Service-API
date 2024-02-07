﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ems.crm.DataAccess;
using ems.crm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System.IO;

namespace ems.crm.Controllers
{
    [Authorize]
    [RoutePrefix("api/SmsCampaign")]
    public class SmsCampaignController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmsCampaign objDasmscmapaign = new DaSmsCampaign();
        // code By snehith
        [ActionName("GetSmsCampaignCount")]
        [HttpGet]
        public HttpResponseMessage GetSmsCampaignCount()
        {
            MdlSmsCampaign values = new MdlSmsCampaign();
            objDasmscmapaign.DaGetSmsCampaignCount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        // code By snehith
        [ActionName("GetSmsCampaign")]
        [HttpGet]
        public HttpResponseMessage GetSmsCampaign()
        {
            MdlSmsCampaign values = new MdlSmsCampaign();
            objDasmscmapaign.DaGetSmsCampaign(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostSmsCampaign")]
        [HttpPost]
        public HttpResponseMessage PostSmsCampaign(smspostcampaign_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDasmscmapaign.DaPostSmsCampaign(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("UpdateSmsCampaign")]
        [HttpPost]
        public HttpResponseMessage UpdateSmsCampaign(smspostcampaign_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDasmscmapaign.DaUpdateSmsCampaign(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("DeleteSmsCampaign")]
        [HttpGet]
        public HttpResponseMessage DeleteSmsCampaign(string id)
        {
            smspostcampaign_list objresult = new smspostcampaign_list();
            objDasmscmapaign.DaDeleteSmsCampaign(id, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("SmsLeadCustomerDetails")]
        [HttpGet]
        public HttpResponseMessage SmsLeadCustomerDetails()
        {
            MdlSmsCampaign values = new MdlSmsCampaign();
            objDasmscmapaign.DaSmsLeadCustomerDetails(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Smssendtolead")]
        [HttpPost]
        public HttpResponseMessage Smssendtolead(smssendtolead values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDasmscmapaign.DaSmssendtolead(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }
    }
}