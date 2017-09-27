using System;
using System.Collections.Generic;
using System.Text;

namespace Pogodynka.Core.Domain
{
    public class Images
    {        
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public int TempMax { get; set; }
        public int TempMin { get; set; }
        public int FeelsLikeMax { get; set; }
        public int FeelsLikeMin { get; set; }
        public int Rain { get; set; }
        public Images()
        {

        }
    }    
}
