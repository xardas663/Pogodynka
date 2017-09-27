using System.Threading.Tasks;
using Weather.Model;

namespace Weather
{
    public interface IApiService
    {
        Task<bool> RecieveDataFromWeatherUndergroundApiForLocationLatLong();
        Task<string> DownloadJson(string strUri);
        Task<string> RecieveLocationInfo(string apiString, string apiFormat);
        Task<string> RecievePhotoOfLocationFromGooglePlacesApi();
        Task<WholeWeather> RecieveWeatherForLocation(string location);  
    }
}