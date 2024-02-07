import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SelectionModel } from '@angular/cdk/collections';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';


interface ISinglePaymentReport {
  payment_date: string;
  entered_by: string;
  priority: string;
  textbox: string;
  email: string;
  contact_no: string;
  vendor_name: string;
  vendor_contact: string;
  vendor_address: string;
  fax: string;
  currency: string;
  exchange_rate: string;
  payment_remarks: string;
  payment_note: string;
  payment_mode: string;
  bank_name: string;
  cheque_no: string;
  branch_name: string;
  transaction_refno: string;
  transaction_no: string;
  card_name: string;
  dd_no: string;
}

@Component({
  selector: 'app-pbl-trn-multipleinvoice2singlepayment',
  templateUrl: './pbl-trn-multipleinvoice2singlepayment.component.html',
  styleUrls: ['./pbl-trn-multipleinvoice2singlepayment.component.scss']
})

export class PblTrnMultipleinvoice2singlepaymentComponent {
  showInput: boolean = false;
  showInput1: boolean = false;
  showInput2: boolean = false;
  showInput3: boolean = false;
  showInput4: boolean = false;
  showInput5: boolean = false;
  reactiveForm!: FormGroup;
  vendor_gid: any;
  selection = new SelectionModel<ISinglePaymentReport>(true, []);
  employeelist: any[] = [];
  singlepayment_list: any[] = [];
  singlepayment_list1: any[] = [];
  bankdetailslist: any[] = [];
  carddetailslist: any[] = [];
  vendor_lists: any[] = [];
  SinglePaymentReport!: ISinglePaymentReport;
  responsedata: any;
  singlepayment_list4: any[] = [];
  payment_mode:any;
  bank_name1:any;
  branch_namedd:any;
  branchche:any;
  cardname:any;
  branchnameneft:any;


  constructor(private formBuilder: FormBuilder,
    private spinnerService: NgxSpinnerService,
    private route: ActivatedRoute,
    private router: Router,
    private ToastrService: ToastrService,
    public service: SocketService,
    public NgxSpinnerService:NgxSpinnerService

  ) {
    this.SinglePaymentReport = {} as ISinglePaymentReport;
  }

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    flatpickr('.date-picker', options);
    this.reactiveForm = new FormGroup({
      payment_date: new FormControl(''),
      payment_date1: new FormControl(''),
      priority: new FormControl(''),
      textbox: new FormControl(''),
      payment_remarks: new FormControl(''),
      payment_note: new FormControl(''),
      payment_mode: new FormControl(''),
      bank_name: new FormControl(''),
      cheque_no: new FormControl(''),
      branch_name: new FormControl(''),
      transaction_refno: new FormControl(''),
      transaction_no: new FormControl(''),
      card_name: new FormControl(''),
      dd_no: new FormControl(''),
      advance: new FormControl(''),
      payment_amount: new FormControl(''),
      balancepo_advance: new FormControl(''),
      grand_total: new FormControl(''),
      tds_amount: new FormControl(''),
      final_amount: new FormControl(''),
      remark: new FormControl(''),
      totalpo_advance: new FormControl(''),

      singlepayment_list: this.formBuilder.array([])
    });

    const vendor_gid = this.route.snapshot.paramMap.get('vendor_gid');
    this.vendor_gid = vendor_gid;

    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.vendor_gid, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    this.Getmultipleinvoice2employee(deencryptedParam)

    var api = 'PblTrnPaymentRpt/GetBankDetail'
    this.service.get(api).subscribe((result: any) => {
      this.bankdetailslist = result.GetBankNameVle;
    });

