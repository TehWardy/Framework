using System;

namespace Noise
{
    public static class Mathf
    {
        public static double Deg2Rad { get { return (Math.PI * 2) / 360; } }

        public static double Clamp(double value, double min, double max)
        {
            if (value > max)
                return max;

            if (value < min)
                return min;

            return value;
        }

        public static int Clamp(int value, int min, int max)
        {
            if (value > max)
                return max;

            if (value < min)
                return min;

            return value;
        }

        public static float Clamp(float value, float min, float max)
        {
            if (value > max)
                return max;

            if (value < min)
                return min;

            return value;
        }

        public static double Clamp01(float value)
        {
            return Clamp(value, 0.0f, 1.0f);
        }

        public static int Max(int p1, int p2)
        {
            throw new NotImplementedException();
        }

        public static int Min(int p1, int p2)
        {
            throw new NotImplementedException();
        }
    }
}