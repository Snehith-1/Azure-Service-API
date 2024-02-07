import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AES, enc } from 'crypto-js';

interface Isalarycomponentedit{
  salarycomponent_gid: string;
  componentgroup_gid: string;
  componentgroup_name: string;
  component_type: string;
  contribution_type: string;
  component_name: string;
  component_code: string;
  affect_in: string;
  lop_deduction: string;
  is_percent: string;
  is_percentage: string;
  employee_percent:string;
  employer_percentage: string;
  employee_amount: string;
  employer_amount: string;
  statutory_pay: string;
  other_allowance: string;
}


@Component({
  selector: 'app-pay-mst-salarycomponentedit',
  templateUrl: './pay-mst-salarycomponentedit.component.html',
  styleUrls: ['./pay-mst-salarycomponentedit.component.scss']
})
export class PayMstSalarycomponenteditComponent {

  showInput0: boolean = false;
  showInput: boolean = false;
  showInput1: boolean = false;
  showInput2: boolean = false;
  showInputs: boolean = false;
  inputValue: string = ''
  inputValue1: string = ''
  inputValue2: string = ''
  is_percent: any;
  is_percentage: any;
  reactiveFormEdit: any;
  componentgrouplist : any[] = [];
  salarycomponentedit!: Isalarycomponentedit;
  responsedata: any;
  editcomponentsummary_list: any;
  salarycomponent_gid: any;
  contribution_type: any;
  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute, private router: Router, private ToastrService: ToastrService, public service: SocketService) {
    this.salarycomponentedit = {} as Isalarycomponentedit;
  }
 
  ngOnInit(): void 
  {
    
   debugger;
    const salarycomponent_gid = this.route.snapshot.paramMap.get('salarycomponent_gid');
    this.salarycomponent_gid = salarycomponent_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.salarycomponent_gid, secretKey).toString(enc.Utf8);
    this.getEditComponent(deencryptedParam);
    console.log(deencryptedParam)


 
  // Form values for Edit popup/////
  this.reactiveFormEdit = new FormGroup({

 
  component_name: new FormControl(this.salarycomponentedit.component_name, []),

  component_type: new FormControl(this.salarycomponentedit.component_type, []),

  contribution_type: new FormControl(this.salarycomponentedit.contribution_type, []),

  component_code: new FormControl(this.salarycomponentedit.component_code, []),

  affect_in: new FormControl(this.salarycomponentedit.affect_in, []),

  lop_deduction: new FormControl(this.salarycomponentedit.lop_deduction, []),

  is_percent: new FormControl(this.salarycomponentedit.is_percent, []),
  
  employee_percent: new FormControl(this.salarycomponentedit.employee_percent, []),
  
  is_percentage: new FormControl(this.salarycomponentedit.is_percentage, []),

  employer_percentage: new FormControl(this.salarycomponentedit.employer_percentage, []),

  employee_amount: new FormControl(this.salarycomponentedit.employee_amount, []),
  
  employer_amount: new FormControl(this.salarycomponentedit.employer_amount, []),

  statutory_pay: new FormControl(this.salarycomponentedit.statutory_pay, []),

  other_allowance: new FormControl(this.salarycomponentedit.other_allowance, []),



  componentgroup_name : new FormControl(this.salarycomponentedit.componentgroup_name, [
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

    ////////////Edit popup validtion////////
    get component_name() {
      return this.reactiveFormEdit.get('component_name')!;
    }

    get component_code() {
      return this.reactiveFormEdit.get('component_code')!;
    }

    get componentgroup_name() {
      return this.reactiveFormEdit.get('componentgroup_name')!;
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
    
    
    
    
    

    getEditComponent(salarycomponent_gid: any) {
      debugger
      var url = 'PayMstSalaryComponent/getEditComponent'
      let param = {salarycomponent_gid : salarycomponent_gid}
      this.service.getparams(url, param).subscribe((result: any) => {
        this.responsedata=result;
        this.editcomponentsummary_list = result.getEditComponent;
  
        // this.product = result;
        console.log(this.salarycomponent_gid)
        console.log(this.editcomponentsummary_list)
  
        this.reactiveFormEdit.get("salarycomponent_gid")?.setValue(this.editcomponentsummary_list[0].salarycomponent_gid);
        this.reactiveFormEdit.get("component_name")?.setValue(this.editcomponentsummary_list[0].component_name);
        this.reactiveFormEdit.get("componentgroup_name")?.setValue(this.editcomponentsummary_list[0].componentgroup_name);
        this.reactiveFormEdit.get("component_type")?.setValue(this.editcomponentsummary_list[0].component_type);
        this.reactiveFormEdit.get("contribution_type")?.setValue(this.editcomponentsummary_list[0].contribution_type);
        this.reactiveFormEdit.get("component_code")?.setValue(this.editcomponentsummary_list[0].component_code);
        this.reactiveFormEdit.get("affect_in")?.setValue(this.editcomponentsummary_list[0].affect_in);
        this.reactiveFormEdit.get("lop_deduction")?.setValue(this.editcomponentsummary_list[0].lop_deduction);
        this.reactiveFormEdit.get("is_percent")?.setValue(this.editcomponentsummary_list[0].is_percent);
        this.reactiveFormEdit.get("employee_percent")?.setValue(this.editcomponentsummary_list[0].is_percent);

        this.reactiveFormEdit.get("employee_percent")?.setValue(this.editcomponentsummary_list[0].employee_percent);
        this.reactiveFormEdit.get("is_percentage")?.setValue(this.editcomponentsummary_list[0].is_percentage);
        this.reactiveFormEdit.get("employer_percentage")?.setValue(this.editcomponentsummary_list[0].employer_percentage);
        this.reactiveFormEdit.get("employee_amount")?.setValue(this.editcomponentsummary_list[0].employee_amount);
        this.reactiveFormEdit.get("employer_amount")?.setValue(this.editcomponentsummary_list[0].employer_amount);
        this.reactiveFormEdit.get("statutory_pay")?.setValue(this.editcomponentsummary_list[0].statutory_pay);
        this.reactiveFormEdit.get("other_allowance")?.setValue(this.editcomponentsummary_list[0].other_allowance);

        
      });
    } 

 
 
  onupdate(){
    debugger;
    var url = 'PayMstSalaryComponent/getUpdatedComponent'

    this.service.post(url, this.reactiveFormEdit.value).subscribe(
     (result: any) => {
 
     
      if (result.status == false) {
        this.ToastrService.warning(result.message)
     
      }
      else {
        this.ToastrService.success(result.message)
        this.router.navigate(['/payroll/PayMstSalaryComponent']);
      
      }

    });



  }

  onback(){
    this.router.navigate(['/payroll/PayMstSalaryComponent']);

  }



}
