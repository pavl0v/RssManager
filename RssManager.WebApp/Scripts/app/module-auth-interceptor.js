(function () {
    angular.module("moduleAuthInterceptor", []).factory("factoryAuthInterceptor", ["$window", "$q", function ($window, $q) {
        return {
            request: function (config) {
                config.headers = config.headers || {};
                if ($window.sessionStorage.getItem("RssManagerToken")) {
                    config.headers.Authorization = "Bearer " + $window.sessionStorage.getItem("RssManagerToken");
                }
                return config || $q.when(config);
            },
            response: function (response) {
                if (response.status === 401) {
                    $window.location.href = "auth.html";
                }
                return response || $q.when(response);
            }
        };
    }])
})()