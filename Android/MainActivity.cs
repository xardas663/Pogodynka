using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Widget;
using System;
using Weather.Model;
using Android.Views.InputMethods;
using Weather.Services;
using Weather.Repositories;
using Android.Runtime;
using Android.Graphics;
using System.IO;
using Android.Content.PM;
using Android.Provider;
using Uri = Android.Net.Uri;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;
using Android.Views;
using Autofac;

namespace Weather
{
    [Activity(Label = "Jak się ubrać?", MainLauncher = false, Icon = "@drawable/Icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : FragmentActivity
    {
        private IView _myView;
        private IApiService _api;
        private IGpsProviderService _gpsProvider;
        private ISettingsService _settingsService;
        private IWeatherRepository _weatherRepository;
        private IPhotoPicker _photoPicker;
        private IPhotoTaker _photoTaker;
        private IPhotoUploadService _upload;
        private IPhotoConditionsRepository _photoConditionsRepository;
        private Bitmap Bitmap;

        static public string Location;   
        private int _i;

        protected override void OnCreate(Bundle bundle)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;
            System.Security.Cryptography.AesCryptoServiceProvider b = new System.Security.Cryptography.AesCryptoServiceProvider();

            base.OnCreate(bundle);
            MobileCenter.Start("25761872-63f8-4097-8317-f27f4447c996",
                         typeof(Analytics), typeof(Crashes));        

            SetContentView(Resource.Layout.Main);
            CreatePages();
            Initialize();
            _settingsService.RetrieveSettings();
         }
        protected override async void OnActivityResult(int requestCode, [GeneratedEnum] Android.App.Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            switch (requestCode)
            {
                case 0:
                    if (resultCode == Android.App.Result.Ok)
                    {
                        Stream stream = ContentResolver.OpenInputStream(data.Data);
                        Bitmap = await _photoPicker.DecodeBitmapFromStreamAsync(data.Data, 200, 200);
                        if (Bitmap != null)
                        {
                            _myView.ImageView.SetImageBitmap(Bitmap);
                        }
                    }
                    break;

                case 1:
                    Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                    Uri contentUri = Uri.FromFile(App.File);
                    mediaScanIntent.SetData(contentUri);
                    SendBroadcast(mediaScanIntent);
                    App.Bitmap = App.File.Path.LoadAndResizeBitmap(300, 300);
                    if (App.Bitmap != null)
                    {
                        _myView.ImageView.SetImageBitmap(App.Bitmap);
                        App.Bitmap = null;
                    }
                    GC.Collect();
                    break;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _settingsService.SaveSettings(Location);
        }

        protected override void OnStop()
        {
            base.OnStop();
            _settingsService.SaveSettings(Location);
        }

        private void TakeAPicture(object sender, EventArgs eventArgs)
        {
            if (_photoTaker.IsThereAnAppToTakePictures())
            {
                _photoTaker.CreateDirectoryForPictures();
                Intent intent = new Intent(MediaStore.ActionImageCapture);
                App.File = new Java.IO.File(App.Dir, string.Format("myPhoto_{0}.jpg", System.Guid.NewGuid()));
                intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(App.File));
                StartActivityForResult(intent, 1);
            }           
        }
     
        private void Initialize()
        {
            Action<ImageView> picAction = PicSelected;
            var builder = new ContainerBuilder();
            //builder.RegisterType<PhotoConditionsRepository>()
            //          .As<IPhotoConditionsRepository>()
            //          .SingleInstance();
            //builder.RegisterType<MyView>()
            //         .As<IView>().SingleInstance();
            //builder.RegisterType<ApiService>()
            //         .As<IApiService>().SingleInstance();
            //builder.RegisterType<WeatherRepository>()
            //         .As<IWeatherRepository>().SingleInstance();
            //builder.RegisterType<GpsProviderService>()
            //      .As<IGpsProviderService>().SingleInstance();
            //builder.RegisterType<SettingsService>()
            //         .As<ISettingsService>().SingleInstance();
            //builder.RegisterType<PhotoPicker>()
            //        .As<IPhotoPicker>().SingleInstance();
            //builder.RegisterType<PhotoTaker>()
            //      .As<IPhotoTaker>().SingleInstance();
            //builder.RegisterType<PhotoUploadService>()
            //      .As<IPhotoUploadService>().SingleInstance();
            //builder.Build();

            _photoConditionsRepository = new PhotoConditionsRepository();
            _myView = new MyView(this, _photoConditionsRepository);
            _api = new ApiService(this, _myView, _photoConditionsRepository);
            _weatherRepository = new WeatherRepository(_api);
            _gpsProvider = new GpsProviderService(this, _api, _myView as MyView);
            _settingsService = new SettingsService();
            _photoPicker = new PhotoPicker(this, picAction, _myView);
            _photoTaker = new PhotoTaker(this);
            _upload = new PhotoUploadService(_photoConditionsRepository, _myView, this);
        }

        private void PicSelected(ImageView selectedPic)
        {
            _myView.ImageView = selectedPic;
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(intent, "Wybierz zdjęcie"), 0);
        }    

        private void CreatePages()
        {
            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;                      
            var pager = FindViewById<ViewPager>(Resource.Id.pager);
            pager.OffscreenPageLimit = 3;           
            var adaptor = new GenericFragmentPagerAdaptor(SupportFragmentManager);

            adaptor.AddFragmentView((i, v, b) =>
            {
                var view = i.Inflate(Resource.Layout.First, v, false);
                InitializeTab1Components(view);                             

                return view;
            });

            adaptor.AddFragmentView((i, v, b) =>
            {
                var view = i.Inflate(Resource.Layout.Second, v, false);
                InitializeTab2Components(view);

                return view;
            });

            adaptor.AddFragmentView((i, v, b) =>
            {
                var view = i.Inflate(Resource.Layout.Third, v, false);
                InitializeTab3Components(view);

                return view;
            });

            adaptor.AddFragmentView((i, v, b) =>
            {                
                var view = i.Inflate(Resource.Layout.Fourth, v, false);
                InitializeTab4Elements(view);
               
                return view;
            });
            
            pager.Adapter = adaptor;
            pager.AddOnPageChangeListener(new ViewPageListenerForActionBar(ActionBar));                      
          
            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "DZIŚ"));
            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "GODZINOWO"));
            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "KOLEJNE DNI"));
            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "ZDJĘCIA"));
        }

        private void InitializeTab3Components(View view)
        {
            _myView.ThirdLayout = view.FindViewById<RelativeLayout>(Resource.Id.ThirdLayout);

            _myView.ThirdLayout.SetBackgroundResource(_myView.Path);
            _myView.ListViewDailyForeCast = view.FindViewById<ListView>(Resource.Id.ListViewDailyForeCast);
        }

        private void InitializeTab2Components(View view)
        {
            _myView.SecondLayout = view.FindViewById<RelativeLayout>(Resource.Id.SecondLayout);
            _myView.ListViewHourlyForeCast = view.FindViewById<ListView>(Resource.Id.ListViewHourlyForeCast);
        }

        private void InitializeTab1Components(View view)
        {
            _myView.FirstLayout = view.FindViewById<RelativeLayout>(Resource.Id.FirstLayout);
            _myView.FirstLayout.SetBackgroundResource(Resource.Drawable.Inne);
            _myView.RelativeLayout = view.FindViewById<RelativeLayout>(Resource.Id.rlTopMainContainer);
            _myView.TxtLocation = view.FindViewById<AutoCompleteTextView>(Resource.Id.txtSearch);            
            _myView.TxtLabel = view.FindViewById<TextView>(Resource.Id.lblLabel);
            _myView.ImPaula = view.FindViewById<ImageView>(Resource.Id.imageView1);
            _myView.LblSunrise = view.FindViewById<TextView>(Resource.Id.lblSunrise);
            _myView.LblSunSet = view.FindViewById<TextView>(Resource.Id.lblSunset);            
            _myView.LblTemp = view.FindViewById<TextView>(Resource.Id.lblTempWC);
            _myView.LblHumidity = view.FindViewById<TextView>(Resource.Id.lblHumidity);
            _myView.BtnSearch = view.FindViewById<Button>(Resource.Id.btnSerach);
            _myView.BtnGps = view.FindViewById<Button>(Resource.Id.btnGps);
            _myView.LblWearIt = view.FindViewById<TextView>(Resource.Id.lblWearIt);
            _myView.ImUmbrella = view.FindViewById<ImageView>(Resource.Id.imUmbrella);

            _myView.HideElements();            

            if (Location != null)
            {
                _myView.TxtLocation.Text = Location;
            }
            _myView.ImPaula.Click += ImPaula_Click;

            _myView.BtnSearch.Click += BtnSearch_Click;
            _myView.BtnGps.Click += BtnGps_Click;
        }

        private void ImPaula_Click(object sender, EventArgs e)
        {
            _i++;
            if (_i==5)
                {                
                    _myView.ImPaula.SetImageResource(Resource.Drawable.bonus);                         
                }
            if (_i == 8)
            {
                _myView.ImPaula.SetImageResource(Resource.Drawable.bonus2);
            }
        }

        private void InitializeTab4Elements(Android.Views.View view)
        {
            _myView.BtnPickPhoto = view.FindViewById<Button>(Resource.Id.btnPickPhoto);
            _myView.BtnSendPhoto = view.FindViewById<Button>(Resource.Id.btnSendPhoto);
            _myView.BtnTakePhoto = view.FindViewById<Button>(Resource.Id.btnTakePhoto);
            _myView.ImageView = view.FindViewById<ImageView>(Resource.Id.myImageView);
            _myView.TxtConditions = view.FindViewById<TextView>(Resource.Id.txtConditions);
            _myView.TxtResponse = view.FindViewById<TextView>(Resource.Id.txtResponse);          

            _myView.BtnPickPhoto.Click -= _photoPicker.BtnPickPhoto_Click1;
            _myView.BtnPickPhoto.Click += _photoPicker.BtnPickPhoto_Click1;
            _myView.BtnTakePhoto.Click += TakeAPicture;
            _myView.BtnSendPhoto.Click += BtnSendPhoto_Click;
        }

        private void BtnSendPhoto_Click(object sender, EventArgs e)
        {           
            _upload.Upload(Bitmap);
            _myView.TxtResponse.Text = "Wysyłam zdjęcie...";
        }

        
        private async void BtnGps_Click(object sender, EventArgs e)
        {
            bool gotLocaion = await _gpsProvider.RecieveLocationFromGps();
            if (gotLocaion) { await _weatherRepository.Recieve(Location); }
        }
        private async void BtnSearch_Click(object sender, EventArgs e)
        {
            InputMethodManager imm = (InputMethodManager)GetSystemService(InputMethodService);
            imm.HideSoftInputFromWindow(_myView.TxtLocation.WindowToken, 0);
            await _weatherRepository.Recieve(_myView.TxtLocation.Text);
        }
    }
}

    








