using At.Matus.StatisticPod;
using Bev.IO.NmmReader.scan_mode;
using System;

namespace NmmQuad
{
    public class DataAnalyst
    {
        public double AverageRadius => allRadii.AverageValue;
        public double MinimumRadius => allRadii.MinimumValue;
        public double MaximumRadius => allRadii.MaximumValue;
        public double AxisRadius => axisRadii.AverageValue;
        public double MedianRadius => medianRadii.AverageValue;

        public int AllSamples => (int)allRadii.SampleSize;
        public int AxisSamples => (int)axisRadii.SampleSize;
        public int MedianSamples => (int)medianRadii.SampleSize;

        public DataAnalyst(Quad[] data)
        {
            EstimateCircleSquashing(data);
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
