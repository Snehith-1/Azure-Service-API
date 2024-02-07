﻿using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Web;
using ems.crm.Models;
using OfficeOpenXml.Style;
using MySql.Data.MySqlClient;


namespace ems.crm.DataAccess
{
    public class DaCrmDashboard
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        MySqlDataReader objMySqlDataReader;
        DataTable dt_datatable;
        string msGetGid;
        int mnResult, mnResult1;

        // My Calls Count
        public void DaGetDashboardCount(string employee_gid, string user_gid, MdlCrmDashboard values)
        {
            try
            {
                 
                msSQL = " select (select count(lead2campaign_gid) from crm_trn_tlead2campaign " +
                    " where leadstage_gid IN (1, 2) and " +
                    " assign_to='" + employee_gid + "') as mycalls_count, " +
                    " (select count(lead2campaign_gid) from crm_trn_tlead2campaign " +
                    " where assign_to='" + employee_gid + "') as myleads_count," +
                    " (select count(schedulelog_gid) from crm_trn_tschedulelog " +
                    " where assign_to='" + employee_gid + "' and schedule_status='Pending' and schedule_date >= curdate()) as myappointments_count," +
                    " (select count(schedulelog_gid) from crm_trn_tschedulelog " +
                    " where assign_to='" + employee_gid + "' and schedule_status='Pending') as assignvisit_count," +
                    " (select count(schedulelog_gid) from crm_trn_tschedulelog " +
                    " where assign_to='" + employee_gid + "' and schedule_status='Closed') as completedvisit_count," +
                    " (select count(proposal_gid) from crm_mst_tproposaltemplate " +
                    " where created_by='" + user_gid + "') as shared_proposal," +
                    " (select count(salesorder_gid) from smr_trn_tsalesorder where" +
                    " salesorder_status<>'SO Amended' and created_by='" + employee_gid + "') as completedorder_count," +
                    " (select count(enquiry_gid) from smr_trn_tsalesenquiry a" +
                    " left join smr_trn_tsalesorder b on b.created_by = a. created_by" +
                    " where a.created_by='" + employee_gid + "' and b.salesorder_status<>'SO Amended') as totalorder_count";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getDashboardList = new List<getDashboardCount_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getDashboardList.Add(new getDashboardCount_List
                        {
                            mycalls_count = (dt["mycalls_count"].ToString()),
                            myleads_count = (dt["myleads_count"].ToString()),
                            myappointments_count = (dt["myappointments_count"].ToString()),
                            assignvisit_count = (dt["assignvisit_count"].ToString()),
                            completedvisit_count = (dt["completedvisit_count"].ToString()),
                            shared_proposal = (dt["shared_proposal"].ToString()),
                            completedorder_count = (dt["completedorder_count"].ToString()),
                            totalorder_count = (dt["totalorder_count"].ToString()),
                        });
                        values.getDashboardCount_List = getDashboardList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Dashboard count!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        // My Calls Count
        public void DaGetDashboardQuotationAmount(string employee_gid, string user_gid, MdlCrmDashboard values)
        {


            try
            {
                 
                //msSQL = "SET GLOBAL sql_mode=\"STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION\"";
                //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                //msSQL = "SET SESSION sql_mode=\"STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION\"";
                //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                //msSQL = "SELECT @@GLOBAL.sql_mode";
                //string lsglobal = objdbconn.GetExecuteScalar(msSQL);

                //msSQL = "SELECT @@SESSION.sql_mode";
                //string lssession = objdbconn.GetExecuteScalar(msSQL);


                string YearLabel = DateTime.Now.Year.ToString();

                msSQL = " select year(quotation_date) as year ,MONTHNAME(quotation_date) month_name, " +
                        " FORMAT(sum(total_amount), 2) as total_amount from smr_trn_treceivequotation " +
                        " where quotation_date like '%" + YearLabel + "%'  " +
                        " group by year(quotation_date),month(quotation_date) order by year(quotation_date), " +
                        " month(quotation_date) ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getDashboardQuotationList = new List<getDashboardQuotationAmt_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getDashboardQuotationList.Add(new getDashboardQuotationAmt_List
                        {
                            year = (dt["year"].ToString()),
                            month_name = (dt["month_name"].ToString()),
                            total_amount = (dt["total_amount"].ToString()),
                        });
                        values.getDashboardQuotationAmt_List = getDashboardQuotationList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Dashboard Quotation Amount!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }           
        }
        public void DaGetBarchartMonthlyLead(string employee_gid, string user_gid, MdlCrmDashboard values)
        {

            try
            {
                 
                //msSQL = "SET GLOBAL sql_mode=\"STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION\"";
                //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                //msSQL = "SET SESSION sql_mode=\"STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION\"";
                //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                //msSQL = "SELECT @@GLOBAL.sql_mode";
                //string lsglobal = objdbconn.GetExecuteScalar(msSQL);

                //msSQL = "SELECT @@SESSION.sql_mode";
                //string lssession = objdbconn.GetExecuteScalar(msSQL);

                string YearLabel = DateTime.Now.Year.ToString();

                msSQL = " select year(b.created_date) as year ,substring(date_format(b.created_date,'%M'),1,3)as month_name," +
                        " MONTH(b.created_date) AS month_number,count(a.leadbank_gid) as lead_count from crm_trn_tleadbank a  " +
                        " left join crm_trn_tlead2campaign b on b.leadbank_gid = a.leadbank_gid  " +
                        " where b.created_date like '%" + YearLabel + "%'  and b.assign_to= '" + employee_gid + "' GROUP BY year,month_name  order by year,month_name  ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getDashboardQuotationList = new List<getleadbasedonemployee_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getDashboardQuotationList.Add(new getleadbasedonemployee_List
                        {
                            lead_year = (dt["year"].ToString()),
                            lead_monthname = (dt["month_name"].ToString()),
                            lead_count = (dt["lead_count"].ToString()),
                        });
                        values.getleadbasedonemployee_List = getDashboardQuotationList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting BarChart Monthly Lead!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }        
        }

