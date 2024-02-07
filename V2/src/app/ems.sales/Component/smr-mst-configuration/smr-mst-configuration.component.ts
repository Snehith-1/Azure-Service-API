import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-smr-mst-configuration',
  templateUrl: './smr-mst-configuration.component.html',
  styleUrls: ['./smr-mst-configuration.component.scss']
})
export class SmrMstConfigurationComponent {

  salesconfigform: FormGroup;  

  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService, private router: ActivatedRoute, private route: Router) {

    this.salesconfigform = new FormGroup({
      addoncharges: new FormControl(false),
      additionaldiscount: new FormControl(false),
      freightcharges: new FormControl(false),
      packing_forwardingcharges: new FormControl(false),
      insurancecharges: new FormControl(false)
    })
  }

  addonchargestoggle() {
    var api = 'SmrMstSalesConfig/UpdateAddOnChargesConfig';
    this.service.post(api, this.salesconfigform.value).subscribe((result: any) => {
      if(result.status == true) {
        this.ToastrService.success(result.message)
      }
      else {
        this.ToastrService.warning(result.message)
      }
    });
  }

  additionaldiscounttoggle() {
    var api = 'SmrMstSalesConfig/UpdateAdditionalDiscountConfig';
    this.service.post(api, this.salesconfigform.value).subscribe((result: any) => {
      if(result.status == true) {
        this.ToastrService.success(result.message)
      }
      else {
        this.ToastrService.warning(result.message)
      }
    });
  }

  freightchargestoggle() {
    var api = 'SmrMstSalesConfig/UpdateFreightChargesConfig';
    this.service.post(api, this.salesconfigform.value).subscribe((result: any) => {
      if(result.status == true) {
        this.ToastrService.success(result.message)
      }
      else {
        this.ToastrService.warning(result.message)
      }
    });
  }

  packing_forwardingchargestoggle() {
    var api = 'SmrMstSalesConfig/UpdatePacking_ForwardingChargesConfig';
    this.service.post(api, this.salesconfigform.value).subscribe((result: any) => {
      if(result.status == true) {
        this.ToastrService.success(result.message)
      }
      else {
        this.ToastrService.warning(result.message)
      }
    });
  }

  insurancechargestoggle() {
    var api = 'SmrMstSalesConfig/UpdateInsuranceChargesConfig';
    this.service.post(api, this.salesconfigform.value).subscribe((result: any) => {
      if(result.status == true) {
        this.ToastrService.success(result.message)
      }
      else {
        this.ToastrService.warning(result.message)
      }
    });
  }
}
