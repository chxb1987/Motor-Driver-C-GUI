using System;
using System.Collections.Generic;
using System.Windows.Media.Animation;

namespace SuperButton.Views
{
    public static class OscilloscopeParameters
    {
        public static List<Tuple<float, float>> ScaleAndGainList= new List<Tuple<float, float>>();

        public static float SingleChanelFreqC = (float) 6666.666667;
        public static float ChanelFreq = SingleChanelFreqC;     
        public static float Step = 1 / SingleChanelFreqC;

        public static float IfullScale = (float) 192.0; // old 576.0
        public static float VfullScale = (float) 68.0;

        public static float FullScale;
        public static float Gain;
        public static float FullScale2;
        public static float Gain2;
        public static float FullScale3;
        public static float Gain3;
        public static float FullScale4;
        public static float Gain4;
        public static int ChanTotalCounter = 0;


        static OscilloscopeParameters()
        {
           
            
        }
        public static void InitList()
        {
            //Init list
            ScaleAndGainList.Add(new Tuple<float, float>((float)1.0, (float)1.0)); //Pause            
            ScaleAndGainList.Add(new Tuple<float, float>((float)1.0, IfullScale));//IqFeedback       
            ScaleAndGainList.Add(new Tuple<float, float>((float)1.0, IfullScale));//I_PhaseA         
            ScaleAndGainList.Add(new Tuple<float, float>((float)1.0, IfullScale));//I_PhaseB         
            ScaleAndGainList.Add(new Tuple<float, float>((float)1.0, IfullScale));//I_PhaseC
            ScaleAndGainList.Add(new Tuple<float, float>((float)2.0, VfullScale));//VDC_Motor
            ScaleAndGainList.Add(new Tuple<float, float>((float)1.0, VfullScale));//BEMF_PhaseA
            ScaleAndGainList.Add(new Tuple<float, float>((float)1.0, VfullScale));//BEMF_PhaseB
            ScaleAndGainList.Add(new Tuple<float, float>((float)1.0, VfullScale));//BEMF_PhaseC
            ScaleAndGainList.Add(new Tuple<float, float>((float)1.0, 1));//HALL_LPF_Speed
            ScaleAndGainList.Add(new Tuple<float, float>((float)1.0, 1));//HALL_Elect_Angle
            ScaleAndGainList.Add(new Tuple<float, float>((float)1.0, 1));//QEP1_LPF_Speed
            ScaleAndGainList.Add(new Tuple<float, float>(1, 1));//QEP1_Elect_Angle
            ScaleAndGainList.Add(new Tuple<float, float>(1, 1));//QEP2_LPF_Speed
            ScaleAndGainList.Add(new Tuple<float, float>(1, 1));//QEP2_Elect_Angle
            ScaleAndGainList.Add(new Tuple<float, float>(1, 1));//SSI_LPF_Speed
            ScaleAndGainList.Add(new Tuple<float, float>(1, 1));//SSI_Elect_Angle
            ScaleAndGainList.Add(new Tuple<float, float>(1, 1));//SL_Elect_Angle
            ScaleAndGainList.Add(new Tuple<float, float>(1, IfullScale));//IRms
            ScaleAndGainList.Add(new Tuple<float, float>(1, IfullScale));//IRms(filterd)
            ScaleAndGainList.Add(new Tuple<float, float>(1, 1));//SL_LPF_Speed
            ScaleAndGainList.Add(new Tuple<float, float>(1, 360));//CommutationAngle
            ScaleAndGainList.Add(new Tuple<float, float>(1, (float)Math.Pow(2, 15)));//PositionFdb
            ScaleAndGainList.Add(new Tuple<float, float>(1, (float)Math.Pow(2, 15)));//PositionRef
            ScaleAndGainList.Add(new Tuple<float, float>((float)1, (float)Math.Pow(2, 15)));//Test_Signal
            ScaleAndGainList.Add(new Tuple<float, float>(1, 1));//Cla_filt0
            ScaleAndGainList.Add(new Tuple<float, float>(1, 1));//Cmd_Ref
            ScaleAndGainList.Add(new Tuple<float, float>(1, 1));//Cmd_Ref_filt

            ScaleAndGainList.Add(new Tuple<float, float>(1, (float)Math.Pow(2, 15)));          // SinEnc
            ScaleAndGainList.Add(new Tuple<float, float>(1, (float)Math.Pow(2, 15)));          // CosEnc
            ScaleAndGainList.Add(new Tuple<float, float>(1, (float)Math.Pow(2, 15)));          // InterAngle
            ScaleAndGainList.Add(new Tuple<float, float>(1, (float)Math.Pow(2, 15)));          // SpeedRefPI
            ScaleAndGainList.Add(new Tuple<float, float>(1, (float)Math.Pow(2, 15)));          // SpeedFdb
            ScaleAndGainList.Add(new Tuple<float, float>(1, IfullScale)); // CurrentRefPI
        }
    }

}