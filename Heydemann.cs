using Bev.IO.NmmReader.scan_mode;

namespace NmmQuad
{
    public class Heydemann
    {
        public Quad[] CorrectedData => heydemann.CorrectedQuadratureValues;

        public Heydemann(Quad[] rawData)
        {
            heydemann = new NLcorrectionHeydemann(rawData);
        }

        private readonly NLcorrectionHeydemann heydemann;
    }
}
