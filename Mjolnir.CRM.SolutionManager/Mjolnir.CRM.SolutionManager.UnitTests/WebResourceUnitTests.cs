using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mjolnir.CRM.SolutionManager.CLI.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;

namespace SolutionManager.UnitTests
{
    [TestClass]
    public class WebResourceUnitTests
    {
        private const string CRMDEVELOPEREXTENSIONTS_CONFIG_PATH = "../../CRMDeveloperExtensions.config";

        [TestMethod]
        public void should_read_crmdeveloperextensions_config()
        {
            CRMDeveloperExtensionsManager manager = new CRMDeveloperExtensionsManager(CRMDEVELOPEREXTENSIONTS_CONFIG_PATH);

            manager.ShouldSatisfyAllConditions(
                () => manager.ShouldNotBeNull(),
                () => manager.WebResourceDeployerModel.ShouldNotBeNull()
            );

        }
    }
}
