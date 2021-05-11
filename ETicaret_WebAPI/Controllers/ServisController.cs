using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ETicaret_WebAPI.Models;
using ETicaret_WebAPI.ViewModel;

namespace ETicaret_WebAPI.Controllers
{
    public class ServisController : ApiController
    {
        ETicaretDbEntities1 db = new ETicaretDbEntities1();
        SonucModel sonuc = new SonucModel();

        #region Kategori

        [HttpGet]
        [Route("api/kategoriliste")]
        public List<KategoriModel> KategoriListe()
        {
            List<KategoriModel> liste = db.Kategori.Select(x => new KategoriModel()
            {
                kategoriId = x.kategoriId,
                kategoriAd = x.kategoriAd
            }).ToList();
            return liste;
        }

        [HttpGet]
        [Route("api/kategoribyid/{kategoriId}")]
        public KategoriModel KategoriById(int kategoriId)
        {
            KategoriModel kayit = db.Kategori.Where(s => s.kategoriId == kategoriId).Select(x => new KategoriModel()
            {
                kategoriId = x.kategoriId,
                kategoriAd = x.kategoriAd
            }).FirstOrDefault();
            return kayit;
        }


        [HttpPost]
        [Route("api/kategoriekle")]
        public SonucModel KategoriEkle(KategoriModel model)
        {
            if (db.Kategori.Count(s => s.kategoriAd == model.kategoriAd) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kategori bilgileri sistemde mevcuttur";
                return sonuc;
            }
            Kategori yeni = new Kategori();
            yeni.kategoriAd = model.kategoriAd;
            db.Kategori.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kategori bilgileri sisteme eklenmiştir.";
            return sonuc;
        }

        [HttpPut]
        [Route("api/kategoriduzenle")]

