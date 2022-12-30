using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Folder : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputFoldedSignal { get; set; }

        public override void Run()
        {
            List<float> samplesList = new List<float>();
            List<int> indeciesList = new List<int>();

            for (int i = InputSignal.SamplesIndices.Count()-1; i>=0; i--)
            {

                float sampleVal = InputSignal.Samples[i];
                int indiceVal = InputSignal.SamplesIndices[i];
                
                samplesList.Add(sampleVal);
                indeciesList.Add(-1 * indiceVal);

            }
            OutputFoldedSignal = new Signal(samplesList, indeciesList,  false);
        }
    }
}
