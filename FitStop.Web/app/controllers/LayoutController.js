app.controller('LayoutController', function ($scope, $state, userService) {

    $scope.currentUser = userService.currentUser;

    $scope.user = {
        email: 'software@enginee.rs',
        password: 'software',
        emailToReset: '',

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
        }

    };

});