using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Normalizer : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputMinRange { get; set; }
        public float InputMaxRange { get; set; }
        public Signal OutputNormalizedSignal { get; set; }

        public override void Run()
        {
            
            List<float> lst = new List<float>();
            for (int i = 0; i < InputSignal.Samples.Count(); i++)
            {
                lst.Add(((InputSignal.Samples[i] - InputSignal.Samples.Min()) / (InputSignal.Samples.Max() - InputSignal.Samples.Min())) * (InputMaxRange - InputMinRange) + InputMinRange);
            }
            OutputNormalizedSignal = new Signal(lst, false);
            OutputNormalizedSignal.SamplesIndices = InputSignal.SamplesIndices;
           
        }
    }
}
