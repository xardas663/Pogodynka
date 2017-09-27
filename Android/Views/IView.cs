using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;

namespace Weather.Model
{
    public interface IView
    {
        Task SetBackground(Model2.RootObjectGooglePhoto root);
        Task SetImage();
        Task BindData();
        void ShowElements();
        void HideElements();        

        int Path { get; set; }
        string StrLocation { get; set; }

        TextView TxtConditions { get; set; }
        TextView LblTemp { get; set; }
        TextView LblHumidity { get; set; }
        TextView LblWindSpeed { get; set; }
        TextView LblSunrise { get; set; }
        TextView LblSunSet { get; set; }
        TextView TxtLabel { get; set; }
        TextView LblWearIt { get; set; }
        TextView TxtResponse { get; set; }
        ImageView ImPaula { get; set; }
        ImageView ImUmbrella { get; set; }
        ImageView ImageView { get; set; }
        RelativeLayout FirstLayout { get; set; }
        RelativeLayout SecondLayout { get; set; }
        RelativeLayout ThirdLayout { get; set; }
        RelativeLayout RelativeLayout { get; set; }
        ListView ListViewDailyForeCast { get; set; }
        ListView ListViewHourlyForeCast { get; set; }
        Button BtnSearch { get; set; }
        Button BtnGps { get; set; }
        Button BtnPickPhoto { get; set; }
        Button BtnTakePhoto { get; set; }
        Button BtnSendPhoto { get; set; }
        ProgressDialog Progress { get; set; }
        AutoCompleteTextView TxtLocation { get; set; }        
        DailyForecastAdapterClass ObjDailyForecastAdapterClass { get; set; }
        WholeWeather ObjWholeWeather { get; set; }
    }
}