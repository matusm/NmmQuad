using Bev.IO.NmmReader.scan_mode;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace NmmQuad
{
    public class Plotter
    {
        private int imageSize;
        private const int dotSize = 2;
        private const double over = 1.15; // 15 % larger
        private const double amplification = 10; // amplify deviation by this amount
        private static Color backgndCol = Color.LightYellow;
        private static Color dataDotCol = Color.Red;
        private static Color devDotCol = Color.DarkOrange;
        private static Color circleCol = Color.DarkGray;
        private static Color axisCol = Color.DarkGray;
        private static Color textCol = Color.Black;

        public Plotter(int size, string text, Quad[] normData)
        {
            imageSize = size;
            bitmap = new Bitmap(imageSize, imageSize, PixelFormat.Format24bppRgb);
            ClearBackgnd(backgndCol);
            DrawAxes(axisCol);
            ClearCenter(backgndCol);
            PlotDeviation(normData, devDotCol);
            PlotLissajous(normData, dataDotCol);
            DrawCircles(circleCol);
            DrawText(text, textCol);
        }

        public void SaveImage(string filename)
        {
            ImageFormat f = new ImageFormat(Guid.NewGuid());
            bitmap.Save(filename, f);
        }

        private void DrawText(string text, Color color)
        {
            int fontSize = (int)((double)imageSize / 30.0);
            if (fontSize < 10) fontSize = 12;
            Font font = new Font("Courier New", fontSize);
            SolidBrush brush = new SolidBrush(color);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            format.Trimming = StringTrimming.None;
            format.FormatFlags = StringFormatFlags.NoClip;
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawString(text, font, brush, Transform(0), Transform(0), format);
            }
        }

        private void ClearBackgnd(Color color)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(color);
            }
        }

        private void ClearCenter(Color color)
        {
            using (SolidBrush aBrush = new SolidBrush(color))
            {
                double radius = 0.85;
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.FillEllipse(aBrush, Corner(radius), Corner(radius), Width(radius), Width(radius));
                }
            }
        }

        private void DrawAxes(Color color)
        {
            Pen pen = new Pen(color, 1F);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawLine(pen, Transform(-1 * over / Math.Sqrt(2)), Transform(-1 * over / Math.Sqrt(2)), Transform(1 * over / Math.Sqrt(2)), Transform(1 * over / Math.Sqrt(2)));
                g.DrawLine(pen, Transform(-1 * over / Math.Sqrt(2)), Transform(1 * over / Math.Sqrt(2)), Transform(1 * over / Math.Sqrt(2)), Transform(-1 * over / Math.Sqrt(2)));
                g.DrawLine(pen, Transform(-1 * over), Transform(0), Transform(1 * over), Transform(0));
                g.DrawLine(pen, Transform(0), Transform(-1 * over), Transform(0), Transform(1 * over));
            }
        }

        private void DrawCircles(Color color)
        {
            Pen pen = new Pen(color, 1F);
            double[] radii = new double[] { 0.9, 0.95, 1, 1.05, 1.1};
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                foreach (double radius in radii)
                {
                    g.DrawEllipse(pen, Corner(radius), Corner(radius), Width(radius), Width(radius));
                }
            }
        }

        private void PlotLissajous(Quad[] data, Color color)
        {
            using (SolidBrush aBrush = new SolidBrush(color))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    foreach (var q in data)
                    {
                        int x = Transform(q.Sin);
                        int y = Transform(q.Cos);
                        g.FillRectangle(aBrush, x, y, dotSize, dotSize);
                    }
                }
            }
        }

        private void PlotDeviation(Quad[] data, Color color)
        {
            using (SolidBrush aBrush = new SolidBrush(color))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    foreach (var q in data)
                    {
                        double deviation = q.Radius - 1;
                        Quad tempQ = new Quad(1 + deviation * amplification, q.Phi, AngleUnit.Radian);
                        int x = Transform(tempQ.Sin);
                        int y = Transform(tempQ.Cos);
                        g.FillRectangle(aBrush, x, y, dotSize, dotSize);
                    }
                }
            }
        }

        private int Transform(double v) => (int)(imageSize * 0.5 * (v / over + 1));

        private int Corner(double radius)
        {
            return Transform(-radius);
        }

        private int Width(double radius)
        {
            return Transform(radius) - Transform(-radius);
        }

        private readonly Bitmap bitmap;
    }

}
