import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
interface ISource {
  source_name: string;
  source_description: string;
  source_gid: string;
  sourceedit_name:string;
  sourceedit_description:string;
  sourceedit_code:string;
  
}

@Component({
  selector: 'app-crm-mst-sourcesummary',
  templateUrl: './crm-mst-sourcesummary.component.html',
  styleUrls: ['./crm-mst-sourcesummary.component.scss']
})
export class CrmMstSourcesummaryComponent {
  isReadOnly = true;
  reactiveForm!: FormGroup;
    reactiveFormEdit!: FormGroup;
    responsedata: any;
    parameterValue: any;
  
    parameterValue1: any;
    source_list: any[] = [];
    source!: ISource;
    constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService,private NgxSpinnerService: NgxSpinnerService) {
      this.source = {} as ISource;
    }

    ngOnInit(): void {
     this.GetSourceSummary();
     
      // Form values for Add popup/////
      this.reactiveForm = new FormGroup({
  
        source_name: new FormControl(this.source.source_name, [
          Validators.required,
          Validators.pattern("^(?!\s*$).+") // Allow letters, numbers, and spaces
        ]),
        source_description:  new FormControl(this.source.source_description,[
         // Validators.maxLength(300),
            
          ]),
        
        
       
      });
        // Form values for Edit popup/////
      this.reactiveFormEdit = new FormGroup({
        sourceedit_code:new FormControl(this.source.sourceedit_code, [
          Validators.required,
          Validators.pattern('^[A-Za-z0-9 ]+$') // Allow letters, numbers, and spaces
        ]),
  
        sourceedit_name:new FormControl(this.source.sourceedit_name, [
          Validators.required,
          Validators.pattern("^(?!\s*$).+") // Allow letters, numbers, and spaces
        ]),
       
        sourceedit_description:  new FormControl(this.source.sourceedit_description,[
          //Validators.maxLength(300),
           
          ]),
        
        
        
        source_gid: new FormControl(''),
        
   
      });
    }
   
    //// Summary Grid//////
    GetSourceSummary(){
      this.NgxSpinnerService.show();
   var url = 'Source/GetSourceSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#source_list').DataTable().destroy();
      this.responsedata = result;
      this.source_list = this.responsedata.source_lists;
      this.NgxSpinnerService.hide();
      //console.log(this.source_list)
      setTimeout(() => {
        $('#source_list').DataTable();
      }, 1);
  
  
    });
  }
  // ////////////Add popup validtion////////
  get source_name() {
    return this.reactiveForm.get('source_name')!;

  }
  get source_description() {
    return this.reactiveForm.get('source_description')!;
  }
  // ////////////Edit popup validtion////////
  get sourceedit_code() {
    return this.reactiveFormEdit.get('sourceedit_code')!;
  }
  get sourceedit_name() {
    return this.reactiveFormEdit.get('sourceedit_name')!;
  }
  // get source_description() {
  //   return this.reactiveForm.get('source_description')!;
  // }
  get sourceedit_description() {
    return this.reactiveFormEdit.get('sourceedit_description')!;
  }
  // ////////////Add popup////////
    public onsubmit(): void {
      if (this.reactiveForm.value.source_name != null ) {
  
        for (const control of Object.keys(this.reactiveForm.controls)) {
          this.reactiveForm.controls[control].markAsTouched();
        }
        this.reactiveForm.value;
        var url='Source/PostSource'
              this.service.post(url,this.reactiveForm.value).subscribe((result:any) => {
  
                if(result.status ==false){
                  window.scrollTo({

                    top: 0, // Code is used for scroll top after event done
        
                  });
                  this.reactiveForm.get("source_name")?.setValue(null);
                 this.reactiveForm.get("source_description")?.setValue(null);
                  this.ToastrService.warning(result.message)
                  this.GetSourceSummary();
                  this.reactiveForm.reset();

                }
                else{
                  window.scrollTo({

                    top: 0, // Code is used for scroll top after event done
        
                  });
                  this.reactiveForm.get("source_name")?.setValue(null);
                 this.reactiveForm.get("source_description")?.setValue(null);
                  this.ToastrService.success(result.message)
                  
                
                  this.GetSourceSummary();
                  this.reactiveForm.reset();

                 
                }
                this.GetSourceSummary();
                this.reactiveForm.reset();

              });
              
      }
      else {
        window.scrollTo({

          top: 0, // Code is used for scroll top after event done

        });
        this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
      }
      
    }
  //   ////////////Edit popup////////
    openModaledit(parameter: string) {
      this.parameterValue1 = parameter
      this.reactiveFormEdit.get("sourceedit_code")?.setValue(this.parameterValue1.source_code);
      this.reactiveFormEdit.get("sourceedit_name")?.setValue(this.parameterValue1.source_name);

      this.reactiveFormEdit.get("sourceedit_description")?.setValue(this.parameterValue1.source_description);
      this.reactiveFormEdit.get("source_gid")?.setValue(this.parameterValue1.source_gid);
     
    }
  //   ////////////Update popup////////
    public onupdate(): void {
      if (this.reactiveFormEdit.value.sourceedit_name != null && this.reactiveFormEdit.value.sourceedit_description != null) {
        for (const control of Object.keys(this.reactiveFormEdit.controls)) {
          this.reactiveFormEdit.controls[control].markAsTouched();
        }
        this.reactiveFormEdit.value;
        this.NgxSpinnerService.show();
        //console.log(this.reactiveFormEdit.value)
        var url = 'Source/Getupdatesourcedetails'
  
        this.service.post(url,this.reactiveFormEdit.value).pipe().subscribe((result:any)=>{
          this.responsedata=result;
          if(result.status ==false){
            window.scrollTo({

              top: 0, // Code is used for scroll top after event done
  
            });
            this.ToastrService.warning(result.message)
            this.GetSourceSummary();
            this.reactiveFormEdit.reset();

          }
          else{
            window.scrollTo({

              top: 0, // Code is used for scroll top after event done
  
            });
            this.ToastrService.success(result.message)
            this.GetSourceSummary();
            this.reactiveFormEdit.reset();

          }
          this.NgxSpinnerService.show();
          this.GetSourceSummary();
        this.reactiveFormEdit.reset();

         
      }); 
  
      }
      else {
        window.scrollTo({

          top: 0, // Code is used for scroll top after event done

        });
        this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
      }
    }
  //   ////////////Delete popup////////
    openModaldelete(parameter: string) {
      this.parameterValue = parameter
    
    }
    ondelete() {
      console.log(this.parameterValue);
      this.NgxSpinnerService.show();
      var url = 'Source/Getdeletesourcedetails'
      let param = {
        source_gid : this.parameterValue 
      }
      this.service.getparams(url,param).subscribe((result: any) => {
        if(result.status ==false){
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.warning(result.message)
        }
        else{
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.success(result.message)
        }
        this.NgxSpinnerService.hide();
        this.GetSourceSummary();
      
    
    
      });
    }
    onclose() {
      this.reactiveForm.reset();
      this.reactiveFormEdit.reset();

  
    }
  

}



