using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace BlazorGolf.Client.Authentication
{
    public class CustomUserAccount : RemoteUserAccount
    {
        [JsonPropertyName("roles")]
        public string[] Roles { get; set; } = Array.Empty<string>();

        [JsonPropertyName("wids")]
        public string[] Wids { get; set; } = Array.Empty<string>();

        [JsonPropertyName("oid")]
        public string Oid { get; set; }
    }
}
