﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ems.system.Models;
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
using MySql.Data.MySqlClient;

namespace ems.system.DataAccess
{
    public class DaSysMstDesignation
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        MySqlDataReader objMySqlDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;

        int mnResult6;

        public void DaGetDesignationtSummary(MdlSysMstDesignation values)
        {
            msSQL = " select designation_gid, designation_code, designation_name,status as designation_status from  adm_mst_tdesignation" +
                    " where 1 = 1 order by designation_name asc";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<Designation_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new Designation_list
                    {
                        designation_gid = dt["designation_gid"].ToString(),
                        designation_code = dt["designation_code"].ToString(),
                        designation_status = dt["designation_status"].ToString(),
                        designation_name = dt["designation_name"].ToString(),
                       

                    });
                    values.Designation_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }

        public void DaPostDesignationAdd(string user_gid, Designation_list values)
        {
            msGetGid1 = objcmnfunctions.GetMasterGID("SDGM");
            lsdesignation_code = objcmnfunctions.GetMasterGID("DESG");
            if (msGetGid1 == "E")
            {
                values.status = false;
                values.message = "Sequence code not generated for Gid";
                return;

            }
            if (lsdesignation_code == "E")
            {
                values.status = false;
                values.message = "Sequence code not generated for Code";
                return;

            }
            msSQL = " insert into adm_mst_tdesignation (" +
                    " designation_gid," +
                    " designation_code," +
                    " designation_name," +
                    " created_by, " +
                    " created_date)" +
                    " values(" +
                    " '" + msGetGid1 + "'," +
                    " '" + lsdesignation_code + "'," +
                    "'" + values.designation_name + "'," +
                    "'" + user_gid + "'," +
                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            if (mnResult != 0)
            {
                values.status = true;
                values.message = "Designation Added Successfully";
            }
            else
            {
                values.status = false;
                values.message = "Error While Adding Designation";
            }
        }

        public void DaDeleteDesignation(string designation_gid, Designation_list values)
        {
            msSQL = "  delete from adm_mst_tdesignation where designation_gid='" + designation_gid + "'  ";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult != 0)
            {
                values.status = true;
                values.message = "Designation Deleted Successfully";
            }
            else
            {
                
                    values.status = false;
                    values.message = "Error While Deleting Designation";
                
            }

        }





        public void DaPostUpdateDesignation(string user_gid, Designation_list values)
        {
            msSQL = " update  adm_mst_tdesignation set " +
             " designation_name = '" + values.designation_name + "'," +
             " updated_by = '" + user_gid + "'," +
             " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where designation_gid='" + values.designation_gid + "'  ";

            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult == 1)
            {
                values.status = true;
                values.message = "Designation Updated Successfully !!";
            }
            else
            {
                values.status = false;
                values.message = "Error While Updating Designation !!";
            }
        }



        public void DaPostDesignationStatus(string user_gid, Designation_list values)
        {
            msSQL = " update  adm_mst_tdesignation set " +
             " status = '" + values.designation_status + "'," +
             " updated_by = '" + user_gid + "'," +
             " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where designation_gid='" + values.designation_gid + "'  ";

            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult == 1)
            {
                values.status = true;
                values.message = "Designation Status Updated Successfully !!";
            }
            else
            {
                values.status = false;
                values.message = "Error While Updating Designation Status !!";
            }
        }






    }
}