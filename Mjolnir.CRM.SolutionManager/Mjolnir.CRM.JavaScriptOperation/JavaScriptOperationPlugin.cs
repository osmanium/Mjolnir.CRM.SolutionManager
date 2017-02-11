using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mjolnir.CRM.Common;

namespace Mjolnir.CRM.JavaScriptOperation
{
    public class JavaScriptOperationPlugin : PluginBase
    {
        public override void ExecuteInternal(CRMContext pluginContext)
        {
            #region Validation for correct plugin message and stage
            if (!pluginContext.PluginExecutionContext.PrimaryEntityName.Equals("mjolnir_jsoperation_io") &&
                    pluginContext.PluginExecutionContext.Stage != 40 &&
                    !pluginContext.PluginExecutionContext.MessageName.Equals("RetrieveMultiple"))
            {
                pluginContext.TracingService.Trace("Plugin is not registered for required Entity, Message or Stage.");
                return;
            }
            #endregion

            #region Validation for input parameters
            QueryExpression query;
            String sOperation = string.Empty;
            String sInputParameter = string.Empty;

            if (pluginContext.PluginExecutionContext.InputParameters.Contains("Query"))
            {
                if (pluginContext.PluginExecutionContext.InputParameters["Query"] is QueryExpression)
                {
                    query = (QueryExpression)pluginContext.PluginExecutionContext.InputParameters["Query"];
                    if (query != null &&
                        query.Criteria != null &&
                        query.Criteria.Conditions != null)
                    {
                        foreach (ConditionExpression expression in query.Criteria.Conditions)
                        {
                            pluginContext.TracingService.Trace("Query Attribute = " + expression.AttributeName);

                            if (expression.AttributeName.Equals("mjolnir_jsoperationname"))
                            {
                                sOperation = expression.Values[0].ToString();
                                pluginContext.TracingService.Trace("Operation Name = " + sOperation);
                            }
                            else if (expression.AttributeName.Equals("mjolnir_input"))
                            {
                                sInputParameter = expression.Values[0].ToString();
                                pluginContext.TracingService.Trace("Input = " + sInputParameter);
                            }
                        }
                    }
                    else
                    {
                        pluginContext.TracingService.Trace("Query Criteria Condition Count: " + query.Criteria.Conditions.Count.ToString() + " is not greater then zero");
                        return;
                    }
                }
                else
                {
                    pluginContext.TracingService.Trace("pluginExecutionContext.InputParameters[ParameterName.Query] is not QueryExpression");
                    return;
                }
            }
            else
            {
                pluginContext.TracingService.Trace("Query does not exist in input parameters");
                return;
            }
            #endregion


            var executingAssembly = System.Reflection.Assembly.GetExecutingAssembly();

            if (executingAssembly != null)
            {
                //"CRMSolutionManager.ConvertAllPatchesToSolutions"
                Type operationType = executingAssembly.GetType(sOperation);
                if (operationType != null)
                {
                    //Get the generic type, first generic input type
                    Type requestType = operationType.BaseType.GenericTypeArguments[0];

                    //Initiate executer
                    dynamic operationExecuterInstance = Activator.CreateInstance(operationType);

                    //Pass the input object to the executer of IJavaScriptOperationExecuter<>
                    if (operationExecuterInstance != null)
                    {
                        var response = operationExecuterInstance.Execute(sInputParameter, pluginContext);

                        //Return response
                        try
                        {
                            if (!pluginContext.PluginExecutionContext.OutputParameters.ContainsKey("BusinessEntityCollection"))
                            {
                                pluginContext.PluginExecutionContext.OutputParameters.Add("BusinessEntityCollection", new EntityCollection());
                            }

                            var output = (EntityCollection)pluginContext.PluginExecutionContext.OutputParameters["BusinessEntityCollection"];

                            EntityCollection outputEntities = (EntityCollection)output;
                            Entity entity = new Entity("mjolnir_jsoperation_io");
                            entity["mjolnir_input"] = sInputParameter;
                            entity["mjolnir_output"] = response;
                            entity["mjolnir_jsoperationname"] = sOperation;
                            entity["mjolnir_jsoperation_ioid"] = Guid.NewGuid();
                            outputEntities.Entities.Add(entity);

                            pluginContext.TracingService.Trace("execution is completed");
                        }
                        catch (Exception ex)
                        {
                            pluginContext.TracingService.Trace($"error : {ex.Message}");
                        }
                    }
                    else
                    {
                        pluginContext.TracingService.Trace("executerInstance null");
                    }
                }
                else
                {
                    pluginContext.TracingService.Trace("operationType is null");
                }
            }
            else
            {
                pluginContext.TracingService.Trace("assembly is null");
            }
        }
    }
}