using System;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

namespace StatisticsAPI.Model
{
    public class Mission
    {
        // {agent: &#39;007&#39;, country: &#39;Brazil&#39;,
        //     address: &#39;Avenida Vieira Souto 168 Ipanema, Rio de Janeiro&#39;,
        //     date: &#39;Dec 17, 1995, 9:45:17 PM&#39;

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public string Id { get; set; }

        public string Agent { get; set; }

        public string Country { get; set; }

        public string Address { get; set; }

        public string Date { get; set; }

        [JsonIgnore]
        public GeoJsonPoint<GeoJson2DCoordinates> Location { get; set; }
    }
}