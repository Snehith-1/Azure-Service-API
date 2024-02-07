import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ExcelService } from 'src/app/Service/excel.service';


interface IEmployee {
  password: string;
  confirmpassword: string;
  showPassword: boolean;
  employee_gid:string;
  user_code:string;
  confirmusercode:string;
  active_flag:string;
  vendorregister_gid:string;
  product_desc:string;
 
}

@Component({
  selector: 'app-pmr-trn-vendorregister-summary',
  templateUrl: './pmr-trn-vendorregister-summary.component.html',
  styleUrls: ['./pmr-trn-vendorregister-summary.component.scss']
})
export class PmrTrnVendorregisterSummaryComponent {
  file!: File;
  reactiveFormReset!: FormGroup;
  reactiveFormEdit!: FormGroup;
  reactiveFormstatus!: FormGroup;
  reactiveFormUpdateUserCode!: FormGroup;
  responsedata: any;
  reset_list: any[] = [];
  employee_list: any[] = [];
  parameterValuecode: any;
   parameterValueReset: any;
   employee!: IEmployee;
   usercode:any;
   user_firstname:any;
   branch:any;
   department:any;
   designation:any;
   parameterValue: any;
   parameterValue1: any;
  
  constructor(public service :SocketService,private excelService : ExcelService,private route:Router,private ToastrService: ToastrService) {
    this.employee = {} as IEmployee;
  }
 
 
  ngOnInit(): void {
    this.reactiveFormReset = new FormGroup({

      password: new FormControl(this.employee.password, [
        Validators.required,
      ]),
      confirmpassword: new FormControl(''),
      employee_gid: new FormControl(''),

    });
    this.reactiveFormUpdateUserCode = new FormGroup({

      password: new FormControl(this.employee.password, [
        Validators.required,
      ]),
      confirmusercode: new FormControl(''),
      employee_gid: new FormControl(''),

    });
    this.reactiveFormstatus = new FormGroup({

      active_flag: new FormControl(this.employee.active_flag, [
        Validators.required,
      ]),
      product_desc: new FormControl(''),
      vendorregister_gid: new FormControl(''),

    });
   this.GetVendorregisterSummary();
  } 
  GetVendorregisterSummary(){
  var api1='PmrMstVendorRegister/GetVendorregisterSummary'
    
  this.service.get(api1).subscribe((result:any)=>{
    $('#employee_list').DataTable().destroy();
    this.responsedata=result;
    this.employee_list = this.responsedata.Getvendor_lists;  
   console.log(this.employee_list)
    setTimeout(()=>{   
      $('#employee_list').DataTable();
    }, 1);
  
    // this.reactiveFormstatus.get("active_flag")?.setValue(this.employee_list[0].active_flag);
    // this.reactiveFormstatus.get("vendorregister_gid")?.setValue(this.employee_list[0].vendorregister_gid);
 
});
}
  get password() {
    return this.reactiveFormReset.get('password')!;
  }
  get user_code() {
    return this.reactiveFormUpdateUserCode.get('user_code')!;
  }
  userpassword(password:any) {
    this.reactiveFormReset.get("confirmpassword")?.setValue(password.value);
  }
  updateusercode(user_code:any) {
    console.log(user_code.value)
    this.reactiveFormUpdateUserCode.get("confirmusercode")?.setValue(user_code.value);
  }
  openModalUpdateCode(parameter: string) {
    
  }
  openModalReset(parameter: string) {
   

  }
  
