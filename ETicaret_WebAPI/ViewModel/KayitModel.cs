using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETicaret_WebAPI.ViewModel
{
    public class KayitModel
    {
        public int kayitId { get; set; }
        public int kayitUrunId { get; set; }
        public int kayitKatId { get; set; }
        public UrunModel urunBilgi { get; set; }
        public KategoriModel kategoriBilgi { get; set; }
    }
}