//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using DSPAlgorithms.DataStructures;

//namespace DSPAlgorithms.Algorithms
//{
//    public class QuantizationAndEncoding : Algorithm
//    {
//        // You will have only one of (InputLevel or InputNumBits), the other property will take a negative value
//        // If InputNumBits is given, you need to calculate and set InputLevel value and vice versa
//        public int InputLevel { get; set; }
//        public int InputNumBits { get; set; }
//        public Signal InputSignal { get; set; }
//        public Signal OutputQuantizedSignal { get; set; }
//        public List<int> OutputIntervalIndices { get; set; }
//        public List<string> OutputEncodedSignal { get; set; }
//        public List<float> OutputSamplesError { get; set; }

//        public override void Run()
//        {
//            if (InputLevel < 0)
//            {
//                InputLevel = (int)Math.Pow(2, InputNumBits);
//                //interval size(delta)=(max_amp - min_amp)/noOf_levels
//                float interval_size = (InputSignal.Samples.Max()-InputSignal.Samples.Min())/InputLevel;

//                //  Dictionary<int, > numberNames = new Dictionary<int, string>();

//                KeyValuePair<float, float>[] my_pair = new KeyValuePair<float, float>[InputLevel];

//                Dictionary<int, KeyValuePair<float,float>> my_intervals = new Dictionary<int, KeyValuePair<float, float>>(InputLevel);
//                float start_interval;
//                float end_interval = InputSignal.Samples.Min();
//                for (int i=1;i<=InputLevel;i++)
//                {
//                    start_interval = end_interval;
//                    end_interval = start_interval + interval_size;
//                    my_intervals.Add(i, new KeyValuePair<float, float>(start_interval, end_interval));
//                }

//                foreach(var p in my_intervals)
//                {
//                    KeyValuePair<float, float> pair = p.Value;

//                }
//                //my_pair.First();
//                //var my_pair3= my_intervals[0].Value;

//                //var x = my_intervals.ElementAt(0);
//                //float y = x.Value[0];
//                //foreach (KeyValuePair<float, float> pair in my_intervals.Keys)
//                //{

//                //}
//                //foreach (KeyValuePair<float, float> entry in my_intervals.Values)
//                //{
//                //   KeyValuePair <float,float> k= entry.Value.GetType();
//                //    // do something with entry.Value or entry.Key
//                //}
//                //OutputQuantizedSignal = new Signal(InputSignal.Samples, false);
//                //InputLevel = (int)Math.Pow(2, InputNumBits);
//                //float min_amp = InputSignal.Samples.Min();
//                //float max_amp = InputSignal.Samples.Max();
//                //float delta = (max_amp - min_amp) / InputLevel;
//                //float sum = min_amp;
//                //List<float> midpoints = new List<float>();
//                //for (int i = 0; i < InputLevel; i++)
//                //{
//                //    float sumbefore = sum;
//                //    sum += delta;
//                //    midpoints.Add((sumbefore + sum) / 2);
//                //}
//                //float sub = Math.Abs(Math.Abs(InputSignal.Samples[0]) - Math.Abs(midpoints[0]));
//                //for (int i = 0; i < InputSignal.Samples.Count(); i++)
//                //{
//                //    for (int j = 0; j < midpoints.Count(); j++)
//                //    {
//                //        if (Math.Abs(Math.Abs(InputSignal.Samples[i]) - Math.Abs(midpoints[j])) < sub)
//                //        {
//                //            sub = Math.Abs(Math.Abs(InputSignal.Samples[i]) - Math.Abs(midpoints[j]));
//                //            OutputQuantizedSignal.Samples[i] = midpoints[j];
//                //            OutputIntervalIndices[i] = j + 1;
//                //            OutputSamplesError[i] = midpoints[j] - InputSignal.Samples[i];
//                //        }
//                //    }
//                //}
//                //for (int i = 0; i < InputLevel; i++)
//                //{
//                //    OutputEncodedSignal.Add(Convert.ToString(i, 2));
//                //}



//            }
//            else
//            {
//                OutputQuantizedSignal = new Signal(InputSignal.Samples, false);
//                InputNumBits = (int)Math.Log(InputLevel);
//                float min_amp = InputSignal.Samples.Min();
//                float max_amp = InputSignal.Samples.Max();
//                float delta = (max_amp - min_amp) / InputLevel;
//                float sum = min_amp;
//                List<float> midpoints = new List<float>();
//                for (int i = 0; i < InputLevel; i++)
//                {
//                    float sumbefore = sum;
//                    sum += delta;
//                    midpoints.Add((sumbefore + sum) / 2);
//                }
//                float sub = Math.Abs(Math.Abs(InputSignal.Samples[0]) - Math.Abs(midpoints[0]));
//                for (int i = 0; i < InputSignal.Samples.Count(); i++)
//                {
//                    for (int j = 0; j < midpoints.Count(); j++)
//                    {
//                        if (Math.Abs(Math.Abs(InputSignal.Samples[i]) - Math.Abs(midpoints[j])) < sub)
//                        {
//                            sub = Math.Abs(Math.Abs(InputSignal.Samples[i]) - Math.Abs(midpoints[j]));
//                            OutputQuantizedSignal.Samples[i] = midpoints[j];
//                            OutputIntervalIndices[i] = j + 1;
//                            OutputSamplesError[i] = midpoints[j] - InputSignal.Samples[i];
//                        }
//                    }
//                }
//                for (int i = 0; i < InputLevel; i++)
//                {
//                    OutputEncodedSignal.Add(Convert.ToString(i, 2));
//                }
//            }

