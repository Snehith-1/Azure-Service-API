import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { AES } from 'crypto-js';



@Component({
  selector: 'app-hrm-trn-probationperiod',
  templateUrl: './hrm-trn-probationperiod.component.html',
  styleUrls: ['./hrm-trn-probationperiod.component.scss']
})
export class HrmTrnProbationperiodComponent {
  employee:any[] = [];
  employee_list: any[] = [];
  response_data :any;


  constructor(private fb: FormBuilder,private route: ActivatedRoute,private router: Router,private service: SocketService,) {} 

  ngOnInit(): void {
    var api = 'Probationperiod/GetProbationperiodSummary';
    this.service.get(api).subscribe((result:any) => {
      this.response_data = result;
      this.employee = this.response_data.employee_list;
      setTimeout(()=>{  
        $('#employee').DataTable();
      }, 1);
    });
  }

  onhistory(params:any){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.router.navigate(['/hrm/Probationhistory',encryptedParam]) 
  }

  onleaveupdate(params:any){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.router.navigate(['/hrm/Probationleaveupdate',encryptedParam]) 
  }






























}
