import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { Subscription, timer } from "rxjs";
import { map, share } from "rxjs/operators";
import { DatePipe } from '@angular/common';
import { NgxSpinnerService } from 'ngx-spinner';
import { saveAs } from 'file-saver';
import { Location } from '@angular/common';
import {
  CountryISO,
  SearchCountryField,

} from "ngx-intl-tel-input";

import { TabsModule } from 'ngx-bootstrap/tabs';
import { TabsetComponent } from 'ngx-bootstrap/tabs';

interface IWatsapp {
  //sourceedit_name: any;
  leadbank_gid: string;
  created_date: string;
  customer_name: string;
  displayName: string;
  mobile: string;
  created_by: string;
  computedDisplayName: string;
  email: string;
  first_letter: string;
  key: string;
  value: string;
  firstName: string;
  lastName: string;
  gender: string;
  identifierValue: string;
  type: string;
  sendtext: string;
  phone:string;

}
interface IMailform {
  leadbank_gid: string;
  mail_from: string;
  sub: string;
  to: string;
  body: string;
  bcc: any;
  cc: any;
  reply_to: any;
  to_mail:any;
}
interface IMyleads {

  
  leadbank_gid: string;
  leadbank_gid1: string;
  lead2campaign_gid: string;
  schedule_type: string;
  schedule: string;
  ScheduleRemarks1: string;
  schedule_status: string;
  schedule_date:string;
  schedule_time:string;
}

@Component({
  selector: 'app-smr-trn-sales360',
  templateUrl: './smr-trn-sales360.component.html',
  styleUrls: ['./smr-trn-sales360.component.scss']
})
export class SmrTrnSales360Component {
  @ViewChild('Inbox') tableRef!: ElementRef;
  time = new Date();
  rxTime = new Date();
  intervalId: any;
  customer_gid:any;
  subscription!: Subscription;
  currentDayName: string;
  fromDate: any; toDate: any;

  search: string = '';
  response_data: any;
  mailmanagement: any[] = [];
  reactiveForm!: FormGroup;
  parameterValue1: any;
  mailsummary_list: any;
  mail: any;
  from_mail: any;
  to_mail: any;
  subject: any;
  body_content: any;
  mailcount_list: any[] = [];
  responsedata: any;
  filteredData: any;
  clicktotal_count: any;
  deliverytotal_count: any;
  opentotal_count: any;
  searchTerm: string = '';
  searchResults: string[] = [];
  searchText: any;
  items = [];
  currentPage: number = 1;
  pageSize: number = 50;
  mailevent_list: any;
  whatsappmessage_list: any[] = [];
  whatsapp_list: any[] = [];
  count_list: any[] = [];
  reactiveMessageForm!: FormGroup;
  leadbank!: IWatsapp;
  file: any;

  leadbank_gid: any;
  mailid_list: any[] = [];
  mailform!: IMailform;
  NotesreactiveForm!: FormGroup;
  branchList: any[] = [];
  designation_list: any[] = [];
  country_list2: any[] = [];
  leadorderdetails_list: any[] = [];
  leadquotationdetails_list: any[] = [];
  leadinvoicedetails_list: any[] = [];
  leadcountdetails_list: any[] = [];
  messagecount_list: any;
  message_count: any;
  mail_sent: any;
  sentmailcount_list: any;
  lspage: any;
  leadbasicdetailslist: any[] = [];
  leaddocumentdetail_list: any[] = [];
  id: any;
  mailopen: boolean = true;
  mailreply: boolean = true;
  mailview_list: any;
  body: any;
  created_date: any;
  created_time: any;
  internalnotes: any;
  leadbanknotes_list: any[] = [];
  leadgig: any;
  txtinternal_notes: any;
  documentuploadlist: any[] = [];
  document_upload: any[] = [];
  file1!: FileList;
  file_name: any;
  formDataObject: FormData = new FormData();
  AutoIDkey: any;
  allattchement: any[] = [];
  reactiveFormEdit!: FormGroup;
  to_address: any;
  opencomposemail: boolean = true;
  from_address: any;
  direction: any;
  document_path: any;
  isReadOnly = true;
  orderleadbank_gid: any;
  orderlead2campaign_gid: any;
  orderleadbankcontact_gid: any;
  quotationleadbank_gid: any;
  quotationlead2campaign_gid: any;
  quotationleadbankcontact_gid: any;
  salesorder_gid: any;
  invoice_gid: any;
  quotation_gid: any;
  absURL: any;
  whatsappMessagetemplatelist: any[] = [];
  openDiv: boolean = false;
  filetype: string = "";
  SearchCountryField = SearchCountryField;
  CountryISO = CountryISO;
  preferredCountries: CountryISO[] = [
    CountryISO.India,
    CountryISO.India
  ];
  Enquirylist: any;
  enquiryleadbank_gid: any;
  enquirylead2campaign_gid: any;
  enquiryleadbankcontact_gid: any;
  enquiry_gid: any;
  gid_list: any[] = [];
  lead2campaign_gid: any;
  leadbankcontact_gid: any;
  parameterValue: any;
  editcustomer_list: any[] = [];
  sending_domain: any;
  receiving_domain: any;
  schedulesummary_list: any[] = [];
  leadbank_name: any;
  reactiveFormfollow!: FormGroup;
  myleads!: IMyleads;
  ScheduleType = [
  
    { type: 'Meeting', },
  
  ];


