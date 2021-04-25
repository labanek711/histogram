using System;
using System.Drawing;
using System.Windows.Forms;

namespace lab6Histogram
{
    public partial class Form1 : Form
    {
        Image picture1;

        Bitmap bitmappicture1;
        private int picture1Width, picture1Height;

        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image) new Bitmap(imgToResize, size);
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            picture1 = pictureBox1.Image;


            picture1 = resizeImage(picture1, new Size(pictureBox1.Size.Width, pictureBox1.Size.Height));

            bitmappicture1 = new Bitmap(picture1);


            picture1Width = bitmappicture1.Width;
            picture1Height = bitmappicture1.Height;

        }


        public int rgbRange(int value)
        {
            if (value >= 255) return 254;
            if (value <= 0) return 1;
            else return value;
        }
        private int contrastReduction(int color, int c)
        {
            if (color < 127 + c) return (127 / (127 + c)) * color;
            else if (color > 127 - c) return ((127 * color) + (255 * c)) / (127 + c);
            else return 127;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Bitmap temp = new Bitmap(picture1);
            for (int y = 0; y < picture1Height; y++)
            {
                for (int x = 0; x < picture1Width; x++)
                {
                    Color picture1 = bitmappicture1.GetPixel(x, y);

                    int a = picture1.A;
                    int r = rgbRange((127 / rgbRange(127 - trackBar1.Value)) * (picture1.R - trackBar1.Value));
                    int g = rgbRange((127 / rgbRange(127 - trackBar1.Value)) * (picture1.G - trackBar1.Value));
                    int b = rgbRange((127 / rgbRange(127 - trackBar1.Value)) * (picture1.B - trackBar1.Value));

                    temp.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                }
            }
            histrogram(temp);
            pictureBox2.Image = temp;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            Bitmap temp = new Bitmap(picture1);
            for (int y = 0; y < picture1Height; y++)
            {
                for (int x = 0; x < picture1Width; x++)
                {
                    Color picture1 = bitmappicture1.GetPixel(x, y);

                    int a = picture1.A;
                    int r = contrastReduction(picture1.R, trackBar2.Value);
                    int g = contrastReduction(picture1.G, trackBar2.Value);
                    int b = contrastReduction(picture1.B, trackBar2.Value);

                    temp.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                }
            }
            histrogram(temp);
            pictureBox2.Image = temp;
        }       

        private void histrogram(Bitmap temp)
        {
            int[] red = new int[256];
            int[] green = new int[256];
            int[] blue = new int[256];

            for (int x = 0; x < picture1Width; x++)
            {
                for (int y = 0; y < picture1Height; y++)
                {
                    Color pixel = temp.GetPixel(x, y);

                    red[pixel.R]++;
                    green[pixel.G]++;
                    blue[pixel.B]++;
                }
            }
            chart1.Series["red"].Points.Clear();
            chart1.Series["green"].Points.Clear();
            chart1.Series["blue"].Points.Clear();

            for (int i = 0; i < 256; i++)

            {
                chart1.Series["red"].Points.AddXY(i, red[i]);
                chart1.Series["green"].Points.AddXY(i, green[i]);
                chart1.Series["blue"].Points.AddXY(i, blue[i]);
            }
            chart1.Invalidate();
        }
    }
}
