import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DataTablesModule } from 'angular-datatables';
import { NgSelectModule } from '@ng-select/ng-select';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { CKEditorModule } from 'ng2-ckeditor';
import { EmsCrmRoutingModule } from './ems.crm-routing.module';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { EmsUtilitiesModule } from '../ems.utilities/ems.utilities.module';
import { NgApexchartsModule } from 'ng-apexcharts';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { CrmTrnSalesordersummaryComponent } from './component/crm-trn-salesordersummary/crm-trn-salesordersummary.component';
import { CrmTrnSalesorderaddComponent } from './component/crm-trn-salesorderadd/crm-trn-salesorderadd.component';
import { CrmMstSourcesummaryComponent } from './component/crm-mst-sourcesummary/crm-mst-sourcesummary.component';
import { CrmMstProductgroupsummaryComponent } from './component/crm-mst-productgroupsummary/crm-mst-productgroupsummary.component';
import { CrmMstCategoryindustrySummaryComponent } from './component/crm-mst-categoryindustry-summary/crm-mst-categoryindustry-summary.component';
import { CrmMstRegionsummaryComponent } from './component/crm-mst-regionsummary/crm-mst-regionsummary.component';
import { CrmSmmFacebookaccountComponent } from './component/crm-smm-facebookaccount/crm-smm-facebookaccount.component';
import { CrmSmmFacebookpageComponent } from './component/crm-smm-facebookpage/crm-smm-facebookpage.component';
import { CrmSmmTelegramaccountComponent } from './component/crm-smm-telegramaccount/crm-smm-telegramaccount.component';
import { CrmSmmLinkedaccountComponent } from './component/crm-smm-linkedaccount/crm-smm-linkedaccount.component';
import { CrmMstProductsummaryComponent } from './component/crm-mst-productsummary/crm-mst-productsummary.component';
import { CrmMstProductAddComponent } from './component/crm-mst-product-add/crm-mst-product-add.component';
import { CrmMstProductEditComponent } from './component/crm-mst-product-edit/crm-mst-product-edit.component';
import { CrmMstProductViewComponent } from './component/crm-mst-product-view/crm-mst-product-view.component';
import { CrmTrnLeadbanksummaryComponent } from './component/crm-trn-leadbanksummary/crm-trn-leadbanksummary.component';
import { CrmTrnLeadbankaddComponent } from './component/crm-trn-leadbankadd/crm-trn-leadbankadd.component';
import { CrmTrnLeadbankviewComponent } from './component/crm-trn-leadbankview/crm-trn-leadbankview.component';
import { CrmTrnLeadbankeditComponent } from './component/crm-trn-leadbankedit/crm-trn-leadbankedit.component';
import { CrmTrnTelemycampaignComponent } from './component/crm-trn-telemycampaign/crm-trn-telemycampaign.component';
import { CrmTrnMyvisitComponent } from './component/crm-trn-myvisit/crm-trn-myvisit.component';
import { CrmTrnMarketingmanagersummaryComponent } from './component/crm-trn-marketingmanagersummary/crm-trn-marketingmanagersummary.component';
import { CrmTrnCampaignmanagerComponent } from './component/crm-trn-campaignmanager/crm-trn-campaignmanager.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CrmCampaignMailmanagementComponent } from './component/crm-campaign-mailmanagement/crm-campaign-mailmanagement.component';
import { TimepickerModule } from 'ngx-bootstrap/timepicker';
import { CrmTrnLeadmastersummaryComponent } from './component/crm-trn-leadmastersummary/crm-trn-leadmastersummary.component';
import { CrmTrnLeadmasteraddComponent } from './component/crm-trn-leadmasteradd/crm-trn-leadmasteradd.component';
import { CrmTrnLeadbankbranchComponent } from './component/crm-trn-leadbankbranch/crm-trn-leadbankbranch.component';
import { CrmTrnLeadbankbrancheditComponent } from './component/crm-trn-leadbankbranchedit/crm-trn-leadbankbranchedit.component';
import { CrmTrnAssignvisitsummaryComponent } from './component/crm-trn-assignvisitsummary/crm-trn-assignvisitsummary.component';
import { CrmSmmInstagramComponent } from './component/crm-smm-instagram/crm-smm-instagram.component';
import { CrmMstCampaignsummaryComponent } from './component/crm-mst-campaignsummary/crm-mst-campaignsummary.component';
import { CrmTrnLeadbankcontactComponent } from './component/crm-trn-leadbankcontact/crm-trn-leadbankcontact.component';
import { CrmTrnLeadbankcontactEditComponent } from './component/crm-trn-leadbankcontact-edit/crm-trn-leadbankcontact-edit.component';
import { CrmTrnMycampaignComponent } from './component/crm-trn-mycampaign/crm-trn-mycampaign.component';
import { CrmTrnAddtocustomerComponent } from './component/crm-trn-addtocustomer/crm-trn-addtocustomer.component';
import { DualListComponent } from './component/crm-mst-campaignsummary/dual-list/dual-list.component';
import { ManagerListComponent } from './component/crm-mst-campaignsummary/manager-list/manager-list.component';
import { CrmDashboardComponent } from './component/crm-dashboard/crm-dashboard.component';
import { CrmCampaignMailmanagementsummaryComponent } from './component/crm-campaign-mailmanagementsummary/crm-campaign-mailmanagementsummary.component';
import { CrmTrnCampaignleadallocationComponent } from './component/crm-trn-campaignleadallocation/crm-trn-campaignleadallocation.component';
import { CrmSmmTelegramsummaryComponent } from './component/crm-smm-telegramsummary/crm-smm-telegramsummary.component';
import { CrmTrnUpcomingmeetingsComponent } from './component/crm-trn-upcomingmeetings/crm-trn-upcomingmeetings.component';
import { CrmTrnNewtaskComponent } from './component/crm-trn-newtask/crm-trn-newtask.component';
import { CrmTrnProspectsComponent } from './component/crm-trn-prospects/crm-trn-prospects.component';
import { CrmTrnPotentialsComponent } from './component/crm-trn-potentials/crm-trn-potentials.component';
import { CrmTrnCompletedComponent } from './component/crm-trn-completed/crm-trn-completed.component';
import { CrmTrnDropleadsComponent } from './component/crm-trn-dropleads/crm-trn-dropleads.component';
import { CrmTrnAllleadsComponent } from './component/crm-trn-allleads/crm-trn-allleads.component';
import { CrmSmmEmailmanagementComponent } from './component/crm-smm-emailmanagement/crm-smm-emailmanagement.component';
import { CrmMstCallresponseComponent } from './component/crm-mst-callresponse/crm-mst-callresponse.component';
import { CrmSmmWatsappComponent } from './component/crm-smm-watsapp/crm-smm-watsapp.component';
import { CrmSmmComposemailComponent } from './component/crm-smm-composemail/crm-smm-composemail.component';
import { CrmSmmShopifycustomerComponent } from './component/crm-smm-shopifycustomer/crm-smm-shopifycustomer.component';
import { CrmSmmWhatsappmessagetemplateComponent } from './component/crm-smm-whatsappmessagetemplate/crm-smm-whatsappmessagetemplate.component';
import { CrmTrnCorporateregisterleadComponent } from './component/crm-trn-corporateregisterlead/crm-trn-corporateregisterlead.component';
import { CrmTrnRetailerregisterleadComponent } from './component/crm-trn-retailerregisterlead/crm-trn-retailerregisterlead.component';
import { CrmTrnCorporateleadbankComponent } from './component/crm-trn-corporateleadbank/crm-trn-corporateleadbank.component';
import { CrmTrnRetailerleadbankComponent } from './component/crm-trn-retailerleadbank/crm-trn-retailerleadbank.component';
import { CrmSmmSendmailComponent } from './component/crm-smm-sendmail/crm-smm-sendmail.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { CrmSocailMediaDashboardComponent } from './component/crm-socail-media-dashboard/crm-socail-media-dashboard.component';
import { CrmSmmShopifycustomerassignedComponent } from './component/crm-smm-shopifycustomerassigned/crm-smm-shopifycustomerassigned.component';
import { CrmSmmShopifycustomerunassignedComponent } from './component/crm-smm-shopifycustomerunassigned/crm-smm-shopifycustomerunassigned.component';
import { CrmSmmMailcampaignsummaryComponent } from './component/crm-smm-mailcampaignsummary/crm-smm-mailcampaignsummary.component';
import { CrmSmmMailcampaigntemplateComponent } from './component/crm-smm-mailcampaigntemplate/crm-smm-mailcampaigntemplate.component';
import { CrmSmmMailcampaignsendComponent } from './component/crm-smm-mailcampaignsend/crm-smm-mailcampaignsend.component';
import { CrmSmmMailcampaigntemplateviewComponent } from './component/crm-smm-mailcampaigntemplateview/crm-smm-mailcampaigntemplateview.component';
import { CrmSmmMailcampaignsendstatusComponent } from './component/crm-smm-mailcampaignsendstatus/crm-smm-mailcampaignsendstatus.component';
import { CrmSmmWhatsappcampaignComponent } from './component/crm-smm-whatsappcampaign/crm-smm-whatsappcampaign.component';
import { CrmMstWabulkmessageComponent } from './component/crm-mst-wabulkmessage/crm-mst-wabulkmessage.component';
import { CrmSmmWhatsapplogComponent } from './component/crm-smm-whatsapplog/crm-smm-whatsapplog.component';
import { CrmTrnRetaileraddComponent } from './component/crm-trn-retaileradd/crm-trn-retaileradd.component';
import { CrmTrnRetailerviewComponent } from './component/crm-trn-retailerview/crm-trn-retailerview.component';
import { CrmTrnRetailereditComponent } from './component/crm-trn-retaileredit/crm-trn-retaileredit.component';
import { CrmTrnOverallviewComponent } from './component/crm-trn-overallview/crm-trn-overallview.component';
import { CrmTrnUpcomingmarketingComponent } from './component/crm-trn-upcomingmarketing/crm-trn-upcomingmarketing.component';
import { CrmTrnNewmarketingComponent } from './component/crm-trn-newmarketing/crm-trn-newmarketing.component';
import { CrmTrnProspectmarketingComponent } from './component/crm-trn-prospectmarketing/crm-trn-prospectmarketing.component';
import { CrmTrnPotentialmarketingComponent } from './component/crm-trn-potentialmarketing/crm-trn-potentialmarketing.component';
import { CrmTrnCustomermarketingComponent } from './component/crm-trn-customermarketing/crm-trn-customermarketing.component';
import { CrmTrnDropmarketingComponent } from './component/crm-trn-dropmarketing/crm-trn-dropmarketing.component';
import { CrmTrnLapsedleadmarketingComponent } from './component/crm-trn-lapsedleadmarketing/crm-trn-lapsedleadmarketing.component';
import { CrmTrnLongestleadmarketingComponent } from './component/crm-trn-longestleadmarketing/crm-trn-longestleadmarketing.component';
import { CrmSmmSmscampaignComponent } from './component/crm-smm-smscampaign/crm-smm-smscampaign.component';
import { CrmSmmSmscampaignsendComponent } from './component/crm-smm-smscampaignsend/crm-smm-smscampaignsend.component';
import { NgxIntlTelInputModule } from 'ngx-intl-tel-input';
import { CrmSmmCampaignsettingsComponent } from './component/crm-smm-campaignsettings/crm-smm-campaignsettings.component';
import { CrmSmmJustdialComponent } from './component/crm-smm-justdial/crm-smm-justdial.component';
import { CrmSmmIndiamartComponent } from './component/crm-smm-indiamart/crm-smm-indiamart.component';
import { CrmSmmTradeindiaComponent } from './component/crm-smm-tradeindia/crm-smm-tradeindia.component';
import { CrmSmmWebsiteComponent } from './component/crm-smm-website/crm-smm-website.component';
import { CrmSmmFlipkartComponent } from './component/crm-smm-flipkart/crm-smm-flipkart.component';
import { CrmSmmAmazonComponent } from './component/crm-smm-amazon/crm-smm-amazon.component';
import { CrmSmmMeeshoComponent } from './component/crm-smm-meesho/crm-smm-meesho.component';
import { CrmSmmFacebookpostviewComponent } from './component/crm-smm-facebookpostview/crm-smm-facebookpostview.component';
import { MatTabsModule } from '@angular/material/tabs';
import { CrmTrnMtdComponent } from './component/crm-trn-mtd/crm-trn-mtd.component';
import { CrmTrnYtdComponent } from './component/crm-trn-ytd/crm-trn-ytd.component';
import { CrmMstCampaignexpandComponent } from './component/crm-mst-campaignexpand/crm-mst-campaignexpand.component';
import { CrmTrnTraisequtationComponent } from './component/crm-trn-traisequtation/crm-trn-traisequtation.component';
import { CrmTrnTraiseorderComponent } from './component/crm-trn-traiseorder/crm-trn-traiseorder.component';
import { CrmTrnTrasieinvoiceComponent } from './component/crm-trn-trasieinvoice/crm-trn-trasieinvoice.component';
import { CrmTrnTcustomerraiseenquiryComponent } from './component/crm-trn-tcustomerraiseenquiry/crm-trn-tcustomerraiseenquiry.component';
import { CrmSmmShopifyproductaddComponent } from './component/crm-smm-shopifyproductadd/crm-smm-shopifyproductadd.component';
import { CrmSmmShopifyproducteditComponent } from './component/crm-smm-shopifyproductedit/crm-smm-shopifyproductedit.component';
import { CrmSmmWebsitechatsComponent } from './component/crm-smm-websitechats/crm-smm-websitechats.component';
import { CrmTrnCustomerProductPriceComponent } from './component/crm-trn-customer-product-price/crm-trn-customer-product-price.component';
import { CrmRptActivitylogreportComponent } from './component/crm-rpt-activitylogreport/crm-rpt-activitylogreport.component';
import { CrmRptEnquiryrptComponent } from './component/crm-rpt-enquiryrpt/crm-rpt-enquiryrpt.component';
import { CrmRptProductconsumptionreportComponent } from './component/crm-rpt-productconsumptionreport/crm-rpt-productconsumptionreport.component';
import { CrmRptProductgroupreportComponent } from './component/crm-rpt-productgroupreport/crm-rpt-productgroupreport.component';
import { CrmRptProductpreviouspricereportComponent } from './component/crm-rpt-productpreviouspricereport/crm-rpt-productpreviouspricereport.component';
@NgModule({
  declarations: [
    CrmTrnSalesordersummaryComponent,
    CrmTrnSalesorderaddComponent,
    CrmMstSourcesummaryComponent,
    CrmMstProductgroupsummaryComponent,
    CrmMstCategoryindustrySummaryComponent,
    CrmMstRegionsummaryComponent,
    CrmSmmFacebookaccountComponent,
    CrmSmmFacebookpageComponent,
    CrmSmmTelegramaccountComponent,
    CrmSmmLinkedaccountComponent,
    CrmMstProductsummaryComponent,
    CrmMstProductAddComponent,
    CrmMstProductEditComponent,
    CrmMstProductViewComponent,
    CrmTrnLeadbanksummaryComponent,
    CrmTrnLeadbankaddComponent,
    CrmTrnLeadbankviewComponent,
    CrmTrnLeadbankeditComponent,
    CrmTrnTelemycampaignComponent,
    CrmTrnMyvisitComponent,
    CrmTrnMarketingmanagersummaryComponent,
    CrmTrnCampaignmanagerComponent,
    CrmCampaignMailmanagementComponent,
    CrmTrnLeadmastersummaryComponent,
    CrmTrnLeadmasteraddComponent,
    CrmTrnLeadbankbranchComponent,
    CrmTrnLeadbankbrancheditComponent,
    CrmTrnAssignvisitsummaryComponent,
    CrmSmmInstagramComponent,
    CrmMstCampaignsummaryComponent,
    CrmTrnLeadbankcontactComponent,
    CrmTrnLeadbankcontactEditComponent,
    CrmTrnMycampaignComponent,
    CrmTrnAddtocustomerComponent,
    ManagerListComponent, DualListComponent,
    CrmDashboardComponent,
    CrmCampaignMailmanagementsummaryComponent,
    CrmTrnCampaignleadallocationComponent,
    CrmSmmTelegramsummaryComponent,
    CrmTrnUpcomingmeetingsComponent,
    CrmTrnNewtaskComponent,
    CrmTrnProspectsComponent,
    CrmTrnPotentialsComponent,
    CrmTrnCompletedComponent,
    CrmTrnDropleadsComponent,
    CrmTrnAllleadsComponent,
    CrmSmmEmailmanagementComponent,
    CrmMstCallresponseComponent,
    CrmSmmWatsappComponent,
    CrmSmmComposemailComponent,
    CrmSmmShopifycustomerComponent,
    CrmSmmWhatsappmessagetemplateComponent,
    CrmTrnCorporateregisterleadComponent,
    CrmTrnRetailerregisterleadComponent,
    CrmTrnCorporateleadbankComponent,
    CrmTrnRetailerleadbankComponent,
    CrmSmmEmailmanagementComponent,
    CrmSmmSendmailComponent,
    CrmSocailMediaDashboardComponent,
    CrmSmmShopifycustomerassignedComponent,
    CrmSmmShopifycustomerunassignedComponent,
    CrmSmmMailcampaignsummaryComponent,
    CrmSmmMailcampaigntemplateComponent,
    CrmSmmMailcampaignsendComponent,
    CrmSmmMailcampaigntemplateviewComponent,
    CrmSmmMailcampaignsendstatusComponent,
    CrmSmmWhatsappcampaignComponent,
    CrmMstWabulkmessageComponent,
    CrmSmmWhatsapplogComponent,
    CrmTrnRetaileraddComponent,
    CrmTrnRetailerviewComponent,
    CrmTrnRetailereditComponent,
    CrmTrnOverallviewComponent,
    CrmTrnUpcomingmarketingComponent,
    CrmTrnNewmarketingComponent,
    CrmTrnProspectmarketingComponent,
    CrmTrnLongestleadmarketingComponent,
    CrmTrnLapsedleadmarketingComponent,
    CrmTrnDropmarketingComponent,
    CrmTrnPotentialmarketingComponent,
    CrmTrnCustomermarketingComponent,
    CrmSmmSmscampaignComponent,
    CrmSmmSmscampaignsendComponent,
    CrmSmmCampaignsettingsComponent,
    CrmSmmJustdialComponent,
    CrmSmmIndiamartComponent,
    CrmSmmTradeindiaComponent,
    CrmSmmWebsiteComponent,
    CrmSmmFlipkartComponent,
    CrmSmmAmazonComponent,
    CrmSmmMeeshoComponent,
    CrmSmmFacebookpostviewComponent,
    CrmTrnMtdComponent,
    CrmTrnYtdComponent,
    CrmMstCampaignexpandComponent,
    CrmTrnTraisequtationComponent,
    CrmTrnTraiseorderComponent,
    CrmTrnTrasieinvoiceComponent,
    CrmTrnTcustomerraiseenquiryComponent,
    CrmSmmShopifyproductaddComponent,
    CrmSmmShopifyproducteditComponent,
    CrmSmmWebsitechatsComponent,
    CrmTrnCustomerProductPriceComponent,
    CrmRptActivitylogreportComponent,
    CrmRptEnquiryrptComponent,
    CrmRptProductconsumptionreportComponent,
    CrmRptProductgroupreportComponent,
    CrmRptProductpreviouspricereportComponent,

  ],
  imports: [
    CommonModule,
    EmsCrmRoutingModule, ReactiveFormsModule,
    DataTablesModule,
    NgSelectModule, EmsUtilitiesModule, NgApexchartsModule, AngularEditorModule, TabsModule.forRoot(), NgbModule, TimepickerModule.forRoot(), FormsModule,
    NgxPaginationModule, NgxIntlTelInputModule,
    MatTabsModule,CKEditorModule
  ]
})
export class EmsCrmModule { }
