(function () {
    angular.module("moduleApiRssChannels", ["moduleCommon"])
        .factory("factoryRssChannels", ["factoryCommon", "$http", "$q", function (factoryCommon, $http, $q) {

            var urlBase = factoryCommon.getApiBaseUrl() + "/api/rsschannels";

            // PUBLIC INTERFACE
            var factory = {
                getChannels: getChannels,
                getChannel: getChannel,
                putChannel: putChannel,
                postChannel: postChannel,
                deleteChannel: deleteChannel,
                updateChannel: updateChannel
            };
            return factory;

            // ===== PRIVATE FUNCTIONS =====

            function getChannels() {
                //return $http.get(urlBase);
                var deferred = $q.defer();
                $http.get(urlBase)
                    .success(function (data) {
                        if (data != undefined && data.length > 0) {
                            for (i = 0; i < data.length; i++) {
                                data[i].HasUpdates = false;
                            }
                        }
                        deferred.resolve({
                            data: data
                        });
                    })
                    .error(function (msg, code) {
                        deferred.reject(msg);
                    });
                return deferred.promise;
            }

            function getChannel(id) {
                var url = urlBase + "/" + id;
                return $http.get(url);
            }

            function putChannel(channel) {
                // Update existing channel
                var url = urlBase + "/" + channel.Id;
                return $http.put(url, channel);
            }

            function postChannel(channel) {
                // Create new channel
                var url = urlBase;
                return $http.post(url, channel);
            }

            function deleteChannel(id) {
                var url = urlBase + "/" + id;
                return $http.delete(url);
            }

            function updateChannel(id) {
                var url = urlBase + "?action=update&id=" + id;
                return $http.put(url);
            }

        }]);
})();
