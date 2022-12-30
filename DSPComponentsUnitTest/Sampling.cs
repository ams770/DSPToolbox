﻿using DSPAlgorithms.DataStructures;
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
            FIR_Obj_Name.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
            FIR_Obj_Name.InputFS = 8000;
            FIR_Obj_Name.InputStopBandAttenuation = 50;
            FIR_Obj_Name.InputCutOffFrequency = 1500;
            FIR_Obj_Name.InputTransitionBand = 500;

            FIR_Obj_Name.Run();
            fdlkcm



        }
    }

}