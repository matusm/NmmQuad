using System.Collections.Generic;
using System.Reflection;

namespace NmmQuad
{
    public class PngMetadata
    {
        public Dictionary<int, string> Metadata => metadata;

        public PngMetadata(string text)
        {
            metadata.Add(0x0320, "SIOS-NMM quadrature signal."); // ImageTitle (short)
            metadata.Add(0x010E, FormatText(text)); // ImageDescription (long)
            metadata.Add(0x013B, "Michael Matus"); // Artist
            metadata.Add(0x8298, GetCopyright()); // Copyright
            metadata.Add(0x0131, GetSoftwareUsed()); // SoftwareUsed
            metadata.Add(0x9286, "red: Lissajous trajectory; blue: zoomed in deviation from LSQ circle."); // Comment
            //metadata.Add(0x0132, "date/time"); // DateTime LEN=20
        }

        private string FormatText(string text)
        {
            string[] sep = new string[] { "\n" };
            string[] tokens = text.Split(sep, System.StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length != 2)
                return text;
            return $"{tokens[0]}-interferometer signal, correction type: {tokens[1]}.";
        }

        private string GetCopyright()
        {
            try
            {
                object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyCopyrightAttribute attribute = attributes[0] as AssemblyCopyrightAttribute;
                    return attribute.Copyright;
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        private string GetSoftwareUsed()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string name = assembly.GetName().Name;
            string version = assembly.GetName().Version.ToString();
            return $"{name} - ver {version}";
        }

        private readonly Dictionary<int, string> metadata = new Dictionary<int, string>(); 

    }

}
