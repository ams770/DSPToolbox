using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Sampling : Algorithm
    {
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }


        public override void Run()
        {
            // !!!!!! Important !!!!!!!
            // in test cases of sampling put the full path of signals in your computer

            FIR firObj = new FIR();
            firObj.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
            firObj.InputFS = 8000;
            firObj.InputStopBandAttenuation = 50;
            firObj.InputCutOffFrequency = 1500;
            firObj.InputTransitionBand = 500;

            // << My Declarations >>
            firObj.InputFilterType = FILTER_TYPES.LOW;
            Signal outSignal = new Signal(new List<float>(), false);

            if (M != 0 && L != 0)
            {
                // Apply Up Sampling First
                Signal s1 = applyUpSampling(InputSignal);

                // Then Apply Low Pass Filter
                firObj.InputTimeDomainSignal = s1;
                firObj.Run();

                // Finally Apply Down Sampling
                outSignal = applyDownSampling(firObj.OutputYn);
            }
            else if (M != 0)
            {
                // Apply Low Pass Filter
                firObj.InputTimeDomainSignal = InputSignal;
                firObj.Run();

                // Then Apply Down Sampling
                outSignal = applyDownSampling(firObj.OutputYn);
            }
            else if (L != 0)
            {
                // Apply Up Sampling
                Signal s = applyUpSampling(InputSignal);

                // Apply Low Pass Filter
                firObj.InputTimeDomainSignal = s;
                firObj.Run();

                // Out
                outSignal = firObj.OutputYn;
            }
            else
            {
                // << Nothing TODO >>
            }
            // << Final Out >>
            OutputSignal = outSignal;

        }

        private Signal applyDownSampling(Signal inputSignal)
        {
            List<float> samples = new List<float>();
            List<int> indices = new List<int>();

            // start index of indices
            int idx = inputSignal.SamplesIndices[0];
            
            // ignore samples by size "M-1" between each input
            for (int i = 0; i < inputSignal.Samples.Count; i += M)// [1,2,3] >> [ 1, 3 ]
            {
                samples.Add(inputSignal.Samples[i]);
                indices.Add(idx);
                idx++;
            }

            return new Signal(samples, indices, false);
        }

        private Signal applyUpSampling(Signal inputSignal)
        {

            List<float> samples = new List<float>();// [ 1, 2, 3 ] >> [1, 0, 2, 0,3]
            List<int> indices = new List<int>();

            // value changes by num of outer and inner loops
            int y = inputSignal.SamplesIndices[0];           

            for (int i = 0; i < inputSignal.Samples.Count; i++)
            {
                samples.Add(inputSignal.Samples[i]);
                indices.Add(y);
               
                if (i == inputSignal.Samples.Count - 1)
                {
                    break;
                }
                   
                // add zeros by size "L-1" between each sample
                for (int x = 0; x < L - 1; x++)
                {
                    samples.Add(0);

                    y += 1;
                    indices.Add(y);
                }

                y += 1;
            }

            return new Signal(samples, indices, false);
        }
    }

}