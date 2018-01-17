import controller from './day.controller';
import './day.css';

export default {
    template: require('./day.template.html'),
    controller,
    bindings: {
        day: '<'
    }
};