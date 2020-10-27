using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.FileProviders;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StatisticsAPI.Model;

namespace StatisticsAPI.Storage
{
    public class MissionService : IMissionService
    {
        private readonly IMongoCollection<Mission> _missions;
        private readonly GeocodingAPI _geocodingApi;

        public MissionService(string connectionString, string dbName, string collectionName, GeocodingAPI geocodingApi)
        {
            _geocodingApi = geocodingApi;
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(dbName);
            database.DropCollection(collectionName);
            _missions = database.GetCollection<Mission>(collectionName);
            PopulateDb(database, collectionName);
        }

        private void PopulateDb(IMongoDatabase database, string collectionName)
        {
            try
            {
                database.DropCollection(collectionName);
                _missions.Indexes.CreateOne(
                    new CreateIndexModel<Mission>(
                        new IndexKeysDefinitionBuilder<Mission>().Geo2DSphere(x => x.Location)));

                var embeddedProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
                using (var reader = embeddedProvider.GetFileInfo("MissionService.sampleData.json").CreateReadStream())
                {
                    StreamReader re = new StreamReader(reader);
                    string st = re.ReadToEnd();

                    JArray jArray = JArray.Parse(st);

                    foreach (var entry in jArray)
                    {
                        var mission = JsonConvert.DeserializeObject<Mission>(entry.ToString());
                        Insert(mission);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Initializing mission by adding required date (i.e. coordinates)
        /// </summary>
        /// <param name="newMission"></param>
        private void InitializeMission(Mission newMission)
        {
            var coordinates = _geocodingApi.GetCoordinates(newMission.Address);
            newMission.Location = new GeoJsonPoint<GeoJson2DCoordinates>(coordinates);
        }

        public void Insert(Mission newMission)
        {
            InitializeMission(newMission);
            _missions.InsertOne(newMission);
        }

        public string GetCountryByIsolation()
        {
            var agents = GetIsolatedAgents();
            var country = GetMostIsolatedCountry(agents);
            return country;
        }

        private List<string> GetIsolatedAgents()
        {
            //get {agentName: count}
            //select entries where agent is isolated (count == 1)
            
            var agents = _missions
                .AsQueryable()
                .GroupBy(x => x.Agent)
                .Select(g => new {g.Key, count = g.Count()})
                .Where(i => i.count == 1)
                .Select(a => a.Key)
                .ToList();

            return agents;
        }

        private string GetMostIsolatedCountry(IList<string> agemts)
        {
            //get the missions that contain isolated agents
            //get this structure {country : countMissionsWithIsolatedAgent}
            //country with highest count is most isolated.
            //return the name of the first highest
            
            var country = _missions
                .AsQueryable()
                .Where(x => agemts.Contains(x.Agent))
                .GroupBy(x => x.Country)
                .Select(x => new {x.Key, count = x.Count()})
                .OrderByDescending(x=>x.count)
                .ToList();

            return country.First().Key;
        }

        public Mission FIndNearestMission(GeoJson2DCoordinates coordinates)
        {
            var point = GeoJson.Point(GeoJson.Geographic(coordinates.X, coordinates.Y));
            var locationQuery = new FilterDefinitionBuilder<Mission>().Near(tag => tag.Location, point);
            // var filter = Builders<Mission>.Filter.Near(x => x.Location, coordinates.X, coordinates.Y);
            var result = _missions.Find(locationQuery).FirstOrDefault();
            return result;
        }
    }
}