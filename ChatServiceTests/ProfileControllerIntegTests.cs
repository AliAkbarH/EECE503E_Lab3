﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ChatService;
using ChatService.Client;
using ChatService.DataContracts;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChatServiceTests
{
    /// <summary>
    /// The integration tests are used to validate the full API execution end-to-end.
    /// These are usually the tests that allow us to find most issues and they tend to
    /// be less fragile because they are decoupled from implementation details (they rely only
    /// on the document API).
    /// </summary>
    [TestClass]
    [TestCategory("Integration")]
    public class ProfileControllerIntegTests
    {
        private HttpClient httpClient;
        private TestServer server;
        private ChatServiceClient chatServiceClient;

        [TestInitialize]
        public void TestInitialize()
        {
            server = new TestServer(WebServer.CreateWebHostBuilder());
            httpClient = server.CreateClient();
            chatServiceClient = new ChatServiceClient(httpClient);
        }

        [TestMethod]
        public async Task CreateGetProfile()
        {
            var createProfileDto = new CreateProfileDto
            {
                Username = "nbilal",
                FirstName = "Nehme",
                LastName = "Bilal"
            };

            await chatServiceClient.CreateProfile(createProfileDto);
            UserProfile userProfile = await chatServiceClient.GetProfile("nbilal");

            Assert.AreEqual(createProfileDto.Username, userProfile.Username);
            Assert.AreEqual(createProfileDto.FirstName, userProfile.FirstName);
            Assert.AreEqual(createProfileDto.LastName, userProfile.LastName);
        }

        [TestMethod]
        public async Task GetNonExistingProfile()
        {
            try
            {
                await chatServiceClient.GetProfile("nbilal");
                Assert.Fail("A ChatServiceException was expected but was not thrown");
            }
            catch (ChatServiceException e)
            {
                Assert.AreEqual(HttpStatusCode.NotFound, e.StatusCode);
            }
        }

        [TestMethod]
        public async Task CreateDuplicateProfile()
        {
            var createProfileDto = new CreateProfileDto
            {
                Username = "nbilal",
                FirstName = "Nehme",
                LastName = "Bilal"
            };

            await chatServiceClient.CreateProfile(createProfileDto);

            try
            {
                await chatServiceClient.CreateProfile(createProfileDto);
                Assert.Fail("A ChatServiceException was expected but was not thrown");
            }
            catch (ChatServiceException e)
            {
                Assert.AreEqual(HttpStatusCode.Conflict, e.StatusCode);
            }
        }
    }
}
