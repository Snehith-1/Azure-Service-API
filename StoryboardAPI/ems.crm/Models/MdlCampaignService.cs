﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ems.crm.Models
{
    public class MdlCampaignService
    {
        public List<campaignservice_list> campaignservice_list { get; set; }
        public List<shopifycampaignservice_list> shopifycampaignservice_list { get; set; }
        public List<mailcampaignservice_list> mailcampaignservice_list { get; set; }
        public List<shopifyservcie_list> shopifyservcie_list { get; set; }
        public List<emailservice_list> emailservice_list { get; set; }
        public List<facebookcampaignservice_list> facebookcampaignservice_list { get; set; }
        public List<instagramcampaignservice_list> instagramcampaignservice_list { get; set; }
        public List<linkedincampaignservice_list> linkedincampaignservice_list { get; set; }
        public List<telegramcampaignservice_list> telegramcampaignservice_list { get; set; }

        

    }
    public class telegramcampaignservice_list : result
    {

        public string bot_id { get; set; }
        public string chat_id { get; set; }
        public string telegram_id { get; set; }

    }
    public class linkedincampaignservice_list : result
    {

        public string linkedin_access_token { get; set; }
        public string linkedin_id { get; set; }


    }
    public class facebookcampaignservice_list : result
    {

        public string facebook_access_token { get; set; }
        public string facebook_id { get; set; }
        public string facebook_page_id { get; set; }



    }
    public class emailservice_list : result
    {
        public string mail_access_token { get; set; }
        public string mail_base_url { get; set; }
        public string email_id { get; set; }
        public string sending_domain { get; set; }
        public string receiving_domain { get; set; }
        public string email_username { get; set; }

    }

        public class shopifyservcie_list : result
    {
        public string shopify_accesstoken { get; set; }
        public string shopify_store_name { get; set; }
        public string store_month_year { get; set; }
        public string shopify_id { get; set; }
        

    }
    public class campaignservice_list : result
    {
        public string access_token { get; set; }
        public string base_url { get; set; }
        public string workspace_id { get; set; }
        public string channel_id { get; set; }
        public string mobile_number { get; set; }
        public string channelgroup_id { get; set; }
        public string channel_name { get; set; }
        public string created_date { get; set; }
        public string created_by { get; set; }
        public string access_token_edit { get; set; }
        public string whatsapp_accesstoken { get; set; }
        public string workspace_id_edit { get; set; }
        public string s_no { get; set; }
        public string whatsapp_id { get; set; }


    }
    public class shopifycampaignservice_list : result
    {
        

        public string shopify_access_token { get; set; }
        public string shopify_store_name { get; set; }
        public string store_month_year { get; set; }
        public string shopify_created_date { get; set; }
        public string shopify_created_by { get; set; }
        public string s_no { get; set; }
        

    }

    public class mailcampaignservice_list : result
    {


        public string mail_access_token { get; set; }
        public string mail_base_url { get; set; }
        public string mail_created_date { get; set; }
        public string mail_created_by { get; set; }
        public string s_no { get; set; }
        public string sending_domain { get; set; }
        public string receiving_domain { get; set; }
        public string email_username { get; set; }

    }
    public class instagramcampaignservice_list : result
    {

        public string instagram_access_token { get; set; }
        public string instagram_id { get; set; }



    }
}