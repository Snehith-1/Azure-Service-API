﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ems.crm.Models;
using ems.utilities.Functions;
using System.Data.Odbc;
using System.Data;
using System.Web;
using OfficeOpenXml;
using System.Configuration;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net.Mail;
using RestSharp;
using Newtonsoft.Json;
using System.Web.UI.WebControls;
using System.Web.Http.Results;
using System.Data.SqlTypes;
using static OfficeOpenXml.ExcelErrorValue;
using System.Globalization;
using MySql.Data.MySqlClient;


namespace ems.crm.DataAccess
{
    public class DaFacebook
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        MySqlDataReader objMySqlDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, lsid, lsuser_name, lsfirst_name, lslast_name,
            lsemail, lsbirthday, lsgender, lsage_range, lsprofile_picture, final_path, lshometown_location, lscurrent_location, lsfriends_count , lscaption, lsviews_count, lsfacebookmain_gid, lspost_url, lspost_id, lspost_type;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5, mnResult6, mnResult9;
        public facebooklist DaGetFacebook()
        {

            //var client = new RestClient("https://graph.facebook.com");
            //var request = new RestRequest("/v18.0/me?fields=id%2Cname%2Cabout%2Cage_range%2Cbirthday%2Ceducation%2Cemail%2Cfirst_name%2Cgender%2Chometown%2Clast_name%2Clocation%2Crelationship_status%2Cfriends%2Clikes%7Bemails%2Clink%2Ccreated_time%7D%2Cpicture%2Cposts%7Blink%2Cfull_picture%2Ccreated_time%7D%2Cevents%2Cgroups&access_token=EAALVntBtuioBAPY4qza7LIA2ZAzQQ4HVBzsqMVCrgweHKFLUj8jBymt3b2emLKcyVwosChfRfrDDDsJQlAFzLcvfOLQYBWqdIRAek9OZCPYawIL2LBFgYbrW10U3YYS4gMy8gO5EIEgZArcCGiCTCGcqruFlVAVHKcMn1O9knJDJ5Q5K9PjBQvXhiXNTrYA01eGTMldd3bDReVumUo5Dw6snNZAex0mN7r4pY6BpwpUgPKtNHevb", Method.GET);
            //IRestResponse response = client.Execute(request);
            //Console.WriteLine(response.Content);
            result objresult = new result();
            facebookconfiguration getfacebookcredentials = facebookcredentials();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            string requestAddressURL = "https://graph.facebook.com/v18.0/me?fields=id%2Cname%2Cabout%2Cage_range%2Cbirthday%2Ceducation%2Cemail%2Cfirst_name%2Cgender%2Chometown%2Clast_name%2Clocation%2Crelationship_status%2Cfriends%2Clikes%7Bemails%2Clink%2Ccreated_time%7D%2Cpicture%2Cposts%7Blink%2Cfull_picture%2Ccreated_time%7D%2Cevents%2Cgroups&access_token=EAASJvmulWEEBO1phBE0OnDRstEGiJiYXlHRpULMPUUYOFZBglInN0WZAXU2fR7xhJCu0ukfQCIEcTOKFtlJkKdUwCYO98WL1Fu9LZCZCSmjGRE5qMACZBpYvLZBsJ27BjxWJB4T0N9odzN5LKa2XQkeILFStQxZAw5yMMCCvFJ2s8H5g8V6KNBj61UQ";
            var clientAddress = new RestClient(requestAddressURL);
            var requestAddress = new RestRequest(Method.GET);
            IRestResponse responseAddress = clientAddress.Execute(requestAddress);
            string address_erpid = responseAddress.Content;
            string errornetsuiteJSON = responseAddress.Content;
            facebooklist objMdlFacebookMessageResponse = new facebooklist();
            objMdlFacebookMessageResponse = JsonConvert.DeserializeObject<facebooklist>(errornetsuiteJSON);
            if (responseAddress.StatusCode == HttpStatusCode.OK)
            {
                msSQL = "delete from crm_smm_facebookdetails where  id = '" + objMdlFacebookMessageResponse.id + "' and facebook_type='Account'"  ;
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                { 
                    msSQL = "insert into crm_smm_facebookdetails(" +
                                         "id," +
                                         "user_name," +
                                         "first_name," +
                                         "last_name," +
                                         "email," +
                                         "birthday," +
                                         "gender," +
                                         "age_range," +
                                         "profile_picture," +
                                        "friends_count," +
                                        "hometown_location," +
                                        "current_location," +
                                        "facebook_type," +
                                         "created_date)" +
                                         "values(" +
                                         "'" + objMdlFacebookMessageResponse.id + "'," +
                                         "'" + objMdlFacebookMessageResponse.name + "'," +
                                         "'" + objMdlFacebookMessageResponse.first_name + "'," +
                                        "'" + objMdlFacebookMessageResponse.last_name + "'," +
                                         "'" + objMdlFacebookMessageResponse.email + "'," +
                                         "'" + objMdlFacebookMessageResponse.birthday + "'," +
                                         "'" + objMdlFacebookMessageResponse.gender + "'," +
                                        "'" + objMdlFacebookMessageResponse.age_range.min + "'," +
                                        "'" + objMdlFacebookMessageResponse.picture.data.url + "'," +
                                        "'" + objMdlFacebookMessageResponse.friends.summary.total_count + "'," +
                                         "'" + objMdlFacebookMessageResponse.hometown.name + "'," +
                                        "'" + objMdlFacebookMessageResponse.location.name + "'," +
                                        " 'Account', " +
                                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

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

            }

            return objMdlFacebookMessageResponse;



        }
        public void DaGetFacebookuserdetails(MdlFacebook values)
        {
            try
            {
                 
                msSQL = "select id,user_name,first_name,last_name,email,birthday,gender,age_range,profile_picture,friends_count,hometown_location,current_location from crm_smm_facebookdetails where facebook_type ='Account'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<facebookuser_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new facebookuser_list
                        {
                            id = dt["id"].ToString(),
                            user_name = dt["user_name"].ToString(),
                            first_name = dt["first_name"].ToString(),
                            last_name = dt["last_name"].ToString(),
                            email = dt["email"].ToString(),
                            birthday = dt["birthday"].ToString(),
                            gender = dt["gender"].ToString(),
                            age_range = dt["age_range"].ToString(),
                            profile_picture = dt["profile_picture"].ToString(),
                            friends_count = dt["friends_count"].ToString(),
                            hometown_name = dt["hometown_location"].ToString(),
                            currentlocation_name = dt["current_location"].ToString(),



                        });
                        values.facebookuser_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting facebook user detail!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetPagedetails(MdlFacebook values)
        {
            try
            {
                 
                result result = new result();
                facebookconfiguration getfacebookcredentials = facebookcredentials();

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                string requestAddressURL = "https://graph.facebook.com/v18.0/me?fields=id%2Cname%2Ccategory%2Cfollowers_count%2Clink%2Cpicture%2Cvideos%7Bpermalink_url%2Csource%2Cid%2Cviews%2Cdescription%2Ccreated_time%2Ccomments%7Bid%2Ccreated_time%2Cmessage%7D%7D%2Cposts%7Bfull_picture%2Cpermalink_url%2Cmessage%2Ccreated_time%2Ccomments%7Bid%2Cmessage%2Ccreated_time%7D%7D&access_token=" + getfacebookcredentials.access_token + "";
                var clientAddress = new RestClient(requestAddressURL);
                var requestAddress = new RestRequest(Method.GET);
                IRestResponse responseAddress = clientAddress.Execute(requestAddress);
                string address_erpid = responseAddress.Content;
                string errornetsuiteJSON = responseAddress.Content;
                facebooklist objMdlFacebookMessageResponse = new facebooklist();
                objMdlFacebookMessageResponse = JsonConvert.DeserializeObject<facebooklist>(errornetsuiteJSON);




                if (responseAddress.StatusCode == HttpStatusCode.OK)
                {
                    msSQL = "delete from crm_smm_facebookdetails where  id = '" + objMdlFacebookMessageResponse.id + "' and facebook_type='Page'";
                    mnResult9 = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = "truncate crm_smm_facebookpage ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = "truncate crm_smm_facebookpagedtl";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult9 == 1)
                    {
                        msSQL = "insert into crm_smm_facebookdetails(" +
                                             "id," +
                                             "user_name," +
                                             "profile_picture," +
                                            "friends_count," +
                                            "page_category," +
                                           "page_link," +
                                            "facebook_type," +
                                             "created_date)" +
                                             "values(" +
                                             "'" + objMdlFacebookMessageResponse.id + "'," +
                                             "'" + objMdlFacebookMessageResponse.name + "'," +
                                            "'" + objMdlFacebookMessageResponse.picture.data.url + "'," +
                                            "'" + objMdlFacebookMessageResponse.followers_count + "'," +
                                            "'" + objMdlFacebookMessageResponse.category + "'," +
                                             "'" + objMdlFacebookMessageResponse.link + "'," +
                                            " 'Page', " +
                                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                        mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult1 == 0)
                        {

                            result.message = "Failed!";
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while editing details!! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Facebook/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                            objcmnfunctions.LogForAudit("");
                        }
                    }
                    else
                    {

                        result.message = "Failed!";
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while editing details!! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Facebook/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        objcmnfunctions.LogForAudit("");
                    }
                    if (objMdlFacebookMessageResponse.posts != null)
                    {
                        var imagelist = objMdlFacebookMessageResponse.posts.data.Take(5);
                        foreach (var item in imagelist)
                        {
                            string message = System.Net.WebUtility.HtmlEncode(item.message);

                            Console.WriteLine("Original Emoji: " + item.message);
                            Console.WriteLine("HTML Entity Code: " + message);

                            msGetGid = objcmnfunctions.GetMasterGID("FB");

                            msSQL = "insert into crm_smm_facebookpage(" +
                                  "facebookmain_gid," +
                                 "post_id," +
                                 "post_type," +
                                 "post_url," +
                                 "permalink_url," +
                                 "caption," +
                                 "postcreated_time )" +
                                "values(" +
                                " '" + msGetGid + "'," +
                                "'" + item.id + "'," +
                                " 'Picture', " +
                                "'" + item.full_picture + "'," +
                                "'" + item.permalink_url + "'," +
                                "'" + message + "'," +
                                "'" + item.created_time.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                if (item.comments != null)
                                {

                                    foreach (var items in item.comments.data)
                                    {
                                        string commentmessage = System.Net.WebUtility.HtmlEncode(items.message);

                                        Console.WriteLine("Original Emoji: " + items.message);
                                        Console.WriteLine("HTML Entity Code: " + commentmessage);

                                        msSQL = "insert into crm_smm_facebookpagedtl(" +
                                              "facebookmain_gid," +
                                             "post_id," +
                                             "post_type," +
                                             "commentmessage_id," +
                                             "comment_message," +
                                             "comment_time )" +
                                            "values(" +
                                             " '" + msGetGid + "'," +
                                            "'" + items.id + "'," +
                                            " 'Picture', " +
                                            "'" + items.id + "'," +
                                            "'" + commentmessage + "'," +
                                            "'" + items.created_time.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                                        mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult2 == 0)

                                        {
                                            result.message = "Failed!";
                                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while editing details!! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Facsbook/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                            objcmnfunctions.LogForAudit("");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                result.message = "Failed!";
                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while editing details!! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Fcebook/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                objcmnfunctions.LogForAudit("");
                            }
                        }
                    }

                    if (objMdlFacebookMessageResponse.videos != null)
                    {
                        var videoslist = objMdlFacebookMessageResponse.videos.data.Take(5);

                        foreach (var item in videoslist)
                        {
                            string description = System.Net.WebUtility.HtmlEncode(item.description);

                            Console.WriteLine("Original Emoji: " + item.description);
                            Console.WriteLine("HTML Entity Code: " + description);



                            msGetGid = objcmnfunctions.GetMasterGID("FB");
                            msSQL = "insert into crm_smm_facebookpage(" +
                                 "facebookmain_gid," +
                                 "post_id," +
                                 "post_type," +
                                 "post_url," +
                                 "permalink_url," +
                                 "views_count," +
                                 "caption," +
                                 "postcreated_time )" +
                                "values(" +
                                 " '" + msGetGid + "'," +
                                "'" + item.id + "'," +
                                " 'Videos', " +
                                "'" + item.source + "'," +
                                "'" + item.permalink_url + "'," +
                                "'" + item.views + "'," +
                                "'" + description + "'," +
                                "'" + item.created_time.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                if (item.comments != null)
                                {

                                    foreach (var items in item.comments.data)
                                    {
                                        string comment_message = System.Net.WebUtility.HtmlEncode(items.message);

                                        Console.WriteLine("Original Emoji: " + items.message);
                                        Console.WriteLine("HTML Entity Code: " + comment_message);

                                        msSQL = "insert into crm_smm_facebookpagedtl(" +
                                            "facebookmain_gid," +
                                            "post_id," +
                                             "post_type," +
                                             "commentmessage_id," +
                                             "comment_message," +
                                             "comment_time )" +
                                            "values(" +
                                             " '" + msGetGid + "'," +
                                            "'" + items.id + "'," +
                                            " 'Videos', " +
                                            "'" + items.id + "'," +
                                            "'" + comment_message + "'," +
                                            "'" + items.created_time.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                                        mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult3 == 0)
                                        {
                                            result.message = "Failed!";
                                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while editing details!! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Facebook/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                            objcmnfunctions.LogForAudit("");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                result.message = "Failed!";
                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while editing details!! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Facebook/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                objcmnfunctions.LogForAudit("");
                            }
                        }
                    }


                }
                msSQL = "select a.facebookmain_gid,a.post_id, a.post_url, DATE_FORMAT( a.postcreated_time, '%d-%m-%Y') AS postcreated_time,a.post_type,a.caption,a.views_count , ifnull(count(b.comment_message),0) as comment_message  " +
                        "  from  crm_smm_facebookpage a left join crm_smm_facebookpagedtl b on b.facebookmain_gid=a.facebookmain_gid   group by a.post_id;";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<facebookpage_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new facebookpage_list
                        {
                            facebookmain_gid = dt["facebookmain_gid"].ToString(),
                            post_url = dt["post_url"].ToString(),
                            postcreated_time = dt["postcreated_time"].ToString(),
                            post_type = dt["post_type"].ToString(),
                            caption = dt["caption"].ToString(),
                            views_count = dt["views_count"].ToString(),
                            comment_message = dt["comment_message"].ToString(),
                            post_id = dt["post_id"].ToString()



                        });
                        values.facebookpage_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting page detail";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        //public void DaGetPagesummary(MdlFacebook values)
        //{
        //    msSQL = "select a.facebookmain_gid,a.post_id, a.post_url, DATE_FORMAT( a.postcreated_time, '%d-%m-%Y') AS postcreated_time,a.post_type,a.caption,a.views_count , ifnull(count(b.comment_message),0) as comment_message  " +
        //        "  from  crm_smm_facebookpage a left join crm_smm_facebookpagedtl b on b.facebookmain_gid=a.facebookmain_gid   group by a.post_id;";
                
        //    dt_datatable = objdbconn.GetDataTable(msSQL);

        //    var getModuleList = new List<facebookpage_list>();
        //    if (dt_datatable.Rows.Count != 0)
        //    {
        //        foreach (DataRow dt in dt_datatable.Rows)
        //        {
        //            getModuleList.Add(new facebookpage_list
        //            {
        //                facebookmain_gid = dt["facebookmain_gid"].ToString(),
        //                post_url = dt["post_url"].ToString(),
        //                postcreated_time = dt["postcreated_time"].ToString(),
        //                post_type = dt["post_type"].ToString(),
        //                caption = dt["caption"].ToString(),
        //                views_count = dt["views_count"].ToString(),
        //                comment_message = dt["comment_message"].ToString(),
        //                post_id = dt["post_id"].ToString()



        //            });
        //            values.facebookpage_list = getModuleList;
        //        }
        //    }
        //    dt_datatable.Dispose();
        //}
        public void DaGetPageuserdetails(MdlFacebook values)
        {
            try
            {
                 
                msSQL = "select id,user_name,facebook_type,profile_picture,friends_count,page_category,page_link from crm_smm_facebookdetails where facebook_type ='Page'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<facebookuser_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new facebookuser_list
                        {
                            id = dt["id"].ToString(),
                            user_name = dt["user_name"].ToString(),
                            profile_picture = dt["profile_picture"].ToString(),
                            friends_count = dt["friends_count"].ToString(),
                            page_category = dt["page_category"].ToString(),
                            page_link = dt["page_link"].ToString(),
                            facebook_type = dt["facebook_type"].ToString(),


                        });
                        values.facebookuser_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting user page detail";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        //public void LogForAudit(string strVal)
        //{
        //    try
        //    {
        //        string lspath = ConfigurationManager.AppSettings["file_path"] + "/erpdocument/Facebook/ErrorLog/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
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
        public void DaUploadImage(HttpRequest httpRequest, string user_gid, result objResult)
        {
            facebookconfiguration getfacebookcredentials = facebookcredentials();

            HttpFileCollection httpFileCollection;
            string lsfilepath = string.Empty;
            string lsdocument_gid = string.Empty;
            MemoryStream ms_stream = new MemoryStream();
            string document_gid = string.Empty;
            string lscompany_code = string.Empty;
            HttpPostedFile httpPostedFile;

            string lspath;
            string msGetGid;

            msSQL = " SELECT a.company_code FROM adm_mst_tcompany a ";
            lscompany_code = objdbconn.GetExecuteScalar(msSQL);
            string image_caption = httpRequest.Form[0];
            MemoryStream ms = new MemoryStream();
            try
            {
                if (httpRequest.Files.Count > 0)
                {
                    string lsfirstdocument_filepath = string.Empty;
                    httpFileCollection = httpRequest.Files;
                    for (int i = 0; i < httpFileCollection.Count; i++)
                    {
                        string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
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
                        if (FileExtension == ".jpg" | FileExtension == ".png" | FileExtension == ".jpeg" | FileExtension == ".gif" | FileExtension == ".JPG" | FileExtension == ".JPEG" | FileExtension == ".JIFF" | FileExtension == ".TIFF" | FileExtension == ".PNG" | FileExtension == ".GIF" | FileExtension == ".JFIF" | FileExtension == ".svg" | FileExtension == ".jfif")
                        {


                            bool status1;
                            status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"],
                                "Facebook/" + msdocument_gid + FileExtension,
                                FileExtension, ms);
                            ms.Close();
                            final_path = ConfigurationManager.AppSettings["blob_containername"] + "/Facebook/";
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                            var client = new RestClient("https://graph.facebook.com");
                            var request = new RestRequest("/" + getfacebookcredentials.page_id + "/photos", Method.POST);
                            request.AlwaysMultipartFormData = true;
                            request.AddParameter("access_token", getfacebookcredentials.access_token);
                            string filePath = Path.Combine(ConfigurationManager.AppSettings["blob_imagepath1"],
                                                            final_path, msdocument_gid + FileExtension +
                                                            ConfigurationManager.AppSettings["blob_imagepath2"] + '&' +
                                                            ConfigurationManager.AppSettings["blob_imagepath3"] + '&' +
                                                            ConfigurationManager.AppSettings["blob_imagepath4"] + '&' +
                                                            ConfigurationManager.AppSettings["blob_imagepath5"] + '&' +
                                                            ConfigurationManager.AppSettings["blob_imagepath6"] + '&' +
                                                            ConfigurationManager.AppSettings["blob_imagepath7"] + '&' +
                                                            ConfigurationManager.AppSettings["blob_imagepath8"]);
                            request.AddParameter("url", filePath);
                            // Add other parameters if needed
                            request.AddParameter("message", image_caption);
                            IRestResponse response = client.Execute(request);
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                objResult.status = true;
                                objResult.message = "Posted in Facebook Successfully !!";
                            }
                            else if (response.StatusCode == HttpStatusCode.BadRequest)
                            {
                                objResult.status = false;
                                objResult.message = "Error While Posting in Facebook !!";

                            }
                        }
                        else if (FileExtension == ".mp4" | FileExtension == ".MP4" | FileExtension == ".avi" | FileExtension == ".mkv" | FileExtension == ".wmv" | FileExtension == ".mov" | FileExtension == ".WebM" | FileExtension == ".flv" | FileExtension == ".hevc" | FileExtension == ".vpg")

                        {
                            bool status1;
                            status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"],
                                 "Facebook/" + msdocument_gid + FileExtension,
                                 FileExtension, ms);
                            ms.Close();

                            final_path = ConfigurationManager.AppSettings["blob_containername"] + "/Facebook/";
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                            var client = new RestClient("https://graph.facebook.com");
                            var request = new RestRequest("/" + getfacebookcredentials.page_id + "/videos", Method.POST);
                            request.AlwaysMultipartFormData = true;
                            request.AddParameter("access_token", getfacebookcredentials.access_token);
                            string filePath = Path.Combine(ConfigurationManager.AppSettings["blob_imagepath1"],
                                                            final_path, msdocument_gid + FileExtension +
                                                            ConfigurationManager.AppSettings["blob_imagepath2"] + '&' +
                                                            ConfigurationManager.AppSettings["blob_imagepath3"] + '&' +
                                                            ConfigurationManager.AppSettings["blob_imagepath4"] + '&' +
                                                            ConfigurationManager.AppSettings["blob_imagepath5"] + '&' +
                                                            ConfigurationManager.AppSettings["blob_imagepath6"] + '&' +
                                                            ConfigurationManager.AppSettings["blob_imagepath7"] + '&' +
                                                            ConfigurationManager.AppSettings["blob_imagepath8"]);

                            request.AddParameter("file_url", filePath);
                            request.AddParameter("message", image_caption);
                            IRestResponse response = client.Execute(request);

                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                objResult.status = true;
                                objResult.message = "Posted in Facebook Successfully !!";
                            }
                            else
                            {
                                objResult.status = false;
                                objResult.message = "Error While Posting in Facebook !!";
                            }

                        }


                    }

                }
                else
                {
                    objResult.status = false;
                    objResult.message = "Error While Posting in Facebook !!";
                }
            }
            catch (Exception ex)
            {
                objResult.message = "Exception occured while Uploading Image in Facebook";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + objResult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
            //return true;

        }

        public void DaUploadVideo(HttpRequest httpRequest, string user_gid, string video_caption, result objResult)
        {
            //uploadimageproductcategorycreationSummary objdocumentmodel = new uploadimageproductcategorycreationSummary();
            HttpFileCollection httpFileCollection;
            facebookconfiguration getfacebookcredentials = facebookcredentials();

            string lsfilepath = string.Empty;
            string lsdocument_gid = string.Empty;
            MemoryStream ms_stream = new MemoryStream();
            string document_gid = string.Empty;
            string lscompany_code = string.Empty;
            HttpPostedFile httpPostedFile;

            string lspath;
            string msGetGid;

            msSQL = " SELECT a.company_code FROM adm_mst_tcompany a ";
            lscompany_code = objdbconn.GetExecuteScalar(msSQL);
            MemoryStream ms = new MemoryStream();
            lspath = ConfigurationManager.AppSettings["imgfile_path"] + "/erpdocument" + "/" + lscompany_code + "/" + "Facebook/Videos post/" + DateTime.Now.Year + "/" + DateTime.Now.Month;

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
                        string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
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

                        lspath = ConfigurationManager.AppSettings["imgfile_path"] + "/erpdocument" + "/" + lscompany_code + "/" + "Facebook/Videos post/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        string status;
                        status = objcmnfunctions.uploadFile(lspath + msdocument_gid, FileExtension);

                        ms.Close();
                        lspath = "/assets/images/erpdocument" + "/" + lscompany_code + "/" + "Facebook/Videos post/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";

                        string final_path = ConfigurationManager.AppSettings["imgfile_path"] + lspath + msdocument_gid + FileExtension;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        var client = new RestClient("https://graph.facebook.com");
                        var request = new RestRequest("/" + getfacebookcredentials.page_id + "/videos", Method.POST);
                        request.AlwaysMultipartFormData = true;
                        request.AddParameter("access_token", getfacebookcredentials.access_token);
                        IRestRequest restRequest = request.AddFile("source", final_path);
                        request.AddParameter("message", video_caption);
                        IRestResponse response = client.Execute(request);

                    }
                    objResult.status = true;
                    objResult.message = "Posted in Facebook Successfully !!";
                }

                else
                {
                    objResult.status = false;
                    objResult.message = "Error While Posting in Facebook !!";
                }
            }
            catch (Exception ex)
            {
                objResult.message = "Exception occured while Uploading Video in Facebook ";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + objResult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
            //return true;

        }
        public void DaGetViewFacebook(string facebookmain_gid, mdlFbPostView values)
        {
            try
            {
                 
                int count = 0;
                msSQL = " select post_url,post_id,facebookmain_gid,views_count,caption,post_type,postcreated_time from crm_smm_facebookpage" +
                   "  where facebookmain_gid='" + facebookmain_gid + "' ";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows)
                {
                    values.post_url = objMySqlDataReader["post_url"].ToString();
                    values.post_id = objMySqlDataReader["post_id"].ToString();
                    values.facebookmain_gid = objMySqlDataReader["facebookmain_gid"].ToString();
                    values.views_count = objMySqlDataReader["views_count"].ToString();
                    values.caption = objMySqlDataReader["caption"].ToString();
                    values.post_type = objMySqlDataReader["post_type"].ToString();
                    values.postcreated_time = objMySqlDataReader["postcreated_time"].ToString();

                }
                msSQL = " SELECT b.facebookmain_gid,b.commentmessage_id,b.comment_message,b.comment_time FROM crm_smm_facebookpagedtl b" +
                "  where b.facebookmain_gid='" + facebookmain_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetViewFaceBookSummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetViewFaceBookSummary
                        {
                            comment_time = dt["comment_time"].ToString(),
                            commentmessage_id = dt["commentmessage_id"].ToString(),
                            comment_message = dt["comment_message"].ToString(),
                            facebookmain_gid = dt["facebookmain_gid"].ToString(),
                        });
                        values.GetViewFaceBookSummary = getModuleList;
                        count++;
                    }
                }
                dt_datatable.Dispose();
                values.comments_count = count;
                objMySqlDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting view page";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }         
        }

        public facebookconfiguration facebookcredentials()
        {
            facebookconfiguration getfacebookcredentials = new facebookconfiguration();

            msSQL = " select page_id,access_token from crm_smm_tfacebookservice";
            objMySqlDataReader = objdbconn.GetDataReader(msSQL);
            if (objMySqlDataReader.HasRows == true)
            {
                 
                getfacebookcredentials.page_id = objMySqlDataReader["page_id"].ToString();
                getfacebookcredentials.access_token = objMySqlDataReader["access_token"].ToString();

                 
            }
            else
            {

            }
                 
            return getfacebookcredentials;
            objMySqlDataReader.Close();
        }


    }
}