        //Social Media Lead count

        //public void DaGetsocialmedialeadcount(string employee_gid, string user_gid, MdlCrmDashboard values)
        //{


        //    try
        //    {
        //        objdbconn.OpenConn();
        //        msSQL = "select source_gid from crm_mst_tsource where source_name = 'Whatsapp'";
        //        string whatsapp_gid = objdbconn.GetExecuteScalar(msSQL);

        //        msSQL = "select source_gid from crm_mst_tsource where source_name = 'Mail'";
        //        string mail_gid = objdbconn.GetExecuteScalar(msSQL);

        //        msSQL = "select source_gid from crm_mst_tsource where source_name ='Shopify'";
        //        string shopify_gid = objdbconn.GetExecuteScalar(msSQL);

        //        msSQL = " select(select count(leadbank_gid)from crm_trn_tleadbank where source_gid = '" + null + "') as whatsapp_count," +
        //                " (select count(leadbank_gid)  from crm_trn_tleadbank where source_gid = '" + null + "') as mail_count," +
        //                " (select count(leadbank_gid)  from crm_trn_tleadbank where source_gid = '" + null + "') as shopify_count";

        //        dt_datatable = objdbconn.GetDataTable(msSQL);
        //        var getDashboardQuotationList = new List<socialmedialeadcount>();
        //        if (dt_datatable.Rows.Count != 0)
        //        {
        //            foreach (DataRow dt in dt_datatable.Rows)
        //            {
        //                getDashboardQuotationList.Add(new socialmedialeadcount
        //                {
        //                    whatsapp_count = dt["whatsapp_count"].ToString(),
        //                    shopify_count = dt["shopify_count"].ToString(),
        //                    mail_count = dt["mail_count"].ToString(),
        //                });
        //                values.socialmedialeadcount = getDashboardQuotationList;
        //            }
        //        }
        //        dt_datatable.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        values.message = "Exception occured while Getting Social Media Lead Count!";
        //    }
        //    finally
        //    {
        //        if (objMySqlDataReader != null)
        //            objMySqlDataReader.Close();
        //        objdbconn.CloseConn();
        //    }


        //}


        public void DaGetsocialmedialeadcount(MdlCrmDashboard values)
        {
            try
            {
                 

                string whatsappgid = string.Empty;
                string mailgid = string.Empty;
                string shopifygid = string.Empty;
                msSQL = "select source_gid from crm_mst_tsource where source_name = 'Whatsapp'";
                string whatsapp_gid = objdbconn.GetExecuteScalar(msSQL);
                if (whatsapp_gid == null || whatsapp_gid == "")
                {
                    whatsappgid = null;
                }
                else
                {
                    whatsappgid = whatsapp_gid;
                }

                msSQL = "select source_gid from crm_mst_tsource where source_name = 'Mail'";
                string mail_gid = objdbconn.GetExecuteScalar(msSQL);
                if (mail_gid == null || mail_gid == "")
                {
                    mailgid = null;
                }
                else
                {
                    mailgid = mail_gid;
                }

                msSQL = "select source_gid from crm_mst_tsource where source_name ='Shopify'";
                string shopify_gid = objdbconn.GetExecuteScalar(msSQL);
                if (shopify_gid == null || shopify_gid == "")
                {
                    shopifygid = null;
                }
                else
                {
                    shopifygid = shopify_gid;
                }
                msSQL = " select(select count(leadbank_gid)from crm_trn_tleadbank where source_gid = '" + whatsappgid + "') as whatsapp_count," +
                        " (select count(leadbank_gid)  from crm_trn_tleadbank where source_gid = '" + mailgid + "') as mail_count," +
                        " (select count(leadbank_gid)  from crm_trn_tleadbank where source_gid = '" + shopifygid + "') as shopify_count";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getDashboardQuotationList = new List<socialmedialeadcount>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getDashboardQuotationList.Add(new socialmedialeadcount
                        {
                            whatsapp_count = dt["whatsapp_count"].ToString(),
                            shopify_count = dt["shopify_count"].ToString(),
                            mail_count = dt["mail_count"].ToString(),
                        });
                        values.socialmedialeadcount = getDashboardQuotationList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Social Media Lead Count";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }


    }
}