// ProjectControllerTests.cs

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Video_Project_Suite.Api;
using Video_Project_Suite.Api.Data;
using Video_Project_Suite.Api.Models;
using Video_Project_Suite.Api.Models.Dto;
using Xunit;

namespace Video_Project_Suite.Tests.Controllers
{
    public class ProjectControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly string _baseUrl = "/api/project";

        private HttpClient _client;

        public ProjectControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }



        private static StringContent CreateJsonContent(object obj)
        {
            var json = JsonSerializer.Serialize(obj);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private static async Task<T?> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content)) return default;

            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        private void Dispose()
        {
            _client?.Dispose();
        }

        #region GetAllProjects

        // True Positive Test: Retrieve all projects when projects exist in db

        // True Negative Test: Retrieve all projects when no projects exist in db


        #endregion

    }



}