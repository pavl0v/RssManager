(function () {
    angular.module("moduleApiUsers", []).factory("factoryUsers", ["factoryCommon", "$http", function (factoryCommon, $http) {

        var urlBase = factoryCommon.getApiBaseUrl() + "/api/users";

        // INTERFACE
        var factory = {
            postUser: postUser,
            putPassword: putPassword
        };
        return factory;

        function postUser(user) {
            return $http.post(urlBase, user);
        }

        function putPassword(oldpwd, newpwd) {
            var url = urlBase + "/password/?oldpwd=" + oldpwd + "&newpwd=" + newpwd;
            return $http.put(url);
        }
    }]);
})();