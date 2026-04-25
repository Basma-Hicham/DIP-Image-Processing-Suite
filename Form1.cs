using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;         
using OpenCvSharp.Extensions; 

namespace DIP
{
    public partial class Form1 : Form
    {
        // Use Mat instead of IplImage - it handles its own memory better
        Mat image1;
        Mat img; 
        public Form1()
        {
            InitializeComponent();
        }

        private void convertToGrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image1 == null) return;

           
              Bitmap bmp = image1.ToBitmap();
              for (int y = 0; y < bmp.Height; y++) {
              for (int x =  0; x < bmp.Width; x++) {
              Color p = bmp.GetPixel(x, y);
              int avg = (p.R + p.G + p.B) / 3;
              bmp.SetPixel(x, y, Color.FromArgb(p.A, avg, avg, avg)); //alpha shfafiet elsora
              }
              }
              pictureBox2.Image = bmp;
             

        }

        private void redToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image1 == null) return;

            // Create destination Mat with same size and type as original
            img = new Mat(image1.Size(), image1.Type());

            // Get the raw memory pointers
            IntPtr srcAdd = image1.Data;
            IntPtr dstAdd = img.Data;

            unsafe
            {
                // Cast IntPtr to byte pointers for math operations
                byte* s = (byte*)srcAdd;
                byte* d = (byte*)dstAdd;

                int channels = image1.Channels();
                int width = image1.Width;
                int height = image1.Height;

                // Loop through pixels using the pointer math requested
                for (int r = 0; r < height; r++)
                {
                    for (int c = 0; c < width; c++)
                    {
                        // Calculate index: (Row * Width * Channels) + (Column * Channels)
                        int index = (r * width * channels) + (c * channels);

                        // OpenCV stores pixels in BGR order
                        d[index + 0] = 0;             // Set Blue to 0
                        d[index + 1] = 0;             // Set Green to 0
                        d[index + 2] = s[index + 2];   // Copy original Red
                    }
                }
            }

            // Display result
            pictureBox2.Image = img.ToBitmap();
        }

        private void greenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image1 == null) return;
            img = new Mat(image1.Size(), image1.Type());
            unsafe
            {
                byte* s = (byte*)image1.Data;
                byte* d = (byte*)img.Data;
                int stride = image1.Width * image1.Channels();

                for (int i = 0; i < image1.Height * stride; i += 3)
                {
                    d[i] = 0;         // Blue 0
                    d[i + 1] = s[i + 1]; // Keep Green
                    d[i + 2] = 0;     // Red 0
                }
            }
            pictureBox2.Image = img.ToBitmap();
        
        }

        private void blueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image1 == null) return;

            img = new Mat(image1.Size(), image1.Type());

            
            IntPtr srcAdd = image1.Data;
            IntPtr dstAdd = img.Data;

            unsafe
            {
                byte* s = (byte*)srcAdd;
                byte* d = (byte*)dstAdd;

                int channels = image1.Channels();
                int width = image1.Width;
                int height = image1.Height;

                for (int r = 0; r < height; r++)
                {
                    for (int c = 0; c < width; c++)
                    {
                       
                        int index = (r * width * channels) + (c * channels);


                        // Keep BLUE channel only
                        d[index + 0] = s[index + 0]; // Copy the original Blue value
                        d[index + 1] = 0;           // Set Green to 0
                        d[index + 2] = 0;           // Set Red to 0
                    }
                }
            }

            pictureBox2.Image = img.ToBitmap();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
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

                    pictureBox1.Image = resized.ToBitmap();

                    image1 = resized;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading image: " + ex.Message);
                }
            }
        }
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
