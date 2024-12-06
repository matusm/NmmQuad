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
        private static Color backgndCol = Color.White;
        private static Color dataDotCol = Color.Red;
        private static Color devDotCol = Color.DarkOrange;
        private static Color circleCol = Color.LightBlue;
        private static Color axisCol = Color.LightGray;

        public Plotter(int size, Quad[] normData)
        {
            imageSize = size;
            bitmap = new Bitmap(imageSize, imageSize, PixelFormat.Format24bppRgb);
            ClearBackgnd(backgndCol);
            PlotDeviation(normData, devDotCol);
            PlotLissajous(normData, dataDotCol);
            DrawAxes(axisCol);
            DrawCircles(circleCol);
        }

        public void SaveImage(string filename)
        {
            ImageFormat f = new ImageFormat(Guid.NewGuid());
            bitmap.Save(filename, f);
        }

        private void ClearBackgnd(Color backgnd)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(backgnd);
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
            SolidBrush aBrush = new SolidBrush(color);
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

        private void PlotDeviation(Quad[] data, Color color)
        {
            SolidBrush aBrush = new SolidBrush(color);
            using (Graphics g=Graphics.FromImage(bitmap))
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
