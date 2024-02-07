﻿using ems.hrm.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;


namespace ems.hrm.DataAccess
{
    public class DaLeaveGrade
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string lsattendance_startdate, lsattendance_enddate;

        MySqlDataReader objMySqlDataReader;
        DataTable dt_datatable;
        int mnResult;
        string msGetGid, msGetGid1, lsempoyeegid, msgetshift;
        public void DaLeaveGradeSummary(MdlLeaveGrade values)
        {
            try
            {
                
                msSQL = " select  a.leavegrade_gid ,a.leavegrade_code, a.leavegrade_name ,c.leavetype_name , " +
               " format(sum(b.total_leavecount),2)as total_leavecount , format(sum(b.available_leavecount),2)as available_leavecount, " +
               " format(sum(b.leave_limit),2)as leave_limit from hrm_mst_tleavegrade a " +
               " left join hrm_mst_tleavegradedtl b on a.leavegrade_gid=b.leavegrade_gid  " +
               " left join hrm_mst_tleavetype c on c.leavetype_gid=b.leavetype_gid group by leavegrade_gid";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leavegrade1_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leavegrade1_list
                        {
                            leavegrade_gid = dt["leavegrade_gid"].ToString(),
                            leavegrade_code = dt["leavegrade_code"].ToString(),
                            leavegrade_name = dt["leavegrade_name"].ToString(),
                            leavetype_name = dt["leavetype_name"].ToString(),
                            total_leavecount = dt["total_leavecount"].ToString(),
                            available_leavecount = dt["available_leavecount"].ToString(),
                            leave_limit = dt["leave_limit"].ToString(),

                        });
                        values.leavegrade_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetleavegradecodesummary(leavegradesubmit_list values)
        {
            try
            {
                
                msSQL = "select leavetype_gid,leavetype_code,leavetype_name from hrm_mst_tleavetype";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leavegradecode_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leavegradecode_list
                        {
                            leavetype_gid = dt["leavetype_gid"].ToString(),
                            leavetype_code = dt["leavetype_code"].ToString(),
                            leavetype_name = dt["leavetype_name"].ToString(),

                        });
                        values.leavegradecode_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaLeaveGradeSubmit( leavegradesubmit_list values)
        {
            try
            {
                
                msSQL = "select leavegrade_code from hrm_mst_tleavegrade  where leavegrade_code = '" + values.leavegrade_code + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    values.status = false;
                    values.message = " Leave Grade Code Already Exist";
                }

                msSQL = "select attendance_startdate,attendance_enddate from adm_mst_Tcompany ";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    lsattendance_startdate = objMySqlDataReader["attendance_startdate"].ToString();
                    lsattendance_enddate = objMySqlDataReader["attendance_enddate"].ToString();
                }

                msGetGid = objcmnfunctions.GetMasterGID("LEGD");

                msSQL = " insert into hrm_mst_tleavegrade ( " +
                    " leavegrade_gid, " +
                    " leavegrade_code, " +
                    " leavegrade_name," +
                    " attendance_startdate," +
                    " attendance_enddate) " +
                   " values (" +
                    "'" + msGetGid + "', " +
                    "'" + values.leavegrade_code + "'," +
                    "'" + values.leavegrade_name + "'," +
                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                foreach (var data in values.leavegradecode_list)
                {
                    msGetGid1 = objcmnfunctions.GetMasterGID("LE2G");
                    msSQL = " insert into hrm_mst_tleavegradedtl ( " +
                   " leavegradedtl_gid, " +
                   " leavegrade_gid, " +
                   " leavetype_gid, " +
                   " total_leavecount," +
                   " available_leavecount," +
                   " leave_limit) " +
                   " values (" +
                    "'" + msGetGid1 + "', " +
                    "'" + msGetGid + "', " +
                    "'" + data.leavetype_gid + "', " +
                    "'" + data.total_leavecount + "'," +
                    "'" + data.available_leavecount + "'," +
                    "'" + data.leave_limit + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Leave Grade Added Sucessfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Leave Grade";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

    }
}