﻿using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.sales.Models;
using MySql.Data.MySqlClient;

namespace ems.sales.DataAccess
{
    public class DaSmrTrnSalesManager
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        MySqlDataReader objMySqlDataReader;
        DataTable dt_datatable;

        public void DaGetSalesManagerTotal(MdlSmrTrnSalesManager values)
        {
            try
            {
               
                msSQL = " Select j.branch_name,c.leadbank_gid,k.campaign_title,c.leadbank_name," +
                 " concat(b.contact_person,' / ',b.contact_number,' / ',b.contact_email)" +
                 " as contact_details, concat(c.leadbank_city,'/',c.leadbank_state) as region_name," +
                 " Case when a.internal_notes is not null then a.internal_notes when a.internal_notes" +
                 " is null then b.enquiry_remarks  end as internal_notes, concat(f.user_firstname,' ',f.user_lastname)" +
                 " AS assigned_to , i.department_name, concat(y.user_firstname,' ',y.user_lastname)As created_by," +
                 " a.lead2campaign_gid, a.enquiry_gid, a.campaign_gid, g.leadbankcontact_gid,z.leadstage_name" +
                 " From crm_trn_tenquiry2campaign a" +
                 " left join smr_trn_tsalesenquiry b on a.enquiry_gid = b.enquiry_gid " +
                 " left join crm_trn_tleadbank c on b.customer_gid=c.customer_gid " +
                 " left join crm_trn_tleadbankcontact g on c.leadbank_gid = g.leadbank_gid " +
                 " left join hrm_mst_temployee e on a.assign_to = e.employee_gid " +
                 " left join adm_mst_tuser f on e.user_gid = f.user_gid " +
                 " left join hrm_mst_tdepartment i on e.department_gid=i.department_gid " +
                 " left join hrm_mst_tbranch j on e.branch_gid=j.branch_gid " +
                 " left join smr_trn_tcampaign k on a.campaign_gid=k.campaign_gid " +
                 " left join hrm_mst_temployee x on a.created_by=x.employee_gid " +
                 " left join adm_mst_tuser y on x.user_gid=y.user_gid " +
                 " left join crm_mst_tenquiry z on z.leadstage_gid=a.leadstage_gid " +
                 " where a.leadstage_gid in ('1', '3', '4', '5') order by b.customer_name asc";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<totalall_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new totalall_list
                    {
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                        lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                        campaign_gid = dt["campaign_gid"].ToString(),
                        campaign_title = dt["campaign_title"].ToString(),
                        department_name = dt["department_name"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        assigned_to = dt["assigned_to"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        internal_notes = dt["internal_notes"].ToString(),
                        leadstage_name = dt["leadstage_name"].ToString(),
                        region_name = dt["region_name"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                        created_by = dt["created_by"].ToString()
                    });
                    values.totalalllist = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Sales Manager Total !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }


        public void DaGetSalesManagerComplete(MdlSmrTrnSalesManager values)
        {
            try
            {
                
                msSQL = " Select j.branch_name,  k.campaign_title,c.leadbank_name, " +
                  " concat(b.contact_person,' / ',b.contact_number,' / ',b.contact_email)" +
                  " as contact_details, concat(c.leadbank_city,'/',c.leadbank_state) as region_name," +
                  " Case when a.internal_notes is not null then a.internal_notes when a.internal_notes" +
                  " is null then b.enquiry_remarks  end as internal_notes, concat(f.user_firstname,' ',f.user_lastname)" +
                  " AS assigned_to , i.department_name, concat(y.user_firstname,' ',y.user_lastname)As created_by," +
                  " a.lead2campaign_gid, a.enquiry_gid, a.campaign_gid, g.leadbankcontact_gid" +
                  " From crm_trn_tenquiry2campaign a" +
                  " left join smr_trn_tsalesenquiry b on a.enquiry_gid = b.enquiry_gid " +
                  " left join crm_trn_tleadbank c on b.customer_gid=c.customer_gid " +
                  " left join hrm_mst_temployee e on a.assign_to = e.employee_gid " +
                  " left join adm_mst_tuser f on e.user_gid = f.user_gid " +
                  " left join crm_trn_tleadbankcontact g on c.leadbank_gid = g.leadbank_gid " +
                  " left join hrm_mst_tdepartment i on e.department_gid=i.department_gid " +
                  " left join hrm_mst_tbranch j on e.branch_gid=j.branch_gid " +
                  " left join smr_trn_tcampaign k on a.campaign_gid=k.campaign_gid " +
                  " left join hrm_mst_temployee x on a.created_by=x.employee_gid" +
                  " left join crm_mst_tenquiry z on z.leadstage_gid=a.leadstage_gid" +
                  " left join adm_mst_tuser y on x.user_gid=y.user_gid " +
                  " where a.leadstage_gid ='4' order by b.customer_name asc";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<complete_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new complete_list
                    {
                        campaign_gid = dt["campaign_gid"].ToString(),
                        campaign_title = dt["campaign_title"].ToString(),
                        department_name = dt["department_name"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        assigned_to = dt["assigned_to"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        internal_notes = dt["internal_notes"].ToString(),
                        //leadstage_name = dt["leadstage_name"].ToString(),
                        region_name = dt["region_name"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                        created_by = dt["created_by"].ToString()
                    });
                    values.completelist = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Manager Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }

        //Prospect summary

        public void DaGetSalesManagerProspect(MdlSmrTrnSalesManager values)
        {
            try
            {
                
                msSQL = " Select j.branch_name,  k.campaign_title,c.leadbank_name, " +
                    " concat(b.contact_person,' / ',b.contact_number,' / ',b.contact_email)" +
                    " as contact_details,concat(c.leadbank_city,'/',c.leadbank_state) as region_name," +
                    " Case when a.internal_notes is not null then a.internal_notes when a.internal_notes" +
                    " is null then b.enquiry_remarks  end as internal_notes, concat(f.user_firstname,' ',f.user_lastname)" +
                    " AS assigned_to , i.department_name, concat(y.user_firstname,' ',y.user_lastname)As created_by," +
                    " a.lead2campaign_gid, a.enquiry_gid, a.campaign_gid, g.leadbankcontact_gid" +
                    " From crm_trn_tenquiry2campaign a" +
                    " left join smr_trn_tsalesenquiry b on a.enquiry_gid = b.enquiry_gid " +
                    " left join crm_trn_tleadbank c on b.customer_gid=c.customer_gid " +
                    " left join crm_trn_tleadbankcontact g on c.leadbank_gid = g.leadbank_gid " +
                    " left join hrm_mst_temployee e on a.assign_to = e.employee_gid " +
                    " left join adm_mst_tuser f on e.user_gid = f.user_gid " +
                    " left join hrm_mst_tdepartment i on e.department_gid=i.department_gid " +
                    " left join hrm_mst_tbranch j on e.branch_gid=j.branch_gid " +
                    " left join smr_trn_tcampaign k on a.campaign_gid=k.campaign_gid " +
                    " left join hrm_mst_temployee x on a.created_by=x.employee_gid " +
                    " left join adm_mst_tuser y on x.user_gid=y.user_gid " +
                    " where a.leadstage_gid ='1' order by b.customer_name asc";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<prospects_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new prospects_list
                    {
                        campaign_gid = dt["campaign_gid"].ToString(),
                        campaign_title = dt["campaign_title"].ToString(),
                        department_name = dt["department_name"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        assigned_to = dt["assigned_to"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        internal_notes = dt["internal_notes"].ToString(),
                        //leadstage_name = dt["leadstage_name"].ToString(),
                        region_name = dt["region_name"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                        created_by = dt["created_by"].ToString()
                    });
                    values.prospectslist = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Manager Prospect !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }

        //Potentials summary
        public void DaGetSalesManagerPotential(MdlSmrTrnSalesManager values)
        {
            try
            {
               
                msSQL = " Select j.branch_name,  k.campaign_title,c.leadbank_name, " +
                " concat(b.contact_person,' / ',b.contact_number,' / ',b.contact_email)" +
                " as contact_details,concat(c.leadbank_city,'/',c.leadbank_state) as region_name," +
                " Case when a.internal_notes is not null then a.internal_notes when a.internal_notes" +
                " is null then b.enquiry_remarks  end as internal_notes, concat(f.user_firstname,' ',f.user_lastname)" +
                " AS assigned_to , i.department_name, concat(y.user_firstname,' ',y.user_lastname)As created_by," +
                " a.lead2campaign_gid, a.enquiry_gid, a.campaign_gid, g.leadbankcontact_gid" +
                " From crm_trn_tenquiry2campaign a" +
                " left join smr_trn_tsalesenquiry b on a.enquiry_gid = b.enquiry_gid " +
                " left join crm_trn_tleadbank c on b.customer_gid=c.customer_gid " +
                " left join crm_trn_tleadbankcontact g on c.leadbank_gid = g.leadbank_gid " +
                " left join hrm_mst_temployee e on a.assign_to = e.employee_gid " +
                " left join adm_mst_tuser f on e.user_gid = f.user_gid " +
                " left join hrm_mst_tdepartment i on e.department_gid=i.department_gid " +
                " left join hrm_mst_tbranch j on e.branch_gid=j.branch_gid " +
                " left join smr_trn_tcampaign k on a.campaign_gid=k.campaign_gid " +
                " left join hrm_mst_temployee x on a.created_by=x.employee_gid " +
                " left join adm_mst_tuser y on x.user_gid=y.user_gid " +
                " where a.leadstage_gid ='5'  order by b.customer_name asc";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<potentials_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new potentials_list
                    {
                        campaign_gid = dt["campaign_gid"].ToString(),
                        campaign_title = dt["campaign_title"].ToString(),
                        department_name = dt["department_name"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        assigned_to = dt["assigned_to"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        internal_notes = dt["internal_notes"].ToString(),
                        //leadstage_name = dt["leadstage_name"].ToString(),
                        region_name = dt["region_name"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                        created_by = dt["created_by"].ToString()
                    });
                    values.potentialslist = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Manager Potential !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }

        public void DaGetSalesManagerDrop(MdlSmrTrnSalesManager values)
        {
            try
            {
               
                msSQL = " Select j.branch_name,  k.campaign_title,c.leadbank_name," +
               " concat(b.contact_person,' / ',b.contact_number,' / ',b.contact_email)" +
               " as contact_details, concat(c.leadbank_city,'/',c.leadbank_state) as region_name," +
               " Case when a.internal_notes is not null then a.internal_notes when a.internal_notes" +
               " is null then b.enquiry_remarks  end as internal_notes, concat(f.user_firstname,' ',f.user_lastname)" +
               " AS assigned_to , i.department_name, concat(y.user_firstname,' ',y.user_lastname)As created_by," +
               " a.lead2campaign_gid, a.enquiry_gid, a.campaign_gid, g.leadbankcontact_gid" +
               " From crm_trn_tenquiry2campaign a" +
               " left join smr_trn_tsalesenquiry b on a.enquiry_gid = b.enquiry_gid " +
               " left join crm_trn_tleadbank c on b.customer_gid=c.customer_gid " +
               " left join crm_trn_tleadbankcontact g on c.leadbank_gid = g.leadbank_gid " +
               " left join hrm_mst_temployee e on a.assign_to = e.employee_gid " +
               " left join adm_mst_tuser f on e.user_gid = f.user_gid " +
               " left join hrm_mst_tdepartment i on e.department_gid=i.department_gid " +
               " left join hrm_mst_tbranch j on e.branch_gid=j.branch_gid " +
               " left join smr_trn_tcampaign k on a.campaign_gid=k.campaign_gid " +
               " left join hrm_mst_temployee x on a.created_by=x.employee_gid " +
               " left join adm_mst_tuser y on x.user_gid=y.user_gid " +
               " where a.leadstage_gid ='3' order by b.customer_name asc";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<drops_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new drops_list
                    {
                        campaign_gid = dt["campaign_gid"].ToString(),
                        campaign_title = dt["campaign_title"].ToString(),
                        department_name = dt["department_name"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        assigned_to = dt["assigned_to"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        internal_notes = dt["internal_notes"].ToString(),
                        //leadstage_name = dt["leadstage_name"].ToString(),
                        region_name = dt["region_name"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                        created_by = dt["created_by"].ToString()
                    });
                    values.dropstatuslist = getModuleList;
                }

            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Manager Drop !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }

        //Count summary

        public void DaGetSmrTrnManagerCount(string employee_gid, string user_gid, MdlSmrTrnSalesManager values)
        {
            try
            {
               
                msSQL = " select(select count(lead2campaign_gid) from crm_trn_tenquiry2campaign where leadstage_gid in ('1','3','5','6','4')) as employeecount," +
                    " (select count(lead2campaign_gid) from crm_trn_tenquiry2campaign where leadstage_gid = '1') as prospect, " +
                    " (select count(lead2campaign_gid) from crm_trn_tenquiry2campaign where leadstage_gid = '5') as potential, " +
                    " (select count(lead2campaign_gid) from crm_trn_tenquiry2campaign where leadstage_gid = '4') as completed, " +
                    " (select count(lead2campaign_gid) from crm_trn_tenquiry2campaign where leadstage_gid = '3') as drop_status";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var customercount_list = new List<teammanagercount_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    customercount_list.Add(new teammanagercount_list
                    {
                        employeecount = dt["employeecount"].ToString(),
                        prospect = dt["prospect"].ToString(),
                        potential = dt["potential"].ToString(),
                        completed = dt["completed"].ToString(),
                        drop_status = dt["drop_status"].ToString(),
                    });
                    values.teamcount_list = customercount_list;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Manager Count !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }
    }
}