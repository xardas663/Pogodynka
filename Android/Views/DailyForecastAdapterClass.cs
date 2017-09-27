using Android.Widget;
using Android.App;
using System.Collections.Generic;
using Android.Views;

namespace Weather.Model
{
    public class DailyForecastAdapterClass : BaseAdapter
    {
        Activity _context;
        List<Forecastday2> _lstForecast;        
        
        public DailyForecastAdapterClass(Activity c, List<Forecastday2> lstForecast)
        {
            _context = c;
            this._lstForecast = lstForecast;
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
                view = _context.LayoutInflater.Inflate(Resource.Layout.ForecastCustomLayout, parent, false);
                objViewHolderClass = new ViewHolderClass();
                objViewHolderClass.ImgWeather = view.FindViewById<ImageView>(Resource.Id.imgWeatherIconFCL);
                objViewHolderClass.TxtTextCondition = view.FindViewById<TextView>(Resource.Id.lblTextFCL);
                objViewHolderClass.TxtHigh = view.FindViewById<TextView>(Resource.Id.lblHighFCL);
                objViewHolderClass.TxtLow = view.FindViewById<TextView>(Resource.Id.lblLowFCL);
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

            objViewHolderClass.TxtDay.Text = string.Format("{0}", _lstForecast[position].date.weekday);
            objViewHolderClass.TxtTextCondition.Text = _lstForecast[position].conditions;
            objViewHolderClass.TxtHigh.Text = string.Format("Max: {0} °C", _lstForecast[position].high.celsius);
            objViewHolderClass.TxtLow.Text = string.Format("Min: {0} °C", _lstForecast[position].low.celsius);
            int resImage = _context.Resources.GetIdentifier(_lstForecast[position].icon, "drawable", "com.paulinka");
            objViewHolderClass.ImgWeather.SetImageResource(resImage);     
            return view;
        }
        public class ViewHolderClass : Java.Lang.Object
        {
            public ImageView ImgWeather;
            public TextView TxtDay;
            public TextView TxtTextCondition;
            public TextView TxtHigh;
            public TextView TxtLow;
        }
    }


}

