using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Widget;
using Newtonsoft.Json;
using Weather.Model;
using System.Globalization;
using Weather.Repositories;
using Weather.Model2;

namespace Weather
{
    public class ApiService : IApiService
    {
        private readonly Activity _activity;
        private readonly IView _view;
        private readonly IPhotoConditionsRepository _repository;
        private readonly Connection _connection;
        private readonly ProgressView _progress;

        private const string UndergroundApiForecastDailyString = "http://api.wunderground.com/api/363082dcb74c3abe/forecast10day/lang:PL/q/{0},{1}.json";
        private const string UndergroundApiForecastHourlyString = "http://api.wunderground.com/api/363082dcb74c3abe/hourly/lang:PL/q/{0},{1}.json";
        private const string GoogleLocationApi = "https://maps.googleapis.com/maps/api/geocode/json?address={0}&key=AIzaSyA3jRfNQ7DIQsXtYGC6oy5R-JKdWceKUzQ&language=PL";
        private const string GooglePlaceApi = "https://maps.googleapis.com/maps/api/place/details/json?placeid={0}&key=AIzaSyCdURfa9ypDSTEg1VUbeaspf8D9lVwJ3MI";
        private const string GooglePlacePhotoApi = "https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&photoreference={0}&key=AIzaSyCJZoBUUo7TSWdndlT2HL6er4MbhFCFkNI";

        private WholeWeather _weather = new WholeWeather();
        private Model2.RootObjectGooglePhoto _googlePlace = new Model2.RootObjectGooglePhoto();
        private JsonSerializerSettings settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };  

