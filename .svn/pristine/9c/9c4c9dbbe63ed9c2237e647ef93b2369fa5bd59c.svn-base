using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Web;
using ems.utilities.Models;
using MySql.Data.MySqlClient;
namespace ems.utilities.Functions
{
    public class session_values
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        MySqlDataReader objMySqlDataReader;
        string msSQL = string.Empty;

        public logintoken gettokenvalues(string token)
        {
            logintoken getlogintoken = new logintoken();
           
            msSQL = " select a.employee_gid,a.user_gid,a.department_gid, b.branch_gid from adm_mst_ttoken a " +
                " left join hrm_mst_temployee b on  a.employee_gid=b.employee_gid " +
                " WHERE token = '" + token + "'";
            objMySqlDataReader = objdbconn .GetDataReader(msSQL);
            if (objMySqlDataReader.HasRows == true)
            {
                objMySqlDataReader.Read();
                getlogintoken.employee_gid = objMySqlDataReader["employee_gid"].ToString();
                getlogintoken.user_gid = objMySqlDataReader["user_gid"].ToString();
                getlogintoken.department_gid = objMySqlDataReader["department_gid"].ToString();
                getlogintoken.branch_gid = objMySqlDataReader["branch_gid"].ToString();
                objMySqlDataReader.Close();
            }
            else
                objMySqlDataReader.Close();
            return getlogintoken;
        }
    }
}