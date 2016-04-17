(function ($, undefined) {
    var app = angular.module("app");
    app.controller("controllerRssItems", ["$scope", "$rootScope", "factoryRssItems", "factoryRssChannels", "factoryDialogs",
        function ($scope, $rootScope, factoryRssItems, factoryRssChannels, factoryDialogs) {
            var itemsContainer = null;
            var itemsEndOfList = null;
            var channel = {};
            var end = false;
            var pageSize = 20; // the value should cause the scroll enabling on first page to provide further page loading

            $scope.toLoadNextPage = "0";
            $scope.channelId = 0;
            $scope.currentPageNo = 0;
            $scope.items = [];
            $scope.fab = { // Floating action button (FAB) settings
                topDirections: ['left', 'up'],
                bottomDirections: ['down', 'right'],
                isOpen: 'false',
                availableModes: ['md-fling', 'md-scale'],
                selectedMode: 'md-scale',
                availableDirections: ['up', 'down', 'left', 'right'],
                selectedDirection: 'down'
            };
            $scope.click = onClick;
            $scope.readAll = onReadAll;
            $scope.updateChannel = onUpdateChannel;
            $scope.modifyChannel = onModifyChannel;

            /**
             * Trick to load next portion of items.
             * There is an element div with id="itemsendoflist" in HTML (home.html).
             * As the div become visible in container (toLoadNextPage turns from 0 to 1)
             * the request for the next portion of items fires.
             */
            $scope.$watch("toLoadNextPage", function (newValue, oldValue) {
                if (newValue == "1" && oldValue == "0") { onLoadNextPage(); }
            }, true);

            /**
             * Load page with certain number and size
             */
            function onLoadPage(channelId, pageNo, pageSize) {
                factoryRssItems.getItems(channelId, pageNo, pageSize).then(
                    function (data) {
                        if (data.data.length > 0) {
                            var i;
                            for (i in data.data) {
                                // Hack: to process possible CDATA meta :(
                                var html = "<span>" + data.data[i].RssDescription + "</span>";
                                var noHtml = $(html).text();
                                data.data[i].RssDescription = noHtml;
                                $scope.items.push(data.data[i]);
                            }
                        }
                        else {
                            end = true;
                        }
                    },
                    function (error) {
                        end = true;
                        factoryDialogs.dlgError(null, null, error);
                    });
            }

            /**
             * Helper to load next page
             */
            function onLoadNextPage() {
                if (end) {
                    return;
                }
                $scope.currentPageNo++;
                onLoadPage($scope.channelId, $scope.currentPageNo, pageSize);
            }

            /**
             * Mark all RSS items as read
             */
            function onReadAll() {
                factoryDialogs.dlgAlert(undefined, "Info", "readall: not implemented yet");
            }

            /**
             * Mark RSS item as read
             */
            function onClick(item) {
                if (item.ReadState != 0)
                    return;

                factoryRssItems.readItem(item.Id).then(
                    function (data) { item.ReadState = 1; },
                    function (error) { factoryDialogs.dlgError(null, null, error); }
                );
            }

            /**
             * Open channel from database (no update from URL)
             */
            function onOpenChannel() {
                $scope.currentPageNo = 0;
                end = false;
                $scope.items.splice(0, $scope.items.length); // clear array
                onLoadNextPage();
            }

            /**
             * Update database and then open channel
             */
            function onUpdateChannel() {
                factoryDialogs.dlgConfirm(
                    undefined,
                    "New RSS items will be delivered from URL",
                    function () {
                        factoryRssChannels.updateChannel($scope.channelId).then(
                            function () { onOpenChannel(); },
                            function (error) { factoryDialogs.dlgError(null, null, error); }
                        );
                    },
                    function () { }
                );

            }

            /**
             * Modify selected RSS chanel
             */
            function onModifyChannel() {
                factoryRssChannels.getChannel($scope.channelId).then(
                    function (data) {
                        channel = data.data;
                        showDialogModifyChannel();
                    },
                    function (error) { factoryDialogs.dlgError(null, null, error); }
                );
            }

            /**
             * Update selected RSS chanel
             */
            function onPutChannel(channel) {
                factoryRssChannels.putChannel(channel).then(
                    function () {
                        $rootScope.$broadcast("ChannelUpdatedEvent", { channelId: $scope.channelId });
                    },
                    function (error) { factoryDialogs.dlgError(null, null, error); }
                );
            }

            /**
             * Channel selected event handler
             */
            $scope.$on("ChannelSelectedEvent", function (event, args) {
                $scope.channelId = args.channelId;
                onOpenChannel();
            });

            /**
             * Dialog for modify selected RSS channel
             */
            var showDialogModifyChannel = function (ev) {
                var args = {
                    dialogHeader: "Modify RSS Channel",
                    channel: channel,
                    isDisabledOK: false,
                    isDisabledURL: true
                };
                factoryDialogs.dlgCustom(
                    $scope,
                    ev,
                    "Html/dialog-channel.html",
                    args,
                    function (answer) { onPutChannel(answer); },
                    function () { /* console.log("Channel modification cancelled"); */ });
            };

            /**
             * jQuery related functions 
             */
            $(document).ready(function () {
                itemsContainer = $("#itemscontainer");
                itemsEndOfList = $("#itemsendoflist");
                onContainerScroll(itemsContainer, itemsEndOfList);
            });
            function onContainerScroll(cont, elem) {
                // http://www.ordinarycoder.com/jquery-fade-content-scroll/
                $(cont).scroll(function () {
                    if ($(cont).scrollTop() == 0) { // skip if scroll to top on list clear
                        return;
                    }

                    checkEndPageMarker(cont, elem);
                })
            }
            function isElementVisibleInContainer(cont, elem) {
                var res = false;

                var contTop = $(cont).position().top;
                var contBottom = contTop + $(cont).height();
                var elemTop = $(elem).offset().top;
                var elemBottom = elemTop + $(elem).height();

                if (contBottom > elemTop && contTop < elemBottom) {
                    res = true;
                }

                return res;
            }
            function checkEndPageMarker(cont, elem) {
                var isVisible = isElementVisibleInContainer(cont, elem);
                if (isVisible) {
                    if ($scope.toLoadNextPage == "0") {
                        $scope.toLoadNextPage = "1";
                        $scope.$apply();
                    }
                }
                else {
                    if ($scope.toLoadNextPage == "1") {
                        $scope.toLoadNextPage = "0";
                        $scope.$apply();
                    }
                }
            }

        }]);
})(jQuery);