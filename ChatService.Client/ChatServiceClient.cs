using ChatService.DataContracts;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ChatService.Client
{

    public class ChatServiceClient : IChatServiceClient
    {
        private readonly HttpClient httpClient;

        public ChatServiceClient(Uri baseUri)
        {
            httpClient = new HttpClient()
            {
                BaseAddress = baseUri
            };
        }

        public ChatServiceClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task CreateProfile(CreateProfileDto profileDto)
        {
            try
            {
                HttpResponseMessage response = await httpClient.PostAsync("api/profile",
                    new StringContent(JsonConvert.SerializeObject(profileDto), Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                {
                    throw new ChatServiceException("Failed to create user profile", response.ReasonPhrase, response.StatusCode);
                }
            }
            catch (Exception e)
            {
                // make sure we don't catch our own exception we threw above
                if (e is ChatServiceException) throw;

                throw new ChatServiceException("Failed to reach chat service", e, "Internal Server Error",
                    HttpStatusCode.InternalServerError);
            }
        }

        public async Task<UserProfile> GetProfile(string username)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync($"api/profile/{username}");
                if (!response.IsSuccessStatusCode)
                {
                    throw new ChatServiceException("Failed to retrieve user profile", response.ReasonPhrase, response.StatusCode);
                }

                string content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserProfile>(content);
            }
            catch (JsonException e)
            {
                throw new ChatServiceException($"Failed to deserialize profile for user {username}", e, 
                    "Serialization Exception", HttpStatusCode.InternalServerError);
            }
            catch (Exception e)
            {
                // make sure we don't catch our own exception we threw above
                if (e is ChatServiceException) throw;

                throw new ChatServiceException("Failed to reach chat service", e, 
                    "Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
