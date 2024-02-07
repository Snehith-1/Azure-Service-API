import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SelectionModel } from '@angular/cdk/collections';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ActivatedRoute } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';

export class IContactList {
  contacts_list: string[] = [];
  project_id: string = "";
  sendtext: string = "";
}

@Component({
  selector: 'app-crm-mst-wabulkmessage',
  templateUrl: './crm-mst-wabulkmessage.component.html',
  styleUrls: ['./crm-mst-wabulkmessage.component.scss']
})
export class CrmMstWabulkmessageComponent implements OnInit {
  responsedata: any;
  contacts_list: any[] = [];
  selection = new SelectionModel<IContactList>(true, []);

  CurObj: IContactList = new IContactList();
  reactiveMessageForm!: FormGroup;
  pick: Array<any> = [];
  cboTemplate: any;
  sendtext:any;
  templateList: any;
  bulkMessageForm: FormGroup | any;
  bulkMessageForm1: FormGroup | any;
  templateview_list: any;
  template_body: any;
  p_name: any;
  footer: any;
  media_url: any;

  ngOnInit(): void {
    this.getContactList();
    this.gettemplateList();
  }
  constructor(private formBuilder: FormBuilder, private route: Router, private router: Router, private ToastrService: ToastrService, public service: SocketService, private NgxSpinnerService: NgxSpinnerService) {
    this.bulkMessageForm = new FormGroup({
      cboTemplate: new FormControl(null, Validators.required),
      id: new FormControl()
    });
    this.bulkMessageForm1 = new FormGroup({
      id: new FormControl(),
      sendtext:new FormControl(),
    });
  }
  getContactList() {
    this.NgxSpinnerService.show();
    var url = 'Whatsapp/GetContact'
    this.service.get(url).subscribe((result: any) => {
      $('#whatsnamelist').DataTable().destroy();
      this.contacts_list = result.whatscontactlist;
      setTimeout(() => {
        $('#whatsnamelist').DataTable();
      }, 1);
      this.NgxSpinnerService.hide();
    });
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.contacts_list.length;
    return numSelected === numRows;
  }

  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.contacts_list.forEach((row: IContactList) => this.selection.select(row));
  }
  onsend(): void {
    this.pick = this.selection.selected
    this.CurObj.contacts_list = this.pick
    this.CurObj.project_id = this.cboTemplate
    if (this.CurObj.project_id == undefined) {
      this.ToastrService.warning("Kindly select a template to send")
      return;
    }
    if (this.CurObj.contacts_list.length == 0) {
      this.ToastrService.warning("Kindly select atleast 1 contact")
      return;
    }
    this.NgxSpinnerService.show();
    var url = 'Whatsapp/bulkMessageSend'
    this.service.post(url, this.CurObj).subscribe((result: any) => {
      if (result.status == false) {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.warning(result.message)
      }
      else {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.success(result.message)
      }
      this.NgxSpinnerService.hide();
      this.selection.clear();
      this.p_name = undefined
      this.template_body = undefined
      this.cboTemplate = undefined
      this.footer = undefined
      this.media_url = undefined
    });
  }

  onsend1(): void {
    this.pick = this.selection.selected
    this.CurObj.contacts_list = this.pick
    this.CurObj.sendtext = this.bulkMessageForm1.value.sendtext
    // if (this.CurObj.project_id == undefined) {
    //   this.ToastrService.warning("Kindly select a template to send")
    //   return;
    // }
    if (this.CurObj.contacts_list.length == 0) {
      this.ToastrService.warning("Kindly select atleast 1 contact")
      return;
    }
    this.NgxSpinnerService.show();
    var url = 'Whatsapp/bulkcustomizeMessageSend'
    this.service.post(url, this.CurObj).subscribe((result: any) => {
      if (result.status == false) {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.warning(result.message)
      }
      else {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.success(result.message)
      }
      this.NgxSpinnerService.hide();
      this.selection.clear();
     this.bulkMessageForm1.reset();
    });
  }

  gettemplateList() {
    this.NgxSpinnerService.show();
    var url = 'Whatsapp/GetMessageTemplatesummary'
    this.service.get(url).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      $('#template_list').DataTable().destroy();
      this.templateList = result.whatsappMessagetemplatelist;
      setTimeout(() => {
        $('#template_list').DataTable();
      }, 1);
    });
  }
  redirecttolist() {
    this.router.navigate(['/crm/CrmSmmWhatsappcampaign']);

  }
  marketingteam() {
    let id = this.bulkMessageForm.get("cboTemplate")?.value;
    let params = {
      project_id: id
    }
    var url = 'Whatsapp/GetTemplatepreview'
    this.service.getparams(url, params).subscribe((result: any) => {
      
      this.p_name = result.Gettemplateview_list[0].p_name;
      this.template_body = result.Gettemplateview_list[0].template_body;
      this.footer = result.Gettemplateview_list[0].footer;
      this.media_url = result.Gettemplateview_list[0].media_url;
    });
  }
  public onMessagesent(gid: string, id: string): void {
    this.reactiveMessageForm.value.identifierValue = gid;
    this.reactiveMessageForm.value.contact_id = id;
    if (this.reactiveMessageForm.value.sendtext != null) {

      var url = 'Whatsapp/WhatsappSend'
      this.service.post(url, this.reactiveMessageForm.value).subscribe((result: any) => {
        if (result.status == false) {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.warning(result.message)
          //this.GetWhatsappMessageSummary();
          this.reactiveMessageForm.reset();
        }
        else {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.success(result.message)
          this.reactiveMessageForm.reset();
        }
      });
    }

    else {
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }


}
