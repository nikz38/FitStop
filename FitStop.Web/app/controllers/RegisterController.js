app.controller('RegisterController', function ($scope, userService) {

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

});