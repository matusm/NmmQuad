using At.Matus.StatisticPod;
using Bev.IO.NmmReader.scan_mode;
using System;
using System.Text;

namespace NmmQuad
{
    public class DataAnalyst
    {
        public double AverageRadius => allRadii.AverageValue;
        public double MinimumRadius => allRadii.MinimumValue;
        public double MaximumRadius => allRadii.MaximumValue;
        public double AxisRadius => axisRadii.AverageValue;
        public double MedianRadius => medianRadii.AverageValue;
        public double RelativeSquashing => (MedianRadius - AxisRadius) / AverageRadius;

        public int AllSamples => (int)allRadii.SampleSize;
        public int AxisSamples => (int)axisRadii.SampleSize;
        public int MedianSamples => (int)medianRadii.SampleSize;

        public DataAnalyst(Quad[] data)
        {
            EstimateCircleSquashing(data);
        }

        public string GetReport()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Average radius:             {AverageRadius,6:F1}");
            sb.AppendLine($"Smallest radius:            {MinimumRadius,6:F1}");
            sb.AppendLine($"Largest radius:             {MaximumRadius,6:F1}");
            sb.AppendLine($"Average radius near axis:   {AxisRadius,6:F1}");
            sb.AppendLine($"Average radius near median: {MedianRadius,6:F1}");
            sb.AppendLine($"Relative squashing:         {RelativeSquashing*100,6:F1} %");
            sb.AppendLine($"Total sample size:          {AllSamples}");
            sb.AppendLine($"Sample size for fit:        {AxisSamples+MedianSamples}");
            return sb.ToString();
        }

        private void EstimateCircleSquashing(Quad[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                Quad q = data[i];
                double r = q.Radius;
                double phi = q.PhiDeg;
                allRadii.Update(r);
                if (IsNearToAxis(phi)) axisRadii.Update(r);
                if (IsNearToMedian(phi)) medianRadii.Update(r);
            }
        }

        private bool IsNearToAxis(double phi)
        {
            if (IsNear(phi, 0)) return true;
            if (IsNear(phi, 90)) return true;
            if (IsNear(phi, 180)) return true;
            if (IsNear(phi, -90)) return true;
            if (IsNear(phi, -180)) return true;
            return false;
        }

        private bool IsNearToMedian(double phi)
        {
            if (IsNear(phi, 45)) return true;
            if (IsNear(phi, 135)) return true;
            if (IsNear(phi, -45)) return true;
            if (IsNear(phi, -135)) return true;
            return false;
        }

        private bool IsNear(double phi, double target)
        {
            const double eps = 2; // near is within 2°
            if (Math.Abs(target - phi) < eps) return true;
            return false;
        }

        private readonly StatisticPod allRadii = new StatisticPod();
        private readonly StatisticPod axisRadii = new StatisticPod();
        private readonly StatisticPod medianRadii = new StatisticPod();
    }
}
