// ===============================
// VISIONARY S.A.S.
// ===============================
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SignalRTest.Shared
{
    /// <summary>
    /// RestClient. Encapsulates and simplifies the REST code
    /// </summary>
    public sealed class RestClient : IDisposable
    {
        readonly HttpClient httpClient;
        string siteName = string.Empty;

        public RestClient(string apiRoute)
        {
            var baseAdress = GetBaseAdress(apiRoute);

            httpClient = new HttpClient()
            {
                BaseAddress = new Uri(baseAdress)
            };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<T>> GetAll<T>(string route)
        {
            try {
                var json = await httpClient.GetStringAsync(FixRoute(route));
                return JsonConvert.DeserializeObject<List<T>>(json);
            }
            catch (Exception exception) {
                Trace.WriteLine($"ERROR. GetAll(): {exception.Message}");
            }
            return null;
        }

        public async Task<T> Get<T>(string route, int pk)
        {
            try {
                var json = await httpClient.GetStringAsync($"{FixRoute(route)}/{pk}/");
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception exception) {
                Console.WriteLine($"ERROR. Get(): {exception.Message}");
            }
            return default;
        }

        public async Task<T> Post<T>(string route, T item)
        {
            /*
            Important note about django API
            --------------------------------
            C# not implement short date, then the model use DateTime
            Serialized create the json as "Date":"YYYY-MM-DDTHH:MM:SS", i.g. "Date":"1976-01-12T00:00:00"
            The POST send in respose this error: 
                {"Date":["Date has wrong format. Use one of these formats instead: YYYY-MM-DD."]}
            For Django to accept this format, YYYY-MM-DDTHH:MM:SS, we must add to Settings.py:
            REST_FRAMEWORK = {
                "DATE_INPUT_FORMATS": ["%Y-%m-%d",'%Y-%m-%dT%H:%M:%S'],
            }

            ** this sample has not date field **
            */
            try {
                var serialized = new JsonContent<T>(item);
                // post it
                var response = await httpClient.PostAsync(FixRoute(route), serialized);
                if (response.StatusCode == HttpStatusCode.Created) {
                    var js = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(js);
                } else {
                    Console.WriteLine($"Create fail: {response.StatusCode}");
                }
                return default;
            }
            catch (Exception exception) {
                Console.WriteLine($"ERROR. {exception.Message}");
            }
            return default;
        }

        public async Task<bool> Put<T>(string route, T item, int pk)
        {
            try {
                var serialized = new JsonContent<T>(item);
                var response = await httpClient.PutAsync($"{FixRoute(route)}/{pk}/", serialized);
                if (response.StatusCode == HttpStatusCode.OK) {
                    return true;
                } else {
                    Console.WriteLine($"Status: {response.StatusCode}");
                }
            }
            catch (Exception exception) {
                Console.WriteLine($"ERROR: {exception.Message}");
            }
            return false;
        }

        public async Task<bool> Delete(string route, int pk)
        {
            try {
                var response = await httpClient.DeleteAsync($"{FixRoute(route)}/{pk}");
                if (response.StatusCode == HttpStatusCode.NoContent) {
                    return true;
                } else {
                    Console.WriteLine($"Status: {response.StatusCode}");
                }
            }
            catch (Exception exception) {
                Console.WriteLine($"ERROR: {exception.Message}");
            }
            return false;
        }

        public async Task<T> GetRandomObject<T>(string route)
        {
            try {
                var json = await httpClient.GetStringAsync(FixRoute(route));
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception exception) {
                Console.WriteLine($"ERROR. Get(): {exception.Message}");
            }
            return default;
        }

        // site path solves when api-server is publish. The base adress
        // has to be isolate. Sample http;//localhost/signalrtest
        // base address is http;//localhost, and siteName = signalrtest
        // the routes complete will be siteName + route
        private string GetBaseAdress(string apiRoute)
        {
            var s = apiRoute;
            if (s.EndsWith("/")) s = s.Substring(0, s.Length - 1);
            var parts = s.Split('/');
            // path[0] is http
            // path[1] is empty
            // patr[2] is remaining
            if (parts.Length == 3) {
                return s;
            }
            siteName = parts[3];
            return s.Replace("/" + siteName, string.Empty);
        }

        private string FixRoute(string route)
        {
            if (siteName == string.Empty) {
                return route;
            }
            return siteName + route;
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }

    #region-- Utility for REST Asp.NEt Core --
    // https://goo.gl/cdJtaQ
    // best code, short code.
    public class JsonContent<T> : StringContent
    {
        public JsonContent(T obj) : base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json") { }
    }
    // for anonymus
    public class JsonContent : StringContent
    {
        public JsonContent(object obj) : base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json") { }
    }
    // sample: new HttpClient().PostAsync("http://...", new JsonContent(new { x = 1, y = 2 });
    #endregion
}
