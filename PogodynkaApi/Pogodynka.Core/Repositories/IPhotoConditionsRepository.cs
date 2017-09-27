using Pogodynka.Core.Domain;
using Pogodynka.Core.Repositiories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pogodynka.Core.Repositories
{
    public interface IPhotoConditionsRepository:IRepository
    {
        Task AddAsync(Images photoConditions);
        Task<IEnumerable<Images>> GetAll();
        Task<Images> RecieveAsync(Images conditions);
    }
}