  matchesSearch(item: any): boolean {
    const searchString = this.searchText.toLowerCase();
    return item.displayName.toLowerCase().includes(searchString) || item.value.toLowerCase().includes(searchString);
  }

  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService, private datePipe: DatePipe, private NgxSpinnerService: NgxSpinnerService, private location: Location) {
    this.leadbank = {} as IWatsapp;
    this.mailform = {} as IMailform;
    this.myleads = {} as IMyleads;
    const today = new Date();
    this.currentDayName = today.toLocaleDateString('en-US', { weekday: 'long' });

  }

  ngOnInit(): void {
    const secretKey = 'storyboarderp';
    const leadbank_gid = this.route.snapshot.paramMap.get('leadbank_gid');
    this.leadbank_gid = leadbank_gid;
    const deencryptedParam = AES.decrypt(this.leadbank_gid, secretKey).toString(enc.Utf8);
    this.leadgig = deencryptedParam;
    const lspage = this.route.snapshot.paramMap.get('lspage');
    this.lspage = lspage;
    const deencryptedParam1 = AES.decrypt(this.lspage, secretKey).toString(enc.Utf8);

    this.lspage = deencryptedParam1;
    this.absURL = window.location.origin
    var url = 'Leadbank360/GetLeadBasicDetails'
    let param = {
      leadbank_gid: deencryptedParam
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.leadbasicdetailslist = this.responsedata.leadbasicdetails_list;
      this.to_mail = this.leadbasicdetailslist[0].email;
      this.leadbank_name = this.leadbasicdetailslist[0].leadbankcontact_name;
      this.reactiveForm.patchValue({
        // leadbank_gid:deencryptedParam,
        to_mail: this.to_mail,
      
      });
      // console.log(this.responsedata.leadbasicdetails_list,'leadbasicdetails_list');
    });
    // var api2 = 'Whatsapp/GetMessageTemplatesummary'
    // this.service.get(api2).subscribe((result: any) => {
    //   this.responsedata = result;
    //   this.whatsappMessagetemplatelist = result.whatsappMessagetemplatelist;
    // });

    var url = 'Leadbank360/GetLeadDocumentDetails'
    let params = {
      leadbank_gid: deencryptedParam
    }    
    this.service.getparams(url, params).subscribe((result: any) => {
      this.responsedata = result;
      this.leaddocumentdetail_list = this.responsedata.leaddocumentdetails;
    });
    var url = 'Leadbank360/GetNotesSummary'
    let params1 = {
      leadbank_gid: deencryptedParam
    }
    this.service.getparams(url, params1).subscribe((result: any) => {
      this.responsedata = result;
      this.internalnotes = this.responsedata.notes;
      this.NotesreactiveForm.get("internalnotestext_area")?.setValue(this.internalnotes[0].internal_notes);
      // this.txtinternal_notes = this.internalnotes[0].internal_notes;
      console.log(this.txtinternal_notes, 'testinternalnotes')
    });

    //this.GetMailSummary(deencryptedParam);
    this.GetOrderDetailsSummary(deencryptedParam);
    this.GetQuotationDetailsSummary(deencryptedParam);
    this.GetInvoiceDetailsSummary(deencryptedParam);
    this.GetLeadCountDetails(deencryptedParam);
    // this.GetWhatsappMessageSummary(deencryptedParam);
    // this.GetWhatsappSummary(deencryptedParam);
    this.GetEnquiryDetailsSummary(deencryptedParam);
    this.GetGiddetails(deencryptedParam);
    // this.Getshedulesummary(deencryptedParam);

    let yesterday = new Date();
    yesterday.setDate(yesterday.getDate() - 1);
    // console.log(yesterday);
    this.fromDate = this.datePipe.transform(yesterday, 'dd-MM-yyyy');
    this.toDate = this.datePipe.transform(new Date(), 'dd-MM-yyyy');
    // console.log(this.fromDate); 

    this.intervalId = setInterval(() => {
      this.time = new Date();
    }, 1000);

    // Using RxJS Timer
    this.subscription = timer(0, 1000)
      .pipe(
        map(() => new Date()),
        share()
      )
      .subscribe(time => {
        let hour = this.rxTime.getHours();
        let minuts = this.rxTime.getMinutes();
        let seconds = this.rxTime.getSeconds();
        //let a = time.toLocaleString('en-US', { hour: 'numeric', hour12: true });
        let NewTime = hour + ":" + minuts + ":" + seconds
        // console.log(NewTime);
        this.rxTime = time;
      });

    this.reactiveForm = new FormGroup({

      customer_name: new FormControl(this.leadbank.customer_name, [
        Validators.required,
      ]),
      displayName: new FormControl(this.leadbank.displayName, [
        Validators.required,
      ]),
      mobile: new FormControl(''),
      value: new FormControl(this.leadbank.value, [
        Validators.required,
      ]),
      phone: new FormControl(this.leadbank.phone, [
        Validators.required,]),
      gender: new FormControl(''),
      firstName: new FormControl(''),
      lastName: new FormControl(''),
      email : new FormControl(''),
      address1 : new FormControl(''),
      sub: new FormControl(this.mailform.sub, [
        Validators.required,
      ]),

      file: new FormControl(''),
      body: new FormControl(''),
      bcc: new FormControl(''),
      cc: new FormControl(''),
      leadbank_gid: new FormControl(''),
      to_mail: new FormControl(''),
      mail_from: new FormControl('')
    });


    this.reactiveMessageForm = new FormGroup({
      identifierValue: new FormControl(''),
      type: new FormControl(''),
      sendtext: new FormControl(''),
      template_name: new FormControl(''),
      p_name: new FormControl(''),
      project_id: new FormControl(),
      version: new FormControl(''),
      message_id: new FormControl(''),
      contact_id: new FormControl(''),
    });
       


    this.NotesreactiveForm = new FormGroup({
      leadgig: new FormControl(''),
      internalnotestext_area: new FormControl(''),
      file: new FormControl(''),
      fileExtension: new FormControl(''),
      fileName: new FormControl(''),
      imagePath: new FormControl(''),
      document_title: new FormControl(''),
      remarks: new FormControl(''),
    });
    this.reactiveFormfollow = new FormGroup({
      schedule_date: new FormControl(this.myleads.schedule_date, [
        Validators.required,
      ]),
      schedule_time: new FormControl(this.myleads.schedule_time, [
        Validators.required,
      ]),
      schedule_type: new FormControl(this.myleads.schedule_type, [
        Validators.required,
      ]),
      schedule_remarks: new FormControl(''),
      ScheduleRemarks1: new FormControl(''),
      leadbank_gid: new FormControl(''),
      lead2campaign_gid: new FormControl(''),
      log_details : new FormControl(''),
      log_legend : new FormControl(''),
    });
    
    var url6 = 'SocialMedia/GetMessageCount'
    this.service.get(url6).subscribe((result,) => {

      this.messagecount_list = result;
      this.message_count = this.messagecount_list.messagecount_list[0].message_count;
    });

    var url10 = 'SocialMedia/GetSentCount'
    this.service.get(url10).subscribe((result,) => {

      this.sentmailcount_list = result;
      this.mail_sent = this.sentmailcount_list.sentmailcount_list[0].mail_sent;

    });
    this.reactiveForm.patchValue({
      leadbank_gid:deencryptedParam,
      // mail_from: this.to_mail,
    
    });
    this.reactiveFormfollow.patchValue({
      leadbank_gid:deencryptedParam,
      // mail_from: this.to_mail,
    
    });

  }
