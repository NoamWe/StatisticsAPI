using MongoDB.Driver.GeoJsonObjectModel;
using StatisticsAPI.Model;

namespace StatisticsAPI.Storage
{
    public interface IMissionService
    {
        /// <summary>
        ///     Inserts a new mission into storage.
        /// </summary>
        /// <param name="newMission"></param>
        public void Insert(Mission newMission);

        /// <summary>
        ///     Returns the name of the country with the highest isolation rank
        /// </summary>
        /// <returns></returns>
        public string GetCountryByIsolation();

        /// <summary>
        ///     Gets nearest mission from storage
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public Mission FIndNearestMission(GeoJson2DCoordinates address);

        public void Init();
    }
}