import controller from './modal.controller';
import './modal.css';

export default {
    template: require('./modal.template.html'),
    controller,
    bindings: {
        appointment: '<',
        date: '<',
        resolve: '<',
        close: '&',
        dismiss: '&'
    }
};
