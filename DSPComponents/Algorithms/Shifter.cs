using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Shifter : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int ShiftingValue { get; set; }
        public Signal OutputShiftedSignal { get; set; }

        public override void Run()
        {
            List<float> list_of_samples = new List<float>(InputSignal.Samples.Count());
            List<int> SamplesIndexes = new List<int>(InputSignal.Samples.Count());
            for (int i = 0; i < InputSignal.Samples.Count(); i++)
            {
                SamplesIndexes.Add(InputSignal.SamplesIndices[i] + ShiftingValue);
                list_of_samples.Add(InputSignal.Samples[i]);
            }
            OutputShiftedSignal = new Signal(list_of_samples, SamplesIndexes, false);
        }
    }
}
