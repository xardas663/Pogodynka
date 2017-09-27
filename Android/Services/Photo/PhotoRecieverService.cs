using Android.Graphics;
using Android.Graphics.Drawables;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Weather.Model;
using Weather.Repositories;

namespace Weather.Services
{
    public class PhotoRecieverService
    {
        private readonly IPhotoConditionsRepository _repository;
        public PhotoRecieverService(IPhotoConditionsRepository reposiotry)
        {
            _repository = reposiotry;
        }
        public async Task<BitmapDrawable>  RecievePhoto()
        {
            Bitmap imageBitmap = null;
            
            string apiUrl = @"http://pogodynkaapi.azurewebsites.net/Conditions";
            var json = JsonConvert.SerializeObject(_repository.PhotoConditions);
            var contentRepo = new StringContent(json, Encoding.UTF8, "application/json");
            var clientRepo = new HttpClient();
            HttpResponseMessage responseRepo = null;

            responseRepo = await clientRepo.PostAsync(apiUrl, contentRepo);
            var response = await responseRepo.Content.ReadAsStringAsync();

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore           

            };
            PhotoConditions photoRecieved = new PhotoConditions();
            photoRecieved=JsonConvert.DeserializeObject<PhotoConditions>(response, settings);
                       
            using (var webClient = new WebClient())
            {
                var imageBytes = await webClient.DownloadDataTaskAsync(string.Format("http://paulinkapogodynka.azurewebsites.net/Uploads/{0}", photoRecieved.ImagePath));
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = await BitmapFactory.DecodeByteArrayAsync(imageBytes, 0, imageBytes.Length);
                    var bitmapDrawable = new BitmapDrawable(imageBitmap);
                    return bitmapDrawable;
                }
                return null;
            }
            
        }
    }
}