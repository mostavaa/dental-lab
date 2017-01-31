(function () {
    'use strict';
    var app = angular.module('app', []);
    app.controller('productController', ['$scope', '$http', productController]);
    app.directive("confirmationNeeded", function () {
        return {
            priority: 1,
            terminal: true,
            link: function (scope, element, attr) {
                var msg = attr.confirmationNeeded || "Are You Sure You Want To Delete This ?";
                var clickAction = attr.ngClick;
                element.bind('click', function () {
                    if (window.confirm(msg)) {
                        scope.$eval(clickAction);
                    }
                });
            }
        };
    });
    app.directive('ngOnFinishRender', function ($timeout) {
        return {
            restrict: 'A',
            link: function (scope, element, attr) {
                if (scope.$last === true) {
                    $timeout(function () {
                        scope.$emit(attr.broadcastEventName ? attr.broadcastEventName : 'ngRepeatFinished');
                    });
                }
            }
        };
    });
    function productController($scope, $http, $location, $window) {

        $scope.$on('ngRepeatFinished', function () {
            $(".datePickers").datepicker();
        });

        $scope.isLoading = true;
        $scope.page = 1;
        $scope.numOfPages = 1;
        $scope.minPage = 1;
        $scope.maxPage = 5;
        $scope.range = function (min, max, step) {
            step = step || 1;
            var input = [];
            for (var i = min; i <= max; i += step) {
                input.push(i);
            }
            return input;
        }
        /*get all*/
        getAllData();
        function getAllData() {
            $http.get('/Products/all/?page=' + $scope.page)
    .success(function (data, status, headers, config) {
        for (var i = 0; i < data.models.length; i++) {
            if (data.models[i].enterDate!=null) {
                
                var date = new Date(parseInt(data.models[i].enterDate.substr(6)));

                var dx = date;

                var dd = dx.getDate();
                var mm = dx.getMonth() + 1;
                var yy = dx.getFullYear();

                if (dd <= 9) {
                    dd = "0" + dd;
                }
                
                if (mm <= 9) {
                    mm = "0" + mm;
                }
               // var hours = date.getHours();
                //var minutes = date.getMinutes();

                var displayDate = mm + "/" + dd + "/" + yy /*+ " " + hours + ":" + minutes*/;
                data.models[i].enterDate = displayDate;
            }
                 
        }
        $scope.models = data.models;
        $scope.numOfPages = data.numOfPages;
        $scope.isLoading = false;
        activateNextandPreviousandCurrent();
        var offset = $scope.page / 10;

        offset = Math.floor(offset);
        offset *= 10;
        $scope.minPage = offset;
        if ($scope.minPage == 0) $scope.minPage++;
        if (offset + 9 > $scope.numOfPages) {
            $scope.maxPage = $scope.numOfPages;
        } else {
            $scope.maxPage = offset + 9;
        }
    })
    .error(function (data, status, headers, config) {
        alert("something went wrong , please try again later");
        $scope.isLoading = false;
    });
        }
        $scope.getAll = function ($event) {
            $scope.page = this.n;
            if ($scope.page <= $scope.numOfPages && $scope.page > 0) {
                getAllData();
            }
        }
        $scope.getAllNext = function () {
            if ($scope.page + 1 <= $scope.numOfPages && $scope.page + 1 > 0) {
                $scope.page = $scope.page + 1;
                getAllData();
            }
        }

        $scope.getAllPrev = function () {
            if ($scope.page - 1 <= $scope.numOfPages && $scope.page - 1 > 0) {
                $scope.page = $scope.page - 1;
                getAllData();
            }

        }
        function activateNextandPreviousandCurrent() {
            $(".next").removeClass("inactive");
            $(".prev").removeClass("inactive");
            if ($scope.page + 1 > $scope.numOfPages) {
                $(".next").addClass("inactive");
            }
            if ($scope.page - 1 < 1) {
                $(".prev").addClass("inactive");
            }
            $(".page-numbers").removeClass("current");
            $(".page-numbers[data-page=" + $scope.page + "]").addClass("current");
        }

        /*edit*/
        $scope.toggleEdit = function () {
            this.model.editMode = !this.model.editMode;
        }

        $scope.save = function () {
            $scope.isLoading = true;
            var model = this.model;
            $http({
                method: 'POST',
                url: '/Products/Edit',
                data: model
            }).success(function (data, status, headers, config) {
                if (data.success === true) {
                    model.editMode = false;
                    console.log(model.enterDate);
                }
                else {
                    alert("something went wrong , please try again later");
                }
                $scope.isLoading = false;
            }).error(function (data, status, headers, config) {
                alert("something went wrong , please try again later");
                $scope.isLoading = false;
            });
        }
        /*add*/
        $scope.addMode = false;
        $scope.disableAddMode = function () {
            $scope.addMode = false;
        }
        $scope.enableAddMode = function () {
            $scope.addMode = true;
        }
        $scope.addNewModel = function () {
            $scope.isLoading = true;
            var model = this.NewModel;
            $http({
                method: 'POST',
                url: '/Products/Add',
                data: model
            }).success(function (data, status, headers, config) {
                if (data.success === true) {
                    //getAllData();
                    $scope.models.push(data.model);
                    $scope.addMode = false;
                }
                else {
                    alert("something went wrong , please try again later");
                }
                $scope.isLoading = false;
            }).error(function (data, status, headers, config) {
                alert("something went wrong , please try again later");
                $scope.isLoading = false;
            });
        }

        /*delete*/
        $scope.delete = function () {
            $scope.isLoading = true;
            var model = this.model;
            $http({
                method: 'POST',
                url: '/Products/Delete',
                data: model
            }).success(function (data, status, headers, config) {
                if (data.success === true) {
                    $.each($scope.models, function (i) {
                        if ($scope.models[i].id === model.id) {
                            $scope.models.splice(i, 1);
                            return false;
                        }
                    });
                    //getAllData();
                    alert("deleted successfully");
                }
                else {
                    alert("something went wrong , please try again later");
                }
                $scope.isLoading = false;
            }).error(function (data, status, headers, config) {
                alert("something went wrong , please try again later");
                $scope.isLoading = false;
            });

        }
    }
    
})();

