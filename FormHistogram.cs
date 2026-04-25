using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;         
using OpenCvSharp.Extensions;
using Microsoft.Win32;

namespace DIP
{
    public partial class FormHistogram : Form
    {
        Mat image1;
        Mat img;

        public FormHistogram()
        {
            InitializeComponent();
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


        private void button1_Click(object sender, EventArgs e)
        {
            if (image1 == null) return;

            // 1. Initialize arrays to store counts for values 0 to 255
            int[] redHisto = new int[256];
            int[] greenHisto = new int[256];
            int[] blueHisto = new int[256];

            // 2. Manual Pointer Math to count pixel values
            unsafe
            {
                byte* p = (byte*)image1.Data;
                int width = image1.Width;
                int height = image1.Height;
                int channels = image1.Channels();

                for (int r = 0; r < height; r++)
                {
                    for (int c = 0; c < width; c++)
                    {
                        int index = (r * width * channels) + (c * channels);

                        // Increment the count for each intensity value found
                        blueHisto[p[index + 0]]++;
                        greenHisto[p[index + 1]]++;
                        redHisto[p[index + 2]]++;
                    }
                }
            }

            // 3. Represent result in the Chart
            chart1.Series["Red"].Points.Clear();
            chart1.Series["Green"].Points.Clear();
            chart1.Series["Blue"].Points.Clear();

            for (int i = 0; i < 256; i++)
            {
                chart1.Series["Red"].Points.AddXY(i, redHisto[i]);
                chart1.Series["Green"].Points.AddXY(i, greenHisto[i]);
                chart1.Series["Blue"].Points.AddXY(i, blueHisto[i]);
            }
        }
        

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (image1 == null) return;
            img = new Mat(image1.Size(), image1.Type());

            int totalPixels = image1.Width * image1.Height;

            // Step 1: Initialize frequency arrays (ni)
            int[] ni_B = new int[256];
            int[] ni_G = new int[256];
            int[] ni_R = new int[256];

            unsafe
            {
                byte* s = (byte*)image1.Data;
                byte* d = (byte*)img.Data;
                int channels = image1.Channels();

                // 1. Calculate ni (Frequencies)
                for (int i = 0; i < totalPixels * channels; i += 3)
                {
                    ni_B[s[i]]++;
                    ni_G[s[i + 1]]++;
                    ni_R[s[i + 2]]++;
                }

                // 2 & 3. Calculate Probability and CDF
                double[] cdf_B = new double[256];
                double[] cdf_G = new double[256];
                double[] cdf_R = new double[256];

                double sumB = 0, sumG = 0, sumR = 0;

                for (int i = 0; i < 256; i++)
                {
                    sumB += (double)ni_B[i] / totalPixels;
                    sumG += (double)ni_G[i] / totalPixels;
                    sumR += (double)ni_R[i] / totalPixels;

                    cdf_B[i] = sumB;
                    cdf_G[i] = sumG;
                    cdf_R[i] = sumR;
                }

                // 4 & 5. Create Lookup Table (Scaling and Ceiling/Rounding)
                byte[] lutB = new byte[256];
                byte[] lutG = new byte[256];
                byte[] lutR = new byte[256];

                for (int i = 0; i < 256; i++)
                {
                    lutB[i] = (byte)Math.Round(cdf_B[i] * 255);
                    lutG[i] = (byte)Math.Round(cdf_G[i] * 255);
                    lutR[i] = (byte)Math.Round(cdf_R[i] * 255);
                }

                // Apply the new values to the destination image
                for (int i = 0; i < totalPixels * channels; i += 3)
                {
                    d[i] = lutB[s[i]];     // Blue
                    d[i + 1] = lutG[s[i + 1]]; // Green
                    d[i + 2] = lutR[s[i + 2]]; // Red
                }
            }
            pictureBox2.Image = img.ToBitmap();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (img == null) return; // 'img' is our result from the equalize button

            int[] rH = new int[256];
            int[] gH = new int[256];
            int[] bH = new int[256];

            unsafe
            {
                byte* p = (byte*)img.Data;
                int total = img.Width * img.Height * img.Channels();

                for (int i = 0; i < total; i += 3)
                {
                    bH[p[i]]++;
                    gH[p[i + 1]]++;
                    rH[p[i + 2]]++;
                }
            }

            
            for (int i = 0; i < 256; i++)
            {
                chart2.Series["Red"].Points.AddXY(i, rH[i]);
                chart2.Series["Green"].Points.AddXY(i, gH[i]);
                chart2.Series["Blue"].Points.AddXY(i, bH[i]);
            }
        }

        private void FormHistogram_Load(object sender, EventArgs e)
        {

        }
    }
}

