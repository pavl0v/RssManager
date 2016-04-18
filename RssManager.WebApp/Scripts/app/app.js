(function () {
    var app = angular.module("app", ["moduleAuthInterceptor", "moduleApiRssChannels", "moduleApiRssItems",
        "moduleCommon", "moduleDialogs", "moduleDirectives", "moduleApiUsers", "ngRoute", "ngMaterial", "ngMessages"]);

    app.config(function ($httpProvider, $routeProvider, $mdThemingProvider) {

        /**
         * AuthInterceptor implicitly provides 
         * headers with token for each request
         */
        $httpProvider.interceptors.push("factoryAuthInterceptor");

        $routeProvider.
            when("/auth", {
                templateUrl: "auth.html",
                controller: "controllerLogin"
            }).
            when("/home", {
                templateUrl: "home.html",
                //controller: ""
            }).
            otherwise({
                redirectTo: "/auth"
            });

        $mdThemingProvider.theme('default')
            .primaryPalette('indigo')
            .accentPalette('orange')
            .warnPalette('orange')
        //.backgroundPalette('teal')
    });

    /**
     *
     */
    app.controller("appctrl", ["$scope", "$rootScope", "$window", "$mdToast", "factoryCommon", "factoryDialogs",
        function ($scope, $rootScope, $window, $mdToast, factoryCommon, factoryDialogs) {

            var showSimpleToast = function (message) {
                $mdToast.show({
                    template: '<md-toast class="md-toast">' + message + '</md-toast>',
                    hideDelay: 6000,
                    position: "bottom right"
                });
            };

            /**
             * ATTENTION!
             * Load necessary script for SignalR
             */
            var signalrHubsUrl = factoryCommon.getApiBaseUrl() + "/signalr/hubs";
            $scope.backendHub = null;
            $.ajax({
                method: "GET",
                url: signalrHubsUrl,
                dataType: "script",
            }).done(function () {
                //$.connection.hub.transportConnectTimeout = 3000;
                $.connection.hub.url = signalrHubsUrl + "/signalr";
                $scope.backendHub = $.connection.backendHub;
                $scope.backendHub.client.broadcastMessage = function (message) {
                    //console.log(message);
                    showSimpleToast("Some RSS channels got updated");
                    //$("#pfooter").hide().fadeIn(3000);
                    var arr = JSON.parse(message);
                    if (arr != undefined && arr.length > 0) {
                        $rootScope.$broadcast("ChannelsUpdatedEvent", arr);
                    }
                };
                $.connection.hub.start()
                    .done(function () {
                        if ($window.sessionStorage.getItem("RssManagerToken") && $window.sessionStorage.getItem("RssManagerUser")) {
                            $scope.backendHub.server.subscribe($window.sessionStorage.getItem("RssManagerUser"));
                            //.done(function () { console.log("backendHub.subscribe OK"); })
                            //.fail(function () { console.log("backendHub.subscribe ERROR"); });
                        }
                    })
                    .fail(function () {
                        factoryDialogs.dlgError(null, null, "Connection to SignalRHub failed");
                    });
            }).fail(function (jqXHR, textStatus) {
                // Not fires when dataType is "script"
                factoryDialogs.dlgError(null, null, "Connection to SignalRHub failed");
            });
        }]);
})();