(function () {
    angular.module("moduleApiUsers", []).factory("factoryUsers", ["factoryCommon", "$http", function (factoryCommon, $http) {

        var urlBase = factoryCommon.getApiBaseUrl() + "/api/users";

        // INTERFACE
        var factory = {
            postUser: postUser
        };
        return factory;

        function postUser(user) {
            return $http.post(urlBase, user);
        }
    }]);
})();