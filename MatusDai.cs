using Bev.IO.NmmReader.scan_mode;

namespace NmmQuad
{
    public class MatusDai
    {
        public Quad[] CorrectedData => matusDai.CorrectedQuadratureValues;
        public double RelativeSquashing => matusDai.RelativeSquashing;

        public MatusDai(Quad[] rawData)
        {
            double[] dummyData = new double[rawData.Length];
            for (int i = 0; i < dummyData.Length; i++)
            {
                dummyData[i] = 0;
            }
            matusDai = new NLcorrectionDai(dummyData, rawData);
        }

        private readonly NLcorrectionDai matusDai;
    }
}
