using Bev.IO.NmmReader.scan_mode;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace NmmQuad
{
    public class Plotter
    {
        private const int imageSize = 800;
        private const double over = 1.1; // 10 % larger
        private Color backgnd = Color.White;
        private Color dataDot = Color.Red;
        private Pen circlePen = new Pen(Color.Blue, 1F);
        private Pen otherPen = new Pen(Color.LightGray, 1F);

        public Plotter(Quad[] data)
        {
            ClearBackgnd(backgnd);
            PlotLissajous(data);
            DrawGuideLines();
        }

        private void ClearBackgnd(Color backgnd)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(backgnd);
            }
        }

        public void SaveImage(string filename)
        {
            ImageFormat f = new ImageFormat(Guid.NewGuid());
            bitmap.Save(filename, f);
        }

        private void DrawGuideLines()
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawLine(otherPen, Transform(-1 * over / Math.Sqrt(2)), Transform(-1 * over / Math.Sqrt(2)), Transform(1 * over / Math.Sqrt(2)), Transform(1 * over / Math.Sqrt(2)));
                g.DrawLine(otherPen, Transform(-1 * over / Math.Sqrt(2)), Transform(1 * over / Math.Sqrt(2)), Transform(1 * over / Math.Sqrt(2)), Transform(-1 * over / Math.Sqrt(2)));
                g.DrawLine(otherPen, Transform(-1 * over), Transform(0), Transform(1 * over), Transform(0));
                g.DrawLine(otherPen, Transform(0), Transform(-1 * over), Transform(0), Transform(1 * over));
                g.DrawEllipse(circlePen, Transform(-1), Transform(-1), Transform(1 / over), Transform(1 / over));
                g.DrawEllipse(circlePen, Transform(-1.05), Transform(-1.05), Transform(1.1 / over), Transform(1.1 / over));

            }
        }

        private void PlotLissajous(Quad[] data)
        {
            foreach (var q in data)
            {
                int x = Transform(q.Sin);
                int y = Transform(q.Cos);
                if (x >= 0 && x < imageSize && y >= 0 && y < imageSize)
                    bitmap.SetPixel(x, y, dataDot);
            }
        }


        private int Transform(double v) => (int)(imageSize * 0.5 * (v / over + 1));

        private readonly Bitmap bitmap = new Bitmap(imageSize, imageSize, PixelFormat.Format24bppRgb);
    }
}
