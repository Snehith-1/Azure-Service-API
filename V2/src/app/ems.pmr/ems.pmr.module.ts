import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DataTablesModule } from 'angular-datatables';
import { EmsPmrRoutingModule } from './ems.pmr-routing.module';
import { NgApexchartsModule } from 'ng-apexcharts';
import { PmrTrnVendorregisterSummaryComponent } from './Component/pmr-trn-vendorregister-summary/pmr-trn-vendorregister-summary.component';
import { PmrMstProductunitComponent } from './Component/pmr-mst-productunit/pmr-mst-productunit.component';
import { PmrMstCurrencySummaryComponent } from './Component/pmr-mst-currency-summary/pmr-mst-currency-summary.component';
import { PmrMstProductgroupComponent } from './Component/pmr-mst-productgroup/pmr-mst-productgroup.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PmrTrnVendorregisterAddComponent } from './Component/pmr-trn-vendorregister-add/pmr-trn-vendorregister-add.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { PmrMstProductAddComponent } from './Component/pmr-mst-product-add/pmr-mst-product-add.component';
import { PmrMstProductSummaryComponent } from './Component/pmr-mst-product-summary/pmr-mst-product-summary.component';
import { PmrMstTaxSummaryComponent } from './Component/pmr-mst-tax-summary/pmr-mst-tax-summary.component';
import { EmsUtilitiesModule } from '../ems.utilities/ems.utilities.module';
import { PmrTrnGrninwardAddComponent } from './Component/pmr-trn-grninward-add/pmr-trn-grninward-add.component';
import { PmrTrnGrninwardaddComponent } from './Component/pmr-trn-grninwardadd/pmr-trn-grninwardadd.component';
import { PmrTrnGrninwardComponent } from './Component/pmr-trn-grninward/pmr-trn-grninward.component';
import { PmrTrnPurchaseorderSummaryComponent } from './Component/pmr-trn-purchaseorder-summary/pmr-trn-purchaseorder-summary.component';
import { PmrMstTermsconditionssummaryComponent } from './Component/pmr-mst-termsconditionssummary/pmr-mst-termsconditionssummary.component';
import { PmrMstTermsconditionsaddComponent } from './Component/pmr-mst-termsconditionsadd/pmr-mst-termsconditionsadd.component';
import { PmrTrnVendorregisterEditComponent } from './Component/pmr-trn-vendorregister-edit/pmr-trn-vendorregister-edit.component';
import { PmrTrnVendorregisterViewComponent } from './Component/pmr-trn-vendorregister-view/pmr-trn-vendorregister-view.component';
import { PmrTrnPurchaseorderViewComponent } from './Component/pmr-trn-purchaseorder-view/pmr-trn-purchaseorder-view.component';
import { PmrRptPurchaseorderdetailedreportComponent } from './Component/pmr-rpt-purchaseorderdetailedreport/pmr-rpt-purchaseorderdetailedreport.component';
import { PmrMstProductViewComponent } from './Component/pmr-mst-product-view/pmr-mst-product-view.component';
import { PmrMstProductEditComponent } from './Component/pmr-mst-product-edit/pmr-mst-product-edit.component';
import { PmrMstVendorAdditionalinformationComponent } from './Component/pmr-mst-vendor-additionalinformation/pmr-mst-vendor-additionalinformation.component';
import { PmrMstVendorregisterdocumentComponent } from './Component/pmr-mst-vendorregisterdocument/pmr-mst-vendorregisterdocument.component';
import { PmrMstVendorregisterimportexcelComponent } from './Component/pmr-mst-vendorregisterimportexcel/pmr-mst-vendorregisterimportexcel.component';
import { PmrTrnGrninwardViewComponent } from './Component/pmr-trn-grninward-view/pmr-trn-grninward-view.component';
import { PmrTrnGrncheckerComponent } from './Component/pmr-trn-grnchecker/pmr-trn-grnchecker.component';
import { PmrTrnGrnqccheckerComponent } from './Component/pmr-trn-grnqcchecker/pmr-trn-grnqcchecker.component';
import { PmrTrnDirectpoAddComponent } from './Component/pmr-trn-directpo-add/pmr-trn-directpo-add.component';
import { PmrTrnRaiseEnquiryComponent } from './Component/pmr-trn-raise-enquiry/pmr-trn-raise-enquiry.component';
import { PmrTrnRaiseEnquiryaddComponent } from './Component/pmr-trn-raise-enquiryadd/pmr-trn-raise-enquiryadd.component';
import { PmrTrnVendorenquiryViewComponent } from './Component/pmr-trn-vendorenquiry-view/pmr-trn-vendorenquiry-view.component';
import { PmrTrnPurchaseordermailComponent } from './Component/pmr-trn-purchaseordermail/pmr-trn-purchaseordermail.component';
import { PmrTrnPurchaseRequisitionComponent } from './Component/pmr-trn-purchase-requisition/pmr-trn-purchase-requisition.component';
import { PmrTrnRaiseRequisitionComponent } from './Component/pmr-trn-raise-requisition/pmr-trn-raise-requisition.component';
import { PmrTrnPurchaseQuotationComponent } from './Component/pmr-trn-purchase-quotation/pmr-trn-purchase-quotation.component';
import { PmrTrnPurchasequotaionSummaryComponent } from './Component/pmr-trn-purchasequotaion-summary/pmr-trn-purchasequotaion-summary.component';
import { PmrDashboardComponent } from './Component/pmr-dashboard/pmr-dashboard.component';
import { NgxIntlTelInputModule } from 'ngx-intl-tel-input';
import { PmrTrnEnquiryAddSelectComponent } from './Component/pmr-trn-enquiry-add-select/pmr-trn-enquiry-add-select.component';
import { PmrTrnRequestForQuoteSummaryComponent } from './Component/pmr-trn-request-for-quote-summary/pmr-trn-request-for-quote-summary.component';
import { PmrTrnEnquiryaddProceedComponent } from './Component/pmr-trn-enquiryadd-proceed/pmr-trn-enquiryadd-proceed.component';
import { PmrRptVendorledgerReportComponent } from './Component/pmr-rpt-vendorledger-report/pmr-rpt-vendorledger-report.component';
import { PmrRptVendorledgerreportComponent } from './Component/pmr-rpt-vendorledgerreport/pmr-rpt-vendorledgerreport.component';
import { PmrRptOverallreportComponent } from './Component/pmr-rpt-overallreport/pmr-rpt-overallreport.component';
import { PmrTrnPurchaserequisitionViewComponent } from './Component/pmr-trn-purchaserequisition-view/pmr-trn-purchaserequisition-view.component';
import { PmrTrnEnquiryAddConfirmComponent } from './Component/pmr-trn-enquiry-add-confirm/pmr-trn-enquiry-add-confirm.component';
import { PmrMstTermsandconditionEditComponent } from './Component/pmr-mst-termsandcondition-edit/pmr-mst-termsandcondition-edit.component';
import { PmrTrnRequestForQuoteViewComponent } from './Component/pmr-trn-request-for-quote-view/pmr-trn-request-for-quote-view.component';