//GetGidDetails
GetGiddetails(deencryptedParam: any){
  var url = 'Leadbank360/GetGidDetails'
  let param = {
    leadbank_gid: deencryptedParam
  }
  this.service.getparams(url, param).subscribe((result: any) => {
    // $('#leadorderdetails_list').DataTable().destroy();
    this.responsedata = result;
    this.gid_list = this.responsedata.gid_list;

    this.leadbank_gid = this.gid_list[0].leadbank_gid;
    this.lead2campaign_gid = this.gid_list[0].lead2campaign_gid;
    this.leadbankcontact_gid = this.gid_list[0].leadbankcontact_gid;
    //this.salesorder_gid = this.gid_list[0].salesorder_gid;

    setTimeout(() => {
      $('#leadorderdetails_list').DataTable();
    }, 1);
    // console.log(this.responsedata.leadorderdetailslist,'leadorderdetails_list'); 
  });

}

  GetOrderDetailsSummary(deencryptedParam: any) {
    // console.log(deencryptedParam,'testleadbank_gid');
    var url = 'Leadbank360/GetLeadOrderDetails'
    let param = {
      leadbank_gid: deencryptedParam
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      // $('#leadorderdetails_list').DataTable().destroy();
      this.responsedata = result;
      this.leadorderdetails_list = this.responsedata.leadorderdetailslist;

      this.orderleadbank_gid = this.leadorderdetails_list[0].leadbank_gid;
      this.orderlead2campaign_gid = this.leadorderdetails_list[0].lead2campaign_gid;
      this.orderleadbankcontact_gid = this.leadorderdetails_list[0].leadbankcontact_gid;
      this.salesorder_gid = this.leadorderdetails_list[0].salesorder_gid;

      setTimeout(() => {
        $('#leadorderdetails_list').DataTable();
      }, 1);
      // console.log(this.responsedata.leadorderdetailslist,'leadorderdetails_list'); 
    });
  }

  GetEnquiryDetailsSummary(deencryptedParam: any) {
    var url = 'Leadbank360/GetEnquiryDetails'
    let param = {
      leadbank_gid: deencryptedParam
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.Enquirylist = this.responsedata.Enquiry_list;

      this.enquiryleadbank_gid = this.Enquirylist[0].leadbank_gid;
      this.enquirylead2campaign_gid = this.Enquirylist[0].lead2campaign_gid;
      this.enquiryleadbankcontact_gid = this.Enquirylist[0].leadbankcontact_gid;
      this.enquiry_gid = this.Enquirylist[0].quotation_gid;
      
      setTimeout(() => {
        $('#Enquirylist').DataTable();
      }, 1);
      // console.log(this.responsedata.leadquotationdetailslist,'leadquotationdetails_list'); 
    });
  }

  GetQuotationDetailsSummary(deencryptedParam: any) {
    var url = 'Leadbank360/GetLeadQuotationDetails'
    let param = {
      leadbank_gid: deencryptedParam
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      // $('#leadquotationdetails_list').DataTable().destroy();
      this.responsedata = result;
      this.leadquotationdetails_list = this.responsedata.leadquotationdetailslist;

      this.quotationleadbank_gid = this.leadquotationdetails_list[0].leadbank_gid;
      this.quotationlead2campaign_gid = this.leadquotationdetails_list[0].lead2campaign_gid;
      this.quotationleadbankcontact_gid = this.leadquotationdetails_list[0].leadbankcontact_gid;
      this.quotation_gid = this.leadquotationdetails_list[0].quotation_gid;
      
      setTimeout(() => {
        $('#leadquotationdetails_list').DataTable();
      }, 1);
      console.log(this.responsedata.leadquotationdetailslist,'leadquotationdetails_list'); 
    });
  }
  GetLeadCountDetails(deencryptedParam: any) {
    var url = 'Leadbank360/GetLeadCountDetails'
    let param = {
      leadbank_gid: deencryptedParam
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.leadcountdetails_list = this.responsedata.leadcountdetails;
      // console.log(this.responsedata.leadcountdetails,'leadcountdetails_list'); 
    });
  }
  GetInvoiceDetailsSummary(deencryptedParam: any) {
    var url = 'Leadbank360/GetLeadInvoiceDetails'
    let param = {
      leadbank_gid: deencryptedParam
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      $('#leadinvoicedetails_list').DataTable().destroy();
      this.responsedata = result;
      this.leadinvoicedetails_list = this.responsedata.leadinvoicedetailslist;

      this.invoice_gid = this.leadinvoicedetails_list[0].invoice_gid;
      setTimeout(() => {
        $('#leadinvoicedetails_list').DataTable();
      }, 1);
      // console.log(this.responsedata.leadinvoicedetailslist,'leadinvoicedetails_list'); 
    });
  }


 //////////////////Mail functions Starts///////////////////
//  GetMailSummary(deencryptedParam: any) {
//   let params = {
//     leadbank_gid: deencryptedParam
//   }
//   var api = 'MailCampaign/GetIndividualMailSummary';
//   this.service.getparams(api, params).subscribe((result: any) => {
//     this.response_data = result;
//     this.mailsummary_list = this.response_data.mailsummary_list;
//     this.sending_domain= this.response_data.sending_domain;
//     this.receiving_domain = this.response_data.receiving_domain;
//      this.reactiveForm.patchValue({
//       mail_from: this.sending_domain,
//     });
//     setTimeout(() => {
//       $('#mailsummary_list').DataTable();
//     }, 1);
//   });
// }
// onbackmail() {
//   this.mailopen = true;
//   this.mailreply=true

