angular.module("umbraco").controller("activeis.umbFormsCustomEmailController",
    function ($scope, activeisCustomEmailPickerResource) {

        $scope.openTreePicker = function () {

            $scope.treePickerOverlay = {
                view: "treepicker",
                treeAlias: "activeisCustomEmailTemplates",
                section: "forms",
                entityType: "customemail-template",
                multiPicker: false,
                show: true,
                onlyInitialized: false,
                select: function (node) {
                    activeisCustomEmailPickerResource.getVirtualPathForEmailTemplate(node.id).then(function (response) {
                        //Set the picked template file path as the setting value
                        $scope.setting.value = response.data.path;
                    });

                    $scope.treePickerOverlay.show = false;
                    $scope.treePickerOverlay = null;
                },
                close: function (model) {
                    // close dialog
                    $scope.treePickerOverlay.show = false;
                    $scope.treePickerOverlay = null;
                }
            };

        };

    });