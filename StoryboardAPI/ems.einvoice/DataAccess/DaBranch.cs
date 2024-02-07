﻿using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using ems.einvoice.Models;
using System.Configuration;
using System.IO;
using MySql.Data.MySqlClient;
using static OfficeOpenXml.ExcelErrorValue;

namespace ems.einvoice.DataAccess
{
    public class DaBranch
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        MySqlDataReader objMySqlDataReader;
        DataTable dt_datatable;
        string msGetGid1;
        int mnResult;
        public void DaBranchSummary(MdlBranch values)
        {
            try
            {

                msSQL = " select  a.branch_gid,a.branch_code, a.branch_name, a.branch_prefix,  concat(c.user_firstname,'',c.user_lastname) as branchmanager_gid " +
                        " from hrm_mst_tbranch a " +
                        " left join hrm_mst_temployee b on b.employee_gid= a.branchmanager_gid " +
                        " left join adm_mst_tuser c on b.user_gid = c.user_gid";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<branch_list1>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new branch_list1
                        {
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_code = dt["branch_code"].ToString(),
                            branch_prefix = dt["branch_prefix"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            branchmanager_gid = dt["branchmanager_gid"].ToString()
                        });
                        values.branch_list1 = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }

            catch (Exception ex)
            {
                values.message = "Exception occured while loading Branch details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
         "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

          
        }
        public void DaBranchSummarydetail(string branch_gid, branch_list1 values)
        {
            try
            {
                
                msSQL = " update hrm_mst_tbranch set" +
                        " branch_logo=?," +
                        " address1='" + (values.Branch_address).Trim().Replace("'", "") + "'," +
                        " city='" + values.City + "'," +
                        " state='" + values.State + "'," +
                        " postal_code='" + values.Postal_code + "', " +
                        " contact_number='" + values.Phone_no + "'," +
                        " email_id='" + values.Email_address + "', " +
                        " gst_no='" + values.GST_no + "' " +
                        " where branch_gid='" + values.branch_gid + "'";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Branch Updated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Branch !!";
                }
            }

            catch (Exception ex)
            {
                values.message = "Exception occured while updating branch details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

           
        }
        public void DaPostBranch(string user_gid, branch_list1 values)
        {
            try
            {

                msSQL = "select branch_gid from hrm_mst_tbranch where branch_name='" + (values.branch_name).Trim().Replace("'", "") + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == false)
                {
                    msGetGid1 = objcmnfunctions.GetMasterGID("HBHM");
                    msSQL = " insert into hrm_mst_tbranch(" +
                            " branch_gid," +
                            " branch_code," +
                            " branch_name," +
                            " branch_prefix," +
                            " created_by, " +
                            " created_date)" +
                            " values(" +
                            " '" + msGetGid1 + "'," +
                            " '" + values.branch_code + "'," +
                            "'" + (values.branch_name).Trim().Replace("'", "") + "'," +
                            "'" + (values.branch_prefix).Trim().Replace("'", "") + "',";
                    msSQL += "'" + user_gid + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Branch Added Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Branch";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Branch Name Already Exist";
                }
            }

            catch (Exception ex)
            {
                values.message = "Exception occured while Adding branch details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
         "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/RBL/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

           
        }
        public void DaUpdatedbranchlogo (HttpRequest httpRequest, result objResult, string user_gid)
        {
            try
            {

                HttpFileCollection httpFileCollection;
                string lsfilepath = string.Empty;
                string lsdocument_gid = string.Empty;
                MemoryStream ms_stream = new MemoryStream();
                string document_gid = string.Empty;
                string lscompany_code = string.Empty;
                HttpPostedFile httpPostedFile;
                string lspath;
                string msGetGid;

                msSQL = " SELECT  a.company_code  FROM adm_mst_tcompany a ";
                lscompany_code = objdbconn.GetExecuteScalar(msSQL);
                string branch_gid = httpRequest.Form[0];

                string branch_code = httpRequest.Form[1];
                string Branch_address = httpRequest.Form[2];
                string City = httpRequest.Form[3];
                string State = httpRequest.Form[4];
                string Postal_code = httpRequest.Form[5];
                string Email_address = httpRequest.Form[6];
                string Phone_no = httpRequest.Form[7];
                string GST_no = httpRequest.Form[7];

                MemoryStream ms = new MemoryStream();
                lspath = ConfigurationManager.AppSettings["imgfile_path"] + "/erpdocument" + "/" + lscompany_code + "/" + "Company/" + "/" + DateTime.Now.Year + "/" + DateTime.Now.Month;

                {
                    if ((!System.IO.Directory.Exists(lspath)))
                        System.IO.Directory.CreateDirectory(lspath);
                }

                try
                {
                    if (httpRequest.Files.Count > 0)
                    {
                        string lsfirstdocument_filepath = string.Empty;
                        httpFileCollection = httpRequest.Files;

                        for (int i = 0; i < httpFileCollection.Count; i++)
                        {
                            string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                            httpPostedFile = httpFileCollection[i];
                            string FileExtension = httpPostedFile.FileName;
                            //string lsfile_gid = msdocument_gid + FileExtension;
                            string lsfile_gid = msdocument_gid;
                            string lscompany_document_flag = string.Empty;
                            FileExtension = Path.GetExtension(FileExtension).ToLower();
                            lsfile_gid = lsfile_gid + FileExtension;
                            Stream ls_readStream;
                            ls_readStream = httpPostedFile.InputStream;
                            ls_readStream.CopyTo(ms);

                            lspath = ConfigurationManager.AppSettings["imgfile_path"] + "/erpdocument" + "/" + "/" + lscompany_code + "/" + "Company/" + "/" + DateTime.Now.Year + "/" + DateTime.Now.Month;
                            string status;
                            status = objcmnfunctions.uploadFile(lspath + msdocument_gid, FileExtension);
                            //string local_path = "E:/Angular15/AngularUI/src";
                            ms.Close();
                            //lspath = "assets/media/images/erpdocument" + "/" + "/" + lscompany_code + "/" + "Company/" + "/" + DateTime.Now.Year + "/" + DateTime.Now.Month;

                            string final_path = lspath + msdocument_gid + FileExtension;

                            msSQL = " update hrm_mst_tbranch set " +
                                    " branch_code='" + branch_code + "'," +
                                    " address1='" + Branch_address + "'," +
                                    " city = '" + City + "'," +
                                    " state = '" + State + "'," +
                                    " postal_code = '" + Postal_code + "'," +
                                    " contact_number = '" + Phone_no + "'," +
                                    " email_id = '" + Email_address + "'," +
                                    " gst_no = '" + GST_no + "'," +
                                    " branch_logo_path='" + final_path + "'," +
                                    " updated_by = '" + user_gid + "'," +
                                    " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                                    " where branch_gid='" + branch_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                    }

                    if (mnResult != 0)
                    {
                        objResult.status = true;
                        objResult.message = "branch's Additional Information Added Successfully !!";
                    }
                    else
                    {
                        objResult.status = false;
                        objResult.message = "Error While Adding branch Additional Information !!";
                    }
                }

                catch (Exception ex)
                {
                    objResult.message = ex.ToString();
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.ToString() +
         "***********" + objResult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                }
            }

            catch (Exception ex)
            {
                objResult.message = "Exception occured while adding branch Additional Information!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.ToString() +
         "***********" + objResult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
          
        }
        public void DagetUpdatedBranch(string user_gid, branch_list1 values)
        {
            try
            {

                msSQL = " select branch_name from hrm_mst_tbranch where branch_name = '" + (values.branch_name_edit).Trim().Replace("'", "") + "' ";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == false)
                {
                    msSQL = " update  hrm_mst_tbranch set " +
                            " branch_code = '" + values.branch_code_edit + "'," +
                            " branch_name = '" + (values.branch_name_edit).Trim().Replace("'", "") + "'," +
                            " branch_prefix = '" + (values.branch_prefix_edit).Trim().Replace("'", "") + "'," +
                            " updated_by = '" + user_gid + "'," +
                            " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where branch_gid='" + values.branch_gid + "'  ";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Branch Updated Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Branch !!";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Branch Name Already Exist !!";
                }
            }
            
            catch (Exception ex)
            {
                values.message = "Exception occured while updating branch details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
         "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
           
        }
        public void DaDeleteBranch(string branch_gid, branch_list1 values)
        {
            try
            {

                msSQL = "  delete from hrm_mst_tbranch where branch_gid='" + branch_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Branch Deleted Successfully";
                }
                else
                {
                    {
                        values.status = false;
                        values.message = "Error While Deleting Branch";
                    }
                }
            }

            catch (Exception ex)
            {
                values.message = "Exception occured while deleting branch details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.ToString() +
         "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

           
        }
    }
}




    