// }
// viewpage(mailmanagement_gid: any) {
//   this.mailopen = !this.mailopen;
//   var url = 'MailCampaign/GetMailView';
//   let param = {
//     mailmanagement_gid: mailmanagement_gid
//   }
//   this.service.getparams(url, param).subscribe((result: any) => {
//     this.mailview_list = result.mailsummary_list;
//     this.from_mail = this.mailview_list[0].mail_from;
//     this.subject = this.mailview_list[0].sub;
//     this.body = this.mailview_list[0].body;
//     this.created_date = this.mailview_list[0].created_date;
//     this.direction = this.mailview_list[0].direction;
//     this.document_path = this.mailview_list[0].document_path;
//     this.created_time = this.mailview_list[0].created_time;
//     this.to_mail = this.mailview_list[0].to;

//   });

// }
// onreply() {
//   this.mailreply = !this.mailreply;

// }
onChange2($event: any): void {
  this.file1 = $event.target.files;

  if (this.file1 != null && this.file1.length !== 0) {
    for (let i = 0; i < this.file1.length; i++) {
      this.AutoIDkey = this.generateKey();
      this.formDataObject.append(this.AutoIDkey, this.file1[i]);
      this.file_name = this.file1[i].name;
      this.allattchement.push({
        AutoID_Key: this.AutoIDkey,
        file_name: this.file1[i].name
      });
      console.log(this.file1[i]);
    }
  }

  //console.log(this.files[i]);
}
onChange4(event: any) {
  this.file = event.target.files[0];
}


generateKey(): string {

  return `AutoIDKey${new Date().getTime()}`;
}
setFileType(data: string) {
  this.filetype = data;
}

popattachments() {
  this.opencomposemail = !this.opencomposemail;
}

// public onadd(): void {
//   console.log(this.reactiveForm.value)
//   this.mailform = this.reactiveForm.value;
//   if ( this.mailform.sub != null) {
//     const allattchement = "" + JSON.stringify(this.allattchement) + "";
//     if (this.file1 != null && this.file1 != undefined) {
//       this.formDataObject.append("filename", allattchement);
//       this.formDataObject.append("mail_from", this.mailform.mail_from);
//       this.formDataObject.append("sub", this.mailform.sub);
//       this.formDataObject.append("to", this.mailform.to_mail);
//       this.formDataObject.append("body", this.mailform.body);
//       this.formDataObject.append("bcc", this.mailform.bcc);
//       this.formDataObject.append("cc", this.mailform.cc);
//       // this.formDataObject.append("reply_to", this.mailform.reply_to);
//       this.formDataObject.append("leadbank_gid", this.mailform.leadbank_gid);

//       var api7 = 'MailCampaign/MailUpload'
//       this.service.post(api7, this.formDataObject).subscribe((result: any) => {
//         this.responsedata = result;
//         if (result.status == false) {
//           this.ToastrService.warning(result.message)
//         }
//         else {
//           window.scrollTo({

//             top: 0, // Code is used for scroll top after event done

//           });
//           // this.router.navigate(['/crm/CrmSmmEmailmanagement']);
//           this.ToastrService.success(result.message)
//         }
//       });
//     }
//     else {
//       var api7 = 'MailCampaign/MailSend'
//       //console.log(this.file)
//       this.service.post(api7, this.mailform).subscribe((result: any) => {

//         if (result.status == false) {
//           window.scrollTo({

//             top: 0, // Code is used for scroll top after event done

//           });
//           this.ToastrService.warning(result.message)
//         }
//         else {
//           window.scrollTo({

//             top: 0, // Code is used for scroll top after event done

//           });
//           // this.router.navigate(['/crm/CrmSmmEmailmanagement']);
//           this.ToastrService.success(result.message)
//         }
//         this.responsedata = result;
//       });
//     }
//   }

//   else {
//     window.scrollTo({

//       top: 0, // Code is used for scroll top after event done

//     });
//     this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
//   }
//   return;

// }
// get mail_from() {
//   return this.reactiveForm.get('mail_from')!;
// }
// get to() {
//   return this.reactiveForm.get('to')!;
// }
// get sub() {
//   return this.reactiveForm.get('sub')!;
// }
// get reply_to() {
//   return this.reactiveForm.get('reply_to')!;
// }
// get cc() {
//   return this.reactiveForm.get('cc')!;
// }
// get bcc() {
//   return this.reactiveForm.get('bcc')!;
// }
get email(){
  return this.reactiveForm.get('email')!;
}
get address1(){
  return this.reactiveForm.get('address1')!;
}

// config: AngularEditorConfig = {
//   editable: true,
//   spellcheck: true,
//   height: '20rem',
//   width: '100%',
//   placeholder: 'Enter text here...',
//   translate: 'no',
//   defaultParagraphSeparator: 'p',
//   defaultFontName: 'Arial',
// };
// config_compose_mail: AngularEditorConfig = {
//   editable: true,
//   spellcheck: true,
//   height: '120px',
//   minHeight: '0rem',
//   width: '1013px',
//   placeholder: 'Enter text here...',
//   translate: 'no',
//   defaultParagraphSeparator: 'p',
//   defaultFontName: 'Arial',