  onview(params:any){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.route.navigate(['/pmr/PmrTrnVendorregisterView',encryptedParam]) 
  }
  onedit(params:any){
    const secretKey = 'node --max_old_space_size=4096 ./node_modules/@angular/cli/bin/ng serve';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.route.navigate(['/pmr/PmrTrnVendorregisterEdit',encryptedParam]) 
  }
  onaddinfo(params:any){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.route.navigate(['/pmr/PmrMstVendorAdditionalinformation',encryptedParam]) 
  }
  onclose() {
    this.reactiveFormReset.reset();

  }
  oncloseupdatecode() {
    this.reactiveFormUpdateUserCode.reset();

  }
  onupdatereset(){
    //console.log(this.reactiveFormReset.value)

    if (this.reactiveFormReset.value.password != null && this.reactiveFormReset.value.password != '') {
      for (const control of Object.keys(this.reactiveFormReset.controls)) {
        this.reactiveFormReset.controls[control].markAsTouched();
      }
      

   
      var url = 'Vendorlist/Getresetpassword'

      this.service.post(url,this.reactiveFormReset.value).pipe().subscribe((result:any)=>{
        this.responsedata=result;
        if(result.status ==false){
          this.ToastrService.warning(result.message)
        
        }
        else{
          this.ToastrService.success(result.message)
          this.GetVendorregisterSummary();
        }
       
    }); 

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
    this.reactiveFormReset.reset();
  }
  onupdateusercode(){
    if (this.reactiveFormUpdateUserCode.value.user_code != null && this.reactiveFormUpdateUserCode.value.user_code != '') {
      for (const control of Object.keys(this.reactiveFormUpdateUserCode.controls)) {
        this.reactiveFormUpdateUserCode.controls[control].markAsTouched();
      }
      

   
      var url = 'Vendorlist/Getupdateusercode'

      this.service.post(url,this.reactiveFormUpdateUserCode.value).pipe().subscribe((result:any)=>{
        this.responsedata=result;
        if(result.status ==false){
          this.ToastrService.warning(result.message)
         
        }
        else{
          this.ToastrService.success(result.message)
          this.GetVendorregisterSummary();
        }
       
    }); 

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
    this.reactiveFormUpdateUserCode.reset();
  }

  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  
  }
  ondelete() {
    debugger
    console.log(this.parameterValue);
    var url = 'PmrMstVendorRegister/VendorRegisterSummaryDelete'
    let param = {
      vendorregister_gid : this.parameterValue 
    }
    this.service.getparams(url,param).subscribe((result: any) => {
      if(result.status ==false){
        this.ToastrService.warning(result.message)
      }
      else{
        this.ToastrService.success(result.message)
      }
      this.GetVendorregisterSummary();
    
  
  
    });
  }
  onattach(params:any){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.route.navigate(['/pmr/PmrMstVendorRegisterDocument',encryptedParam]) 
  }

  activestatus(parameter:any){
    debugger
    this.parameterValue1 = parameter
    // // const secretKey = 'storyboarderp';
    // // const param = (params);
    // // const encryptedParam = AES.encrypt(param,secretKey).toString();
    // // this.route.navigate(['/pmr/PmrMstVendorRegisterDocument',encryptedParam])
    this.reactiveFormstatus.get("active_flag")?.setValue(this.parameterValue1.active_flag); 
    this.reactiveFormstatus.get("vendorregister_gid")?.setValue(this.parameterValue1.vendorregister_gid); 
  }

  validate(){
    debugger
    console.log(this.reactiveFormstatus.value)

    this.employee = this.reactiveFormstatus.value;
    // this.service.Profileupload(this.reactiveForm.value).subscribe(result => {  
    //   this.responsedata=result;
    // });   
    if (this.employee.active_flag != null && this.employee.product_desc != '') {
      let formData = new FormData();
      if (this.file != null && this.file != undefined) {
        formData.append("active_flag", this.employee.active_flag);
        formData.append("vendorregister_gid",this.employee.vendorregister_gid);
        formData.append("product_desc",this.employee.product_desc);
      }
      this.reactiveFormstatus.value;

        var url1 = 'PmrMstVendorRegister/UpdateVendorStatus'

        this.service.post(url1,this.reactiveFormstatus.value).pipe().subscribe((result:any) =>{
          this.responsedata=result;
          if(result.status ==false){
            this.ToastrService.warning(result.message)
            this.GetVendorregisterSummary();
          }
          else{
            this.ToastrService.success(result.message)
            this.GetVendorregisterSummary();
          }
        }); 
      // }
      
      
    }
    
    else {
      this.ToastrService.warning('Kindly Fill The Mandatory Field !! ')
    }
    this.GetVendorregisterSummary();
  }
  
  vrndorexportExcel(){
debugger
// var api7 = 'PmrMstVendorRegister/GetVendorReportExport'
// this.service.generateexcel(api7).subscribe((result: any) => {
//   this.responsedata = result;
//   var phyPath = this.responsedata.vendorexport_list[0].lspath1;
//   var relPath = phyPath.split("src");
//   var hosts = window.location.host;
//   var prefix = location.protocol + "//";
//   var str = prefix.concat(hosts, relPath[1]);
//   var link = document.createElement("a");
//   var name = this.responsedata.vendorexport_list[0].lsname2.split('.');
//   link.download = name[0];
//   link.href = str;
//   link.click();
//   this.ToastrService.success("Vendor Excel Exported Successfully")

// });
const VendorExcel = this.employee_list.map(item => ({
  VendorCode: item.vendor_code || '',
  VendorCompanyname : item.vendor_companyname || '',
  ContactpersonName : item.contactperson_name || '',
  ContactTelephonenumber : item.contact_telephonenumber || '',
  Status : item.vendor_status || '',
  
 
}));

     
      this.excelService.exportAsExcelFile(VendorExcel, 'Vendor_Excel');
  }
  
}