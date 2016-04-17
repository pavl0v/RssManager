(function () {
    var directives = angular.module("moduleDirectives", []);

    directives.directive("rssItems", function () {
        return {
            restrict: "E",
            templateUrl: "Html/rss-items.html"
        }
    });

    directives.directive("rssItemsMenu", function () {
        return {
            scope: {
                readall: '&'
            },
            restrict: "E",
            templateUrl: "Html/rss-items-menu.html",
            link: function (scope, element, attrs) {
            }
        }
    });

    directives.directive("rssMainMenu", function () {
        return {
            restrict: "E",
            templateUrl: "Html/rss-main-menu.html",
            link: function (scope, element, attrs) {
            }
        }
    });

    directives.directive("rssChannels", function () {
        return {
            restrict: "E",
            templateUrl: "Html/rss-channels.html"
        }
    });

    directives.directive("rssCaptcha", function () {
        return {
            scope: {
                x: "=",
                invalid: "="
            },
            restrict: "E",
            //transclude: "element",
            templateUrl: "Html/captcha.html",
            link: function (scope, element, attrs) {
                scope.s = Math.floor(Math.random() * 101);
                scope.a = Math.floor(Math.random() * scope.s);
                scope.b = scope.s - scope.a;
                scope.$watch(function () { return scope.x; }, function (newValue, oldValue) {
                    if (typeof newValue !== 'undefined' && newValue == scope.b) {
                        scope.invalid = false;
                        //console.log("valid");
                    }
                    else {
                        scope.invalid = true;
                        //console.log("invalid");
                    }
                });
            }
        }
    });
})();

