using At.Matus.IO.NmmReader;
using CommandLine;

namespace NmmQuad
{
    class Options
    {
        private int _size;
        [Option('g', "size", Default = 700, HelpText = "Size of plot graphic (in pixels).")]
        public int BitmapSize
        {
            get => _size;
            set => _size = value < 200 ? 200 : value;
        }

        [Option('s', "scan", Default = 0, HelpText = "Scan index for multi-scan files.")]
        public int ScanIndex { get; set; }

        [Option('p', "profile", Default = 0, HelpText = "Extract single profile only. (0 for all)")]
        public int ProfileIndex { get; set; }

        [Option("back", HelpText = "Use backtrace profile (when present).")]
        public bool UseBack { get; set; }

        [Option("corr", HelpText = "Plot raw data and results of correction methods.")]
        public bool PlotCorr { get; set; }

        [Value(0, MetaName = "InputPath", Required = true, HelpText = "Input file-name including path")]
        public string InputPath { get; set; }

        public TopographyProcessType ScanDirection => GetScanDirection();

        private TopographyProcessType GetScanDirection()
        {
            if (UseBack) return TopographyProcessType.BackwardOnly;
            return TopographyProcessType.ForwardOnly;
        }
    }
}
