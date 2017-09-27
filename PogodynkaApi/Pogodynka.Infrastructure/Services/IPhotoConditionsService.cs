using Pogodynka.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pogodynka.Infrastructure.Services
{
    public interface IPhotoConditionsService: IService
    {
        Task<IEnumerable<Images>> GetAllAsync();
        Task AddAsync(Images photo);
        Task<Images> RecieveAsync(Images conditions);
    }
}
