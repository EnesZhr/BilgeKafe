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
            InitializeComponent();
            MasalariOlustu();
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
                lvi.ImageKey = db.Siparisler.Any(x=> x.MasaNo == i && x.SiparisDurum== SiparisDurum.Aktif) ? "dolu":"bos";
                lvwMasalar.Items.Add(lvi);
            }
        }

        private void lvwMasalar_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem lvi = lvwMasalar.SelectedItems[0];
            lvi.ImageKey = "dolu";
            int masaNo = (int)lvi.Tag;

            //Tıklanan masaya ait varsa siparişi bul
            Siparis siparis = db.Siparisler.FirstOrDefault(x => x.MasaNo == masaNo && x.SiparisDurum == SiparisDurum.Aktif);

            //Eger siparis o masaya ait oluşturulmamışsa
            if (siparis == null)
            {
                siparis = new Siparis() { MasaNo = masaNo };
                db.Siparisler.Add(siparis);
            }


            SiparisForm frmSiparis = new SiparisForm(db, siparis);
            frmSiparis.MasaTasindi += FrmSiparis_MasaTasindi;
            frmSiparis.ShowDialog();

            if (siparis.SiparisDurum != SiparisDurum.Aktif)
            {
                lvi.ImageKey = "bos";
            }


        }

        private void FrmSiparis_MasaTasindi(object sender, MasaTasindiEventArgs e)
        {
            foreach (ListViewItem lvi in lvwMasalar.Items)
            {
                if ((int)lvi.Tag == e.EskiMasaNo)
                    lvi.ImageKey = "bos";
                if ((int)lvi.Tag == e.YeniMasaNo)
                    lvi.ImageKey = "dolu";
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

    }
}
