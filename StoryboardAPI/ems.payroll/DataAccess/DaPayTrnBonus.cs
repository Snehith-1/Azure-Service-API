﻿using ems.payroll.Models;
using ems.utilities.Functions;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Web;
using static OfficeOpenXml.ExcelErrorValue;
using static System.Collections.Specialized.BitVector32;
using MySql.Data.MySqlClient;


namespace ems.payroll.DataAccess
{
    public class DaPayTrnBonus
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;

        MySqlDataReader objMySqlDataReader;
        DataTable dt_datatable;
        int mnResult;
        string msGetGid, msGetGid1, lsempoyeegid;

        // Module Master Summary
        public void DaGetBonusSummary(MdlPayTrnBonus values)
        {
            msSQL = "select a.bonus_gid,a.bonus_name,a.bonus_date,date_format(a.bonus_date,'%d-%m-%Y') as bonus_fromdate," +
                    " date_format(a.bonus_todate,'%d-%m-%Y') as bonus_todate,a.bonus_percentage from pay_trn_tbonus a " +
                    "where system_flag='N' order by a.bonus_gid desc";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetBonus>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetBonus
                    {
                        bonus_gid = dt["bonus_gid"].ToString(),
                        bonus_name = dt["bonus_name"].ToString(),
                        bonus_date = dt["bonus_date"].ToString(),
                        bonus_fromdate = dt["bonus_fromdate"].ToString(),
                        bonus_todate = dt["bonus_todate"].ToString(),
                        bonus_percentage = dt["bonus_percentage"].ToString(),


                    });
                    values.GetBonus = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }


        public void DaPostBonusEmployee(string user_gid,string bonus_gid)
        {

        }

        public void DaGetBonusEmployeeSummary(string bonus_gid, MdlPayTrnBonus values)
        {
            msSQL = " Select distinct a.user_gid, " +
                   " a.user_firstname," +
                   " a.user_code,  " +
                   " d.designation_name ,c.employee_gid,e.branch_name, " +
                   " c.department_gid,c.branch_gid, g.department_name " +
                   " FROM adm_mst_tuser a " +
                   " left join hrm_mst_temployee c on a.user_gid = c.user_gid " +
                   " left join adm_mst_tdesignation d on c.designation_gid = d.designation_gid " +
                   " left join hrm_mst_tbranch e on c.branch_gid = e.branch_gid " +
                   " left join hrm_mst_tdepartment g on g.department_gid = c.department_gid " +
                   " left join hrm_trn_temployeetypedtl h on c.employee_gid=h.employee_gid " +
                   " left join hrm_mst_twagestype i on h.wagestype_gid= i.wagestype_gid " +
                   " left join hrm_mst_temployeetype j on h.employeetype_name= j.employee_type " +
                   " inner join pay_trn_tsalary s on s.employee_gid=c.employee_gid " +
                   " WHERE a.user_status='Y' " +
                   " and c.employee_gid not in  " +
                   " (select employee_gid from pay_trn_temployee2bonus where bonus_gid='" + bonus_gid + "' and bonus_flag='Y')";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<Getemployee2bonusSummary>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new Getemployee2bonusSummary
                    {



                        user_gid = dt["user_gid"].ToString(),
                        user_firstname = dt["user_firstname"].ToString(),
                        user_code = dt["user_code"].ToString(),
                        designation_name = dt["designation_name"].ToString(),
                        employee_gid = dt["employee_gid"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        department_gid = dt["department_gid"].ToString(),
                        branch_gid = dt["branch_gid"].ToString(),
                        department_name = dt["department_name"].ToString(),



                    });
                    values.Getemployee2bonusSummary = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }


        }
}