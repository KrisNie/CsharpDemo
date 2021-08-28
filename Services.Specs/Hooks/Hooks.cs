using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Services.Specs.Hooks
{
    [Binding]
    public sealed class Hooks
    {
        // Hooks are global,
        // but can be restricted to run only for features or scenarios by defining a scoped binding,
        // which can be filtered with tags.
        // The execution order of hooks for the same type is undefined,
        // unless specified explicitly.

        [BeforeScenario(Order = 0)]
        public void BeforeScenario()
        {
            //TODO: implement logic that has to run before executing each scenario
        }

        [BeforeScenario(Order = 2)]
        public void SetupTestUsers(ScenarioContext scenarioContext)
        {
            //scenarioContext...
        }

        [AfterScenario]
        public void AfterScenario()
        {
            //TODO: implement logic that has to run after executing each scenario
        }

        [BeforeFeature]
        public static void SetupStuffForFeatures(FeatureContext featureContext)
        {
            Console.WriteLine("Starting " + featureContext.FeatureInfo.Title);
        }

        [BeforeTestRun]
        public static void BeforeTestRunInjection(ITestRunnerManager testRunnerManager, ITestRunner testRunner)
        {
            //All parameters are resolved from the test thread container automatically.
            //Since the global container is the base container of the test thread container, globally registered services can be also injected.

            //ITestRunManager from global container
            var location = testRunnerManager.TestAssembly.Location;

            //ITestRunner from test thread container
            var threadId = testRunner.ThreadId;

            Console.WriteLine($"Location: {location}; ThreadId: {threadId}");
        }
    }
}