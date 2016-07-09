app.factory('mealService', function ($http, config) {
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
});