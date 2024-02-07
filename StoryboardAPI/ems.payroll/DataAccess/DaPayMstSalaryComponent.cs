﻿using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.utilities.Functions;
using ems.utilities.Models;
using ems.payroll.Models;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using MySql.Data.MySqlClient;

//using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;


namespace ems.payroll.DataAccess
{


    public class DaPayMstSalaryComponent
    {

        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL, msSQL1 = string.Empty;
        string msGetsalarycomponentgid, msGetgid, lsaffect_in, lssalary_flag;
        string lscomponenetamountemployer, lscomponenetamount, lscomponenetpercentage, lscomponenetemployerpercentage, lspercentage;
        MySqlDataReader objMySqlDataReader;
        DataTable dt_datatable;
        int mnResult;
        private object Else;
        private readonly string componenttype;
        private readonly string componentname;

        public void DaGetSalaryComponentSummary(MdlPayMstSalaryComponent values)
        {
            try
            {
                
                msSQL = " select salarycomponent_gid,componentgroup_gid, component_code, component_name, componentgroup_name,  component_type ,   affecting_in," +
                    " case when component_percentage='' or component_percentage is null or component_percentage=0 then format(component_amount,2)" +
                    " else cast(concat(component_percentage,' %') as char) end as component,  case when component_percentage_employer='' or component_percentage_employer is null or component_percentage_employer=0 then format(component_amount_employer,2)" +
                    " else cast(concat(component_percentage_employer,' %') as char) end as employer_component, component_flag_employer, contribution_type  from pay_mst_tsalarycomponent Order by salarycomponent_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<salarycompoent_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new salarycompoent_list
                        {
                            salarycomponent_gid = dt["salarycomponent_gid"].ToString(),
                            componentgroup_gid = dt["componentgroup_gid"].ToString(),
                            component_code = dt["component_code"].ToString(),
                            component_name = dt["component_name"].ToString(),
                            componentgroup_name = dt["componentgroup_name"].ToString(),
                            component_type = dt["component_type"].ToString(),
                            affecting_in = dt["affecting_in"].ToString(),
                            component = dt["component"].ToString(),
                            employer_component = dt["employer_component"].ToString(),
                            contribution_type = dt["contribution_type"].ToString(),


                        });
                        values.salarycompoent_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Salary Component!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        public void DaGetComponentGroupDtl(MdlPayMstSalaryComponent values)
        {
            try
            {
                
                msSQL = " Select componentgroup_gid,componentgroup_name  " +
                    " from pay_mst_tcomponentgroupmaster  GROUP BY componentgroup_name ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetComponentGroupdropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetComponentGroupdropdown
                        {
                            componentgroup_gid = dt["componentgroup_gid"].ToString(),
                            componentgroup_name = dt["componentgroup_name"].ToString(),
                        });
                        values.GetComponentGroupDtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Component Group detail!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        public void DaPostComponent(string user_gid, salarycompoent_list values)
        {
            try
            {
                
                msSQL = " SELECT component_code " +
                     " FROM pay_mst_tsalarycomponent " +
                     " WHERE component_code = '" + values.component_code + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                msSQL1 = " SELECT component_name  FROM " +
                         " pay_mst_tsalarycomponent  WHERE component_name = '" + values.component_name + "' ";
                DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL1);
                if (dt_datatable.Rows.Count > 0)
                {
                    values.message = "Component Code Already Exists";
                    values.status = false;
                    return;

                }
                else if (dt_datatable1.Rows.Count > 0)
                {
                    values.message = "Component Name Already Exists";
                    values.status = false;
                    return;

                }
                else
                {

                    msGetsalarycomponentgid = objcmnfunctions.GetMasterGID("PSCM");
                    if (msGetsalarycomponentgid == "E")
                    {
                        values.message = "Error Occured While Generating the Gid";
                        values.status = false;
                        return;

                    }
                    else
                    {

                        if (values.employee_amount == "" || values.employee_amount is null)
                        {
                            lscomponenetamount = "0.00";

                        }
                        else
                        {

                            lscomponenetamount = values.employee_amount;
                            // Parsing was successful, and abc now holds the double value

                        }

                        if (values.employee_percent == "" || values.employee_percent is null)
                        {
                            lscomponenetpercentage = "0.00 ";

                        }
                        else
                        {
                            lscomponenetpercentage = values.employee_percent;

                        }
                        if (values.employer_amount == "" || values.employer_amount is null)
                        {
                            lscomponenetamountemployer = "0.00";

                        }
                        else
                        {
                            lscomponenetamountemployer = values.employer_amount;

                        }

                        if (values.employer_percentage == "" || values.employer_percentage is null)
                        {
                            lscomponenetemployerpercentage = "0.00 ";

                        }
                        else
                        {
                            lscomponenetemployerpercentage = values.employer_percentage;

                        }

                        if (componenttype == "Other")
                        {
                            string lsaffect_in = "N/A";
                        }
                        else

                        {

                            string lsaffect_in = "affectingin";

                        }

                        if (componentname == "Basic salary")
                        {
                            string lssalary_flag = "Y";

                        }
                        else

                        {
                            string lssalary_flag = "N";
                        }

                        msSQL1 = " SELECT componentgroup_gid  FROM " +
                                 " pay_mst_tcomponentgroupmaster  WHERE componentgroup_name = '" + values.componentgroup_name + "' ";
                        string lscomponenentgroupgid = objdbconn.GetExecuteScalar(msSQL1);

                        msSQL = " INSERT INTO pay_mst_tsalarycomponent (" +
                                " salarycomponent_gid, " +
                                " componentgroup_gid, " +
                                " component_code, " +
                                " component_name, " +
                                " componentgroup_name, " +
                                " component_percentage, " +
                                " component_amount," +
                                " component_percentage_employer, " +
                                " component_amount_employer," +
                                " component_type, " +
                                " affecting_in, " +
                                " contribution_type, " +
                                " othercomponent_type, " +
                                " statutory_flag, " +
                                " lop_flag, " +
                                " component_flag," +
                                " component_flag_employer," +
                                " created_by, " +
                                " created_date) " +
                                " VALUES (" +
                                           "'" + msGetsalarycomponentgid + "', " +
                                           "'" + lscomponenentgroupgid + "', " +
                                           "'" + values.component_code + "', " +
                                           "'" + values.component_name + "'," +
                                           "'" + values.componentgroup_name + "'," +
                                           "'" + lscomponenetpercentage + "'," +
                                           "'" + lscomponenetamount + "'," +
                                           "'" + lscomponenetemployerpercentage + "'," +
                                           "'" + lscomponenetamountemployer + "'," +
                                           "'" + values.component_type + "', " +
                                           "'" + values.affect_in + "', " +
                                           "'" + values.contribution_type + "'," +
                                           "'" + values.other_allowance + "'," +
                                           "'" + values.statutory_pay + "'," +
                                           "'" + values.lop_deduction + "', " +
                                           "'" + values.is_percent + "', " +
                                           "'" + values.is_percentage + "', " +
                                           "'" + user_gid + "', " +
                                           "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Component Added Successfully";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Adding Component";
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Component!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        public void DagetEditComponent(string salarycomponent_gid, MdlPayMstSalaryComponent values)
        {
            try
            {
                
                msSQL = " select salarycomponent_gid,componentgroup_gid, componentgroup_name, component_code, lop_flag,statutory_flag,component_name,component_flag,component_amount,component_percentage,component_type, " +
                    " affecting_in,component_percentage_employer,component_amount_employer,component_flag_employer,contribution_type,othercomponent_type " +
                    " from pay_mst_tsalarycomponent where salarycomponent_gid= '" + salarycomponent_gid + " '";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<salarycomponentedit_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new salarycomponentedit_list
                        {
                            salarycomponent_gid = dt["salarycomponent_gid"].ToString(),
                            componentgroup_gid = dt["componentgroup_gid"].ToString(),
                            componentgroup_name = dt["componentgroup_name"].ToString(),
                            component_code = dt["component_code"].ToString(),
                            lop_deduction = dt["lop_flag"].ToString(),
                            statutory_pay = dt["statutory_flag"].ToString(),
                            component_name = dt["component_name"].ToString(),
                            employee_percent = dt["component_percentage"].ToString(),
                            employee_amount = dt["component_amount"].ToString(),
                            component_type = dt["component_type"].ToString(),
                            affect_in = dt["affecting_in"].ToString(),
                            is_percent = dt["component_flag"].ToString(),
                            is_percentage = dt["component_flag_employer"].ToString(),
                            employer_percentage = dt["component_percentage_employer"].ToString(),
                            employer_amount = dt["component_amount_employer"].ToString(),
                            contribution_type = dt["contribution_type"].ToString(),
                            other_allowance = dt["othercomponent_type"].ToString()



                        });
                        values.getEditComponent = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading edit component!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }


        public void DagetUpdatedComponent(string user_gid, salarycompoent_list values)
        {
            try
            {
                

                if (values.employee_amount == "" || values.employee_amount is null)
                {
                    lscomponenetamount = "0.00";

                }
                else
                {

                    lscomponenetamount = values.employee_amount;
                    // Parsing was successful, and abc now holds the double value

                }

                if (values.employee_percent == "" || values.employee_percent is null)
                {
                    lscomponenetpercentage = "0.00 ";

                }
                else
                {
                    lscomponenetpercentage = values.employee_percent;

                }
                if (values.employer_amount == "" || values.employer_amount is null)
                {
                    lscomponenetamountemployer = "0.00";

                }
                else
                {
                    lscomponenetamountemployer = values.employer_amount;

                }

                if (values.employer_percentage == "" || values.employer_percentage is null)
                {
                    lscomponenetemployerpercentage = "0.00 ";

                }
                else
                {
                    lscomponenetemployerpercentage = values.employer_percentage;

                }

                msSQL = " update pay_mst_tsalarycomponent SET " +
                        " component_name='" + values.component_name + "'," +
                        " componentgroup_name='" + values.componentgroup_name + "'," +
                        " component_percentage='" + lscomponenetpercentage + "'," +
                        " component_amount='" + lscomponenetamount + "'," +
                        " component_type='" + values.component_type + "'," +
                        " affecting_in='" + values.affect_in + "'," +
                        " component_percentage_employer='" + lscomponenetemployerpercentage + "'," +
                        " component_amount_employer='" + lscomponenetamountemployer + "'," +
                        " contribution_type='" + values.contribution_type + "'," +
                        " othercomponent_type='" + values.other_allowance + "'," +
                        " lop_flag='" + values.lop_deduction + "'," +
                        " updated_by='" + user_gid + "'," +
                        " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                        " WHERE salarycomponent_gid='" + values.salarycomponent_gid + "'";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Component Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Component";
                }


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while update component!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        public void DagetDeleteComponent(string salarycomponent_gid, salarycompoent_list values)
        {
            try
            {
                
                msSQL = "  delete from pay_mst_tsalarycomponent where salarycomponent_gid='" + salarycomponent_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Component Deleted Successfully";
                }
                else
                {
                    {
                        values.status = false;
                        values.message = "Error While Deleting Component";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while deleting component!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           



        }
    }
}