import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmsUtilitiesModule } from '../ems.utilities/ems.utilities.module';
import { HrmTrnAdmincontrolComponent } from './component/hrm-trn-admincontrol/hrm-trn-admincontrol.component';
import { HrmMstEmployeeviewComponent } from './component/hrm-mst-employeeview/hrm-mst-employeeview.component';
import { HrmTrnEmployeeonboardComponent } from './component/hrm-trn-employeeonboard/hrm-trn-employeeonboard.component';
import { HrmTrnEmployeeonboardaddComponent } from './component/hrm-trn-employeeonboardadd/hrm-trn-employeeonboardadd.component';
import { HrmTrnEmployeeonboardEditComponent } from './component/hrm-trn-employeeonboard-edit/hrm-trn-employeeonboard-edit.component';
import { HrmTrnEmployeeboardviewpendingComponent } from './component/hrm-trn-employeeboardviewpending/hrm-trn-employeeboardviewpending.component';
import { HrmTrnEmployeeonboardViewCompletedComponent } from './component/hrm-trn-employeeonboard-view-completed/hrm-trn-employeeonboard-view-completed.component';
import { HrmMstSubfunctionComponent } from './component/hrm-mst-subfunction/hrm-mst-subfunction.component';
import { HrmMstBaselocationComponent } from './component/hrm-mst-baselocation/hrm-mst-baselocation.component';
import { HrmMstTeammasterComponent } from './component/hrm-mst-teammaster/hrm-mst-teammaster.component';
import { HrmMstRoleSummaryComponent } from './component/hrm-mst-role-summary/hrm-mst-role-summary.component';
import { HrmMstRoleAddComponent } from './component/hrm-mst-role-add/hrm-mst-role-add.component';
import { HrmMstRoleEditComponent } from './component/hrm-mst-role-edit/hrm-mst-role-edit.component';
import { HrmMstBranchSummaryComponent } from './component/hrm-mst-branch-summary/hrm-mst-branch-summary.component';
import { HrmMstDepartmentSummaryComponent } from './component/hrm-mst-department-summary/hrm-mst-department-summary.component';
import { HrmMstDesignationComponent } from './component/hrm-mst-designation/hrm-mst-designation.component';
import { HrmMstEntityComponent } from './component/hrm-mst-entity/hrm-mst-entity.component';
import { HrmMstBloodgroupComponent } from './component/hrm-mst-bloodgroup/hrm-mst-bloodgroup.component';
import { HrmMstHrdocumentComponent } from './component/hrm-mst-hrdocument/hrm-mst-hrdocument.component';
import { HrmMstTaskmasterComponent } from './component/hrm-mst-taskmaster/hrm-mst-taskmaster.component';
import { HrmMemberDashboardComponent } from './component/hrm-member-dashboard/hrm-member-dashboard.component';
import { HrmMemberMyleaveComponent } from './component/hrm-member-myleave/hrm-member-myleave.component';
import { HrmMemberApproveleaveComponent } from './component/hrm-member-approveleave/hrm-member-approveleave.component';
import { HrmTrnProfileComponent } from './component/hrm-trn-profile/hrm-trn-profile.component';
import { HrmTrnIattendanceComponent } from './component/hrm-trn-iattendance/hrm-trn-iattendance.component';
import { HrmTrnProbationperiodComponent } from './component/hrm-trn-probationperiod/hrm-trn-probationperiod.component';
import { HrmTrnProbationhistoryComponent } from './component/hrm-trn-probationhistory/hrm-trn-probationhistory.component';
import { HrmTrnProbationleaveupdateComponent } from './component/hrm-trn-probationleaveupdate/hrm-trn-probationleaveupdate.component';
import { HrmTrnAppointmentorderComponent } from './component/hrm-trn-appointmentorder/hrm-trn-appointmentorder.component';
import { HrmTrnAppointmentordereditComponent } from './component/hrm-trn-appointmentorderedit/hrm-trn-appointmentorderedit.component';
import { HrmTrnAssetcustodianComponent } from './component/hrm-trn-assetcustodian/hrm-trn-assetcustodian.component';
import { HrmTrnAddassetcustodianComponent } from './component/hrm-trn-addassetcustodian/hrm-trn-addassetcustodian.component';
import { HrmTrnCompanypolicyComponent } from './component/hrm-trn-companypolicy/hrm-trn-companypolicy.component';
import { HrmMstEmployeeaddComponent } from './component/hrm-mst-employeeadd/hrm-mst-employeeadd.component';
import { HrmMstEmployeeeditComponent } from './component/hrm-mst-employeeedit/hrm-mst-employeeedit.component';
import { HrmTrnLeavebalancesummaryComponent } from './hrm-trn-leavebalancesummary/hrm-trn-leavebalancesummary.component';
import { HrmTrnLeavebalanceeditComponent } from './hrm-trn-leavebalanceedit/hrm-trn-leavebalanceedit.component';
import { HrmMemberMonthlyattendanceComponent } from './component/hrm-member-monthlyattendance/hrm-member-monthlyattendance.component';
import { HrmMemberOfficecalendarComponent } from './component/hrm-member-officecalendar/hrm-member-officecalendar.component';
import { HrmMstShifttypeComponent } from './component/hrm-mst-shifttype/hrm-mst-shifttype.component';
import { HrmMstAddshifttypeComponent } from './component/hrm-mst-addshifttype/hrm-mst-addshifttype.component';
import { HrmMstLeavetypeComponent } from './component/hrm-mst-leavetype/hrm-mst-leavetype.component';
import { HrmMstLeavegradeComponent } from './component/hrm-mst-leavegrade/hrm-mst-leavegrade.component';
import { HrmMstAddleavegradeComponent } from './component/hrm-mst-addleavegrade/hrm-mst-addleavegrade.component';
import { HrmMstAddholidaygradeComponent } from './component/hrm-mst-addholidaygrade/hrm-mst-addholidaygrade.component';
import { HrmTrnAddholidayasignComponent } from './component/hrm-trn-addholidayasign/hrm-trn-addholidayasign.component';

