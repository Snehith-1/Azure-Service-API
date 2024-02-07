import { Component,OnInit,TemplateRef,ElementRef, ViewChild ,ChangeDetectorRef } from '@angular/core'; 
import { Router,ActivatedRoute } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AES, enc } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import {SelectionModel} from '@angular/cdk/collections';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from 'src/environments/environment.development';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms'; 


interface objInterface { module_gid: string; employee_gid: string,module_checked:boolean   } 
interface Sub2MenuItem { module_gid: string; text:string ,module_checked:boolean}
interface Sub1MenuItem { sub2menu: Sub2MenuItem[]; }
interface SubMenuItem { sub1menu: Sub1MenuItem[]; }
interface UserRoleMenuItem { submenu: SubMenuItem[]; }

@Component({
  selector: 'app-sys-mst-assignemployee',
  templateUrl: './sys-mst-assignemployee.component.html',
  styleUrls: ['./sys-mst-assignemployee.component.scss']
})
export class SysMstAssignemployeeComponent implements OnInit {
  module_gid : any
  user_gid: any;
  employeelist: any
  cboselectedEmployee: any
  cboassignhierarchy: any
  ModuleAssignedemployeeinfo: any 
  ModuleHierarchy: any; 
  userRoleMenulist: any; 
  selectedCheckboxData: any[] = []; 
  selection = new SelectionModel<objInterface>(true, []); 

  constructor(public router:Router,private ActivatedRoute: ActivatedRoute,public NgxSpinnerService:NgxSpinnerService,
    private SocketService: SocketService,private ToastrService: ToastrService,private FormBuilder: FormBuilder,
    private changeDetectorRef: ChangeDetectorRef) {
      
    } 
  ngOnInit(): void {
    this.NgxSpinnerService.show();
    this.ActivatedRoute.queryParams.subscribe(params => {
      const urlparams = params['hash'];  
      if (urlparams) { 
        const decryptedParam = AES.decrypt(urlparams, environment.secretKey).toString(enc.Utf8); 
        const paramvalues = decryptedParam.split('&'); 
        this.module_gid = paramvalues[0]; 
      } 
    });
    var params = {
      module_gid: this.module_gid
      }
   var url = 'SysMstModuleManage/GetEmployeeAssignlist';
  this.SocketService.getparams(url,params).subscribe((result: any) => { 
    this.employeelist  = result.employeelist;  
  });
  
  var url = 'SysMstModuleManage/GetModuleAssignedEmployee';
  this.SocketService.getparams(url,params).subscribe((result: any) => {
    if(result.status!=null){
      this.ModuleAssignedemployeeinfo  = result.mdlModuleAssigneddtl;
      this.ModuleHierarchy = result.mdlModuleHierarchy;
      this.NgxSpinnerService.hide();  
    } 
    setTimeout(()=>{    
      $('#assignemployeetable').DataTable();
    }, 1);
  });
  }
  addmoduleuser(){
    debugger
      var employeeList = this.cboselectedEmployee.map(function(employeeId: any) {
      return { employee_gid: employeeId };
      });
    var params ={
      module_gid: this.module_gid,
      assign_hierarchy: this.cboassignhierarchy,
      Mdlassignemployeelist: employeeList 
    }
    this.NgxSpinnerService.show();
  var url = 'SysMstModuleManage/PostModuleEmployeeAssign';
  this.SocketService.post(url,params).subscribe((result: any) => { 
    if(result.status ==true){
      this.ToastrService.success(result.message)
      this.NgxSpinnerService.hide(); 
      this.ngOnInit();
      this.cboselectedEmployee = null;
      this.cboassignhierarchy = null;
    }
    else{
      this.ToastrService.warning(result.message)
      this.NgxSpinnerService.hide();   
    }
  });
  }

  UserRoleClick(user_gid: any, user_code: any, user_name: any){
    this.selection.clear();
    this.user_gid= user_gid;
  const scrollContainer = document.getElementById('scroll-container');
  if (scrollContainer) {
    scrollContainer.scrollTop = 0;  
  }
    var params = {
      module_parentgid: this.module_gid,
      user_gid: user_gid
      } 
      var url = 'SysMstModuleManage/GetUserRoleList';
      this.SocketService.post(url,params).subscribe((result: any) => {
        debugger;
        if(result.status!=null){
          this.userRoleMenulist  = result.menu_list; 
          
          console.log(this.userRoleMenulist);
           this.NgxSpinnerService.hide(); 
           this.initializeSelection();
         }  
      });
  }
  initializeSelection() {
    for (const data of this.userRoleMenulist) {
      for (const j of data.submenu) {
        for (const k of j.sub1menu) {
          if (k.menu_access === 'Y') {
            this.updateSelection(k);
          }
        }
      }
    }
  }
  updateSelection(item: any) {
    if (item.menu_access === 'Y') {
      this.selection.select(item);
    } 
  }

  isAllSelected() { 
    const numSelected = this.selection.selected.length;  
    return numSelected;
  }
  masterToggle() { 
    if(this.isAllSelected())
     this.selection.clear()
     else{
      const ModuleCheckedDate = this.userRoleMenulist.flatMap((item1: UserRoleMenuItem) =>
          item1.submenu.flatMap((item2: SubMenuItem) =>
          item2.sub1menu
                )
              
            );  
      this.selection.select(...ModuleCheckedDate); 
    }  
  }
 
  UserRoleselected(){ 
    debugger;
    const moduleListGid = this.selection.selected.map(data => data.module_gid).join(',');   
    var params ={
      module_gid: moduleListGid ,
      module_parentgid: this.module_gid,
      user_gid: this.user_gid 
    } 
    this.NgxSpinnerService.show();
  var url = 'SysMstModuleManage/PostPrivilege';
  this.SocketService.post(url,params).subscribe((result: any) => { 
    if(result.status ==true){
      this.ToastrService.success(result.message)
      this.NgxSpinnerService.hide(); 
      this.ngOnInit();
    }
    else{
      this.ToastrService.warning(result.message)
      this.NgxSpinnerService.hide();   
    }
  });
  }
  backbutton(){
    this.router.navigate(['/system/SysMstModuleManager']); 
  }

  
  
}