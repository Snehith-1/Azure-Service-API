import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { NgxSpinnerService } from 'ngx-spinner';

  
@Component({
  selector: 'app-ims-trn-raise-deliveryorder',
  templateUrl: './ims-trn-raise-deliveryorder.component.html',
  styleUrls: ['./ims-trn-raise-deliveryorder.component.scss']
})
export class ImsTrnRaiseDeliveryorderComponent {
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

    defaultParagraphSeparator: 'p',

    defaultFontName: 'Arial',



  };


  deliveryform: FormGroup | any;
  deliveryform1: FormGroup | any;
  template_content : FormGroup |any;
  productform: FormGroup | any;
  raisedeliveryorder_list: any;
  branch_list : any [] = [];
  customer_list: any [] = [];
  contact_list: any [] = [];
  sales_list: any [] = [];
  currency_list: any [] = [];
  product_list: any [] = [];
  productgroup_list: any [] = [];
  productname_list: any [] = [];
  tax_list: any [] = [];
  POproductlist: any [] = [];
  OutstandingQty_list: any [] = [];
  IssuedQty_list: any [] = [];
  totalamount: any;
  taxamount3: any;
  mdlTaxName3: any;
  taxamount2: any;
  taxamount1: any;
  discountamount: any;
  discountpercentage: any;
  quantity: any;
  salesorder_gid:any;
  deliveryorder: any;
  raisedelivery_list :any
  response_data :any
  parameter: any;
  responsedata: any;
  producttotalamount:any;
  raisedeliveryorder_list1:any;
  total_amount:any;
  constructor(private formBuilder: FormBuilder, public ToastrService: ToastrService, public service: SocketService, public NgxSpinnerService: NgxSpinnerService, private route: Router,private router: ActivatedRoute) {
   
    this.deliveryform1 = new FormGroup ({

      product_gid: new FormControl(''),
      salesorder_gid: new FormControl(''),
    producttotalamount:new FormControl(''),
    total_amount:new FormControl(''),
    
    product_name:new FormControl(''),
    uom_name:new FormControl(''),
    display_field:new FormControl(''),
    outstanding_qty:new FormControl(''),
    created_date:new FormControl(''),
    reference_gid:new FormControl(''),
    display:new FormControl(''),    
    stock_gid:new FormControl(''),
    stock_qty:new FormControl(''),

    })

    this.deliveryform = new FormGroup ({
      salesorder_gid: new FormControl(''),
      salesorder_date: new FormControl(''),
      customer_branch: new FormControl(''),
      branch_gid: new FormControl(''),
      so_referencenumber: new FormControl(''),
      customer_gid: new FormControl(''),
      customer_name: new FormControl(''),
      customer_address_so : new FormControl(''),
      customer_contact_gid: new FormControl(''),
      customercontact_names: new FormControl(''),
      customer_mobile: new FormControl(''),
      customer_email: new FormControl(''),
      customer_address: new FormControl(''),
      dc_no : new FormControl(''),
      customer_mode : new FormControl(''),
      customer_city:new FormControl(''),
      productgroup_gid: new FormControl(''),
      productgroup_name: new FormControl(''),
      customerproduct_code: new FormControl(''),
      product_code: new FormControl(''),
      product_gid: new FormControl(''),
      product_name: new FormControl(''),
      display_field: new FormControl(''),
      template_content : new FormControl(''),
      qty_quoted: new FormControl(''),
      product_uom: new FormControl(''),
      product_requireddate: new FormControl(''),
      product_requireddateremarks: new FormControl(''),
      reamark : new FormControl(''),
      available_quantity : new FormControl(''),
      product_delivered : new FormControl(''),
      producttotalamount:new FormControl(''),
      total_amount:new FormControl(''),
      
    
      
        
        });
  }

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    flatpickr('.date-picker', options);
    

  this.deliveryform = new FormGroup ({
  salesorder_gid: new FormControl(''),
  salesorder_date: new FormControl(''),
  customer_branch: new FormControl(''),
  branch_gid: new FormControl(''),
  so_referencenumber: new FormControl(''),
  customer_gid: new FormControl(''),
  customer_name: new FormControl(''),
  customer_address_so : new FormControl(''),
  customer_contact_gid: new FormControl(''),
  customercontact_names: new FormControl(''),
  customer_mobile: new FormControl(''),
  customer_email: new FormControl(''),
  customer_address: new FormControl(''),
  dc_no: new FormControl(''),
  customer_mode : new FormControl(''),
  customer_city:new FormControl(''),
  productgroup_gid: new FormControl(''),
  productgroup_name: new FormControl(''),
  customerproduct_code: new FormControl(''),
  product_code: new FormControl(''),
  product_gid: new FormControl(''),
  product_name: new FormControl(''),
  display_field: new FormControl(''),
  template_content : new FormControl(''),
  qty_quoted: new FormControl(''),
  product_uom: new FormControl(''),
  product_requireddate: new FormControl(''),
  product_requireddateremarks: new FormControl(''),
  reamark : new FormControl(''),
  available_quantity : new FormControl(''),
  product_delivered : new FormControl(''),
  producttotalamount:new FormControl(''),
  total_amount:new FormControl(''),
  

  
    
    });
 
    this.deliveryform1 = new FormGroup ({
      product_gid: new FormControl(''),
      salesorder_gid: new FormControl(''),

    product_name:new FormControl(''),
    uom_name:new FormControl(''),
    display_field:new FormControl(''),
    outstanding_qty:new FormControl(''),
    created_date:new FormControl(''),
    reference_gid:new FormControl(''),
    display:new FormControl(''),    
    stock_gid:new FormControl(''),
    stock_qty:new FormControl(''),
    producttotalamount:new FormControl(''),
    total_amount:new FormControl(''),
    
      

    })
    

