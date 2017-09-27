using System.Threading.Tasks;
using Weather.Model;

namespace Weather.Repositories
{
    public interface IWeatherRepository
    {
        Task Recieve(string location);    

    }
}