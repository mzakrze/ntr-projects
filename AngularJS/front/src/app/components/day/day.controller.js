import moment from 'moment';

export default class DayController {
    constructor($uibModal, $state) {
        'ngInject';
        
        this.$uibModal = $uibModal;
        this.$state = $state;
    }

    addAppointment() {
        this.openModal();
    }

    editAppointment(appointment) {
        this.openModal(appointment);
    }

    openModal(appointment) {
        this.$uibModal.open({
            component: 'calModal',
            resolve: {
                appointment: () => { return angular.copy(appointment) },
                date: () => { return this.day.Date }
            }
        })
        .result.then( appointment => {
            this.$state.reload();
        });
    }

}
