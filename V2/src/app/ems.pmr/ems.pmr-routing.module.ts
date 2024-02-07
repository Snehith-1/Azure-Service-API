import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmsUtilitiesModule } from '../ems.utilities/ems.utilities.module';
import { PmrMstCurrencySummaryComponent } from './Component/pmr-mst-currency-summary/pmr-mst-currency-summary.component';
import { PmrTrnVendorregisterSummaryComponent } from './Component/pmr-trn-vendorregister-summary/pmr-trn-vendorregister-summary.component';
import { PmrMstProductunitComponent } from './Component/pmr-mst-productunit/pmr-mst-productunit.component';
import { PmrMstTaxSummaryComponent } from './Component/pmr-mst-tax-summary/pmr-mst-tax-summary.component';
import { PmrTrnVendorregisterAddComponent } from './Component/pmr-trn-vendorregister-add/pmr-trn-vendorregister-add.component';
import { PmrMstProductgroupComponent } from './Component/pmr-mst-productgroup/pmr-mst-productgroup.component';
import { PmrMstProductSummaryComponent } from './Component/pmr-mst-product-summary/pmr-mst-product-summary.component';
import { PmrMstProductAddComponent } from './Component/pmr-mst-product-add/pmr-mst-product-add.component';
import { PmrTrnDirectpoAddComponent } from './Component/pmr-trn-directpo-add/pmr-trn-directpo-add.component';
import { PmrTrnPurchaseorderSummaryComponent } from './Component/pmr-trn-purchaseorder-summary/pmr-trn-purchaseorder-summary.component';
import { PmrTrnGrninwardComponent } from './Component/pmr-trn-grninward/pmr-trn-grninward.component';
import { PmrTrnGrninwardAddComponent } from './Component/pmr-trn-grninward-add/pmr-trn-grninward-add.component';
import { PmrTrnGrninwardaddComponent } from './Component/pmr-trn-grninwardadd/pmr-trn-grninwardadd.component';
import { PmrMstTermsconditionssummaryComponent } from './Component/pmr-mst-termsconditionssummary/pmr-mst-termsconditionssummary.component';
import { PmrMstTermsconditionsaddComponent } from './Component/pmr-mst-termsconditionsadd/pmr-mst-termsconditionsadd.component';
import { PmrRptOutstandingamountreportSummaryComponent } from './Component/pmr-rpt-outstandingamountreport-summary/pmr-rpt-outstandingamountreport-summary.component';
import { PmrTrnVendorregisterEditComponent } from './Component/pmr-trn-vendorregister-edit/pmr-trn-vendorregister-edit.component';
import { PmrTrnVendorregisterViewComponent } from './Component/pmr-trn-vendorregister-view/pmr-trn-vendorregister-view.component';
import { PmrRptPurchaseorderdetailedreportComponent } from './Component/pmr-rpt-purchaseorderdetailedreport/pmr-rpt-purchaseorderdetailedreport.component';
import { PmrTrnPurchaseorderViewComponent } from './Component/pmr-trn-purchaseorder-view/pmr-trn-purchaseorder-view.component';
import { PmrMstProductViewComponent } from './Component/pmr-mst-product-view/pmr-mst-product-view.component';
import { PmrMstProductEditComponent } from './Component/pmr-mst-product-edit/pmr-mst-product-edit.component';
import { PmrMstVendorregisterdocumentComponent } from './Component/pmr-mst-vendorregisterdocument/pmr-mst-vendorregisterdocument.component';
import { PmrMstVendorAdditionalinformationComponent } from './Component/pmr-mst-vendor-additionalinformation/pmr-mst-vendor-additionalinformation.component';
import { PmrMstVendorregisterimportexcelComponent } from './Component/pmr-mst-vendorregisterimportexcel/pmr-mst-vendorregisterimportexcel.component';
import { PmrTrnGrninwardViewComponent } from './Component/pmr-trn-grninward-view/pmr-trn-grninward-view.component';
import { PmrTrnGrncheckerComponent } from './Component/pmr-trn-grnchecker/pmr-trn-grnchecker.component';
import { PmrTrnGrnqccheckerComponent } from './Component/pmr-trn-grnqcchecker/pmr-trn-grnqcchecker.component';
import { PmrTrnRaiseEnquiryComponent } from './Component/pmr-trn-raise-enquiry/pmr-trn-raise-enquiry.component';
import { PmrTrnRaiseEnquiryaddComponent } from './Component/pmr-trn-raise-enquiryadd/pmr-trn-raise-enquiryadd.component';
import { PmrTrnVendorenquiryViewComponent } from './Component/pmr-trn-vendorenquiry-view/pmr-trn-vendorenquiry-view.component';
import { PmrTrnPurchaseordermailComponent } from './Component/pmr-trn-purchaseordermail/pmr-trn-purchaseordermail.component';
import { PmrTrnPurchaseRequisitionComponent } from './Component/pmr-trn-purchase-requisition/pmr-trn-purchase-requisition.component';
import { PmrTrnRaiseRequisitionComponent } from './Component/pmr-trn-raise-requisition/pmr-trn-raise-requisition.component';
import { PmrTrnPurchaseQuotationComponent } from './Component/pmr-trn-purchase-quotation/pmr-trn-purchase-quotation.component';
import { PmrTrnPurchasequotaionSummaryComponent } from './Component/pmr-trn-purchasequotaion-summary/pmr-trn-purchasequotaion-summary.component';
import { PmrDashboardComponent } from './Component/pmr-dashboard/pmr-dashboard.component';
import { PmrTrnEnquiryAddSelectComponent } from './Component/pmr-trn-enquiry-add-select/pmr-trn-enquiry-add-select.component';
import { PmrTrnRequestForQuoteSummaryComponent } from './Component/pmr-trn-request-for-quote-summary/pmr-trn-request-for-quote-summary.component';
import { PmrTrnEnquiryaddProceedComponent } from './Component/pmr-trn-enquiryadd-proceed/pmr-trn-enquiryadd-proceed.component';
import { PmrRptVendorledgerreportComponent } from './Component/pmr-rpt-vendorledgerreport/pmr-rpt-vendorledgerreport.component';
import { PmrRptOverallreportComponent } from './Component/pmr-rpt-overallreport/pmr-rpt-overallreport.component';
import { PmrTrnPurchaserequisitionViewComponent } from './Component/pmr-trn-purchaserequisition-view/pmr-trn-purchaserequisition-view.component';
import { PmrTrnEnquiryAddConfirmComponent } from './Component/pmr-trn-enquiry-add-confirm/pmr-trn-enquiry-add-confirm.component';
import { PmrMstTermsandconditionEditComponent } from './Component/pmr-mst-termsandcondition-edit/pmr-mst-termsandcondition-edit.component';
import { PmrTrnRequestForQuoteViewComponent } from './Component/pmr-trn-request-for-quote-view/pmr-trn-request-for-quote-view.component';

