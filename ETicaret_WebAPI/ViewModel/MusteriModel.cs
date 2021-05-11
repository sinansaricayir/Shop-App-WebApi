using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETicaret_WebAPI.ViewModel
{
    public class MusteriModel
    {
        public int musteriId { get; set; }
        public string musteriAd { get; set; }
        public string musteriSoyad { get; set; }
        public string musteriTel { get; set; }
        public string musteriMail { get; set; }
        public string musteriSifre { get; set; }
    }
}