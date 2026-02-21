using At.Matus.IO.NmmReader.Interferometry;

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
