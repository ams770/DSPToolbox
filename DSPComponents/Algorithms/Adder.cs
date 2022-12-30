using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Adder : Algorithm
    {
        public List<Signal> InputSignals { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            OutputSignal = new Signal(InputSignals[0].Samples, false);
           
            for (int i = 0; i < InputSignals[0].Samples.Count(); i++)
            {
                OutputSignal.Samples[i]= InputSignals[0].Samples[i] + InputSignals[1].Samples[i];
            }

            
            /*for (int i = 0; i < InputSignals[0].Samples.Count(); i++)
            {
                float sumation = 0;
                for (int j = 0; j < InputSignals.Count(); j++)
                {
                    sumation += InputSignals[j].Samples[i];
                }
                OutputSignal.Samples[i] = sumation;
            }*/


        }
    }
}