using BilgeKafe.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BilgeKafe.UI
{
    public partial class SiparisForm : Form
    {
        private readonly KafeVeri db;
        private readonly Siparis siparis;
        private readonly BindingList<SiparisDetay> blSiparisDetaylar;
        public event EventHandler<MasaTasindiEventArgs> MasaTasindi;


        public SiparisForm(KafeVeri db, Siparis siparis)
        {
            this.db = db;
            this.siparis = siparis;
            blSiparisDetaylar = new BindingList<SiparisDetay>(siparis.SiparisDetaylar.ToList());
            blSiparisDetaylar.ListChanged += BlSiparisDetaylar_ListChanged;
            InitializeComponent();
            dgvSiparişDetaylari.AutoGenerateColumns = false; // otomatik sütun oluşturmasını kapattık
            UrunleriListele();
            MasaNoyuGuncelle();
            dgvSiparişDetaylari.DataSource = blSiparisDetaylar;
            MasaNolariListele();
            blSiparisDetaylar.ResetBindings();
        }

        private void MasaNolariListele()
        {
            cboMasaNo.DataSource = Enumerable.Range(1, 20).Where(x => !db.Siparisler.Any(i => i.MasaNo ==x && i.SiparisDurum == SiparisDurum.Aktif)).ToList();

            //cboMasaNo.Items.Clear();
            //for (int i = 0; i < db.MasaAdet; i++)
            //{
            //    if (!db.AktifSiparisler.Any(x => x.MasaNo == i))
            //    {
            //        cboMasaNo.Items.Add(i);
            //    }
            //}


        }

        //Binding list üzerinde değişiklik yapıldığında tetiklenen evente bağlı
        private void BlSiparisDetaylar_ListChanged(object sender, ListChangedEventArgs e)
        {   
            OdemeTutariniGuncelle();
        }

        private void OdemeTutariniGuncelle()
        {
            lblOdemeTutari.Text = siparis.ToplamTutarTL;
        }

        private void UrunleriListele()
        {
            cboUrun.DataSource = db.Urunler.ToList();
        }

        private void MasaNoyuGuncelle()
        {
            Text = $"Masa {siparis.MasaNo} (Açılış Zamanı: {siparis.AcilisZamani})";
            lblMasaNo.Text = $"{siparis.MasaNo:00}";
        }

        private void btnDetayEkle_Click(object sender, EventArgs e)
        {
            Urun urun = (Urun)cboUrun.SelectedItem;
            int adet = (int)nudAdet.Value;

            if (urun == null)
            {
                MessageBox.Show("Önce bir ürün seçmelisiniz.");
                return;
            }

            SiparisDetay sd = new SiparisDetay()
            {
                UrunAd = urun.UrunAd,
                BirimFiyat = urun.BirimFiyat,
                Adet = adet,
                Urun = urun
            };
            siparis.SiparisDetaylar.Add(sd);
            db.SaveChanges();
            blSiparisDetaylar.Add(sd);
            

        }

        private void btnAnasayfa_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOdemeAl_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show($"{siparis.ToplamTutarTL} tutarı tahsil edildiyse sipariş kapatılacaktır.Onaylıyor musunuz?",
                "Ödeme onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            if (dr == DialogResult.Yes)
            {
                SiparisiKapat(SiparisDurum.Odendi);
            }
        }

        private void btnSiparisIptal_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show($"Sipariş iptal edilecek? Onaylıyor musunuz?",
                "İptal onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

            if (dr == DialogResult.Yes)
            {
                SiparisiKapat(SiparisDurum.Iptal);
            }
        }

        private void SiparisiKapat(SiparisDurum durum)
        {
            siparis.OdenenTutar = durum == SiparisDurum.Odendi ? siparis.ToplamTutar() : 0;
            siparis.SiparisDurum = durum;
            siparis.KapanisZamani = DateTime.Now;
            db.SaveChanges();
            Close();
        }

        private void btnMasaTasi_Click(object sender, EventArgs e)
        {
            int yeniMasaNo =(int)cboMasaNo.SelectedItem;
            int eskiMasaNo = siparis.MasaNo;
            siparis.MasaNo = yeniMasaNo;
            db.SaveChanges();
            MasaNoyuGuncelle();
            MasaNolariListele();

            MasaTasindiEventArgs args = new MasaTasindiEventArgs()
            {
                EskiMasaNo = eskiMasaNo,
                YeniMasaNo = yeniMasaNo
            };

            if (MasaTasindi != null)
                MasaTasindi(this, args);
        }

        private void dgvSiparişDetaylari_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            SiparisDetay sd = (SiparisDetay)e.Row.DataBoundItem;
            db.SiparisDetaylar.Remove(sd);
            db.SaveChanges();
        }
    }
}
