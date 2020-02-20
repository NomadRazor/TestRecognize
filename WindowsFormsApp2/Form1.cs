using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;

namespace WindowsFormsApp2
{
  

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();

            switch (result)
            {
                case DialogResult.Yes:
                case DialogResult.OK:
                    pictureBox1.Image = Bitmap.FromFile(openFileDialog1.FileName);
                    OutputLog($"File {openFileDialog1.FileName} loaded");
                    break;
                case DialogResult.Cancel:
                case DialogResult.Abort:
                    OutputLog("File not selected");
                    break;
            }

        }

        private void OutputLog(string text)
        {
            console.AppendText($"[{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}]: {text} \r\n");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap bpm = pictureBox2.Image as Bitmap;

            for(int i = 0; i < bpm.Width; i++)
            {
                for (int j = 0; j < bpm.Height; j++)
                {
                    Color pixel = bpm.GetPixel(i, j);
                    int lum = (int)(pixel.R * 0.29) + (int)(pixel.G * 0.72) + (int)(pixel.B * 0.07);
                    if (lum > 160)
                    {
                        bpm.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                    } else
                    {
                        bpm.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                    }
                    
                    pictureBox2.Image = bpm;
                    Application.DoEvents();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = Bitmap.FromFile(openFileDialog1.FileName);
            OutputLog($"File2 {openFileDialog1.FileName} loaded");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            TesseractEngine eng = new TesseractEngine("./tessdata", "rus", EngineMode.Default);
            Page pg = eng.Process(pictureBox1.Image as Bitmap, PageSegMode.Auto);
            string data = pg.GetText();
            OutputLog($"Recognized first -> {data}");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            TesseractEngine eng = new TesseractEngine("./tessdata", "rus", EngineMode.Default);
            Page pg = eng.Process(pictureBox2.Image as Bitmap, PageSegMode.Auto);
            string data = pg.GetText();
            OutputLog($"Recognized second -> {data}");
        }
    }
}
