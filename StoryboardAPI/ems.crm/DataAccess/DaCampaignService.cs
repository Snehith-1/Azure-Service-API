﻿using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.crm.Models;
using MySql.Data.MySqlClient;


namespace ems.crm.DataAccess
{
    public class DaCampaignService
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        MySqlDataReader objMySqlDataReader;
        DataTable dt_datatable;
        int mnResult;
        string msGetGid, msGetGid1;

        public void DaGetWhatsappSummary(MdlCampaignService values)
        {
            msSQL = " select s_no,workspace_id,channel_id,access_token,channelgroup_id,mobile_number, " +
                    " channel_name,created_by,created_date from crm_smm_whatsapp_service limit 1";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getmodulelist = new List<campaignservice_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getmodulelist.Add(new campaignservice_list
                    {
                        s_no = dt["s_no"].ToString(),
                        workspace_id = dt["workspace_id"].ToString(),
                        channel_id = dt["channel_id"].ToString(),
                        access_token = dt["access_token"].ToString(),
                        channelgroup_id = dt["channelgroup_id"].ToString(),
                        mobile_number = dt["mobile_number"].ToString(),
                        channel_name = dt["channel_name"].ToString(),
                        created_by = dt["created_by"].ToString(),
                        created_date = dt["created_date"].ToString(),
                    });
                    values.campaignservice_list = getmodulelist;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaUpdateWhatsappService(string user_gid, campaignservice_list values)

        {

            if (values.whatsapp_id == null || values.whatsapp_id == "")
            {
                msSQL = " insert into crm_smm_whatsapp_service(" +
                 " workspace_id," +
                 " channel_id," +
                 " access_token," +
                 " mobile_number," +
                 " channel_name," +
                 " channelgroup_id," +
                 " created_by," +
                 " created_date)" +
                 " values(" +
                  "'" + values.workspace_id + "'," +
                  "'" + values.channel_id + "'," +
                  "'AccessKey " + values.whatsapp_accesstoken + "'," +
                  "'" + values.mobile_number + "'," +
                  "'" + values.channel_name + "'," +
                  "'" + values.channelgroup_id + "'," +
                  "'" + user_gid + "'," +
                  "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                  mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "\"Whatsapp Credentials Updated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Whatsapp Credentials !!";
                }

            }
            else
            {
                msSQL = " update  crm_smm_whatsapp_service set " +

                " workspace_id = '" + values.workspace_id + "'," +
                " channel_id = '" + values.channel_id + "'," +
                " access_token = '" + values.whatsapp_accesstoken + "'," +
                " mobile_number = '" + values.mobile_number + "'," +
                " channel_name = '" + values.channel_name + "'," +
                " channelgroup_id =  '" + values.channelgroup_id + "'," +
                " updated_by = '" + user_gid + "'," +

                " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where s_no='" + values.whatsapp_id + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Whatsapp Credentials Updated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Whatsapp Credentials !!";
                }
            }
        }

        public void DaGetShopifySummary(MdlCampaignService values)
        {
            msSQL = " select s_no,access_token,shopify_store_name,store_month_year,created_by,created_date" +
                    " from crm_smm_shopify_service limit 1 ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getmodulelist = new List<shopifycampaignservice_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getmodulelist.Add(new shopifycampaignservice_list
                    {
                        s_no = dt["s_no"].ToString(),
                        shopify_access_token = dt["access_token"].ToString(),
                        shopify_store_name = dt["shopify_store_name"].ToString(),
                        store_month_year = dt["store_month_year"].ToString(),
                        shopify_created_by = dt["created_by"].ToString(),
                        shopify_created_date = dt["created_date"].ToString(),
                    });
                    values.shopifycampaignservice_list = getmodulelist;
                }
            }
            dt_datatable.Dispose();
        }

        public void DaGetMailSummary(MdlCampaignService values)
        {
            msSQL = " select access_token,base_url,created_by,s_no,created_date,receiving_domain," +
                    " sending_domain,email_username from crm_smm_mail_service limit 1 ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getmodulelist = new List<mailcampaignservice_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getmodulelist.Add(new mailcampaignservice_list
                    {
                        mail_access_token = dt["access_token"].ToString(),
                        mail_base_url = dt["base_url"].ToString(),
                        mail_created_by = dt["created_by"].ToString(),
                        s_no = dt["s_no"].ToString(),
                        mail_created_date = dt["created_date"].ToString(),
                        receiving_domain = dt["receiving_domain"].ToString(),
                        sending_domain = dt["sending_domain"].ToString(),
                        email_username = dt["email_username"].ToString(),
                    });
                    values.mailcampaignservice_list = getmodulelist;
                }
            }
            dt_datatable.Dispose();
        }
 
        public void DaUpdateShopifyService(string user_gid, shopifyservcie_list values)
        {

            if (values.shopify_id == null || values.shopify_id == "")
            {


                msSQL = " insert into crm_smm_shopify_service(" +
                        " access_token," +
                        " shopify_store_name," +
                        " store_month_year," +
                        " created_by," +
                        " created_date)" +
                        " values(" +
                        "'" + values.shopify_accesstoken + "'," +
                         " '" + values.shopify_store_name + "'," +
                        " '" + values.store_month_year + "',";
                msSQL += "'" + user_gid + "'," +
                         "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Shopify Credentials Updated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Shopify Credentials!!";
                }

            }
            else
            {
                msSQL = " update  crm_smm_shopify_service set " +

                " access_token = '" + values.shopify_accesstoken + "'," +

                " shopify_store_name = '" + values.shopify_store_name + "'," +
                " store_month_year = '" + values.store_month_year + "'," +

                " updated_by = '" + user_gid + "'," +

                " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where s_no='" + values.shopify_id + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Shopify Credentials Updated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Shopify Credentials !!";
                }
            }

        }
        public void DaUpdateEmailService(string user_gid, emailservice_list values)
        {

            if (values.email_id == null || values.email_id == "")
            {


                msSQL = " insert into crm_smm_mail_service(" +
                        " access_token," +
                        " base_url," +
                        " receiving_domain," +
                        " sending_domain," +
                        " email_username," +
                        " created_by," +
                        " created_date)" +
                        " values(" +
                        "'" + values.mail_access_token + "'," +
                        " '" + values.mail_base_url + "'," +
                        " '" + values.receiving_domain + "'," +
                        " '" + values.sending_domain + "',"+
                        " '" + values.email_username + "',";
                msSQL += "'" + user_gid + "'," +
                         "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Email Credentials Updated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Email Credentials !!";
                }

            }
            else
            {
                msSQL = " update  crm_smm_mail_service set " +

                " access_token = '" + values.mail_access_token + "'," +
                " base_url = '" + values.mail_base_url + "'," +
                " updated_by = '" + user_gid + "'," +
                " receiving_domain= '" +values.receiving_domain + "'," +
                " sending_domain= '"+ values.sending_domain + "',"+
                " email_username= '" + values.email_username + "',"+

                " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where s_no='" + values.email_id + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Email Credentials Updated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Email Credentials !!";
                }
            }

        }

        public void DaGetFacebookServiceSummary(MdlCampaignService values)
        {
            msSQL = " select access_token,page_id,s_no from crm_smm_tfacebookservice limit 1 ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getmodulelist = new List<facebookcampaignservice_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getmodulelist.Add(new facebookcampaignservice_list
                    {
                        facebook_access_token = dt["access_token"].ToString(),
                        facebook_page_id = dt["page_id"].ToString(),
                        facebook_id = dt["s_no"].ToString(),
                    });
                    values.facebookcampaignservice_list = getmodulelist;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaUpdateFacebookService(string user_gid, facebookcampaignservice_list values)
        {

            if (values.facebook_id == null || values.facebook_id == "")
            {


                msSQL = " insert into crm_smm_tfacebookservice(" +
                       " page_id," +
                        " access_token," +
                        " created_by," +
                        " created_date)" +
                        " values(" +
                          "'" + values.facebook_page_id + "'," +
                        "'" + values.facebook_access_token + "',";
                msSQL += "'" + user_gid + "'," +
                         "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Facebook Credentials Updated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Facebook Credentials !!";
                }

            }
            else
            {
                msSQL = " update  crm_smm_tfacebookservice set " +

                " access_token = '" + values.facebook_access_token + "'," +
               " page_id = '" + values.facebook_page_id + "'," +
                " updated_by = '" + user_gid + "'," +
                " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where s_no='" + values.facebook_id + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Facebook Credentials Updated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Facebook Credentials !!";
                }
            }

        }

        public void DaGetLinkedinServiceSummary(MdlCampaignService values)
        {
            msSQL = " select access_token,s_no from crm_smm_tlinkedinservice limit 1 ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getmodulelist = new List<linkedincampaignservice_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getmodulelist.Add(new linkedincampaignservice_list
                    {
                        linkedin_access_token = dt["access_token"].ToString(),
                        linkedin_id = dt["s_no"].ToString(),
                    });
                    values.linkedincampaignservice_list = getmodulelist;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaUpdateLinkedinService(string user_gid, linkedincampaignservice_list values)
        {

            if (values.linkedin_id == null || values.linkedin_id == "")
            {


                msSQL = " insert into crm_smm_tlinkedinservice(" +
                        " access_token," +
                        " created_by," +
                        " created_date)" +
                        " values(" +
                        "'" + values.linkedin_access_token + "',";
                msSQL += "'" + user_gid + "'," +
                         "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Linedin Credentials Updated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Linedin Credentials !!";
                }

            }
            else
            {
                msSQL = " update  crm_smm_tlinkedinservice set " +

                " access_token = '" + values.linkedin_access_token + "'," +
                " updated_by = '" + user_gid + "'," +
                " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where s_no='" + values.linkedin_id + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Linkedin Credentials Updated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Linkedin Credentials !!";
                }
            }

        }
        public void DaGetTelegramServiceSummary(MdlCampaignService values)
        {
            msSQL = " select bot_id,chat_id,s_no from crm_smm_ttelegramservice limit 1 ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getmodulelist = new List<telegramcampaignservice_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getmodulelist.Add(new telegramcampaignservice_list
                    {
                        bot_id = dt["bot_id"].ToString(),
                        chat_id = dt["chat_id"].ToString(),
                        telegram_id = dt["s_no"].ToString(),
                    });
                    values.telegramcampaignservice_list = getmodulelist;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaUpdateTelegramService(string user_gid, telegramcampaignservice_list values)
        {

            if (values.telegram_id == null || values.telegram_id == "")
            {


                msSQL = " insert into crm_smm_ttelegramservice(" +
                        " bot_id," +
                        " chat_id," +
                        " created_by," +
                        " created_date)" +
                        " values(" +
                        "'" + values.bot_id + "'," +
                           "'" + values.chat_id + "',";
                msSQL += "'" + user_gid + "'," +
                         "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Telegram Credentials Updated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Telegram Credentials !!";
                }

            }
            else
            {
                msSQL = " update  crm_smm_ttelegramservice set " +
                " bot_id = '" + values.bot_id + "'," +
                " chat_id = '" + values.chat_id + "'," +
                " updated_by = '" + user_gid + "'," +
                " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where s_no='" + values.telegram_id + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Telegram Credentials Updated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Telegram Credentials !!";
                }
            }

        }

        public void DaGetInstagramServiceSummary(MdlCampaignService values)
        {
            msSQL = " select access_token,s_no from crm_smm_tinstagramservice limit 1 ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getmodulelist = new List<instagramcampaignservice_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getmodulelist.Add(new instagramcampaignservice_list
                    {
                        instagram_access_token = dt["access_token"].ToString(),
                        instagram_id = dt["s_no"].ToString(),
                    });
                    values.instagramcampaignservice_list = getmodulelist;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaUpdateInstagramService(string user_gid, instagramcampaignservice_list values)
        {

            if (values.instagram_id == null || values.instagram_id == "")
            {


                msSQL = " insert into crm_smm_tinstagramservice(" +
                        " access_token," +
                        " created_by," +
                        " created_date)" +
                        " values(" +
                        "'" + values.instagram_access_token + "',";
                msSQL += "'" + user_gid + "'," +
                         "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Instagram Credentials Updated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Instagram Credentials !!";
                }

            }
            else
            {
                msSQL = " update  crm_smm_tinstagramservice set " +

                " access_token = '" + values.instagram_access_token + "'," +
                " updated_by = '" + user_gid + "'," +
                " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where s_no='" + values.instagram_id + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Instagram Credentials Updated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Instagram Credentials !!";
                }
            }

        }
    }
}
    