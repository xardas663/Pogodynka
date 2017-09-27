using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Widget;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Weather.Model;
using Weather.Repositories;



namespace Weather.Services
{
    public class PhotoUploadService:IPhotoUploadService
    {
        private readonly IPhotoConditionsRepository _repository;
        private readonly Activity _activity;
        private readonly IView _view;

        string _filename;
        
        public PhotoUploadService(IPhotoConditionsRepository repository,IView view, Context context)
        {
            _repository = repository;
            _view = view;
            _activity = (Activity)context;
        }
        public async Task Upload(Bitmap bitmap)
        {
            var memStream = new MemoryStream();
            
            if (bitmap != null)
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 50, memStream);
                byte[] bitmapData = memStream.ToArray();
                var fileContent = new ByteArrayContent(bitmapData);

                _filename = DateTime.Now.ToString("yyyyMMddHHmmss")+".png";              

                var content = new MultipartFormDataContent();
                content.Add(fileContent,_filename,_filename);                
                var httpClient = new HttpClient();

                var uploadServiceAdress = "http://paulinkapogodynka.azurewebsites.net/api/Files/Upload";
                //var uploadServiceAdress = "http://pogodynkaapi.azurewebsites.net/Upload";
                var httpResponseMessage = await httpClient.PostAsync(uploadServiceAdress, content);
                
                var response = await httpResponseMessage.Content.ReadAsStringAsync();
                var filename = $"\"{_filename}\"";


                if (response==filename)
                {
                    _view.TxtResponse.Text = "http://paulinkapogodynka.azurewebsites.net/Uploads/"+_filename;
                    //_view.TxtResponse.Text = "http://pogodynkaapi.azurewebsites.net/Uploads/" + _filename;
                    _repository.PhotoConditions.ImagePath = _filename;
                    //string apiUrl = @"http://paulinkapogodynka.azurewebsites.net/api/Files/PhotoConditionsPost";
                    string apiUrl = @"http://pogodynkaapi.azurewebsites.net/Upload/add";
                    var json = JsonConvert.SerializeObject(_repository.PhotoConditions);
                    var contentRepo = new StringContent(json, Encoding.UTF8, "application/json");
                    var clientRepo = new HttpClient();
                    HttpResponseMessage responseRepo = null;
                    responseRepo = await clientRepo.PostAsync(apiUrl, contentRepo);                    
                }

                else
                {
                    _view.TxtResponse.Text = "Coœ posz³o Ÿle :( ";
                }
            }
        }

        public void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {            
            _activity.RunOnUiThread(() => {
                Toast.MakeText(_activity, "Wys³ano zdjêcie ! ", ToastLength.Long).Show();
            });
        }
    }
}