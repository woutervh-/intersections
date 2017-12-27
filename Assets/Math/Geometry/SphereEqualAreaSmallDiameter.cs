using System;
using UnityEngine;

namespace Math.Geometry
{
    public static class SphereEqualAreaSmallDiameter
    {
        private static float AreaOfIdealRegion(int dim, int n, float minColatitude = 0f, float maxColatitude = Mathf.PI)
        {
            return AreaOfCollar(dim, minColatitude, maxColatitude) / n;
        }

        private static float PolarColatitude(int dim, int n)
        {
            if (n == 1)
            {
                return Mathf.PI;
            }
            else if (n == 2)
            {
                return Mathf.PI / 2f;
            }
            else
            {
                return ColatitudeOfCap(dim, AreaOfIdealRegion(dim, n));
            }
        }

        private static float ColatitudeOfCap(int dim, float area)
        {
            if (dim == 1)
            {
                return area / 2f;
            }
            else if (dim == 2)
            {
                return 2f * Mathf.Asin(Mathf.Sqrt(area / Mathf.PI) / 2f);
            }

            throw new ArgumentOutOfRangeException("dim", "must be 1 <= dim <= 2");
        }

        private static float IdealCollarAngle(int dim, int n)
        {
            return Mathf.Pow(AreaOfIdealRegion(dim, n), 1f / dim);
        }

        private static int NumberCollars(int n, float polarColatitude, float idealCollarAngle)
        {
            if (n > 2 && idealCollarAngle > 0f)
            {
                return System.Math.Max(1, Mathf.RoundToInt((Mathf.PI - 2f * polarColatitude) / idealCollarAngle));
            }
            else
            {
                return 0;
            }
        }

        private static float AreaOfCap(int dim, float colatitude)
        {
            if (dim == 1)
            {
                return 2f * colatitude;
            }
            else if (dim == 2)
            {
                return 4f * Mathf.PI * Mathf.Pow(Mathf.Sin(colatitude / 2f), 2f);
            }

            throw new ArgumentOutOfRangeException("dim", "must be 1 <= dim <= 2");
        }

        private static float AreaOfCollar(int dim, float topColatitude, float bottomColatitude)
        {
            return AreaOfCap(dim, bottomColatitude) - AreaOfCap(dim, topColatitude);
        }

        private static int[] RoundIdealRegionList(int n, float[] idealRegionList)
        {
            int[] regionList = new int[idealRegionList.Length];
            float discrepancy = 0f;
            for (int i = 0; i < idealRegionList.Length; i++)
            {
                regionList[i] = Mathf.RoundToInt(idealRegionList[i] + discrepancy);
                discrepancy += idealRegionList[i] - regionList[i];
            }
            return regionList;
        }

        private static float[] IdealRegionList(int dim, int n, float polarColatitude, int numberCollars)
        {
            float[] regionList = new float[numberCollars + 2];
            regionList[0] = 1f;
            if (numberCollars >= 1)
            {
                float angleFitting = (Mathf.PI - 2f * polarColatitude) / numberCollars;
                float idealRegionArea = AreaOfIdealRegion(dim, n);
                for (int i = 0; i < numberCollars; i++)
                {
                    float idealCollarArea = AreaOfCollar(dim, polarColatitude + i * angleFitting, polarColatitude + (i + 1) * angleFitting);
                    regionList[i + 1] = idealCollarArea / idealRegionArea;
                }
            }
            regionList[numberCollars + 1] = 1f;
            return regionList;
        }

        private static float[] CapColatitudes(int dim, int n, float polarColatitude, int[] regionList)
        {
            float[] colatitudes = new float[regionList.Length];
            float idealRegionArea = AreaOfIdealRegion(dim, n);
            int numberCollars = regionList.Length - 2;
            int subtotal = 1;

            colatitudes[0] = polarColatitude;
            for (int i = 0; i < numberCollars; i++)
            {
                subtotal += regionList[i + 1];
                colatitudes[i + 1] = ColatitudeOfCap(dim, subtotal * idealRegionArea);
            }
            colatitudes[numberCollars + 1] = Mathf.PI;

            return colatitudes;
        }

        public static Tuple<float[], int[]> GenerateCaps(int dim, int n, float minPolarColatitude)
        {
            if (dim == 1)
            {
                float[] colatitudes = new float[n];
                int[] regionList = new int[n];
                for (int i = 0; i < n; i++)
                {
                    colatitudes[i] = (i + 1) * 2f * Mathf.PI / n;
                    regionList[i] = 1;
                }
                return new Tuple<float[], int[]>(colatitudes, regionList);
            }
            else if (n == 1)
            {
                float[] colatitudes = new float[1] { Mathf.PI };
                int[] regionList = new int[1] { 1 };
                return new Tuple<float[], int[]>(colatitudes, regionList);
            }
            else
            {
                float polarColatitude = PolarColatitude(dim, n);
                float idealCollarAngle = IdealCollarAngle(dim, n);
                int numberOfCollars = NumberCollars(n, polarColatitude, idealCollarAngle);
                float[] idealRegionList = IdealRegionList(dim, n, polarColatitude, numberOfCollars);
                int[] regionList = RoundIdealRegionList(n, idealRegionList);
                float[] colatitudes = CapColatitudes(dim, n, polarColatitude, regionList);
                return new Tuple<float[], int[]>(colatitudes, regionList);
            }
        }
    }
}
