(function () {

    var auth = angular.module("moduleAuth", []);

    auth.factory("factoryAuth", ["$window", function ($window) {
        // INTERFACE
        var factory = {
            isAuthenticated: isAuthenticated,
            hasToken: hasToken,
            hasUser: hasUser,
            getToken: getToken,
            getUser: getUser,
            setAuthentication: setAuthentication,
            clearAuthentication: clearAuthentication
        };
        return factory;

        function setAuthentication(token, user) {
            $window.sessionStorage.setItem("RssManagerToken", token);
            $window.sessionStorage.setItem("RssManagerUser", user);
        }

        function isAuthenticated() {
            var res =
                $window.sessionStorage.getItem("RssManagerToken") &&
                $window.sessionStorage.getItem("RssManagerUser");
            return res;
        }

        function hasToken() {
            var t = $window.sessionStorage.getItem("RssManagerToken");
            if (t == null || t == undefined)
                return false;
            else
                return true;
        }

        function getToken() {
            var t = $window.sessionStorage.getItem("RssManagerToken");
            return t;
        }

        function hasUser() {
            var t = $window.sessionStorage.getItem("RssManagerUser");
            if (t == null || t == undefined)
                return false;
            else
                return true;
        }

        function getUser() {
            var t = $window.sessionStorage.getItem("RssManagerUser");
            return t;
        }

        function clearAuthentication() {
            $window.sessionStorage.removeItem("RssManagerToken");
            $window.sessionStorage.removeItem("RssManagerUser");
        }

    }]);

    auth.factory("factoryAuthInterceptor", ["$window", "$q", "factoryAuth", function ($window, $q, factoryAuth) {
        return {
            request: function (config) {
                config.headers = config.headers || {};
                if (factoryAuth.hasToken()) {
                    config.headers.Authorization = "Bearer " + factoryAuth.getToken();
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
    }]);
})()