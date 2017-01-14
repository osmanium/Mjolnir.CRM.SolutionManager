
function executeJsOperation(operation, input, successCallback, errorCallback) {

    try {

        //TODO: Start loading pop up

        var req = new XMLHttpRequest();
        req.open("GET", Xrm.Page.context.getClientUrl() + "/api/data/v8.1/mjolnir_jsoperation_ios?$select=mjolnir_input,mjolnir_jsoperation_ioid,mjolnir_jsoperationname,mjolnir_output&$filter=mjolnir_input eq '" + input + "' and  mjolnir_jsoperationname eq '" + operation + "'", true);
        req.setRequestHeader("OData-MaxVersion", "4.0");
        req.setRequestHeader("OData-Version", "4.0");
        req.setRequestHeader("Accept", "application/json");
        req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
        req.setRequestHeader("Prefer", "odata.include-annotations=\"*\"");
        req.onreadystatechange = function () {
            if (this.readyState === 4) {
                req.onreadystatechange = null;
                if (this.status === 200) {
                    var results = JSON.parse(this.response);
                    for (var i = 0; i < results.value.length; i++) {
                        var mjolnir_io = {
                            mjolnir_input: results.value[i]["mjolnir_input"],
                            mjolnir_jsoperation_ioid: results.value[i]["mjolnir_jsoperation_ioid"],
                            mjolnir_jsoperationname: results.value[i]["mjolnir_jsoperationname"],
                            mjolnir_name: results.value[i]["mjolnir_name"],
                            mjolnir_output: results.value[i]["mjolnir_output"]
                        }

                        //TODO: End loading pop up
                        successCallback(mjolnir_io);
                        return;
                    }
                } else {
                    //TODO: End loading pop up
                    errorCallback(this.statusText);
                }
            }
        };
        req.send();
    } catch (e) {
        //TODO: End loading pop up
        //TODO: Show error message
    }
}


function alertResult(resultObj) {
    if (!resultObj.IsSuccesful) {
        alert(resultObj.ErrorMessage);
    }
    else {
        alert("Success!");
    }
}

function PatchToSolution(selectedSolutionId) {
    debugger;
    var operation = 'CRMSolutionManager.JsOperations.ConvertPatchesToSolutionsOperation';
    var inputObj = {
        SelectedSolutionIds: selectedSolutionId
    }

    var inputJson = JSON.stringify(inputObj);

    executeJsOperation(operation, inputJson,
        function (io_obj) {

            var resultString = io_obj.mjolnir_output;
            var resultObj = jQuery.parseJSON(io_obj.mjolnir_output);

            alertResult(resultObj);
        },
        function (error) {
            alert(error);
        }
    );

}

function ApplySolutionUpgrade(selectedSolutionId) {

    var operation = 'CRMSolutionManager.JsOperations.ApplySolutionUpgradeOperation';
    var inputObj = {
        SelectedSolutionIds: selectedSolutionId
    }

    var inputJson = JSON.stringify(inputObj);

    executeJsOperation(operation, inputJson,
        function (io_obj) {

            var resultString = io_obj.mjolnir_output;
            var resultObj = jQuery.parseJSON(io_obj.mjolnir_output);

            alertResult(resultObj);
        },
        function (error) {
            alert(error);
        }
    );

}