// };
//////////////////Mail functions Ends///////////////////


  // GetWhatsappMessageSummary(deencryptedParam: any) {
  //   var url = 'Leadbank360/GetWhatsappLeadMessage'
  //   let param = {
  //     leadbank_gid: deencryptedParam
  //   }
  //   this.service.getparams(url, param).subscribe((result: any) => {
  //     $('#whatsmessagelist').DataTable().destroy();
  //     this.responsedata = result;
  //     this.whatsappmessage_list = this.responsedata.leadwhatsmessagelist;
  //     //console.log(this.source_list)

  //   });
  // }

  // GetWhatsappSummary(deencryptedParam: any) {
  //   var url = 'Leadbank360/GetWhatsappLeadContact'
  //   let param = {
  //     leadbank_gid: deencryptedParam
  //   }
  //   var url = 'Leadbank360/GetWhatsappLeadContact'
  //   this.service.getparams(url, param).subscribe((result: any) => {
  //     $('#whatsnamelist').DataTable().destroy();
  //     this.responsedata = result;
  //     this.whatsapp_list = this.responsedata.leadwhatscontactlist;
  //     //console.log(this.source_list)

  //   });
  // }


  // showResponsiveOutput(gid: string) {
  //   console.log('Clicked with gid:', gid);
  //   this.GetWhatsappMessageSummary(gid);
  // }

  // GetContactCount() {
  //   var url = 'Whatsapp/GetContactCount'
  //   this.service.get(url).subscribe((result: any) => {
  //     $('#count_list').DataTable().destroy();
  //     this.responsedata = result;
  //     this.count_list = this.responsedata.contactcount_list;
  //     //console.log(this.source_list)

  //   });
  // }

  // onclose() {
  //   this.reactiveForm.reset();
  // }

  // onChange1(event: any) {
  //   this.file = event.target.files[0];

  // }

  // isDropdownOpen = false;

  // sendMessage() {
  //   // Add your send message logic here
  // }
  // poptemplatemodal(parameter: string) {
  //   this.reactiveMessageForm.get("identifierValue")?.setValue(parameter);

  // }
  // toggleDropdown() {
  //   this.isDropdownOpen = !this.isDropdownOpen;
  // }



  // get customer_name() {
  //   return this.reactiveForm.get('customer_name')!;
  // }
  // get mobile() {
  //   return this.reactiveForm.get('mobile')!;
  // }
  // get value() {
  //   return this.reactiveForm.get('mobile')!;
  // }
  // get firstName() {
  //   return this.reactiveForm.get('customer_name')!;
  // }
  // get lastName() {
  //   return this.reactiveForm.get('mobile')!;
  // }
  get displayName() {
    return this.reactiveForm.get('mobile')!;
  }
  // get identifierValue() {
  //   return this.reactiveMessageForm.get('identifierValue')!;
  // }
  // get document_title() {
  //   return this.NotesreactiveForm.get('document_title')!;
  // }
  // attachments() {
  //   this.openDiv = !this.openDiv;
  // }
  // public onsubmit(): void {
  //    if (this.reactiveForm.value.displayName != null && this.reactiveForm.value.mobile.e164Number != null
  //     && this.reactiveForm.value.email != null && this.reactiveForm.value.address1) {

  //     this.reactiveForm.value;
  //     var url = 'Leadbank360/Getupdatecontactdetails'
  //     this.service.post(url, this.reactiveForm.value).subscribe((result: any) => {
  //       if (result.status == false) {
  //         window.scrollTo({
  //           top: 0, // Code is used for scroll top after event done
  //         });
  //         this.ToastrService.warning(result.message);
  //         window.location.reload();
  //         this.reactiveForm.reset();
  //       }
  //       else {
  //         window.scrollTo({
  //           top: 0, // Code is used for scroll top after event done
  //         });
  //         this.ToastrService.success(result.message);
  //         window.location.reload();
  //         this.reactiveForm.reset();
  //       }
  //       this.reactiveForm.reset();
  //     });
  //   }
  //   else {
  //     window.scrollTo({
  //       top: 0, // Code is used for scroll top after event done
  //     });
  //     this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
  //   }
  // }

  // // Message send //
  // public onMessagesent(gid: string, id: string): void {
  //   this.reactiveMessageForm.value.identifierValue = gid;
  //   this.reactiveMessageForm.value.contact_id = id;
  //   if (this.reactiveMessageForm.value.sendtext != null) {

  //     const identifierValue = this.reactiveMessageForm?.get('identifierValue')?.value;

  //     this.reactiveMessageForm.value;
  //     console.log("The passing vaues are" + this.reactiveMessageForm.value);

  //     var url = 'Whatsapp/WhatsappSend'
  //     this.service.post(url, this.reactiveMessageForm.value).subscribe((result: any) => {
  //       if (result.status == false) {
  //         window.scrollTo({
  //           top: 0, // Code is used for scroll top after event done
  //         });
  //         this.ToastrService.warning(result.message)
  //         //this.GetWhatsappMessageSummary();
  //         this.reactiveMessageForm.reset();
  //       }
  //       else {
  //         window.scrollTo({
  //           top: 0, // Code is used for scroll top after event done
  //         });
  //         this.ToastrService.success(result.message)
  //         //this.GetWhatsappMessageSummary();
  //         this.reactiveMessageForm.reset();
  //       }
  //       // this.GetSourceSummary();
  //       this.GetWhatsappMessageSummary(this.leadgig);
  //     });
  //   }
  //   else {
  //     window.scrollTo({
  //       top: 0, // Code is used for scroll top after event done
  //     });
  //     this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
  //   }

  // }
  // public onTemplatesent(id: string, version: string): void {
    
  //   this.reactiveMessageForm.get("project_id")?.setValue(id);
  //   this.reactiveMessageForm.get("version")?.setValue(version);
  //   let identifierValue = this.reactiveMessageForm.value.identifierValue;
  //   let project_id = id;
  //   let param = {
  //     identifierValue: identifierValue,
  //     project_id: project_id,
  //   }

  //   if (project_id != null) {
  //     this.reactiveMessageForm.value.param = param
     
  //     var url = 'Whatsapp/WhatsappSend'
  //     this.service.post(url, this.reactiveMessageForm.value).subscribe((result: any) => {
      
  //       if (result.status == false) {
  //         window.scrollTo({
  //           top: 0, // Code is used for scroll top after event done
  //         });
  //         this.ToastrService.warning(result.message)
  //         this.reactiveMessageForm.reset();
  //         //this.GetWhatsappMessageSummary();
  //       }
  //       else {
  //         window.scrollTo({
  //           top: 0, // Code is used for scroll top after event done
  //         });
  //         this.ToastrService.success(result.message)
  //         this.reactiveMessageForm.reset();
  //       }
  //       this.GetWhatsappMessageSummary(this.leadgig);
  //     });
  //   }

  //   else {
  //     window.scrollTo({
  //       top: 0, // Code is used for scroll top after event done
  //     });
  //     this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
  //   }
  // }

  onback() {
    if (this.lspage == 'MM-Total') {
      this.router.navigate(['/crm/CrmTrnMarketingManagerSummary']);
    }
    else if (this.lspage == 'MM-Upcoming') {
      this.router.navigate(['/crm/CrmTrnUpcomingmarketing']);
    }
    else if (this.lspage == 'MM-Lapsed') {
      this.router.navigate(['/crm/CrmTrnLapsedleadmarketing']);
    }
    else if (this.lspage == 'MM-Longest') {
      this.router.navigate(['/crm/CrmTrnLongestleadmarketing']);
    }
    else if (this.lspage == 'MM-New') {
      this.router.navigate(['/crm/CrmTrnNewmarketing']);
    }
    else if (this.lspage == 'MM-Prospect') {
      this.router.navigate(['/crm/CrmTrnProspectmarketing']);
    }
    else if (this.lspage == 'MM-Potential') {
      this.router.navigate(['/crm/CrmTrnPotentialmarketing']);
    }
    else if (this.lspage == 'MM-mtd') {
      this.router.navigate(['/crm/CrmCrmTrnMtd']);
    }
    else if (this.lspage == 'MM-ytd') {
      this.router.navigate(['/crm/CrmTrnYtd']);
    }
    else if (this.lspage == 'MM-Customer') {
      this.router.navigate(['/crm/CrmTrnCustomermarketing']);
    }
    else if (this.lspage == 'MM-Drop') {
      this.router.navigate(['/crm/CrmTrnDropmarketing']);
    }
    else if (this.lspage == 'My-Today') {
      this.router.navigate(['/crm/CrmTrnMycampaign']);
    }
    else if (this.lspage == 'My-New') {
      this.router.navigate(['/crm/CrmTrnNewtask']);
    }
    else if (this.lspage == 'My-Prospect') {
      this.router.navigate(['/crm/CrmTrnProspects']);
    }
    else if (this.lspage == 'My-Potential') {
      this.router.navigate(['/crm/CrmTrnPotentials']);
    }
    else if (this.lspage == 'My-Customer') {
      this.router.navigate(['/crm/CrmTrnCompleted']);
    }
    else if (this.lspage == 'My-Drop') {
      this.router.navigate(['/crm/CrmTrnDropleads']);
    }
    else if (this.lspage == 'My-All') {
      this.router.navigate(['/crm/CrmTrnAllleads']);
    }
    else if (this.lspage == 'My-Upcoming') {
      this.router.navigate(['/crm/CrmTrnUpcomingMeetings']);
    }
    else {
      this.router.navigate(['/crm/CrmDashboard']);
    }
  }

  public Update(): void {
    this.NotesreactiveForm.value.leadgig = this.leadgig;
    // this.leadbanknotes_list = this.NotesreactiveForm.value;
    console.log(this.NotesreactiveForm.value, 'formdata');

    if (this.NotesreactiveForm.value.internalnotestext_area != null &&
      this.NotesreactiveForm.value.internalnotestext_area != "" ) {

      var api7 = 'Leadbank360/LeadNotesUpload'
      this.service.post(api7, this.NotesreactiveForm.value).subscribe((result: any) => {
        if (result.status == false) {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.warning(result.message)
          window.location.reload()
       
          
        }
        else {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.success(result.message)
          window.location.reload()
        }
        this.responsedata = result;
      });
    }
    else {
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
    return;
  }

  myModaladddetails(parameter: string) {
    this.parameterValue1 = parameter
    this.reactiveForm.get("product_gid")?.setValue(this.parameterValue1.product_gid);

  }
  //upload documents
  public ondocumentsubmit(): void {
    let formData = new FormData();
    this.document_upload = this.NotesreactiveForm.value;
    let document_title = this.NotesreactiveForm.value.document_title;
    let remarks = this.NotesreactiveForm.value.remarks;
    if (this.file != null && this.file != undefined) {
      formData.append("file", this.file, this.file.name);
      formData.append("leadbank_gid", this.leadgig);
      formData.append("remarks", remarks);
      formData.append("document_title", document_title);
      //this.NgxSpinnerService.show();
      var api7 = 'Leadbank360/LeadDocumentUpload'
      this.service.postfile(api7, formData).subscribe((result: any) => {
        if (result.status == false) {
          this.ToastrService.warning(result.message);
          window.location.reload();
          //this.NgxSpinnerService.hide();
        }
        else {
          // this.GetProductSummary();
          this.ToastrService.success(result.message);
          window.location.reload();
          //this.NgxSpinnerService.hide();

        }
        this.responsedata = result;
      });

    }
  }

  ondocChange2(event: any) {
    this.file = event.target.files[0];
  }


  downloadImage(doctitlelist: any) {
    if (doctitlelist.document_upload != null && doctitlelist.document_upload != "") {
      if (doctitlelist.document_type === 'mylead') {
        saveAs(doctitlelist.document_upload, `${doctitlelist.leadbank_gid}_file`);
      }
    
      else {
        saveAs(doctitlelist.document_upload, `${doctitlelist.leadbank_gid}_file`);
      }
    }
    else {
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });
      this.ToastrService.warning('No Image Found')

    }


  }


