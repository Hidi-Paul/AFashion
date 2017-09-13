using Newtonsoft.Json;

namespace OCS.MVC.Models
{
    public class Token
    {
        [JsonProperty("Username")]
        public string Username { get; set; }
        [JsonProperty("Role")]
        public string Role { get; set; }
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonProperty("error")]
        public string Error { get; set; }
    }
}