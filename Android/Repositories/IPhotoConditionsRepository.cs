using Weather.Model;

namespace Weather.Repositories
{

    public interface IPhotoConditionsRepository
    {
        PhotoConditions PhotoConditions { get; set; }
        IPhotoConditionsRepository FillRespository(WholeWeather _weather, IPhotoConditionsRepository _repository);
    }
}