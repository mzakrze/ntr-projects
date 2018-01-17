import moment from 'moment';

export default class CalendarController {
    constructor($state, $stateParams) {
        'ngInject';

        this.$state = $state;
        this.$stateParams = $stateParams;

        this.weekDays = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];
    }

    prepareDate(days) {
        let date;
        if(this.$stateParams.date) {
            date = moment(this.$stateParams.date);
        } else {
            date = moment();
        }
        return date.add(days, 'days').format('YYYY-MM-DD');
    }

    go(days) {
        this.$state.go( 'calendar', { date: this.prepareDate(days) });
    }

}