        public ApiService(Context context, IView view, IPhotoConditionsRepository repository)
        {
            _activity = (Activity)context;
            _view = view;
            _repository = repository;
            _connection = new Connection(_activity);
            _progress = new ProgressView(_activity,_view);
        }
        public async Task<string> DownloadJson(string strUri)
        {
            System.Net.WebClient webclient = new System.Net.WebClient();
            string strResultData;

            try
            {
                strResultData = await webclient.DownloadStringTaskAsync(new System.Uri(strUri));
            }
            catch (Exception)
            {
                strResultData = "Exception";
                _progress.DismissActivityIndicator();
                _progress.ThrowServerError("Weather Underground");
            }
            finally
            {
                webclient.Dispose();
                webclient = null;
            }

            return strResultData;
        }
        public async Task<string> RecieveLocationInfo(string apiString, string apiFormat)
        {

            if (!_connection.IsConnected())
            {
                _activity.RunOnUiThread(() =>
                {
                    Toast.MakeText(_activity, "W³¹cz po³¹czenie internetowe!", ToastLength.Short).Show();
                });
                return "Error";
            }

            try
            {
                if (_googlePlace.results == null)
                {
                    _activity.RunOnUiThread(() =>
                    {
                        _view.Progress = ProgressDialog.Show(_activity, "", "Oczekujê na dane z serwera Google");

                    });
                }

                string strLocation = await DownloadJson(string.Format(apiString, apiFormat));
                if (_googlePlace.results == null)
                {
                    _progress.DismissActivityIndicator();
                }

                if (strLocation != "Exception")
                {

                    RootObjectGooglePhoto location = JsonConvert.DeserializeObject<RootObjectGooglePhoto>(strLocation, settings);

                    if (location != null)
                    {
                        _googlePlace = location;


                        return "done";
                    }
                    else
                    {
                        _progress.ThrowServerError("Google");
                        return "Error";
                    }
                }
                else
                {
                    _progress.ThrowServerError("Google");
                    return "Error";
                }
            }
            catch (Exception)
            {
                _progress.ThrowServerError("Google");
                return "Error";
            }
        }
        public async Task<string> RecievePhotoOfLocationFromGooglePlacesApi()
        {

            if (!_connection.IsConnected())
            {
                _activity.RunOnUiThread(() =>
                {
                    Toast.MakeText(_activity, "W³¹cz po³¹czenie internetowe!", ToastLength.Short).Show();
                });
                return "Error";
            }

            try
            {
                string strPhotos = await DownloadJson(string.Format(GooglePlaceApi, _googlePlace.results[0].place_id));

                if (strPhotos != "Exception")
                {

                    _googlePlace = JsonConvert.DeserializeObject<Model2.RootObjectGooglePhoto>(strPhotos, settings);

                    if (_googlePlace != null)
                    {
                        return "done";
                    }
                    else
                    {
                        _activity.RunOnUiThread(() =>
                        {
                            Toast.MakeText(_activity, "B³¹d serwera Google :( Mo¿e wpisano z³¹ lokalizacjê ?", ToastLength.Short).Show();
                        });
                        return "Error";
                    }
                }
                else
                {
                    _activity.RunOnUiThread(() =>
                    {
                        Toast.MakeText(_activity, "B³¹d, serwer Google nie odpowiada :( ", ToastLength.Short).Show();
                    });
                    return "Error";
                }
            }
            catch (Exception e)
            {
                _activity.RunOnUiThread(() =>
                {
                    Toast.MakeText(_activity, "B³¹d serwera Google, spróbuj ponownie " + e.Message, ToastLength.Long).Show();
                });
                return "Error";
            }
        }
        public async Task<bool> RecieveDataFromWeatherUndergroundApiForLocationLatLong()
        {
            string IsDone = await RecieveLocationInfo(GoogleLocationApi, MainActivity.Location);
            
            if (IsDone == "done")
            {
                if (!_connection.IsConnected())
                {
                    _activity.RunOnUiThread(() =>
                    {
                        Toast.MakeText(_activity, "W³¹cz po³¹czenie internetowe!", ToastLength.Short).Show();
                    });
                    return false;
                }

                if(_googlePlace.results.Count < 1)
                {
                    _progress.ThrowBadRequest();
                    return false;
                }

                else if (_googlePlace.results[0].address_components != null)
                {
                    try
                    {
                        _activity.RunOnUiThread(() =>
                        {
                            if (_googlePlace.results[0].address_components.Count > 2)
                            {
                                _view.Progress = ProgressDialog.Show(_activity, "",
                                     "Oczekujê na dane z serwera WeatherUnderground dla lokalizacji: " + _googlePlace.results[0].address_components[0].long_name +
                                     ", " + _googlePlace.results[0].address_components[2].short_name ?? "");
                            }
                            else
                            {
                                _view.Progress = ProgressDialog.Show(_activity, "", "Oczekujê na dane z serwera WeatherUnderground dla lokalizacji ");
                            }
                        });

                        string latt = _googlePlace.results[0].geometry.location.lat.ToString(CultureInfo.InvariantCulture);
                        string lngg = _googlePlace.results[0].geometry.location.lng.ToString(CultureInfo.InvariantCulture);
                        string lat = latt.Replace(@",", ".");
                        string lng = lngg.Replace(@",", ".");
                        string strForecastDailyJson = "a";
                        string strForecastHourlyJson = "a";

                        if (MainActivity.Location != "Poznañ" && MainActivity.Location != "Pi³a" && MainActivity.Location != "Niedoradz")
                        {
                            Task<string> task1 = DownloadJson(string.Format(UndergroundApiForecastDailyString, lat, lng));
                            Task<string> task2 = DownloadJson(string.Format(UndergroundApiForecastHourlyString, lat, lng));
                            Task<string> task3 = RecieveLocationInfo(GooglePlaceApi, _googlePlace.results[0].place_id);

                            string[] tasks = await Task.WhenAll(task1, task2, task3).ConfigureAwait(false);
                            strForecastDailyJson = tasks[0];
                            strForecastHourlyJson = tasks[1];
                            _progress.DismissActivityIndicator();
                        }
                        else
                        {
                            Task<string> task1 = DownloadJson(string.Format(UndergroundApiForecastDailyString, lat, lng));
                            Task<string> task2 = DownloadJson(string.Format(UndergroundApiForecastHourlyString, lat, lng));
                            string[] tasks = await Task.WhenAll(task1, task2).ConfigureAwait(false);
                            strForecastDailyJson = tasks[0];
                            strForecastHourlyJson = tasks[1];
                            _progress.DismissActivityIndicator();
                        }


                        if (strForecastDailyJson != "Exception" && strForecastHourlyJson != "Exception")
                        {
                            _weather.ObjRootObjectDailyForecast = JsonConvert.DeserializeObject<RootObjectDailyForecast>(strForecastDailyJson, settings);
                            _weather.ObjRootObjectHourlyForecast = JsonConvert.DeserializeObject<RootObjectHourlyForecast>(strForecastHourlyJson, settings);
                            if (_weather.ObjRootObjectDailyForecast.forecast != null && _weather.ObjRootObjectHourlyForecast.hourly_forecast != null)
                            {
                                return true;
                            }
                            else
                            {
                                _progress.ThrowServerError("Weather Underground");
                                return false;
                            }
                        }
                        else
                        {
                            _progress.ThrowServerError("Weather Underground");
                            return false;
                        }
                    }
                    catch (Exception)
                    {
                        _progress.ThrowServerError("Weather Underground");
                        return false;
                    }
                }
                _progress.ThrowBadRequest();
                return false;
            }
            _progress.ThrowServerError("Weather Underground");
            return false;
        }        
        public async Task<WholeWeather> RecieveWeatherForLocation(string location)
        {
            MainActivity.Location = location;
            var isValid = false;
            isValid = await RecieveDataFromWeatherUndergroundApiForLocationLatLong();

            if (isValid)
            {
                _repository.FillRespository(_weather, _repository);
                _view.ObjWholeWeather = _weather;

                _view.Progress = ProgressDialog.Show(_activity, "", "Pobieranie zdjêcia ");
                Task task1 = _view.SetImage();
                Task task2 = _view.SetBackground(_googlePlace);
                Task task3 = _view.BindData();
                await Task.WhenAll(task1,task2, task3);
                _progress.DismissActivityIndicator();
            }
            return _weather;
        }    
    }
}