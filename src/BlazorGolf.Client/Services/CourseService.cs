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

        public async Task<IEnumerable<Course>> GetCoursesAsync()
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
            }
            else
            {
                _logger.LogInformation("GetCourses token failure");
            }
            return null;
        }

        public async Task<Course> GetCourseAsync(Guid courseId) {
            _logger.LogInformation("GetCourse called");
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
                return await _httpClient.GetFromJsonAsync<Course>($"api/courses/{courseId}");
            }
            else
            {
                _logger.LogInformation("GetCourse token failure");
            }
            return null;
        }

        public async Task<Course> CreateCourseAsync(Course course)
        {
            _logger.LogInformation("AddCourse called");
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
                var response = await _httpClient.PostAsJsonAsync<Course>("api/courses", course);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Call to API to add course succeeded.");
                    return await response.Content.ReadFromJsonAsync<Course>();
                }
            }
            else
            {
                _logger.LogInformation("GetCourse token failure");
            }
            return null;
        }

        public async Task<Course> UpdateCourseAsync(Course course)
        {
            _logger.LogInformation("UpdateCourse called");
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
                var response = await _httpClient.PutAsJsonAsync<Course>($"api/courses/{course.CourseId}", course);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Call to API to update course succeeded.");
                    return await response.Content.ReadFromJsonAsync<Course>();
                } else {
                    _logger.LogError($"Call to API to update course failed : {response.StatusCode}" );                   
                }
            }
            else
            {
                _logger.LogInformation("UpdateCourse token failure");
            }
            return null;

        }

        public async Task DeleteCourseAsync(Course course)
        {
            _logger.LogInformation("DeleteCourse called");
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
                var response = await _httpClient.DeleteAsync($"api/courses/{course.CourseId}");
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Call to API to delete course succeeded.");
                }
            }
            else
            {
                _logger.LogInformation("UpdateCourse token failure");
            }
            return;
        }
    }
}