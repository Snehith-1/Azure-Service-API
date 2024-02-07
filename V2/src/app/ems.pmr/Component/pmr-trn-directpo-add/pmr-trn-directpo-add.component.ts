import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { NgxSpinnerService } from 'ngx-spinner';


@Component({
  selector: 'app-pmr-trn-directpo-add',
  templateUrl: './pmr-trn-directpo-add.component.html',
  styleUrls: ['./pmr-trn-directpo-add.component.scss']
})

export class PmrTrnDirectpoAddComponent {
  currency_code1: any
  showInput: boolean = false;
  inputValue: string = ''
  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '25rem',
    minHeight: '5rem',
    width: '1160px',
    placeholder: 'Enter text here...',
    translate: 'no',
  };

  selectedValue: string = '';
  POAddForm: FormGroup | any;
  product_list: any;
  branch_list: any;
  vendor_list: any;
  dispatch_list: any;
  currency_list: any;
  currency_list1: any;
  netamount:any;
  overall_tax:any;
  tax_list: any;
  productcode_list: any;
  productgroup_list: any;
  terms_list: any[] = [];
  productform: FormGroup | any;
  responsedata: any;
  productunit_list: any;
  mdlProductName: any;
  mdlTerms :any;
  mdlProductGroup: any;
  mdlProductUnit: any;
  mdlProductCode: any;
  mdlBranchName: any;
  mdlVendorName: any;
  mdlDispatchName: any;
  mdlCurrencyName: any;
  mdlTaxName1: any;
  mdlTaxName2: any;
  mdlTaxName3: any;
  unitprice: number = 0;
  quantity: number = 0;
  discountpercentage: number = 0;
  discountamount: number = 0;
  totalamount: number = 0;
  addoncharges: number = 0;
  POdiscountamount: number = 0;
  frieghtcharges: number = 0;
  forwardingCharges: number = 0;
  insurancecharges: number = 0;
  roundoff: number = 0;
  grandtotal: any;
  taxamount1: number = 0;
  taxamount : number =0;
  taxpercentage: any;
  productdetails_list: any;
  taxamount2: number = 0;
  taxamount3: number = 0;
  producttotalamount: any;
  parameterValue: string | undefined;
  POproductlist: any;
  productnamelist: any;
  selectedCurrencyCode: any;
  POadd_list: any;
  total_amount: any;
  addoncharge: any;
  insurance_charges: any;
  additional_discount: any;
  freightcharges: any;
  packing_charges: any;
  buybackorscrap: any;
  tax_amount4:number=0;
  mdlTaxName4:any;



  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    flatpickr('.date-picker', options);

    this.POproductsummary();
    var api = 'PmrTrnPurchaseOrder/GetBranch';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.branch_list = this.responsedata.GetBranch;

    });
    var api = 'PmrTrnPurchaseOrder/GetVendor';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.vendor_list = this.responsedata.GetVendor;

    });
    var api = 'PmrTrnPurchaseOrder/GetDispatchToBranch';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.dispatch_list = this.responsedata.GetDispatchToBranch;

    });

    var api = 'PmrTrnPurchaseOrder/GetCurrency';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.currency_list = this.responsedata.GetCurrency;

    });
    var api = 'PmrTrnPurchaseOrder/GetTax';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.tax_list = result.GetTax;

    });
    var api = 'PmrTrnPurchaseOrder/GetProductCode';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.productcode_list = this.responsedata.GetProductCode;

    });
    var api = 'PmrMstProduct/GetProductUnit';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.productunit_list = this.responsedata.GetProductUnit;

    });
    var api = 'PmrTrnPurchaseQuotation/GetTermsandConditions';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.terms_list = this.responsedata.GetTermsandConditions
    });
    var api = 'PmrMstProduct/GetProductGroup';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.productgroup_list = this.responsedata.GetProductGroup;
      setTimeout(() => {

        $('#productgroup_list').DataTable();

      }, 0.1);
    });
    var api = 'PmrTrnPurchaseOrder/GetProduct';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.productdetails_list = this.responsedata.GetProduct;

      setTimeout(() => {

        $('#product_list').DataTable();

      }, 0.1);
    });
  }

  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService,public NgxSpinnerService:NgxSpinnerService) {

    this.POAddForm = new FormGroup({
      branch: new FormControl('', Validators.required),
      branch_name: new FormControl('', Validators.required),
      dispatch_name: new FormControl('', Validators.required),
      po_date: new FormControl(this.getCurrentDate(), Validators.required),
      vendor_companyname: new FormControl(''),
      tax_amount4: new FormControl(''),
      currency_code: new FormControl(''),
      payment_term: new FormControl(''),
      contact_person: new FormControl(''),
      email_address: new FormControl(''),
      contact_number: new FormControl(''),
      currency: new FormControl('', Validators.required),
      exchange_rate: new FormControl(''),
      remarks: new FormControl(''),
      Shipping_address: new FormControl(''),
      vendor_address: new FormControl(''),
      vendor_fax: new FormControl(''),
      priority_n: new FormControl('N'),
      taxamount1: new FormControl(''),
      buybackorscrap: new FormControl(''),
      payment_terms: new FormControl(''),
      freight_terms: new FormControl(''),
      delivery_location: new FormControl(''),
      template_content: new FormControl(''),
      delivery_period: new FormControl(''),
      payment_days: new FormControl(''),
      product_total: new FormControl(''),
      tax_name: new FormControl(''),
      discount_percentage: new FormControl(''),
      qty: new FormControl(''),
      mrp: new FormControl(''),
      unitprice: new FormControl(''),
      productuom_name: new FormControl(''),
      product_code: new FormControl(''),
      productgroup_name: new FormControl(''),
      product_name: new FormControl(''),
      totalamount: new FormControl(''),
      totalamount3: new FormControl(''),
      tax_name3: new FormControl(''),
      taxamount2: new FormControl(''),
      tax_name2: new FormControl(''),
      tax_name1: new FormControl(''),
      discountamount: new FormControl(''),
      discountpercentage: new FormControl(''),
      quantity: new FormControl(''),
      productcode: new FormControl(''),
      productgroup: new FormControl(''),
      priority_remarks: new FormControl(''),
      pocovernote_address: new FormControl(''),
      roundoff: new FormControl(''),
      ship_via: new FormControl(''),
      po_no: new FormControl(''),
      grandtotal: new FormControl(''),
      additional_discount: new FormControl(''),
      insurance_charges: new FormControl(''),
      freightcharges: new FormControl(''),
      addoncharge: new FormControl(''),
      delivery_days: new FormControl(''),
      template_name :new FormControl(''),
      total_amount: new FormControl(''),
      packing_charges: new FormControl(''),
      


    })

    this.productform = new FormGroup({
      tmppurchaseorderdtl_gid: new FormControl(''),
      product_gid: new FormControl(''),
      productuom_gid: new FormControl(''),
      productgroup_gid: new FormControl(''),
      product_code: new FormControl(''),
      productcode: new FormControl(''),
      productgroup: new FormControl(''),
      productuom_name: new FormControl(''),
      productname: new FormControl(''),
      tax_name1: new FormControl(''),
      tax_name2: new FormControl(''),
      tax_name3: new FormControl(''),
      remarks: new FormControl(''),
      product_name: new FormControl(''),
      productgroup_name: new FormControl(''),
      unitprice: new FormControl(''),
      quantity: new FormControl(''),
      discountpercentage: new FormControl(''),
      discountamount: new FormControl(''),
      taxname1: new FormControl(''),
      taxamount1: new FormControl(''),
      taxname2: new FormControl(''),
      taxamount2: new FormControl(''),
      taxname3: new FormControl(''),
      taxamount3: new FormControl(''),
      totalamount: new FormControl(''),
      tax_name: new FormControl(''),
      total_amount: new FormControl(''),
      packing_charges: new FormControl(''),
      netamount: new FormControl(''),
      overall_tax: new FormControl(''),
      template_content :new FormControl(''),





    })
  }
  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();
   
    return dd + '-' + mm + '-' + yyyy;
  }

  redirecttolist() {
    this.router.navigate(['/pmr/PmrTrnPurchaseorderSummary']);

  }
  get Shipping_address() {
    return this.POAddForm.get('Shipping_address')!;
  }
  get vendor_address() {
    return this.POAddForm.get('vendor_address')!;
  }
  get contact_person() {
    return this.POAddForm.get('contact_person')!;
  }
  get contact_number() {
    return this.POAddForm.get('contact_number')!;
  }
  get vendor_fax() {
    return this.POAddForm.get('vendor_fax')!;
  }
  get email_address() {
    return this.POAddForm.get('email_address')!;
  }
  get product_name() {
    return this.productform.get('product_name')!;
  }
  get product_code() {
    return this.productform.get('product_code')!;
  }
  get branch_name() {
    return this.productform.get('branch_name')!;
  }
  get tax_name1() {
    return this.productform.get('tax_name1')!;
  }
  get productuom_name() {
    return this.productform.get('productuom_name')!;
  }
  get productgroup_name() {
    return this.productform.get('productgroup_name')!;
  }
  get prodnameControl() {
    return this.productform.get('productgid');
  }
  get priority_remarks() {
    return this.productform.get('priority_remarks')
  }
  get tax() {
    return this.productform.get('tax')!;
  }
  OnChangeBranch() {
    let branch_gid = this.POAddForm.get("branch_name")?.value;

    let param = {
      branch_gid: branch_gid
    }
    var url = 'PmrTrnPurchaseOrder/GetOnChangeBranch';
    this.service.getparams(url, param).subscribe((result: any) => {

      this.POAddForm.get("Shipping_address")?.setValue(result.GetBranch[0].address1);
      console.log(result.GetBranch[0].address1)
    });

  }
  OnChangeVendor() {
    let vendor_gid = this.POAddForm.get("vendor_companyname")?.value;

    let param = {
      vendor_gid: vendor_gid
    }
    var url = 'PmrTrnPurchaseOrder/GetOnChangeVendor';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.POAddForm.get("vendor_address")?.setValue(result.GetVendor[0].address1);
      this.POAddForm.get("contact_number")?.setValue(result.GetVendor[0].contact_telephonenumber);
      this.POAddForm.get("contact_person")?.setValue(result.GetVendor[0].contactperson_name);
      this.POAddForm.get("vendor_fax")?.setValue(result.GetVendor[0].fax);
      this.POAddForm.get("email_address")?.setValue(result.GetVendor[0].email_id);
    });
  }

  GetOnChangeProductName() {
    debugger;
    let product_gid = this.productform.value.productname.product_gid;
    let param = {
      product_gid: product_gid
    }
    var url = 'PmrTrnPurchaseOrder/GetOnChangeProductName';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.productform.get("productcode")?.setValue(result.GetProductCode[0].product_code);
      this.productform.get("productuom_name")?.setValue(result.GetProductCode[0].productuom_name);
      this.productform.get("productgroup")?.setValue(result.GetProductCode[0].productgroup_name);
      this.productform.get("unitprice")?.setValue(result.GetproductsCode[0].unitprice);
      this.productform.value.productgroup_gid = result.GetProductCode[0].productgroup_gid,
        this.productform.value.productuom_gid = result.GetProductCode[0].productuom_gid
    });

  }
  OnChangeProductGroup() {
    let product_gid = this.productform.get("product_gid")?.value;

    let param = {
      product_gid: product_gid
    }
  

  }
  GetOnChangeTerms() {
    debugger

    let termsconditions_gid = this.POAddForm.value.template_name;
    let param = {
      termsconditions_gid: termsconditions_gid
    }
    var url = 'PmrTrnPurchaseQuotation/GetOnChangeTerms';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.POAddForm.get("template_content")?.setValue(result.terms_list[0].termsandconditions);
      this.POAddForm.value.termsconditions_gid = result.terms_list[0].termsconditions_gid
      //this.cusraiseform.value.productuom_gid = result.GetProductsName[0].productuom_gid
    });

    }




  OnChangeProductName() {
    let product_gid = this.productform.value.productname.product_gid;

    let param = {
      product_gid: product_gid
    }
    var url = 'PmrTrnPurchaseOrder/GetOnChangeProductName';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      debugger
      console.log(this.productdetails_list[0].productgroup_name)
      this.productdetails_list = this.responsedata.Getproductonchangedetails;
      this.productform.get("productcode")?.setValue(this.productdetails_list[0].product_code);
      this.productform.get("mrp")?.setValue(this.productdetails_list[0].product_price);
    })
  }




  productSubmit() {
    console.log(this.productform.value)
    var params = {
      tmppurchaseorderdtl_gid: this.productform.value.tmppurchaseorderdtl_gid,
      productname: this.productform.value.productname.product_name,
      product_gid: this.productform.value.productname.product_gid,
      quantity: this.productform.value.quantity,
      mrp: this.productform.value.totalamount,
      tax_name1: this.productform.value.tax_name1,
      tax_name2: this.productform.value.tax_name2,
      tax_name3: this.productform.value.tax_name3,
      taxamount1: this.productform.value.taxamount1,
      taxamount2: this.productform.value.taxamount2,
      taxamount3: this.productform.value.taxamount3,
      discountpercentage: this.productform.value.discountpercentage,
      discountamount: this.productform.value.discountamount,
      unitprice: this.productform.value.unitprice,
      productgroup_gid: this.productform.value.productgroup_gid,
      productgroup: this.productform.value.productgroup,
      productcode: this.productform.value.productcode,
      productuom_gid: this.productform.value.productuom_gid,
      productuom_name: this.productform.value.productuom_name,
      totalamount: this.productform.value.totalamount,
    }
    console.log(params)
    var api = 'PmrTrnPurchaseOrder/GetOnAdd';
    this.service.post(api, params).subscribe((result: any) => {
    this.productform.reset();
    this.POproductsummary()
    },
    );
  }

  prodtotalcal() {
    const subtotal = this.unitprice * this.quantity;
    this.discountamount = (subtotal * this.discountpercentage) / 100;
    this.totalamount = subtotal - this.discountamount;
  }

  OnChangeTaxAmount1() {
    debugger;
    let tax_gid = this.productform.get('tax_name1')?.value;

    this.taxpercentage = this.getDimensionsByFilter(tax_gid);
    console.log(this.taxpercentage);
    let tax_percentage = this.taxpercentage[0].percentage;
    console.group(tax_percentage)

    this.taxamount1 = ((tax_percentage) * (this.totalamount) / 100);

    if (this.taxamount1 == undefined) {
      const subtotal = this.unitprice * this.quantity;
      this.discountamount = (subtotal * this.discountpercentage) / 100;
      this.totalamount = subtotal - this.discountamount;
    }
    else {
      this.totalamount = ((this.totalamount) + (+this.taxamount1));
    }
  }
  OnChangeTaxAmount4() {
    debugger
    let tax_gid = this.POAddForm.get('tax_name')?.value;
 
    this.taxpercentage = this.getDimensionsByFilter(tax_gid);
    console.log(this.taxpercentage);
    let tax_percentage = this.taxpercentage[0].percentage;
    console.group(tax_percentage);
 

    this.tax_amount4 = ((tax_percentage) * (this.netamount) / 100);
 
    
  }



  OnChangeTaxAmount2() {
    let tax_gid2 = this.productform.get('tax_name2')?.value;

    this.taxpercentage = this.getDimensionsByFilter(tax_gid2);
    console.log(this.taxpercentage);
    let tax_percentage = this.taxpercentage[0].percentage;
    console.group(tax_percentage);

    const subtotal = this.unitprice * this.quantity;
    this.discountamount = (subtotal * this.discountpercentage) / 100;
    this.totalamount = subtotal - this.discountamount;

    this.taxamount2 = ((tax_percentage) * (this.totalamount) / 100);

    if (this.taxamount1 == undefined && this.taxamount2 == undefined) {
      const subtotal = this.unitprice * this.quantity;
      this.discountamount = (subtotal * this.discountpercentage) / 100;
      this.totalamount = subtotal - this.discountamount;
    }
    else {
      this.totalamount = ((this.totalamount) + (+this.taxamount1) + (+this.taxamount2));
    }
  }
  OnChangeTaxAmount3() {
    let tax_gid3 = this.productform.get('tax_name3')?.value;

    this.taxpercentage = this.getDimensionsByFilter(tax_gid3);
    console.log(this.taxpercentage);
    let tax_percentage = this.taxpercentage[0].percentage;
    console.group(tax_percentage);

    const subtotal = this.unitprice * this.quantity;
    this.discountamount = (subtotal * this.discountpercentage) / 100;
    this.totalamount = subtotal - this.discountamount;

    this.taxamount3 = ((tax_percentage) * (this.totalamount) / 100);

    if (this.taxamount1 == undefined && this.taxamount2 == undefined && this.taxamount3 == undefined) {
      const subtotal = this.unitprice * this.quantity;
      this.discountamount = (subtotal * this.discountpercentage) / 100;
      this.totalamount = subtotal - this.discountamount;
    }
    else {
      this.totalamount = ((this.totalamount) + (+this.taxamount1) + (+this.taxamount2) + (+this.taxamount3));
    }
  }
  getDimensionsByFilter(id: any) {
    return this.tax_list.filter((x: { tax_gid: any; }) => x.tax_gid === id);
  }

  finaltotal() {
    this.grandtotal = ((this.netamount) + (+this.addoncharge) + (+this.freightcharges) + (+this.buybackorscrap) + (+this.insurance_charges) + (+this.roundoff) - (+this.additional_discount)+(+this.tax_amount4));
  }

  // openModaldelete() {

  // }
  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }
  ondelete() {

    var url = 'PmrTrnPurchaseOrder/DeleteProductSummary'
    let param = {
      tmppurchaseorderdtl_gid: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {

      if (result.status == false) {
        this.ToastrService.warning(result.message)
        window.location.reload()
      }
      else {

        this.ToastrService.success(result.message)
        this.POproductsummary();
      }
    });
  }
  POproductsummary() {
    var api = 'PmrTrnPurchaseOrder/GetProductSummary';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;

      this.POproductlist = this.responsedata.productsummary_list;
      this.POAddForm.get("totalamount")?.setValue(this.responsedata.grand_total);
      this.POAddForm.get("grandtotal")?.setValue(this.responsedata.grandtotal);

      this.currency_code1 = ""
    });
  }




  showTextBox(event: Event) {
    const target = event.target as HTMLInputElement;
    this.showInput = target.value === 'Y';
  }

  OnChangeCurrency() {
    debugger
    let currencyexchange_gid = this.POAddForm.get("currency_code")?.value;
    console.log(currencyexchange_gid)
    let param = {
      currencyexchange_gid: currencyexchange_gid
    }
    var url = 'PmrTrnPurchaseOrder/GetOnChangeCurrency';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.currency_list1 = this.responsedata.GetOnchangeCurrency;
      this.POAddForm.get("exchange_rate")?.setValue(this.currency_list1[0].exchange_rate);
      this.currency_code1 = this.currency_list1[0].currency_code


    });

  }
  onCurrencyCodeChange(event: Event) {
    debugger
    const target = event.target as HTMLSelectElement;
    const selectedCurrencyCode = target.value;

    this.selectedCurrencyCode = selectedCurrencyCode;
    this.POAddForm.controls.currency_code.setValue(selectedCurrencyCode);
    this.POAddForm.get("currency_code")?.setValue(this.currency_list[0].currency_code);

  }

  onSubmit() {
    var params = {
      po_no: this.POAddForm.value.po_no,
      template_name :this.POAddForm.value.template_name,
      po_date: this.POAddForm.value.po_date,
      branch_name: this.POAddForm.value.branch_name,
      vendor_companyname: this.POAddForm.value.vendor_companyname,
      dispatch_name: this.POAddForm.value.dispatch_name,
      contact_number: this.POAddForm.value.contact_number,
      contact_person: this.POAddForm.value.contact_person,
      vendor_fax: this.POAddForm.value.vendor_fax,
      email_address: this.POAddForm.value.email_address,
      vendor_address: this.POAddForm.value.vendor_address,
      exchange_rate: this.POAddForm.value.exchange_rate,
      ship_via: this.POAddForm.value.ship_via,
      payment_terms: this.POAddForm.value.payment_terms,
      freight_terms: this.POAddForm.value.freight_terms,
      delivery_location: this.POAddForm.value.delivery_location,
      currency_code: this.POAddForm.value.currency_code,
      currency_gid: this.POAddForm.value.currency_gid,
      Shipping_address: this.POAddForm.value.Shipping_address,
      pocovernote_address: this.POAddForm.value.pocovernote_address,
      priority_flag: this.POAddForm.value.priority_flag,
      priority_remarks: this.POAddForm.value.priority_remarks,
      remarks: this.POAddForm.value.remarks,
      additional_discount: this.POAddForm.value.additional_discount,
      totalamount: this.POAddForm.value.totalamount,
      addoncharge: this.POAddForm.value.addoncharge,
      freightcharges: this.POAddForm.value.freightcharges,
      buybackorscrap: this.POAddForm.value.buybackorscrap,
      packing_charges: this.POAddForm.value.packing_charges,
      insurance_charges: this.POAddForm.value.insurance_charges,
      roundoff: this.POAddForm.value.roundoff,
      grandtotal: this.POAddForm.value.grandtotal,
      payment_days: this.POAddForm.value.payment_days,
      delivery_days: this.POAddForm.value.delivery_days,
      template_content: this.POAddForm.value.template_content,
      template_gid: this.POAddForm.value.template_gid,
      quantity: this.productform.value.quantity,
      mrp: this.productform.value.totalamount,
      tax_name1: this.productform.value.tax_name1,
      tax_name2: this.productform.value.tax_name2,
      tax_name3: this.productform.value.tax_name3,
      taxamount1: this.productform.value.taxamount1,
      taxamount2: this.productform.value.taxamount2,
      taxamount3: this.productform.value.taxamount3,
      discountpercentage: this.productform.value.discountpercentage,
      discountamount: this.productform.value.discountamount,
      unitprice: this.productform.value.unitprice,
      productgroup_gid: this.productform.value.productgroup_gid,
      productgroup: this.productform.value.productgroup,
      productcode: this.productform.value.productcode,
      productuom_gid: this.productform.value.productuom_gid,
      productuom_name: this.productform.value.productuom_name,
    }
    this.NgxSpinnerService.show();
    var api = 'PmrTrnPurchaseOrder/ProductSubmit';
    this.service.postparams(api, params).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)
      }
      else {
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message)
        this.router.navigate(['/pmr/PmrTrnPurchaseorderSummary']);
      }
      this.NgxSpinnerService.hide();
    });

  }
  OnTax() {


  }

}
