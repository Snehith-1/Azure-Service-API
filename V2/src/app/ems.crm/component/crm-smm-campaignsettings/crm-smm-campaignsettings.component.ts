import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
interface ICampaingService {
  workspace_id: string;
  whatsapp_accesstoken: string;
  channel_id: string;
  mobile_number: string;
  channel_name: string;
  access_token_edit: string;
  base_url_edit: string;
  whatsapp_id: string;
  channelgroup_id: string
}
interface IShopifyService {
  shopify_accesstoken: string;
  shopify_store_name: string;
  store_month_year: string;
  shopify_accesstokenedit: string;
  shopify_store_nameedit: string;
  store_month_yearedit: string;
}
interface IEmailService {
  mail_access_token: string;
  mail_base_url: string;
  email_id: string;
  receiving_domain: string;
  sending_domain: string;
  email_username:string;

}
interface IFacebookService {
  facebook_access_token: string;
  facebook_page_id: string;
  facebook_id: string;

}
interface IInstagramService {
  instagram_access_token: string;
  instagram_id: string;


}
interface ILinkedinService {
  linkedin_access_token: string;
  linkedin_id: string;

}
interface ITelegramService {
  bot_id: string;
  chat_id: string;
  telegram_id: string;

}
@Component({
  selector: 'app-crm-smm-campaignsettings',
  templateUrl: './crm-smm-campaignsettings.component.html',
  styleUrls: ['./crm-smm-campaignsettings.component.scss']
})
export class CrmSmmCampaignsettingsComponent {
  services: any;
  parameterValue: any;
  leadbank_list: any[] = [];
  parameterValue1: any;
  reactiveFormFacebook!: FormGroup;
  reactiveFormInstagram!: FormGroup;
  reactiveFormShopify!: FormGroup;
  reactiveForm: any;
  reactiveFormEmail!: FormGroup;
  reactiveFormWhatsapp!: FormGroup;
  reactiveFormLinkedin!: FormGroup;
  reactiveFormTelegram!: FormGroup;
  campaignserv_list: any[] = [];
  campaignser_list: any[] = [];
  facebookcampaignservicelist: any[] = [];
  instagramcampaignservicelist: any[] = [];

  campaignservice_list: any[] = [];
  shopifycampaignservice_list:any[]=[];
  mailtemplateview_list: any;
  access_token: any;
  EmailService!:IEmailService;
  responsedata: any;
  CampaingService!: ICampaingService;
  ShopifyService!:IShopifyService;
  FacebookService!:IFacebookService;
  InstagramService!:IInstagramService;
  LinkedinService!:ILinkedinService;
  TelegramService!:ITelegramService;
  linkedincampaignservicelist: any[] = [];
  telegramcampaignservicelist: any[] = [];
  datas: any;
  constructor(private formBuilder: FormBuilder, private NgxSpinnerService: NgxSpinnerService, private ToastrService: ToastrService, public service: SocketService) {
    this.CampaingService = {} as ICampaingService;
    this.ShopifyService = {} as IShopifyService;
    this.EmailService = {} as IEmailService;
    this.FacebookService = {} as IFacebookService;
    this.LinkedinService = {} as ILinkedinService;
    this.TelegramService = {} as ITelegramService;
    this.InstagramService ={} as IInstagramService;

  }

  ngOnInit(): void {
    // Form values for Add popup/////
    this.GetWhatsappSummary();
    this.GetShopifySummary();
    this.GetMailSummary();
    this.GetFacebookServiceSummary();
    this.GetInstagramServiceSummary();
    this.GetLinkedinServiceSummary();
    this.GetTelegramServiceSummary();
    this.reactiveFormShopify = new FormGroup({
      shopify_accesstoken: new FormControl(this.ShopifyService.shopify_accesstoken, [
        Validators.required,
      ]),
      shopify_store_name: new FormControl(this.ShopifyService.shopify_store_name, [
        Validators.required,
      ]),
      store_month_year: new FormControl(this.ShopifyService.store_month_year, [
        Validators.required,
      ]),
      shopify_id: new FormControl()
    });
    this.reactiveFormWhatsapp = new FormGroup({

      whatsapp_accesstoken: new FormControl(this.CampaingService.whatsapp_accesstoken, [

        Validators.required

      ]),

      workspace_id: new FormControl(this.CampaingService.workspace_id, [
        Validators.required
      ]),
      channel_id: new FormControl(this.CampaingService.channel_id, [

        Validators.required
      ]),
      channelgroup_id: new FormControl(this.CampaingService.channelgroup_id, [

        Validators.required
      ]),
      mobile_number: new FormControl(this.CampaingService.mobile_number, [
        Validators.required
      ]),
      channel_name: new FormControl(this.CampaingService.channel_name, [

        Validators.required
      ]),
      whatsapp_id: new FormControl()

    });
    this.reactiveFormEmail = new FormGroup({
      mail_access_token: new FormControl(this.EmailService.mail_access_token, [
        Validators.required,
      ]),
      mail_base_url: new FormControl(this.EmailService.mail_base_url, [
        Validators.required,
      ]),
      email_id: new FormControl(),
      receiving_domain: new FormControl(this.EmailService.receiving_domain, [
        Validators.required,
      ]),
      sending_domain: new FormControl(this.EmailService.sending_domain, [
        Validators.required,
      ]),
      email_username: new FormControl(this.EmailService.email_username, [
        Validators.required,
      ]),
    });
    this.reactiveFormFacebook = new FormGroup({
      facebook_access_token: new FormControl(this.FacebookService.facebook_access_token, [
        Validators.required,
      ]),
      facebook_page_id: new FormControl(this.FacebookService.facebook_page_id, [
        Validators.required,
      ]),
      facebook_id: new FormControl()
    });
    this.reactiveFormInstagram = new FormGroup({
      instagram_access_token: new FormControl(this.InstagramService.instagram_access_token, [
        Validators.required,
      ]),
     
      instagram_id: new FormControl()
    });
    this.reactiveFormLinkedin = new FormGroup({
      linkedin_access_token: new FormControl(this.LinkedinService.linkedin_access_token, [
        Validators.required,
      ]),
     
      linkedin_id: new FormControl()
    });
    this.reactiveFormTelegram = new FormGroup({
      bot_id: new FormControl(this.TelegramService.bot_id, [
        Validators.required,
      ]),
      chat_id: new FormControl(this.TelegramService.chat_id, [
        Validators.required,
      ]),
      telegram_id: new FormControl()
    });
  }