/*app.directive("myRepeatDirective", function () {
    return function (scope, element, attrs) {
        if (scope.$last) {
            alert();
        }
    };
});


    app.filter("dateFilter", function () {
        return function (item) {
            if (item != null) {
                return new Date(parseInt(item.substr(6)));
            }
        };
    });

        app.directive('myDirective', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attrs, ngModelController) {
                ngModelController.$parsers.push(function (data) {
                    //convert data from view format to model format
                    if (data != null) {
                        data = new Date(parseInt(data.substr(6)));
                    }
                    return data; //converted
                });

                ngModelController.$formatters.push(function (data) {
                    //convert data from model format to view format
                    if (data != null) {
                        data = new Date(parseInt(data.substr(6)));
                    }
                    return data; //converted
                });
            }
        }
    });
        app.directive('datepickerMyDirective', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attrs, ngModelController) {
                ngModelController.$parsers.push(function (data) {
                    //convert data from view format to model format
                    if (data != null) {
                        data = new Date(parseInt(data.substr(6)));
                    }
                    return data; //converted
                });

                ngModelController.$formatters.push(function (data) {
                    //convert data from model format to view format
                    if (data != null) {
                        var dx = new Date(parseInt(data.substr(6)));

                        var dd = dx.getDate();
                        var mm = dx.getMonth() + 1;
                        var yy = dx.getFullYear();

                        if (dd <= 9) {
                            dd = "0" + dd;
                        }

                        if (mm <= 9) {
                            mm = "0" + mm;
                        }

                        var displayDate = mm + "/" + dd + "/" + yy;
                    }
                    return displayDate; //converted
                });
                
                element.datepicker({
                    minDate: 0,
                    onSelect: function (y) {
                        var edate = element.siblings('.sdate');
                        edate.datepicker();
                        // edate.datepicker();
                        //edate.datepicker('option', 'todayHighlight', true);
        
                        //edate.datepicker('option', 'minDate', element.datepicker("getDate"));
                        //edate.datepicker('option', 'format', "yyyy-MM-dd");
        
                    }
                });
                
}
}
});

*/