debugger
  this.deliveryorder = this.router.snapshot.paramMap.get('salesorder_gid');

     const secretKey = 'storyboarderp';
 
     const deencryptedParam = AES.decrypt(this.deliveryorder, secretKey).toString(enc.Utf8);
 
     this.GetRaiseDeliveryorderSummary(deencryptedParam);
     this.GetProductdelivery(deencryptedParam);

   
 
}
  
 
  GetRaiseDeliveryorderSummary(salesorder_gid: any) {
    debugger
        var url = 'ImsTrnDeliveryorderSummary/GetRaiseDeliveryorderSummary'
        this.NgxSpinnerService.show()
    
        let param = {
    
          salesorder_gid: salesorder_gid
        }
        this.service.getparams(url, param).subscribe((result: any) => {
    debugger;
          this.raisedelivery_list = result.raisedelivery_list;
          this.deliveryform.get("salesorder_gid")?.setValue(result.raisedelivery_list[0].salesorder_gid);
          this.deliveryform.get("product_gid")?.setValue(result.raisedelivery_list[0].product_gid);
          this.deliveryform.get("salesorder_date")?.setValue(result.raisedelivery_list[0].salesorder_date);
          this.deliveryform.get("customer_branch")?.setValue(result.raisedelivery_list[0].customer_branch);
          this.deliveryform.get("customer_name")?.setValue(result.raisedelivery_list[0].customer_name);
          this.deliveryform.get("so_referencenumber")?.setValue(result.raisedelivery_list[0].so_referencenumber);
          this.deliveryform.get("customercontact_names")?.setValue(result.raisedelivery_list[0].customercontact_names);
          this.deliveryform.get("customer_mobile")?.setValue(result.raisedelivery_list[0].customer_mobile);
          this.deliveryform.get("customer_email")?.setValue(result.raisedelivery_list[0].customer_email);       
          this.deliveryform.get("customer_address")?.setValue(result.raisedelivery_list[0].customer_address_so);
          this.deliveryform.get("customer_address_so")?.setValue(result.raisedelivery_list[0].customer_address_so);
          this.deliveryform.get("productgroup_name")?.setValue(result.raisedelivery_list[0].productgroup_name);
          this.deliveryform.get("customerproduct_code")?.setValue(result.raisedelivery_list[0].customerproduct_code);
          this.deliveryform.get("product_code")?.setValue(result.raisedelivery_list[0].product_code);
          this.deliveryform.get("product_name")?.setValue(result.raisedelivery_list[0].product_name);
          this.deliveryform.get("display_field")?.setValue(result.raisedelivery_list[0].display_field);
          this.deliveryform.get("uom_name")?.setValue(result.raisedelivery_list[0].uom_name);
          this.deliveryform.get("available_quantity")?.setValue(result.raisedelivery_list[0].available_quantity);
          this.deliveryform.get("qty_quoted")?.setValue(result.raisedelivery_list[0].qty_quoted);
          this.deliveryform.get("product_delivered")?.setValue(result.raisedelivery_list[0].product_delivered);
          this.NgxSpinnerService.hide()
         
        
        });
      }
    
      GetProductdelivery(salesorder_gid :any){

        debugger
        
        var api = 'ImsTrnDeliveryorderSummary/GetProductdelivery';
        let param = {
    
          salesorder_gid: salesorder_gid
        }
        this.service.getparams(api,param).subscribe((result:any) => {
          this.raisedeliveryorder_list1 = result.raisedelivery_list;
          setTimeout(()=>{  
            $('#raisedelivery_list').DataTable();
          }, 1);
        });
      
      }
      
      onadd(salesorder_gid:any, product_gid:any) {
        debugger
        var api = 'ImsTrnDeliveryorderSummary/IssueFromStock';
        let param1 = {
    
          product_gid: product_gid,
          salesorder_gid:salesorder_gid
          
        }
        this.service.getparams(api,param1).subscribe((result:any) => {
          this.IssuedQty_list = result.IssuedQty_list;
          setTimeout(()=>{  
            $('#IssuedQty_list').DataTable();
          }, 1);
        });



        var api = 'ImsTrnDeliveryorderSummary/GetOutstandingQty';
        let param2 = {
    
          salesorder_gid: salesorder_gid
        }
        this.service.getparams(api,param2).subscribe((result:any) => {
          this.OutstandingQty_list = result.OutstandingQty_list;
          setTimeout(()=>{  
            $('#raisedelivery_list').DataTable();
          }, 1);
        });
        
        //this.deliveryform.get("total_amount")?.setValue(this.responsedata.producttotalamount);


      }

