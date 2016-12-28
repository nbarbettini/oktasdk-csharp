using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Okta.Core.Clients;
using System.Collections.Generic;

namespace Okta.Core.Tests.Clients
{
    [TestClass]
    public class EventsClientTests
    {

        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        private Tenant oktaTenant;
        private OktaClient oktaClient;
        private string strDateSuffix;

        [TestInitialize()]
        public void InitializeTenant()
        {
            oktaTenant = Helpers.GetTenantFromConfig();
            oktaClient = new OktaClient(oktaTenant.ApiKey, new Uri(oktaTenant.Url));
            strDateSuffix = DateTime.Now.ToString("yyyy.MM.dd.hh.mm.ss.ff");
        }

        [TestMethod()]
        public void GetEvents()
        {
            try
            {
                var eventsClient = oktaClient.GetEventsClient();
                FilterBuilder filter = new FilterBuilder("published gt \"2016-12-04T23:23:59.000Z\"");

                var events = eventsClient.GetList(pageSize: 10, filter: filter);
                int eventsCount = events.Results.Count;
                Assert.AreEqual(eventsCount, 10, 0, $"The number of returned events is different from 10: {eventsCount}.");
            }
            catch (Exception ex)
            {
                string strEx = ex.ToString();
            }
        }
    }
}
