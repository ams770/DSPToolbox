using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Derivatives: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal FirstDerivative { get; set; }
        public Signal SecondDerivative { get; set; }

        public override void Run()
        {
            List<float> first_derivative = new List<float>(InputSignal.Samples.Count());
            List<float> second_derivative = new List<float>(InputSignal.Samples.Count());

            first_derivative.Add(InputSignal.Samples[0]);
            for (int i = 1; i < InputSignal.Samples.Count() - 1; i++)
            {
                first_derivative.Add(InputSignal.Samples[i] - InputSignal.Samples[i-1]);
            }

            second_derivative.Add(InputSignal.Samples[1] - 2 * InputSignal.Samples[0]);
            for (int i = 1; i < InputSignal.Samples.Count() - 1; i++)
            {
                if (i == InputSignal.Samples.Count()-1)
                {
                    second_derivative.Add(0);
                }
                else
                {
                    second_derivative.Add(InputSignal.Samples[i+1] - 2 * InputSignal.Samples[i] + InputSignal.Samples[i-1]);
                }
            }

            FirstDerivative = new Signal(first_derivative, false);
            SecondDerivative = new Signal(second_derivative, false);
        }
    }
}
