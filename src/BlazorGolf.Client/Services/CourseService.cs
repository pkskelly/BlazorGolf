using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Json;
using BlazorGolf.Core.Models;

namespace BlazorGolf.Client.Services
{
    public class CourseService : ICourseService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CourseService> _logger;
        private readonly IAccessTokenProvider _tokenProvider;


        public CourseService(HttpClient httpClient, IAccessTokenProvider tokenProvider, ILogger<CourseService> logger)
        {
            Guard.Against.Null(httpClient, nameof(httpClient));
            Guard.Against.Null(logger, nameof(logger));
            Guard.Against.Null(tokenProvider, nameof(tokenProvider));

            _httpClient = httpClient;
            _tokenProvider = tokenProvider;
            _logger = logger;
        }

        public async Task<IEnumerable<Course>> GetCourses()
        {
            _logger.LogInformation("GetCourses called");
            var tokenResult = await _tokenProvider.RequestAccessToken(
                new AccessTokenRequestOptions
                {
                    Scopes = new[] { "api://weather/weather.read" }
                }
            );           
            if (tokenResult.TryGetToken(out var token))
            {
                var accessToken = token.Value;
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                return await _httpClient.GetFromJsonAsync<IEnumerable<Course>>("api/courses");
                _logger.LogInformation("GetCourses completed");
            }
            else
            {
                _logger.LogInformation("GetCourses token failure");
            }          
            return null;
        }
    }
}