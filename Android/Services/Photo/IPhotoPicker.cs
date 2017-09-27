using System;
using Android.Graphics;
using System.Threading.Tasks;

namespace Weather.Services
{   
    public interface  IPhotoPicker
    {
        void BtnPickPhoto_Click1(object sender, EventArgs e);
        int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight);
        Task<Bitmap> DecodeBitmapFromStreamAsync(Android.Net.Uri data, int requestedWidth, int requestedHeight);
    }
}