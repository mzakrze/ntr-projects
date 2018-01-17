import moment from 'moment';

export default class ModalController {
    constructor($http, $state) {
        'ngInject';

        this.$http = $http;
        this.$state = $state;
        this.api = '/api/appointments/';
    }

    $onInit() {
        this.appointment = this.resolve.appointment;
        this.date = this.resolve.date;

        this.errors = [];
        if(this.appointment) {
            this.mode = 'Edit';
            this.initAppointment();
        } else {
            this.mode = 'Add new';
            this.appointment = {
                Title: '',
                Description: '',
                AppointmentDate: this.date,
            };
        }
    }

    initAppointment(){
        const startTime = this.appointment.StartTime.split(':');
        this.appointment.StartTime = new Date()
        this.appointment.StartTime.setHours(startTime[0]);
        this.appointment.StartTime.setMinutes(startTime[1]);

        const endTime = this.appointment.EndTime.split(':');
        this.appointment.EndTime = new Date();
        this.appointment.EndTime.setHours(endTime[0]);
        this.appointment.EndTime.setMinutes(endTime[1]);
    }

    save(force) {
        this.validate();
        if(this.errors.length !== 0 || Object.keys(this.form.$error).length > 0) {
            return;
        }
        const appointment = {
            Title: this.appointment.Title,
            Description: this.appointment.Description,
            AppointmentDate: this.appointment.AppointmentDate,
            StartTime: moment(this.appointment.StartTime).format('HH:mm'),
            EndTime: moment(this.appointment.EndTime).format('HH:mm'),
            timestamp: this.appointment.timestamp
        };
        
        if(this.appointment.AppointmentID) {
            // edit
            this.$http.put(this.api + this.appointment.AppointmentID, appointment)
            .then( response => {
                if(!response.data.Success){
                    this.appointment = response.data.Object;
                    debugger;
                    if(this.appointment == null){
                        alert('sorry, someone else deleted this appointment');
                        this.dismiss();
                        return;
                    }
                    this.initAppointment();
                    this.errors = ['Appointment has been edited by someone else. New appointment has been loaded.'];
                } else {
                    this.close(response.data);
                }
            });
        } else {
            // add
            this.$http.post(this.api, appointment)
            .then( response => {
                this.close(response.data);
            });
        }
    }

    remove() {
        this.$http.delete(this.api + this.appointment.AppointmentID)
        .then( () => {
            this.close();
        });
    }

    validate() {
        this.errors = [];
        this.form.Title.$touched = true;
        this.form.Description.$touched = true;
        if(this.appointment.StartTime >= this.appointment.EndTime){
            this.errors.push('End of the appointment is before the start time');
        }
    }

    cancel() {
        this.dismiss();
    }
}
