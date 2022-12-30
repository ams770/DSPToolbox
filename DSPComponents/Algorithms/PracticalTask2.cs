using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace DSPAlgorithms.Algorithms
{
    public class PracticalTask2 : Algorithm
    {
        public String SignalPath { get; set; }
        public float Fs { get; set; }
        public float miniF { get; set; }
        public float maxF { get; set; }
        public float newFs { get; set; }
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal OutputFreqDomainSignal { get; set; }

        // !!!!!! Important !!!!!!!
        // in test cases of practical task 2 put the full path of signal in your computer

        // << My Declarations >>

        private String myFolderPath = ""; //<< PUT FOLDER PATH TO SAVE FILES >>

        private enum DOMAIN_TYPES
        {
            FREQUENCY, TIME
        }

        public override void Run()
        {
            
            OutputFreqDomainSignal = new Signal(new List<float>(), false);

            Signal InputSignal = LoadSignal(SignalPath);

           

            Signal firOut = getFilteringOut(InputSignal, Fs, miniF, maxF);

           // bool isValidNewFs = isValidFs(newFs, maxF);            
            bool isValidNewFs = newFs >= 2*maxF;            
            if (isValidNewFs)
            {
                firOut = getResampligOut(firOut, L, M);
            }
           
            Signal dcOut = getDcOut(firOut);

            Signal normalizationOut = getNormalizationOut(dcOut);

            // float validNewFs = getValidFs(Fs, newFs, maxF);
            float validNewFs = newFs > 2 * maxF ? newFs : Fs;
            Signal dftOut = getDftOut(normalizationOut, validNewFs);

            OutputFreqDomainSignal = dftOut;
        }
       

        private Signal getFilteringOut(Signal input, float fs, float f1, float f2)
        {
            FIR firObj = new FIR();
            firObj.InputStopBandAttenuation = 50;
            firObj.InputTransitionBand = 500;
            firObj.InputTimeDomainSignal = input;
            firObj.InputFS = fs;
            firObj.InputF1 = f1;
            firObj.InputF2 = f2;
            firObj.InputFilterType = FILTER_TYPES.BAND_PASS;
            firObj.Run();
            displayOut("FIR_OUT", DOMAIN_TYPES.TIME, firObj.OutputYn);
            return firObj.OutputYn;
        }

        private Signal getResampligOut(Signal input, int l, int m)
        {
            Sampling samplingObj = new Sampling();
            samplingObj.InputSignal = input;
            samplingObj.L = l;
            samplingObj.M = m;
            samplingObj.Run();
            displayOut("SAMPLING_OUT", DOMAIN_TYPES.TIME, samplingObj.OutputSignal);
            return samplingObj.OutputSignal;
        }

        private Signal getDcOut(Signal input)
        {
            DC_Component dc = new DC_Component();
            dc.InputSignal = input;
            dc.Run();
            displayOut("DC_OUT", DOMAIN_TYPES.TIME, dc.OutputSignal);
            return dc.OutputSignal;
        }

        private Signal getNormalizationOut(Signal input)
        {
            Normalizer normalizerObj = new Normalizer();
            normalizerObj.InputSignal = input;
            normalizerObj.InputMinRange = -1.0f;
            normalizerObj.InputMaxRange = 1.0f;
            normalizerObj.Run();
            displayOut("NORMALIZATION_OUT", DOMAIN_TYPES.TIME, normalizerObj.OutputNormalizedSignal);
            return normalizerObj.OutputNormalizedSignal;
        }

        private Signal getDftOut(Signal input, float fs)
        {
            DiscreteFourierTransform dftObj = new DiscreteFourierTransform();
            dftObj.InputTimeDomainSignal = input;
            dftObj.InputSamplingFrequency = fs;
            dftObj.Run();
            displayOut("DFT_OUT", DOMAIN_TYPES.FREQUENCY, dftObj.OutputFreqDomainSignal);
            return dftObj.OutputFreqDomainSignal;
        }


        private void displayOut(String fileName, DOMAIN_TYPES type, Signal outSignal)
        {
            displayOutInConsol(fileName, type, outSignal);  
            saveOutInFile(fileName, type,false , outSignal);
            
        }


        private float getValidFs(float fs, float fsNew, float fsMax)
        {
            if(fsNew>2*fsMax)
            {
                return fsNew;
            }
            return fs;
        }

        private bool isValidFs(float fs, float fsMax)
        {
            return fs >= 2 * fsMax;
        }


        private void saveOutInFile(String fileName, DOMAIN_TYPES type , bool isPeriodic, Signal outSignal)
        {
            String fullpath = myFolderPath + "/" + fileName + ".txt";

            using (StreamWriter writer = new StreamWriter(fullpath))
            {
                writer.WriteLine(getDomainTypeNumber(type));
                writer.WriteLine(isPeriodic? 0: 1);
                
                if (type.Equals(DOMAIN_TYPES.TIME))
                {
                    writer.WriteLine(outSignal.Samples.Count);
                    for (int i = 0; i < outSignal.Samples.Count; i++)
                    {
                        writer.Write(outSignal.SamplesIndices[i]);
                        writer.Write(" ");
                        writer.Write(outSignal.Samples[i]);
                        writer.WriteLine();
                    }                    
                }
                else
                {
                    writer.WriteLine(outSignal.Frequencies.Count);
                    for (int i = 0; i < outSignal.Frequencies.Count; i++)
                    {
                        writer.Write(outSignal.Frequencies[i]);
                        writer.Write(" ");
                        writer.Write(outSignal.FrequenciesAmplitudes[i]);
                        writer.Write(" ");
                        writer.Write(outSignal.FrequenciesPhaseShifts[i]);
                        writer.WriteLine();
                    }
                }
                writer.Close();
            }

        }

        private void displayOutInConsol(String fileName, DOMAIN_TYPES type, Signal outSignal)
        {
            Console.WriteLine("==================================");
            Console.WriteLine("      "+fileName);
            Console.WriteLine("==================================");
            
            if (type.Equals(DOMAIN_TYPES.TIME))
            {
                for (int i = 0; i < outSignal.Samples.Count; i++)
                {
                    Console.Write(outSignal.SamplesIndices[i]);
                    Console.Write(" ");
                    Console.Write(outSignal.Samples[i]);
                    Console.WriteLine();
                }                
            }

            else
            {
                for (int i = 0; i < outSignal.Frequencies.Count; i++)
                {
                    Console.Write(outSignal.Frequencies[i]);
                    Console.Write(" ");
                    Console.Write(outSignal.FrequenciesAmplitudes[i]);
                    Console.Write(" ");
                    Console.Write(outSignal.FrequenciesPhaseShifts[i]);
                    Console.WriteLine();
                }
            }
            Console.WriteLine();
        }

        private int getDomainTypeNumber(DOMAIN_TYPES type) {
            return type.Equals(DOMAIN_TYPES.FREQUENCY) ? 1 : 0 ;
        }


        public Signal LoadSignal(string filePath)
        {
            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(stream);

            var sigType = byte.Parse(sr.ReadLine());
            var isPeriodic = byte.Parse(sr.ReadLine());
            long N1 = long.Parse(sr.ReadLine());

            List<float> SigSamples = new List<float>(unchecked((int)N1));
            List<int> SigIndices = new List<int>(unchecked((int)N1));
            List<float> SigFreq = new List<float>(unchecked((int)N1));
            List<float> SigFreqAmp = new List<float>(unchecked((int)N1));
            List<float> SigPhaseShift = new List<float>(unchecked((int)N1));

            if (sigType == 1)
            {
                SigSamples = null;
                SigIndices = null;
            }

            for (int i = 0; i < N1; i++)
            {
                if (sigType == 0 || sigType == 2)
                {
                    var timeIndex_SampleAmplitude = sr.ReadLine().Split();
                    SigIndices.Add(int.Parse(timeIndex_SampleAmplitude[0]));
                    SigSamples.Add(float.Parse(timeIndex_SampleAmplitude[1]));
                }
                else
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            if (!sr.EndOfStream)
            {
                long N2 = long.Parse(sr.ReadLine());

                for (int i = 0; i < N2; i++)
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            stream.Close();
            return new Signal(SigSamples, SigIndices, isPeriodic == 1, SigFreq, SigFreqAmp, SigPhaseShift);
        }


    }
}
