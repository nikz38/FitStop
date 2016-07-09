var app = angular.module('fitstop', ['ui.router', 'ngSanitize']);

app.config(['$stateProvider', '$urlRouterProvider', '$httpProvider', '$locationProvider', function ($stateProvider, $urlRouterProvider, $httpProvider, $locationProvider) {
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
                //controller: 'LayoutController',
                templateUrl: '/views/reset-password.html',
            })
            .state('layout.dashboard', {
                url: '/dashboard',
                controller: 'DashboardController',
                templateUrl: '/views/dashboard.html',
            })
            .state('layout.ResetPasswordConfirmation', {
                url: '/reset-password/change-password/:hash',
                //controller: 'LayoutController',
                templateUrl: '/views/ResetPasswordConfirmation.html',
            });

    $locationProvider.html5Mode(true);

    $httpProvider.interceptors.push(['$injector', function ($injector) {
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
    }]);

}]);

app.run(['$rootScope', '$state', function ($rootScope, $state) {

    $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
        if (toState.pageName) {
            document.title = 'FitStop | ' + toState.pageName;
        }
    });

}]);

app.constant('config', {
    baseAddress: 'http://localhost:4857/api/'
});
app.controller('DashboardController', ['$scope', 'mealService', 'userService', function ($scope, mealService, userService) {

    $scope.user.getAllUsers();

    $scope.meal = {
        getMeal: function () {
            mealService.getMeal($scope.currentUser.id,
            successCallback = function (meal) {
                $scope.mealInfo = meal;
            });
        }
    };

    $scope.userActions = {
        deleteUser: function (id, index) {
            userService.deleteUser(id,
            successCallback = function () {
                $scope.allUsers.splice(index, 1);
            });
        }
    }

}]);
app.controller('LayoutController', ['$scope', '$state', 'userService', function ($scope, $state, userService) {

    $scope.currentUser = userService.currentUser;



    $scope.user = {
        email: 'software@enginee.rs',
        password: 'software',
        emailToReset: '',
        newPassword: '',
        hash: $state.params.hash || '',


        login: function () {
            userService.login($scope.user.email, $scope.user.password,
            successCallback = function () {
                $state.go('layout.dashboard');
            });
        },

        logout: function () {
            localStorage.clear();
            $scope.user.email = '';
            $scope.user.password = '';
            $state.go('layout.login');
        },

        getAllUsers: function () {
            userService.allUsers(
            successCallback = function (users) {
                $scope.allUsers = users;
                console.log($scope.allUsers);
            });
        },

        resetUserPassword: function (email) {
            userService.resetPassword(email);
        },

        setNewPassword: function (email, password, hash) {
            userService.setNewPassword(email, password, hash);
    }

    };
    console.log($scope.user.hash);
}]);
app.controller('LoginController', ['$scope', function ($scope) {

}]);
app.controller('RegisterController', ['$scope', 'userService', function ($scope, userService) {

    $scope.registerNewUser = function () {
        userService.register(
        $scope.register.firstName,
        $scope.register.lastName,
        $scope.register.address,
        $scope.register.email,
        $scope.register.phoneNumber,
        $scope.register.password,
        successCallback = function (user) {
            $scope.newUser = user;
            console.log($scope.newUser);
        });
    }

}]);
app.filter("correctedDate", function () {
    return function (date) {
        if (date) {
            return moment.utc(date).local().format("MM/DD/YYYY hh:mm A");
        }
    };


});
app.factory('mealService', ['$http', 'config', function ($http, config) {
    var factory = {};

    factory.getMeal = function (id, errorCallback) {
        return $http({
            url: config.baseAddress + 'meals/get/' + id,
            method: 'GET',
        })
        .then(function (response) {
            if (response.data) {
                successCallback(response.data);
            }
            else {

            }
        },
        function () {
            throw 'userService.login: Failure';
        });
    };

    return factory;
}]);
app.factory('userService', ['$http', 'config', function ($http, config) {
    var factory = {};

    factory.currentUser = {};

    function getCurrentUser() {
        if (localStorage.currentUser) {
            var userFromLS = JSON.parse(localStorage.currentUser);
            angular.copy(userFromLS, factory.currentUser);
        }
    }

    getCurrentUser();

    factory.login = function (username, password, successCallback) {
        return $http({
            url: config.baseAddress + 'users/login',
            method: 'POST',
            data: {
                email: username,
                password: password
            }
        })
        .then(function (response) {
            if (response.data) {
                localStorage.currentUser = JSON.stringify(response.data.user);
                localStorage.token = response.data.token;
                getCurrentUser();
                successCallback();
            }
            else {

            }
        },
        function () {
            throw 'userService.login: Failure';
        });
    };

    factory.register = function (firstName, lastName, address, email, phoneNumber, password, successCallback) {
        return $http({
            url: config.baseAddress + 'users/register',
            method: 'POST',
            data: {
                firstName: firstName,
                lastName: lastName,
                address: address,
                email: email,
                phoneNumber: phoneNumber,
                password: password
            }
        })
        .then(function (response) {
            if (response.data) {
                successCallback(response.data);
            }
            else {

            }
        },
        function () {
            throw 'userService.login: Failure';
        });
    };

    factory.allUsers = function (successCallback) {
        return $http({
            url: config.baseAddress + 'users/GetAll',
            method: 'GET',
        })
        .then(function (response) {
            if (response.data) {
                successCallback(response.data);
            }
            else {

            }
        },
        function () {
            throw 'userService.login: Failure';
        });
    };

    factory.deleteUser = function (id, successCallback) {
        return $http({
            url: config.baseAddress + 'users/delete/' + id,
            method: 'DELETE',
            data: {
                id: id,
            }
        })
        .then(function (response) {
            if (response.data) {
                successCallback();
            }
            else {

            }
        },
        function () {
            throw 'userService.login: Failure';
        });
    };

    factory.resetPassword = function (email) {
        return $http({
            url: config.baseAddress + 'users/ForgotPassword',
            method: 'POST',
            data: {
                email: email
            }
        })
        .then(function (response) {
            if (response.data) {
                console.log(response.data);
            }
            else {

            }
        },
        function (response) {
            throw response.data.message;
        });
    };

    factory.setNewPassword = function (email, password, hash) {
        return $http({
            url: config.baseAddress + 'users/SetNewPassword',
            method: 'PUT',
            data: {
                eMail: email,
                confirmHash: hash,
                newPassword: password
            }
        })
        .then(function (response) {
            if (response.data) {
                console.log(response.data);
            }
            else {

            }
        },
        function (response) {
            throw response.data.message;
        });
    };


    return factory;
}]);