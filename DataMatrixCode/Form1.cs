using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataMatrixCode
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer t;
        public int MSize = 32;
        public List<string> lista = new List<string>();
        public int generated = 0, duplicated = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataMatrix.net.DmtxImageEncoder enc = new DataMatrix.net.DmtxImageEncoder();
            DataMatrix.net.DmtxImageEncoderOptions opt = new DataMatrix.net.DmtxImageEncoderOptions();
            opt.ModuleSize = MSize;
            Bitmap bmp = enc.EncodeImage(textBox1.Text, opt);
            pictureBox1.Image = bmp;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataMatrix.net.DmtxImageDecoder dec = new DataMatrix.net.DmtxImageDecoder();
            Bitmap img = new Bitmap(pictureBox1.Image);
            textBox1.Text = string.Join("",dec.DecodeImage(img));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog opn = new OpenFileDialog();
            opn.Filter = "Bitmap|*.bmp";
            opn.Title = "Open bitmap";
            if(opn.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(opn.OpenFile());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(pictureBox1.Image != null)
            {
                SaveFileDialog svn = new SaveFileDialog();
                svn.Filter = "Bitmapa|*.bmp";
                svn.Title = "Save bitmap...";
                svn.ShowDialog();
                if(svn.FileName != "")
                {
                    FileStream fs = (FileStream)svn.OpenFile();
                    Bitmap ts = new Bitmap(pictureBox1.Image);
                    ts.Save(fs, System.Drawing.Imaging.ImageFormat.Bmp);
                    fs.Close();
                }
                
            }
        }
        public void InitTimer()
        {
            t = new System.Windows.Forms.Timer();
            t.Stop();
            t.Tick += new EventHandler(t_Tick);
            t.Interval = Int32.Parse(textBox2.Text);
            t.Start();
        }

        private void T_Tick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            InitTimer();
        }
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private void t_Tick(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(DateTime.Now.Year);
            sb.Append(DateTime.Now.Month);
            sb.Append(DateTime.Now.Day);
            sb.Append(DateTime.Now.Hour);
            sb.Append(RandomString(20));
            sb.Append("AutoJet");
            string tenc = sb.ToString();
            if (lista.Find(x => x == tenc) == null)
            {
                generated++;
                lista.Add(tenc);
                textBox1.Text = tenc;
                DataMatrix.net.DmtxImageEncoder enc = new DataMatrix.net.DmtxImageEncoder();
                DataMatrix.net.DmtxImageEncoderOptions opt = new DataMatrix.net.DmtxImageEncoderOptions();
                opt.ModuleSize = MSize;
                Bitmap bmp = enc.EncodeImage(tenc, opt);
                pictureBox1.Image = bmp;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                duplicated++;
            }
            label3.Text = generated.ToString();
            label5.Text = duplicated.ToString();
        }
    }
}
