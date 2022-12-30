using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MovingAverage : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int InputWindowSize { get; set; }
        public Signal OutputAverageSignal { get; set; }
 
        public override void Run()
        {
            List<float> lst = new List<float>();
            for (int i = 0; i < InputSignal.Samples.Count(); i++)
            {
                if (InputSignal.Samples.Count() - i < InputWindowSize)
                {
                    break;
                }
                float sum = 0;
                for (int j = 0; j < InputWindowSize; j++)
                {
                    sum += InputSignal.Samples[j+i];
                }
                lst.Add(sum / InputWindowSize);
            }
            OutputAverageSignal = new Signal(lst, false);
        }
    }
}
