using Bev.IO.NmmReader;
using Bev.IO.NmmReader.scan_mode;
using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NmmQuad
{
    class Program
    {
        private static Options options = new Options(); // this must be set in Run()

        public static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            Parser parser = new Parser(with => with.HelpWriter = null);
            ParserResult<Options> parserResult = parser.ParseArguments<Options>(args);
            parserResult
                .WithParsed<Options>(options => Run(options))
                .WithNotParsed(errs => DisplayHelp(parserResult, errs));
        }

        private static void DisplayHelp<T>(ParserResult<T> result, IEnumerable<Error> errs)
        {
            string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            HelpText helpText = HelpText.AutoBuild(result, h =>
            {
                h.AutoVersion = false;
                h.AdditionalNewLineAfterOption = false;
                h.AddPreOptionsLine("\nProgram to check the quadrature signals of the laser interferometers of SIOS NMM-1. The signals must be recorded in the respective scan files.");
                h.AddPreOptionsLine("");
                h.AddPreOptionsLine($"Usage: {appName} InputPath [OutPath] [options]");
                return HelpText.DefaultParsingErrorsHandler(result, h);
            }, e => e);
            Console.WriteLine(helpText);
        }

        private static void Run(Options ops)
        {
            options = ops;
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            NmmFileName nmmFileName = new NmmFileName(options.InputPath);

            NmmDescriptionFileParser nmmDsc = new NmmDescriptionFileParser(nmmFileName);
            if (nmmDsc.Procedure == MeasurementProcedure.NoFile)
                ErrorExit("!file not found(?)", 1);
            if (nmmDsc.NumberOfScans < options.ScanIndex)
                ErrorExit("!scan number not present in files", 1);
            nmmFileName.SetScanIndex(options.ScanIndex);
            NmmScanData nmmScanData = new NmmScanData(nmmFileName);

            if (!nmmScanData.ColumnPresent("F4"))
                ErrorExit("!sin channel absent", 2);
            if (!nmmScanData.ColumnPresent("F5"))
                ErrorExit("!cos channel absent", 3);
            double[] sinData = nmmScanData.ExtractProfile("F4", options.ProfileIndex, options.ScanDirection);
            double[] cosData = nmmScanData.ExtractProfile("F5", options.ProfileIndex, options.ScanDirection);
            Quad[] data = CombineSignals(sinData, cosData);
            Array.Sort(data);

            DataAnalyst dataAnalyst = new DataAnalyst(data);
            Console.WriteLine(dataAnalyst.GetReport());

            string csvString = CsvContents(data, dataAnalyst);
            string outPutBaseFilename = GetOutputBaseFilename(nmmFileName.BaseFileName, ops);
            File.WriteAllText(outPutBaseFilename+".csv", csvString);
            Console.WriteLine($"Sorted data written in {outPutBaseFilename}");

            Plotter plotter = new Plotter(dataAnalyst.NormalizedData);
            plotter.SaveImage(outPutBaseFilename+".png");

        }
        /**********************************************************************/

        private static string GetOutputBaseFilename(string baseFilename, Options opt)
        {
            if (!string.IsNullOrWhiteSpace(opt.OutputPath))
                return opt.OutputPath;
            string s1 = string.Empty;
            string s2 = string.Empty;
            string s3 = "_f";
            if (opt.ProfileIndex != 0) s1 = $"_p{opt.ProfileIndex}";
            if (opt.ScanIndex != 0) s2 = $"_s{opt.ScanIndex}";
            if (opt.UseBack) s3 = "_b";
            return $"{baseFilename}{s2}{s1}{s3}_Zquad";
        }

        /**********************************************************************/

        private static void ErrorExit(string message, int code)
        {
            Console.WriteLine($"{message} (error code {code})");
            Environment.Exit(code);
        }

        /**********************************************************************/

        private static Quad[] CombineSignals(double[] sinValues, double[] cosValues)
        {
            Quad[] quad = new Quad[sinValues.Length];
            for (int i = 0; i < quad.Length; i++)
            {
                quad[i] = new Quad(sinValues[i], cosValues[i]);
            }
            return quad;
        }

        /**********************************************************************/

        private static string CsvContents(Quad[] data, DataAnalyst dataAnalyst)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.AppendLine($"{data[i].PhiDeg + 180,8:F3}, {data[i].Radius:F3}, {data[i].Radius - dataAnalyst.AverageRadius:F3}");
            }
            return sb.ToString();
        }
    }
}
