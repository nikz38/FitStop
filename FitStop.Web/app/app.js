var app = angular.module('fitstop', ['ui.router', 'ngSanitize']);

app.config(function ($stateProvider, $urlRouterProvider, $httpProvider, $locationProvider) {
    $urlRouterProvider.otherwise(function ($injector) {
        var $state = $injector.get('$state');
        $state.go('layout.login');
    });

    $stateProvider
        .state('layout', {
            controller: 'LayoutController',
            templateUrl: '/views/layout.html',
        })
            .state('layout.login', {
                url: '/login',
                controller: 'LoginController',
                templateUrl: '/views/login.html',
            })
            .state('layout.register', {
                url: '/register',
                controller: 'RegisterController',
                templateUrl: '/views/register.html',
            })
            .state('layout.reset-password', {
                url: '/reset-password',
                templateUrl: '/views/reset-password.html',
            })
            .state('layout.dashboard', {
                url: '/dashboard',
                controller: 'DashboardController',
                templateUrl: '/views/dashboard.html',
            })
            .state('layout.dashboard.users', {
                url: '/users',
                templateUrl: '/views/users.html',
            })
            .state('layout.dashboard.meals', {
                url: '/meals',
                templateUrl: '/views/meals.html',
            })
            .state('layout.dashboard.editProfile', {
                url: '/editProfile',
                templateUrl: '/views/edit-profile.html',
            })
            .state('layout.ResetPasswordConfirmation', {
                url: '/reset-password/change-password/:hash',
                templateUrl: '/views/ResetPasswordConfirmation.html',
            });

    $locationProvider.html5Mode(true);

    $httpProvider.interceptors.push(function ($injector) {
        return {
            request: function (config) {
                var token = localStorage.token;
                config.headers.Authorization = token;
                return config;
            },
            response: function (response) {
                return response;
            }
        };
    });

});

app.run(function ($rootScope, $state) {

    $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
        if (toState.pageName) {
            document.title = 'FitStop | ' + toState.pageName;
        }

        $rootScope.showStartNav = function () {
            if (toState.name == 'layout.login' || toState.name == 'layout.register') {
                return true;
            };
        }
    });

});

app.constant('config', {
    baseAddress: 'http://localhost:4857/api/'
});