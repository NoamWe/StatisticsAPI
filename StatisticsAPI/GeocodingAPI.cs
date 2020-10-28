using System;
using System.Net.Http;
using System.Web;
using MongoDB.Driver.GeoJsonObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StatisticsAPI
{
    public class GeocodingAPI
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private readonly string _key;
        private readonly string _url;

        public GeocodingAPI(string url, string key)
        {
            _url = url;
            _key = key;
        }

        /// <summary>
        ///     Converts address to GeoJson2DCoordinates
        /// </summary>
        /// <param name="address"></param>
        /// <returns>GeoJson2DCoordinates or null</returns>
        public GeoJson2DCoordinates GetCoordinates(string address)
        {
            try
            {
                var url = UrlBuilder(address);

                var httpResponseMessage = HttpClient.GetAsync(url).Result;
                httpResponseMessage.EnsureSuccessStatusCode();

                var responseBody = httpResponseMessage.Content.ReadAsStringAsync().Result;
                var responseJson = JsonConvert.DeserializeObject<JToken>(responseBody);
                var resultsArr = responseJson["results"] as JArray;
                var result = resultsArr[0];
                var geo = result["geometry"];
                var location = geo["location"];
                var lat = double.Parse(location["lat"].ToString());
                var lng = double.Parse(location["lng"].ToString());
                return new GeoJson2DCoordinates(lat, lng);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private string UrlBuilder(string address)
        {
            var builder = new UriBuilder(_url);
            builder.Port = -1;

            var query = HttpUtility.ParseQueryString(builder.Query);
            query["address"] = address;
            query["key"] = _key;
            builder.Query = query.ToString();
            var url = builder.ToString();

            return url;
        }
    }
}