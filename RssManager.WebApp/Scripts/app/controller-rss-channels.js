(function () {
    var app = angular.module("app");
    app.controller("controllerRssChannels", ["$scope", "$rootScope", "$window", "factoryRssChannels", "factoryDialogs",
        function ($scope, $rootScope, $window, factoryRssChannels, factoryDialogs) {

        onRefresh();

        $scope.click = onClick;

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
                }
            };

            factoryDialogs.dlgCustom(
                $scope,
                ev,
                "Html/dialog-channel.html",
                args,
                function (answer) { onAdd(answer); },
                function () { /* console.log("Channel deletion cancelled"); */ });
        }

        /**
         * Dialog for delete RSS channel
         */
        $scope.showDialogDeleteChannel = function (ev, id) {
            factoryDialogs.dlgConfirm(
                ev,
                "Would you like to delete channel?",
                function () { onDelete(id); },
                function () { /*console.log("Channel deletion cancelled");*/ }
            );
        }

        /**
         * Channel update event handler
         */
        $scope.$on("ChannelUpdatedEvent", function (event, args) {
            onRefresh();
        });

        /**
         * Channel added event handler
         */
        $scope.$on("ChannelAddedEvent", function (event, args) {
            onRefresh();
        });

        /**
         * Channels updated event handler
         */
        $scope.$on("ChannelsUpdatedEvent", function (event, args) {
            for (var i = 0; i < $scope.channels.length; i++) {
                var newItems = 0;
                for (var j = 0; j < args.length; j++) {
                    if (args[j].ChannelId == $scope.channels[i].Id) {
                        newItems = args[j].NewItems;
                        break;
                    }
                }
                $scope.channels[i].NewItems = newItems;
            }
        });

        /**
         * Refresh channels list
         */
        function onRefresh() {
            factoryRssChannels.getChannels().then(
                function (channels) {
                    $scope.channels = channels.data;
                },
                function (error) { factoryDialogs.dlgError(null, null, error); }
            );
        }

        /**
         * Channel click
         */
        function onClick(channel) {
            channel.NewItems = 0;
            $rootScope.$broadcast("ChannelSelectedEvent", { channelId: channel.Id });
        }

        /**
         * Add new RSS channel
         */
        function onAdd(channel) {
            factoryRssChannels.postChannel(channel).then(
                function () { onRefresh(); },
                function (error) { factoryDialogs.dlgError(null, null, error); }
            );
        }

        /**
         * Delete RSS channel
         */
        function onDelete(id) {
            factoryRssChannels.deleteChannel(id).then(
                function () { onRefresh(); },
                function (error) { factoryDialogs.dlgError(null, null, error); }
            );
        }

    }]);
})();