  onemailupdate() {
   
    if (this.reactiveFormEmail.status === 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'CampaignService/UpdateEmailService'
      this.service.post(url, this.reactiveFormEmail.value).subscribe((result: any) => {

        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.GetMailSummary();
        }
        else {
         
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.GetMailSummary();

        }

      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
    
  }
  onfacebookupdate() {
  
    if (this.reactiveFormFacebook.status === 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'CampaignService/UpdateFacebookService'
      this.service.post(url, this.reactiveFormFacebook.value).subscribe((result: any) => {

        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.GetFacebookServiceSummary();
        }
        else {
         
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.GetFacebookServiceSummary();

        }

      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
    
  }
  onlinkedinupdate() {
  
    if (this.reactiveFormLinkedin.status === 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'CampaignService/UpdateLinkedinService'
      this.service.post(url, this.reactiveFormLinkedin.value).subscribe((result: any) => {

        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.GetLinkedinServiceSummary();
        }
        else {
         
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.GetLinkedinServiceSummary();

        }

      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
    
  }
  ontelegramupdate() {
  
    if (this.reactiveFormTelegram.status === 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'CampaignService/UpdateTelegramService'
      this.service.post(url, this.reactiveFormTelegram.value).subscribe((result: any) => {

        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.GetTelegramServiceSummary();
        }
        else {
         
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.GetTelegramServiceSummary();

        }

      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
    
  }
  get bot_id() {
    return this.reactiveFormTelegram.get('bot_id')!;
  }
  get linkedin_access_token() {
    return this.reactiveFormLinkedin.get('linkedin_access_token')!;
  }
  get chat_id() {
    return this.reactiveFormTelegram.get('chat_id')!;
  }
  get facebook_access_token() {
    return this.reactiveFormFacebook.get('facebook_access_token')!;
  }
  get facebook_page_id() {
    return this.reactiveFormFacebook.get('facebook_page_id')!;
  }
  get instagram_access_token() {
    return this.reactiveFormInstagram.get('instagram_access_token')!;
  }
  get shopify_accesstoken() {
    return this.reactiveFormShopify.get('shopify_accesstoken')!;
  }
  get shopify_store_name() {
    return this.reactiveFormShopify.get('shopify_store_name')!;
  }
  get store_month_year() {
    return this.reactiveFormShopify.get('store_month_year')!;
  }
  get workspace_id() {
    return this.reactiveFormWhatsapp.get('workspace_id')!;
  }
 get whatsapp_accesstoken() {
    return this.reactiveFormWhatsapp.get('whatsapp_accesstoken')!;
  }
  get mobile_number() {
    return this.reactiveFormWhatsapp.get('mobile_number')!;
  }
  get channelgroup_id() {
    return this.reactiveFormWhatsapp.get('channelgroup_id')!;
  }
  get channel_id() {
    return this.reactiveFormWhatsapp.get('channel_id')!;
  }
  get channel_name() {
    return this.reactiveFormWhatsapp.get('channel_name')!;
  }
  get mail_access_token() {
    return this.reactiveFormEmail.get('mail_access_token')!;
  }
 get mail_base_url() {
    return this.reactiveFormEmail.get('mail_base_url')!;
  }
  get receiving_domain() {
    return this.reactiveFormEmail.get('receiving_domain')!;
  }get sending_domain() {
    return this.reactiveFormEmail.get('sending_domain')!;
  }
  get email_username() {
    return this.reactiveFormEmail.get('email_username')!;
  }
  onshopifyupdate(){
   
    if (this.reactiveFormShopify.status === 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'CampaignService/UpdateShopifyService'
      this.service.post(url, this.reactiveFormShopify.value).subscribe((result: any) => {

        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.GetShopifySummary();
        }
        else {
         
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.GetShopifySummary();

        }

      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }


  
  }
  oninstagramupdate() {
  
    if (this.reactiveFormInstagram.status === 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'CampaignService/UpdateInstagramService'
      this.service.post(url, this.reactiveFormInstagram.value).subscribe((result: any) => {

        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.GetInstagramServiceSummary();
        }
        else {
         
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.GetInstagramServiceSummary();

        }

      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
    
  }
  onwhatsappupdate(){ 
    if (this.reactiveFormWhatsapp.status === 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'CampaignService/UpdateWhatsappService'
      this.service.post(url, this.reactiveFormWhatsapp.value).subscribe((result: any) => {

        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.GetWhatsappSummary();
        }
        else {
         
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.GetWhatsappSummary();

        }

      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }
  GetWhatsappSummary() {
    var api = 'CampaignService/GetWhatsappSummary'
    this.service.get(api).subscribe((result: any) => {

      this.responsedata = result;
      this.campaignservice_list = this.responsedata.campaignservice_list;
      
      this.reactiveFormWhatsapp.get("whatsapp_accesstoken")?.setValue(this.campaignservice_list[0].access_token);
      this.reactiveFormWhatsapp.get("workspace_id")?.setValue(this.campaignservice_list[0].workspace_id);
      this.reactiveFormWhatsapp.get("channel_id")?.setValue(this.campaignservice_list[0].channel_id);
      this.reactiveFormWhatsapp.get("whatsapp_id")?.setValue(this.campaignservice_list[0].s_no);
      this.reactiveFormWhatsapp.get("mobile_number")?.setValue(this.campaignservice_list[0].mobile_number);
      this.reactiveFormWhatsapp.get("channel_name")?.setValue(this.campaignservice_list[0].channel_name);
   
    });
  }

  GetShopifySummary() {
    var api = 'CampaignService/GetShopifySummary'
    this.service.get(api).subscribe((result: any) => {

      this.responsedata = result;
      this.campaignserv_list = this.responsedata.shopifycampaignservice_list;
      this.reactiveFormShopify.get("shopify_store_name")?.setValue(this.campaignserv_list[0].shopify_store_name);
      this.reactiveFormShopify.get("store_month_year")?.setValue(this.campaignserv_list[0].store_month_year);
      this.reactiveFormShopify.get("shopify_accesstoken")?.setValue(this.campaignserv_list[0].shopify_access_token);
      this.reactiveFormShopify.get("shopify_id")?.setValue(this.campaignserv_list[0].s_no);
       //console.log(this.campaignserv_list)
    });
  }

  GetMailSummary() {
    var api = 'CampaignService/GetMailSummary'
    this.service.get(api).subscribe((result: any) => {

      this.responsedata = result;
      this.campaignser_list = this.responsedata.mailcampaignservice_list;
      this.reactiveFormEmail.get("mail_access_token")?.setValue(this.campaignser_list[0].mail_access_token);
      this.reactiveFormEmail.get("mail_base_url")?.setValue(this.campaignser_list[0].mail_base_url);
      this.reactiveFormEmail.get("email_id")?.setValue(this.campaignser_list[0].s_no);
      this.reactiveFormEmail.get("receiving_domain")?.setValue(this.campaignser_list[0].receiving_domain);
      this.reactiveFormEmail.get("sending_domain")?.setValue(this.campaignser_list[0].sending_domain);
      this.reactiveFormEmail.get("email_username")?.setValue(this.campaignser_list[0].email_username);
    });
  }

  GetFacebookServiceSummary() {
    var api = 'CampaignService/GetFacebookServiceSummary'
    this.service.get(api).subscribe((result: any) => {

      this.responsedata = result;
      this.facebookcampaignservicelist = this.responsedata.facebookcampaignservice_list;
     this.reactiveFormFacebook.get("facebook_access_token")?.setValue(this.facebookcampaignservicelist[0].facebook_access_token);
     this.reactiveFormFacebook.get("facebook_page_id")?.setValue(this.facebookcampaignservicelist[0].facebook_page_id);

     this.reactiveFormFacebook.get("facebook_id")?.setValue(this.facebookcampaignservicelist[0].facebook_id);
    });
  }
  GetInstagramServiceSummary() {
    var api = 'CampaignService/GetInstagramServiceSummary'
    this.service.get(api).subscribe((result: any) => {

      this.responsedata = result;
      this.instagramcampaignservicelist = this.responsedata.instagramcampaignservice_list;
     this.reactiveFormInstagram.get("instagram_access_token")?.setValue(this.instagramcampaignservicelist[0].instagram_access_token);
    });
  }
  GetLinkedinServiceSummary() {
    var api = 'CampaignService/GetLinkedinServiceSummary'
    this.service.get(api).subscribe((result: any) => {

      this.responsedata = result;
      this.linkedincampaignservicelist = this.responsedata.linkedincampaignservice_list;
     this.reactiveFormLinkedin.get("linkedin_access_token")?.setValue(this.linkedincampaignservicelist[0].linkedin_access_token);
     this.reactiveFormLinkedin.get("linkedin_id")?.setValue(this.linkedincampaignservicelist[0].linkedin_id);
    });
  }
  GetTelegramServiceSummary() {
    var api = 'CampaignService/GetTelegramServiceSummary'
    this.service.get(api).subscribe((result: any) => {

      this.responsedata = result;
      this.telegramcampaignservicelist = this.responsedata.telegramcampaignservice_list;
     this.reactiveFormTelegram.get("bot_id")?.setValue(this.telegramcampaignservicelist[0].bot_id);
     this.reactiveFormTelegram.get("chat_id")?.setValue(this.telegramcampaignservicelist[0].chat_id);
     this.reactiveFormTelegram.get("telegram_id")?.setValue(this.telegramcampaignservicelist[0].telegram_id);
    
    });
  }
}
