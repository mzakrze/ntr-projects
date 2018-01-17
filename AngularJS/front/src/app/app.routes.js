export default function routesConfig($stateProvider, $locationProvider) {
    'ngInject';

    $locationProvider.html5Mode({enabled: true});

    $stateProvider.state('calendar', {
        url: '/?date',
        component: 'calendar',
        resolve: {
            weeks: ($stateParams, $http, $q) => {
                'ngInject';
                const deferred = $q.defer();
                $http.get('/api/weeks/', { params: { date: $stateParams.date } })
                    .then( response => {
                        deferred.resolve(response.data);
                    })
                return deferred.promise; 
            }
        }
    })
}