//       prodtotalcal(){
//         this.total_amount = this.producttotalamount;
//       }

OnSubmit(){
  this.deliveryorder = this.router.snapshot.paramMap.get('salesorder_gid');

     const secretKey = 'storyboarderp';
 
     const deencryptedParam = AES.decrypt(this.deliveryorder, secretKey).toString(enc.Utf8);
     this.salesorder_gid = deencryptedParam

  console.log(this.IssuedQty_list)
  var params = {
   salesorder_gid : this.salesorder_gid,
    product_gid:this.IssuedQty_list[0].product_gid,
      //salesorder_gid:this.IssuedQty_list[0].salesorder_gid,
      stock_gid:this.IssuedQty_list[0].stock_gid,
      outstanding_qty:this.OutstandingQty_list[0].outstanding_qty,
      stock_qty:this.IssuedQty_list[0].stock_qty,
    producttotalamount:this.deliveryform1.value.producttotalamount,
    total_amount:this.deliveryform1.value.total_amount,
  }

  
      var url='ImsTrnDeliveryorderSummary/PostSelectIssueQtySubmit'

      this.service.post(url, params).subscribe((result: any) => {
        if(result.status == false){
          this.ToastrService.warning(result.message)        
        }
        else{
          this.ToastrService.success(result.message)
          // this.route.navigate(['/smr/SmrTrnQuotationSummary']);   
        }       
      });
}


onSubmitDO(){
  
    debugger
    var params = {

      salesorder_date: this.deliveryform.value.salesorder_date,
      customer_name:this.deliveryform.value.customer_name,
      customer_branch:this.deliveryform.value.customer_branch,
      customercontact_names:this.deliveryform.value.customercontact_names,
      customer_mobile:this.deliveryform.value.customer_mobile,
      customer_email:this.deliveryform.value.customer_email,
      customer_address:this.deliveryform.value.customer_address,
      customer_address_so:this.deliveryform.value.customer_address_so,
      so_referencenumber:this.deliveryform.value.so_referencenumber,
      customer_mode:this.deliveryform.value.customer_mode,
      dc_no:this.deliveryform.value.dc_no,
      customer_city:this.deliveryform.value.customer_city,
      template_content:this.deliveryform.value.template_content,
     
      //  salesorderdtl_gid:this.raisedelivery_list[0].salesorderdtl_gid,
      //  productgroup_gid:this.raisedelivery_list[0].productgroup_gid,
      //  productgroup_name : this.raisedelivery_list[0].productgroup_name,
      // customerproduct_code:this.raisedelivery_list[0].customerproduct_code,
      //  product_code:this.raisedelivery_list[0].product_code,
      //  product_name:this.raisedelivery_list[0].product_name,
      //  display_field:this.raisedelivery_list[0].display_field,
      //  uom_name:this.raisedelivery_list[0].uom_name,
      // qty_quoted:this.raisedelivery_list[0].qty_quoted,
      // available_quantity:this.raisedelivery_list[0].available_quantity,
      //  product_delivered:this.raisedelivery_list[0].product_delivered,
      //  product_requireddate:this.raisedelivery_list[0].product_requireddate,
      // product_gid:this.raisedelivery_list[0].product_gid,
    //   salesorder_gid:this.raisedeliveryorder_list[0].salesorder_gid,
    //   stock_gid:this.raisedeliveryorder_list[0].stock_gid,
      
    // producttotalamount:this.deliveryform1.value.producttotalamount,
    // total_amount:this.deliveryform1.value.total_amount,

      

    }
        var url='ImsTrnDeliveryorderSummary/PostDeliveryOrderSubmit'
        this.NgxSpinnerService.show()
   
        this.service.post(url, params).subscribe((result: any) => {
          if(result.status == false){
            this.ToastrService.warning(result.message)    
            this.NgxSpinnerService.hide()
          }
          else{
            this.ToastrService.success(result.message)
             this.route.navigate(['/ims/ImsTrnDeliveryorder']);
             this.NgxSpinnerService.hide()
          }      
        });
  
}

onclose(){
  this.deliveryform1.reset();
}
}




