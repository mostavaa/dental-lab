(function () {
    'use strict';
    var app = angular.module('app', []);
    app.controller('DoctorsController', ['$scope', '$http', DoctorsController]);
    function DoctorsController($scope, $http, $location, $window) {
        $scope.isLoading = true;

        /*get all*/
        getAllData();
        function getAllData() {
            $http.get('/doctors/all/')
    .success(function (data, status, headers, config) {
        $scope.models = data;
        console.log(config);
        $scope.isLoading = false;
    })
    .error(function (data, status, headers, config) {
        alert("something went wrong , please try again later");
        $scope.isLoading = false;
    });
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
                url: '/doctors/Edit',
                data: model
            }).success(function (data, status, headers, config) {
                if (data.success === true) {
                    model.editMode = false;
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
                url: '/doctors/Add',
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
                url: '/doctors/Delete',
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


/* template
angular.module('app', [])
.controller('doctorController', function ($scope, $http, $location, $window) {
    emplpyees();
    function emplpyees() {
        $http.get()
            .success(function ($data, $status, headers, config) {
                            $scope.ListCustomer = data;
            })
            .error(function ($data, $status, headers, config) {

            });
    }

    $scope.getCustomer = function (custModel) {
        $http.get('/Home/GetbyID/' + custModel.Id)
        .success(function (data, status, headers, config) {
            //debugger;
            $scope.custModel = data;
            getallData();
            console.log(data);
        })
        .error(function (data, status, headers, config) {
            $scope.message = 'Unexpected Error while loading data!!';
            $scope.result = "color-red";
            console.log($scope.message);
        });
    };


    $scope.htmlfunctionname = function (params) {
        $scope.isViewLoading = true;
        $http({
            method: 'POST',
            url: '/Home/Insert',
            data: $scope.custModel
        }).success(function (data, status, headers, config) {
            if (data.success === true) {
                
                $scope.message = 'Form data Saved!';
                $scope.result = "color-green";
                getallData();
                $scope.custModel = {};
                console.log(data);
                
            }
            else {
                
                $scope.message = 'Form data not Saved!';
                $scope.result = "color-red";
                
            }
        }).error(function (data, status, headers, config) {
            
            $scope.message = 'Unexpected Error while saving data!!' + data.errors;
            $scope.result = "color-red";
            console.log($scope.message);
            
        });
        getallData();
        $scope.isViewLoading = false;
    };

    
    $http.delete('/Home/Delete/' + custModel.Id)
.success..
}).config(function ($locationProvider) {
    $locationProvider.html5Mode(true);
});
*/