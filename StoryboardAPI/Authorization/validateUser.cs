﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ems.utilities.Functions;
using System.Data;
using System.Data.Odbc;
using System.Configuration;
using System.IO;
using System.Text.Json;
using MySql.Data.MySqlClient;
namespace StoryboardAPI.Authorization
{

    public class validateUser
    {
        dbconn objdbconn = new dbconn();
        MySqlDataReader objMySqlDataReader;
        string mssql;
        public bool isvalid(string username, string password, string companycode = null)
        { 
            mssql = " SELECT user_gid FROM " +companycode + ".adm_mst_tuser " +
                    " WHERE user_code='" + username + "' AND user_password='" + password + "'";
            objMySqlDataReader = objdbconn.GetDataReader(mssql, companycode);
            objMySqlDataReader.Read();
            if (objMySqlDataReader.HasRows)
            {
                objMySqlDataReader.Close();
                return true;
            }
            else
            {
                objMySqlDataReader.Close();
                return false;
            }
        } 
        public class MdlCmnConn
        {
            public string connection_string { get; set; }
            public string company_code { get; set; }
            public string company_dbname { get; set; }
        }
    }
}