//        }
//    }
//}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class QuantizationAndEncoding : Algorithm
    {
        // You will have only one of (InputLevel or InputNumBits), the other property will take a negative value
        // If InputNumBits is given, you need to calculate and set InputLevel value and vice versa
        public int InputLevel { get; set; }
        public int InputNumBits { get; set; }
        public Signal InputSignal { get; set; }
        public Signal OutputQuantizedSignal { get; set; }
        public List<int> OutputIntervalIndices { get; set; }
        public List<string> OutputEncodedSignal { get; set; }
        public List<float> OutputSamplesError { get; set; }

        public override void Run()
        {
            if (InputLevel == 0)
            {
                List<float> lst = new List<float>(InputSignal.SamplesIndices.Count());
                OutputIntervalIndices = new List<int>();
                OutputEncodedSignal = new List<string>();
                OutputSamplesError = new List<float>();

                InputLevel = (int)Math.Pow(2, InputNumBits);
                List<float> midpoints = new List<float>(InputLevel);
                float min_amp = InputSignal.Samples.Min();
                float max_amp = InputSignal.Samples.Max();
                float delta = (max_amp - min_amp) / InputLevel;
                float sum = min_amp;

                for (int i = 0; i < InputLevel; i++)
                {
                    float sumbefore = sum;
                    sum += delta;
                    midpoints.Add((sumbefore + sum) / 2);
                }
                for (int i = 0; i < InputSignal.Samples.Count(); i++)
                {
                    float sub = Math.Abs(InputSignal.Samples[i] - midpoints[0]);
                    lst.Add(0);
                    OutputIntervalIndices.Add(0);
                    OutputSamplesError.Add(0);
                    for (int j = 0; j < midpoints.Count(); j++)
                    {
                        if (Math.Abs(InputSignal.Samples[i] - midpoints[j]) <= sub)
                        {
                            sub = Math.Abs(InputSignal.Samples[i] - midpoints[j]);
                            lst[i] = midpoints[j];
                            OutputIntervalIndices[i] = j + 1;
                            OutputSamplesError[i] = midpoints[j] - InputSignal.Samples[i];
                        }
                    }
                }

                OutputQuantizedSignal = new Signal(lst, false);
                for (int i = 0; i < OutputIntervalIndices.Count(); i++)
                {
                    OutputEncodedSignal.Add(Convert.ToString((OutputIntervalIndices[i] - 1), 2));
                    while (OutputEncodedSignal[i].Length != InputNumBits)
                    {
                        OutputEncodedSignal[i] = "0" + OutputEncodedSignal[i];
                    }
                }
            }

            else
            {
                List<float> lst = new List<float>(InputSignal.SamplesIndices.Count());
                OutputIntervalIndices = new List<int>();
                OutputEncodedSignal = new List<string>();
                OutputSamplesError = new List<float>();

                InputNumBits = (int)Math.Log(InputLevel, 2.0);
                List<float> midpoints = new List<float>(InputLevel);
                float min_amp = InputSignal.Samples.Min();
                float max_amp = InputSignal.Samples.Max();
                float delta = (max_amp - min_amp) / InputLevel;
                float sum = min_amp;

                for (int i = 0; i < InputLevel; i++)
                {
                    float sumbefore = sum;
                    sum += delta;
                    midpoints.Add((sumbefore + sum) / 2);
                }
                for (int i = 0; i < InputSignal.Samples.Count(); i++)
                {
                    float sub = Math.Abs(InputSignal.Samples[i] - midpoints[0]);
                    lst.Add(0);
                    OutputIntervalIndices.Add(0);
                    OutputSamplesError.Add(0);
                    for (int j = 0; j < midpoints.Count(); j++)
                    {
                        if (Math.Abs(InputSignal.Samples[i] - midpoints[j]) <= sub)
                        {
                            sub = Math.Abs(InputSignal.Samples[i] - midpoints[j]);
                            lst[i] = midpoints[j];
                            OutputIntervalIndices[i] = j + 1;
                            OutputSamplesError[i] = midpoints[j] - InputSignal.Samples[i];
                        }
                    }
                }

                OutputQuantizedSignal = new Signal(lst, false);
                for (int i = 0; i < OutputIntervalIndices.Count(); i++)
                {
                    OutputEncodedSignal.Add(Convert.ToString((OutputIntervalIndices[i] - 1), 2));
                    while (OutputEncodedSignal[i].Length != InputNumBits)
                    {
                        OutputEncodedSignal[i] = "0" + OutputEncodedSignal[i];
                    }
                }
            }
        }
    }
}
