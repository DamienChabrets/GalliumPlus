using GalliumPlusApi.Dto;
using GalliumPlusApi.Exceptions;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GalliumPlusApi
{
    internal class GalliumPlusHttpClient : HttpClient
    {
        private JsonSerializerOptions jsonOptions;

        public JsonSerializerOptions JsonOptions => jsonOptions;

        public GalliumPlusHttpClient()
        {
            BaseAddress = new Uri("https://" + ApiConfig.Current.Host);
            DefaultRequestHeaders.Add("X-Api-Key", ApiConfig.Current.ApiKey);
            jsonOptions = new JsonSerializerOptions();
            jsonOptions.PropertyNameCaseInsensitive = true;
        }

        public void UseSessionToken(string token)
        {
            DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }

        private static T WaitForTask<T>(Task<T> task)
        {
            task.Wait();

            if (task.IsFaulted)
            {
                throw task.Exception ?? new Exception("Unknown error in task");
            }

            return task.Result;
        }

        public async Task<string> LookUpVersionAsync()
        {
            var response = await this.GetAsync("");
            return response.Headers.GetValues("x-gallium-version").First();
        }

        public T Get<T>(string resource)
        {
            var response = WaitForTask(this.GetAsync(resource));

            ExceptionFactory.ThrowForStatus(response);

            return WaitForTask(response.Content.ReadFromJsonAsync<T>(jsonOptions))
                ?? throw new NullReferenceException("Json deserialisation return null");
        }

        public T Post<T>(string resource, object data)
        {
            var response = WaitForTask(this.PostAsJsonAsync(resource, data, jsonOptions));

            ExceptionFactory.ThrowForStatus(response);

            return WaitForTask(response.Content.ReadFromJsonAsync<T>(jsonOptions))
                ?? throw new NullReferenceException("Json deserialisation return null");
        }

        public void Post(string resource, object data)
        {
            var response = WaitForTask(this.PostAsJsonAsync(resource, data, jsonOptions));

            ExceptionFactory.ThrowForStatus(response);
        }

        public void Put(string resource, object data)
        {
            var response = WaitForTask(this.PutAsJsonAsync(resource, data, jsonOptions));

            ExceptionFactory.ThrowForStatus(response);
        }

        public void Delete(string resource)
        {
            var response = WaitForTask(this.DeleteAsync(resource));

            ExceptionFactory.ThrowForStatus(response);
        }
    }
}