// Raise Enquiry
raiseenquiry() {
  debugger
  const secretKey = 'storyboarderp';
  console.log(this.leadbank_gid);
  console.log(this.lead2campaign_gid);
  console.log(this.leadbankcontact_gid);
  const lspage1 = this.lspage;
  const lspage = AES.encrypt(lspage1, secretKey).toString();
  const leadbank_gid = AES.encrypt(this.leadbank_gid, secretKey).toString();
  const lead2campaign_gid = AES.encrypt(this.lead2campaign_gid, secretKey).toString();
  const leadbankcontact_gid = AES.encrypt(this.leadbankcontact_gid, secretKey).toString();
  this.router.navigate(['/crm/CrmTrnTraiseenquiry',leadbank_gid,lead2campaign_gid,leadbankcontact_gid,lspage]);
} 

  // Raise order
raiseorder() {
  const secretKey = 'storyboarderp';
  console.log(this.leadbank_gid);
  console.log(this.lead2campaign_gid);
  console.log(this.leadbankcontact_gid);
  const lspage1 = this.lspage;
  const lspage = AES.encrypt(lspage1, secretKey).toString();
  const leadbank_gid = AES.encrypt(this.leadbank_gid, secretKey).toString();
  const lead2campaign_gid = AES.encrypt(this.lead2campaign_gid, secretKey).toString();
  const leadbankcontact_gid = AES.encrypt(this.leadbankcontact_gid, secretKey).toString();
  this.router.navigate(['/crm/CrmTrnTraiseorder',leadbank_gid,lead2campaign_gid,leadbankcontact_gid,lspage]);
} 

