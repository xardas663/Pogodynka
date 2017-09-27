using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Weather.Model;
using Weather.Repositories;
using Weather.Services;
using static Android.Widget.ImageView;

namespace Weather
{
    public class MyView: IView
    {
        public   ProgressDialog Progress { get; set; }
        public   AutoCompleteTextView TxtLocation { get; set; }
        public   string StrLocation { get; set; }        
        public WholeWeather ObjWholeWeather { get; set; }
        public DailyForecastAdapterClass ObjDailyForecastAdapterClass { get; set; }
        public  HourlyForecastAdapterClass ObjHourlyForecastAdapterClass;
        public  List<Forecastday2> LstDailyForeCast;
        public  List<HourlyForecast> LstHourlyForeCast;
        public TextView LblTemp { get; set; }
        public  TextView LblHumidity { get; set; }
        public  TextView LblWindSpeed { get; set; }
        public  TextView LblSunrise { get; set; }
        public  TextView LblSunSet { get; set; }
        public  TextView TxtLabel { get; set; }
        public  TextView LblWearIt { get; set; }
        public  ImageView ImPaula { get; set; }
        public  RelativeLayout FirstLayout { get; set; }
        public  RelativeLayout SecondLayout { get; set; }
        public RelativeLayout ThirdLayout { get; set; }
        public  RelativeLayout RelativeLayout { get; set; }
        public  ListView ListViewDailyForeCast { get; set; }
        public  ListView ListViewHourlyForeCast { get; set; }
        public  Button BtnSearch { get; set; }
        public  Button BtnGps { get; set; }
        public Button BtnPickPhoto { get; set; }
        public Button BtnTakePhoto { get; set; }
        public int Path { get; set; } = Resource.Drawable.Inne;
        public ImageView ImageView { get; set; }
        public TextView TxtConditions { get; set; }   
        public Button BtnSendPhoto { get; set; }
        public TextView TxtResponse { get; set;}
        public ImageView ImUmbrella { get; set; }

        private readonly Context _context;
        private readonly IPhotoConditionsRepository _photoConditionsRepository;

        public MyView(Context context, IPhotoConditionsRepository repository)
        {
            _context = context;
            _photoConditionsRepository = repository;
        }

