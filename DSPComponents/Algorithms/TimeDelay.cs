using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class TimeDelay:Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public float InputSamplingPeriod { get; set; }
        public float OutputTimeDelay { get; set; }

        public override void Run()
        {
            List<float> lst = new List<float>();

            // normalize
            float sum_sq1 = 0, sum_sq2 = 0;
            for (int i = 0; i < InputSignal1.Samples.Count(); i++)
            {
                sum_sq1 += (float)Math.Pow(InputSignal1.Samples[i], 2);
                sum_sq2 += (float)Math.Pow(InputSignal2.Samples[i], 2);
            }

            for (int j = 0; j < InputSignal1.Samples.Count(); j++)
            {
                // summation 
                float sum_correlation = 0;
                for (int i = 0; i < InputSignal1.Samples.Count(); i++)
                {
                    sum_correlation += InputSignal1.Samples[i] * InputSignal2.Samples[i];
                }
                float average = sum_correlation / InputSignal1.Samples.Count();

                lst.Add(average);

                // shift left
                float x = InputSignal2.Samples[0];
                for (int i = 0; i < InputSignal2.Samples.Count() - 1; i++)
                {
                    InputSignal2.Samples[i] = InputSignal2.Samples[i + 1];
                }
                InputSignal2.Samples[InputSignal2.Samples.Count() - 1] = x;
            }

            // time delay
            float max_value = Math.Abs(lst[0]);
            int max_index = 0;
            for (int j = 1; j < lst.Count(); j++)
            {
                if (Math.Abs(lst[j]) >= max_value)
                {
                    max_value = Math.Abs(lst[j]);
                    max_index = j;
                }
            }
            OutputTimeDelay = max_index * InputSamplingPeriod;
        }
    }
}
