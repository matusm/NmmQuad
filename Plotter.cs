using Bev.IO.NmmReader.scan_mode;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace NmmQuad
{
    public class Plotter
    {
        private const int imageSize = 1024;
        private readonly Bitmap bitmap = new Bitmap(imageSize, imageSize, PixelFormat.Format24bppRgb);
        Pen circlePen = new Pen(Color.BlueViolet, 2F);
        Pen otherPen = new Pen(Color.Blue, 2F);

        public Plotter(Quad[] data)
        {
            using (Graphics g = Graphics.FromImage(bitmap)) 
            { 
                g.Clear(Color.White);
                g.DrawEllipse(circlePen, 10, 10, imageSize - 20, imageSize - 20);
            }
            PlotLissajous(data);
        }

        public void SaveImage(string filename)
        {
            ImageFormat f = new ImageFormat(Guid.NewGuid());
            bitmap.Save(filename, f);
        }

        private void PlotLissajous(Quad[] data)
        {
            foreach (var q in data)
            {
                int x = (int)q.Sin + imageSize / 2;
                int y = (int)q.Cos + imageSize / 2;
                bitmap.SetPixel(x, y, Color.Red);
            }
        }
    }
}
