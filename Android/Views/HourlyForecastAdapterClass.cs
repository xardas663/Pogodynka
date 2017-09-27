using Android.Widget;
using Android.App;
using System.Collections.Generic;
using Android.Views;
using Android.Content.Res;

namespace Weather.Model
{
    public class HourlyForecastAdapterClass : BaseAdapter
    {
        Activity _context;
        List<HourlyForecast> _lstForecast;
      
        public HourlyForecastAdapterClass(Activity c, List<HourlyForecast> lstForecast)
        {
            _context = c;
            this._lstForecast = lstForecast;
            if (lstForecast.Count > 24) { lstForecast.RemoveRange(24, lstForecast.Count - 24); }
           
        }
        public override int Count
        {
            get { return _lstForecast.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override void NotifyDataSetChanged()
        {
            base.NotifyDataSetChanged();    
        }

        public override long GetItemId(int position)
        {
            return 0;
        }
        
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            ViewHolderClass objViewHolderClass;
            if (view==null)
            {
                view = _context.LayoutInflater.Inflate(Resource.Layout.ForecastHourly, parent, false);
                objViewHolderClass = new ViewHolderClass();
                objViewHolderClass.ImgWeather = view.FindViewById<ImageView>(Resource.Id.imgWeatherIconFCL);
                objViewHolderClass.TxtTextCondition = view.FindViewById<TextView>(Resource.Id.lblTextFCL);
                objViewHolderClass.TxtTemp = view.FindViewById<TextView>(Resource.Id.lblTempFCL);
                objViewHolderClass.TxtRain = view.FindViewById<TextView>(Resource.Id.lblRainFCL);
                objViewHolderClass.TxtWind = view.FindViewById<TextView>(Resource.Id.lblWindFCL);
                objViewHolderClass.TxtDay = view.FindViewById<TextView>(Resource.Id.lbldateDayFCL);
                view.Tag = objViewHolderClass;
            }
            else
            {
                objViewHolderClass = (ViewHolderClass)view.Tag;
            }
            if (position == 0)
            {
                view.SetBackgroundColor(Android.Graphics.Color.ParseColor("#99000000"));
            }
            else if (position % 2 == 0)
            {
                view.SetBackgroundColor(Android.Graphics.Color.ParseColor("#80000000"));
            }


            objViewHolderClass.TxtDay.Text= string.Format("Godzina: {0}.00", _lstForecast[position].FCTTIME.hour);
            objViewHolderClass.TxtTextCondition.Text = string.Format("{0}", _lstForecast[position].condition);
            objViewHolderClass.TxtTemp.Text = string.Format("Temperatura: {0}°C", _lstForecast[position].temp.metric);
            objViewHolderClass.TxtRain.Text = string.Format("Odczuwalna: {0}°C", _lstForecast[position].feelslike.metric);
            objViewHolderClass.TxtWind.Text = string.Format("Opady: {0}mm", _lstForecast[position].qpf.metric);
            int resImage = _context.Resources.GetIdentifier(_lstForecast[position].icon, "drawable", "com.paulinka");
            objViewHolderClass.ImgWeather.SetImageResource(resImage);          

            return view;
        }
        public class ViewHolderClass : Java.Lang.Object
        {
            public ImageView ImgWeather;
            public TextView TxtDay;
            public TextView TxtTextCondition;
            public TextView TxtTemp;
            public TextView TxtRain;
            public TextView TxtWind;
        }
    }


}

