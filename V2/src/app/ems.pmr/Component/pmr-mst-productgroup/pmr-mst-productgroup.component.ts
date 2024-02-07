import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';

interface product
{
  productgroup_gid: string;
  productgroup_code: string;
  productgroup_name: string;
  productgroupedit_code: string;
  productgroupedit_name: string;
  

}

@Component({
  selector: 'app-pmr-mst-productgroup',
  templateUrl: './pmr-mst-productgroup.component.html',
  styleUrls: ['./pmr-mst-productgroup.component.scss']
})
export class PmrMstProductgroupComponent {
  productForm: FormGroup | any;
  productFormEdit: FormGroup | any;
  responsedata: any;
  parameterValue: any;
  productgroup_list: any[] = [];
  parameterValue1: any;

 
  product!: product;
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService,public NgxSpinnerService:NgxSpinnerService,) {
    this.product = {} as product;
  }
  ngOnInit(): void {
    this.GetProductGroupSummary();
    this.productForm = new FormGroup({



      productgroup_code: new FormControl(this.product.productgroup_code, [

        Validators.required,



      ]),

      productgroup_name: new FormControl(this.product.productgroup_name, [

        Validators.required,



      ])

    });

    this.productFormEdit = new FormGroup({



      productgroupedit_code: new FormControl(this.product.productgroupedit_code, [

        Validators.required,

      ]),


      productgroupedit_name: new FormControl(this.product.productgroupedit_name, [

        Validators.required,

      ]),
      productgroup_gid: new FormControl(''),
    });


  
   
  
   
  }
    
  
  
   //// Summary Grid//////
   GetProductGroupSummary(){
   var url = 'PmrMstProductGroup/GetProductGroupSummary'
   this.NgxSpinnerService.show()
   this.service.get(url).subscribe((result: any) => {
    $('#productgroup_list').DataTable().destroy();
     this.responsedata = result;
     this.productgroup_list = this.responsedata.productgroup_list;
     //console.log(this.entity_list)
     setTimeout(() => {
       $('#productgroup_list').DataTable()
     }, 1);
     this.NgxSpinnerService.hide()
 
   });
 
}
 ////////////Add popup validtion////////
  get productgroupcontrol_code() {
    return this.productForm.get('productgroup_code')!;
  }

get productgroupcontrol_name() {
  return this.productForm.get('productgroup_name')!;
}
////////////Edit popup validtion////////
get productgroupedit_code() {
  return this.productFormEdit.get('productgroupedit_code')!;
}

get productgroupedit_name() {
return this.productFormEdit.get('productgroupedit_name')!;
}


////////////Add popup////////
   onsubmit() {
    if (this.productForm.value.productgroup_code != null && this.productForm.value.productgroup_name != '') {

      for (const control of Object.keys(this.productForm.controls)) {
        this.productForm.controls[control].markAsTouched();
      }
      this.productForm.value;
      var url='PmrMstProductGroup/PostProductGroup'
      this.NgxSpinnerService.show()
      this.service.post(url,this.productForm.value).subscribe((result:any) => {

        if(result.status == false){
          this.ToastrService.warning(result.message)
          this.GetProductGroupSummary();
        }
        else{
          this.productForm.get("productgroup_code")?.setValue(null);
          this.productForm.get("productgroup_name")?.setValue(null);
          
          this.ToastrService.success(result.message) 
          this.productForm.reset();
         
          this.GetProductGroupSummary();
          this.NgxSpinnerService.hide();
          
          
         
        }
      
              
            });
            
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
    
  }
  //////////Edit popup////////
  openModaledit(parameter: string) {
    this.parameterValue1 = parameter
    this.productFormEdit.get("productgroupedit_code")?.setValue(this.parameterValue1.productgroup_code);
    this.productFormEdit.get("productgroupedit_name")?.setValue(this.parameterValue1.productgroup_name);
    this.productFormEdit.get("productgroup_gid")?.setValue(this.parameterValue1.productgroup_gid);
   
  }
  ////////////Update popup////////
   onupdate() {
    debugger
    if (this.productFormEdit.value.productgroupedit_code != null && this.productFormEdit.value.productgroupedit_name != '') {
      for (const control of Object.keys(this.productFormEdit.controls)) {
        this.productFormEdit.controls[control].markAsTouched();
      }
      this.productFormEdit.value;

      
      var url = 'PmrMstProductGroup/GetUpdatedProductgroup'
      this.NgxSpinnerService.show()

      this.service.post(url,this.productFormEdit.value).pipe().subscribe((result:any)=>{
        this.responsedata=result;
        if(result.status ==false){
          this.ToastrService.warning(result.message)
          this.GetProductGroupSummary();
          this.NgxSpinnerService.hide()
        }
        else{
          this.ToastrService.success(result.message)
          this.GetProductGroupSummary();
          this.NgxSpinnerService.hide()
        }
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
    
    var url = 'PmrMstProductGroup/GetDeleteProductSummary'
    let param = {
      productgroup_gid : this.parameterValue 
    }
    this.service.getparams(url,param).subscribe((result: any) => {
      if(result.status ==false){
        this.ToastrService.warning(result.message)
      }
      else{
        
        this.ToastrService.success(result.message)
        
      }
      
      this.GetProductGroupSummary();
     
      this.NgxSpinnerService.show();
  
    });
  }
  onclose() {
    this.productForm.reset();

  }
  
}

  