        public SonucModel KategoriDuzenle(KategoriModel model)
        {
            Kategori kayit = db.Kategori.Where(s => s.kategoriId == model.kategoriId).FirstOrDefault(); if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kategori bilgileri bulunamadı. Lütfen geçerli bir kategori seçiniz.";
                return sonuc;
            }
            kayit.kategoriAd = model.kategoriAd;
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kategori bilgileri başarılı bir şekilde düzenlenmiştir.";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/kategorisil/{kategoriid}")]
        public SonucModel KategoriSil(int kategoriId)
        {
            Kategori kayit = db.Kategori.Where(s => s.kategoriId == kategoriId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kategori bilgileri bulunamadı.";
                return sonuc;
            }
            if (db.Urun.Count(s => s.urunKatId == kategoriId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Ürün kaydı bulunan kategori silinemez. Ürünleri siliniz.";
                return sonuc;
            }
            db.Kategori.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kategori bilgileri başarılı bir şekilde silindi.";
            return sonuc;
        }

        #endregion

        #region Ürün

        [HttpGet]
        [Route("api/urunliste")]
        public List<UrunModel> UrunListe()
        {
            List<UrunModel> liste = db.Urun.Select(x => new UrunModel()
            {
                urunId = x.urunId,
                urunAd = x.urunAd,
                urunFiyat = x.urunFiyat,
                urunGorsel = x.urunGorsel,
                urunAciklama = x.urunAciklama,
                urunKatId = x.urunKatId,

            }).ToList();
            return liste;
        }
        [HttpGet]
        [Route("api/urunlistebykatid/{kategoriId}")]
        public List<UrunModel> UrunListeByKatId(int kategoriId)
        {
            List<UrunModel> liste = db.Urun.Where(s => s.urunKatId == kategoriId).Select(x => new UrunModel()
            {
                urunId = x.urunId,
                urunAd = x.urunAd,
                urunFiyat = x.urunFiyat,
                urunGorsel = x.urunGorsel,
                urunAciklama = x.urunAciklama,
                urunKatId = x.urunKatId
            }).ToList();
            return liste;
        }
        [HttpGet]
        [Route("api/urunbyid/{urunId}")]
        public UrunModel UrunById(int urunId)
        {
            UrunModel kayit = db.Urun.Where(s => s.urunId == urunId).Select(x => new UrunModel()
            {
                urunId = x.urunId,
                urunAd = x.urunAd,
                urunFiyat = x.urunFiyat,
                urunGorsel = x.urunGorsel,
                urunAciklama = x.urunAciklama,
                urunKatId = x.urunKatId
            }
            ).FirstOrDefault();
            return kayit;
        }

        [HttpPost]
        [Route("api/urunekle")]
        public SonucModel UrunEkle(UrunModel model)
        {
            if (db.Urun.Count(s => s.urunAd == model.urunAd && s.urunKatId == model.urunKatId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Ürün ilgili kategoride mevcuttur.";
                return sonuc;
            }
            Urun yeni = new Urun();
            yeni.urunAd = model.urunAd;
            yeni.urunFiyat = model.urunFiyat;
            yeni.urunGorsel = model.urunGorsel;
            yeni.urunAciklama = model.urunAciklama;
            yeni.urunKatId = model.urunKatId;
            db.Urun.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Ürün bilgileri başarılı bir şekilde sisteme eklenmiştir.";
            return sonuc;
        }
        [HttpPut]
        [Route("api/urunduzenle")]
        public SonucModel UrunDuzenle(UrunModel model)
        {
            Urun kayit = db.Urun.Where(s => s.urunId == model.urunId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girdiğiniz bilgilere ait ürün bulunamadı.";
                return sonuc;
            }
            kayit.urunAd = model.urunAd;
            kayit.urunFiyat = model.urunFiyat;
            kayit.urunGorsel = model.urunGorsel;
            kayit.urunAciklama = model.urunAciklama;
            kayit.urunKatId = model.urunKatId;
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Ürün bilgileri başarılı bir şekilde güncellenmiştir.";
            return sonuc;
        }
        [HttpDelete]
        [Route("api/urunsil/{urunId}")]
        public SonucModel UrunSil(int urunId)
        {
            Urun kayit = db.Urun.Where(s => s.urunId == urunId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Ürün bulunamadı.";
                return sonuc;
            }
            db.Urun.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Ürün bilgileri sistemden silinmiştir.";
            return sonuc;
        }

        [HttpPost]
        [Route("api/urunfotoguncelle")]
        public SonucModel UrunFotoGuncelle(UrunFotoModel model)
        {
            Urun urun = db.Urun.Where(s => s.urunId == model.urunId).SingleOrDefault();
            if (urun == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt bulunamadı.";
                return sonuc;
            }

            if (urun.urunGorsel != "default.jpg")
            {
                string yol = System.Web.Hosting.HostingEnvironment.MapPath("~/Dosyalar/" + urun.urunGorsel);
                if (File.Exists(yol))
                {
                    File.Delete(yol);
                }
            }
            string data = model.fotoData;
            string base64 = data.Substring(data.IndexOf(',') + 1);
            base64 = base64.Trim('\0');
            byte[] imgbyte = Convert.FromBase64String(base64);
            string dosyaAdi = urun.urunId + model.fotoUzanti.Replace("image/",".");

            using (var ms = new MemoryStream(imgbyte, 0, imgbyte.Length))
            {
                Image img = Image.FromStream(ms, true);
                img.Save(System.Web.Hosting.HostingEnvironment.MapPath("~/Dosyalar/" + dosyaAdi));
            }
            urun.urunGorsel = dosyaAdi;
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Fotoğraf güncellendi.";
            return sonuc;
        }

        #endregion


        #region Kayit
        [HttpGet]
        [Route("api/urunkategoriliste/{urunId}")]
        public List<KayitModel> UrunKategoriListesi(int urunId)
        {
            List<KayitModel> liste = db.Kayit.Where(s => s.kayitUrunId == urunId).Select(x => new KayitModel()
            {
                kayitId = x.kayitId,
                kayitUrunId = x.kayitUrunId,
                kayitKatId = x.kayitKatId
            }).ToList();

            foreach (var kayit in liste)
            {
                kayit.urunBilgi = UrunById(kayit.kayitUrunId);
                kayit.kategoriBilgi = KategoriById(kayit.kayitKatId);
            }
            return liste;
        }
        [HttpGet]
        [Route("api/kategoriurunliste/{dersId}")]
        public List<KayitModel> KategoriUrunListesi(int kategoriId)
        {
            List<KayitModel> liste = db.Kayit.Where(s => s.kayitKatId == kategoriId).Select(x => new KayitModel()
            {
                kayitId = x.kayitId,
                kayitUrunId = x.kayitUrunId,
                kayitKatId = x.kayitKatId
            }).ToList();

            foreach (var kayit in liste)
            {
                kayit.urunBilgi = UrunById(kayit.kayitUrunId);
                kayit.kategoriBilgi = KategoriById(kayit.kayitKatId);
            }
            return liste;
        }
        [HttpPost]
        [Route("api/kayitekle")]
        public SonucModel KayitEkle(KayitModel model)
        {
            if (db.Kayit.Count(s => s.kayitKatId == model.kayitKatId && s.kayitUrunId == model.kayitUrunId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Ürün bu kategoriye kayıtlıdır.";
                return sonuc;
            }

            Kayit yeni = new Kayit();
            yeni.kayitId = model.kayitId;
            yeni.kayitUrunId = model.kayitUrunId;
            yeni.kayitKatId = model.kayitKatId;
            db.Kayit.Add(yeni);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Ürün kategoriye eklendi.";
            return sonuc;
        }
        [HttpDelete]
        [Route("api/kayitsil")]
        public SonucModel KayitSil(int kayitId)
        {
            Kayit kayit = db.Kayit.Where(s => s.kayitId == kayitId).SingleOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt bulunamadı.";
                return sonuc;
            }
            db.Kayit.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kayıt silindi";
            return sonuc;
        }
        #endregion

        #region Müşteri

        [HttpGet]
        [Route("api/musteriliste")]
        public List<MusteriModel> MusteriListe()
        {
            List<MusteriModel> liste = db.Musteri.Select(x => new MusteriModel()
            {
                musteriId = x.musteriId,
                musteriAd = x.musteriAd,
                musteriSoyad = x.musteriSoyad,
                musteriTel = x.musteriTel,
                musteriMail = x.musteriMail,
                musteriSifre = x.musteriSifre
            }).ToList();
            return liste;
        }


        [HttpGet]
        [Route("api/musteribyid/{musteriId}")]
        public MusteriModel MusteriById(int musteriId)
        {
            MusteriModel kayit = db.Musteri.Where(s => s.musteriId == musteriId).Select(x => new MusteriModel()
            {
                musteriId = x.musteriId,
                musteriAd = x.musteriAd,
                musteriSoyad = x.musteriSoyad,
                musteriTel = x.musteriTel,
                musteriMail = x.musteriMail,
                musteriSifre = x.musteriSifre
            }).FirstOrDefault();
            return kayit;
        }

        [HttpPost]
        [Route("api/musteriekle")]

        public SonucModel MusteriEkle(MusteriModel model)
        {
            if (db.Musteri.Count(s => s.musteriMail == model.musteriMail) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Müşteri bilgileri sistemde mevcuttur";
                return sonuc;
            }
            Musteri yeni = new Musteri();
            yeni.musteriAd = model.musteriAd;
            yeni.musteriSoyad = model.musteriSoyad;
            yeni.musteriTel = model.musteriTel;
            yeni.musteriMail = model.musteriMail;
            yeni.musteriSifre = model.musteriSifre;
            db.Musteri.Add(yeni);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Müşteri bilgileri sisteme eklenmiştir.";
            return sonuc;
        }

        [HttpPut]
        [Route("api/musteriduzenle")]
        public SonucModel MusteriDuzenle(MusteriModel model)
        {
            Musteri kayit = db.Musteri.Where(s => s.musteriId == model.musteriId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girdiğiniz bilgilere ait müşteri bulunamadı.";
                return sonuc;
            }
            kayit.musteriAd = model.musteriAd;
            kayit.musteriSoyad = model.musteriSoyad;
            kayit.musteriTel = model.musteriTel;
            kayit.musteriMail = model.musteriMail;
            kayit.musteriSifre = model.musteriSifre;
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Müşteri bilgileri başarılı bir şekilde güncellenmiştir.";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/musterisil/{musteriId}")]
        public SonucModel MusteriSil(int musteriId)
        {
            Musteri kayit = db.Musteri.Where(s => s.musteriId == musteriId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Müşteri bulunamadı.";
                return sonuc;
            }
            db.Musteri.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Müşteri bilgileri sistemden silinmiştir.";
            return sonuc;
        }
        #endregion

        #region Sepet

        [HttpGet]
        [Route("api/sepetliste")]
        public List<SepetModel> SepetListe()
        {
            List<SepetModel> liste = db.Sepet.Select(x => new SepetModel()
            {
                sepetId = x.sepetId,
                sepetSiparisId = x.sepetSiparisId,
                sepetUrunId = x.sepetUrunId,
                urunAdet = x.urunAdet,
                toplamFiyat = x.toplamFiyat
            }).ToList();
            return liste;
        }

        [HttpGet]
        [Route("api/sepetbyid/{sepetId}")]
        public SepetModel SepetById(int sepetId)
        {
            SepetModel kayit = db.Sepet.Where(s => s.sepetId == sepetId).Select(x => new SepetModel()
            {
                sepetId = x.sepetId,
                sepetSiparisId = x.sepetSiparisId,
                sepetUrunId = x.sepetUrunId,
                urunAdet = x.urunAdet,
                toplamFiyat = x.toplamFiyat
            }).FirstOrDefault();
            return kayit;
        }

        [HttpPost]
        [Route("api/sepetekle")]

        public SonucModel SepetEkle(SepetModel model)
        {
            if (db.Sepet.Count(s => s.sepetId == model.sepetId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu sepet içeriği doludur.";
                return sonuc;
            }
            Sepet yeni = new Sepet();
            yeni.sepetId = model.sepetId;
            yeni.sepetSiparisId = model.sepetSiparisId;
            yeni.sepetUrunId = model.sepetUrunId;
            yeni.urunAdet = model.urunAdet;
            yeni.toplamFiyat = model.toplamFiyat;
            db.Sepet.Add(yeni);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Sepet başarı ile oluşturulmuştur.";
            return sonuc;
        }

        [HttpPut]
        [Route("api/sepetduzenle")]
        public SonucModel SepetDuzenle(SepetModel model)
        {
            Sepet kayit = db.Sepet.Where(s => s.sepetId == model.sepetId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girdiğiniz bilgilere ait sepet bilgisi bulunamadı.";
                return sonuc;
            }
            kayit.sepetId = model.sepetId;
            kayit.sepetSiparisId = model.sepetSiparisId;
            kayit.sepetUrunId = model.sepetUrunId;
            kayit.urunAdet = model.urunAdet;
            kayit.toplamFiyat = model.toplamFiyat;
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Sepet bilgileri başarılı bir şekilde güncellenmiştir.";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/sepetsil/{sepetId}")]
        public SonucModel SepetSil(int sepetId)
        {
            Sepet kayit = db.Sepet.Where(s => s.sepetId == sepetId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Sepet bilgisi bulunamadı.";
                return sonuc;
            }
            db.Sepet.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Sepet kaydı sistemden silinmiştir.";
            return sonuc;
        }

        #endregion

        #region Sipariş

        [HttpGet]
        [Route("api/siparisliste")]
        public List<SiparisModel> SiparisListe()
        {
            List<SiparisModel> liste = db.Siparis.Select(x => new SiparisModel()
            {
                siparisId = x.siparisId,
                genelToplam = x.genelToplam,
                siparisTarih = x.siparisTarih,
                siparisAdres = x.siparisAdres,
                siparisIl = x.siparisIl,
                siparisIlce = x.siparisIlce,
                siparisMusteriId = x.siparisMusteriId
            }).ToList();
            return liste;
        }

        [HttpGet]
        [Route("api/siparisbyid/{siparisId}")]
        public SiparisModel SiparsiById(int siparsiId)
        {
            SiparisModel kayit = db.Siparis.Where(s => s.siparisId == siparsiId).Select(x => new SiparisModel()
            {
                siparisId = x.siparisId,
                genelToplam = x.genelToplam,
                siparisTarih = x.siparisTarih,
                siparisAdres = x.siparisAdres,
                siparisIl = x.siparisIl,
                siparisIlce = x.siparisIlce,
                siparisMusteriId = x.siparisMusteriId
            }).FirstOrDefault();
            return kayit;
        }

        [HttpPost]
        [Route("api/siparisekle")]
        public SonucModel SiparisEkle(SiparisModel model)
        {
            if (db.Siparis.Count(s => s.siparisId == model.siparisId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Sipariş bilgileri zaten mevcut.";
                return sonuc;
            }
            Siparis yeni = new Siparis();
            yeni.siparisId = model.siparisId;
            yeni.genelToplam = model.genelToplam;
            yeni.siparisTarih = model.siparisTarih;
            yeni.siparisAdres = model.siparisAdres;
            yeni.siparisIl = model.siparisIl;
            yeni.siparisIlce = model.siparisIlce;
            yeni.siparisMusteriId = model.siparisMusteriId;
            db.Siparis.Add(yeni);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Sipariş başarı ile oluşturulmuştur.";
            return sonuc;
        }

        [HttpPut]
        [Route("api/siparsiduzenle")]
        public SonucModel SiparisDuzenle(SiparisModel model)
        {
            Siparis kayit = db.Siparis.Where(s => s.siparisId == model.siparisId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girdiğiniz bilgilere ait sipariş bilgisi bulunamadı.";
                return sonuc;
            }
            kayit.siparisId = model.siparisId;
            kayit.genelToplam = model.genelToplam;
            kayit.siparisTarih = model.siparisTarih;
            kayit.siparisAdres = model.siparisAdres;
            kayit.siparisIl = model.siparisIl;
            kayit.siparisIlce = model.siparisIlce;
            kayit.siparisMusteriId = model.siparisMusteriId;
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Sipariş bilgileri başarılı bir şekilde güncellenmiştir.";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/siparissil/{siparisId}")]
        public SonucModel SiparisSil(int siparisId)
        {
            Siparis kayit = db.Siparis.Where(s => s.siparisId == siparisId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girdiğiniz bilgilere ait sipariş bilgisi bulunamadı.";
                return sonuc;
            }
            db.Siparis.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Sipariş bilgileri başarı ile sistemden silinmiştir.";
            return sonuc;
        }

        #endregion
        #region Yönetici

        [HttpGet]
        [Route("api/yoneticiliste")]
        public List<YoneticiModel> YoneticiListe()
        {
            List<YoneticiModel> liste = db.Yonetici.Select(x => new YoneticiModel()
            {
                yoneticiId = x.yoneticiId,
                yoneticiAd = x.yoneticiAd,
                yoneticiSifre = x.yoneticiSifre
            }).ToList();
            return liste;
        }

        [HttpGet]
        [Route("api/yoneticibyid/{yoneticiId}")]
        public YoneticiModel YoneticiById(int yoneticiId)
        {
            YoneticiModel kayit = db.Yonetici.Where(s => s.yoneticiId == yoneticiId).Select(x => new YoneticiModel()
            {
                yoneticiId = x.yoneticiId,
                yoneticiAd = x.yoneticiAd,
                yoneticiSifre = x.yoneticiSifre
            }).FirstOrDefault();
            return kayit;
        }


        [HttpPost]
        [Route("api/yoneticiekle")]
        public SonucModel YoneticiEkle(YoneticiModel model)
        {
            if (db.Yonetici.Count(s => s.yoneticiAd == model.yoneticiAd) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Yönetici sisteme kayıtlıdır.";
                return sonuc;
            }
            Yonetici yeni = new Yonetici();
            yeni.yoneticiAd = model.yoneticiAd;
            yeni.yoneticiSifre = model.yoneticiSifre;

            db.Yonetici.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Yönetici sisteme eklenmiştir.";
            return sonuc;
        }

        [HttpPut]
        [Route("api/yoneticiduzenle")]

        public SonucModel YoneticiDuzenle(YoneticiModel model)
        {
            Yonetici kayit = db.Yonetici.Where(s => s.yoneticiId == model.yoneticiId).FirstOrDefault(); if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Yönetici bilgileri bulunamadı. Lütfen geçerli bir yönetici seçiniz.";
                return sonuc;
            }
            kayit.yoneticiAd = model.yoneticiAd;
            kayit.yoneticiSifre = model.yoneticiSifre;
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Yönetici bilgileri başarılı bir şekilde düzenlenmiştir.";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/yoneticisil/{yoneticiId}")]
        public SonucModel YoneticiSil(int yoneticiId)
        {
            Yonetici kayit = db.Yonetici.Where(s => s.yoneticiId == yoneticiId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girdiğiniz bilgilere ait yönetici bilgisi bulunamadı.";
                return sonuc;
            }
            db.Yonetici.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Yönetici bilgileri başarı ile sistemden silinmiştir.";
            return sonuc;
        }

        #endregion


    }



}
