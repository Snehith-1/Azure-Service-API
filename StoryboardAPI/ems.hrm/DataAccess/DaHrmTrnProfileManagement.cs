﻿using ems.hrm.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
//using OfficeOpenXml;
//using OfficeOpenXml.Style;
using MySql.Data.MySqlClient;



namespace ems.hrm.DataAccess
{
    public class DaHrmTrnProfileManagement
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        MySqlDataReader objMySqlDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, lsuser_password, lsuser_firstname;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;

        int mnResult6;

        public void DaUpdatePersonalDetails(string user_gid, personaldetails values)
        {
            try
            {
                
                msSQL = " update adm_mst_tuser set " +
                    " user_firstname  = '" + values.firstedit_name + "'," +
                    " user_lastname  = '" + values.lastedit_name + "'," +
                    " updated_by = '" + user_gid + "'," +
                     " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where user_gid='" + user_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                msSQL = " update hrm_mst_temployee set " +
                         " employee_gender = '" + values.gender_edit + "'," +
                        " employee_dob = '" + values.dateof_birthedit + "'," +
                        " employee_mobileno = '" + values.mobile_edit + "'," +
                        " employee_personalno = '" + values.personal_noedit + "'," +
                        " employee_qualification = '" + values.qualification_edit + "'," +
                        " bloodgroup_name = '" + values.blood_groupedit + "'," +
                        " employee_experience = '" + values.experience_edit + "'," +
                         " updated_by = '" + user_gid + "'," +
                         " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where user_gid='" + user_gid + "'  ";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Personal Details Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Personal Details";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
    public void DaUpdatePassword(string user_gid, passwordupdate values)
        {
            try
            {
                
                msSQL = " select user_password from adm_mst_tuser where user_gid = '" + user_gid + "' ";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows)
                {
                    lsuser_password = objMySqlDataReader["user_password"].ToString();
                }
                if (lsuser_password != values.confedit_pwd)
                {
                    msSQL = " update  adm_mst_tuser set " +
                     " user_password = '" + objcmnfunctions.ConvertToAscii(values.confedit_pwd) + "'," +
                     " updated_by = '" + user_gid + "'," +
                     " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where user_gid='" + user_gid + "'  ";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Password Updated Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Password";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = " Password Already Exist";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaWorkExperience(string user_gid, workexperience values)
        {
            try
            {
                
                msGetGid = objcmnfunctions.GetMasterGID("EMPH");
                msSQL = " insert into hrm_mst_temployeehistory(" +
                        " employeehistory_gid," +
                        " previous_company," +
                        " employee_code," +
                        " previuos_occupation," +
                        " employee_dept," +
                        " date_joining," +
                        " date_releiving," +
                        " work_period," +
                        " conduct_info," +
                        " leave_reason," +
                        " reporting_to," +
                        " remarks," +
                        " created_by, " +
                       " created_date)" +
                        " values(" +
                        " '" + msGetGid + "'," +
                        "'" + values.empl_prevcomp + "'," +
                         "'" + values.empl_code + "'," +
                         "'" + values.prev_occp + "'," +
                         "'" + values.department + "'," +
                         "'" + values.date_ofjoining + "'," +
                          "'" + values.date_ofreleiving + "'," +
                         "'" + values.work_period + "'," +
                           "'" + values.HR_name + "'," +
                          "'" + values.reason + "'," +
                         "'" + values.report + "'," +
                          "'" + values.rmrks + "'," +
                           "'" + user_gid + "'," +
                         "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Work Experience Added Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Work Experience";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaNomination(string user_gid, nomination values)
        {
            try
            {
                
                msSQL = " SELECT employee_gid FROM adm_mst_tuser a left join hrm_mst_temployee b on b.user_gid=a.user_gid WHERE a.user_gid='" + user_gid + "' ";
                lsemployee_gid = objdbconn.GetExecuteScalar(msSQL);
                msGetGid = objcmnfunctions.GetMasterGID("FFMLY");
                msSQL = " insert into hrm_trn_tformfamilydetails(" +
                        " familydtl_gid," +
                        " familymember_name," +
                        " dob," +
                        " age," +
                        " mobile_no," +
                        " relationship," +
                        " residing_with," +
                        " residing_address," +
                        " nominee_for," +
                        "employee_gid," +
                        " created_by, " +
                        " created_date)" +
                        " values(" +
                        " '" + msGetGid + "'," +
                        "'" + values.name + "'," +
                         "'" + values.dateofbirth + "'," +
                         "'" + values.age + "'," +
                         "'" + values.mobile_no + "'," +
                         "'" + values.relt_employee + "'," +
                          "'" + values.resign_employee + "'," +
                          "'" + values.residing_addr + "'," +
                          "'" + values.nominee_for + "'," +
                         "'" + lsemployee_gid + "'," +
                           "'" + user_gid + "'," +
                         "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Nomination Added Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Nomination";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

    public void DaStatutory(string user_gid, statutory values)
        {
            try
            {
                
                msSQL = " SELECT employee_gid FROM adm_mst_tuser a left join hrm_mst_temployee b on b.user_gid=a.user_gid WHERE a.user_gid='" + user_gid + "' ";
                lsemployee_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " insert  hrm_mst_tstatutorydetail(" +
                "pfref_no," +
                " esicref_no," +
                " esic_branchoffice," +
                //" dispensary," +
                " created_by, " +
               " created_date)" +
                " values(" +
                "'" + values.provident_no + "'," +
                 "'" + values.date_ofjoinPF + "'," +
                  "'" + values.employee_no + "'," +
                   //"'" + values.remarks + "'," +
                   "'" + user_gid + "'," +
                 "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Statutory Added Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Statutory";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaEmergencyContact(string user_gid, emergencycontact values)
        {
            try
            {
                
                msSQL = " SELECT employee_gid FROM adm_mst_tuser a left join hrm_mst_temployee b on b.user_gid=a.user_gid WHERE a.user_gid='" + user_gid + "' ";
                lsemployee_gid = objdbconn.GetExecuteScalar(msSQL);
                msGetGid = objcmnfunctions.GetMasterGID("HREC");
                msSQL = " insert into hrm_tmp_temergency(" +
                        " emergency_gid," +
                        " contact_name," +
                        " contact_address," +
                        " contact_no," +
                        " contact_email," +
                        " reference_details," +
                        " employee_gid," +
                        " created_by, " +
                        " created_date)" +
                        " values(" +
                        " '" + msGetGid + "'," +
                        "'" + values.contact_person + "'," +
                         "'" + values.cont_addr + "'," +
                         "'" + values.cont_no + "'," +
                         "'" + values.cont_emailid + "'," +
                         "'" + values.remarks + "'," +
                         "'" + lsemployee_gid + "'," +
                         "'" + user_gid + "'," +
                         "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Emergency Contact Added Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Emergency Contact";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaDependent(string user_gid, dependent values)
        {
            try
            {
                
                msSQL = " SELECT employee_gid FROM adm_mst_tuser a left join hrm_mst_temployee b on b.user_gid=a.user_gid WHERE a.user_gid='" + user_gid + "' ";
                lsemployee_gid = objdbconn.GetExecuteScalar(msSQL);
                msGetGid = objcmnfunctions.GetMasterGID("HRDE");
                msSQL = " insert into hrm_tmp_tdependent(" +
                        " dependent_gid," +
                        " name," +
                        " relationship," +
                        " dob," +
                        "employee_gid," +
                        " created_by, " +
                        " created_date)" +
                        " values(" +
                        " '" + msGetGid + "'," +
                        "'" + values.name_user + "'," +
                        "'" + values.relationship + "'," +
                        "'" + values.date_ofbirth + "'," +
                        "'" + lsemployee_gid + "'," +
                          "'" + user_gid + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Dependent Added Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Dependent";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaEducation(string user_gid, education values)
        {
            try
            {
                
                msSQL = " SELECT employee_gid FROM adm_mst_tuser a left join hrm_mst_temployee b on b.user_gid=a.user_gid WHERE a.user_gid='" + user_gid + "' ";
                lsemployee_gid = objdbconn.GetExecuteScalar(msSQL);
                msGetGid = objcmnfunctions.GetMasterGID("HRED");
                msSQL = " insert into hrm_tmp_teducation(" +
                                   " education_gid," +
                                   " institution_name," +
                                   " degree_diploma," +
                                   " field_study," +
                                   " date_completion," +
                                   " address_notes," +
                                   "employee_gid," +
                                   " created_by, " +
                                   " created_date)" +
                                   " values(" +
                                   " '" + msGetGid + "'," +
                                   "'" + values.inst_name + "'," +
                                    "'" + values.deg_dip + "'," +
                                      "'" + values.field_ofstudy + "'," +
                                    "'" + values.date_ofcompletion + "'," +
                                    "'" + values.addn_notes + "'," +
                                      "'" + lsemployee_gid + "'," +
                                      "'" + user_gid + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Education Added Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Education";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
    public void DaGetNominationSummary(MdlHrmTrnProfileManagement values, string employee_gid)
        {
            try
            {
                
                msSQL = " select familydtl_gid,familymember_name,dob,age,mobile_no,relationship,residing_with,residing_address,nominee_for from hrm_trn_tformfamilydetails where employee_gid='" + employee_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<nomination_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new nomination_list
                        {
                            familydtl_gid = dt["familydtl_gid"].ToString(),
                            familymember_name = dt["familymember_name"].ToString(),
                            dob = dt["dob"].ToString(),
                            age = dt["age"].ToString(),
                            mobile_no = dt["mobile_no"].ToString(),
                            relationship = dt["relationship"].ToString(),
                            residing_with = dt["residing_with"].ToString(),
                            residing_address = dt["residing_address"].ToString(),
                            nominee_for = dt["nominee_for"].ToString(),
                        });
                        values.nominationlist = getModuleList;
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
    public void DaGetEmergencyContactSummary(MdlHrmTrnProfileManagement values, string employee_gid)
        {
            try
            {
                
                msSQL = " select emergency_gid,contact_name,contact_address,contact_no,contact_email,reference_details from hrm_tmp_temergency where employee_gid='" + employee_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<emergencycontact_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new emergencycontact_list
                        {
                            emergency_gid = dt["emergency_gid"].ToString(),
                            contact_name = dt["contact_name"].ToString(),
                            contact_address = dt["contact_address"].ToString(),
                            contact_no = dt["contact_no"].ToString(),
                            contact_email = dt["contact_email"].ToString(),
                            reference_details = dt["reference_details"].ToString(),
                        });
                        values.emergencycontactlist = getModuleList;
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
        public bool DaGetEmployeeList(string user_gid, MdlHrmTrnProfileManagement values)
        {
            try
            {
                msSQL = " SELECT employee_gid FROM adm_mst_tuser a left join hrm_mst_temployee b on b.user_gid=a.user_gid WHERE a.user_gid='" + user_gid + "' ";
                lsemployee_gid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " Select distinct a.user_gid,c.useraccess,c.employee_emailid,c.employee_mobileno,case when c.entity_gid is null then c.entity_name else z.entity_name end as entity_name , " +
                     " a.user_code,concat(a.user_firstname,' ',a.user_lastname) as Name ,c.employee_joiningdate," +
                     " c.employee_gender,  " +
                     " concat(j.address1,' ',j.address2,'/', j.city,'/', j.state,'/',k.country_name,'/', j.postal_code) as emp_address, " +
                     " d.designation_name,c.designation_gid,c.employee_gid,c.employee_emailid,e.branch_name,concat(v.user_firstname,' ',v.user_lastname) as employeereporting_to, " +
                     " CASE " +
                     " WHEN v.user_status = 'Y' THEN 'Active'  " +
                     " WHEN v.user_status = 'N' THEN 'Inactive' " +
                     " END as user_status,c.department_gid,c.branch_gid, e.branch_name, g.department_name " +
                     " FROM adm_mst_tuser a " +
                     " left join hrm_mst_temployee c on a.user_gid = c.user_gid " +
                     " left join hrm_mst_temployee p on c.employeereporting_to = p.employee_gid " +
                     " left join adm_mst_tuser v on p.user_gid = v.user_gid " +
                     " left join adm_mst_tdesignation d on c.designation_gid = d.designation_gid " +
                     " left join hrm_mst_tbranch e on c.branch_gid = e.branch_gid " +
                     " left join hrm_mst_tdepartment g on g.department_gid = c.department_gid " +
                     " left join adm_mst_taddress j on c.employee_gid=j.parent_gid " +
                     " left join adm_mst_tcountry k on j.country_gid=k.country_gid " +
                     " left join adm_mst_tentity z on z.entity_gid=c.entity_gid" +
                     //" left join sys_mst_tbaselocation s on s.baselocation_gid=c.baselocation_gid" +
                     " left join hrm_trn_temployeedtl m on m.permanentaddress_gid=j.address_gid " +
                     " where a.user_gid='" + user_gid + "'" +
                     " group by c.employee_gid " +
                     " order by c.employee_gid desc  ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var get_holiday = new List<employeename_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        get_holiday.Add(new employeename_list
                        {
                            Name = dt["Name"].ToString(),
                            UserCode = dt["user_code"].ToString(),
                            Designation = dt["designation_name"].ToString(),
                            Branch = dt["branch_name"].ToString(),
                            Department = dt["department_name"].ToString(),
                            Joiningdate = dt["employee_joiningdate"].ToString(),
                            Address = dt["emp_address"].ToString(),
                            Gender = dt["employee_gender"].ToString(),
                            email = dt["employee_emailid"].ToString(),
                            employeemobileNo = dt["employee_mobileno"].ToString(),
                        });
                    }
                    values.employeenamelist = get_holiday;
                }
                dt_datatable.Dispose();
                values.status = true;
                return true;
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                return false;
            }
        }
        public void DaGetDependentSummary(MdlHrmTrnProfileManagement values, string employee_gid)
        {
            try
            {
                
                msSQL = " select dependent_gid,name,relationship,dob from hrm_tmp_tdependent where employee_gid='" + employee_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<dependent_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new dependent_list
                        {
                            dependent_gid = dt["dependent_gid"].ToString(),
                            name = dt["name"].ToString(),
                            relationship = dt["relationship"].ToString(),
                            dob = dt["dob"].ToString(),

                        });
                        values.dependentlist = getModuleList;
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

    public void DaGetEducationSummary(MdlHrmTrnProfileManagement values, string employee_gid)
        {
            try
            {
                
                msSQL = " select education_gid,institution_name,degree_diploma,field_study,date_completion,address_notes from hrm_tmp_teducation where employee_gid='" + employee_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<education_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new education_list
                        {
                            education_gid = dt["education_gid"].ToString(),
                            institution_name = dt["institution_name"].ToString(),
                            degree_diploma = dt["degree_diploma"].ToString(),
                            field_study = dt["field_study"].ToString(),
                            date_completion = dt["date_completion"].ToString(),
                            address_notes = dt["address_notes"].ToString(),
                        });
                        values.educationlist = getModuleList;
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
        public void DaGetCompanyPolicies(MdlHrmTrnProfileManagement values)
        {
            try
            {
                
                msSQL = " select policy_name,policy_desc from hrm_trn_tcompanypolicy; ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<CompanyPolicy>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new CompanyPolicy
                        {
                            policy_name = dt["policy_name"].ToString(),
                            policy_desc = dt["policy_desc"].ToString(),
                        });
                        values.CompanyPolicy = getModuleList;
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
        public void DaGetBloodGroup(MdlHrmTrnProfileManagement values)
        {
            try
            {
                
                msSQL = " SELECT a.bloodgroup_gid ,a.api_code,a.bloodgroup_name, date_format(a.created_date,'%d-%m-%Y %h:%i %p') as created_date, " +
                        " concat(c.user_firstname,' ',c.user_lastname,' / ',c.user_code) as created_by," +
                        " case when a.status='N' then 'Inactive' else 'Active' end as status" +
                        " FROM sys_mst_tbloodgroup a" +
                        " left join hrm_mst_temployee b on a.created_by = b.employee_gid" +
                        " left join adm_mst_tuser c on c.user_gid = b.user_gid where a.delete_flag='N' order by a.bloodgroup_gid  desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getbloodgroup_list = new List<bloodgroup_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        getbloodgroup_list.Add(new bloodgroup_list
                        {
                            bloodgroup_gid = (dr_datarow["bloodgroup_gid"].ToString()),
                            api_code = (dr_datarow["api_code"].ToString()),
                            bloodgroup_name = (dr_datarow["bloodgroup_name"].ToString()),
                            created_by = (dr_datarow["created_by"].ToString()),
                            created_date = (dr_datarow["created_date"].ToString()),
                            status = (dr_datarow["status"].ToString()),
                        });
                    }
                    values.bloodgroup_list = getbloodgroup_list;
                }
                dt_datatable.Dispose();
                values.status = true;
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetrelationshipwithemployee(MdlHrmTrnProfileManagement values)
        {
            try
            {
                
                msSQL = " select relationship, relation_gid from hrm_mst_tformrelation ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<relationshipwith_employee_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new relationshipwith_employee_list
                        {
                            relation_gid = dt["relation_gid"].ToString(),
                            relationship = dt["relationship"].ToString(),
                        });
                        values.relationshipwith_employee_list = getModuleList;
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
    }

}
