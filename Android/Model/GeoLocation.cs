using System.Collections.Generic;
using Weather.Model;

namespace Weather.Model2
{
    
    public class Photo
    {
        public int height { get; set; }
        public List<string> html_attributions { get; set; }
        public string photo_reference { get; set; }
        public int width { get; set; }
    }

    public class Result
    {
        public List<AddressComponent> address_components { get; set; }
        public string formatted_address { get; set; }
        public Geometry geometry { get; set; }
        public string place_id { get; set; }
        public List<string> types { get; set; }

        public string adr_address { get; set; }
     
        public string icon { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public List<Photo> photos { get; set; }
        public string reference { get; set; }
        public string scope { get; set; }
        public string url { get; set; }
        public int utc_offset { get; set; }
        public string vicinity { get; set; }
    }

    public class RootObjectGooglePhoto
    {
        public List<object> html_attributions { get; set; }
        public Result result { get; set; }
        public List<Result> results { get; set; }
        public string status { get; set; }
    }
    public class AddressComponent
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public List<string> types { get; set; }
    }

    public class Northeast
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Southwest
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Bounds
    {
        public Northeast northeast { get; set; }
        public Southwest southwest { get; set; }
    }

    public class Location
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Northeast2
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Southwest2
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Viewport
    {
        public Northeast2 northeast { get; set; }
        public Southwest2 southwest { get; set; }
    }

    public class Geometry
    {
        public Bounds bounds { get; set; }
        public Location location { get; set; }
        public string location_type { get; set; }
        public Viewport viewport { get; set; }
    }    

  
}