app.factory('userService', function ($http, config) {
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
});