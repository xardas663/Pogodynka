using System;
using System.Linq;
using Weather.Model;

namespace Weather.Repositories
{
    public class PhotoConditionsRepository:IPhotoConditionsRepository
    {
        public PhotoConditions PhotoConditions { get; set; } = new PhotoConditions();
   

        public IPhotoConditionsRepository FillRespository(WholeWeather _weather, IPhotoConditionsRepository _repository)
        {
            
            _repository.PhotoConditions.TempMax = Int32.Parse(_weather.ObjRootObjectHourlyForecast.hourly_forecast
                .Take(12).Max(p => p.temp.metric));
            _repository.PhotoConditions.TempMin = Int32.Parse(_weather.ObjRootObjectHourlyForecast.hourly_forecast
                .Take(12).Min(p => p.temp.metric));
            _repository.PhotoConditions.FeelsLikeMax = Int32.Parse(_weather.ObjRootObjectHourlyForecast.hourly_forecast
                .Take(12).Max(p => p.feelslike.metric));
            _repository.PhotoConditions.FeelsLikeMin = Int32.Parse(_weather.ObjRootObjectHourlyForecast.hourly_forecast
                .Take(12).Min(p => p.feelslike.metric));
            _repository.PhotoConditions.Rain = Int32.Parse(_weather.ObjRootObjectHourlyForecast.hourly_forecast
                .Take(12).Max(p => p.qpf.metric));
            return _repository;
        }

    }
}