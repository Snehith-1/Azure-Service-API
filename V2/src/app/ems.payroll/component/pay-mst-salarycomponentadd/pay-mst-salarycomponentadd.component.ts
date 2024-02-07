import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
interface Isalarycomponent{
  salarycomponent_gid: string;
  componentgroup_gid: string;
  componentgroup_name: string;
  component_name: string;
  component_code: string;
    }

@Component({
  selector: 'app-pay-mst-salarycomponentadd',
  templateUrl: './pay-mst-salarycomponentadd.component.html',
  styleUrls: ['./pay-mst-salarycomponentadd.component.scss']
})
export class PayMstSalarycomponentaddComponent {
  showInput0: boolean = false;
  showInput: boolean = false;
  showInput1: boolean = false;
  showInput2: boolean = false;
  showInputs: boolean = false;
  inputValue: string = ''
  inputValue1: string = ''
  inputValue2: string = ''
  is_percent :any;
  is_percentage: any;
  reactiveForm: any;
  componentgrouplist: any[] = [];
  salarycomponent!: Isalarycomponent;
  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute, private router: Router, private ToastrService: ToastrService, public service: SocketService) {
    this.salarycomponent = {} as Isalarycomponent;
  }

  ngOnInit(): void {
 
    // Form values for Add popup/////
    this.reactiveForm = new FormGroup({
      
      component_name: new FormControl(''),
      component_type: new FormControl(''),
      contribution_type: new FormControl(''),
      component_code: new FormControl(''),
      affect_in: new FormControl(''),
      lop_deduction: new FormControl(''),
      statutory_pay: new FormControl(''),
      other_allowance: new FormControl(''),
      is_percent: new FormControl(''),
      is_percentage: new FormControl(''),
      employee_percent: new FormControl(''), 
      employer_percentage: new FormControl(''),
      employee_amount: new FormControl(''),
      employer_amount: new FormControl(''),



      componentgroup_name : new FormControl(this.salarycomponent.componentgroup_name, [
        Validators.required,
        Validators.minLength(1),
        ]),
     
        salarycomponent_gid: new FormControl(''),
        componentgroup_gid: new FormControl(''),
   
    });
    
    var api='PayMstSalaryComponent/GetComponentGroupDtl'
    this.service.get(api).subscribe((result:any)=>{
    this.componentgrouplist = result.GetComponentGroupDtl;
    //console.log(this.componentgrouplist)
   });


  }

   ////////////Add popup validation////////
get componentgroup_name() {
  return this.reactiveForm.get('componentgroup_name')!;
}
get component_code() {
  return this.reactiveForm.get('component_code')!;
}
get component_name() {
  return this.reactiveForm.get('component_name')!;
}

showTextBox0(event: Event) {
  const target = event.target as HTMLInputElement;
  this.showInput0 = target.value === 'EMPLOYEE ONLY' ;
  
}


showTextBox(event: Event) {
  const target = event.target as HTMLInputElement;
  this.showInput = target.value === 'EMPLOYER ONLY' ;
  this.showInputs = target.value === 'EMPLOYER ONLY' ;
}

showTextBox1(event: Event) {
  const target = event.target as HTMLInputElement;
  this.showInput1 = target.value === 'BOTH' ;
 

}
showTextBoxsta(event: Event) {
  const target = event.target as HTMLInputElement;
  this.showInputs = target.value === 'BOTH' ;

}

showTextBox2(event: Event) {
  const target = event.target as HTMLInputElement;
  this.showInput2 = target.value === 'Other' ;
}













  onsubmit(){
    debugger;

    if (this.reactiveForm.value.componentgroup_name != null && this.reactiveForm.value.componentgroup_name != '')

    if (this.reactiveForm.value.component_code != null && this.reactiveForm.value.component_code != '')
   
    if (this.reactiveForm.value.component_name != null && this.reactiveForm.value.component_name != '')
  

    {
    for (const control of Object.keys(this.reactiveForm.controls)) {
    this.reactiveForm.controls[control].markAsTouched();
    }
   this.reactiveForm.value;
   var url1 = 'PayMstSalaryComponent/PostComponent'
  debugger;
          this.service.postparams(url1, this.reactiveForm.value).subscribe((result: any) => {
  
              if (result.status == false) {
  
                 this.ToastrService.warning(result.message)
              // this.LoanSummary();
              }
  
              else {
                this.reactiveForm.get("salarycomponent_gid")?.setValue(null);
                this.reactiveForm.get("componentgroup_name")?.setValue(null);
                this.reactiveForm.get("component_code")?.setValue(null);
                this.reactiveForm.get("component_name")?.setValue(null);
                this.reactiveForm.get("component_type")?.setValue(null);
                this.reactiveForm.get("contribution_type")?.setValue(null);
                this.reactiveForm.get("affect_in")?.setValue(null);
                this.reactiveForm.get("lop_deduction")?.setValue(null);
                this.reactiveForm.get("statutory_pay")?.setValue(null);
                this.reactiveForm.get("other_allowance")?.setValue(null);
                this.reactiveForm.get("is_percent")?.setValue(null);
                this.reactiveForm.get("employee_percent")?.setValue(null);
                this.reactiveForm.get("employee_amount")?.setValue(null);
                this.reactiveForm.get("is_percentage")?.setValue(null);
                this.reactiveForm.get("employer_percentage")?.setValue(null);
                this.reactiveForm.get("employer_amount")?.setValue(null);           
                
               
                  this.ToastrService.success(result.message);  
              }
              this.router.navigate(['/payroll/PayMstSalaryComponent']);

            });
          }
       
        }

  onback(){
    this.router.navigate(['/payroll/PayMstSalaryComponent']);

  }

}

