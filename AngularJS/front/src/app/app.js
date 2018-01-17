import angular from 'angular';
import ngMessages from 'angular-messages';
import angularMoment from 'angular-moment';
import uiBootstrap from 'angular-ui-bootstrap';
import modal from 'angular-ui-bootstrap/src/modal/index-nocss'
import uiRouter from 'angular-ui-router';

import 'bootstrap/dist/css/bootstrap.min.css';
import '../style/app.css';

import routesConfig from './app.routes';

import CalendarComponent from './components/calendar/calendar.component';
import WeekComponent from './components/week/week.component';
import DayComponent from './components/day/day.component';
import ModalComponent from './components/modal/modal.component';

const MODULE_NAME = 'app';

console.log('modal', modal)

angular.module(MODULE_NAME, [
  ngMessages,
  'ui.bootstrap',
  'angularMoment',
  uiBootstrap,
  uiRouter,
  modal
])
  .config(routesConfig)
  .component('calendar', CalendarComponent)
  .component('calWeek', WeekComponent)
  .component('calDay', DayComponent)
  .component('calModal', ModalComponent);

export default MODULE_NAME;