using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BilgeKafe.Data;
using BilgeKafe.UI.Properties;
using Newtonsoft.Json;

namespace BilgeKafe.UI
{
    public partial class AnaForm : Form
    {
        KafeVeri db = new KafeVeri();
        private readonly BindingList<GecmisSiparislerForm> gecmisSiparislerForm;
        public AnaForm()
        {
            VerileriOku();
            InitializeComponent();
            MasalariOlustu();
        }

        private void VerileriOku()
        {
            try
            {
                string json = File.ReadAllText("veri.json");
                db = JsonConvert.DeserializeObject<KafeVeri>(json);
            }
            catch (Exception)
            {        
            }
        }

        private void MasalariOlustu()
        {
            #region Imaj Listesinin Olusturulması
            ImageList imageList = new ImageList();
            imageList.Images.Add("bos", Resources.masabos);
            imageList.Images.Add("dolu", Resources.masadolu);
            imageList.ImageSize = new Size(105, 105);
            lvwMasalar.LargeImageList = imageList;
            #endregion

            for (int i = 1; i <= db.MasaAdet; i++)
            {
                ListViewItem lvi = new ListViewItem($"Masa{i}");
                lvi.Tag = i;
                lvi.ImageKey = db.AktifSiparisler.Any(x=> x.MasaNo == i ) ? "dolu":"bos";
                lvwMasalar.Items.Add(lvi);
            }
        }

        private void lvwMasalar_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem lvi = lvwMasalar.SelectedItems[0];
            lvi.ImageKey = "dolu";
            int masaNo = (int)lvi.Tag;

            //Tıklanan masaya ait varsa siparişi bul
            Siparis siparis = db.AktifSiparisler.FirstOrDefault(x => x.MasaNo == masaNo);

            //Eger siparis o masaya ait oluşturulmamışsa
            if (siparis == null)
            {
                siparis = new Siparis() { MasaNo = masaNo };
                db.AktifSiparisler.Add(siparis);
            }


            SiparisForm frmSiparis = new SiparisForm(db, siparis);
            frmSiparis.ShowDialog();

            if (siparis.SiparisDurum != SiparisDurum.Aktif)
            {
                lvi.ImageKey = "bos";
            }


        }

        private void tsmiGecmisSiparisler_Click_1(object sender, EventArgs e)
        {
            new GecmisSiparislerForm(db).ShowDialog();
        }

        private void tsmiUrunler_Click(object sender, EventArgs e)
        {
            new UrunlerForm(db).ShowDialog();
        }

        private void AnaForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string json = JsonConvert.SerializeObject(db);
            File.WriteAllText("veri.json", json);
        }
    }
}
