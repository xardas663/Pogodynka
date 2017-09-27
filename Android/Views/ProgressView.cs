using Android.App;
using Android.Widget;
using Weather.Model;

namespace Weather
{

    public class ProgressView
    {
        private readonly IView _view;
        private readonly Activity _activity;
        public ProgressView(Activity activity,IView view)
        {
            _activity = activity;
            _view = view;           
        }

        public void DismissActivityIndicator()
        {
            _activity.RunOnUiThread(() =>
            {
                if (_view.Progress != null)
                {
                    _view.Progress.Dismiss();
                    _view.Progress = null;
                }
            });
        }

        public void ThrowServerError(string serverName)
        {
            string message = string.Format("Błąd, serwer {0} nie odpowiada :(", serverName);
            _activity.RunOnUiThread(() =>
            {
                Toast.MakeText(_activity, message, ToastLength.Short).Show();
            });
        }
        public void ThrowBadRequest()
        {
            string message = "Spróbuj wpisać inną lokalizację ";
            _activity.RunOnUiThread(() =>
            {
                Toast.MakeText(_activity, message, ToastLength.Short).Show();
            });
        }
    }
}