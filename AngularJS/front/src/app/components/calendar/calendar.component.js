import controller from './calendar.controller';
import './calendar.css';

export default {
    template: require('./calendar.template.html'),
    controller,
    bindings: {
        weeks: '<'
    }
};