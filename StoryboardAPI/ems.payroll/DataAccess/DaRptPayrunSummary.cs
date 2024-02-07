﻿using ems.payroll.Models;

using ems.utilities.Functions;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using ems.payroll.Models;

using ems.utilities.Functions;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
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
    public class DaRptPayrunSummary
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;

        string msGetloangid;
        MySqlDataReader objMySqlDataReader;
        DataTable dt_datatable;
        int mnResult;
        public void DaGetPayrunSummary(string branch_gid, string department_gid, string month, string year, MdlRptPayrunSummary values)
        {
            try
            {
                
                msSQL = "select a.salary_gid,a.month,a.year, a.employee_gid,concat_ws(' ', c.user_firstname, user_lastname) as employee_name,b.branch_gid,c.user_code," +
                    " d.branch_name,e.department_name as department,a.leave_taken,a.lop_days as lop, " +
                     " format(a.basic_salary, 2) as basic_salary , format(a.earned_basic_salary, 2) as earned_basic_salary ,b.user_gid, " +
                     " format(a.gross_salary, 2) as gross_salary , format(a.earned_gross_salary, 2) as earned_gross_salary ,a.public_holidays, " +
                     " format(a.net_salary, 2)As net_salary, format(a.earned_net_salary, 2)As earned_net_salary, a.actual_month_workingdays,a.month_workingdays " +
                     " from pay_trn_tsalary a " +
                     " left join hrm_mst_temployee b on a.employee_gid = b.employee_gid " +
                     " left join adm_mst_tuser c on b.user_gid = c.user_gid " +
                     " left join hrm_mst_tbranch d on b.branch_gid = d.branch_gid " +
                     " left join hrm_mst_tdepartment e on b.department_gid = e.department_gid ";
                if (year != "null" || month != "null" || branch_gid != "null" || department_gid != "null")
                {
                    msSQL += " where ";
                    if (year != null)
                    {
                        msSQL += " a.year = '" + year + "' ";
                        addfunyear();
                    }
                    if (month != "null")
                    {
                        msSQL += " a.month = '" + month + "' ";
                        addfunmonth();
                    }
                    if (branch_gid != "null")
                    {
                        msSQL += " d.branch_gid = '" + branch_gid + "' ";
                        addfunbranch();
                    }
                    if (department_gid != "null")
                    {
                        msSQL += " e.department_gid = '" + department_gid + "' ";
                    }
                    void addfunyear()
                    {
                        if (month != "null" || branch_gid != "null" || department_gid != "null") { msSQL += " and "; }
                    }
                    void addfunmonth()
                    {
                        if (branch_gid != "null" || department_gid != "null") { msSQL += " and "; }
                    }
                    void addfunbranch()
                    {
                        if (department_gid != "null") { msSQL += " and "; }
                    }

                }


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetPayrunlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetPayrunlist
                        {
                            salary_gid = dt["salary_gid"].ToString(),
                            month = dt["month"].ToString(),
                            year = dt["year"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            department = dt["department"].ToString(),
                            leave_taken = dt["leave_taken"].ToString(),
                            lop = dt["lop"].ToString(),
                            basic_salary = dt["basic_salary"].ToString(),
                            earned_basic_salary = dt["earned_basic_salary"].ToString(),
                            public_holidays = dt["public_holidays"].ToString(),
                            gross_salary = dt["gross_salary"].ToString(),
                            earned_gross_salary = dt["earned_gross_salary"].ToString(),
                            net_salary = dt["net_salary"].ToString(),
                            earned_net_salary = dt["earned_net_salary"].ToString(),
                            actual_month_workingdays = dt["actual_month_workingdays"].ToString(),
                            month_workingdays = dt["month_workingdays"].ToString(),

                        });
                        values.payrunlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Payrun Report!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        public void DaGetBranchDtl(MdlRptPayrunSummary values)
        {
            try
            {
                
                msSQL = " Select branch_gid,branch_name  " +
                    " from  hrm_mst_tbranch ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetBranchdropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetBranchdropdown
                        {
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                        });
                        values.GetBranchDtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding Branch detail!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        public void DaGetDepartmentDtl(MdlRptPayrunSummary values)
        {
            try
            {
                
                msSQL = " select department_gid,department_name" +
                    " from hrm_mst_tdepartment ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetDepartmentdropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetDepartmentdropdown
                        {
                            department_gid = dt["department_gid"].ToString(),
                            department_name = dt["department_name"].ToString(),
                        });
                        values.GetDepartmentDtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding Department!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        public void Daadditionalsummary(string salary_gid, MdlRptPayrunSummary values)
        {
            try
            {
                
                msSQL = " select a.salary_gid,(c.componentgroup_name) as salarycomponent_name,b.salarycomponent_percentage," +
                            " format(b.earned_salarycomponent_amount,2)As earned_salarycomponent_amount" +
                            "  from pay_trn_tsalary a" +
                            " left join pay_trn_tsalarydtl b on a.salary_gid=b.salary_gid" +
                            " left join pay_mst_tsalarycomponent c on b.salarycomponent_gid=c.salarycomponent_gid " +
                            " where a.salary_gid ='" + salary_gid + "' and b.salarygradetype='Addition' and c.primecomponent_flag='N'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<addsummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new addsummary
                        {
                            salary_gid = dt["salary_gid"].ToString(),
                            salarycomponent_name = dt["salarycomponent_name"].ToString(),
                            earned_amount = dt["earned_salarycomponent_amount"].ToString(),

                        });
                        values.addsummary = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Additional Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        public void Dadeductsummary(string salary_gid, MdlRptPayrunSummary values)
        {
            try
            {
                
                msSQL = " select a.salary_gid,(c.componentgroup_name) as salarycomponent_name,b.salarycomponent_percentage," +
                            " format(b.earned_salarycomponent_amount,2)As earned_salarycomponent_amount" +
                            "  from pay_trn_tsalary a" +
                            " left join pay_trn_tsalarydtl b on a.salary_gid=b.salary_gid" +
                            " left join pay_mst_tsalarycomponent c on b.salarycomponent_gid=c.salarycomponent_gid " +
                            " where a.salary_gid ='" + salary_gid + "' and b.salarygradetype='Deduction' and c.primecomponent_flag='N'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<addsummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new addsummary
                        {
                            salary_gid = dt["salary_gid"].ToString(),
                            salarycomponent_name = dt["salarycomponent_name"].ToString(),
                            earned_amount = dt["earned_salarycomponent_amount"].ToString(),

                        });
                        values.addsummary = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Deduction Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        public void Daothersummary(string salary_gid, MdlRptPayrunSummary values)
        {
            try
            {
                
                msSQL = " select a.salary_gid,(c.componentgroup_name) as salarycomponent_name,b.salarycomponent_percentage," +
                            " format(b.earned_salarycomponent_amount,2)As earned_salarycomponent_amount" +
                            "  from pay_trn_tsalary a" +
                            " left join pay_trn_tsalarydtl b on a.salary_gid=b.salary_gid" +
                            " left join pay_mst_tsalarycomponent c on b.salarycomponent_gid=c.salarycomponent_gid " +
                            " where a.salary_gid ='" + salary_gid + "' and b.salarygradetype='other' and c.primecomponent_flag='N'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<addsummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new addsummary
                        {
                            salary_gid = dt["salary_gid"].ToString(),
                            salarycomponent_name = dt["salarycomponent_name"].ToString(),
                            earned_amount = dt["earned_salarycomponent_amount"].ToString(),

                        });
                        values.addsummary = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Other Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
          
       }
    }
}