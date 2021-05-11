using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETicaret_WebAPI.ViewModel
{
    public class SepetModel
    {
        public int sepetId { get; set; }
        public int sepetSiparisId { get; set; }
        public int sepetUrunId { get; set; }
        public string urunAdet { get; set; }
        public decimal toplamFiyat { get; set; }
    }
}