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
    [RoutePrefix("api/Whatsapp")]
    public class WhatsappController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaWhatsapp objDaWhatsapp = new DaWhatsapp();

        [ActionName("CreateContact")]
        [HttpPost]
        public HttpResponseMessage CreateContact(mdlCreateContactInput values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            result objResult = new result();
            objResult = objDaWhatsapp.dacreatecontact(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        [ActionName("UpdateContact")]
        [HttpPost]
        public HttpResponseMessage UpdateContact(mdlUpdateContactInput values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            result objResult = new result();
            objResult = objDaWhatsapp.daUpdateContact(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        [ActionName("CreateProject")]
        [HttpPost]
        public HttpResponseMessage CreateProject(mdlCreateTemplateInput values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            result objResult = new result();
            objResult = objDaWhatsapp.daCreateProject(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        [ActionName("Createtexttemplate")]
        [HttpPost]
        public HttpResponseMessage Createtexttemplate(mdlCreateTemplateInput1 values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            result objResult = new result();
            objResult = objDaWhatsapp.daCreatetexttemplate(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        [ActionName("CreateTemplate")]
        [HttpPost]
        public HttpResponseMessage CreateTemplate()
        {
            HttpRequest httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objResult = objDaWhatsapp.daCreateTemplate(httpRequest);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        [ActionName("PublishTemplate")]
        [HttpPost]
        public HttpResponseMessage PublishTemplate(mdlpublishtemplate values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            result objResult = new result();
            objResult = objDaWhatsapp.daPublishTemplate(values);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        [ActionName("WhatsappSend")]
        [HttpPost]
        public HttpResponseMessage WhatsappSend(sendmessage values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            result objResult = new result();
            objResult=objDaWhatsapp.DaWhatsappSend(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }


        [ActionName("GetContact")]
        [HttpGet]
        public HttpResponseMessage GetContact()
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetContact(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetMessage")]
        [HttpGet]
        public HttpResponseMessage GetMessage(string whatsapp_gid)
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetMessage(values, whatsapp_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetTemplate")]
        [HttpGet]
        public HttpResponseMessage GetTemplate()
        {
            result values = new result();
            values = objDaWhatsapp.DaGetTemplate();
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetMessageTemplatesummary")]
        [HttpGet]
        public HttpResponseMessage GetMessageTemplatesummary()
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetMessageTemplatesummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetMessageTemplateview")]
        [HttpGet]
        public HttpResponseMessage GetMessageTemplateview(string project_id)
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetMessageTemplateview(values, project_id);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getcampaign")]
        [HttpGet]
        public HttpResponseMessage Getcampaign()
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetcampaign(values);
            
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("DeleteCampaign")]
        [HttpGet]
        public HttpResponseMessage DeleteCampaign(string project_id)
        {
            whatsappCampaign objresult = new whatsappCampaign();
            objDaWhatsapp.DaDeleteCampaign(project_id, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("Getlog")]
        [HttpGet]
        public HttpResponseMessage Getlog(string project_id)
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetlog(values, project_id);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetTemplatepreview")]
        [HttpGet]
        public HttpResponseMessage GetTemplatepreview(string project_id)
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetTemplatepreview(values, project_id);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetContactCount")]
        [HttpGet]
        public HttpResponseMessage GetContactCount()
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetContactCount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetWhatsappMessageCount")]
        [HttpGet]
        public HttpResponseMessage GetWhatsappMessageCount()
        {
            MdlWhatsapp values = new MdlWhatsapp();
            objDaWhatsapp.DaGetWhatsappMessageCount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("bulkMessageSend")]
        [HttpPost]
        public HttpResponseMessage bulkMessageSend(mdlBulkMessageList values)
        {
            result objresult = new result();
            objresult = objDaWhatsapp.dabulkMessageSend(values);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        //[ActionName("bulkcustomizeMessageSend")]
        //[HttpPost]
        //public HttpResponseMessage bulkcustomizeMessageSend(mdlBulkMessageList values)
        //{
        //    result objresult = new result();
        //    objresult = objDaWhatsapp.dabulkcustomizeMessageSend(values);
        //    return Request.CreateResponse(HttpStatusCode.OK, objresult);
        //}

        [ActionName("waNotifications")]
        [HttpGet]
        public HttpResponseMessage waNotifications()
        {
            notifications objNotifications = new notifications();
            objNotifications = objDaWhatsapp.daNotifications();
            return Request.CreateResponse(HttpStatusCode.OK, objNotifications);
        }

        [ActionName("waSendDocuments")]
        [HttpPost]
        public HttpResponseMessage waSendDocuments()
        {
            HttpRequest httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objResult= objDaWhatsapp.daSendDocuments(httpRequest);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }

        [ActionName("waGetDocuments")]
        [HttpGet]
        public HttpResponseMessage waGetDocuments(string contact_id)
        {
            MdlWaFiles obj = new MdlWaFiles();
            obj = objDaWhatsapp.daGetDocuments(contact_id);
            return Request.CreateResponse(HttpStatusCode.OK, obj);
        }
    }
}