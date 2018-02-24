using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Mjolnir.CRM.Core.EntityManagers;
using Mjolnir.CRM.SolutionManager.BusinessManagers;

namespace SolutionManager.UnitTests
{
    [TestClass]
    public class WebResourceUnitTests : CrmUnitTestBase
    {
        private const string CRMDEVELOPEREXTENSIONTS_CONFIG_PATH = "../../CRMDeveloperExtensions.config";

        [TestMethod]
        public void should_read_crmdeveloperextensions_config()
        {
            CrmDeveloperExtensionsBusinessManager manager = new CrmDeveloperExtensionsBusinessManager(CRMDEVELOPEREXTENSIONTS_CONFIG_PATH);

            manager.ShouldSatisfyAllConditions(
                () => manager.ShouldNotBeNull(),
                () => manager.WebResourceDeployerModel.ShouldNotBeNull()
            );

        }

        [TestMethod]
        public void should_compare_crm_and_developerextension_webresources()
        {
            CrmDeveloperExtensionsBusinessManager manager = new CrmDeveloperExtensionsBusinessManager(CRMDEVELOPEREXTENSIONTS_CONFIG_PATH);
            

            manager.ShouldSatisfyAllConditions(
                () => manager.ShouldNotBeNull(),
                () => manager.WebResourceDeployerModel.ShouldNotBeNull()
            );

        }

    }
}
