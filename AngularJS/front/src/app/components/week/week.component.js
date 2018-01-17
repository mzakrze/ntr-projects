import controller from './week.controller';
import './week.css';

export default {
    template: require('./week.template.html'),
    controller,
    bindings: {
        week: '<'
    }
};