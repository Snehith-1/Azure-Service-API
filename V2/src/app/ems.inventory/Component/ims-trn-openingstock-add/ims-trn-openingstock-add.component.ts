import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
interface addopeningstock{
  branch_name: string;
  branch_gid: string;
  productgroup_gid: string;
  productgroup_name: string;
  customerproduct_code: string;
  product_code: string;
  product_gid: string;
  product_name: string;
  cost_price: string;
  product_desc: string;
  stock_qty: string;
  uom_gid: string;
  location_gid: string;
  location_name: string;
  productuom_name: string;
  productuom_gid: string;
}

@Component({
  selector: 'app-ims-trn-openingstock-add',
     templateUrl: './ims-trn-openingstock-add.component.html',
     styleUrls: ['./ims-trn-openingstock-add.component.scss']
})
export class ImsTrnOpeningstockAddComponent {
  file!: File;
  producttype_list: any[] = [];
  productgroup_list: any[] = [];
  branch_list : any [] =[];
  GetLocation :any[]= [];
  productunitclass_list: any[] = [];
  productunit_list: any[] = [];
  product_list: any[] = [];
  productcode_list: any[] = [];
  rbo_status: any[] = [];
  Form: FormGroup | any;
  hasError?: boolean;
  returnUrl?: string;
  mdlPrdType:any;
  mdlPrdUnitC:any;
  mdlPrdUnit:any;
  mdlPrdName:any;
  productsCode:any;
  responsedata: any;
  submitted = false;
  
  result: any;
  productform: FormGroup | any;
  addopeningstock !:addopeningstock;
  fb: any;
  mdlProductName:any;
  mdlBranchName :any;
 uom_gid:any;
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService,public NgxSpinnerService:NgxSpinnerService, public service: SocketService, private route: ActivatedRoute,private router:Router) {
    this.addopeningstock = {} as addopeningstock;
  
    this.productform = new FormGroup({
      branch_name: new FormControl('', Validators.required),
      branch_gid: new FormControl(''),
      product_gid :new FormControl(''),
      productgroup_name:new FormControl(''),
      productuomclass_name:new FormControl('',),
      productuom_name:new FormControl(''),
      productuom_gid:new FormControl(''),
      product_code:new FormControl(''),
      product_name:new FormControl('',Validators.required),
      product_desc:new FormControl('',Validators.required),
      location_name :new FormControl(''),
      location_gid :new FormControl(''),
      stock_qty:new FormControl('',Validators.required),
      cost_price:new FormControl('',Validators.required),
      

    });
  
  }
  ngOnInit(): void {

    var api = 'PmrMstProduct/GetProductGroup';
    this.service.get(api).subscribe((result: any) => {
      $('#productgroup_list').DataTable().destroy();
      this.productgroup_list = result.GetProductGroup;
      // setTimeout(()=>{  

      //   $('#productgroup_list').DataTable();

      // }, 0.1);
    });
    var api = 'PmrMstProduct/GetProducttype';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.producttype_list = this.responsedata.GetProducttype;

    });

    var api = 'PmrMstProduct/GetProductUnitclass';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.productunitclass_list = this.responsedata.GetProductUnitclass;

    });
          //// Product Dropdown ////
          debugger
   var url = 'SmrTrnSalesorder/GetProductNamDtl'
   this.service.get(url).subscribe((result:any)=>{
     this.product_list = result.GetProductNamDtl;
    });

    var api = 'PmrMstProduct/GetProductUnit';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.productunit_list = this.responsedata.GetProductUnit;

    });
    var url = 'SmrTrnSalesorder/GetBranchDtl'
    this.service.get(url).subscribe((result:any)=>{
      this.branch_list = result.GetBranchDtl;
      this.productform.get("branch_gid")?.setValue(result.GetBranchDtl[0].branch_gid);

     });

     
    var api = 'ImsTrnOpeningStock/GetImsTrnOpeningstockAdd';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.productcode_list = this.responsedata.stockadd_list;

    });
  }

  get branch_name() {

    return this.productform.get('branch_name')!;

  };
  get productgroup_name() {

    return this.productform.get('productgroup_name')!;

  };
  get productuomclass_name() {

    return this.productform.get('productuomclass_name')!;

  };
  get productuom_name() {

    return this.productform.get('productuom_name')!;

  };
  get productuom_gid() {

    return this.productform.get('productuom_gid')!;

  };
  get product_code() {

    return this.productform.get('product_code')!;

  };
  get stock_qty() {

    return this.productform.get('stock_qty')!;

  };
  get product_name() {

    return this.productform.get('product_name')!;

  }
  get location_name() {

    return this.productform.get('location_name')!;

  }
  get product_desc() {

    return this.productform.get('product_desc')!;

  }
  get cost_price() {

    return this.productform.get('cost_price')!;

  }




  initForm() {
    this.productform = this.fb.group({
      branch_name: [ this.productform.branch_name,  Validators.compose([

          Validators.required,
          ]),
      ],
      location_name: [
        this.productform.location_name,
        Validators.compose([
          Validators.required

        ]),
      ],
      productgroup_name: [
        this.productform.productgroup_name,
        Validators.compose([
          Validators.required

        ]),
      ],

      productuomclass_name: [
        this.productform.productuomclass_name,
        Validators.compose([
          Validators.required

        ]),
      ],
      productuom_name: [
        this.productform.productuom_name,
        Validators.compose([
        Validators.required,

        ]),
      ],
      product_code: [
        this.productform.product_code,
        Validators.compose([
        Validators.required,

        ]),
      ],
      product_name: [
        this.productform.product_name,
        Validators.compose([
        Validators.required,

        ]),
      ],
      product_desc: [
        this.productform.product_desc,
        Validators.compose([
        Validators.required,

        ]),
      ],

      stock_qty: [
        this.productform.stock_qty,
        Validators.compose([
        Validators.required,

        ]),
      ],
      

      cost_price: [
        this.productform.cost_price,
        Validators.compose([
          Validators.required

        ]),
      ],
    });
  
  }
 

