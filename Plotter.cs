using Bev.IO.NmmReader.scan_mode;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace NmmQuad
{
    public class Plotter
    {
        private const int imageSize = 1024;
        private Color backgnd = Color.White;
        private Color dataDot = Color.Red;
        private Pen circlePen = new Pen(Color.BlueViolet, 1F);
        private Pen otherPen = new Pen(Color.Blue, 1F);

        public Plotter(Quad[] data)
        {
            DrawGuideLines();
            PlotLissajous(data);
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
                g.Clear(backgnd);
                g.DrawLine(otherPen, 0, 0, imageSize, imageSize);
                g.DrawLine(otherPen, 0, imageSize, imageSize,0);
                g.DrawLine(otherPen, 0, imageSize/2, imageSize, imageSize/2);
                g.DrawLine(otherPen, imageSize / 2, 0, imageSize/2, imageSize);
                g.DrawEllipse(circlePen, 10, 10, imageSize - 20, imageSize - 20);
            }
        }

        private void PlotLissajous(Quad[] data)
        {
            foreach (var q in data)
            {
                int x = (int)q.Sin + imageSize / 2;
                int y = (int)q.Cos + imageSize / 2;
                bitmap.SetPixel(x, y, dataDot);
            }
        }

        private readonly Bitmap bitmap = new Bitmap(imageSize, imageSize, PixelFormat.Format24bppRgb);
    }
}
