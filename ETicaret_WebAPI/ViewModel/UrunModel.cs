using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETicaret_WebAPI.ViewModel
{
    public class UrunModel
    {
        public int urunId { get; set; }
        public string urunAd { get; set; }
        public decimal urunFiyat { get; set; }
        public string urunGorsel { get; set; }
        public string urunAciklama { get; set; }
        public int urunKatId { get; set; }
    }
}