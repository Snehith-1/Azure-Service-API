import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
interface ITax {
  tax_gid: string;
  tax_code: string;
  tax_name: string;
  percentage: string;
  parameter: string;
  taxedit_name: string;
  editpercentage: string;
}
@Component({
  selector: 'app-pmr-mst-tax-summary',
  templateUrl: './pmr-mst-tax-summary.component.html',
  styleUrls: ['./pmr-mst-tax-summary.component.scss']
})
export class PmrMstTaxSummaryComponent {
  reactiveForm!: FormGroup;
  reactiveFormEdit!: FormGroup;
  responsedata: any;
  parameter: any;
  parameterValue: any;
  parameterValue1: any;
  pmrtax_list: any[] = [];


  tax!: ITax;
  getData: any;
  constructor(private formBuilder: FormBuilder,public NgxSpinnerService:NgxSpinnerService, private ToastrService: ToastrService, public service: SocketService) {
    this.tax = {} as ITax;
  }
  
  
 
  ngOnInit(): void {
    this.GetTaxSummary();
    // Form values for Add popup/////
    this.reactiveForm = new FormGroup({

      tax_name: new FormControl(this.tax.tax_name, [
        Validators.required,

      ]),
      percentage: new FormControl(this.tax.percentage, [
        Validators.required
      ])

     

    });
    // Form values for Edit popup/////
    this.reactiveFormEdit = new FormGroup({

      taxedit_name: new FormControl(this.tax.taxedit_name, [
        Validators.required ,
        
      ]),

      editpercentage: new FormControl(this.tax.editpercentage, [
        Validators.required,
       
      ]),

      tax_gid: new FormControl(''),

    


    });

  }

  //// Summary Grid//////
  GetTaxSummary() {
    var url = 'PmrMstTax/GetTaxSummary'
    this.NgxSpinnerService.show()
    this.service.get(url).subscribe((result: any) => {
      $('#pmrtax_list').DataTable().destroy();
      this.responsedata = result;
      this.pmrtax_list = this.responsedata.pmrtax_list;
      setTimeout(() => {
        $('#pmrtax_list').DataTable();
      }, 1);
      this.NgxSpinnerService.hide()


    })
  }

  ////////////Add popup validtion////////
  get tax_name() {
    return this.reactiveForm.get('tax_name')!;
    
  }
  get percentage() {
    return this.reactiveForm.get('percentage')!;
  }
  


  ////////////Add popup////////

  onsubmit() {
    console.log(this.reactiveForm)
    if (this.reactiveForm.value.tax_name != null && this.reactiveForm.value.percentage != '') {

      for (const control of Object.keys(this.reactiveForm.controls)) {
        this.reactiveForm.controls[control].markAsTouched();
      }
      this.reactiveForm.value;
      var url = 'PmrMstTax/PostTax'
      this.NgxSpinnerService.show()
      this.service.post(url, this.reactiveForm.value).subscribe((result: any) => {

        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.GetTaxSummary();
          this.NgxSpinnerService.hide()
        }
        else {
          this.reactiveForm.get("tax_name")?.setValue(null);
          this.reactiveForm.get("percentage")?.setValue(null);
          this.ToastrService.success(result.message)

          

          this.reactiveForm.reset();
          this.GetTaxSummary();
          this.NgxSpinnerService.hide()

        }
        

      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }


  }
  ////////////Edit popup////////
  openModaledit(parameter: string) {
    this.parameterValue1 = parameter

    this.reactiveFormEdit.get("taxedit_name")?.setValue(this.parameterValue1.tax_name);
    this.reactiveFormEdit.get("editpercentage")?.setValue(this.parameterValue1.percentage);
    this.reactiveFormEdit.get("tax_gid")?.setValue(this.parameterValue1.tax_gid);
  }
////////////Edit popup validation////////

  get taxedit_name() {
    return this.reactiveFormEdit.get('taxedit_name')!
  }
  get editpercentage() {
    return this.reactiveFormEdit.get('editpercentage')!;
  }
  
  get  tax_gid() {
    return this.reactiveFormEdit.get('tax_gid')!;

    
  }
  
  ////////////Update popup////////
  public onupdate(): void {
    if (this.reactiveFormEdit.value.taxedit_name != null && this.reactiveFormEdit.value.editpercentage != null) {
      for (const control of Object.keys(this.reactiveFormEdit.controls)) {
        this.reactiveFormEdit.controls[control].markAsTouched();
      }
      this.reactiveFormEdit.value;

      //console.log(this.reactiveFormEdit.value)
      var url = 'PmrMstTax/UpdatedTaxSummary'
      this.NgxSpinnerService.show()
      this.service.post(url,this.reactiveFormEdit.value).pipe().subscribe((result:any)=>{
        this.responsedata=result;
        if(result.status ==false){
          this.ToastrService.warning(result.message)
          this.GetTaxSummary();
        }
        else{
          this.ToastrService.success(result.message)
          this.GetTaxSummary();
          this.NgxSpinnerService.hide()
        }
        this.reactiveForm.reset();
       
    }); 

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }

  ////////////Delete popup////////
 openModaldelete(parameter: string) {
  this.parameterValue = parameter

}

  ondelete() {
    debugger;
    console.log(this.parameterValue);
    var url = 'PmrMstTax/deleteTaxSummary'
    this.NgxSpinnerService.show()
    let param = {
      tax_gid : this.parameterValue 
    }
    this.service.getparams(url,param).subscribe((result: any) => {
      if(result.status ==false){
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide()
      }
      else{
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide()
     window.location.reload();
      }
      
      this.GetTaxSummary();
      window.location.reload();
    
  
  
    });
  }
  onclose() {
    this.reactiveForm.reset();

  }
  
 
}
