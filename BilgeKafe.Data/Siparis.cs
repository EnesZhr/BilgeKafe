using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeKafe.Data
{
    public class Siparis
    {
        public int MasaNo { get; set; }

        public SiparisDurum SiparisDurum { get; set; } = SiparisDurum.Aktif;

        public decimal OdenenTutar { get; set; }

        public DateTime? AcilisZamani { get; set; } = DateTime.Now;

        public DateTime? KapanisZamani { get; set; }

        public List<SiparisDetay> SiparisDetaylar { get; set; } = new List<SiparisDetay>();

        public string ToplamTutarTL => $"{ToplamTutar():n2}₺";


        public decimal ToplamTutar() => SiparisDetaylar.Sum(sd => sd.Tutar());

        //public decimal ToplamTutar()
        //{
        //    decimal toplam = 0;
        //    foreach (SiparisDetay detay in SiparisDetaylar)
        //    {
        //        toplam += detay.Tutar();
        //    }
        //    return toplam;
        //}



        //* MasaNo: int
        //* Durum: SiparisDurum
        //* OdenenTutar: decimal
        //* AcilisZamani: DateTime?
        //* KapanisZamani: DateTime?
        //* SiparisDetaylar: List<SiparisDetay>
        //* ToplamTutarTL: string-readonly
        //- ToplamTutar() : decimal

    }
}
