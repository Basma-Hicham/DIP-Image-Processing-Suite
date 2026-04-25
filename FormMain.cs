using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;        
using OpenCvSharp.Extensions;

namespace DIP
{
    public partial class FormMain : Form
    {

        Mat image1;
        Mat img; 
        public FormMain()
        {
            InitializeComponent();
        }

        private void picEditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 frmEditing = new Form1();

            frmEditing.Show();
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormHistogram frmHisto = new FormHistogram();

            frmHisto.Show();
        }

        private void filtersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Negative.Visible = true;
            Brightness.Visible = true;

            Contrast.Visible = true;


            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    image1 = Cv2.ImRead(openFileDialog1.FileName, ImreadModes.Color);

                    Mat resized = new Mat();
                    OpenCvSharp.Size size = new OpenCvSharp.Size(pictureBox1.Width, pictureBox1.Height);
                    Cv2.Resize(image1, resized, size);

                    // Display in PictureBox1 using the Extension method .ToBitmap()
                    pictureBox1.Image = resized.ToBitmap();

                    image1 = resized;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading image: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (image1 == null) return;
            img = new Mat(image1.Size(), image1.Type());

            unsafe
            {
                byte* s = (byte*)image1.Data;
                byte* d = (byte*)img.Data;
                int totalBytes = (int)(image1.Total() * image1.Channels());

                for (int i = 0; i < totalBytes; i++)
                {
                    // Math: Invert the color by subtracting from 255
                    d[i] = (byte)(255 - s[i]);
                }
            }
            pictureBox2.Image = img.ToBitmap();
        }

        private void Brightness_Click(object sender, EventArgs e)
        {
            if (image1 == null) return;
            img = new Mat(image1.Size(), image1.Type());
            int offset = 50; // Increase brightness by 50 units

            unsafe
            {
                byte* s = (byte*)image1.Data;
                byte* d = (byte*)img.Data;
                int totalBytes = (int)(image1.Total() * image1.Channels());

                for (int i = 0; i < totalBytes; i++)
                {
                    // Math: Current + Offset. 
                    // Math.Min/Max prevents "overflow" (values going above 255 or below 0)
                    int val = s[i] + offset;
                    d[i] = (byte)Math.Max(0, Math.Min(255, val));
                }
            }
            pictureBox2.Image = img.ToBitmap();
        }

        private void Contrast_Click(object sender, EventArgs e)
        {
            if (image1 == null) return;
            img = new Mat(image1.Size(), image1.Type());
            double threshold = 1.5; // Multiply by 1.5 to increase contrast

            unsafe
            {
                byte* s = (byte*)image1.Data;
                byte* d = (byte*)img.Data;
                int totalBytes = (int)(image1.Total() * image1.Channels());

                for (int i = 0; i < totalBytes; i++)
                {
                    // Math: PixelValue * Factor
                    // This pulls dark colors darker and light colors lighter
                    int val = (int)(s[i] * threshold);
                    d[i] = (byte)Math.Max(0, Math.Min(255, val));
                }
            }
            pictureBox2.Image = img.ToBitmap();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
        private void FormMain_Load(object sender, EventArgs e)
        {

        }
    }
}