    var api = 'PblTrnPaymentRpt/GetCardDetail'
    this.service.get(api).subscribe((result: any) => {
      this.carddetailslist = result.GetCardNameVle;
    });

    
  }

  Getmultipleinvoice2employee(params: any) {
    var param = {
      vendor_gid: params
    }
    var url = 'PblTrnPaymentRpt/Getmultipleinvoice2employeedtl'
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.vendor_lists = this.responsedata.Getmultipleinvoice2employeedtl;
    });

    var url = 'PblTrnPaymentRpt/GetpaymentInvoiceSummary'
    this.NgxSpinnerService.show();
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.singlepayment_list = this.responsedata.GetMultipleInvoiceSummary;
      this.NgxSpinnerService.hide();

      for (let i = 0; i < this.singlepayment_list.length; i++) {
        this.reactiveForm.addControl(`advance_${i}`, new FormControl(this.singlepayment_list[i].advance));
        this.reactiveForm.addControl(`payment_amount_${i}`, new FormControl(this.singlepayment_list[i].payment_amount));
        this.reactiveForm.addControl(`balancepo_advance_${i}`, new FormControl(this.singlepayment_list[i].balancepo_advance));
        this.reactiveForm.addControl(`totalpo_advance_${i}`, new FormControl(this.singlepayment_list[i].totalpo_advance));
        this.reactiveForm.addControl(`grand_total_${i}`, new FormControl(this.singlepayment_list[i].grand_total));
        this.reactiveForm.addControl(`tds_amount_${i}`, new FormControl(this.singlepayment_list[i].tds_amount));
        this.reactiveForm.addControl(`final_amount_${i}`, new FormControl(this.singlepayment_list[i].final_amount));
        this.reactiveForm.addControl(`remark_${i}`, new FormControl(this.singlepayment_list[i].remark));
      }
    });
    const currentDate = new Date().toISOString().split('T')[0];
    this.reactiveForm.get('payment_date')?.setValue(currentDate);
  }

  onKeyPress(event: any) {
    // Get the pressed key
    const key = event.key;

    if (!/^[0-9.]$/.test(key)) {
      // If not a number or dot, prevent the default action (key input)
      event.preventDefault();
    }
  }
  get bank_name() {
    return this.reactiveForm.get('bank_name')!;
  }
  get card_name() {
    return this.reactiveForm.get('card_name')!;
  }

  onsubmit() {
    window.scrollTo({ top: 0, behavior: 'smooth' });
    const selectedData = this.selection.selected; // Get the selected items
    if (selectedData.length === 0) {
      this.ToastrService.warning("Select Atleast one Invoice  to submit");
      return;
    }
    debugger;
    let f=0;
    if(this.reactiveForm.value.payment_mode==null ||
      this.reactiveForm.value.payment_mode==""
      ){f=1}
   if(f==0){
    for (const data of selectedData) {
      this.singlepayment_list1.push(data);
    }
    var url = 'PblTrnPaymentRpt/Postmultipleinvoice2singlepayment';

    // Convert the FormGroup's value to a plain object
    const formValues = this.reactiveForm.value;
    const params = {
      Getmultipleinvoice2employeedtl: this.vendor_lists,
      GetMultipleInvoiceSummary: this.singlepayment_list1,
      paymentdtls: formValues,
    };
    debugger;
    this.NgxSpinnerService.show();
    this.service.postparams(url, params).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      if (result.status === false) {
        this.ToastrService.warning(result.message);
      } else {
        this.ToastrService.success(result.message);
        this.router.navigate(['/payable/PblTrnPaymentAddProceed'])
      }
    });
  }
  else{
    this.ToastrService.warning("Fill All Mandatory Fields")
  }
  }

  onback() {
    this.router.navigate(['/payable/PblTrnPaymentAddProceed']);
  }

  showTextBox(event: Event) {
    debugger
    const target = event.target as HTMLInputElement;
    this.showInput = target.value === 'Cheque';
    this.showInput2 = target.value === 'DD';
    this.showInput3 = target.value === 'Credit Card';
    this.showInput4 = target.value === 'NEFT';
    this.showInput5 = target.value === 'Cash';
  }

  showTextBox2(event: Event) {
    const target = event.target as HTMLInputElement;
    this.showInput1 = target.value === 'High';
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.singlepayment_list.length;
    return numSelected === numRows;
  }

  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.singlepayment_list.forEach((row: ISinglePaymentReport) => this.selection.select(row));
  }
}