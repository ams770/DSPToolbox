using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DC_Component: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            //Removing the mean of the signal 
            
            OutputSignal = new Signal(InputSignal.Samples, false);
            
            float sum = 0, mean;
            
            for(int i = 0; i < InputSignal.Samples.Count(); i++)
            {
                sum+=InputSignal.Samples[i];    
            }
            
            mean = sum/InputSignal.Samples.Count();

            for (int i = 0; i < InputSignal.Samples.Count(); i++)
            {                
                OutputSignal.Samples[i]=InputSignal.Samples[i]-mean;
            }
            OutputSignal.SamplesIndices = InputSignal.SamplesIndices;
        }
    }
}
