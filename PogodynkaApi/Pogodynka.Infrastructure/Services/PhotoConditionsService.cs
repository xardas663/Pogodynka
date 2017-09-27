using Pogodynka.Core.Domain;
using Pogodynka.Core.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pogodynka.Infrastructure.Services
{

    public class PhotoConditionsService: IPhotoConditionsService
    {
        private readonly IPhotoConditionsRepository _repository;
        public PhotoConditionsService(IPhotoConditionsRepository repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(Images photo)
        {
            await _repository.AddAsync(photo);
        }

        public async Task<IEnumerable<Images>> GetAllAsync()
        {
            var photos = await _repository.GetAll();
            return photos;
        }

        public async Task<Images> RecieveAsync(Images conditions)
        {
            var photo = await _repository.RecieveAsync(conditions);
            return photo;
        }

    }
}