import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';

interface IDesignation{
  designation_gid:string;
  designation_code: string;
  designation_name: string;
}
@Component({
  selector: 'app-sys-mst-designation',
  templateUrl: './sys-mst-designation.component.html',
  styleUrls: ['./sys-mst-designation.component.scss']
})
export class SysMstDesignationComponent { reactiveForm!: FormGroup;
  //reactiveForm!: FormGroup;
  reactiveFormEdit!: FormGroup;
  reactiveFormEdit1!: FormGroup;
  responsedata: any;
  parameterValue: any; 
  parameterValue1: any;
  Designation_list: any[] = []
  Designation!: IDesignation;

  constructor(public service :SocketService,private route:Router,private ToastrService: ToastrService) {
    this.Designation = {} as IDesignation;
  }

  ngOnInit(): void {

    this.DesignationSummary();

    //Form values for Add popup//
    this.reactiveForm = new FormGroup({
      designation_gid: new FormControl(''),
      designation_code: new FormControl('', Validators.required),
      designation_name: new FormControl('', Validators.required),
      designation_status: new FormControl(''),
    });

    this.reactiveFormEdit1 = new FormGroup({
      designation_gid: new FormControl(''),
      designation_code: new FormControl(''),
      designation_name: new FormControl(''),
      designation_status: new FormControl(''),
    });

    //Form values for Edit popup//
    this.reactiveFormEdit = new FormGroup({
      designation_gid: new FormControl(''),
      designation_code: new FormControl(''),
      designation_name: new FormControl(''),
      designation_status: new FormControl(''),
    });
  }

  DesignationSummary() {
    var url = 'SysMstDesignation/GetDesignationtSummary'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result; 
    this.Designation_list = this.responsedata.Designation_list;
     //console.log(this.branch_list)
    setTimeout(() => {
    $('#Designation_list').DataTable();
    }, 1);
    });
  }

  //Add popup validtion//
  get adddesignation_code() {
    return this.reactiveForm.get('designation_code')!;
  }
  get adddesignation_name() {
    return this.reactiveForm.get('designation_name')!;
  }

  //Edit popup validtion//
  get designation_code() {
    return this.reactiveFormEdit.get('designation_code')!;
  }
  get designation_name() {
    return this.reactiveFormEdit.get('designation_name')!;
  }
  get designation_gid() {
    return this.reactiveFormEdit.get('designation_gid')!;
  }

  //Edit1 popup validtion//
  designation_status() {
    return this.reactiveFormEdit1.get('designation_status')!;
  }

  //Add popup//
  public onsubmit(): void {
    if (this.reactiveForm.value.designation_name != null && this.reactiveForm.value.designation_name != '') {
      for (const control of Object.keys(this.reactiveForm.controls)) {
        this.reactiveForm.controls[control].markAsTouched();
      }

      this.reactiveForm.value;
      var url1 = 'SysMstDesignation/PostDesignationAdd'
      this.service.post(url1, this.reactiveForm.value).subscribe((result: any) => {
        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.DesignationSummary();
        }
        else {
          this.reactiveForm.get("designation_name")?.setValue(null);
          this.ToastrService.success(result.message)
          this.DesignationSummary();
        }
      });
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }

  //Edit popup//
  openModaledit(parameter: string) {
    this.parameterValue1 = parameter
    this.reactiveFormEdit.get("designation_gid")?.setValue(this.parameterValue1.designation_gid);
    this.reactiveFormEdit.get("designation_code")?.setValue(this.parameterValue1.designation_code);
    this.reactiveFormEdit.get("designation_name")?.setValue(this.parameterValue1.designation_name);
  }

  public onupdate(): void {
    if (this.reactiveFormEdit.value.designation_name != null && this.reactiveFormEdit.value.designation_name != '') {
      for (const control of Object.keys(this.reactiveFormEdit.controls)) {
        this.reactiveFormEdit.controls[control].markAsTouched();
      }
      this.reactiveFormEdit.value;

      console.log(this.reactiveFormEdit.value)
      var url = 'SysMstDesignation/PostUpdateDesignation'

      this.service.postparams(url, this.reactiveFormEdit.value).pipe().subscribe(result => {
        this.responsedata = result;
        if (result.status == false) {
          this.ToastrService.warning("Error While Updating Branch")
          this.DesignationSummary();
        }
        else {
          this.ToastrService.success("Updated Branch Successfully")
          this.DesignationSummary();
        }
      });
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }

  openModalactive(parameter: string) {
    this.parameterValue1 = parameter
    this.reactiveFormEdit1.get("designation_gid")?.setValue(this.parameterValue1.designation_gid);
    this.reactiveFormEdit1.get("designation_status")?.setValue(this.parameterValue1.designation_status);
  }

  public onsubmit1(): void {
    if (this.reactiveFormEdit1.value.designation_status != null && this.reactiveFormEdit1.value.designation_status != '') {
      for (const control of Object.keys(this.reactiveFormEdit1.controls)) {
        this.reactiveFormEdit1.controls[control].markAsTouched();
      }
      this.reactiveFormEdit1.value;

      console.log(this.reactiveFormEdit1.value)
      var url4 = 'SysMstDesignation/PostDesignationStatus'

      this.service.postparams(url4, this.reactiveFormEdit1.value).pipe().subscribe(result => {
        this.responsedata = result;
        if (result.status == false) {
          this.ToastrService.warning("Error While Updating Designation Status")
          this.DesignationSummary();
        }
        else {
          this.ToastrService.success("Updated Designation Status Successfully")
          this.DesignationSummary();
        }
      });
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }

  onclose() {
    this.reactiveForm.reset();
  }

}

