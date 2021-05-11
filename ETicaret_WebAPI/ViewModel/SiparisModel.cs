using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETicaret_WebAPI.ViewModel
{
    public class SiparisModel
    {
        public int siparisId { get; set; }
        public decimal araToplam { get; set; }
        public decimal kdvOranı { get; set; }
        public decimal genelToplam { get; set; }
        public string siparisTarih { get; set; }
        public string siparisAdres { get; set; }
        public string siparisIl { get; set; }
        public string siparisIlce { get; set; }
        public int siparisMusteriId { get; set; }
    }
}