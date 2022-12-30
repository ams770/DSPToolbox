using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FIR : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public FILTER_TYPES InputFilterType { get; set; }
        public float InputFS { get; set; }
        public float? InputCutOffFrequency { get; set; }
        public float? InputF1 { get; set; }
        public float? InputF2 { get; set; }
        public float InputStopBandAttenuation { get; set; }
        public float InputTransitionBand { get; set; }
        public Signal OutputHn { get; set; }
        public Signal OutputYn { get; set; }

        private enum METHOD_TYPES
        {
            RECTANGULAR, HANNING, HAMMING, BLACK_MAN
        }

        public override void Run()
        {
            List<float> samples = new List<float>();
            List<int> indices = new List<int>();
            METHOD_TYPES method = getMethodType();

            int N = calculateN(method);// num of cofficients first iteration = 53        

            int middle = calculateValOfn(N);// first iteration = 26    

            bool isBandPassOrStop = InputFilterType.Equals(FILTER_TYPES.BAND_PASS) || InputFilterType.Equals(FILTER_TYPES.BAND_STOP);

            if (isBandPassOrStop)
            {
                for (int i = -middle; i <= middle; i++)
                {

                    float cutOffFrequency1 = calculateFcDash(true);
                    float deltaFDashNormalized1 = (cutOffFrequency1 / InputFS);

                    float cutOffFrequency2 = calculateFcDash(false);
                    float deltaFDashNormalized2 = (cutOffFrequency2 / InputFS);

                    float hDN = (float)calculateHdOfn(Math.Abs(i), null, deltaFDashNormalized1, deltaFDashNormalized2);
                    float wN = (float)calculateWOfn(method, Math.Abs(i), N);

                    float result = hDN * wN;

                    samples.Add(result);
                    indices.Add(i);
                }
            }

            else
            {
                for (int i = -middle; i <= middle; i++)
                {
                    float cutOffFrequency = calculateFcDash(null);
                    float deltaFDashNormalized = (cutOffFrequency / InputFS);

                    float hDN = (float)calculateHdOfn(Math.Abs(i), deltaFDashNormalized, null, null);
                    float wN = (float)calculateWOfn(method, Math.Abs(i), N);

                    float result = hDN * wN;

                    samples.Add(result);
                    indices.Add(i);
                }
            }
        
            OutputYn = new Signal(new List<float>(), false);
            OutputHn = new Signal(samples, indices, false);
            OutputHn.Frequencies = InputTimeDomainSignal.Frequencies;
            OutputYn = applyConvolution(InputTimeDomainSignal, OutputHn);
        }


        private METHOD_TYPES getMethodType()
        {
            int rectangularMax = 21;
            int hanningMax = 44;
            int hammingMax = 53;
            // int blackManMax = 74;

            if (InputStopBandAttenuation <= rectangularMax) return METHOD_TYPES.RECTANGULAR;
            else if (InputStopBandAttenuation <= hanningMax) return METHOD_TYPES.HANNING;
            else if (InputStopBandAttenuation <= hammingMax) return METHOD_TYPES.HAMMING;

            return METHOD_TYPES.BLACK_MAN;

        }

        private float calculateFcDash(bool? isFirstHalf)
        {

            if (InputFilterType.Equals(FILTER_TYPES.LOW))// << FILTER_TYPES.LOW >>
            {
                return getDeltaFDashAtLowPass(InputCutOffFrequency ?? 0);
            }


            else if (InputFilterType.Equals(FILTER_TYPES.HIGH))// << FILTER_TYPES.HIGH >>
            {
                return getDeltaFDashAtHighPass(InputCutOffFrequency ?? 0);
            }


            else if (InputFilterType.Equals(FILTER_TYPES.BAND_PASS))// << FILTER_TYPES.BAND_PASS >>
            {
                if (isFirstHalf ?? true) // <<<<<< check if in the first half >>>>>>>
                {
                    return getDeltaFDashAtHighPass(InputF1 ?? 0);
                }
                else
                {
                    return getDeltaFDashAtLowPass(InputF2 ?? 0);
                }
            }


            else // << FILTER_TYPES.BAND_STOP >>
            {

                if (isFirstHalf ?? true) // check if in the first half
                {
                    return getDeltaFDashAtLowPass(InputF1 ?? 0);
                }
                else
                {
                    return getDeltaFDashAtHighPass(InputF2 ?? 0);
                }
            }

        }

        private float getDeltaFDashAtLowPass(float previousFc)
        {
            return previousFc + (InputTransitionBand / 2);
        }

        private float getDeltaFDashAtHighPass(float previousFc)
        {
            return previousFc - (InputTransitionBand / 2);
        }


        private int calculateN(METHOD_TYPES method)
        {
            float normalizedVal = InputTransitionBand / InputFS;
            double N = 0;
            
            switch (method)
            {
                case METHOD_TYPES.RECTANGULAR:
                    float rec = 0.9f;
                    N = getNByLaw(rec, normalizedVal);
                    break;

                case METHOD_TYPES.HANNING:
                    float han = 3.1f;
                    N = getNByLaw(han, normalizedVal);
                    break;

                case METHOD_TYPES.HAMMING:
                    float ham = 3.3f;
                    N = getNByLaw(ham, normalizedVal);
                    break;

                case METHOD_TYPES.BLACK_MAN:
                    float bm = 5.5f;
                    N = getNByLaw(bm, normalizedVal);
                    break;

            }

            N = Math.Round(N);
            if (N % 2 == 0)
            {
                N++; // to be always odd >> to get "n" always even
            }
            return (int) N;
        }

        private double getNByLaw(float numerator, float normalizedF)
        {
            return (numerator / normalizedF);
        }


        private int calculateValOfn(int N)
        {
            // >> n = | (N - 1) / 2 |
            int res = (int)Math.Ceiling((double)(N - 1) / 2);
            return res < 0 ? -res : res;
        }




        // <<< hD(n)  Calculations >>>>
        private double calculateHdOfn(int i, float? fc, float? fc1, float? fc2)
        {
            switch (InputFilterType)
            {
                case FILTER_TYPES.LOW:
                    return calculateLowPassHdOfn(i, fc ?? 0);
                case FILTER_TYPES.HIGH:
                    return calculateHighPassHdOfn(i, fc ?? 0);
                case FILTER_TYPES.BAND_PASS:
                    return calculateBandPassHdOfn(i, fc1 ?? 0, fc2 ?? 0);
                case FILTER_TYPES.BAND_STOP:
                    return calculateBandStopHdOfn(i, fc1 ?? 0, fc2 ?? 0);
                default: return 0;
            }
        }

        double calculateLowPassHdOfn(int i, float fc)
        {
            double wc = 2 * ((float)Math.PI) * fc;

            if (i != 0) // n always +ve
            {
                // hD(n) = 2fc( sin(n*wc) / n*wc)
                double angle = Math.Sin(i * 2 * 180 * fc * (Math.PI / 180));
                return (2 * fc) * (angle) / (i * wc);
            }
            else
            {
                // hD(n) = 2fc
                return 2 * fc;
            }
        }


        double calculateHighPassHdOfn(int i, float fc)
        {
            double wc = 2 * ((float)Math.PI) * fc;

            if (i != 0) // n always +ve
            {
                // hD(n) = 2fc( sin(n*wc) / n*wc)
                double angle = Math.Sin(i * 2 * 180 * fc * (Math.PI / 180));
                return (-2 * fc) * (angle) / (i * wc);
            }
            else
            {
                // hD(n) = 1 - 2fc
                return 1 - 2 * fc;
            }
        }

        double calculateBandPassHdOfn(int n, float fc1, float fc2)
        {

            if (n != 0) // n always +ve
            {
                // hD(n) = 2fc( sin(n*wc) / n*wc)
                return calculateLowPassHdOfn(n, fc2) + calculateHighPassHdOfn(n, fc1);
            }
            else
            {
                // hD(n) = 2fc
                return 2 * (fc2 - fc1);
            }
        }


        double calculateBandStopHdOfn(int n, float fc1, float fc2)
        {
            if (n != 0) // n always +ve
            {
                // hD(n) = 2fc( sin(n*wc) / n*wc)
                return calculateLowPassHdOfn(n, fc1) + calculateHighPassHdOfn(n, fc2);
            }
            else
            {
                // hD(n) = 2fc
                return 1 - 2 * (fc2 - fc1);
            }
        }


        // <<< W(n)  Calculations >>>>
        private double calculateWOfn(METHOD_TYPES method, int n, int N)
        {
            switch (method)
            {
                case METHOD_TYPES.RECTANGULAR:
                    return calculateRectangularWOfn();
                case METHOD_TYPES.HANNING:
                    return calculateHanningWOfn(n, N);
                case METHOD_TYPES.HAMMING:
                    return calculateHammingWOfn(n, N);
                case METHOD_TYPES.BLACK_MAN:
                    return calculateBlackManWOfn(n, N);
                default: return 0;
            }
        }

        double calculateRectangularWOfn()
        {
            return 1;
        }

        double calculateHanningWOfn(int n, int N)
        {
            // w(n) = 0.5 + 0.5cos(2(pi)n/N)
            double angle = Math.Cos(2 * Math.PI * n / N);
            return 0.5 + 0.5 * angle;
        }
        double calculateHammingWOfn(int n, int N)
        {
            // w(n) = 0.54 + 0.46cos(2(pi)n/N)
            double angle = Math.Cos(2 * Math.PI * n / N);
            return 0.54 + 0.46 * angle;
        }

        double calculateBlackManWOfn(int n, int N)
        {
            return 0.42 + 0.5 * Math.Cos(2 * Math.PI * n / (N - 1)) + 0.08 * Math.Cos(4 * Math.PI * n / (N - 1));
        }

        Signal applyConvolution(Signal input1, Signal input2)
        {
            Signal output = new Signal(new List<float>(), false);
            DirectConvolution DC = new DirectConvolution();
            DC.InputSignal1 = input1;
            DC.InputSignal2 = input2;
            DC.Run();
            output = DC.OutputConvolvedSignal;
            return output;
        }

    }

}
