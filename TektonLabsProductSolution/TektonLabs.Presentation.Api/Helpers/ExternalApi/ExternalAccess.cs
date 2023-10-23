using Newtonsoft.Json;

namespace TektonLabs.Presentation.Api.Helpers.ExternalApi
{
    public static class ExternalAccess
    {
        public static async Task<HttpResponseMessage> GetJsonDataFromUrlAsync(string url)
        {
            using (var httpClient = new HttpClient())
            {
                return await httpClient.GetAsync(url);
            }
        }

        public static async Task<List<T>> ConvertHttpResponseToObject<T>(HttpResponseMessage response)
        {
            string responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<T>>(responseString);
        }
    }
}
