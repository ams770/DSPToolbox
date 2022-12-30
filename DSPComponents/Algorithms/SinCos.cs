using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class SinCos: Algorithm
    {
        public string type { get; set; }  // sin or cos
        public float A { get; set; }  // ammplitude
        public float PhaseShift { get; set; } //theta
        public float AnalogFrequency { get; set; }//no of cycles per sec
        public float SamplingFrequency { get; set; }// sampling frequency
        public List<float> samples { get; set; }  //output
        public override void Run()
        {
            //our goal in this func: convert from continuous to discrete 
            // to do this goal we divide each component / sampling freq

            samples = new List<float>();
            if (type == "sin")
            {
                for (int i = 0; i < SamplingFrequency; i++)
                {
                    samples.Add(A * (float)Math.Sin((2 * Math.PI * (AnalogFrequency / SamplingFrequency) * i) + PhaseShift));
                }
            }
            else
            {
                for (int i = 0; i < SamplingFrequency; i++)
                {
                    samples.Add(A * (float)Math.Cos((2 * Math.PI * (AnalogFrequency / SamplingFrequency) * i) + PhaseShift));
                }
            }

        }
    }
}
