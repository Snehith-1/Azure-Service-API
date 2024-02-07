import { TestBed } from '@angular/core/testing';

import { DistrictserviceService } from './districtservice.service';

describe('DistrictserviceService', () => {
  let service: DistrictserviceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DistrictserviceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