//Raise quotation
raisequotation() {
  const secretKey = 'storyboarderp';
  console.log(this.leadbank_gid);
  console.log(this.lead2campaign_gid);
  console.log(this.leadbankcontact_gid);
  const lspage1 = this.lspage;
  const lspage = AES.encrypt(lspage1, secretKey).toString();
  const leadbank_gid = AES.encrypt(this.leadbank_gid, secretKey).toString();
  const lead2campaign_gid = AES.encrypt(this.lead2campaign_gid, secretKey).toString();
  const leadbankcontact_gid = AES.encrypt(this.leadbankcontact_gid, secretKey).toString();
  this.router.navigate(['/crm/CrmTrnTraisequtation',leadbank_gid,lead2campaign_gid,leadbankcontact_gid,lspage]);
} 

//Raise Invoice
raiseinvoice() {
  const secretKey = 'storyboarderp';
  console.log(this.leadbank_gid);
  console.log(this.lead2campaign_gid);
  console.log(this.leadbankcontact_gid);
  const lspage1 = this.lspage;
  const lspage = AES.encrypt(lspage1, secretKey).toString();
  const leadbank_gid = AES.encrypt(this.leadbank_gid, secretKey).toString();
  const lead2campaign_gid = AES.encrypt(this.lead2campaign_gid, secretKey).toString();
  const leadbankcontact_gid = AES.encrypt(this.leadbankcontact_gid, secretKey).toString();
  this.router.navigate(['/einvoice/bobainvoiceadd',leadbank_gid,lead2campaign_gid,leadbankcontact_gid,lspage])
}

//view enquiry
viewenquiry(){
  const secretKey = 'storyboarderp';
  console.log(this.enquiry_gid );
  const lspage = this.lspage;
  const enquiry_gid = AES.encrypt(this.enquiry_gid , secretKey).toString();
  this.router.navigate(['/smr/SmrTrnSalesorderview', enquiry_gid,lspage]);
}


//view order
vieworder(params:any){
  const secretKey = 'storyboarderp';
  console.log(this.salesorder_gid );
  console.log(this.leadbank_gid);
  const lspage1 = this.lspage;
  const salesorder_gid = AES.encrypt(this.salesorder_gid , secretKey).toString();
  const leadbank_gid = AES.encrypt(this.leadbank_gid, secretKey).toString();
  const lspage = AES.encrypt(lspage1, secretKey).toString();
  this.router.navigate(['/smr/SmrTrnSalesorderview', salesorder_gid,leadbank_gid,lspage]);
}

//view invoice
viewinvoice(params: any){
  const secretKey = 'storyboarderp';
  console.log(params);
  const lspage = this.lspage;
  const invoice_gid = AES.encrypt(params, secretKey).toString();
  this.router.navigate(['/einvoice/Invoice-View', invoice_gid])
}

//view quotation
viewquotation(params:any){
  const secretKey = 'storyboarderp';
  const param = (params);
  const lspage1 = this.lspage;
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  const lspage = AES.encrypt(lspage1, secretKey).toString();
  this.router.navigate(['/smr/SrmTrnNewquotationview',encryptedParam,lspage]) 
}

//enquiry to quote
raisequotations(params:any){
  const secretKey = 'storyboarderp';
  const param = (params);
  const lspage1 = this.lspage;
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  const lspage = AES.encrypt(lspage1, secretKey).toString();
  this.router.navigate(['',encryptedParam,lspage]) 
}
//quote to order
raiseorders(params:any) {
  const secretKey = 'storyboarderp';
  console.log(params);
  const param = params;
  const lspage = this.lspage;
  const leadbank_gid = AES.encrypt(this.leadbank_gid, secretKey).toString();
  const lead2campaign_gid = AES.encrypt(this.lead2campaign_gid, secretKey).toString();
  this.router.navigate(['',param,leadbank_gid,lead2campaign_gid,lspage])
}

//order to sales Invoice
raisesalesinvoice(params:any) {
  const secretKey = 'storyboarderp';
  console.log(params);
  const param = params;
  const lspage = this.lspage;
  const leadbank_gid = AES.encrypt(this.leadbank_gid, secretKey).toString();
  const lead2campaign_gid = AES.encrypt(this.lead2campaign_gid, secretKey).toString();
  this.router.navigate(['/einvoice/Invoiceaccountingaddconfirm',param,leadbank_gid,lead2campaign_gid,lspage])
}

//invoice to payment
raisepayment(params:any){
  const secretKey = 'storyboarderp';
  console.log(params);
  const param = params;
  const lspage = this.lspage;
  const leadbank_gid = AES.encrypt(this.leadbank_gid, secretKey).toString();
  const lead2campaign_gid = AES.encrypt(this.lead2campaign_gid, secretKey).toString();
  this.router.navigate(['',param,leadbank_gid,lead2campaign_gid,lspage])
}

