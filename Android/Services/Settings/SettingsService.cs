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

namespace Weather.Services
{
    public class SettingsService:ISettingsService
    {
        public void SaveSettings(string location)
        {
            var preferences = Application.Context.GetSharedPreferences("Weather", FileCreationMode.WorldWriteable);
            var prefEditor = preferences.Edit();

            prefEditor.PutString("location", location);
            prefEditor.Commit();
        }

        public void RetrieveSettings()
        {
            var prefs = Application.Context.GetSharedPreferences("Weather", FileCreationMode.WorldReadable);
            var someprefs = prefs.GetString("location", null);

            MainActivity.Location = someprefs;
        }
    }
}