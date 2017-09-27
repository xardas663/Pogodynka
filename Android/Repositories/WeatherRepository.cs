using Weather.Model;
using System.Threading.Tasks;

namespace Weather.Repositories
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly IApiService _service;
        
        public WholeWeather Weather = new WholeWeather();
       
        public WeatherRepository(IApiService service)
        {
            _service = service;
        }

        public async Task Recieve(string location)
        {
            if(location != null)
            { 
                await _service.RecieveWeatherForLocation(location);
            }         
         }       
      
    }
}