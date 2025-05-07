using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;
using FerreteriaWebApp.Models;

public class ApiService<T>
{
    private readonly string _baseUrl;
    private readonly string _controllerRoute;

    public ApiService(string controllerRoute)
    {
        _baseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];
        _controllerRoute = controllerRoute;
    }

    private async Task<ApiResponse<R>> HandleRequest<R>(Func<HttpClient, Task<HttpResponseMessage>> sendRequest)
    {
        using (var client = new HttpClient())
        {
            var apiResponse = new ApiResponse<R>();
            try
            {
                var response = await sendRequest(client);
                var json = await response.Content.ReadAsStringAsync();

                apiResponse = JsonConvert.DeserializeObject<ApiResponse<R>>(json);
                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<R>
                {
                    StatusCode = 500,
                    Message = "Error en la comunicación con la API: " + ex.Message,
                    data = default
                };
            }
        }
    }

    public async Task<ApiResponse<List<T>>> GetAllAsync()
    {
        return await HandleRequest<List<T>>(client =>
            client.GetAsync($"{_baseUrl}{_controllerRoute}"));
    }

    public async Task<ApiResponse<T>> GetByIdAsync(int id)
    {
        return await HandleRequest<T>(client =>
            client.GetAsync($"{_baseUrl}{_controllerRoute}/{id}"));
    }

    public async Task<ApiResponse<string>> CreateAsync(T entity)
    {
        var json = JsonConvert.SerializeObject(entity);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        return await HandleRequest<string>(client =>
            client.PostAsync($"{_baseUrl}{_controllerRoute}", content));
    }

    public async Task<ApiResponse<string>> UpdateAsync(int id, T entity)
    {
        var json = JsonConvert.SerializeObject(entity);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        return await HandleRequest<string>(client =>
            client.PutAsync($"{_baseUrl}{_controllerRoute}/{id}", content));
    }

    public async Task<ApiResponse<string>> DeleteAsync(int id)
    {
        return await HandleRequest<string>(client =>
            client.DeleteAsync($"{_baseUrl}{_controllerRoute}/{id}"));
    }
}
