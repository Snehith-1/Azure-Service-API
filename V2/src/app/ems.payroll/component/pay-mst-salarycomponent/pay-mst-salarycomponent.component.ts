import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
interface ISalary {
  

}
@Component({
  selector: 'app-pay-mst-salarycomponent',
  templateUrl: './pay-mst-salarycomponent.component.html',
  styleUrls: ['./pay-mst-salarycomponent.component.scss']
})
export class PayMstSalarycomponentComponent {
  salarycompoent_list:any;
  salarycomponent_gid: any;
  responsedata: any;
  Salary!: ISalary;
  parameterValue: any;
  constructor(private formBuilder: FormBuilder, private router:Router,private ToastrService: ToastrService, public service: SocketService){
  this.Salary = {} as ISalary;
   }
ngOnInit(){
  
  //// Summary Grid//////
 
this.summary()

  }
  summary(){
    var url = 'PayMstSalaryComponent/GetSalaryComponentSummary'
    this.service.get(url).subscribe((result:any) => {
      this.responsedata = result;
      this.salarycompoent_list = this.responsedata.salarycompoent_list;  
      setTimeout(() => {
        $('#salarycompoent_list').DataTable();
      },1);
    });
  }

addsalary() {
  this.router.navigate(['/payroll/PayMstSalarycomponentadd'])
}
openedit(params:any){
  const secretKey = 'storyboarderp';
  const param = (params);
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  this.router.navigate(['/payroll/PayMstSalarycomponentedit',encryptedParam]) 
}
openModaldelete(parameter: string) {
  this.parameterValue = parameter
}
ondelete(){
    console.log(this.parameterValue);
    var url3 = 'PayMstSalaryComponent/getDeleteComponent'
    this.service.getid(url3, this.parameterValue).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.summary();

      }
      else {
        this.ToastrService.success(result.message)
        this.summary();
      }

    });
}



}