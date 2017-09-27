using Android.Graphics;
using System.Net;
using System.Threading.Tasks;

namespace Weather.Services
{

    public interface IPhotoUploadService
    {
        void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e);
        Task Upload(Bitmap bitmap);
    }
}