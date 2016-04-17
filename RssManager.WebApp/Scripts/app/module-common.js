(function () {
    angular.module("moduleCommon", [])
        .factory("factoryCommon", function () {

            // INTERFACE
            var service = {
                getApiBaseUrl: getApiBaseUrl,
            };
            return service;

            function getApiBaseUrl() {
                var url = "http://localhost:64910";
                return url;
            }
        });
})()