using Bev.IO.NmmReader.scan_mode;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace NmmQuad
{
    public class Plotter
    {
        private const int imageSize = 800;
        private const double over = 1.15; // 15 % larger
        private static Color backgndCol = Color.White;
        private static Color dataDotCol = Color.Red;
        private static Color circleCol = Color.Blue;
        private static Color axisCol = Color.LightGray;
        private static Pen circlePen = new Pen(circleCol, 1F);
        private static Pen axesPen = new Pen(axisCol, 1F);

        public Plotter(Quad[] normData)
        {
            ClearBackgnd(backgndCol);
            PlotLissajous(normData, dataDotCol);
            DrawAxes(axesPen);
            DrawCircles(circlePen);
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

        private void DrawAxes(Pen pen)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawLine(pen, Transform(-1 * over / Math.Sqrt(2)), Transform(-1 * over / Math.Sqrt(2)), Transform(1 * over / Math.Sqrt(2)), Transform(1 * over / Math.Sqrt(2)));
                g.DrawLine(pen, Transform(-1 * over / Math.Sqrt(2)), Transform(1 * over / Math.Sqrt(2)), Transform(1 * over / Math.Sqrt(2)), Transform(-1 * over / Math.Sqrt(2)));
                g.DrawLine(pen, Transform(-1 * over), Transform(0), Transform(1 * over), Transform(0));
                g.DrawLine(pen, Transform(0), Transform(-1 * over), Transform(0), Transform(1 * over));
            }
        }

        private void DrawCircles(Pen pen)
        {
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
                    g.FillRectangle(aBrush, x, y, 1, 1);
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

        private readonly Bitmap bitmap = new Bitmap(imageSize, imageSize, PixelFormat.Format24bppRgb);
    }

}
