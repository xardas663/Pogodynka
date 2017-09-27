using Android.Content;
using Android.Content.PM;
using System.Collections.Generic;
using Android.Provider;

namespace Weather.Services
{

    public class PhotoTaker : IPhotoTaker
    {
        Context _context;
        public PhotoTaker(Context context)
        {
            _context = context;
        }
        public void CreateDirectoryForPictures()
        {
            App.Dir = new Java.IO.File(
             Android.OS.Environment.GetExternalStoragePublicDirectory(
                 Android.OS.Environment.DirectoryPictures), "CameraAppDemo");
            if (!App.Dir.Exists())
            {
                App.Dir.Mkdirs();
            }
        }


        public bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities =
            _context.PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }
    }
}