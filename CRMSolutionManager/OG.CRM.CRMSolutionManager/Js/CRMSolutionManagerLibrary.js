
function executeJsOperation() {
    var req = new XMLHttpRequest();

    var operationName = 'OG.CRM.CRMSolutionManager.JsOperations.ConvertAllPatchesToSolutionsOperation';
    var requestObject = {};
    var requestString = JSON.stringify(requestObject);

    req.open("GET", Xrm.Page.context.getClientUrl() + "/api/data/v8.2/mjolnir_jsoperation_ios?$select=mjolnir_input,mjolnir_jsoperationname,mjolnir_output&$filter=mjolnir_input eq '" + requestString + "' and mjolnir_output eq '1' and mjolnir_jsoperationname eq '" + operationName + "'", true);
    req.setRequestHeader("OData-MaxVersion", "4.0");
    req.setRequestHeader("OData-Version", "4.0");
    req.setRequestHeader("Accept", "application/json");
    req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    req.setRequestHeader("Prefer", "odata.include-annotations=\"*\",odata.maxpagesize=1");
    req.onreadystatechange = function () {
        if (this.readyState === 4) {
            req.onreadystatechange = null;
            if (this.status === 200) {
                var results = JSON.parse(this.response);
                for (var i = 0; i < results.value.length; i++) {
                    debugger;
                    var mjolnir_input = results.value[i]["mjolnir_input"];
                    var mjolnir_jsoperationname = results.value[i]["mjolnir_jsoperationname"];
                    var mjolnir_output = results.value[i]["mjolnir_output"];
                }
            } else {
                Xrm.Utility.alertDialog(this.statusText);
            }
        }
    };
    req.send();
}

executeJsOperation();