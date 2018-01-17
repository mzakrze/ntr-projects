import moment from 'moment';

export default class WeekController {
    constructor() {
        'ngInject';
    }

    $onInit() {
        this.currentWeek = moment().week() === this.week.No;
    }

}
