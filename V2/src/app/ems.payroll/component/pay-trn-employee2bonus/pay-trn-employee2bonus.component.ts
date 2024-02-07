import { Component, OnInit, OnDestroy, ChangeDetectorRef, Renderer2, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { SelectionModel } from '@angular/cdk/collections';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { Subscription, Observable } from 'rxjs';
import { first } from 'rxjs/operators';
import { NgbTimepickerModule, NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ActivatedRoute, Router } from '@angular/router';
export class IEmployee {
  employee2bonus_list: string[] = [];
  employee_gid:any;
}
@Component({
  selector: 'app-pay-trn-employee2bonus',
  templateUrl: './pay-trn-employee2bonus.component.html',
  styleUrls: ['./pay-trn-employee2bonus.component.scss']
})

export class PayTrnEmployee2bonusComponent {
  bonusgid: any;
  bonusgid1: any;
  user_gid: any;
  reactiveForm!: FormGroup;
  employee2bonus_list: any[] = [];
  selectedEmployeeList: any[] = [];
  selection = new SelectionModel<IEmployee>(true, []);

  constructor(
    private renderer: Renderer2,
    private el: ElementRef,
    public service: SocketService,
    private ToastrService: ToastrService,
    private route: Router,
    private router: ActivatedRoute,
    private formBuilder: FormBuilder
  ) {}

  ngOnInit(): void {
    debugger;
    const bonus_gid = this.router.snapshot.paramMap.get('bonus_gid');
    this.bonusgid = bonus_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.bonusgid, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam);
    this.bonusgid1 = deencryptedParam;
    this.Getemployee2bonusSummary(deencryptedParam);
    this.reactiveForm = this.formBuilder.group({
      // Define form controls here if needed
    });
  }
  hasSelectedEmployees(): boolean {
    return this.selectedEmployeeList.length > 0;
  }
  

  Getemployee2bonusSummary(bonus_gid: any) {
    var url = 'PayTrnBonus/GetBonusEmployeeSummary';
    let param = {
      bonus_gid: bonus_gid,
    };
    this.service.getparams(url, param).subscribe((result: any) => {
      this.employee2bonus_list = result.Getemployee2bonusSummary;
    });
  }

  submit() {
    debugger;
    for (const data of this.selectedEmployeeList) {
      const url = 'PayTrnBonus/PostBonusEmployee';
      const param = {
        user_gid: data.user_gid,
        bonus_gid: this.bonusgid1,
      };
      this.service.getparams(url, param).subscribe((result: any) => {
        if (result.status === false) {
          this.ToastrService.warning(result.message);
        } else {
          this.ToastrService.success(result.message);
        }
      });
    }
  }

  
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.employee2bonus_list.length;
    return numSelected === numRows;
  }
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.employee2bonus_list.forEach((row: IEmployee) => this.selection.select(row));
  }
}