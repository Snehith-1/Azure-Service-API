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

namespace ems.crm.Controllers
{
    [RoutePrefix("api/Instagram")]
    [Authorize]


    public class InstagramController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaInstagram objDaInstagram = new DaInstagram();

        [ActionName("GetInstagram")]
        [HttpGet]

        public HttpResponseMessage GetInstagram()

        {
            instagramlist values = new instagramlist();
            objDaInstagram.DaGetInstagram(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetInstagramProfile")]
        [HttpGet]
        public HttpResponseMessage GetInstagramProfile()
        {
            MdlInstagram values = new MdlInstagram();
            objDaInstagram.DaGetInstagramProfile(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}