app.controller('DashboardController', function ($scope, mealService, userService) {

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

});