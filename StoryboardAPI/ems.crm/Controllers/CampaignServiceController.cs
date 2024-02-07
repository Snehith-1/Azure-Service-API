﻿using ems.crm.DataAccess;
using ems.crm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;


namespace ems.crm.Controllers
{
    [Authorize]
    [RoutePrefix("api/CampaignService")]
    public class CampaignServiceController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaCampaignService objdacampaignservice = new DaCampaignService();

        [ActionName("GetWhatsappSummary")]
        [HttpGet]
        public HttpResponseMessage GetWhatsappSummary()
        {
            MdlCampaignService values = new MdlCampaignService();
            objdacampaignservice.DaGetWhatsappSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateWhatsappService")]
        [HttpPost]
        public HttpResponseMessage UpdateWhatsappService(campaignservice_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.DaUpdateWhatsappService(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetShopifySummary")]
        [HttpGet]
        public HttpResponseMessage GetShopifySummary()
        {
            MdlCampaignService values = new MdlCampaignService();
            objdacampaignservice.DaGetShopifySummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetMailSummary")]
        [HttpGet]
        public HttpResponseMessage GetMailSummary()
        {
            MdlCampaignService values = new MdlCampaignService();
            objdacampaignservice.DaGetMailSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateShopifyService")]
        [HttpPost]
        public HttpResponseMessage UpdateShopifyService(shopifyservcie_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.DaUpdateShopifyService(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateEmailService")]
        [HttpPost]
        public HttpResponseMessage UpdateEmailService(emailservice_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.DaUpdateEmailService(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetFacebookServiceSummary")]
        [HttpGet]
        public HttpResponseMessage GetFacebookServiceSummary()
        {
            MdlCampaignService values = new MdlCampaignService();
            objdacampaignservice.DaGetFacebookServiceSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateFacebookService")]
        [HttpPost]
        public HttpResponseMessage UpdateFacebookService(facebookcampaignservice_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.DaUpdateFacebookService(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetLinkedinServiceSummary")]
        [HttpGet]
        public HttpResponseMessage GetLinkedinServiceSummary()
        {
            MdlCampaignService values = new MdlCampaignService();
            objdacampaignservice.DaGetLinkedinServiceSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateLinkedinService")]
        [HttpPost]
        public HttpResponseMessage UpdateLinkedinService(linkedincampaignservice_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.DaUpdateLinkedinService(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetTelegramServiceSummary")]
        [HttpGet]
        public HttpResponseMessage GetTelegramServiceSummary()
        {
            MdlCampaignService values = new MdlCampaignService();
            objdacampaignservice.DaGetTelegramServiceSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateTelegramService")]
        [HttpPost]
        public HttpResponseMessage UpdateTelegramService(telegramcampaignservice_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.DaUpdateTelegramService(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetInstagramServiceSummary")]
        [HttpGet]
        public HttpResponseMessage GetInstagramServiceSummary()
        {
            MdlCampaignService values = new MdlCampaignService();
            objdacampaignservice.DaGetInstagramServiceSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateInstagramService")]
        [HttpPost]
        public HttpResponseMessage UpdateInstagramService(instagramcampaignservice_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdacampaignservice.DaUpdateInstagramService(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}