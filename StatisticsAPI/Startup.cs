using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Conventions;
using StatisticsAPI.Storage;

namespace StatisticsAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var mongoConnectionString = Configuration["mongo:connection.string"];
            var collection = Configuration["mongo:collection"];
            var dbName = Configuration["mongo:db"];

            var geocodingApiUrl = Configuration["geocoding:url"];
            var geocodingApiKey = Configuration["geocoding:api.key"];

            services.AddSingleton(typeof(GeocodingAPI),
                sp => new GeocodingAPI(geocodingApiUrl, geocodingApiKey));

            services.AddSingleton(typeof(IMissionService),
                sp => new MissionService(
                    mongoConnectionString,
                    dbName,
                    collection,
                    new GeocodingAPI(geocodingApiUrl, geocodingApiKey)
                ));

            RegisterMongoConventions();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IMissionService missionService)

        {
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            missionService.Init();
        }

        private void RegisterMongoConventions()
        {
            ConventionRegistry.Register(
                "camelCase",
                new ConventionPack
                {
                    new CamelCaseElementNameConvention()
                },
                t => true
            );

            ConventionRegistry.Register(
                "ignoreIfNull",
                new ConventionPack
                {
                    new IgnoreIfNullConvention(true)
                },
                t => true);
        }
    }
}