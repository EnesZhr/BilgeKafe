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
        public int SiparisDurum { get; set; }
        public decimal OdenecekTutar { get; set; }
        public DateTime? AcilisZamani { get; set; } = DateTime.Now;
        public DateTime? KapanisZamani { get; set; }
        public List<SiparisDetay> SiparisDetaylar { get; set; }
        public string ToplamTutarTL { get; }

        public decimal ToplamTutar()
        {
            decimal toplam = 0;
            foreach (SiparisDetay detay in SiparisDetaylar)
            {
                toplam += detay.Tutar();
            }
            return toplam;
        }



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
