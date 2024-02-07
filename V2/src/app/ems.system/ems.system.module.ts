import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DataTablesModule } from 'angular-datatables';
import { NgSelectModule } from '@ng-select/ng-select';
import { EmsSystemRoutingModule } from './ems.system-routing.module';
import { SysMstEmployeeAddComponent } from './component/sys-mst-employee-add/sys-mst-employee-add.component';
import { SysMstMenuMappingComponent } from './component/sys-mst-menu-mapping/sys-mst-menu-mapping.component';
import { FormsModule } from '@angular/forms';
import { SystemDashboardComponent } from './component/system-dashboard/system-dashboard.component';
import { NgApexchartsModule } from 'ng-apexcharts';
import { EmsUtilitiesModule } from '../ems.utilities/ems.utilities.module';
import { ReactiveFormsModule } from '@angular/forms';
import { SysMstEmployeePendingSummaryComponent } from './component/sys-mst-employee-pending-summary/sys-mst-employee-pending-summary.component';
import { SysMstEmployeeEditComponent } from './component/sys-mst-employee-edit/sys-mst-employee-edit.component';
import { SysMstEmployeeViewComponent } from './component/sys-mst-employee-view/sys-mst-employee-view.component';
import { SysMstEmployeeSummaryComponent } from './component/sys-mst-employee-summary/sys-mst-employee-summary.component';
import { SysMstEntitySummaryComponent } from './component/sys-mst-entity-summary/sys-mst-entity-summary.component';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { SysRptEmployeereportComponent } from './component/sys-rpt-employeereport/sys-rpt-employeereport.component';
import { SysMstBranchComponent } from './component/sys-mst-branch/sys-mst-branch.component';
import { SysMstDepartmentComponent } from './component/sys-mst-department/sys-mst-department.component';
import { SysMstDesignationComponent } from './component/sys-mst-designation/sys-mst-designation.component';
import { SysMstUserprofileComponent } from './component/sys-mst-userprofile/sys-mst-userprofile.component';
import { SysMstModulemanagerComponent } from './component/sys-mst-modulemanager/sys-mst-modulemanager.component';
import { SysMstAssignemployeeComponent } from './component/sys-mst-assignemployee/sys-mst-assignemployee.component';
import { SysMstTemplateSummaryComponent } from './component/sys-mst-template-summary/sys-mst-template-summary.component';
import { SysMstTemplateAddComponent } from './component/sys-mst-template-add/sys-mst-template-add.component';
import { SysMstTemplateEditComponent } from './component/sys-mst-template-edit/sys-mst-template-edit.component';

@NgModule({
  declarations: [
    SysMstEmployeeAddComponent,
    SysMstMenuMappingComponent,
    SysRptEmployeereportComponent,
    SystemDashboardComponent,
    SysMstEmployeePendingSummaryComponent,
    SysMstEmployeeEditComponent,
    SysMstEmployeeViewComponent,
    SysMstEmployeeSummaryComponent,
    SysMstEntitySummaryComponent,
    SysMstBranchComponent,
    SysMstDepartmentComponent,
    SysMstDesignationComponent,
    SysMstModulemanagerComponent,
    SysMstUserprofileComponent,
    SysMstAssignemployeeComponent,
    SysMstTemplateSummaryComponent,
    SysMstTemplateAddComponent,
    SysMstTemplateEditComponent,
  ],

  imports: [
    CommonModule,
    EmsSystemRoutingModule,
    EmsUtilitiesModule,
    FormsModule,
    NgApexchartsModule,
    ReactiveFormsModule,
    DataTablesModule,
    NgSelectModule,AngularEditorModule
  ]
})

export class EmsSystemModule { }
