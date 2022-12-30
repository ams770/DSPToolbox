using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            List<float> convolution_list = new List<float>();
            List<int> indecies_list = new List<int>();
            for (int n = 0; n < InputSignal1.SamplesIndices.Count() + InputSignal2.SamplesIndices.Count() - 1; n++)
            {
                float y = 0;
                for (int k = 0; k < InputSignal1.SamplesIndices.Count(); k++)
                {
                    if (n - k < InputSignal2.SamplesIndices.Count() && n - k >= 0)
                    {
                        y += InputSignal1.Samples[k] * InputSignal2.Samples[n - k];
                    }
                    else if (n - k >= InputSignal2.SamplesIndices.Count())
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                convolution_list.Add(y);
            }

            for (int i = InputSignal1.SamplesIndices[0] + InputSignal2.SamplesIndices[0]; i < convolution_list.Count(); i++)
            {
                if (indecies_list.Count() == convolution_list.Count())
                {
                    break;
                }
                indecies_list.Add(i);
            }
            OutputConvolvedSignal = new Signal(convolution_list, indecies_list, false);
        }
    }
}
