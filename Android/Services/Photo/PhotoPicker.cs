using System;
using Android.Content;
using Android.Widget;
using Android.Graphics;
using Weather.Model;
using System.Threading.Tasks;
using Android.Media;

namespace Weather.Services
{

    public class PhotoPicker:IPhotoPicker
    {
        private readonly Action<ImageView> _picSelected;
        private readonly Context _context;
        private readonly IView _view; 
        public PhotoPicker(Context context, Action<ImageView> picSelected, IView view)
        {
            _context = context;
            _picSelected = picSelected;
            _view = view;        

        }

        public async Task<Bitmap> DecodeBitmapFromStreamAsync(Android.Net.Uri data, int requestedWidth, int requestedHeight)
        {
            
            System.IO.Stream stream2 = _context.ContentResolver.OpenInputStream(data);
            BitmapFactory.Options options = new BitmapFactory.Options
            {
                InJustDecodeBounds = true
            };

            await BitmapFactory.DecodeStreamAsync(stream2, null, options);
            stream2.Dispose();
            int imageHeight = options.OutHeight;
            int imageWidth = options.OutWidth;

            options.InSampleSize = CalculateInSampleSize(options, requestedWidth, requestedHeight);

            stream2 = _context.ContentResolver.OpenInputStream(data); //Must read again

            options.InJustDecodeBounds = false;
            Bitmap bitmap = await BitmapFactory.DecodeStreamAsync(stream2, null, options);
            return bitmap;
        }



        public void BtnPickPhoto_Click1(object sender, EventArgs e)
        {
            _picSelected.Invoke(_view.ImageView);
        }

        
        public int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            float height = options.OutHeight;
            float width = options.OutWidth;
            double inSampleSize = 1D;

            if (height > reqHeight || width > reqWidth)
            {
                int halfHeight = (int)(height / 2);
                int halfWidth = (int)(width / 2);

                while ((halfHeight / inSampleSize) > reqHeight && (halfWidth / inSampleSize) > reqWidth)
                {
                    inSampleSize *= 2;
                }
            }

            return (int)inSampleSize;
        }
    }
}