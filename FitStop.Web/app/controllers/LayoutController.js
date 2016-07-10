app.controller('LayoutController', function ($scope, $state, userService, $rootScope) {

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
                if ($scope.currentUser.role != "AppUser") {
                    $state.go('layout.dashboard.users');
                }
                else {
                    $state.go('layout.dashboard.meals');

                }
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


});