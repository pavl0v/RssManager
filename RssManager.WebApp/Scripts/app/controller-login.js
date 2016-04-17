(function () {
    var app = angular.module("app");
    app.controller("controllerLogin", ["$scope", "$http", "$window", "$location", "factoryDialogs", "factoryUsers", "factoryCommon",
        function ($scope, $http, $window, $location, factoryDialogs, factoryUsers, factoryCommon) {

            $scope.model = {
                Username: "",
                Password: ""
            };

            // INTERFACE
            $scope.login = onLogin;
            $scope.showDialogSignUp = function (ev) {
                var args = {
                    dialogHeader: "Sign Up",
                    usr: {
                        Id: "0",
                        FirstName: "",
                        LastName: "",
                        Username: "",
                        Password: ""
                    },
                    isCaptchaInvalid: true
                };

                factoryDialogs.dlgCustom(
                    $scope,
                    ev,
                    "Html/dialog-sign-up.html",
                    args,
                    function (answer) { onPostUser(answer); },
                    function () { /* console.log("Sign Up cancelled"); */ }
                );
            }

            /**
             * Create new user account
             */
            function onPostUser(user) {
                factoryUsers.postUser(user).then(
                    function () { factoryDialogs.dlgAlert(null, "DONE", "Account created successfuly"); },
                    function (error) { factoryDialogs.dlgError(null, null, error); }
                );
            }

            /**
             * User log in
             */
            function onLogin(model) {
                var url = factoryCommon.getApiBaseUrl() + "/token";
                var data = "grant_type=password&username=" + model.Username + "&password=" + model.Password;
                var config = {
                    headers: {
                        "Content-Type": "application/x-www-form-urlencoded"
                    }
                };
                $http.post(url, data, config).then(
                    function (response) {
                        $window.sessionStorage.setItem("RssManagerToken", response.data.access_token);
                        $window.sessionStorage.setItem("RssManagerUser", model.Username);
                        $location.path("/home");
                        $scope.$parent.backendHub.server.subscribe($window.sessionStorage.getItem("RssManagerUser"));
                            //.done(function () { console.log("backendHub.subscribe OK"); })
                            //.fail(function () { console.log("backendHub.subscribe ERROR"); });
                    },
                    function (error) { factoryDialogs.dlgError(null, null, error); }
                );
            }

        }]);
})();