//Price Segment
openModalpricesegment(params:any)
{
  const secretKey = 'storyboarderp';
  const param = (params);
  const lspage1 = this.lspage;
  const lspage = AES.encrypt(lspage1, secretKey).toString();
  const leadbank_gid = AES.encrypt(this.leadbank_gid, secretKey).toString();
  const lead2campaign_gid = AES.encrypt(this.lead2campaign_gid, secretKey).toString();
  const leadbankcontact_gid = AES.encrypt(this.leadbankcontact_gid, secretKey).toString();
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  this.router.navigate(['/crm/CrmTrnCustomerProductPrice',encryptedParam,leadbank_gid,lead2campaign_gid,leadbankcontact_gid,lspage])
}
public onup(): void {
  let formData = new FormData();
  if (this.file != null && this.file != undefined) {
    formData.append("file", this.file, this.file.name);
  }
  else {
    this.ToastrService.warning('Kindly select atleast one file!')
  }
}

// public onupload(id:string): void {
//   this.attachments();
//   let formData = new FormData();
//   this.reactiveMessageForm.value.contact_id=id;
//   if (this.file != null && this.file != undefined) {
//     formData.append("file", this.file, this.file.name);
//     formData.append("file_type", this.filetype)
//     formData.append("contact_id", this.reactiveMessageForm.value.contact_id)
//     this.NgxSpinnerService.show();
//     var url = 'Whatsapp/waSendDocuments'
//     this.service.post(url, formData).subscribe((result: any) => {
//       this.NgxSpinnerService.hide();
//       if (result.status == false) {
//         window.scrollTo({
//           top: 0, // Code is used for scroll top after event done
//         });
//         this.ToastrService.warning(result.message)
//         //this.GetWhatsappMessageSummary();
//       }
//       else {
//         window.scrollTo({
//           top: 0, // Code is used for scroll top after event done
//         });
//         this.ToastrService.success(result.message)

//       }
//       this.GetWhatsappMessageSummary(this.leadgig);
//     });
//   }

//   else {
//     window.scrollTo({
//       top: 0, // Code is used for scroll top after event done
//     });
//     this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
//   }
// }


openmodaleditcustomer(parameter :any){
 //this.leadbank_gid =  parameter;
  this.Geteditcustomerdetails(parameter );
  //this.leadbank_name = parameter;
}
Geteditcustomerdetails(leadbank_gid: any) {
  var url = 'Leadbank360/GetEditContactdetails'
  let param = {
    leadbank_gid: leadbank_gid
  }
  this.service.getparams(url, param).subscribe((result: any) => {
    // this.responsedata=result;
    this.editcustomer_list = result.contactedit_list;
    console.log(this.editcustomer_list)
   // console.log(this.callresponse_list[0].branch_gid)
    this.reactiveForm.get("displayName")?.setValue(this.editcustomer_list[0].displayName);
    this.reactiveForm.get("email")?.setValue(this.editcustomer_list[0].email);
    this.reactiveForm.get("mobile")?.setValue(this.editcustomer_list[0].mobile1);
    this.reactiveForm.get("address1")?.setValue(this.editcustomer_list[0].address1);
    this.reactiveForm.get("leadbank_gid")?.setValue(this.editcustomer_list[0].leadbank_gid);
    console.log(this.reactiveForm.value);
  });
}
openModaladdtocustomer() {
  // this.parameterValue = parameter
 }

onaddtocustomer() {
  console.log(this.leadbank_gid);
  var url = 'Leadbank360/Addtocustomer'
  let param = {
    leadbank_gid: this.leadbank_gid
  }
  this.service.getparams(url, param).subscribe((result: any) => {
    if (result.Status == true) {
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });
       window.location.reload()
       this.NgxSpinnerService.hide();
      this.ToastrService.success(result.message)
    }
    else {
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });
       window.location.reload()
       this.NgxSpinnerService.hide();
      this.ToastrService.warning(result.message)
    }
  });
}

// get schedule_date() {
//   return this.reactiveFormfollow.get('schedule_date')!;
// }
// get schedule_time() {
//   return this.reactiveFormfollow.get('schedule_time')!;
// }
// get schedule_type() {
//   return this.reactiveFormfollow.get('schedule_type')!;
// }
// Getshedulesummary(leadbank_gid: any) {
//   var url = 'MyLead/GetSchedulelogsummary'
//   let param = {
//     leadbank_gid: leadbank_gid
//   }
//   this.service.getparams(url, param).subscribe((result: any) => {
//     // this.responsedata=result;
//     this.schedulesummary_list = result.schedulesummary_list;
//     console.log(this.schedulesummary_list)
//    // console.log(this.callresponse_list[0].branch_gid)

//   });
// }
// public schedule(): void {
//   if (this.reactiveFormfollow.value.schedule_date != null && this.reactiveFormfollow.value.schedule_time != null) {

//     for (const control of Object.keys(this.reactiveFormfollow.controls)) {
//       this.reactiveFormfollow.controls[control].markAsTouched();
//     }
//     this.reactiveFormfollow.value;
//     var url = 'MyLead/PostNewschedulelog'
//     this.service.post(url, this.reactiveFormfollow.value).subscribe((result: any) => {

//       console.log(this.reactiveFormfollow.value);

//       if (result.status == false) { 
//          window.scrollTo({

//         top: 0, // Code is used for scroll top after event done

//       });
//         this.ToastrService.warning(result.message)
//         this.ToastrService.warning("false")
       
       
//         this.reactiveFormfollow.reset();
//       }
//       else {  
//         window.scrollTo({

//         top: 0, // Code is used for scroll top after event done

//       });
//         this.reactiveFormfollow.get("schedule_date")?.setValue(null);
//         this.reactiveFormfollow.get("schedule_time")?.setValue(null);
//         this.ToastrService.success(result.message)
      
       
//         this.reactiveFormfollow.reset();
//       }
//       this.reactiveFormfollow.reset();
//     });

//   }
//   else {
//     this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
//   }

// }
onclose(){}
onsubmit(){}
onupload(param:any){}
onChange1(param:any){}

}
