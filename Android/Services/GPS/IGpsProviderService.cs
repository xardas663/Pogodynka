using System.Threading.Tasks;

namespace Weather
{
    public interface IGpsProviderService
    {
        Task<bool> GetCurrentPosition();        
        Task<bool> RecieveLocationFromGps();
        Task<bool> IsLocationEnabled();
    }
}