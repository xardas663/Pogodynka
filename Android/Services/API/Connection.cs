using System;
using Android.App;
using Android.Content;
using Android.Net;

namespace Weather
{
    public class Connection
    {
        private readonly Activity _activity;
        public Connection(Activity activity)
        {
            _activity = activity;
        }
        public bool IsConnected()
        {
            try
            {
                var connectionManager = (ConnectivityManager)_activity.GetSystemService(Context.ConnectivityService);
                NetworkInfo networkInfo = connectionManager.ActiveNetworkInfo;
                if (networkInfo != null && networkInfo.IsConnected)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

    }
}