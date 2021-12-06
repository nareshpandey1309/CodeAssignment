import {Component, OnInit} from '@angular/core';
import {ApiService} from './api.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent {
  title = 'ClientApp';
  sampleContent = null;

  constructor(private api:ApiService) {}

  ngOnInit() {
    this.api.getTestData().subscribe((data)=>{
      this.sampleContent = data;
     
      console.log(this.sampleContent[0].id);
      console.log(this.sampleContent[0].physicalData.yearBuilt);
    });
  }

  SaveRecord() {

    alert("Record saved successfully.!");

  }

}