        private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / _context.Resources.DisplayMetrics.Density);
            return dp;
        }      

        public async Task SetBackground(Model2.RootObjectGooglePhoto root)
        {                        
            if(root != null && root.result != null && root.result.photos != null && MainActivity.Location != "Poznañ" && MainActivity.Location != "Pi³a" && MainActivity.Location != "Niedoradz")
            {
                var metrics = _context.Resources.DisplayMetrics;
                var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
                var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);
                double ratio = (double)heightInDp / widthInDp;                
               
                var photo = root.result.photos.Where(x => (x == null) ? false : (double)x.height / x.width <= ratio + 1 && (double)x.height / x.width >= ratio - 1)                                              
                                              .Select(x => x.photo_reference)
                                              .FirstOrDefault();
                 
                if (photo != null)
                {
                    Bitmap imageBitmap = null;
                    using (var webClient = new WebClient())
                    {
                        var imageBytes = await webClient.DownloadDataTaskAsync(new Uri(string.Format("https://maps.googleapis.com/maps/api/place/photo?maxwidth=500&maxheight=839&photoreference={0}&key=AIzaSyCJZoBUUo7TSWdndlT2HL6er4MbhFCFkNI", photo)));
                       

                        if (imageBytes != null && imageBytes.Length > 0)
                        {
                            imageBitmap = await BitmapFactory.DecodeByteArrayAsync(imageBytes, 0, imageBytes.Length);
                            var bitmapDrawable = new BitmapDrawable(imageBitmap);
                            FirstLayout.SetBackgroundDrawable(bitmapDrawable);
                            SecondLayout.SetBackgroundDrawable(bitmapDrawable);
                            ThirdLayout.SetBackgroundDrawable(bitmapDrawable);
                        }
                        else
                        {
                            FirstLayout.SetBackgroundResource(Resource.Drawable.Inne);
                            SecondLayout.SetBackgroundResource(Resource.Drawable.Inne);
                            ThirdLayout.SetBackgroundResource(Resource.Drawable.Inne);                            
                        }                        
                    }                    
                }
            }

            else
            {
                if (MainActivity.Location == "Poznañ")
                {
                    FirstLayout.SetBackgroundResource(Resource.Drawable.Poznan);
                    SecondLayout.SetBackgroundResource(Resource.Drawable.Poznan);
                    ThirdLayout.SetBackgroundResource(Resource.Drawable.Poznan);                   

                }
                else if (MainActivity.Location == "Pi³a")
                {
                    FirstLayout.SetBackgroundResource(Resource.Drawable.Pila);
                    SecondLayout.SetBackgroundResource(Resource.Drawable.Pila);
                    ThirdLayout.SetBackgroundResource(Resource.Drawable.Pila);                 
                }
                else if (MainActivity.Location == "Niedoradz")
                {
                    FirstLayout.SetBackgroundResource(Resource.Drawable.Niedoradz);
                    SecondLayout.SetBackgroundResource(Resource.Drawable.Niedoradz);
                    ThirdLayout.SetBackgroundResource(Resource.Drawable.Niedoradz);                  
                }
                else
                {
                    FirstLayout.SetBackgroundResource(Resource.Drawable.Inne);
                    SecondLayout.SetBackgroundResource(Resource.Drawable.Inne);
                    ThirdLayout.SetBackgroundResource(Resource.Drawable.Inne);                
                }
            }
           

        }

        public async Task SetImage()
        {
            PhotoRecieverService service = new PhotoRecieverService(_photoConditionsRepository);
            var bitmapDrawable = await service.RecievePhoto();
            if (bitmapDrawable != null)
            {

                if (_photoConditionsRepository.PhotoConditions.Rain > 1)
                {
                    ImPaula.SetImageDrawable(bitmapDrawable);
                    ImPaula.SetScaleType(ScaleType.FitXy);
                    ImUmbrella.SetImageResource(Resource.Drawable.umbrella);
                    ImUmbrella.SetScaleType(ScaleType.FitXy);
                }
                else
                {
                    ImPaula.SetImageDrawable(bitmapDrawable);
                    ImPaula.SetScaleType(ScaleType.FitXy);
                    ImUmbrella.SetImageResource(0);
                }
            }
        }      

        public async Task BindData()

        {
            LstDailyForeCast = ObjWholeWeather.ObjRootObjectDailyForecast.forecast.simpleforecast.forecastday;
            LstHourlyForeCast = ObjWholeWeather.ObjRootObjectHourlyForecast.hourly_forecast;

            ShowElements();            

            LblTemp.Text = string.Format("Temperatura   Max: {0} °C  Min: {1} °C", ObjWholeWeather.ObjRootObjectDailyForecast.forecast.simpleforecast.forecastday[0].high.celsius, ObjWholeWeather.ObjRootObjectDailyForecast.forecast.simpleforecast.forecastday[0].low.celsius);
            LblHumidity.Text = string.Format("Chmury :  {0} ", ObjWholeWeather.ObjRootObjectDailyForecast.forecast.simpleforecast.forecastday[0].conditions);            
            LblSunrise.Text = string.Format("Deszcz :  {0} mm", ObjWholeWeather.ObjRootObjectDailyForecast.forecast.simpleforecast.forecastday[0].qpf_allday.mm);
            LblSunSet.Text = string.Format("Prêdkoœæ wiatru :  {0} km/h", ObjWholeWeather.ObjRootObjectDailyForecast.forecast.simpleforecast.forecastday[0].maxwind.kph);

            TxtConditions.Text = string.Format(@"
                Przez najbli¿sze 10 godzin: 
                Temperatura max: {0} °C, Temperatura min: {1} °C,                 
                Odczuwalna max: {2} °C, Odczuwalna min: {3} °C,                 
                Deszcz max: {4} mm
                Powy¿sze dane zostan¹ zapisane wraz ze zdjêciem
                celem inteligentnego doboru ubioru",
                 _photoConditionsRepository.PhotoConditions.TempMax,
                 _photoConditionsRepository.PhotoConditions.TempMin,
                 _photoConditionsRepository.PhotoConditions.FeelsLikeMax,
                 _photoConditionsRepository.PhotoConditions.FeelsLikeMin,
                 _photoConditionsRepository.PhotoConditions.Rain);     
            
            ObjDailyForecastAdapterClass = new DailyForecastAdapterClass((Activity)_context, LstDailyForeCast);
            if(LstHourlyForeCast!=null) { ObjHourlyForecastAdapterClass = new HourlyForecastAdapterClass((Activity)_context, LstHourlyForeCast); }         
            if(ObjHourlyForecastAdapterClass != null) { ListViewHourlyForeCast.Adapter = ObjHourlyForecastAdapterClass; }
            if (ObjDailyForecastAdapterClass != null) { ListViewDailyForeCast.Adapter = ObjDailyForecastAdapterClass; }                                                    
        }
        
        public void HideElements()
        {
            ImPaula.Visibility = ViewStates.Invisible;
            LblSunrise.Visibility = ViewStates.Invisible;
            LblSunSet.Visibility = ViewStates.Invisible;      
            LblTemp.Visibility = ViewStates.Invisible;
            LblHumidity.Visibility = ViewStates.Invisible;
            RelativeLayout.Visibility = ViewStates.Invisible;
            LblWearIt.Visibility = ViewStates.Invisible;
        }

        public void ShowElements()
        {
            ImPaula.Visibility = ViewStates.Visible;
            LblSunrise.Visibility = ViewStates.Visible;
            LblSunSet.Visibility = ViewStates.Visible;
            LblTemp.Visibility = ViewStates.Visible;
            LblHumidity.Visibility = ViewStates.Visible;
            RelativeLayout.Visibility = ViewStates.Visible;
            LblWearIt.Visibility = ViewStates.Visible;
        }
    }
}