
import { Component, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import * as ApexCharts from 'apexcharts';
import { ApexOptions } from 'apexcharts';
import {
  ApexNonAxisChartSeries,
  ApexResponsive,
  ApexChart,
  ChartComponent 
} from "ng-apexcharts";
export type ChartOptions1 = {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  responsive: ApexResponsive[];
  labels: any;
  DashboardCount_List: any
};
export type ChartOptions4 = {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  responsive: ApexResponsive[];
  labels: any;
  DashboardCount_List: any
};

@Component({
  selector: 'app-smr-dashboard',
  templateUrl: './smr-dashboard.component.html',
  styleUrls: ['./smr-dashboard.component.scss']
})

export class SmrDashboardComponent {
  chartOptions1: any = {};
  chartOptions!: ApexOptions; 
  chartOptions4: any = {};
 
  noleadstatus: any;
  response_data :any;

  GetOverallSalesOrderChart_List :any;

  DashboardQuotationAmt_List :any;

  noquotation :any;

  year : any;
  
  noquotation_status : any;

  show = true;
  emptyFlag: boolean=false;
  series_Value: any;
  labels_value: any;
  chartOptions2: any = {};
  GetOrderForLastSixMonths_List: any;
  GetOrderForLastSixMonths_List1: any;
  GetMonthSalesReportCount_list: any;
  GetDaySalesReportCount_List: any;
  GetWeekSalesReportCount_List: any;
  GetMonthSalesReportCount_List: any;
  GetSalesOrderCount_List: any[] = [];
  total_so: any;
  responsedata: any;
 // chartOptions: any;
  parameter: any;
  parameterValue: any;
  Date: string;
  getData: any;
  total_payment: any;
  todaytask_count: any;
  pending_So: any;
  approved_so: any;
  advanced_paid: any;
  rejected_so: any;
  invoice_raised: any
  totalinvoice: any;
  delivery_done_partial: any;
  delivery_completed: any;
  approvalpendinginnvoice: any;
  payment_pending: any;
  approval_pending: any;
  quotation_canceled: any;
  quotation_status: any;
  total_quotation: any;
  completed_quotation: any;
  total_quotation1: number = 0;
  quotation_canceled1: number = 0;
  so_amended: any;
  quotation_amended: any;
  work_in_progress: any;
  payment_don_partial: any;
  compelted_payment: any;
  today_total_so: any;
  today_total_do: any;
  today_total_invoice: any;
  today_total_payment: any;
  today_invoice_amount: any;
  today_payment_amount: any;
  today_outstanding_amount: any;
  yest_total_so: any;
  yest_total_do: any;
  yest_total_invoice: any;
  yest_total_payment: any;
  yest_invoice_amount: any;
  yest_payment_amount: any;
  yest_outstanding_amount: any;
  cw_total_so: any;
  cw_total_do: any;
  cw_total_invoice: any;
  cw_total_payment: any;
  cw_invoice_amount: any;
  cw_payment_amount: any;
  cw_outstanding_amount: any;
  lw_total_so: any;
  lw_total_do: any;
  lw_total_invoice: any;
  lw_total_payment: any;
  lw_invoice_amount: any;
  lw_payment_amount: any;
  lw_outstanding_amount: any;
  cm_total_so: any;
  cm_total_do: any;
  cm_total_invoice: any;
  cm_total_payment: any;
  cm_invoice_amount: any;
  cm_payment_amount: any;
  cm_outstanding_amount: any;
  lm_total_so: any;
  lm_total_do: any;
  lm_total_invoice: any;
  lm_total_payment: any;
  lm_invoice_amount: any;
  lm_payment_amount: any;
  lm_outstanding_amount: any;
  cy_total_so: any;
  cy_total_do: any;
  cy_total_invoice: any;
  cy_total_payment: any;
  cy_invoice_amount: any;
  cy_payment_amount: any;
  cy_outstanding_amount: any;
  ly_total_so: any;
  ly_total_do: any;
  ly_total_invoice: any;
  ly_total_payment: any;
  ly_invoice_amount: any;
  ly_payment_amount: any;
  ly_outstanding_amount: any;
  mtd_over_due_payment: any;
  mtd_over_due_payment_amount: any;
  mtd_over_due_invoice_amount: any;
  mtd_over_due_invoice: any;
  ytd_over_due_payment: any;
  ytd_over_due_payment_amount: any;
  ytd_over_due_invoice_amount: any;
  ytd_over_due_invoice: any;
  mtd_invoice: any;
  mtd_payment: any;
  ytd_invoice: any;
  ytd_payment: any;
  countOwnValues: any;
 
  paymentDay: string[] = [];
  amount: number[] = [];
  monthlySalesData: any;
  invoice_count: any;
  today_invoice: any;
  aproved_invoice: any;
  pending_invoice: any;
  total_do: any;
  pending_do: any;
  completed_do: any;
  Partial_done: any;
  totalpayment: any;
  payment_completed: any;

 
  constructor(private router: Router, private service: SocketService) {
    this.Date = new Date().toString();
    SmrDashboardComponent.constructor(); {
      setInterval(() => {
        this.Date = new Date().toString();
      }, 1000);

    }
    var url = 'SmrDashboard/GetSalesOrderCount';
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
  
      console.log(this.GetSalesOrderCount_List)
      this.GetSalesOrderCount_List = this.responsedata.GetSalesOrderCount_List;
      this.total_quotation = this.GetSalesOrderCount_List[0].total_quotation;
      this.quotation_canceled = this.GetSalesOrderCount_List[0].quotation_canceled;
      this.quotation_amended = this.GetSalesOrderCount_List[0].quotation_canceled;
      this.total_quotation1 = this.total_quotation;
      this.quotation_canceled1 = this.quotation_canceled;
      this.completed_quotation = this.total_quotation1 - this.quotation_canceled;
      this.total_so = this.GetSalesOrderCount_List[0].total_so
      this.approved_so = this.GetSalesOrderCount_List[0].approved_so
      this.pending_So = this.GetSalesOrderCount_List[0].pending_So
      this.invoice_count = this.GetSalesOrderCount_List[0].invoice_count
      this.so_amended = this.GetSalesOrderCount_List[0].so_amended
      this.totalinvoice = this.GetSalesOrderCount_List[0].totalinvoice
      this.invoice_raised = this.GetSalesOrderCount_List[0].invoice_raised
      this.invoice_raised = this.GetSalesOrderCount_List[0].invoice_raised
      this.delivery_completed = this.GetSalesOrderCount_List[0].delivery_completed
      this.total_do = this.GetSalesOrderCount_List[0].total_do
      this.rejected_so = this.GetSalesOrderCount_List[0].rejected_so
      this.pending_do = this.GetSalesOrderCount_List[0].pending_do
      this.today_invoice = this.GetSalesOrderCount_List[0].today_invoice
      this.completed_do = this.GetSalesOrderCount_List[0].completed_do
      this.Partial_done = this.GetSalesOrderCount_List[0].Partial_done
      this.payment_pending = this.GetSalesOrderCount_List[0].payment_pending
      this.totalpayment = this.GetSalesOrderCount_List[0].totalpayment
      this.payment_don_partial = this.GetSalesOrderCount_List[0].payment_don_partial
      this.payment_completed = this.GetSalesOrderCount_List[0].payment_completed
      this.aproved_invoice = this.GetSalesOrderCount_List[0].aproved_invoice
      this.pending_invoice = this.GetSalesOrderCount_List[0].pending_invoice
      console.log(this.GetSalesOrderCount_List, 'testdata');
    });

  }

  ngOnInit() {
    this.getMonthlySalesChart();
    var api = 'SmrDashboard/GetOwnOverallSalesOrderChart';
    
    this.service.get(api).subscribe((result: any) => {
      this.response_data = result;
      
      this.GetOverallSalesOrderChart_List = this.response_data.GetOverallSalesOrderChart_List; 
      if(this.GetOverallSalesOrderChart_List==null){
        this.series_Value= [0]; 
        this.chartOptions1 = {
         series: this.series_Value,
         chart: {
           width: 390,
           type: "pie", // Use donut type to create a hole in the center
         },
         // plotOptions: {
         //   pie: {
         //     customScale: 0.8,
         //   },
         // },
         plotOptions: {
           pie: {
             customScale: 0.8,
             dataLabels: {
               offset: -5, // Adjust the offset for better positioning
               minAngleToShowLabel: 10, // Set a minimum angle to show labels
               enabled: true,
             },
           },
         },
         responsive: [
           {
             breakpoint: 300,
             options: {
               chart: {
                 width: 200,
               },
               legend: {
                 position: "top",
               },
             },
           },
         ],
         fill: {
           colors: ['#3498db'], // Set the color for the center label
         },
         labels: ['There is No Sales Status Found'], // Set the label for the center
         legend: {
           show: false,
         },
         dataLabels: {
           enabled: false, // Disable data labels for percentage
         },
   
         
       };
         }
         else{
      const totalCount = this.GetOverallSalesOrderChart_List.reduce((acc: number, item: { count_own: string; }) => acc + parseInt(item.count_own), 0);
      this.chartOptions1.series = this.GetOverallSalesOrderChart_List.map((item: { count_own: any; }) =>  (parseInt(item.count_own) / totalCount) * 100);
      this.chartOptions1.labels = this.GetOverallSalesOrderChart_List.map((item: {
        count_own: any; salesorder_status_own: any; 
}) => `${item.salesorder_status_own}: ${item.count_own}`);
    }
  });
    var api = 'SmrDashboard/GetOwnOverallDeliveryOrderChart';
    
    this.service.get(api).subscribe((result: any) => {
      this.response_data = result;
      
      this.GetOverallSalesOrderChart_List = this.response_data.GetOverallSalesOrderChart_List; 
      if(this.GetOverallSalesOrderChart_List==null){
        this.series_Value= [0]; 
        this.chartOptions4 = {
         series: this.series_Value,
         chart: {
           width: 390,
           type: "pie", // Use donut type to create a hole in the center
         },
         // plotOptions: {
         //   pie: {
         //     customScale: 0.8,
         //   },
         // },
         plotOptions: {
           pie: {
             customScale: 0.8,
             dataLabels: {
               offset: -5, // Adjust the offset for better positioning
               minAngleToShowLabel: 10, // Set a minimum angle to show labels
               enabled: true,
             },
           },
         },
         responsive: [
           {
             breakpoint:300,
             options: {
               chart: {
                 width: 200,
               },
               legend: {
                 position: "top",
               },
             },
           },
         ],
         fill: {
           colors: ['#3498db'], // Set the color for the center label
         },
         labels: ['There is No Delivery Status Found'], // Set the label for the center
         legend: {
           show: false,
         },
         dataLabels: {
           enabled: false, // Disable data labels for percentage
         },
   
         
       };
         }
         else{
      
      const totalCount = this.GetOverallSalesOrderChart_List.reduce((acc: number, item: { do_count_own: string; }) => acc + parseInt(item.do_count_own), 0);
      this.chartOptions4.series = this.GetOverallSalesOrderChart_List.map((item: { do_count_own: any; }) =>  (parseInt(item.do_count_own) / totalCount) * 100);
      this.chartOptions4.labels = this.GetOverallSalesOrderChart_List.map((item: {
        do_count_own: any; delivery_status_own: any; 
}) => `${item.delivery_status_own}: ${item.do_count_own}`);
         }
    });
   // this.chartOptions = getChartOptions(350);
    this.chartOptions1 = getChartOptions1(390);
    this.chartOptions4 = getChartOptions4(390);
    //this.chartOptions2 = getChartOptions2(380);
    this.GetDaySummary();
    this.GetWeekSummary();
    this.GetMonthSummary();
    this.GetYearSummary();
    this.GetYTDCounts();
    this.GetMTDCounts();

    setInterval(() => {
      this.Date = new Date().toString();
    }, 1000);
  }
  GetYearSummary() {
    var url = 'SmrRptTodaysSalesReport/GetYearSalesReportCount'
    this.service.get(url).subscribe((result: any) => {
      console.log(result);
      this.cy_total_so = result.cy_total_so;
      this.cy_total_do = result.cy_total_do;
      this.cy_total_invoice = result.cy_total_invoice;
      this.cy_total_payment = result.cy_total_payment;
      this.cy_invoice_amount = result.cy_invoice_amount;
      this.cy_payment_amount = result.cy_payment_amount;
      this.cy_outstanding_amount = result.cy_outstanding_amount;
      this.ly_total_so = result.ly_total_so;
      this.ly_total_do = result.ly_total_do;
      this.ly_total_invoice = result.ly_total_invoice;
      this.ly_total_payment = result.ly_total_payment;
      this.ly_invoice_amount = result.ly_invoice_amount;
      this.ly_payment_amount = result.ly_payment_amount;
      this.ly_outstanding_amount = result.ly_outstanding_amount;

    })
  }
  GetMonthSummary() {
    var url = 'SmrRptTodaysSalesReport/GetMonthSalesReportCount'
    this.service.get(url).subscribe((result: any) => {
      console.log(result);
      this.cm_total_so = result.cm_total_so;
      this.cm_total_do = result.cm_total_do;
      this.cm_total_invoice = result.cm_total_invoice;
      this.cm_total_payment = result.cm_total_payment;
      this.cm_invoice_amount = result.cm_invoice_amount;
      this.cm_payment_amount = result.cm_payment_amount;
      this.cm_outstanding_amount = result.cm_outstanding_amount;
      this.lm_total_so = result.lm_total_so;
      this.lm_total_do = result.lm_total_do;
      this.lm_total_invoice = result.lm_total_invoice;
      this.lm_total_payment = result.lm_total_payment;
      this.lm_invoice_amount = result.lm_invoice_amount;
      this.lm_payment_amount = result.lm_payment_amount;
      this.lm_outstanding_amount = result.lm_outstanding_amount;

    })
  }
  GetWeekSummary() {
    var url = 'SmrRptTodaysSalesReport/GetWeekSalesReportCount'
    this.service.get(url).subscribe((result: any) => {
      console.log(result);
      this.cw_total_so = result.cw_total_so;
      this.cw_total_do = result.cw_total_do;
      this.cw_total_invoice = result.cw_total_invoice;
      this.cw_total_payment = result.cw_total_payment;
      this.cw_invoice_amount = result.cw_invoice_amount;
      this.cw_payment_amount = result.cw_payment_amount;
      this.cw_outstanding_amount = result.cw_outstanding_amount;
      this.lw_total_so = result.lw_total_so;
      this.lw_total_do = result.lw_total_do;
      this.lw_total_invoice = result.lw_total_invoice;
      this.lw_total_payment = result.lw_total_payment;
      this.lw_invoice_amount = result.lw_invoice_amount;
      this.lw_payment_amount = result.lw_payment_amount;
      this.lw_outstanding_amount = result.lw_outstanding_amount;

    })
  }
  GetDaySummary() {

    var url = 'SmrRptTodaysSalesReport/GetDaySalesReportCount'
    this.service.get(url).subscribe((result: any) => {
      console.log(result);
      this.today_total_so = result.today_total_so;
      this.today_total_do = result.today_total_do;
      this.today_total_invoice = result.today_total_invoice;
      this.today_total_payment = result.today_total_payment;
      this.today_invoice_amount = result.today_invoice_amount;
      this.today_payment_amount = result.today_payment_amount;
      this.today_outstanding_amount = result.today_outstanding_amount;
      this.yest_total_so = result.yest_total_so;
      this.yest_total_do = result.yest_total_do;
      this.yest_total_invoice = result.yest_total_invoice;
      this.yest_total_payment = result.yest_total_payment;
      this.yest_invoice_amount = result.yest_invoice_amount;
      this.yest_payment_amount = result.yest_payment_amount;
      this.yest_outstanding_amount = result.yest_outstanding_amount;

    })
  }
  GetMTDCounts() {

    var url = 'SmrDashboard/GetMTDCounts'
    this.service.get(url).subscribe((result: any) => {
      console.log(result);

      this.mtd_invoice = result.mtd_over_due_invoice + ' / ' + result.mtd_over_due_invoice_amount;

      this.mtd_payment = + result.mtd_over_due_payment_amount + ' / ' +result.mtd_over_due_payment;

    })
  }

  GetYTDCounts() {

    var url = 'SmrDashboard/GetYTDCounts'
    this.service.get(url).subscribe((result: any) => {
      console.log(result);

      this.ytd_invoice = result.ytd_over_due_invoice_amount + ' / ' +result.ytd_over_due_invoice ;
      this.ytd_payment = result.ytd_over_due_payment + ' / ' + result.ytd_over_due_payment_amount;
    })
  }
  getMonthlySalesChart() {

    var url = 'SmrDashboard/GetMonthlySalesChart'
    this.service.get(url).subscribe((result: any) => {
      debugger;
      this.response_data = result;
      
      this.GetOverallSalesOrderChart_List = this.response_data.GetSalesPerformanceChart_List; 
      const categories = this.GetOverallSalesOrderChart_List.map((entry: { payment_day: any; }) => entry.payment_day);
      const data = this.GetOverallSalesOrderChart_List.map((entry: { amount: any; }) => entry.amount);
console.log(categories)
console.log(data)
      // Initialize chart options
      this.chartOptions = {
        chart: {
          type: 'bar',
          height: 400, // Adjust the height of the chart as needed
          width:600,
          
          background: 'White',
          foreColor: '#0F0F0F',
          toolbar: {
            show: false, // Set to false to hide the toolbar/menu icon
          },
        },
        colors: ['#8062D6', '#FFD54F', '#66BB6A', '#EF5350'], // Use a set of colors for better combinations
        plotOptions: {
          bar: {
            horizontal: false,
            columnWidth: '60%', // Adjust the width of the bars
           
            borderRadius: 1, // Add some border radius for a more modern look
            
          },
        },
        dataLabels: {
          enabled: false, // Disable data labels for a cleaner look
        },
        stroke: {
          show: true,
          width: 15,
          
          colors: ['transparent'],
        },
        xaxis: {
          categories: categories,
          labels: {
            style: {
              
              fontWeight: 'bold',
              fontSize: '14px',
              
              
              //colors: ['#FF5733', '#33FF57', '#5733FF', '#FFFF33'], // Set different colors for each label
            },
          },
        },
        yaxis: {
          title: {
            text: 'Sales Amount',
            style: {
              fontWeight: 'bold',
              fontSize: '14px',
              color: '#0F0F0F', // Set a different color for the y-axis title
            },
          },
        },
        tooltip: {
          y: {
            formatter: function (val: any) {
              return val.toFixed(2); // Format tooltip values as needed
            },
          },
        },
        series: [
          {
            name: 'Paid Amount',
            data: data,
            // data: data.map((value: any, index: number) => ({
            //   y: value,
            //   color: ['#64B5F6', '#FFD54F', '#66BB6A', '#EF5350'][index % 4], // Use the colors you want
            // })),
          },
        ],
      };
      //end
      // this.chartOptions = {
      //   chart: {
      //     type: 'bar',
      //   },
      //   xaxis: {
      //     categories: categories,
      //   },
      //   series: [
      //     {
      //       name: 'Monthly Sales',
      //       data: data,
      //     },
      //   ],
      // };

      // Render the chart
      var chart = new ApexCharts(document.getElementById('chart'), this.chartOptions);
      chart.render();

      
    })
  }
}

function getChartOptions1(height: number) {
  const commonWidth = 436;
  return {
   // series: [47, 25, 38, 39],
    chart: {
      width: commonWidth,
      type: "pie"
    },
   // labels: ["Item1", "Item1 2", "Item1 3", "Item1 4"],
    // responsive: [
    //   {
    //     breakpoint:commonWidth,
    //     options: {
    //       chart: {
    //         width: '100%',
    //       },
    //       legend: {
    //         position: "center"
    //       }

    //     }
    //   }
    // ]
  };
}
function getChartOptions4(height: number) {
  const commonWidth = 418;
  return {
   // series: [47, 25, 38, 39],
    chart: {
      width: commonWidth,
      type: "pie"
    },
   // labels: ["Item1", "Item1 2", "Item1 3", "Item1 4"],
    // responsive: [
    //   {
    //     breakpoint:commonWidth,
    //     options: {
    //       chart: {
    //         width: '100%',
    //       },
    //       legend: {
    //         position: "center"
    //       }

    //     }
    //   }
    // ]
  };
}





