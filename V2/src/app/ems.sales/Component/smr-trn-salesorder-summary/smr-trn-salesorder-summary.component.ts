import { Component } from '@angular/core';
import { FormBuilder, FormGroup,} from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { AES } from 'crypto-js';
import { SelectionModel } from '@angular/cdk/collections';
import { NgxSpinnerService } from 'ngx-spinner';



export class salesorderstatus {
  salesorders_list: string[] = [];
  employee_gid:any;
}

@Component({
  selector: 'app-smr-trn-salesorder-summary',
  templateUrl: './smr-trn-salesorder-summary.component.html',
  styleUrls: ['./smr-trn-salesorder-summary.component.scss']
})
export class SmrTrnSalesorderSummaryComponent {
  
  reactiveForm!: FormGroup;
  responsedata: any;
  salesorder_list: any[] = [];
  salesproduct_list: any[] = [];
  getData: any;
  boolean: any;
  pick: Array<any> = [];
  parameterValue1: any;
  parameterValue2: any;
  parameterValue3: any;
  salesorder_gid: any;
  CurObj: salesorderstatus = new salesorderstatus();
  selection = new SelectionModel<salesorderstatus>(true, []);
  products: any[] = [];
  constructor (private formBuilder: FormBuilder,private NgxSpinnerService: NgxSpinnerService,public route:ActivatedRoute,public service :SocketService,private router:Router,private ToastrService: ToastrService) {
  }
    
  
  ngOnInit(): void {
    debugger
    this.GetSmrTrnSalesordersummary();
}

PrintPDF(salesorder_gid: string) {
  this.GetSmrTrnSalesordersummary();
}
//// Summary Grid//////
GetSmrTrnSalesordersummary( )

 {

  var url = 'SmrTrnSalesorder/GetSmrTrnSalesordersummary'
  this.NgxSpinnerService.show()
  this.service.get(url).subscribe((result: any) => {
    $('#salesorder_list').DataTable().destroy();
    this.responsedata = result;
    this.salesorder_list = this.responsedata.salesorder_list;
    setTimeout(() => {
      $('#salesorder_list').DataTable();
    }, 1);
    this.NgxSpinnerService.hide()


  })
  
  
}
onview(params:any){
  const secretKey = 'storyboarderp';
  const param = (params);
  const lspage1 = "SmrTrnSalesorderview";
  const leadbank_gid1 = "";
  const leadbank_gid = AES.encrypt(leadbank_gid1, secretKey).toString();
  const lspage = AES.encrypt(lspage1, secretKey).toString();
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  this.router.navigate(['/smr/SmrTrnSalesorderview',encryptedParam,leadbank_gid,lspage]);
}
onamend(params:any){
  const secretKey = 'storyboarderp';
  const param = (params);
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  this.router.navigate(['/smr/SmrTrnSalesorderamend',encryptedParam])
}

add(){
  this.router.navigate(['/smr/SmrTrnRaiseSalesOrder'])
} 
  openModaledit(){}
  openModaldelete(){}
  onattach(){}
  openModalshop(){}

  openModalcancel(parameter: string){
    this.parameterValue1 = parameter
  }

  openModalmakepayment(){
   
  }

  openModalmakeapprove(){
   
  }
  onsubmitsalesupdate(){
    this.pick = this.selection.selected
    let list = this.pick
    this.CurObj.salesorders_list = list
    if (this.CurObj.salesorders_list.length != 0) {
      
      this.NgxSpinnerService.show();
      var url1 = 'SmrTrnSalesorder/Getsalesonupdate'
      this.service.post(url1, this.CurObj).pipe().subscribe((result: any) => {

        if (result.status == false) {

          this.NgxSpinnerService.hide();
          this.ToastrService.warning('Error While Upadating Status!')
        }
        else {
          this.GetSmrTrnSalesordersummary();
          this.NgxSpinnerService.hide();
          this.selection.clear() ;
          this.ToastrService.success('Status Updated Successfully!')

        }

      });
    }
    else {

      this.ToastrService.warning("Error While Upadating Status!")
    }
  }
  

  oncancel(){
    debugger;
    console.log(this.parameterValue1);
    var url3 = 'SmrTrnSalesorder/getCancelSalesOrder'
    this.service.getid(url3, this.parameterValue1).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning('Error While Cancelling Order')
        this.GetSmrTrnSalesordersummary();
  
      }
      else {
        this.ToastrService.success('Order Cancelled Successfully')
        this.GetSmrTrnSalesordersummary();
        window.location.reload();
      }
  
    });
  }
  openModalupdate(parameter: string){
    this.parameterValue2 = parameter
  }
  onupdate(){
    debugger;
    console.log(this.parameterValue2);
    var url3 = 'SmrTrnSalesorder/getupdate'
    this.service.getid(url3, this.parameterValue2).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning('Error While Upadating Status!')
        this.GetSmrTrnSalesordersummary();
  
      }
      else {
        this.ToastrService.success('Status Updated Successfully!')
        this.GetSmrTrnSalesordersummary();
        window.location.reload();
      }
  
    });
  }

  Details(parameter: string,salesorder_gid: string){
  this.parameterValue1 = parameter;
  this.salesorder_gid = parameter;

  var url='SmrTrnSalesorder/GetSalesProductdetails'
    let param = {
      salesorder_gid : salesorder_gid 
    }
    this.service.getparams(url,param).subscribe((result:any)=>{
    this.responsedata=result;
    this.salesproduct_list = result.salesproduct_list;   
    });
    
  
}
isAllSelected() {
  const numSelected = this.selection.selected.length;
  const numRows = this.salesorder_list.length;
  return numSelected === numRows;
}
masterToggle() {
  this.isAllSelected() ?
    this.selection.clear() :
    this.salesorder_list.forEach((row: salesorderstatus) => this.selection.select(row));
}
}


