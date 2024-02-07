import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { SelectionModel } from '@angular/cdk/collections';
import { NgxSpinnerService } from 'ngx-spinner';
export class IAssignlead {
  mailtemplate_list: string[] = [];
  temp_mail_gid: string = "";
}

@Component({
  selector: 'app-crm-smm-mailcampaignsummary',
  templateUrl: './crm-smm-mailcampaignsummary.component.html',
  styleUrls: ['./crm-smm-mailcampaignsummary.component.scss']
})
export class CrmSmmMailcampaignsummaryComponent {
  reactiveForm!: FormGroup;
  CurObj: IAssignlead = new IAssignlead();
  selection = new SelectionModel<IAssignlead>(true, []);
  pick: Array<any> = [];
  responsedata: any;
  formData: any = {};
  mailtemplate_list: any;
  temp_mail_gid: any;
  mailevent_list: any;
  mailcount_list: any[] = [];
  clicktotal_count:any;
  deliverytotal_count:any;
  opentotal_count:any;
  mailsendtotal_count:any;
  template_count:any;

  constructor(public service: SocketService, private route: Router, private ToastrService: ToastrService, private NgxSpinnerService: NgxSpinnerService)  {

  }
  ngOnInit(): void {
    this.GetTemplateSummary();
    this.GetMailEventCount();
    this.GetMailEventClick();
    this.GetMailEventDelivery();
    this.GetMailEventOpen();
    

  }


  GetTemplateSummary() {
    this.NgxSpinnerService.show();
    var api3 = 'MailCampaign/TemplateSummary'
    this.service.get(api3).subscribe((result: any) => {
      $('#mailtemplate_list').DataTable().destroy();
      this.responsedata = result;
      this.mailtemplate_list = this.responsedata.mailtemplate_list;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#mailtemplate_list').DataTable();
      }, 1);
    });

  }
  GetMailEventCount(){
    var api2 = 'MailCampaign/GetMailEventCount'
    this.service.get(api2).subscribe((result:any) => {
    this.mailcount_list = result.mailcount_list; 
    this.clicktotal_count=this.mailcount_list[0].clicktotal_count;
    this.opentotal_count=this.mailcount_list[0].opentotal_count;
    this.deliverytotal_count=this.mailcount_list[0].deliverytotal_count;
    this.mailsendtotal_count=this.mailcount_list[0].mailsendtotal_count;
    this.template_count=this.mailcount_list[0].template_count;
    });}
    
    GetMailEventOpen() {
      var api = 'MailCampaign/GetMailEventOpen'
      this.service.get(api).subscribe((result: any) => {
        this.responsedata = result;
        this.mailevent_list = this.responsedata.mailevent_list;
      });
    }
    GetMailEventClick() {
      var api = 'MailCampaign/GetMailEventClick'
      this.service.get(api).subscribe((result: any) => {
        this.responsedata = result;
        this.mailevent_list = this.responsedata.mailevent_list;
      });
    }
    GetMailEventDelivery() {
      var api = 'MailCampaign/GetMailEventDelivery'
      this.service.get(api).subscribe((result: any) => {
        this.responsedata = result;
        this.mailevent_list = this.responsedata.mailevent_list;
      });
    }
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.mailtemplate_list.length;
    return numSelected === numRows;
  }

  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.mailtemplate_list.forEach((row: IAssignlead) => this.selection.select(row));
  }
  public onsend(params: any): void {
    const secretKey = 'storyboarderp';
    //console.log(params)
    const temp_mail_gid = AES.encrypt(params.temp_mail_gid, secretKey).toString();
    
    this.route.navigate(['/crm/CrmSmmMailcampaignsend',temp_mail_gid])

  }
  

  onadd() {
    this.route.navigate(['/crm/CrmSmmMailcampaigntemplate']);
  }

  onopen() {
    this.route.navigate(['/crm/CrmSmmEmailmanagement']);
  }

  public ontemplateview(params: any): void{
    const secretKey = 'storyboarderp';
    //console.log(params)
    const temp_mail_gid = AES.encrypt(params.temp_mail_gid, secretKey).toString();
    this.route.navigate(['/crm/CrmSmmMailcampaigntemplateview',temp_mail_gid])
  }

  public mailstatus(params: any): void {
    const secretKey = 'storyboarderp';
    //console.log(params)
    const temp_mail_gid = AES.encrypt(params.temp_mail_gid, secretKey).toString();
   
    this.route.navigate(['/crm/CrmSmmMailcampaignsendstatus',temp_mail_gid])



  }



}