const routes: Routes = [
  { path: 'HrmTrnAdmincontrol', component: HrmTrnAdmincontrolComponent },
  { path: 'HrmMstEmployeeadd', component: HrmMstEmployeeaddComponent },
  { path: 'Employeeprofile', component: HrmTrnProfileComponent },
  { path: 'HrmMstEmployeedit/:employee_gid', component: HrmMstEmployeeeditComponent },
 { path: 'HrmTrnCompanyPolicy', component: HrmTrnCompanypolicyComponent },
  { path: 'HrmMstEmployeview/:employee_gid', component: HrmMstEmployeeviewComponent },  
  { path: 'HrmtrnEmployeeonboard', component: HrmTrnEmployeeonboardComponent },
  { path: 'HrmtrnEmployeeonboardadd', component: HrmTrnEmployeeonboardaddComponent },
  { path: 'HrmtrnEmployeeonboardedit', component: HrmTrnEmployeeonboardEditComponent },
  { path: 'HrmtrnEmployeeonboardview', component: HrmMstEmployeeviewComponent },
  { path: 'HrmtrnEmployeeonboardviewpending', component: HrmTrnEmployeeboardviewpendingComponent },
  { path: 'HrmtrnEmployeeonboardviewcompleted', component: HrmTrnEmployeeonboardViewCompletedComponent },
  { path: 'HrmMStSubfunction', component: HrmMstSubfunctionComponent },
  { path: 'HrmMstBaselocation', component: HrmMstBaselocationComponent },
  { path: 'HrmMstTeamMaster', component: HrmMstTeammasterComponent },
  { path: 'HrmMstRoleSummary', component: HrmMstRoleSummaryComponent },
  { path: 'HrmMstRoleAdd', component: HrmMstRoleAddComponent },
  { path: 'HrmMstRoleEdit', component: HrmMstRoleEditComponent },
  { path: 'HrmMstBranchSummary', component: HrmMstBranchSummaryComponent },
  { path: 'HrmMstDepartmentSummary', component: HrmMstDepartmentSummaryComponent },
  { path: 'HrmMstDesignation', component: HrmMstDesignationComponent },
  { path: 'HrmMstEntity', component: HrmMstEntityComponent },
  { path: 'HrmMstBloodGroup', component: HrmMstBloodgroupComponent },
  { path: 'HrmMstHrDocument', component: HrmMstHrdocumentComponent },
  { path: 'HrmMstTaskmaster', component: HrmMstTaskmasterComponent },
  { path: 'HrmMemberDashboard', component: HrmMemberDashboardComponent },
  { path: 'HrmMyLeave', component: HrmMemberMyleaveComponent },
  { path: 'HrmApproveLeave', component: HrmMemberApproveleaveComponent },
  {path:  'HrmTrnIattendance', component:HrmTrnIattendanceComponent},
  { path: 'hrm-trn-probationperiod', component: HrmTrnProbationperiodComponent},
  { path: 'Probationhistory/:employee_gid', component: HrmTrnProbationhistoryComponent},
  { path: 'Probationleaveupdate/:employee_gid', component: HrmTrnProbationleaveupdateComponent},
  { path: 'HrmTrnAppointmentorder', component: HrmTrnAppointmentorderComponent},
  { path: 'HrmTrnAppointmentorderedit/:appointmentorder_gid', component: HrmTrnAppointmentordereditComponent},
  { path: 'HrmTrnAssetCustodian', component: HrmTrnAssetcustodianComponent},
  { path: 'HrmTrnAddAssetcustidian/:employee_gid', component: HrmTrnAddassetcustodianComponent},
  { path: 'HrmTrnLeavebalancesummary', component: HrmTrnLeavebalancesummaryComponent},
  { path: 'HrmTrnLeavebalanceedit/:employee_gid', component: HrmTrnLeavebalanceeditComponent},
  {path: 'HrmMonthlyattendance',component:HrmMemberMonthlyattendanceComponent},
  {path: 'HrmOfficecalendar',component:HrmMemberOfficecalendarComponent},
  {path: 'HrmMstShiftTypeSummary',component:HrmMstShifttypeComponent},
  {path: 'HrmMstAddShiftType',component:HrmMstAddshifttypeComponent},
  {path: 'HrmMstLeaveType',component:HrmMstLeavetypeComponent},
  {path: 'HrmMstLeaveGrade',component:HrmMstLeavegradeComponent},
  {path: 'HrmMstAddLeaveGrade',component:HrmMstAddleavegradeComponent},
  {path: 'HrmMstAddHolidaygrademanagement',component:HrmMstAddholidaygradeComponent},
  {path: 'HrmTrnAddHolidayAssign',component:HrmTrnAddholidayasignComponent},



];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmsHrmRoutingModule { }