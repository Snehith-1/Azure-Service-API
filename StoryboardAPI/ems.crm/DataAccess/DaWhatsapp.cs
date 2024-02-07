﻿using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.crm.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using System.Configuration;
using System.IO;
using static OfficeOpenXml.ExcelErrorValue;
using System.Web.Http.Results;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using MySql.Data.MySqlClient;


namespace ems.crm.DataAccess
{
    public class DaWhatsapp
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        MySqlDataReader objMySqlDataReader;
        DataTable dt_datatable;
        string lscustomer_gid, msGetGid, msGetGid1, msGetGid2, lssource_gid, final_path, lscompany_code;
        int mnResult, mnResult3, mnResult1;

        public result dacreatecontact(mdlCreateContactInput values, string user_gid)
        {
            result objresult = new result();
            try
            {
                int i = 0;
                whatsappconfiguration getwhatsappcredentials = whatsappcredentials();
                Rootobject objRootobject = new Rootobject();
                string contactjson = "{\"displayName\":\"" + values.displayName + "\",\"identifiers\":[{\"key\":\"phonenumber\",\"value\":\"" + values.phone.e164Number + "\"}],\"firstName\":\"" + values.firstName + "\",\"gender\":\"" + values.gender + "\",\"lastName\":\"" + values.lastName + "\"}";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                var request = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/contacts", Method.POST);
                request.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                var responseoutput = response.Content;
                objRootobject = JsonConvert.DeserializeObject<Rootobject>(responseoutput);
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    msSQL = "insert into crm_smm_whatsapp(id,wkey,wvalue,displayName,firstName,lastName,gender,created_date,created_by)values(" +
                            "'" + objRootobject.id + "'," +
                            "'" + values.key + "'," +
                            "'" + values.phone.e164Number + "'," +
                            "'" + values.displayName + "'," +
                            "'" + values.firstName + "'," +
                            "'" + values.lastName + "'," +
                            "'" + values.gender + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                            "'" + user_gid + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {

                        msSQL = "select source_gid from crm_mst_tsource where source_name = 'Whatsapp'";
                        string source_gid = objdbconn.GetExecuteScalar(msSQL);

                        if (string.IsNullOrEmpty(source_gid))
                        {
                            msGetGid = objcmnfunctions.GetMasterGID("BSEM");
                            msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='BSEM' order by finyear desc limit 0,1 ";
                            string lsCode = objdbconn.GetExecuteScalar(msSQL);
                            string lssource_code = "SCM" + "000" + lsCode;

                            msSQL = " insert into crm_mst_tsource(" +
                                    " source_gid," +
                                    " source_code," +
                                    " source_name," +
                                    " created_by, " +
                                    " created_date)" +
                                    " values(" +
                                    " '" + msGetGid + "'," +
                                    " '" + lssource_code + "'," +
                                    "'Whatsapp'," +
                                    "'" + user_gid + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult == 1)
                            {
                                lssource_gid = msGetGid;
                            }
                        }
                        else
                        {
                            lssource_gid = source_gid;
                        }

                        msSQL = " select employee_gid from hrm_mst_temployee where user_gid='" + user_gid + "'";
                        string lsemployee_gid = objdbconn.GetExecuteScalar(msSQL);

                        msGetGid = objcmnfunctions.GetMasterGID("BMCC");
                        msGetGid1 = objcmnfunctions.GetMasterGID("BLBP");

                        msSQL = " INSERT INTO crm_trn_tleadbank(" +
                                " leadbank_gid," +
                                " source_gid," +
                                " leadbank_id," +
                                " leadbank_name," +
                                " status," +
                                " approval_flag, " +
                                " lead_status," +
                                " leadbank_code," +
                                " customer_type," +
                                " created_by," +
                                " main_branch," +
                                " created_date)" +
                                " values(" +
                                " '" + msGetGid1 + "'," +
                                " '" + lssource_gid + "'," +
                                " '" + msGetGid + "'," +
                                " '" + values.displayName + "'," +
                                " 'y'," +
                                " 'Approved'," +
                                " 'Not Assigned'," +
                                " 'H.Q'," +
                                " '" + values.customer_type + "'," +
                                " '" + lsemployee_gid + "'," +
                                " 'Y'," +
                                " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msGetGid2 = objcmnfunctions.GetMasterGID("BLBP");
                        if (msGetGid2 == "E")
                        {
                            objresult.status = false;
                            objresult.message = "Create sequence code BLCC for Lead Bank";
                        }
                        msSQL = " INSERT INTO crm_trn_tleadbankcontact" +
                            " (leadbankcontact_gid," +
                            " leadbank_gid," +
                            " leadbankcontact_name," +
                            " mobile," +
                            " created_date," +
                            " created_by," +
                            " leadbankbranch_name, " +
                            " main_contact)" +
                            " values( " +
                            " '" + msGetGid2 + "'," +
                            " '" + msGetGid1 + "'," +
                            " '" + values.displayName + "'," +
                            " '" + values.phone.e164Number + "'," +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                            " '" + lsemployee_gid + "'," +
                            " 'H.Q'," +
                            " 'y'" + ")";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                        msSQL = "update crm_smm_whatsapp set leadbank_gid='" + msGetGid1 + "' where id='" + objRootobject.id + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = "update crm_trn_tleadbank set wh_id='" + objRootobject.id + "' where leadbank_gid='" + msGetGid1 + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                    }

                    if (mnResult == 1)
                    {
                        objresult.status = true;
                        objresult.message = "Contact created successfully!";
                    }
                    else
                    {
                        objresult.message = "Error occured while adding contact!";
                    }
                }
                else
                {
                    objresult.status = false;
                    objresult.message = "Error occured while posting contact!!";
                }
            }
            catch (Exception ex)
            {
                objresult.message = "Error occured while posting contact!!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
          
            return objresult;
        }
        public result daUpdateContact(mdlUpdateContactInput values, string user_gid)
        {
            result objresult = new result();
            try
            {

                int i = 0;
                whatsappconfiguration getwhatsappcredentials = whatsappcredentials();
                Rootobject objRootobject = new Rootobject();
                string contactjson = "{\"identifiers\":[{\"key\":\"phonenumber\",\"value\":\"" + values.phone_edit.e164Number + "\"}],\"displayName\":\"" + values.displayName_edit + "\",\"firstName\":\"" + values.firstname_edit + "\",\"lastName\":\"" + values.lastname_edit + "\"}";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                var request = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/contacts/identifiers/phonenumber/" + values.phone_edit.e164Number + "", Method.PATCH);
                request.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                var responseoutput = response.Content;
                objRootobject = JsonConvert.DeserializeObject<Rootobject>(responseoutput);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    msSQL = "update  crm_smm_whatsapp set " +
                         " displayName = '" + values.displayName_edit + "'," +
                         " firstName = '" + values.firstname_edit + "'," +
                         " lastName = '" + values.lastname_edit + "'," +
                         " updated_by = '" + user_gid + "'," +
                         " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                         "' where id ='" + objRootobject.id + "'  ";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {

                        msSQL = " select employee_gid from hrm_mst_temployee where user_gid='" + user_gid + "'";
                        string lsemployee_gid = objdbconn.GetExecuteScalar(msSQL);

                        msGetGid = objcmnfunctions.GetMasterGID("BMCC");
                        msGetGid1 = objcmnfunctions.GetMasterGID("BLBP");

                        msSQL = " INSERT INTO crm_trn_tleadbank(" +
                                " leadbank_gid," +
                                " source_gid," +
                                " leadbank_id," +
                                " leadbank_name," +
                                " status," +
                                " approval_flag, " +
                                " lead_status," +
                                " leadbank_code," +
                                " customer_type," +
                                " created_by," +
                                " main_branch," +
                                " created_date)" +
                                " values(" +
                                " '" + msGetGid1 + "'," +
                                " 'BSEM231215135'," +
                                " '" + msGetGid + "'," +
                                " '" + values.displayName_edit + "'," +
                                " 'y'," +
                                " 'Approved'," +
                                " 'Not Assigned'," +
                                " 'H.Q'," +
                                " '" + values.customertype_edit + "'," +
                                " '" + lsemployee_gid + "'," +
                                " 'Y'," +
                                " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msGetGid2 = objcmnfunctions.GetMasterGID("BLBP");
                        if (msGetGid2 == "E")
                        {
                            objresult.status = false;
                            objresult.message = "Create sequence code BLCC for Lead Bank";
                        }
                        msSQL = " INSERT INTO crm_trn_tleadbankcontact" +
                            " (leadbankcontact_gid," +
                            " leadbank_gid," +
                            " leadbankcontact_name," +
                            " mobile," +
                            " created_date," +
                            " created_by," +
                            " leadbankbranch_name, " +
                            " main_contact)" +
                            " values( " +
                            " '" + msGetGid2 + "'," +
                            " '" + msGetGid1 + "'," +
                            " '" + values.displayName_edit + "'," +
                            " '" + values.phone_edit.e164Number + "'," +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                            " '" + lsemployee_gid + "'," +
                            " 'H.Q'," +
                            " 'y'" + ")";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = "update crm_smm_whatsapp set leadbank_gid='" + msGetGid1 + "' where id='" + objRootobject.id + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = "update crm_trn_tleadbank set wh_id='" + objRootobject.id + "' where leadbank_gid='" + msGetGid1 + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    }

                    if (mnResult == 1)
                    {
                        objresult.status = true;
                        objresult.message = "Contact Updated successfully!";
                    }
                    else
                    {
                        objresult.message = "Error occured while adding contact!";
                    }

                }
                else
                {
                    objresult.status = false;
                    objresult.message = "Error occured while posting contact!!";
                }
            }
            catch (Exception ex)
            {
                objresult.message = "Error occured while posting contact!!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.ToString() + "***********" + objresult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
          
            return objresult;
        }

        public result daCreateProject(mdlCreateTemplateInput values, string user_gid)
        {
            result objresult = new result();
            try
            {

                int i = 0;
                whatsappconfiguration getwhatsappcredentials = whatsappcredentials();
                createProject objcreateProject = new createProject();
                string contactjson = "{\"type\":\"channelTemplate\",\"name\":\"" + values.name + "\",\"description\":\"" + values.description + "\"}";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                var request = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/projects", Method.POST);
                request.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                var responseoutput = response.Content;
                objcreateProject = JsonConvert.DeserializeObject<createProject>(responseoutput);
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    msSQL = "insert into crm_smm_whatsapptemplate(project_id,p_type,p_name,created_date)values(" +
                            "'" + objcreateProject.id + "'," +
                            "'" + objcreateProject.type + "'," +
                            "'" + objcreateProject.name + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        objresult.status = true;
                        objresult.message = "Project created successfully!";
                    }
                    else
                    {
                        objresult.message = "Error occured while adding Project!";
                    }
                }
                else
                {
                    objresult.status = false;
                    objresult.message = "Error occured while posting Project!!";
                }
            }
            catch (Exception ex)
            {
                objresult.message = "Error occured while posting Project!!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + objresult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            return objresult;
        }

        public result daCreateTemplate(HttpRequest httpRequest)
        {
            result objresult = new result();
            try
            {
                HttpFileCollection httpFileCollection;
                HttpPostedFile httpPostedFile;
                string file_type = httpRequest.Form[0];
                string body = httpRequest.Form[1];  
                string p_name = httpRequest.Form[2];
                string project_id = httpRequest.Form[3];
                string footer = httpRequest.Form[4];
                msSQL = " select company_code from adm_mst_tcompany";
                lscompany_code = objdbconn.GetExecuteScalar(msSQL);


                if (httpRequest.Files.Count > 0)
                {
                    string lsfirstdocument_filepath = string.Empty;
                    httpFileCollection = httpRequest.Files;
                    for (int i = 0; i < httpFileCollection.Count; i++)
                    {
                        string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                        httpPostedFile = httpFileCollection[i];
                        string file_name = httpPostedFile.FileName;
                        string lsfile_gid = msdocument_gid;
                        string lscompany_document_flag = string.Empty;
                        string FileExtension = Path.GetExtension(file_name).ToLower();
                        lsfile_gid += FileExtension;
                        Stream ls_readStream = httpPostedFile.InputStream;
                        MemoryStream ms = new MemoryStream();
                        ls_readStream.CopyTo(ms);
                        string mime_type = MimeMapping.GetMimeMapping(httpPostedFile.FileName);


                        bool status1;
                        status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"], lscompany_code + "/" + "CRM/Whatsapp/WhatsappTemplate" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension, FileExtension, ms);
                        ms.Close();

                        final_path = ConfigurationManager.AppSettings["blob_containername"] + "/" + lscompany_code + "/" + "CRM/Whatsapp/WhatsappTemplate" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";


                        msSQL = "insert into crm_trn_tfiles(" +
                                "file_gid," +
                                "document_name," +
                                "document_path)values(" +
                                "'" + msdocument_gid + "'," +
                                "'" + file_name.Replace("'", "\\'") + "'," +
                                "'" + final_path + lsfile_gid.Replace("'", "\\'") + "')";
                        mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult1 == 1)
                        {
                            string mediaurl = ConfigurationManager.AppSettings["blob_imagepath1"] + final_path + msdocument_gid + FileExtension + ConfigurationManager.AppSettings["blob_imagepath2"] +
                                                                           '&' + ConfigurationManager.AppSettings["blob_imagepath3"] + '&' + ConfigurationManager.AppSettings["blob_imagepath4"] + '&' + ConfigurationManager.AppSettings["blob_imagepath5"] +
                                                                           '&' + ConfigurationManager.AppSettings["blob_imagepath6"] + '&' + ConfigurationManager.AppSettings["blob_imagepath7"] + '&' + ConfigurationManager.AppSettings["blob_imagepath8"];

                            whatsappconfiguration getwhatsappcredentials = whatsappcredentials();
                            TemplateCreation objcreatetemplate = new TemplateCreation();
                            
                                string contactjson = "{\"defaultLocale\":\"en\",\"genericContent\":[],\"platformContent\":[{\"platform\":\"whatsapp\",\"locale\":\"en\",\"blocks\":[{\"type\":\"image\",\"role\":\"header\",\"image\":{\"mediaUrl\":\"" + mediaurl + "\"}," +
                                                 "\"id\":\"7vY1pgyRZ7ETflSK46d5qL\"},{\"type\":\"text\",\"role\":\"body\",\"text\":{\"text\":\"" + body + "\"},\"id\":\"cbA0XdmkFDAbJiGfS41kK9\"},{\"type\":\"text\",\"role\":\"footer\"," +
                                                 "\"text\":{\"text\":\""+ footer + "\"},\"id\":\"lA0CtTqaNO2zFz9p_Uoxo7\"}],\"type\":\"text\",\"channelGroupIds\":[\""+ getwhatsappcredentials.channelgroup_id + "\"]}],\"supportedPlatforms\":[\"whatsapp\"]," +
                                                 "\"deployments\":[{\"key\":\"whatsappTemplateName\",\"platform\":\"whatsapp\",\"value\":\"" + p_name + "\"},{\"key\":\"whatsappCategory\",\"platform\":\"whatsapp\",\"value\":\"MARKETING\"}," +
                                                 "{\"key\":\"whatsappAllowCategoryChange\",\"platform\":\"whatsapp\",\"value\":\"false\"}]}";
                            
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                            var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                            var request = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/projects/" + project_id + "/channel-templates", Method.POST);
                            request.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                            request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                            IRestResponse response = client.Execute(request);
                            var responseoutput = response.Content;
                            objcreatetemplate = JsonConvert.DeserializeObject<TemplateCreation>(responseoutput);                            
                            if (response.StatusCode == HttpStatusCode.Created)
                            {
                                int j = 0;

                                msSQL = " update  crm_smm_whatsapptemplate  set " +
                                        " template_id = '" + objcreatetemplate.id + "'," +
                                        " template_body = '" + body + "'," +
                                        " footer = '" + footer + "'," +
                                        " media_url = '" + mediaurl + "' " +
                                        " where project_id='" + project_id + "'  ";


                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult == 1)
                                {
                                    objresult.status = true;
                                    objresult.message = "Template created successfully!";
                                }
                                else
                                {
                                    objresult.message = "Error occured while adding Template!";
                                }
                            }
                            else
                            {
                                objresult.status = false;
                                objresult.message = "Error occured while posting Template!!";
                            }

                        }
                        else
                        {
                            objresult.message = "Error occured while uploading document!";
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + objresult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objresult.message = "Exception occured while sending message!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + objresult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

            return objresult;
        }
        public result daCreatetexttemplate(mdlCreateTemplateInput1 values, string user_gid)
        {
            whatsappconfiguration getwhatsappcredentials = whatsappcredentials();
            result objresult = new result();
            try
            {

                TemplateCreation objcreatetemplate = new TemplateCreation();

                string contactjson = "{\"defaultLocale\":\"en\",\"genericContent\":[],\"platformContent\":[{\"platform\":\"whatsapp\"," +
                                                   "\"locale\":\"en\",\"blocks\":[{\"type\":\"text\",\"role\":\"body\",\"text\":{\"text\":\"" + values.body + "\"}" +
                                                   ",\"id\":\"cbA0XdmkFDAbJiGfS41kK9\"},{\"type\":\"text\",\"role\":\"footer\"," +
                                                   " \"text\":{\"text\":\"" + values.whatsapptemplate.footer + "\"},\"id\":\"lA0CtTqaNO2zFz9p_Uoxo7\"}],\"type\":\"text\",\"channelGroupIds\"" +
                                                   ":[\"" + getwhatsappcredentials.channelgroup_id + "\"]}],\"supportedPlatforms\":[\"whatsapp\"],\"deployments\":[{\"key\":\"whatsappTemplateName\",\"platform\":\"whatsapp\",\"value\":\"" + values.whatsapptemplate.p_name + "\"},{\"key\":\"whatsappCategory" +
                                                   "\",\"platform\":\"whatsapp\",\"value\":\"MARKETING\"},{\"key\":\"whatsappAllowCategoryChange\",\"platform\":\"whatsapp\",\"value\":\"false\"}]}";

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                var request = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/projects/" + values.whatsapptemplate.project_id + "/channel-templates", Method.POST);
                request.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                var responseoutput = response.Content;
                objcreatetemplate = JsonConvert.DeserializeObject<TemplateCreation>(responseoutput);
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    int j = 0;

                    msSQL = " update  crm_smm_whatsapptemplate  set " +
                            " template_id = '" + objcreatetemplate.id + "'," +
                            " template_body = '" + values.body + "'," +
                            " footer = '" + values.whatsapptemplate.footer + "'" +
                            " where project_id='" + values.whatsapptemplate.project_id + "'  ";


                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        objresult.status = true;
                        objresult.message = "Template created successfully!";
                    }
                    else
                    {
                        objresult.message = "Error occured while adding Template!";
                    }
                }
                else
                {
                    objresult.status = false;
                    objresult.message = "Error occured while posting Template!!";
                }
            }
            catch (Exception ex)
            {
                objresult.message = "Error occured while posting Template!!";

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + objresult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

            return objresult;
        }

        public result daPublishTemplate(mdlpublishtemplate values)
        {
            result objresult = new result();
            try
            {

                whatsappconfiguration getwhatsappcredentials = whatsappcredentials();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                string project_id = values.project_id;
                string template_id = values.template_id;
                var request = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/projects/" + values.project_id + "/channel-templates/" + values.template_id + "/activate", Method.PUT);
                request.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    objresult.status = true;
                    objresult.message = "Published successfully!";
                }
                else
                {
                    objresult.message = "Error occured while Publishing Template!";
                }
            }
            catch (Exception ex)
            {
                objresult.message = "Error occured while Publishing Template!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + objresult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
            return objresult;

        }

        public result DaWhatsappSend(sendmessage values, string user_gid)
        {
            result objresult = new result();
            try
            {

                int i = 0;
                whatsappconfiguration getwhatsappcredentials = whatsappcredentials();

                Result objsendmessage = new Result();
                if (values.project_id != null)
                {
                    string contactjson = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\"" + values.identifierValue + "\",\"identifierKey\":\"phonenumber\"}]},\"template\":{\"projectId\":\"" + values.project_id + "\",\"version\":\"" + values.version + "\",\"locale\":\"en\"}}";

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                    var request = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/channels/" + getwhatsappcredentials.channel_id + "/messages", Method.POST);
                    request.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                    request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    string waresponse = response.Content;
                    objsendmessage = JsonConvert.DeserializeObject<Result>(waresponse);

                    if (response.StatusCode == HttpStatusCode.Accepted)
                    {
                        msSQL = "insert into crm_trn_twhatsappmessages(" +
                                 "message_id," +
                                 "contact_id," +
                                 "direction," +
                                 "type," +
                                 "message_text," +
                                 "content_type," +
                                  "project_id," +
                                 "version_id," +
                                 "status," +
                                 "created_date)" +
                                 "values(" +
                                 "'" + objsendmessage.id + "'," +
                                 "'" + objsendmessage.receiver.contacts[0].id + "'," +
                                 "'" + objsendmessage.direction + "'," +
                                "'" + objsendmessage.body.type + "',";
                        if (objsendmessage.body.type == "text")
                        {
                            msSQL += "'" + objsendmessage.body.text.text.Replace("'", "\\'") + "'," +
                                     "null,";
                        }
                        else if (objsendmessage.body.type == "list")
                        {
                            msSQL += "'" + objsendmessage.body.list.text.Replace("'", "\\'") + "'," +
                                     "null,";
                        }
                        else if (objsendmessage.body.type == "image")
                        {
                            msSQL += "'" + objsendmessage.body.image.images[0].mediaUrl.Replace("'", "\\'") + "'," +
                                     "null,";
                        }
                        else
                        {
                            msSQL += "'" + objsendmessage.body.file.files[0].mediaUrl.Replace("'", "\\'") + "'," +
                                     "'" + objsendmessage.body.file.files[0].contentType.Replace("'", "\\'") + "',";
                        }
                        msSQL += "'" + objsendmessage.template.projectId + "'," +
                                 "'" + objsendmessage.template.version + "'," +
                                 "'" + objsendmessage.status + "'," +
                                 "'" + objsendmessage.createdAt.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            objresult.status = true;
                            objresult.message = "Sent Successfully!!!";
                        }
                        else
                        {
                            objresult.message = "Failed to Send!!!";
                        }
                    }
                    else
                    {
                        objresult.status = false;
                        objresult.message = "Failed to Send!!!";
                    }
                }
                else
                {
                    Servicewindow objsendmessage1 = new Servicewindow();

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                    var request = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/channels/" + getwhatsappcredentials.channel_id + "/contacts/" + values.contact_id, Method.GET);
                    request.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                    IRestResponse response1 = client.Execute(request);
                    string waresponse1 = response1.Content;
                    objsendmessage1 = JsonConvert.DeserializeObject<Servicewindow>(waresponse1);
                    if (objsendmessage1.serviceWindowExpireAt != null)
                    {
                        string contactjson = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\"" + values.identifierValue + "\",\"identifierKey\":\"phonenumber\"}]},\"body\":{\"type\":\"text\",\"text\":{\"text\":\"" + values.sendtext + "\"}}}";

                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        var client1 = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                        var request1 = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/channels/" + getwhatsappcredentials.channel_id + "/messages", Method.POST);
                        request1.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                        request1.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                        IRestResponse response2 = client1.Execute(request1);
                        string waresponse = response2.Content;
                        objsendmessage = JsonConvert.DeserializeObject<Result>(waresponse);

                        if (response2.StatusCode == HttpStatusCode.Accepted)
                        {
                            msSQL = "insert into crm_trn_twhatsappmessages(" +
                                     "message_id," +
                                     "contact_id," +
                                     "direction," +
                                     "type," +
                                     "message_text," +
                                     "content_type," +
                                     "status," +
                                     "created_date)" +
                                     "values(" +
                                     "'" + objsendmessage.id + "'," +
                                     "'" + objsendmessage.receiver.contacts[0].id + "'," +
                                     "'" + objsendmessage.direction + "'," +
                                    "'" + objsendmessage.body.type + "',";
                            if (objsendmessage.body.type == "text")
                            {
                                msSQL += "'" + objsendmessage.body.text.text.Replace("'", "\\'") + "'," +
                                         "null,";
                            }
                            else if (objsendmessage.body.type == "list")
                            {
                                msSQL += "'" + objsendmessage.body.list.text.Replace("'", "\\'") + "'," +
                                         "null,";
                            }
                            else if (objsendmessage.body.type == "image")
                            {
                                msSQL += "'" + objsendmessage.body.image.images[0].mediaUrl.Replace("'", "\\'") + "'," +
                                         "null,";
                            }
                            else
                            {
                                msSQL += "'" + objsendmessage.body.file.files[0].mediaUrl.Replace("'", "\\'") + "'," +
                                         "'" + objsendmessage.body.file.files[0].contentType.Replace("'", "\\'") + "',";
                            }
                            msSQL += "'" + objsendmessage.status + "'," +
                                     "'" + objsendmessage.createdAt.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                            mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult3 == 1)
                            {
                                objresult.status = true;
                                objresult.message = "Sent Successfully!";
                            }
                            else
                            {
                                objresult.message = "Failed to Send!";
                            }
                        }
                        else
                        {
                            objresult.status = false;
                            objresult.message = "Service Window closed";
                        }
                    }
                    else
                    {
                        objresult.status = false;
                        objresult.message = "Service Window closed";
                    }
                }
            }
            catch (Exception ex)
            {
                objresult.message = "Service Window closed";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + objresult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           

            return objresult;
        }


        public result DaGetTemplate()
        {
            whatsappconfiguration getwhatsappcredentials = whatsappcredentials();
            result objresult = new result();
            try
            {

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient("https://nest.messagebird.com");
                var request = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/projects", Method.GET);
                request.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                IRestResponse response = client.Execute(request);
                IRestResponse responseAddress = client.Execute(request);
                string errornetsuiteJSON = responseAddress.Content;
                Templatelist objMdlWhatsappTemplate = new Templatelist();
                objMdlWhatsappTemplate = JsonConvert.DeserializeObject<Templatelist>(errornetsuiteJSON);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    foreach (var item in objMdlWhatsappTemplate.results)
                    {

                        if (item.activeResourceId != null)
                        {
                            msSQL = " update  crm_smm_whatsapptemplate  set " +
                                    " version_id = '" + item.activeResourceId + "' where project_id='" + item.id + "'  ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                else
                {
                    objresult.status = false;
                    objresult.message = "template receive failed";
                }
            }
            catch (Exception ex)
            {
                objresult.message = "template receive failed";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + objresult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

            return objresult;
        }

        public void DaGetContact(MdlWhatsapp values)
        {
            try
            {


                msSQL = "SELECT c.source_name,b.customer_type,b.lead_status,SUBSTRING(displayName, 1, 1) AS first_letter,displayName,Wvalue,id," +
                               " (SELECT COUNT(read_flag) FROM crm_trn_twhatsappmessages WHERE contact_id = id  AND direction = 'incoming' AND read_flag = 'N') AS count," +
                               " (SELECT CASE WHEN DATE(created_date) = CURDATE() THEN TIME_FORMAT(created_date, '%h:%i %p') " +
                               " WHEN DATE(created_date) = CURDATE() - INTERVAL 1 DAY THEN 'Yesterday'  ELSE DATE_FORMAT(created_date, '%d/%m/%y') END AS formatted_date" +
                               " FROM crm_trn_twhatsappmessages WHERE contact_id = id ORDER BY created_date DESC LIMIT 1) AS last_seen " +
                               " FROM crm_smm_whatsapp " +
                               " LEFT JOIN crm_trn_tleadbank b ON b.wh_id =id " +
                              " LEFT JOIN crm_mst_tsource c ON c.source_gid = b.source_gid" +
                               " ORDER BY CASE WHEN last_seen REGEXP '^[0-9]{2}:[0-9]{2} [APMapm]{2}$' THEN 1 WHEN last_seen = 'Yesterday' THEN 2 ELSE 3 END, last_seen DESC";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<whatscontactlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new whatscontactlist
                        {
                            whatsapp_gid = dt["id"].ToString(),
                            displayName = dt["displayName"].ToString(),
                            value = dt["Wvalue"].ToString(),
                            first_letter = dt["first_letter"].ToString(),
                            read_flag = dt["count"].ToString(),
                            last_seen = dt["last_seen"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            source_name = dt["source_name"].ToString(),
                            lead_status = dt["lead_status"].ToString(),


                        });
                        values.whatscontactlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error while Fetching Contacts";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        public void DaGetMessage(MdlWhatsapp values, string whatsapp_gid)
        {
            try
            {


                msSQL = "update crm_trn_twhatsappmessages set read_flag = 'Y' where contact_id ='" + whatsapp_gid + "' and direction='incoming'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = "SELECT a.message_id,a.created_date,a.direction,a.contact_id,b.lastName,b.firstName, a.message_text,a.type,a.status," +
                        " CONCAT(DATE_FORMAT(a.created_date, '%e %b %y, '), DATE_FORMAT(a.created_date, '%h:%i %p')) AS time," +
                        " b.wvalue AS identifierValue,SUBSTRING(b.displayName, 1, 1) AS first_letter, b.displayName,c.document_name,c.document_path" +
                        " FROM crm_trn_twhatsappmessages a LEFT JOIN crm_smm_whatsapp b ON b.id = a.contact_id" +
                        " left join crm_trn_tfiles c on a.message_id = c.message_gid" +
                        " WHERE  contact_id = '" + whatsapp_gid + "'ORDER BY a.created_date DESC, time DESC";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<whatsmessagelist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new whatsmessagelist
                        {
                            displayName = dt["displayName"].ToString(),
                            first_letter = dt["first_letter"].ToString(),
                            firstName = dt["firstName"].ToString(),
                            lastName = dt["lastName"].ToString(),
                            message_text = dt["message_text"].ToString(),
                            type = dt["type"].ToString(),
                            status = dt["status"].ToString(),
                            time = dt["time"].ToString(),
                            contact_id = dt["contact_id"].ToString(),
                            direction = dt["direction"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            message_id = dt["message_id"].ToString(),
                            identifierValue = dt["identifierValue"].ToString(),
                            document_name = dt["document_name"].ToString(),
                            document_path = dt["document_path"].ToString(),
                        });
                        values.whatsmessagelist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error while Fetching Messages";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        public void DaGetMessageTemplatesummary(MdlWhatsapp values)
        {
            try
            {


                msSQL = " select project_id,template_id,template_body,p_type,p_name,footer,created_date,version_id from crm_smm_whatsapptemplate";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<whatsappMessagetemplatelist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new whatsappMessagetemplatelist
                        {
                            id = dt["project_id"].ToString(),
                            template_id = dt["template_id"].ToString(),
                            template_body = dt["template_body"].ToString(),
                            footer = dt["footer"].ToString(),
                            p_type = dt["p_type"].ToString(),
                            p_name = dt["p_name"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            version = dt["version_id"].ToString(),
                        });
                        values.whatsappMessagetemplatelist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error while Fetching Template Summary";

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        public void DaGetMessageTemplateview(MdlWhatsapp values, string project_id)
        {
            try
            {


                msSQL = " select project_id,template_id,template_body,p_type,p_name,footer,media_url,created_date,version_id from crm_smm_whatsapptemplate" +
                      " WHERE  project_id = '" + project_id + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<whatsappMessagetemplatelist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new whatsappMessagetemplatelist
                        {
                            id = dt["project_id"].ToString(),
                            template_id = dt["template_id"].ToString(),
                            template_body = dt["template_body"].ToString(),
                            footer = dt["footer"].ToString(),
                            p_type = dt["p_type"].ToString(),
                            media_url = dt["media_url"].ToString(),
                            p_name = dt["p_name"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            version = dt["version_id"].ToString(),
                        });
                        values.whatsappMessagetemplatelist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error while Fetching Template View";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }
        public void DaGetContactCount(MdlWhatsapp values)
        {
            try
            {


                msSQL = "select count(*) as count  from crm_smm_whatsapp";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<contactcount_list1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new contactcount_list1
                        {
                            contact_count1 = dt["count"].ToString(),

                        });
                        values.contactcount_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error while Fetching Contact Count";

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        public void DaGetcampaign(MdlWhatsapp values)
        {
            try
            {


                msSQL = "SELECT distinct(a.project_id), a.version_id, a.p_type, a.p_name, DATE_FORMAT(a.created_date, '%d-%m-%Y') AS created_date," +
                    " (SELECT COUNT(b.status) FROM crm_trn_twhatsappmessages b WHERE a.project_id = b.project_id) AS send_campaign FROM crm_smm_whatsapptemplate a " +
                    "LEFT JOIN crm_trn_twhatsappmessages b ON a.project_id = b.project_id";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<whatsappCampaign>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new whatsappCampaign
                        {
                            project_id = dt["project_id"].ToString(),
                            version_id = dt["version_id"].ToString(),
                            p_type = dt["p_type"].ToString(),
                            p_name = dt["p_name"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            send_campaign = dt["send_campaign"].ToString(),
                        });
                        values.whatsappCampaign = getModuleList;
                    }

                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Error while Fetching Campaign";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }


        public void DaDeleteCampaign(string project_id, whatsappCampaign values)
        {
         try
            {
                msSQL = "delete from crm_smm_whatsapptemplate where project_id='" + project_id + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Deleted Successfully!!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting!!";
                }

            }
            catch (Exception ex)
            {
                values.message = "Error while Deleting Campaign";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        public void DaGetlog(MdlWhatsapp values, string project_id)
        {
            try
            {


                string msSQL = "select d.p_name,e.source_name,c.customer_type,c.lead_status,wvalue,displayName,b.identifiervalue,b.status,b.direction, DATE_FORMAT(b.created_date, '%d-%m-%Y') AS created_date from crm_smm_whatsapp a " +
                     " left join crm_trn_twhatsappmessages b on b.contact_id = a.id " +
                      " LEFT JOIN crm_trn_tleadbank c ON c.wh_id = a.id " +
                       " LEFT JOIN crm_smm_whatsapptemplate d ON d.version_id = b.version_id " +
                              " LEFT JOIN crm_mst_tsource e ON e.source_gid = c.source_gid " +
                     " where b.project_id='" + project_id + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<log>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new log
                        {
                            wvalue = dt["wvalue"].ToString(),
                            displayName = dt["displayName"].ToString(),
                            identifiervalue = dt["identifiervalue"].ToString(),
                            status = dt["status"].ToString(),
                            direction = dt["direction"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            source_name = dt["source_name"].ToString(),
                            lead_status = dt["lead_status"].ToString(),
                            p_name = dt["p_name"].ToString(),

                        });
                        values.log = getModuleList;
                    }

                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Error while Fetching Log";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }

        public void DaGetTemplatepreview(MdlWhatsapp values, string project_id)
        {
            try
            {


                string msSQL = "select p_name,template_body,footer,media_url from crm_smm_whatsapptemplate a " +
                    "where project_id='" + project_id + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Gettemplatepreviewview_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Gettemplatepreviewview_list
                        {
                            p_name = dt["p_name"].ToString(),
                            template_body = dt["template_body"].ToString(),
                            footer = dt["footer"].ToString(),
                            media_url = dt["media_url"].ToString(),
                        });
                        values.Gettemplateview_list = getModuleList;
                    }

                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Error while Fetching Template Preview";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }

        public result dabulkMessageSend(mdlBulkMessageList values)
        {
            whatsappconfiguration getwhatsappcredentials = whatsappcredentials();
            result result = new result();
            try
            {

                Result objsendmessage = new Result();
                msSQL = "select version_id,p_name from crm_smm_whatsapptemplate where project_id = '" + values.project_id + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows)
                {
                     

                    string version_id = objMySqlDataReader["version_id"].ToString();
                    string project_name = objMySqlDataReader["p_name"].ToString();
                     
                    foreach (var item in values.contacts_list)
                    {
                        try
                        {
                            string contactjson = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\"" + item.value + "\",\"identifierKey\":\"phonenumber\"}]},\"template\":{\"projectId\":\"" + values.project_id + "\",\"version\":\"" + version_id + "\",\"locale\":\"en\"}}";

                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                            var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                            var request = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/channels/" + getwhatsappcredentials.channel_id + "/messages", Method.POST);
                            request.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                            request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                            IRestResponse response = client.Execute(request);
                            string waresponse = response.Content;
                            objsendmessage = JsonConvert.DeserializeObject<Result>(waresponse);

                            if (response.StatusCode == HttpStatusCode.Accepted)
                            {
                                msSQL = "insert into crm_trn_twhatsappmessages(" +
                                        "message_id," +
                                        "contact_id," +
                                         "identifiervalue," +
                                        "direction," +
                                        "type," +
                                        "message_text," +
                                        "content_type," +
                                        "project_id," +
                                        "version_id," +
                                        "status," +
                                        "created_date)" +
                                        "values(" +
                                        "'" + objsendmessage.id + "'," +
                                        "'" + objsendmessage.receiver.contacts[0].id + "'," +
                                        "'" + objsendmessage.receiver.contacts[0].identifierValue + "'," +
                                        "'" + objsendmessage.direction + "'," +
                                        "'" + objsendmessage.body.type + "',";
                                if (objsendmessage.body.type == "text")
                                {
                                    msSQL += "'" + objsendmessage.body.text.text.Replace("'", "\\'") + "'," +
                                             "null,";
                                }
                                else if (objsendmessage.body.type == "list")
                                {
                                    msSQL += "'" + objsendmessage.body.list.text.Replace("'", "\\'") + "'," +
                                             "null,";
                                }
                                else if (objsendmessage.body.type == "image")
                                {
                                    msSQL += "'" + objsendmessage.body.image.images[0].mediaUrl.Replace("'", "\\'") + "'," +
                                             "null,";
                                }
                                msSQL += "'" + objsendmessage.template.projectId + "'," +
                                        "'" + objsendmessage.template.version + "'," +
                                        "'" + objsendmessage.status + "'," +
                                        "'" + objsendmessage.createdAt.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult == 1)
                                {
                                    result.status = true;
                                    result.message = "Sent Successfully!";
                                }
                                else
                                {
                                    result.message = "Failed to Send!";
                                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}"  + "***********" + result.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                                }
                            }
                            else
                            {
                                result.status = false;
                                result.message = "Sending failed";
                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error Occured while sending message to " + item.value + " project ID : " + values.project_id + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + result.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                            }
                        }
                        catch (Exception ex)
                        {
                            result.message = "Sending failed";
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Exception Occured while sending message to " + item.value + " project ID : " + values.project_id + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + result.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                        }
                    }
                }
                objMySqlDataReader.Close();
            }
            catch (Exception ex)
            {
                result.message = "Error while Sending Message";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + result.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

            return result;
        }

        public void DaGetWhatsappMessageCount(MdlWhatsapp values)
        {
            try
            {


                msSQL = "SELECT COUNT(*) AS Total_messages,COUNT(CASE WHEN direction = 'incoming' THEN 1 END) AS Received_messages,COUNT(CASE WHEN direction = 'outgoing' THEN 1 END) AS Sent_messages FROM crm_trn_twhatsappmessages";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<whatsappmessagescount>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new whatsappmessagescount
                        {

                            Total_messages = dt["Total_messages"].ToString(),
                            Received_messages = dt["Received_messages"].ToString(),
                            Sent_messages = dt["Sent_messages"].ToString(),
                        });
                        values.whatsappmessagescount = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error while Message Count";

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        public notifications daNotifications()
        {
            int count = 0;
            notifications objNotifications = new notifications();
            try
            {
                msSQL = " SELECT b.displayName,a.contact_id,CONCAT(d.user_firstname, ' ', d.user_lastname) AS assigned_to,COUNT(a.read_flag) AS count,b.leadbank_gid,(SELECT " +
                        " MAX(created_date)FROM crm_trn_twhatsappmessages WHERE contact_id = a.contact_id) AS created_date, " +
                        " 'wa' AS ca_type FROM crm_trn_twhatsappmessages a " +
                        " LEFT JOIN crm_smm_whatsapp b ON b.id = a.contact_id " +
                        " LEFT JOIN crm_trn_tleadbank c ON c.leadbank_gid = b.leadbank_gid " +
                        " LEFT JOIN adm_mst_tuser d ON d.user_gid = c.assign_to " +
                        " WHERE a.direction = 'incoming' AND a.read_flag = 'N' GROUP BY a.contact_id " +
                        " UNION SELECT to_mail AS displayName,mailmanagement_gid AS contact_id,NULL AS assigned_to,COUNT(read_flag) AS count,leadbank_gid, " +
                        " MAX(created_date) AS created_date,'em' AS ca_type " +
                        " FROM crm_smm_mailmanagement WHERE direction = 'incoming' AND read_flag = 'N' GROUP BY mailmanagement_gid ORDER BY created_date DESC ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count > 0)
                {
                    var getNotificationList = new List<notification_list>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getNotificationList.Add(new notification_list
                            {
                                displayName = dt["displayName"].ToString(),
                                contact_id = dt["contact_id"].ToString(),
                                count = dt["count"].ToString(),
                                leadbank_gid = dt["leadbank_gid"].ToString(),
                                ca_type = dt["ca_type"].ToString(),
                                assigned_to = dt["assigned_to"].ToString()

                            });
                            objNotifications.notification_Lists = getNotificationList;
                            count++;
                        }
                    }
                    dt_datatable.Dispose();
                    objNotifications.notification_count = count;
                }
            }
            catch (Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

            return objNotifications;
        }

        public result daSendDocuments(HttpRequest httpRequest)
        {
            whatsappconfiguration getwhatsappcredentials = whatsappcredentials();
            result objresult = new result();
            try
            {


                Servicewindow objsendmessage1 = new Servicewindow();
                string contact_id = httpRequest.Form[1];

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                var request = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/channels/" + getwhatsappcredentials.channel_id + "/contacts/" + contact_id, Method.GET);
                request.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                IRestResponse response1 = client.Execute(request);
                string waresponse1 = response1.Content;
                objsendmessage1 = JsonConvert.DeserializeObject<Servicewindow>(waresponse1);
                if (objsendmessage1.serviceWindowExpireAt != null)
                {
                    try
                    {
                        HttpFileCollection httpFileCollection;
                        HttpPostedFile httpPostedFile;
                        string file_type = httpRequest.Form[0];
                        string contactjson = "";
                        msSQL = " select company_code from adm_mst_tcompany";
                        lscompany_code = objdbconn.GetExecuteScalar(msSQL);

                        if (httpRequest.Files.Count > 0)
                        {
                            string lsfirstdocument_filepath = string.Empty;
                            httpFileCollection = httpRequest.Files;
                            for (int i = 0; i < httpFileCollection.Count; i++)
                            {
                                string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                                httpPostedFile = httpFileCollection[i];
                                string file_name = httpPostedFile.FileName;
                                string lsfile_gid = msdocument_gid;
                                string lscompany_document_flag = string.Empty;
                                string FileExtension = Path.GetExtension(file_name).ToLower();
                                lsfile_gid += FileExtension;
                                Stream ls_readStream = httpPostedFile.InputStream;
                                MemoryStream ms = new MemoryStream();
                                ls_readStream.CopyTo(ms);
                                string mime_type = MimeMapping.GetMimeMapping(httpPostedFile.FileName);

                                msSQL = "select wvalue from crm_smm_whatsapp where id = '" + contact_id + "'";
                                string phonenumber = objdbconn.GetExecuteScalar(msSQL);

                                bool status1;
                                status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"], lscompany_code + "/" + "CRM/Whatsapp/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension, FileExtension, ms);
                                ms.Close();

                                final_path = ConfigurationManager.AppSettings["blob_containername"] + "/" + lscompany_code + "/" + "CRM/Whatsapp/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";


                                msSQL = "insert into crm_trn_tfiles(" +
                                        "file_gid," +
                                        "document_name," +
                                        "document_path)values(" +
                                        "'" + msdocument_gid + "'," +
                                        "'" + file_name.Replace("'", "\\'") + "'," +
                                        "'" + ConfigurationManager.AppSettings["blob_imagepath1"] + final_path + msdocument_gid + FileExtension + ConfigurationManager.AppSettings["blob_imagepath2"] +
                                                           '&' + ConfigurationManager.AppSettings["blob_imagepath3"] + '&' + ConfigurationManager.AppSettings["blob_imagepath4"] + '&' + ConfigurationManager.AppSettings["blob_imagepath5"] +
                                                           '&' + ConfigurationManager.AppSettings["blob_imagepath6"] + '&' + ConfigurationManager.AppSettings["blob_imagepath7"] + '&' + ConfigurationManager.AppSettings["blob_imagepath8"] + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult == 1)
                                {

                                    string mediaurl = ConfigurationManager.AppSettings["blob_imagepath1"] + final_path + msdocument_gid + FileExtension + ConfigurationManager.AppSettings["blob_imagepath2"] +
                                                     '&' + ConfigurationManager.AppSettings["blob_imagepath3"] + '&' + ConfigurationManager.AppSettings["blob_imagepath4"] + '&' + ConfigurationManager.AppSettings["blob_imagepath5"] +
                                                     '&' + ConfigurationManager.AppSettings["blob_imagepath6"] + '&' + ConfigurationManager.AppSettings["blob_imagepath7"] + '&' + ConfigurationManager.AppSettings["blob_imagepath8"];
                                    if (file_type == "image")
                                        contactjson = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\"" + phonenumber + "\",\"identifierKey\":\"phonenumber\"}]},\"body\":{\"type\":\"image\",\"image\":{\"images\":[{\"mediaUrl\":\"" + mediaurl + "\"}]}}}";
                                    else
                                        contactjson = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\" " + phonenumber + " \",\"identifierKey\":\"phonenumber\"}]},\"body\":{\"type\":\"file\",\"file\":{\"files\":[{\"contentType\":\"" + mime_type + "\",\"mediaUrl\":\"" + mediaurl + "\"}]}}}";
                                    Result objsendmessage = new Result();
                                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                    var client1 = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                                    var request1 = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/channels/" + getwhatsappcredentials.channel_id + "/messages", Method.POST);
                                    request1.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
                                    request1.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                                    IRestResponse response = client1.Execute(request1);
                                    string waresponse = response.Content;
                                    objsendmessage = JsonConvert.DeserializeObject<Result>(waresponse);

                                    if (response.StatusCode == HttpStatusCode.Accepted)
                                    {
                                        msSQL = "insert into crm_trn_twhatsappmessages(" +
                                                "message_id," +
                                                "contact_id," +
                                                "direction," +
                                                "type," +
                                                "message_text," +
                                                "content_type," +
                                                "status," +
                                                "created_date)" +
                                                "values(" +
                                                "'" + objsendmessage.id + "'," +
                                                "'" + objsendmessage.receiver.contacts[0].id + "'," +
                                                "'" + objsendmessage.direction + "'," +
                                                "'" + objsendmessage.body.type + "',";
                                        if (objsendmessage.body.type == "text")
                                        {
                                            msSQL += "'" + objsendmessage.body.text.text.Replace("'", "\\'") + "'," +
                                                     "null,";
                                        }
                                        else if (objsendmessage.body.type == "list")
                                        {
                                            msSQL += "'" + objsendmessage.body.list.text.Replace("'", "\\'") + "'," +
                                                     "null,";
                                        }
                                        else if (objsendmessage.body.type == "image")
                                        {
                                            msSQL += "'Image'," +
                                                     "null,";
                                        }
                                        else
                                        {
                                            msSQL += "'File'," +
                                                     "'" + objsendmessage.body.file.files[0].contentType.Replace("'", "\\'") + "',";
                                        }
                                        msSQL += "'" + objsendmessage.status + "'," +
                                                 "'" + objsendmessage.createdAt.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult == 1)
                                        {
                                            msSQL = "update crm_trn_tfiles set " +
                                                    "message_gid = '" + objsendmessage.id + "'," +
                                                    "contact_gid = '" + objsendmessage.receiver.contacts[0].id + "' where file_gid = '" + msdocument_gid + "'";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                            if (mnResult == 0)
                                            {
                                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Update failed: " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                                            }
                                            objresult.status = true;
                                            objresult.message = "Delivered!";
                                        }
                                        else
                                        {
                                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Insert failed: " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                                        }
                                    }
                                    else
                                    {
                                    }
                                }
                                else
                                {
                                    objresult.message = "Error occured while uploading document!";
                                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Insert failed: " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");


                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        objresult.message = "Exception occured while sending message!";
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + objresult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");


                    }
                }
                else
                {
                    objresult.status = false;
                    objresult.message = "Service Window closed";
                }
            }
            catch (Exception ex)
            {

                objresult.message = "Exception occured while sending message!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + objresult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
          
            return objresult;
        }

        public MdlWaFiles daGetDocuments(string contact_id)
        {
            MdlWaFiles obj = new MdlWaFiles();
            var getimagesList = new List<wa_images_list>();
            var getfilesList = new List<wa_files_list>();
            try
            {
                msSQL = "select a.file_gid ,a.document_name,a.document_path,b.content_type, DATE_FORMAT(a.created_date, '%d-%m-%Y') AS created_date from crm_trn_tfiles a " +
                        "left join crm_trn_twhatsappmessages b on b.message_id = a.message_gid " +
                        "where contact_gid = '" + contact_id + "' and b.type = 'image'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getimagesList.Add(new wa_images_list
                        {
                            created_date = dt["created_date"].ToString(),
                            file_gid = dt["file_gid"].ToString(),
                            file_name = dt["document_name"].ToString(),
                            file_path = dt["document_path"].ToString(),
                        });
                        obj.wa_images_list = getimagesList;
                        obj.status = true;
                    }
                }
                msSQL = "select a.file_gid ,a.document_name,a.document_path,b.content_type, DATE_FORMAT(a.created_date, '%d-%m-%Y') AS created_date from crm_trn_tfiles a " +
                        "left join crm_trn_twhatsappmessages b on b.message_id = a.message_gid " +
                        "where contact_gid = '" + contact_id + "' and b.type = 'file'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getfilesList.Add(new wa_files_list
                        {
                            created_date = dt["created_date"].ToString(),
                            file_gid = dt["file_gid"].ToString(),
                            file_name = dt["document_name"].ToString(),
                            file_path = dt["document_path"].ToString(),
                        });
                        obj.wa_files_list = getfilesList;
                        obj.status = true;
                    }
                }
            }
            catch (Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

            return obj;
        }

        //public void LogForAuditWhatsApp(string strVal)
        //{
        //    try
        //    {
        //        string lspath = ConfigurationManager.AppSettings["file_path"] + "/erpdocument/CRM/Whatsapp/ErrorLog/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
        //        if ((!System.IO.Directory.Exists(lspath)))
        //            System.IO.Directory.CreateDirectory(lspath);

        //        lspath = lspath + @"\" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt";

        //        System.IO.StreamWriter sw = new System.IO.StreamWriter(lspath, true);
        //        sw.WriteLine(strVal);
        //        sw.Close();

        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        public whatsappconfiguration whatsappcredentials()
        {
            whatsappconfiguration getwhatsappcredentials = new whatsappconfiguration();
            try
            {


                msSQL = " select workspace_id,channel_id,access_token,channelgroup_id from crm_smm_whatsapp_service";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                     
                    getwhatsappcredentials.workspace_id = objMySqlDataReader["workspace_id"].ToString();
                    getwhatsappcredentials.channel_id = objMySqlDataReader["channel_id"].ToString();
                    getwhatsappcredentials.access_token = objMySqlDataReader["access_token"].ToString();
                    getwhatsappcredentials.channelgroup_id = objMySqlDataReader["channelgroup_id"].ToString();

                     
                }
                else
                {

                }
                objMySqlDataReader.Close();
            }
            catch (Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString()  + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

            return getwhatsappcredentials;
        }
    }
}