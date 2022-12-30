using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();

            List<float> lst = new List<float>();
            if (InputSignal2 == null) //auto correlation
            {
                for (int i = 0; i < InputSignal1.Samples.Count(); i++)
                {
                    lst.Add(InputSignal1.Samples[i]);
                }
                InputSignal2 = new Signal(lst, false);
            }

            // normalize
            float sum_sq1 = 0, sum_sq2 = 0;
            for (int i = 0; i < InputSignal1.Samples.Count(); i++)
            {
                sum_sq1 += (float)Math.Pow(InputSignal1.Samples[i], 2);
                sum_sq2 += (float)Math.Pow(InputSignal2.Samples[i], 2);
            }

            for (int j = 0; j < InputSignal1.Samples.Count; j++)
            {
                // summation 
                float sum_correlation = 0;
                for (int i = 0; i < InputSignal1.Samples.Count(); i++)
                {
                    sum_correlation += InputSignal1.Samples[i] * InputSignal2.Samples[i];
                }

                float average = sum_correlation / InputSignal1.Samples.Count();

                OutputNonNormalizedCorrelation.Add(average);
                OutputNormalizedCorrelation.Add((float)(average / (Math.Sqrt(sum_sq1 * sum_sq2) / InputSignal1.Samples.Count)));

                // shift left
                float x;
                if (InputSignal1.Periodic == true)
                {
                    x = InputSignal2.Samples[0];
                }
                else
                {
                    x = 0;
                }
                for (int i = 0; i < InputSignal2.Samples.Count() - 1; i++)
                {
                    InputSignal2.Samples[i] = InputSignal2.Samples[i + 1];
                }
                InputSignal2.Samples[InputSignal2.Samples.Count() - 1] = x;
            }
        }
    }
}