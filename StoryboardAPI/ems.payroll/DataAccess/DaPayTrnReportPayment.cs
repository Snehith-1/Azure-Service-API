﻿using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.payroll.Models;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Configuration;
using System.Drawing;
using System.IO;
using MySql.Data.MySqlClient;

namespace ems.payroll.DataAccess
{
    public class DaPayTrnReportPayment
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string msGetloangid;

        MySqlDataReader objMySqlDataReader;
        DataTable dt_datatable;
        int mnResult;
        public void DaPaymentSummary(MdlPayTrnReportPayment values)
        {
            try
            {
                
                msSQL = " select sum(net_salary) as payment_amount,count(employee_gid) as employee_count, payment_month, payment_year " +
              " from pay_trn_tpayment group by payment_year, payment_month order by payment_year desc,MONTH(STR_TO_DATE(substring(payment_month,1,3),'%b')) desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetPaylist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetPaylist
                        {
                            Amount = dt["payment_amount"].ToString(),
                            Employee_count = dt["employee_count"].ToString(),
                            month = dt["payment_month"].ToString(),
                            year = dt["payment_year"].ToString(),

                        });
                        values.GetPaylist = getModuleList;
                    }

                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Payment report!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }
        public void DareportPaymentExpand(string month, string year, MdlPayTrnReportPayment values)
        {
            try
            {
                
                msSQL = " select a.employee_gid,payment_year,payment_month,c.user_code,concat(c.user_firstname,' ',c.user_lastname) as employee_name, " +
                    " d.designation_name,e.department_name, " +
                    " a.net_salary from pay_trn_tpayment a " +
                    " inner join hrm_mst_temployee b on a.employee_gid=b.employee_gid " +
                    " inner join adm_mst_tuser c on b.user_gid=c.user_gid " +
                    " inner join adm_mst_tdesignation d on b.designation_gid=d.designation_gid " +
                    " inner join hrm_mst_tdepartment e on b.department_gid=e.department_gid " +
                    " group by employee_gid order by c.user_firstname asc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetreportExpand>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetreportExpand
                        {
                            employee_gid = dt["employee_gid"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            net_salary = dt["net_salary"].ToString(),

                        });
                        values.GetreportExpand = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Payment expand!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        public void DaGetReportExportExcel(MdlPayTrnReportPayment values)
        {
            try
            {
                msSQL = " select sum(net_salary) as payment_amount,count(employee_gid) as employee_count, payment_month, payment_year " +
              " from pay_trn_tpayment group by payment_year, payment_month order by payment_year desc,MONTH(STR_TO_DATE(substring(payment_month,1,3),'%b')) desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                string lscompany_code = string.Empty;
                ExcelPackage excel = new ExcelPackage();
                var workSheet = excel.Workbook.Worksheets.Add("Payment Report");
                try
                {
                    msSQL = " select company_code from adm_mst_tcompany";

                    lscompany_code = objdbconn.GetExecuteScalar(msSQL);
                    string lspath = ConfigurationManager.AppSettings["exportexcelfile"] + "/Payment/export" + "/" + lscompany_code + "/" + DateTime.Now.Year + "/" + DateTime.Now.Month;
                    //values.lspath = ConfigurationManager.AppSettings["file_path"] + "/erp_documents" + "/" + lscompany_code + "/" + "SDC/TestReport/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                    {
                        if ((!System.IO.Directory.Exists(lspath)))
                            System.IO.Directory.CreateDirectory(lspath);
                    }

                    string lsname = "payment_Report" + DateTime.Now.ToString("(dd-MM-yyyy HH-mm-ss)") + ".xlsx";
                    string lspath1 = ConfigurationManager.AppSettings["exportexcelfile"] + "/Payment/export" + "/" + lscompany_code + "/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + lsname;

                    workSheet.Cells[1, 1].LoadFromDataTable(dt_datatable, true);
                    FileInfo file = new FileInfo(lspath1);
                    using (var range = workSheet.Cells[1, 1, 1, 8])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                        range.Style.Font.Color.SetColor(Color.White);
                    }
                    excel.SaveAs(file);

                    var getModuleList = new List<GetreportExportExcel>();
                    if (dt_datatable.Rows.Count != 0)
                    {

                        getModuleList.Add(new GetreportExportExcel
                        {

                            lsname = lsname,
                            lspath1 = lspath1,



                        });
                        values.GetreportExportExcel = getModuleList;

                    }
                    dt_datatable.Dispose();
                    values.status = true;
                    values.message = "Success";
                }
                catch (Exception ex)
                {
                    values.status = false;
                    values.message = "Failure";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Payment expand!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
     

    }
}