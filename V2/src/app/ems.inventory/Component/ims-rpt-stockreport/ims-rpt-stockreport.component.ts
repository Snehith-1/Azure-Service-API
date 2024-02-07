import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators,} from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
interface Istockreport {
 
  branch_gid: string;
  branch_name: any;

}
@Component({
  selector: 'app-ims-rpt-stockreport',
  templateUrl: './ims-rpt-stockreport.component.html',
  styleUrls: ['./ims-rpt-stockreport.component.scss']
})
export class ImsRptStockreportComponent {
  stockreport_list: any[] = [];
  responsedata: any;
  getData: any;
  branch_list :any;
  mdlBranchName:any;
  reactiveform: FormGroup | any;
  stockreport: Istockreport;
  combinedFormData: any;
  constructor(private formBuilder: FormBuilder,public NgxSpinnerService: NgxSpinnerService,public service :SocketService,private route:Router,private ToastrService: ToastrService) {
    this.stockreport = {} as Istockreport;
    
}

  ngOnInit(): void {debugger
    this.GetImsRptStockreport();
    this.reactiveform = new FormGroup({
      branch_name: new FormControl(''),

    })
    
    var url = 'ImsRptStockreport/GetBranch'
    this.service.get(url).subscribe((result: any) => {
      this.branch_list = result.branch_list;  
    });
  
}

// // //// Summary Grid//////
GetImsRptStockreport() {
  debugger

 
 
}

OnChangeBranch(branch_name :any) {
  debugger;
  const branch_gid =branch_name.branch_gid;
  let param = {
    branch_gid: branch_gid
  }
  var url = 'ImsRptStockreport/GetImsRptStockreport'
  this.NgxSpinnerService.show()
  this.service.getparams(url,param).subscribe((result: any) => {
    $('#stockreport_list').DataTable().destroy();
    this.responsedata = result;
    this.stockreport_list = this.responsedata.stockreport_list;
    this.combinedFormData.get("display_field")?.setValue(this.stockreport_list[0].display_field);
    setTimeout(() => {
      $('#stockreport_list').DataTable();
            }, 1);
             
    })
    this.NgxSpinnerService.hide();
 
}
openModaldelete(){

}
}
