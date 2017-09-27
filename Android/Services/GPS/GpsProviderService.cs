using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Widget;
using Plugin.Geolocator;
using Android.Locations;
using System;

namespace Weather.Model
{
    public class GpsProviderService : IGpsProviderService
    {
        private readonly Activity _activity;
        private readonly IApiService _api;
        private readonly MyView _view;
        private readonly ProgressView _progress;
        public GpsProviderService(Context context, IApiService api, MyView view)
        {
            _activity = (Activity)context;
            _api = api;
            _view = view;
            _progress = new ProgressView(_activity,_view);
        }     
        
        public async Task<bool> GetCurrentPosition()
        {
            bool done = false;

            _view.TxtLocation.Hint = "Oczekujê na GPS...";

            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;

                var position = await locator.GetPositionAsync();
                
                if (position != null)
                {
                    Geocoder geocoder = new Geocoder(_activity);          
                    IList<Address> addressList = await geocoder.GetFromLocationAsync(position.Latitude, position.Longitude, 10);
                                        
                    Address address = addressList.FirstOrDefault();

                    if (address != null)
                    {
                        done = true;

                        if(address.Locality != null)
                        {
                            _activity.RunOnUiThread(() =>
                            {
                                _view.TxtLocation.Text = address.Locality;
                            });
                        }                                                          
                                              
                        _activity.RunOnUiThread(() => {
                            _view.StrLocation = _view.TxtLocation.Text;
                            _view.TxtLocation.ClearFocus();
                        });

                    }
                    else
                    {
                        done = false;
                        _progress.DismissActivityIndicator();
                        _activity.RunOnUiThread(() => {
                        Toast.MakeText(_activity, "B³¹d ! :(!", ToastLength.Short).Show();
                        });
                    }
                }
                else
                {
                    done = false;
                }
            }
            catch (Exception ex)
            {
                _progress.DismissActivityIndicator();
                _activity.RunOnUiThread(() => {
                    Toast.MakeText(_activity, "B³¹d ! :("+ex.Message, ToastLength.Short).Show();
                });
                done = false;
            }
            return done;
        }

        public async Task<bool> IsLocationEnabled()
        {
            LocationManager lm = (LocationManager)_activity.GetSystemService(Context.LocationService);
            if (!lm.IsProviderEnabled(LocationManager.GpsProvider) || !lm.IsProviderEnabled(LocationManager.NetworkProvider))
            {
                _activity.RunOnUiThread(() =>
                {
                    Toast.MakeText(_activity, "W³¹cz lokalizacjê.", ToastLength.Short).Show();

                });
                await Task.Delay(1000);
                return false;
            }
            return true;
        }
      
        public async Task<bool> RecieveLocationFromGps()
        {
            bool isEnabled = await IsLocationEnabled();
            if ((isEnabled))
            {
                _activity.RunOnUiThread(() =>
                {
                    _view.Progress = ProgressDialog.Show(_activity, "", "Szukam Twojego po³o¿enia");
                });
            }

            bool gotLocation = await GetCurrentPosition();
            if (_view.StrLocation != null) { MainActivity.Location = _view.StrLocation.Replace(",", ""); }           

            _progress.DismissActivityIndicator();
            return gotLocation;
        }
    }
}