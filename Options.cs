using CommandLine;

namespace NmmQuad
{
    class Options
    {
        [Option('s', "scan", Default = 0, HelpText = "Scan index for multi-scan files.")]
        public int ScanIndex { get; set; }

        [Value(0, MetaName = "InputPath", Required = true, HelpText = "Input file-name including path")]
        public string InputPath { get; set; }

        [Value(1, MetaName = "OutputPath", HelpText = "Output file-name including path")]
        public string OutputPath { get; set; }
    }
}
