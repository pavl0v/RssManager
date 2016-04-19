(function () {
    var app = angular.module("app");
    app.controller("controllerMainMenu", ["$scope", "$rootScope", "$window", "$location", "factoryRssChannels", "factoryCommon", "factoryDialogs", "factoryUsers", "factoryAuth",
        function ($scope, $rootScope, $window, $location, factoryRssChannels, factoryCommon, factoryDialogs, factoryUsers, factoryAuth) {

            /**
             * Dialog for add new RSS channel
             */
            $scope.showDialogAddChannel = function (ev) {
                var args = {
                    dialogHeader: "Add RSS Channel",
                    channel: {
                        "Id": "0",
                        "Copyright": "",
                        "Description": "",
                        "Language": "",
                        "Name": "",
                        "Title": "",
                        "Url": ""
                    },
                    isDisabledOK: false,
                    isDisabledURL: false
                };

                factoryDialogs.dlgCustom(
                    $scope,
                    ev,
                    "Html/dialog-channel.html",
                    args,
                    function (answer) { onAdd(answer); },
                    function () { /* console.log("Channel deletion cancelled"); */ });
            }

            $scope.showDialogPassword = function (ev) {
                var args = {
                    dialogHeader: "Change Password",
                    pwd: {
                        "CurrentPassword": "",
                        "NewPassword": "",
                        "RepeatNewPassword": ""
                    },
                    isCaptchaInvalid: false,
                    isDisabledOK: false,
                    isDisabledURL: false
                };

                factoryDialogs.dlgCustom(
                    $scope,
                    ev,
                    "Html/dialog-password.html",
                    args,
                    function (answer) { onChangePassword(answer); },
                    function () { /* console.log("Channel deletion cancelled"); */ });
            }

            $scope.logout = onLogOut;

            /**
             * Add new RSS channel
             */
            function onAdd(channel) {
                factoryRssChannels.postChannel(channel).then(
                    function () { $rootScope.$broadcast("ChannelAddedEvent", {}); },
                    function (error) { factoryDialogs.dlgError(null, null, error); }
                );
            }

            function onChangePassword(pwd) {
                console.log(pwd);
                factoryUsers.putPassword(pwd.CurrentPassword, pwd.NewPassword).then(
                    function () { factoryDialogs.dlgAlert(null, "DONE", "Password changed successfully"); },
                    function (error) { factoryDialogs.dlgError(null, null, error); }
                );
            }

            /**
             * User log out
             */
            function onLogOut() {
                $scope.$parent.backendHub.server.unsubscribe(factoryAuth.getUser())
                    .done(function () { factoryAuth.clearAuthentication(); })
                    .fail(function () { factoryAuth.clearAuthentication(); });
                //$location.path('/');
                $window.location.href = "/rssmanager/";
            }

        }]);
})()
    