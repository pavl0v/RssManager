(function () {
    angular.module("moduleApiRssItems", ["moduleCommon"])
        .factory("factoryRssItems", ["factoryCommon", "$http", "$q", function (factoryCommon, $http, $q) {

            var urlBase = factoryCommon.getApiBaseUrl() + "/api/rssitems";

            // INTERFACE
            var factory = {
                getItems: getItems,
                putItem: putItem,
                readItem: readItem,
                readAll: readAll
            };
            return factory;

            function getItems(channelId, pageNo, pageSize) {
                var url = urlBase + "?channelid=" + channelId + "&pageno=" + pageNo + "&pagesize=" + pageSize;
                var deferred = $q.defer();
                $http.get(url)
                    .success(function (data) {
                        deferred.resolve({
                            data: data
                        });
                    })
                    .error(function (msg, code) {
                        deferred.reject(msg);
                    });
                return deferred.promise;
                //return $http.get(url);
            }

            function putItem(item) {
                var url = urlBase + "/" + item.Id;
                return $http.put(url, item);
            }

            function readItem(itemId) {
                var url = urlBase + "?action=read&id=" + itemId;
                return $http.put(url);
            }

            function readAll(channelId) {
                var url = urlBase + "?action=readall&id=" + channelId;
                return $http.put(url);
            }
        }]);
})();