const routes: Routes = [
  { path: 'PmrMstTaxSummary', component: PmrMstTaxSummaryComponent },
  { path: 'PmrMstProductSummary', component: PmrMstProductSummaryComponent },
  { path: 'PmrMstProductAdd', component: PmrMstProductAddComponent },
  { path: 'PmrMstProductunit', component: PmrMstProductunitComponent },
  { path: 'PmrMstCurrencySummary', component: PmrMstCurrencySummaryComponent },
  { path: 'PmrMstProductGroup', component: PmrMstProductgroupComponent },
  { path: 'PmrMstVendorregister', component: PmrTrnVendorregisterSummaryComponent },
  { path: 'PmrTrnVendorregisterView/:vendorregister_gid', component: PmrTrnVendorregisterViewComponent },
  { path: 'PmrMstVendorAdditionalinformation/:vendorregister_gid', component: PmrMstVendorAdditionalinformationComponent },
  { path: 'PmrTrnVendorregisterAdd', component: PmrTrnVendorregisterAddComponent },
  { path: 'PmrTrnVendorregisterEdit/:vendorregister_gid', component: PmrTrnVendorregisterEditComponent },
  { path: 'PmrTrnPurchaseorderSummary', component: PmrTrnPurchaseorderSummaryComponent },
  { path: 'PmrTrnDirectpoAdd', component: PmrTrnDirectpoAddComponent },
  { path: 'PmrTrnGrninward', component: PmrTrnGrninwardComponent },
  { path: 'PmrTrnGrninwardadd', component: PmrTrnGrninwardAddComponent },
  { path: 'PmrTrnGrninwardsubmit/:purchaseorder_gid', component: PmrTrnGrninwardaddComponent },
  { path: 'PmrMstTermsconditionsadd', component: PmrMstTermsconditionsaddComponent },
  { path: 'PmrMstTermsconditionssummary', component: PmrMstTermsconditionssummaryComponent },
  { path: 'PmrRptOutstandingamountreport', component: PmrRptOutstandingamountreportSummaryComponent },
  { path: 'PmrRptPurchaseorderdetailedreport', component: PmrRptPurchaseorderdetailedreportComponent },
  { path: 'PmrTrnPurchaseOrderView/:purchaseorder_gid', component: PmrTrnPurchaseorderViewComponent },
  { path: 'PmrMstProductView/:product_gid', component: PmrMstProductViewComponent },
  { path: 'PmrMstProductEdit/:product_gid', component: PmrMstProductEditComponent },
  { path: 'PmrMstVendorRegisterDocument/:vendorregister_gid', component: PmrMstVendorregisterdocumentComponent },
  { path: 'PmrTrnGrninwardView/:grn_gid', component: PmrTrnGrninwardViewComponent },
  { path: 'PmrMstVendorRegisterImportExcel', component: PmrMstVendorregisterimportexcelComponent },
  { path: 'PmrTrnGrncheckerSummary', component: PmrTrnGrncheckerComponent },
  { path: 'PmrTrnGrnqcchecker/:grn_gid', component: PmrTrnGrnqccheckerComponent },
  { path: 'PmrTrnRaiseEnquiry', component: PmrTrnRaiseEnquiryComponent },
  { path: 'PmrTrnRaiseEnquiryadd', component: PmrTrnRaiseEnquiryaddComponent },
  { path: 'PmrTrnVendorenquiryView/:enquiry_gid', component: PmrTrnVendorenquiryViewComponent },
  { path: 'PmrTrnPurchaseordermail/:purchaseorder_gid', component: PmrTrnPurchaseordermailComponent },
  { path: 'PmrTrnPurchaseRequisition', component: PmrTrnPurchaseRequisitionComponent},
  { path: 'PmrTrnRaiseRequisition', component: PmrTrnRaiseRequisitionComponent},
  { path: 'PmrTrnPurchaseQuotation', component: PmrTrnPurchaseQuotationComponent },
  { path: 'PmrTrnPurchasequotaionSummary', component: PmrTrnPurchasequotaionSummaryComponent },
  { path: 'PmrDashboard', component: PmrDashboardComponent },
  { path: 'PmrTrnRequestForQuoteSummary', component: PmrTrnRequestForQuoteSummaryComponent},
  { path: 'PmrTrnEnquiryAddSelect', component: PmrTrnEnquiryAddSelectComponent},
  { path: 'PmrTrnEnquiryaddProceed/:purchaserequisition_gid', component: PmrTrnEnquiryaddProceedComponent },
  { path: 'PmrTrnEnquiryAddConfirm/:purchaserequisition_gid', component: PmrTrnEnquiryAddConfirmComponent },
  { path: 'PmrRptVendorLedgerreport', component: PmrRptVendorledgerreportComponent },
  { path: 'PmrRptOverallreport', component:PmrRptOverallreportComponent},
  { path: 'PmrTrnPurchaseRequisitionView/:purchaserequisition_gid', component: PmrTrnPurchaserequisitionViewComponent},
  { path: 'PmrMstTermsandconditionEdit/:termsconditions_gid', component: PmrMstTermsandconditionEditComponent},
  { path: 'PmrTrnRequestForQuoteView/:enquiry_gid', component: PmrTrnRequestForQuoteViewComponent},


];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmsPmrRoutingModule { } 
