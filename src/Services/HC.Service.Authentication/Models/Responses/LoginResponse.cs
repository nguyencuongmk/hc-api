using Newtonsoft.Json;

namespace HC.Service.Authentication.Models.Responses
{
    public class LoginResponse
    {
        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }
    }
}
