using MetroFramework;
using MetroFramework.Forms;
using Microsoft.Win32;
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

namespace OEMInformation
{
    public partial class Form1 : MetroForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InfoRead();

        }

        void InfoRead()
        {
            try
            {
                //RegistryKey mykey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\Microsoft\Windows\CurrentVersion\OEMInformation", true);

                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                using (var mykey = hklm.OpenSubKey(@"SOFTWARE\\Microsoft\Windows\CurrentVersion\OEMInformation"))
                {
                    txbManf.Text = mykey.GetValue("Manufacturer").ToString();
                    txbManf.Text = mykey.GetValue("Manufacturer").ToString();
                    txbModel.Text = mykey.GetValue("Model").ToString();
                    txbSupUrl.Text = mykey.GetValue("SupportURL").ToString();
                    txbSupHours.Text = mykey.GetValue("SupportHours").ToString();
                    txbSupPhone.Text = mykey.GetValue("SupportPhone").ToString();
                    if (mykey.GetValue("Logo").ToString() == "")
                    {

                    }
                    else
                    {
                        OEMLogo.Load(mykey.GetValue("Logo").ToString());
                    }
                }

            }
            catch
            {
                return;
            }
        }

        string imgsrc;
        OpenFileDialog fd = new OpenFileDialog();
        private void BtnLoad_Click(object sender, EventArgs e)
        {
            fd.Title = "Select the patch of image";
            fd.Filter = "Images Only|*.bmp*";
            if (fd.ShowDialog() == DialogResult.OK)
            {

                OEMLogo.Load(fd.FileName);
                imgsrc = fd.FileName.ToString();
                File.Copy(fd.FileName, @"C:\Windows\OEM\Logo.png",true);

            }
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {

            //RegistryKey oemKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\OEMInformation", true);
            using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            using (var oemKey = hklm.OpenSubKey(@"SOFTWARE\\Microsoft\Windows\CurrentVersion\OEMInformation",true))
            {
                oemKey.SetValue("Manufacturer", txbManf.Text);
                oemKey.SetValue("Model", txbModel.Text);
                oemKey.SetValue("SupportURL", txbSupUrl.Text);
                oemKey.SetValue("SupportHours", txbSupHours.Text);
                oemKey.SetValue("SupportPhone", txbSupPhone.Text);

                if (imgsrc == null)
                {
                    oemKey.SetValue("Logo", "");
                }
                else
                {
                    oemKey.SetValue("Logo", @"C:\Windows\OEM\Logo.png");
                }
            oemKey.Close();
            }
            MetroMessageBox.Show(this, string.Format("OEM Information has been changed\n\nManufacturer: {0}\nModel: {1}", txbManf.Text, txbModel.Text),"SUCCESS",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
        }

        private void OEMLogo_Click(object sender, EventArgs e)
        {

        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            txbManf.Text = " ";
            txbModel.Text = " ";
            txbSupHours.Text = " "; 
            txbSupPhone.Text = " ";
            txbSupUrl.Text = " ";
        }

        private void BtnLoadDefaults_Click(object sender, EventArgs e)
        {
            InfoRead();
        }
    }
}
