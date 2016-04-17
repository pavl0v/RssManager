(function () {
    var dialogs = angular.module("moduleDialogs", []);
    dialogs.factory("factoryDialogs", function ($mdDialog, $mdMedia) {

        // PUBLIC INTERFACE
        var factoryInterface = {
            dlgController: dlgController,
            dlgAlert: dlgAlert,
            dlgError: dlgError,
            dlgConfirm: dlgConfirm,
            dlgCustom: dlgCustom
        };
        return factoryInterface;

        function dlgController($scope, $mdDialog, args) {
            $scope.args = args;
            $scope.hide = function () {
                $mdDialog.hide();
            };
            $scope.cancel = function () {
                $mdDialog.cancel();
            };
            $scope.answer = function (answer) {
                $mdDialog.hide(answer);
            };
        }

        /**
         * Alert dialog
         */
        function dlgAlert(ev, title, textContent) {
            // Appending dialog to document.body to cover sidenav in docs app
            // Modal dialogs should fully cover application
            // to prevent interaction outside of dialog
            $mdDialog.show(
              $mdDialog.alert()
                .parent(angular.element(document.querySelector('#popupContainer')))
                .clickOutsideToClose(true)
                .title(title)
                .textContent(textContent)
                .ariaLabel('')
                .ok('OK')
                .targetEvent(ev)
            );
        }

        /**
         * Alert dialog to process error
         */
        function dlgError(ev, title, error) {
            if (title == undefined || title == "") {
                title = "Error";
            }

            var msg = "";
            try {
                msg = error.data.Message || error.data
            }
            catch (err) {
                msg = error;
            }
            console.log(msg);
            $mdDialog.show(
              $mdDialog.alert()
                .parent(angular.element(document.querySelector('#popupContainer')))
                .clickOutsideToClose(true)
                .title(title)
                .textContent(msg)
                .ariaLabel('')
                .ok('OK')
                .targetEvent(ev)
            );
        }

        /**
         * Confirm dialog
         */
        function dlgConfirm(ev, title, onConfirm, onCancel) {
            // Appending dialog to document.body to cover sidenav in docs app
            var confirm = $mdDialog.confirm()
                  .title(title)
                  .textContent('')
                  .ariaLabel('')
                  .targetEvent(ev)
                  .ok('OK')
                  .cancel('Cancel');
            $mdDialog.show(confirm).then(function () {
                onConfirm();
            }, function () {
                onCancel();
            });
        }

        /**
         * Custom dialog
         */
        function dlgCustom($scope, ev, templateUrl, args, onConfirm, onCancel) {
            var useFullScreen = ($mdMedia('sm') || $mdMedia('xs')) && $scope.customFullscreen;
            $mdDialog.show({
                controller: dlgController,
                locals: {
                    args: args
                },
                templateUrl: templateUrl,
                parent: angular.element(document.body),
                targetEvent: ev,
                clickOutsideToClose: true,
                fullscreen: useFullScreen
            }).then(
                function (answer) {
                    if (answer != "cancel") {
                        onConfirm(answer);
                    }
                    else {
                        onCancel();
                    }
                },
                function () {
                    onCancel();
                }
            );
            $scope.$watch(
                function () {
                    return $mdMedia('xs') || $mdMedia('sm');
                },
                function (wantsFullScreen) {
                    $scope.customFullscreen = (wantsFullScreen === true);
                },
                true
            );
        }
    })
})();
