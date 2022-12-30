using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }
        public List<KeyValuePair<float, float>> complex { get; set; }
        public override void Run()
        {

            List<float> frequenciesOut = new List<float>();
            List<float> amplitudesOut = new List<float>();
            List<float> phaseShiftsOut = new List<float>();            

            complex = new List<KeyValuePair<float, float>>();
            OutputFreqDomainSignal = new Signal(InputTimeDomainSignal.Samples, false);
            float omegaVAl = getOmega();
            

            for (int k = 0; k < InputTimeDomainSignal.Samples.Count(); k++)
            {
                float realVal = 0; 
                float imaginVal = 0;

                
                for (int n = 0; n < InputTimeDomainSignal.Samples.Count(); n++)
                {
                    if (k != 0 && n != 0)
                    {
                        float powVal = getPowVAl(n, k);
                        realVal += calcRealVAl(n, powVal);
                        imaginVal += calcImaginVAl(n, powVal);                                                
                    }
                    else
                    {
                        // <<so it has a real val only>>
                        realVal += InputTimeDomainSignal.Samples[n];
                    }
                }
                
                // << generate the complex number >>
                complex.Add(createComplexPair(realVal, imaginVal));

                // << add current signal frequency val >>
                frequenciesOut.Add(getFrequencyVal(k, omegaVAl));

                // << add current signal frequency amplitude >>
                amplitudesOut.Add(getFrequencyAmplitude(k));

                // << add current signal phase shift >>
                phaseShiftsOut.Add(getFrequencyPhaseShift(k));

            }

            OutputFreqDomainSignal = new Signal(false, frequenciesOut, amplitudesOut, phaseShiftsOut);

        }

        private float getFrequencyVal(int k, float omega)
        {
            return k * omega;
        }
        private float getFrequencyAmplitude(int k)
        {
            return (float)Math.Sqrt(Math.Pow(complex[k].Key, 2) + Math.Pow(complex[k].Value, 2));
        }
        private float getFrequencyPhaseShift(int k)
        {
            return (float)Math.Atan2(complex[k].Value, complex[k].Key);
        }


        private float getOmega()
        {
            return (float)((2 * Math.PI * InputSamplingFrequency) / InputTimeDomainSignal.Samples.Count());
        }



        private float calcRealVAl(int n, float powVal)
        {
            return (float)(InputTimeDomainSignal.Samples[n] * Math.Cos(powVal));
        }


        private float calcImaginVAl(int n, float powVal)
        {
            return (float)(-1 * InputTimeDomainSignal.Samples[n] * Math.Sin(powVal));
        }



        private float getPowVAl(int n, int k)
        {
            int N = InputTimeDomainSignal.Samples.Count();
            return (float) ((2 * Math.PI * k * n) / N);            
        }  

                

        private KeyValuePair<float, float> createComplexPair(float key, float val) {
            return new KeyValuePair<float, float>(key, val);
        }

    }
}
