import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators,} from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-smr-rpt-salesreport',
  templateUrl: './smr-rpt-salesreport.component.html',
  styleUrls: ['./smr-rpt-salesreport.component.scss']
})
export class SmrRptSalesreportComponent {
  GetMonthwiseOrderReport_List :any;
  GetOrderwiseOrderReport_List : any;
  month :any;
  data: any; 
  salesorder_gid :any; 
  parameterValue: any;
  reactiveForm: FormGroup | any;
  responsedata: any;
  constructor(private formBuilder: FormBuilder,public route:ActivatedRoute,public service :SocketService,private router:Router,private ToastrService: ToastrService,public NgxSpinnerService :NgxSpinnerService) {
    
    
    

  }
  

  ngOnInit(): void {
    this.GetMonthwiseOrderReport();
    this.GetOrderwiseOrderReport();
  }
  GetMonthwiseOrderReport( )
 {
  var url = 'SmrRptOrderReport/GetMonthwiseOrderReport'
  this.NgxSpinnerService.show()
  this.service.get(url).subscribe((result: any) => {
    $('#GetMonthwiseOrderReport_List').DataTable().destroy();
    this.responsedata = result;
    this.GetMonthwiseOrderReport_List = this.responsedata.GetMonthwiseOrderReport_List;
    setTimeout(() => {
      $('#GetMonthwiseOrderReport_List');
    }, 1);
    this.NgxSpinnerService.hide()

  })

  
}

  GetOrderwiseOrderReport( )
  {
   var url = 'SmrRptOrderReport/GetOrderwiseOrderReport'
   this.NgxSpinnerService.show()
   this.service.get(url).subscribe((result: any) => {
     $('#GetOrderwiseOrderReport_List').DataTable().destroy();
     this.responsedata = result;
     this.GetOrderwiseOrderReport_List = this.responsedata.GetOrderwiseOrderReport_List;
     setTimeout(() => {
       $('#GetOrderwiseOrderReport_List');
     }, 1);
     this.NgxSpinnerService.hide()
 
   })
  
  
}

}

