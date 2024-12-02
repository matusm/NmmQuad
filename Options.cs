using Bev.IO.NmmReader;
using CommandLine;

namespace NmmQuad
{
    class Options
    {
        [Option('s', "scan", Default = 0, HelpText = "Scan index for multi-scan files.")]
        public int ScanIndex { get; set; }

        [Option('p', "profile", Default = 0, HelpText = "Extract single profile. (0 for all)")]
        public int ProfileIndex { get; set; }

        [Option("back", HelpText = "Use backtrace profile (when present).")]
        public bool UseBack { get; set; }

        [Value(0, MetaName = "InputPath", Required = true, HelpText = "Input file-name including path")]
        public string InputPath { get; set; }

        [Value(1, MetaName = "OutputPath", HelpText = "Output file-name including path")]
        public string OutputPath { get; set; }

        public TopographyProcessType ScanDirection => GetScanDirection();

        private TopographyProcessType GetScanDirection()
        {
            if (UseBack) return TopographyProcessType.BackwardOnly;
            return TopographyProcessType.ForwardOnly;
        }
    }
}