redirecttolist(){
  this.router.navigate(['/ims/ImsTrnOpeningstockSummary']);

}


GetOnChangeProductsName(){
  debugger
  let product_gid = this.productform.value.product_name.product_gid;
  

  let param = {
    product_gid: product_gid
    
  }
  var url = 'ImsTrnOpeningStock/GetOnChangeproductName';
  this.service.getparams(url, param).subscribe((result: any) => {
    this.responsedata = result;
    this.productsCode = result.ProductsCode;
    this.productform.get("product_code")?.setValue(result.ProductsCode[0].product_code);
    this.productform.get("productuom_name")?.setValue(result.ProductsCode[0].productuom_name);
    this.productform.get("productgroup_name")?.setValue(result.ProductsCode[0].productgroup_name);
    this.productform.value.productgroup_gid = result.ProductsCode[0].productgroup_gid;
    this.uom_gid = result.ProductsCode[0].productuom_gid;
    console.log(this.uom_gid)
  });
}
GetOnChangeLocation() {
  
  let branch_gid = this.productform.value.branch_name;
  let param = {
    branch_gid: branch_gid
  }
  var url = 'ImsTrnOpeningStock/GetOnChangeLocation';
  this.service.getparams(url, param).subscribe((result: any) => {
    this.responsedata = result;
    this.GetLocation = this.responsedata.GetLocation;
    this.productform.get("location_name")?.setValue(result.GetLocation[0].location_name);
    this.productform.get("location_gid")?.setValue(result.GetLocation[0].location_gid);
     // this.productform.value.productuom_gid = result.GetProductsCode[0].productuom_gid
  });
}



public validate(): void {
  this.addopeningstock = this.productform.value;

  if (this.addopeningstock.stock_qty != null && this.addopeningstock.cost_price != null && this.addopeningstock.branch_name != null && this.addopeningstock.product_name != null  ) {
    // let formData = new FormData();
    if (this.file != null && this.file != undefined) {
      debugger
      console.log(this.productform);
  var params = {    
    product_gid:this.productform.value.product_name.product_gid,
    branch_gid:this.productform.value.branch_name.branch_gid,
    branch_name:this.productform.value.branch_name.branch_name,
    product_name:this.productform.value.product_name.product_name,
    product_code:this.productform.value.product_code,
    productuom_name:this.productform.value.productuom_name,
    productgroup_name:this.productform.value.productgroup_name,
    location_name:this.productform.value.location_name,
    location_gid:this.productform.value.location_gid,
    unit_price:this.productform.value.cost_price,
    display_field:this.productform.value.product_desc,
    stock_qty:this.productform.value.stock_qty,
    uom_gid:this.uom_gid
   }
  
   var url = 'ImsTrnOpeningStock/PostOpeningstock'
   this.NgxSpinnerService.show();
   this.service.post(url,params).subscribe((result: any) => {
        debugger
        this.responsedata = result;
        if (result.status == false) {
          this.ToastrService.warning(result.message)
        }
        else {
          this.router.navigate(['/ims/ImsTrnOpeningstockSummary']);
          this.ToastrService.success(result.message)
          this.NgxSpinnerService.hide();
        }
        
      });

    }
    else {
      var params = {    
        product_gid:this.productform.value.product_name.product_gid,
        branch_gid:this.productform.value.branch_name.branch_gid,
        branch_name:this.productform.value.branch_name.branch_name,
        product_name:this.productform.value.product_name.product_name,
        product_code:this.productform.value.product_code,
        productuom_name:this.productform.value.productuom_name,
        productgroup_name:this.productform.value.productgroup_name,
        location_name:this.productform.value.location_name,
        location_gid:this.productform.value.location_gid,
        unit_price:this.productform.value.cost_price,
        display_field:this.productform.value.product_desc,
        stock_qty:this.productform.value.stock_qty,
        uom_gid:this.uom_gid
       }
      var url2 = 'ImsTrnOpeningStock/PostOpeningstock'
   this.service.post(url2,params).subscribe((result: any) => {
        if (result.status == false) {
          this.ToastrService.warning(result.message)
        }
        else {
          this.router.navigate(['/ims/ImsTrnOpeningstockSummary']);
          this.ToastrService.success(result.message)
        }
        this.responsedata = result;
      });
    }
  }
  else {
    this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
  }
  this.NgxSpinnerService.hide();
  return;
  
  


}

}
