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
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.IO;
using System.Web.Http.Results;
using System.Xml.Linq;
using System.Web.Http.Filters;
using System.Reflection;
using MySql.Data.MySqlClient;
using static OfficeOpenXml.ExcelErrorValue;


namespace ems.crm.DataAccess
{
    public class DaLeadbank360
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        MySqlDataReader objMySqlDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, msGetGid2, msGetGid3, lsdesignation_code, lscustomer_gid, lswhatsapp_contactid, lswhatsapp_gid, lsCode, lslead_email, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        int mnResult6;

        // Code by SNEHITH

        //get indivdual contact details for whatsapp
        public void DaGetWhatsappLeadContact(MdlLeadbank360 values, string leadbank_gid)
        {

            try
            {
                 
                msSQL = " select wh_id from crm_trn_tleadbank where leadbank_gid  = '" + leadbank_gid + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows)
                {
                    lswhatsapp_contactid = objMySqlDataReader["wh_id"].ToString();

                }
                 
                msSQL = "select SUBSTRING(displayName, 1, 1) AS first_letter, displayName,Wvalue,id from crm_smm_whatsapp where id = '" + lswhatsapp_contactid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leadwhatscontactlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leadwhatscontactlist
                        {
                            whatsapp_gid = dt["id"].ToString(),
                            displayName = dt["displayName"].ToString(),
                            value = dt["Wvalue"].ToString(),
                            first_letter = dt["first_letter"].ToString(),
                        });
                        values.leadwhatscontactlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
                objMySqlDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Whatsapp Leadcount!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
          
        }
        //get indivdual contact message for whatsapp

        public result dacreatecontact(mdlCreateContactInput values, string user_gid)
        {
            int i = 0;
            result objresult = new result();
            Rootobject objRootobject = new Rootobject();
            string contactjson = "{\"displayName\":\"" + values.displayName + "\",\"identifiers\":[{\"key\":\"phonenumber\",\"value\":\"" + values.value + "\"}],\"firstName\":\"" + values.firstName + "\",\"gender\":\"" + values.gender + "\",\"lastName\":\"" + values.lastName + "\"}";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
            var request = new RestRequest(ConfigurationManager.AppSettings["messagebirdcontact"].ToString(), Method.POST);
            request.AddHeader("authorization", ConfigurationManager.AppSettings["messagebirdaccesskey"].ToString());
            request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            var responseoutput = response.Content;
            objRootobject = JsonConvert.DeserializeObject<Rootobject>(responseoutput);


            try
            {
                 
                   if (response.StatusCode == HttpStatusCode.Created)
                {
                    msSQL = "insert into crm_smm_whatsapp(id,wkey,wvalue,displayName,firstName,lastName,gender,created_date,created_by)values(" +
                            "'" + objRootobject.id + "'," +
                            "'" + values.key + "'," +
                            "'" + values.value + "'," +
                            "'" + values.displayName + "'," +
                            "'" + values.firstName + "'," +
                            "'" + values.lastName + "'," +
                            "'" + values.gender + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                            "'" + user_gid + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        msSQL = "update crm_trn_tleadbank set" +
                               "whatsappcontact_id = '" + objRootobject.id + "'" +
                               "where leadbank_gid = '" + values.leadbank_gid + "'";
                        mnResult4 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    if (mnResult4 == 1)
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
                values.message = "Exception occured while loading Purchase Liability Report Chart!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
            return objresult;

        }


        public void DaGetWhatsappLeadMessage(MdlLeadbank360 values, string leadbank_gid)
        {

            try
            {
                 
                msSQL = " select wh_id from crm_trn_tleadbank where leadbank_gid  = '" + leadbank_gid + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows)
                {
                    lswhatsapp_contactid = objMySqlDataReader["wh_id"].ToString();

                }
                 


                msSQL = "update crm_trn_twhatsappmessages set read_flag = 'Y' where contact_id ='" + lswhatsapp_contactid + "' and direction='incoming'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = "SELECT a.message_id,a.created_date,a.direction,a.contact_id,b.lastName,b.firstName, a.message_text,a.type,a.status," +
                        " CONCAT(DATE_FORMAT(a.created_date, '%e %b %y, '), DATE_FORMAT(a.created_date, '%h:%i %p')) AS time," +
                        " b.wvalue AS identifierValue,SUBSTRING(b.displayName, 1, 1) AS first_letter, b.displayName,c.document_name,c.document_path" +
                        " FROM crm_trn_twhatsappmessages a LEFT JOIN crm_smm_whatsapp b ON b.id = a.contact_id" +
                        " left join crm_trn_tfiles c on a.message_id = c.message_gid" +
                        " WHERE  contact_id = '" + lswhatsapp_contactid + "'ORDER BY a.created_date DESC, time DESC";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leadwhatsmessagelist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leadwhatsmessagelist
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
                        values.leadwhatsmessagelist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
                objMySqlDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Whatsapp Lead Message!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
         
        }

        //post indivdual contact message for whatsapp
        public result DaPostLeadWhatsappMessage(leadwhatsappsendmessage values, string user_gid)
        {
            int i = 0;
            Result objsendmessage = new Result();

            result objresult = new result();
            if (values.project_id != null)
            {
                string contactjson = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\"" + values.identifierValue + "\",\"identifierKey\":\"phonenumber\"}]},\"template\":{\"projectId\":\"" + values.project_id + "\",\"version\":\"" + values.version + "\",\"locale\":\"en\"}}";

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                var request = new RestRequest("/workspaces/8f60b013-65ac-4db2-ad01-e9d0ee7c0d5d/channels/c21b849f-5e1a-49d2-a7dc-414a96b19391/messages", Method.POST);
                request.AddHeader("authorization", ConfigurationManager.AppSettings["messagebirdaccesskey"].ToString());
                request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                string waresponse = response.Content;
                objsendmessage = JsonConvert.DeserializeObject<Result>(waresponse);

                try
                {
                     
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
                            objresult.message = "Delivered!";
                        }
                        else
                        {
                            objresult.message = "Failed!";
                        }
                    }
                    else
                    {
                        objresult.status = false;
                        objresult.message = "Sending failed";
                    }
                }
                catch (Exception ex)
                {
                    values.message = "Exception occured while Sending Whatsapp Template!";
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                }
               
            }
            else
            {
                Servicewindow objsendmessage1 = new Servicewindow();

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                var request = new RestRequest("/workspaces/8f60b013-65ac-4db2-ad01-e9d0ee7c0d5d/channels/c21b849f-5e1a-49d2-a7dc-414a96b19391/contacts/" + values.contact_id, Method.GET);
                request.AddHeader("authorization", ConfigurationManager.AppSettings["messagebirdaccesskey"].ToString());
                IRestResponse response1 = client.Execute(request);
                string waresponse1 = response1.Content;
                objsendmessage1 = JsonConvert.DeserializeObject<Servicewindow>(waresponse1);
                if (objsendmessage1.serviceWindowExpireAt != null)
                {
                    string contactjson = "{\"receiver\":{\"contacts\":[{\"identifierValue\":\"" + values.identifierValue + "\",\"identifierKey\":\"phonenumber\"}]},\"body\":{\"type\":\"text\",\"text\":{\"text\":\"" + values.sendtext + "\"}}}";

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var client1 = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                    var request1 = new RestRequest("/workspaces/8f60b013-65ac-4db2-ad01-e9d0ee7c0d5d/channels/c21b849f-5e1a-49d2-a7dc-414a96b19391/messages", Method.POST);
                    request.AddHeader("authorization", ConfigurationManager.AppSettings["messagebirdaccesskey"].ToString());
                    request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                    IRestResponse response2 = client.Execute(request);
                    string waresponse = response2.Content;
                    objsendmessage = JsonConvert.DeserializeObject<Result>(waresponse);

                    try
                    {
                         
                        if (response1.StatusCode == HttpStatusCode.Accepted)
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
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                objresult.status = true;
                                objresult.message = "Delivered!";
                            }
                            else
                            {
                                objresult.message = "Failed!";
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
                        values.message = "Exception occured while Sending Whatsapp!";
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                    }

                }
                else
                {
                    objresult.status = false;
                    objresult.message = "Service Window closed";
                }
            }

            return objresult;
        }
        //get indivdual email details based on lead email
        public void DaGetEmailSendDetails(MdlLeadbank360 values, string leadbank_gid)
        {
            try
            {
                 
                msSQL = " select  email from crm_trn_tleadbank a left join crm_trn_tleadbankcontact b on b.leadbank_gid=a.leadbank_gid where b.leadbank_gid='" + leadbank_gid + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows)
                {
                    lslead_email = objMySqlDataReader["email"].ToString();

                }
                 
                msSQL = " select mailmanagement_gid, sub, body, created_by, created_date, image_path, from_mail, to_mail, transmission_id, bcc, cc, reply_to, status_delivery, status_open, status_click, scheduled_time, temp_mail_gid, schedule_id from crm_smm_mailmanagement where to_mail = '" + lslead_email + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leademailsendlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leademailsendlist
                        {
                            mailmanagement_gid = dt["mailmanagement_gid"].ToString(),
                            sub = dt["sub"].ToString(),
                            body = dt["body"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            image_path = dt["image_path"].ToString(),
                            from_mail = dt["from_mail"].ToString(),
                            to_mail = dt["to_mail"].ToString(),
                            transmission_id = dt["transmission_id"].ToString(),
                            bcc = dt["bcc"].ToString(),
                            cc = dt["cc"].ToString(),
                            reply_to = dt["reply_to"].ToString(),
                            status_delivery = dt["status_delivery"].ToString(),
                            status_open = dt["status_open"].ToString(),
                            status_click = dt["status_click"].ToString(),
                            scheduled_time = dt["scheduled_time"].ToString(),
                            temp_mail_gid = dt["temp_mail_gid"].ToString(),
                            schedule_id = dt["schedule_id"].ToString(),


                        });
                        values.leademailsendlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
                objMySqlDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Email Send Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }        
        }
        //Post indivdual text email based on lead email
        public void DaLeadMailSend(leadmailsummary_list values, string user_gid, result objResult)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient("https://api.sparkpost.com");
                var request = new RestRequest("/api/v1/transmissions", Method.POST);
                request.AddHeader("Authorization", "14e9f31c9e5002fb9dcf7e28b89f7d0d92759a4c");
                request.AddHeader("Content-Type", "application/json");
                var body = "{\"options\":{\"open_tracking\":true,\"click_tracking\":false},\"recipients\":[{\"address\":{\"email\":" + "\"" + values.to + "\"" + "}},{\"address\":{\"email\":" + "\"" + values.cc + "\"" + ",\"header_to\":" + "\"" + values.to + "\"" + "}},{\"address\":{\"email\":" + "\"" + values.bcc + "\"" + ",\"header_to\":" + "\"" + values.to + "\"" + "}}],\"content\":{\"from\":" + "\"" + values.mail_from + "\"" + ",\"headers\":{\"CC\":" + "\"" + values.cc + "\"" + "},\"subject\":" + "\"" + values.sub + "\"" + ",\"reply_to\":" + "\"" + values.reply_to + "\"" + ",\"html\":" + "\"" + values.body.Replace("\"", "\\\"") + "\"" + "}}";
                var body_content = JsonConvert.DeserializeObject(body);
                request.AddParameter("application/json", body_content, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                string errornetsuiteJSON = response.Content;
                sendmail_list objMdlMailCampaignResponse = new sendmail_list(); ;
                objMdlMailCampaignResponse = JsonConvert.DeserializeObject<sendmail_list>(errornetsuiteJSON);


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // Insert email details into the database
                    msGetGid = objcmnfunctions.GetMasterGID("MILC");
                    msSQL = "INSERT INTO crm_smm_mailmanagement (" +
                           "mailmanagement_gid, " +
                        "from_mail, " +
                        "to_mail, " +
                        "sub, " +
                        "transmission_id, " +
                        "bcc, " +
                        "cc, " +
                        "reply_to, " +
                        "body, " +
                         " created_by, " +
                        "created_date) " +
                        "VALUES (" +
                        "'" + msGetGid + "', " +
                        "'" + values.mail_from + "', " +
                        "'" + values.to + "', " +
                        "'" + values.sub + "', " +
                        "'" + objMdlMailCampaignResponse.results.id + "', " +
                        "'" + values.bcc + "', " +
                        "'" + values.cc + "', " +
                        "'" + values.reply_to + "', " +
                        "'" + values.body + "', " +
                          "'" + user_gid + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        objResult.status = true;
                        objResult.message = "Mail Send Successfully !!";
                    }
                    else
                    {
                        objResult.status = false;
                        objResult.message = "Error While Sending Mail !!";
                    }
                }

            }
            catch (Exception ex)
            {
                objResult.status = false;
                //objResult.message = ex.ToString();
                values.message = "Exception occured while Sending Lead Mail!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        //Post indivdual text with attachment email based on lead email
        public void DaLeadMailUpload(HttpRequest httpRequest, string user_gid, result objResult)
        {
            HttpFileCollection httpFileCollection;
            HttpPostedFile httpPostedFile;
            string basecode = httpRequest.Form[0];
            string mail_from = httpRequest.Form[1];
            string sub = httpRequest.Form[2];
            string to = httpRequest.Form[3];
            string body = httpRequest.Form[4];
            string bcc = httpRequest.Form[5];
            string cc = httpRequest.Form[6];
            string reply_to = httpRequest.Form[7];
            httpFileCollection = httpRequest.Files;
            string lsfilepath = string.Empty;
            string document_gid = string.Empty;
            string lspath, lspath1;
            string lscompany_code = string.Empty;
            msSQL = " SELECT a.company_code FROM adm_mst_tcompany a ";
            lscompany_code = objdbconn.GetExecuteScalar(msSQL);
            lspath = ConfigurationManager.AppSettings["imgfile_path"] + "/erpdocument" + "/" + lscompany_code + "/" + "Mail/post/" + DateTime.Now.Year + "/" + DateTime.Now.Month;
            {
                if ((!System.IO.Directory.Exists(lspath)))
                    System.IO.Directory.CreateDirectory(lspath);
            }
            try
            {
                if (httpRequest.Files.Count > 0)
                {
                    string lsfirstdocument_filepath = string.Empty;
                    httpFileCollection = httpRequest.Files;
                    for (int i = 0; i < httpFileCollection.Count; i++)
                    {
                        MemoryStream ms = new MemoryStream();
                        string msdocument_gid = objcmnfunctions.GetMasterGID("MILC");
                        httpPostedFile = httpFileCollection[i];
                        string name = httpPostedFile.FileName;
                        string type = httpPostedFile.ContentType;
                        string apibasecode = "data:" + type + ";base64,";
                        basecode = basecode.Substring(apibasecode.Length);
                        string lsfile_gid = msdocument_gid;
                        string lscompany_document_flag = string.Empty;
                        name = Path.GetExtension(name).ToLower();
                        lsfile_gid = lsfile_gid + name;
                        Stream ls_readStream;
                        ls_readStream = httpPostedFile.InputStream;
                        ls_readStream.CopyTo(ms);

                        lspath = ConfigurationManager.AppSettings["imgfile_path"] + "/erpdocument" + "/" + lscompany_code + "/" + "Mail/post/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        string status;
                        status = objcmnfunctions.uploadFile(lspath + msdocument_gid, name);

                        ms.Close();
                        lspath = "erpdocument" + "/" + lscompany_code + "/" + "Mail/post/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        lspath1 = "/assets/media/images/erpdocument" + "/" + lscompany_code + "/" + "Mail/post/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + name;

                        string final_path = ConfigurationManager.AppSettings["imgfile_path"] + lspath + msdocument_gid + name;


                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        var client = new RestClient("https://api.sparkpost.com");
                        var request = new RestRequest("/api/v1/transmissions", Method.POST);
                        request.AddHeader("Authorization", "14e9f31c9e5002fb9dcf7e28b89f7d0d92759a4c");
                        request.AddHeader("Content-Type", "application/json");
                        var bodies = "{\"options\":{\"open_tracking\":true,\"click_tracking\":false},\"recipients\":[{\"address\":{\"email\":" + "\"" + to + "\"" + "}},{\"address\":{\"email\":" + "\"" + cc + "\"" + ",\"header_to\":" + "\"" + to + "\"" + "}},{\"address\":{\"email\":" + "\"" + bcc + "\"" + ",\"header_to\":" + "\"" + to + "\"" + "}}],\"content\":{\"from\":" + "\"" + mail_from + "\"" + ",\"headers\":{\"CC\":" + "\"" + cc + "\"" + "},\"subject\":" + "\"" + sub + "\"" + ",\"reply_to\":" + "\"" + reply_to + "\"" + ",\"html\":" + "\"" + body.Replace("\"", "\\\"") + "\"" + ",\"attachments\":[{\"name\":" + "\"" + httpPostedFile.FileName + "\"" + ",\"type\":" + "\"" + type + "\"" + ",\"data\":" + "\"" + basecode + "\"" + "}]}}";
                        var body_content = JsonConvert.DeserializeObject(bodies);
                        request.AddParameter("application/json", body_content, ParameterType.RequestBody);
                        IRestResponse response = client.Execute(request);
                        string errornetsuiteJSON = response.Content;
                        sendmail_list objMdlMailCampaignResponse = new sendmail_list(); ;
                        objMdlMailCampaignResponse = JsonConvert.DeserializeObject<sendmail_list>(errornetsuiteJSON);

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            // Insert email details into the database
                            msGetGid = objcmnfunctions.GetMasterGID("MILC");
                            msSQL = "INSERT INTO crm_smm_mailmanagement (" +
                                 "mailmanagement_gid, " +
                                "from_mail, " +
                                "image_path, " +
                                "to_mail, " +
                                "sub, " +
                                "transmission_id, " +
                                "bcc, " +
                                "cc, " +
                                "reply_to, " +
                                "body, " +
                                 " created_by, " +
                                "created_date) " +
                                "VALUES (" +
                                 "'" + msGetGid + "', " +
                                "'" + mail_from + "', " +
                                "'" + lspath1 + "', " +
                                "'" + to + "', " +
                                "'" + sub + "', " +
                                "'" + objMdlMailCampaignResponse.results.id + "', " +
                                "'" + bcc + "', " +
                                "'" + cc + "', " +
                                "'" + reply_to + "', " +
                                "'" + body + "', " +
                                  "'" + user_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                objResult.status = true;
                                objResult.message = "Mail Send Successfully !!";
                            }
                            else
                            {
                                objResult.status = false;
                                objResult.message = "Error While Send Mail !!";
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                objResult.status = false;
                //objResult.message = ex.ToString();
                objResult.message = "Exception occured while Upload Lead Mail!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + objResult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        //Get sales order details
        public void DaGetLeadOrderDetails(MdlLeadbank360 values, string leadbank_gid)
        {

            try
            {
                 
                msSQL = " select customer_gid from crm_trn_tleadbank where leadbank_gid  = '" + leadbank_gid + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows)
                {
                    lscustomer_gid = objMySqlDataReader["customer_gid"].ToString();

                }
                msSQL = " select distinct  k.leadbank_gid,j.leadbankcontact_gid,i.lead2campaign_gid,a.salesorder_gid, a.so_referencenumber, date_format(a.salesorder_date,'%d-%m-%Y') as salesorder_date,c.user_firstname,a.so_type,a.currency_code, " +
                    "  a.customer_contact_person, a.salesorder_status,a.currency_code, " +
                    " case when a.grandtotal_l ='0.00' then format(a.Grandtotal,2) else format(a.grandtotal_l,2) end as Grandtotal," +
                    " a.customer_name, " +
                    " case when a.customer_email is null then concat(e.customercontact_name,'/',e.mobile,'/',e.email) " +
                    " when a.customer_email is not null then concat(a.customer_contact_person,' / ',a.customer_mobile,' / ',a.customer_email) end as contact,invoice_flag " +
                    "  from smr_trn_tsalesorder a " +
                    " left join crm_mst_tcustomer d on a.customer_gid=d.customer_gid " +
                    " left join crm_mst_tcustomercontact e on d.customer_gid=e.customer_gid " +
                    " left join crm_trn_tleadbank k on a.customer_gid=k.customer_gid " +
                    " left join crm_trn_tlead2campaign i on i.leadbank_gid = k.leadbank_gid" +
                    " left join crm_trn_tleadbankcontact j on j.leadbank_gid = k.leadbank_gid" +
                    " left join hrm_mst_temployee b on b.employee_gid=a.created_by " +
                    " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code " +
                    " left join adm_mst_tuser c on b.user_gid= c.user_gid " +
                    " where 1=1  and k.leadbank_gid ='" + leadbank_gid + "' order by  a.salesorder_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leadorderdetailslist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leadorderdetailslist
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            lead2campaign_gid = dt["leadbank_gid"].ToString(),
                            leadbankcontact_gid = dt["leadbankcontact_gid"].ToString(),
                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            so_referenceno1 = dt["so_referencenumber"].ToString(),
                            salesorder_date = dt["salesorder_date"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            //so_typecurrency_code = dt["so_typecurrency_code"].ToString(),
                            customer_contact_person = dt["customer_contact_person"].ToString(),
                            salesorder_status = dt["salesorder_status"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            Grandtotal = dt["Grandtotal"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            contact = dt["contact"].ToString(),
                            invoice_flag = dt["invoice_flag"].ToString(),

                        });
                        values.leadorderdetailslist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
                objMySqlDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Lead Order Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }         
        }
        //Get sales Quotation details
        public void DaGetLeadQuotationDetails(MdlLeadbank360 values, string leadbank_gid)
        {

            try
            {
                 
                msSQL = "select customer_gid from crm_trn_tleadbank where leadbank_gid='" + leadbank_gid + "'";
                lscustomer_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select distinct i.lead2campaign_gid,j.leadbankcontact_gid,d.leadbank_gid,a.quotation_gid, a.quotation_referenceno1, date_format(a.quotation_date,'%d-%m-%Y') as quotation_date,c.user_firstname,a.quotation_type,a.currency_code, " +
                     " case when a.grandtotal_l ='0.00' then format(a.Grandtotal,2) else format(a.grandtotal_l,2) end as Grandtotal," +
                     " a.customer_name," +
                     " a.customer_contact_person, a.quotation_status,a.enquiry_gid, " +
                     " case when a.contact_mail is null then concat(e.leadbankcontact_name,'/',e.mobile,'/',e.email) " +
                     " when a.contact_mail is not null then concat(a.customer_contact_person,' / ',a.contact_no,' / ',a.contact_mail) end as contact " +
                     " from smr_trn_treceivequotation a " +
                     " left join hrm_mst_temployee b on b.employee_gid=a.created_by " +
                     " left join adm_mst_tuser c on b.user_gid= c.user_gid " +
                     " left join crm_trn_tleadbank d on d.leadbank_gid=a.customer_gid " +
                     " left join crm_trn_tlead2campaign i on i.leadbank_gid = d.leadbank_gid " +
                     " left join crm_trn_tleadbankcontact j on j.leadbank_gid = d.leadbank_gid " +
                     " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code " +
                     " left join crm_trn_tleadbankcontact e on e.leadbank_gid=d.leadbank_gid " +
                     " where d.customer_gid='" + lscustomer_gid + "' order by a.quotation_gid desc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leadquotationdetailslist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leadquotationdetailslist
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            lead2campaign_gid = dt["leadbank_gid"].ToString(),
                            leadbankcontact_gid = dt["leadbankcontact_gid"].ToString(),
                            quotation_gid = dt["quotation_gid"].ToString(),
                            quotation_referenceno1 = dt["quotation_gid"].ToString(),
                            quotation_date = dt["quotation_date"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            quotation_type = dt["quotation_type"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            Grandtotal = dt["Grandtotal"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            customer_contact_person = dt["customer_contact_person"].ToString(),
                            quotation_status = dt["quotation_status"].ToString(),
                            enquiry_gid = dt["enquiry_gid"].ToString(),
                            contact = dt["contact"].ToString(),



                        });
                        values.leadquotationdetailslist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Lead Quotation Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
          
        }
        //Get sales Invoice details
        public void DaGetLeadInvoiceDetails(MdlLeadbank360 values, string leadbank_gid)
        {

            try
            {
                 
                msSQL = " select customer_gid from crm_trn_tleadbank where leadbank_gid  = '" + leadbank_gid + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows)
                {
                    lscustomer_gid = objMySqlDataReader["customer_gid"].ToString();

                }
                msSQL = " select distinct a.invoice_gid, case when a.invoice_reference like '%AREF%' then j.agreement_referencenumber else  " +
                        " cast(concat(s.so_referenceno1,if(s.so_referencenumber<>'',concat(' ',' | ',' ',s.so_referencenumber),'') ) as char)  end as so_referencenumber, " +
                        " a.invoice_refno, " +
                        " a.mail_Status,a.customer_gid,date_format(a.invoice_date, '%d-%m-%Y') as 'invoice_date'," +
                        " a.invoice_reference,a.additionalcharges_amount,a.discount_amount,  " +
                        " CASE when a.payment_flag <> 'PY Pending' then a.payment_flag else a.invoice_flag end as 'overall_status', " +
                        " a.payment_flag,  format(a.initialinvoice_amount,2) as initialinvoice_amount,a.invoice_status,a.invoice_flag,  " +
                        " format(a.invoice_amount,2) as invoice_amount, " +
                        " c.customer_name,a.currency_code,  " +
                        " a.customer_contactnumber  as mobile,a.invoice_from,i.directorder_gid,a.progressive_invoice " +
                        " from rbl_trn_tinvoice a  " +
                        " left join rbl_trn_tinvoicedtl b on a.invoice_gid = b.invoice_gid  " +
                        " left join crm_mst_tcustomer c on a.customer_gid = c.customer_gid  " +
                        " left join crm_trn_tleadbank p on p.customer_gid = c.customer_gid  " +
                        " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code  " +
                        " left join smr_trn_tsalesorder s on a.invoice_reference = s. salesorder_gid  " +
                        " left join crm_trn_tagreement j on j.agreement_gid = a.invoice_reference  " +
                        " left join smr_trn_tdeliveryorder i on s.salesorder_gid=i.salesorder_gid " +
                        " where a. customer_gid='" + lscustomer_gid + "' order by a.invoice_gid desc limit 1";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leadinvoicedetailslist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leadinvoicedetailslist
                        {
                            invoice_gid = dt["invoice_gid"].ToString(),
                            so_referencenumber = dt["so_referencenumber"].ToString(),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            mail_status = dt["mail_Status"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            invoice_reference = dt["invoice_reference"].ToString(),
                            additionalcharges_amount = dt["additionalcharges_amount"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            overall_status = dt["overall_status"].ToString(),
                            payment_flag = dt["payment_flag"].ToString(),
                            initialinvoice_amount = dt["initialinvoice_amount"].ToString(),
                            invoice_status = dt["invoice_status"].ToString(),
                            invoice_flag = dt["invoice_flag"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            mobile = dt["mobile"].ToString(),
                            invoice_from = dt["invoice_from"].ToString(),
                            directorder_gid = dt["directorder_gid"].ToString(),
                            progressive_invoice = dt["progressive_invoice"].ToString(),

                        });
                        values.leadinvoicedetailslist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
                objMySqlDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Lead Invoice Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }            
        }

        //Get Lead count details
        public void DaGetLeadCountDetails(MdlLeadbank360 values, string leadbank_gid)
        {

            try
            {
                 
                msSQL = " select customer_gid from crm_trn_tleadbank where leadbank_gid  = '" + leadbank_gid + "'";
                string lscustomer_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " SELECT id FROM crm_smm_whatsapp WHERE  leadbank_gid =  '" + leadbank_gid + "' ";
                string lscontact_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "SELECT(SELECT COUNT(mailmanagement_gid) FROM crm_smm_mailmanagement WHERE leadbank_gid= '" + leadbank_gid + "') AS mail_count," +
        " (SELECT COUNT(version_id) FROM crm_trn_twhatsappmessages WHERE contact_id = '" + lscontact_gid + "') AS whatsapp_count," +
        " (SELECT COUNT(*) FROM smr_trn_treceivequotation WHERE customer_gid = '" + leadbank_gid + "') AS totalquotation_count," +
        " (SELECT round(SUM(Grandtotal),2) FROM smr_trn_treceivequotation WHERE customer_gid = '" + leadbank_gid + "') AS totalquotation_amount," +
        " (SELECT COUNT(quotation_gid) FROM smr_trn_treceivequotation WHERE quotation_status = 'Quotation Amended' AND customer_gid = '" + leadbank_gid + "') AS quotationcancelled_count," +
        " (SELECT round(SUM(Grandtotal),2) FROM smr_trn_treceivequotation WHERE quotation_status = 'Quotation Amended' AND customer_gid = '" + leadbank_gid + "') AS quotationcancelled_amount," +
        " (SELECT COUNT(*) FROM smr_trn_tsalesenquiry WHERE enquiry_status = 'Quotation Accepted' AND customer_gid = '" + leadbank_gid + "') AS quotation_accepted," +
        " (SELECT round(SUM(potorder_value),2) FROM smr_trn_tsalesenquiry WHERE enquiry_status = 'Quotation Accepted' AND customer_gid = '" + leadbank_gid + "') AS quotationaccepted_amount," +
        " (SELECT COUNT(salesorder_gid) FROM smr_trn_tsalesorder WHERE salesorder_status NOT IN('SO Amended') AND customer_gid = '" + leadbank_gid + "') AS order_count," +
        " (SELECT round(SUM(Grandtotal),2) FROM smr_trn_tsalesorder WHERE salesorder_status NOT IN('SO Amended') AND customer_gid = '" + leadbank_gid + "') AS order_amount," +
        " (SELECT COUNT(*) FROM smr_trn_tsalesorder WHERE salesorder_status = 'Delivery Completed' AND customer_gid = '" + lscustomer_gid + "') AS delivery_count," +
        " (SELECT round(SUM(Grandtotal),2) FROM smr_trn_tsalesorder WHERE salesorder_status = 'Delivery Completed' AND customer_gid = '" + lscustomer_gid + "') AS delivery_amount," +
        " (SELECT COUNT(*) FROM smr_trn_tsalesorder WHERE salesorder_status NOT IN('SO Amended') AND customer_gid = '" + lscustomer_gid + "') -" +
        " (SELECT COUNT(*) FROM smr_trn_tsalesorder WHERE salesorder_status = 'Delivery Completed' AND customer_gid = '" + lscustomer_gid + "') AS deliverypending_count," +
        " (SELECT round(SUM(Grandtotal),2) FROM smr_trn_tsalesorder WHERE salesorder_status NOT IN('SO Amended') AND customer_gid = '" + lscustomer_gid + "') - " +
        " (SELECT round(SUM(Grandtotal),2) FROM smr_trn_tsalesorder WHERE salesorder_status = 'Delivery Completed' AND customer_gid = '" + lscustomer_gid + "') AS deliverypending_amount," +
        " (select  COUNT(*) from rbl_trn_tinvoice where customer_gid= '" + lscustomer_gid + "') AS invoice_count," +
        " (select round(SUM(invoice_amount),2) from rbl_trn_tinvoice where customer_gid= '" + lscustomer_gid + "') AS invoice_amount," +
        " (SELECT COUNT(*) FROM rbl_trn_tinvoice WHERE invoice_status = 'payment done' AND customer_gid ='" + lscustomer_gid + "') AS paymentreceived_count," +
        " (SELECT round(SUM(invoice_amount),2) FROM rbl_trn_tinvoice WHERE invoice_status = 'payment done' AND customer_gid = '" + lscustomer_gid + "') AS paymentreceived_amount," +
        " (SELECT COUNT(invoice_gid) FROM rbl_trn_tinvoice WHERE payment_amount = '0.00' AND invoice_status = 'Payment Pending' AND customer_gid = '" + lscustomer_gid + "') AS paymentpending_count," +
        " (SELECT round(SUM(invoice_amount),2) FROM rbl_trn_tinvoice WHERE payment_amount = '0.00' AND invoice_status = 'Payment Pending' AND customer_gid ='" + lscustomer_gid + "') AS paymentpending_amount";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leadcountdetails>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leadcountdetails
                        {
                            whatsappcampaign_count = dt["whatsapp_count"].ToString(),
                            mail_count = dt["mail_count"].ToString(),
                            totalquotation_count = dt["totalquotation_count"].ToString(),
                            totalquotation_amount = dt["totalquotation_amount"].ToString(),
                            quotationaccepted_count = dt["quotation_accepted"].ToString(),
                            quotationaccepted_amount = dt["quotationaccepted_amount"].ToString(),
                            quotationdropped_count = dt["quotationcancelled_count"].ToString(),
                            quotationdropped_amount = dt["quotationcancelled_amount"].ToString(),
                            totalorder_count = dt["order_count"].ToString(),
                            totalorder_amount = dt["order_amount"].ToString(),
                            delevery_count = dt["delivery_count"].ToString(),
                            delevery_amount = dt["delivery_amount"].ToString(),
                            orderpending_count = dt["deliverypending_count"].ToString(),
                            orderpending_amount = dt["deliverypending_amount"].ToString(),
                            totalinvoice_count = dt["invoice_count"].ToString(),
                            totalinvoice_amount = dt["invoice_amount"].ToString(),
                            paymentreceived_count = dt["paymentreceived_count"].ToString(),
                            paymentreceived_amount = dt["paymentreceived_amount"].ToString(),
                            paymentpending_count = dt["paymentpending_count"].ToString(),
                            paymentpending_amount = dt["paymentpending_amount"].ToString(),
                        });
                        values.leadcountdetails = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Lead Count Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }         
        }

        //Get Lead Document details
        public void DaGetLeadDocumentDetails(MdlLeadbank360 values, string leadbank_gid)
        {

            try
            {
                 
                msSQL = "Select document_gid,document_title,document_upload,leadbank_gid ,document_type, remarks,date_format(created_date,'%d-%m-%Y') as created_date" +
              " from crm_trn_tdocument where leadbank_gid='" + leadbank_gid + "' and document_type='mylead'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leaddocumentdetails>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leaddocumentdetails
                        {
                            document_gid = dt["document_gid"].ToString(),
                            document_title = dt["document_title"].ToString(),
                            document_upload = dt["document_upload"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            document_type = dt["document_type"].ToString(),
                            created_date = dt["created_date"].ToString(),


                        });
                        values.leaddocumentdetails = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Document Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        //Document Upload 
        public void DaLeadDocumentUpload(HttpRequest httpRequest, string user_gid)
        {
            result objresult = new result();
            try
            {
                // Access form data using updated structure
                string leadbank_gid = httpRequest.Form["leadbank_gid"];
                string document_title = httpRequest.Form["document_title"];
                string remarks = httpRequest.Form["remarks"];
                string lscompany_code = string.Empty;
                string lspath, lspath1;

                msSQL = " SELECT a.company_code FROM adm_mst_tcompany a ";
                lscompany_code = objdbconn.GetExecuteScalar(msSQL);
                HttpFileCollection httpFileCollection;
                HttpPostedFile httpPostedFile;
                MemoryStream ms = new MemoryStream();
                lspath = ConfigurationManager.AppSettings["upload_file"] + "erpdocument" + "/" + lscompany_code + "/" + "LeadDocuments/upload/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";

                {
                    if ((!System.IO.Directory.Exists(lspath)))
                        System.IO.Directory.CreateDirectory(lspath);
                }

                if (httpRequest.Files.Count > 0)
                {
                    string lsfirstdocument_filepath = string.Empty;
                    httpFileCollection = httpRequest.Files;
                    for (int i = 0; i < httpFileCollection.Count; i++)
                    {
                        string msdocument_gid = objcmnfunctions.GetMasterGID("BDNP");
                        httpPostedFile = httpFileCollection[i];
                        string FileExtension = httpPostedFile.FileName;
                        //string lsfile_gid = msdocument_gid + FileExtension;   
                        string lsfile_gid = msdocument_gid;
                        string lscompany_document_flag = string.Empty;
                        FileExtension = Path.GetExtension(FileExtension).ToLower();
                        lsfile_gid = lsfile_gid + FileExtension;
                        Stream ls_readStream;
                        ls_readStream = httpPostedFile.InputStream;
                        ls_readStream.CopyTo(ms);

                        lspath = ConfigurationManager.AppSettings["upload_file"] + "/erpdocument" + "/" + lscompany_code + "/" + "LeadDocuments/upload/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        string status;
                        status = objcmnfunctions.uploadFile(lspath + msdocument_gid, FileExtension);

                        //bool status1;
                        //status1 = objcmnfunctions.UploadStream(msdocument_gid + FileExtension, FileExtension, ms);
                        //string local_path = "E:/Angular15/AngularUI/src";
                        ms.Close();
                        //lspath = "LeadDocuments/upload/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        //lspath1 = "/erpdocument" + "/" + lscompany_code + "/" + "LeadDocuments/upload/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension;

                        lspath = "assets/media/images/erpdocument" + "/" + lscompany_code + "/" + "LeadDocuments/upload/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension; ; ;
                        string final_path = lspath + msdocument_gid + FileExtension;

                        msSQL = " insert into crm_trn_tdocument( " +
                               " document_gid," +
                               " leadbank_gid," +
                               " document_title, " +
                               " document_type, " +
                               " remarks," +
                               " document_upload," +
                               " created_by," +
                               " created_date" +
                               " )values( " +
                               "'" + msdocument_gid + "'," +
                               "'" + leadbank_gid + "'," +
                               "'" + document_title + "'," +
                               "'mylead'," +
                               "'" + remarks + "'," +
                               "'" + lspath + "', " +
                               "'" + user_gid + "'," +
                               "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            objresult.status = true;
                            objresult.message = "Document Uploaded Successfully";
                        }
                        else
                        {
                            objresult.status = false;
                            objresult.message = "Error while uploading document!!";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objresult.status = false;
                //objresult.message = "Exception occured while uploading document!";
                objresult.message = "Exception occured while uploading document!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + objresult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        //Document download
        public void DaLeadDocumentdownload(string document_gid, MdlLeadbank360 values)
        {

            try
            {
                 
                msSQL = "SELECT document_upload, document_title FROM crm_trn_tdocument WHERE document_gid = '" + document_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<document_download>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new document_download
                        {
                            document_upload = dt["document_upload"].ToString(),
                            document_title = dt["document_title"].ToString()
                        });
                    }
                    values.document_download = getModuleList;
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Lead Document Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }           
        }

        //Notes summary
        public void DaGetNotesSummary(MdlLeadbank360 values, string leadbank_gid)
        {
            try
            {
                 

                msSQL = "select leadbank_gid,internal_notes from crm_trn_tlead2campaign where leadbank_gid='" + leadbank_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<notes>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new notes
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            internal_notes = dt["internal_notes"].ToString(),
                        });
                        values.notes = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Notes Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        //Upload Notes details
        public void DaLeadNotesUpload(notes values, string user_gid, result objResult)
        {
            try
            {
                 
                msSQL = "update crm_trn_tlead2campaign set internal_notes = '" + values.internalnotestext_area + "' " +
                   "where leadbank_gid = '" + values.leadgig + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    objResult.status = true;
                    objResult.message = "Notes Updated Successfully";
                }
                else
                {
                    objResult.status = false;
                    objResult.message = "Error while updating Notes!!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Uploading Lead Notes!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }  
        }
        // Lead Basic Details
        public void DaGetLeadBasicDetails(MdlLeadbank360 values, string leadbank_gid)
        {

            try
            {
                 
                msSQL = " select a.leadbank_gid,a.customer_gid,a.leadbank_name, date_format(a.created_date, '%e %b %Y') as created_date," +
                   " a.customer_type,b.leadbankcontact_name,b.email,b.mobile from crm_trn_tleadbank a " +
                " left join crm_trn_tleadbankcontact b on b.leadbank_gid = a.leadbank_gid where a.leadbank_gid = '" + leadbank_gid + "'";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var leadbasicdetails = new List<leadbasicdetails_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        leadbasicdetails.Add(new leadbasicdetails_list
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                            mobile = dt["mobile"].ToString(),
                            email = dt["email"].ToString(),
                            leadbankcontact_name = dt["leadbankcontact_name"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            customer_type = dt["customer_type"].ToString()
                        });
                        values.leadbasicdetails_list = leadbasicdetails;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Lead Basic Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }           
        }
        //edit contact details//
        public void DaGetupdatecontactdetails(contactedit_list values)
        {

            try
            {
                 
                msSQL = "select customer_gid from crm_trn_tleadbank where leadbank_gid='" + values.leadbank_gid + "'";
                lscustomer_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " update  crm_trn_tleadbankcontact set " +
                        " leadbankcontact_name = '" + values.displayName + "'," +
                        " mobile = '" + values.mobile.e164Number + "'," +
                        " email = '" + values.email + "'," +
                        " address1 = '" + values.address1 + "'" +
                        " where leadbank_gid = '" + values.leadbank_gid + "'";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                if (lscustomer_gid != null && lscustomer_gid.Trim() != "")
                {
                    msSQL = "update crm_mst_tcustomercontact set" +
                      " customercontact_name = '" + values.displayName + "'," +
                      " mobile = '" + values.mobile.e164Number + "'," +
                      " email =  '" + values.email + "'," +
                      " address1 = '" + values.address1 + "'" +
                      " where customer_gid = '" + lscustomer_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Update Customer Contact Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

            if (mnResult == 1)
            {
              
                Rootobject objRootobject = new Rootobject();
                string contactjson = "{\"displayName\":\"" + values.displayName + "\",\"identifiers\":[{\"key\":\"phonenumber\",\"value\":\"" + values.mobile.e164Number + "\"}],\"firstName\":\"" + values.displayName + "\",\"lastName\":\"" + values.displayName + "\"}";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                var request = new RestRequest(ConfigurationManager.AppSettings["messagebirdcontact"].ToString(), Method.POST);
                request.AddHeader("authorization", ConfigurationManager.AppSettings["messagebirdaccesskey"].ToString());
                request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                var responseoutput = response.Content;
                objRootobject = JsonConvert.DeserializeObject<Rootobject>(responseoutput);

                try
                {
                     
                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        msSQL = "insert into crm_smm_whatsapp(leadbank_gid,id,wkey,wvalue,displayName,created_date)values(" +
                                " '" + values.leadbank_gid + "'," +
                                "'" + objRootobject.id + "'," +
                                "'phonenumber'," +
                                "'" + values.mobile.e164Number + "'," +
                                "'" + values.displayName + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            msSQL = "update crm_trn_tleadbank set wh_flag = 'Y', wh_id = '" + objRootobject.id + "' where leadbank_gid = '" + values.leadbank_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        }

                    }
                }
                catch (Exception ex)
                {
                    values.message = "Exception occured while Updating Contact Details in Whatsapp!";
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                }

               
                values.status = true;
                values.message = "Contact Updated Successfully !!";
            }
          
            else
            {
                values.status = false;
                values.message = "Error while updating contact !!";
            }


        }
        //Get edit contact summary
        public void DaGetEditContactdetails(string leadbank_gid, MdlLeadbank360 values, string user_gid)
        {

            try
            {
                 
                msSQL = "select leadbank_gid,leadbankcontact_name,email,mobile,address1 from crm_trn_tleadbankcontact where leadbank_gid='" + leadbank_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<contactedit_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new contactedit_list
                        {
                            displayName = dt["leadbankcontact_name"].ToString(),
                            email = dt["email"].ToString(),
                            mobile1 = dt["mobile"].ToString(),
                            address1 = dt["address1"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),

                        });
                        values.contactedit_list = getModuleList;
                    }
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Edit Contact Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        //Raise enquiry approval//
        //public void DaGetenquiryapproved(MdlLeadbank360 values, string leadbank_gid)

        //{

        //    msSQL = " Select a.leadbank_gid, a.leadbank_name ," +
        //        " from crm_trn_tleadbank a  " +
        //        " left join crm_mst_tcustomer b on a.customer_gid = b.customer_gid " +
        //        " where b.customer_gid is not null and a.leadbank_gid='" + leadbank_gid + "' and b.status='Active' " +
        //        " order by a.leadbank_name asc ";

        //    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

        //    if (mnResult == 0)
        //    {
        //        values.status = false;
        //        values.message = "Change lead to customer to raise enquiry !!";
        //    }
        //}

            //Get sales Enquiry details
        public void DaGetEnquiryDetails(MdlLeadbank360 values, string leadbank_gid)
        {
            try
            {
                 
                msSQL = "select customer_gid from crm_trn_tleadbank where leadbank_gid='" + leadbank_gid + "'";
                lscustomer_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " Select distinct concat(a.enquiry_gid,' / ',a.enquiry_type) as enquiry_refno, a.enquiry_gid,a.leadbank_gid,n.leadbankcontact_gid,f.lead2campaign_gid," +
                        " date_format(a.enquiry_date, '%d-%m-%Y') as enquiry_date,b.user_firstname,a.customer_name,a.branch_gid," +
                        " a.customer_gid,a.lead_status," +
                        " concat(o.region_name, ' / ', m.leadbank_city, ' / ', m.leadbank_state) as region_name," +
                        " a.enquiry_referencenumber,a.enquiry_status,a.enquiry_type," +
                        " concat(b.user_firstname, ' ', b.user_lastname) as campaign,a.enquiry_remarks," +
                        " a.contact_person,a.contact_email,a.contact_address," +
                        " case when a.contact_person is null then concat(n.leadbankcontact_name,' / ',n.mobile,' / ',n.email)" +
                        " when a.contact_person is not null then concat(a.contact_person,' / ',a.contact_number,' / ',a.contact_email) end as contact_details," +
                        " r.leadstage_name,a.enquiry_type from smr_trn_tsalesenquiry a" +
                        " left join crm_trn_tleadbank m on m.customer_gid = a.customer_gid" +
                        " left join crm_trn_tleadbankcontact n on n.leadbank_gid = a.leadbank_gid" +
                        " left join crm_mst_tregion o on m.leadbank_region = o.region_gid" +
                        " left join crm_trn_tenquiry2campaign p on p.enquiry_gid = a.enquiry_gid" +
                        " left join crm_trn_tlead2campaign f on f.leadbank_gid = a.leadbank_gid" +
                        " left join crm_mst_tleadstage r on r.leadstage_gid = p.leadstage_gid" +
                        " left join smr_trn_tcampaign q on q.campaign_gid = p.campaign_gid" +
                        " left join hrm_mst_temployee d on d.employee_gid = p.assign_to" +
                        " left join adm_mst_tuser b on b.user_gid = d.user_gid" +
                        " where m.customer_gid = '" + lscustomer_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getEnquiryList = new List<Enquiry_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getEnquiryList.Add(new Enquiry_list
                        {
                            enquiry_refno = dt["enquiry_refno"].ToString(),
                            enquiry_gid = dt["enquiry_gid"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            leadbankcontact_gid = dt["leadbankcontact_gid"].ToString(),
                            lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                            enquiry_date = dt["enquiry_date"].ToString(),
                            enquiry_status = dt["enquiry_status"].ToString(),
                            enquiry_type = dt["enquiry_type"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            user_firstname = dt["user_firstname"].ToString()
                        });
                        values.Enquiry_list = getEnquiryList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Enquiry Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }           
        }

        //Get Gid details
        public void DaGetGidDetails(MdlLeadbank360 values, string leadbank_gid)
        {

            try
            {
                 
                msSQL = "select a.leadbank_gid,a.lead2campaign_gid, b.leadbankcontact_gid from crm_trn_tlead2campaign a" +
                   " left join crm_trn_tleadbankcontact b on b.leadbank_gid = a.leadbank_gid" +
                   " where a.leadbank_gid = '" + leadbank_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getGidList = new List<gid_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getGidList.Add(new gid_list
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            leadbankcontact_gid = dt["leadbankcontact_gid"].ToString(),
                            lead2campaign_gid = dt["lead2campaign_gid"].ToString(),

                        });
                        values.gid_list = getGidList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Gid Detail!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }          
        }


        // Add to customer
        public void DaAddtocustomer(MdlLeadbank360 values, string leadbank_gid, string user_gid)
        {

            try
            {
                 
                msSQL = "select customer_gid from crm_trn_tleadbank where leadbank_gid='" + leadbank_gid + "'";
                lscustomer_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select a.leadbank_gid,b.leadbankcontact_gid,a.leadbank_name,a.company_website,a.leadbank_address1," +
                        " a.leadbank_address2,a.leadbank_city,a.leadbank_country,a.leadbank_region,a.leadbank_state,a.leadbank_pin," +
                        " a.customer_type,b.leadbankcontact_name,b.email,b.mobile,b.leadbankbranch_name,b.main_contact,b.designation," +
                        " b.address1,b.address2,b.state,b.city,b.country_gid,b.region_name,b.fax,b.fax_area_code,b.fax_country_code" +
                        " from crm_trn_tleadbank a" +
                        " left join crm_trn_tleadbankcontact b on b.leadbank_gid = a.leadbank_gid" +
                        " where a.leadbank_gid ='" + leadbank_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getGidList = new List<addtocustomer>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getGidList.Add(new addtocustomer
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            leadbankcontact_gid = dt["leadbankcontact_gid"].ToString(),
                            customer_name = dt["leadbank_name"].ToString(),
                            company_website = dt["company_website"].ToString(),
                            customer_address = dt["leadbank_address1"].ToString(),
                            customer_address2 = dt["leadbank_address2"].ToString(),
                            customer_city = dt["leadbank_city"].ToString(),
                            countryname_gid = dt["leadbank_country"].ToString(),
                            region = dt["leadbank_region"].ToString(),
                            customer_state = dt["leadbank_state"].ToString(),
                            customer_pin = dt["leadbank_pin"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            customercontact_name = dt["leadbankcontact_name"].ToString(),
                            email = dt["email"].ToString(),
                            mobile = dt["mobile"].ToString(),
                            customerbranch_name = dt["leadbankbranch_name"].ToString(),
                            main_contact = dt["main_contact"].ToString(),
                            designation = dt["designation"].ToString(),
                            address1 = dt["address1"].ToString(),
                            address2 = dt["address2"].ToString(),
                            state = dt["state"].ToString(),
                            city = dt["city"].ToString(),
                            country_gid = dt["country_gid"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            fax = dt["fax"].ToString(),
                            fax_area_code = dt["fax_area_code"].ToString(),
                            fax_country_code = dt["fax_country_code"].ToString(),

                        });
                        values.addtocustomer = getGidList;
                    }
                }
                result objresult = new result();

                //customer table
                msGetGid = objcmnfunctions.GetMasterGID("CC");
                msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='CC' order by finyear asc limit 0,1 ";
                string lsCode = objdbconn.GetExecuteScalar(msSQL);

                string lscustomer_code = "CC-" + "00" + lsCode;
                //string lscustomercode = "H.Q";
                //string lscustomer_branch = "H.Q";


                foreach (var item in getGidList)
                {
                    msGetGid = objcmnfunctions.GetMasterGID("BCRM");
                    msGetGid1 = objcmnfunctions.GetMasterGID("BCCM");
                    msGetGid2 = objcmnfunctions.GetMasterGID("BLBP");
                    msGetGid3 = objcmnfunctions.GetMasterGID("BLCC");

                    msSQL = " insert into crm_mst_tcustomer (" +
                       " customer_gid," +
                       " customer_id, " +
                       " customer_name, " +
                       " company_website, " +
                       " customer_address, " +
                       " customer_address2," +
                       " customer_city," +
                       " currency_gid," +
                       " customer_country," +
                       " customer_region," +
                       " customer_state," +
                       " gst_number ," +
                       " customer_pin ," +
                       " customer_type ," +
                      " created_by," +
                       "created_date" +
                        ") values (" +
                       "'" + msGetGid + "', " +
                       "'" + lscustomer_code + "'," +
                       "'" + item.customer_name + "'," +
                       "'" + item.company_website + "'," +
                       "'" + item.customer_address + "'," +
                       "'" + item.customer_address2 + "'," +
                       "'" + item.customer_city + "'," +
                       "'" + item.currencyexchange_gid + "'," +
                       "'" + item.country_gid + "'," +
                       "'" + item.region_name + "'," +
                       "'" + item.customer_state + "'," +
                       "'" + item.gst_number + "'," +
                       "'" + item.postal_code + "'," +
                        "'" + item.customer_type + "'," +
                        "'" + user_gid + "'," +
                         "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        msSQL = " insert into crm_mst_tcustomercontact  (" +
                        " customercontact_gid," +
                        " customer_gid," +
                        " customercontact_name, " +
                        " customerbranch_name, " +
                        " email, " +
                        " mobile, " +
                        " main_contact, " +
                        " designation," +
                        " address1," +
                        " address2," +
                        " state," +
                        " city," +
                        " country_gid," +
                        " region," +
                        " fax, " +
                        " zip_code, " +
                        " fax_area_code, " +
                        " fax_country_code," +
                        " gst_number, " +
                        " created_by," +
                        " created_date" +
                        ") values (" +
                        "'" + msGetGid1 + "', " +
                        "'" + msGetGid + "', " +
                        "'" + item.customercontact_name + "'," +
                        "'" + item.customerbranch_name + "'," +
                        "'" + item.email + "'," +
                        "'" + item.mobile + "'," +
                        "'Y'," +
                        "'" + item.designation + "'," +
                        "'" + item.address1 + "'," +
                        "'" + item.address2 + "'," +
                        "'" + item.customer_state + "'," +
                        "'" + item.customer_city + "'," +
                        "'" + item.country_gid + "'," +
                        "'" + item.region_name + "'," +
                        "'" + item.fax + "'," +
                        "'" + item.postal_code + "'," +
                        "'" + item.fax_area_code + "'," +
                        "'" + item.fax_country_code + "'," +
                        "'" + item.gst_number + "'," +
                        "'" + user_gid + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                        msSQL = "  update crm_trn_tleadbank set customer_gid ='" + msGetGid + "' where leadbank_gid = '" + leadbank_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult == 1)
                        {
                            objresult.status = true;
                            objresult.message = "Lead added to customer";
                        }
                        else
                        {
                            objresult.status = false;
                            objresult.message = "Error while adding lead to customer!!";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Purchase Liability Report Chart!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }             
        }

    }
}