@NgModule({
  declarations: [
    PmrMstTaxSummaryComponent,
    PmrMstCurrencySummaryComponent,
    PmrMstProductSummaryComponent,
    PmrMstProductAddComponent,
    PmrMstProductgroupComponent,
    PmrMstProductunitComponent,
    PmrTrnVendorregisterSummaryComponent,
    PmrTrnVendorregisterAddComponent,
    PmrTrnGrninwardAddComponent,
    PmrTrnGrninwardaddComponent,
    PmrTrnGrninwardComponent,
    PmrTrnPurchaseorderSummaryComponent,
    PmrTrnPurchaseordermailComponent,
    PmrMstTermsconditionssummaryComponent,
    PmrMstTermsconditionsaddComponent,
    PmrTrnVendorregisterEditComponent,
    PmrTrnVendorregisterViewComponent,
    PmrRptPurchaseorderdetailedreportComponent,
    PmrTrnPurchaseorderViewComponent,
    PmrMstProductViewComponent,
    PmrMstProductEditComponent,
    PmrMstVendorAdditionalinformationComponent,
    PmrMstVendorregisterdocumentComponent,
    PmrMstVendorregisterimportexcelComponent,
    PmrTrnGrninwardViewComponent,
    PmrTrnGrncheckerComponent,
    PmrTrnGrnqccheckerComponent,
    PmrTrnDirectpoAddComponent,
    PmrTrnRaiseEnquiryComponent,
    PmrTrnRaiseEnquiryaddComponent,
    PmrTrnVendorenquiryViewComponent,
    PmrTrnPurchaseRequisitionComponent,
    PmrTrnRaiseRequisitionComponent,
    PmrTrnPurchaseQuotationComponent,
    PmrTrnPurchasequotaionSummaryComponent,PmrDashboardComponent,
    PmrTrnEnquiryAddSelectComponent,
    PmrTrnRequestForQuoteSummaryComponent,
    PmrTrnEnquiryaddProceedComponent,
    PmrRptVendorledgerReportComponent,
    PmrRptVendorledgerreportComponent,
    PmrRptOverallreportComponent,
    PmrTrnPurchaserequisitionViewComponent,
    PmrTrnEnquiryAddConfirmComponent,
    PmrMstTermsandconditionEditComponent,
     PmrTrnRequestForQuoteViewComponent
       
  ],

  imports: [
     CommonModule,
     EmsPmrRoutingModule,
     FormsModule, ReactiveFormsModule,EmsUtilitiesModule,
     FormsModule,NgApexchartsModule,ReactiveFormsModule,DataTablesModule,
     NgSelectModule,AngularEditorModule,
     NgxIntlTelInputModule
   ]
})
export class EmsPmrModule { }