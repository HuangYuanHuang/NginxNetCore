import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { HubConnection } from "@aspnet/signalr-client";

@Component({
    selector: 'fetchdata',
    templateUrl: './fetchdata.component.html'
})
export class FetchDataComponent {
    public forecasts: SignalrNode[]=[];
    private hub: HubConnection;
    signalrlog: string = "Connecting push server ....";
    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {

        this.inistSignalr();
        //http.get(baseUrl + 'api/SampleData/WeatherForecasts').subscribe(result => {
        //    this.forecasts = result.json() as WeatherForecast[];
        //}, error => console.error(error));
    }

    inistSignalr() {
        this.hub = new HubConnection('/messageHub');
        this.hub.on("pushMessage", (time, message, id) => {
            console.log(message);
            this.forecasts.unshift({ time: time, task: id, text: message });
        });
        this.hub.start().then(d => {
            this.signalrlog = 'Successful connection to the push server ';
        });
    }
}
interface SignalrNode {
    time: string;
    task: number;

    text: string;
}

interface WeatherForecast {
    dateFormatted: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}
