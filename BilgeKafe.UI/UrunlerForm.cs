﻿using BilgeKafe.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BilgeKafe.UI
{
    public partial class UrunlerForm : Form
    {
        private readonly KafeVeri db;
        private readonly BindingList<Urun> blUrunler;

        public UrunlerForm(KafeVeri db)
        {
            this.db = db;
            blUrunler = new BindingList<Urun>(db.Urunler.ToList());
            InitializeComponent();
            dgvUrunler.AutoGenerateColumns = false;
            dgvUrunler.DataSource = blUrunler;
            
        }

        private void btnUrunEkle_Click(object sender, EventArgs e)
        {
            string ad = txtUrunAd.Text.Trim();
            decimal birimFİyat = nudBirimFiyat.Value;

            if (ad == "")
            {
                MessageBox.Show("Önce ürün adı belirlemelisiniz.");
                return;
            }

            if (btnUrunEkle.Text =="EKLE")
            {
                Urun urun= new Urun { BirimFiyat = birimFİyat, UrunAd = ad };
                blUrunler.Add(urun);
                db.Urunler.Add(urun);
                
            }
            else
            {
                DataGridViewRow satir = dgvUrunler.SelectedRows[0];
                Urun urun = (Urun)satir.DataBoundItem;
                urun.UrunAd = ad;
                urun.BirimFiyat = birimFİyat;
                blUrunler.ResetBindings();
            }
            db.SaveChanges();
            FormuResetle();
           
        }

        private void dgvUrunler_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Seçili ürün silinecektir. Onaylıyor musunuz?" ,"Silme Onayı",MessageBoxButtons.YesNo,MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

            if (dr == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }

            Urun urun = (Urun)e.Row.DataBoundItem;
            db.Urunler.Remove(urun);
            db.SaveChanges();
        }

        private void btnDüzenle_Click(object sender, EventArgs e)
        {
            if (dgvUrunler.SelectedRows.Count == 0)
                return;

            DataGridViewRow satir = dgvUrunler.SelectedRows[0];
            Urun urun = (Urun)satir.DataBoundItem;
            txtUrunAd.Text = urun.UrunAd;
            nudBirimFiyat.Value = urun.BirimFiyat;
            btnUrunEkle.Text = "Kaydet";
            btnIptal.Show();
            dgvUrunler.Enabled = false;
            btnDüzenle.Enabled = false;
            txtUrunAd.Focus();

        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            FormuResetle();
        }

        private void FormuResetle()
        {
            txtUrunAd.Clear();
            nudBirimFiyat.Value = 0;
            btnUrunEkle.Text = "EKLE";
            btnIptal.Hide();
            dgvUrunler.Enabled = true;
            btnDüzenle.Enabled = true;
            txtUrunAd.Focus();

        }
    }
}
