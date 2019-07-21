using SuperButton.Models.DriverBlock;
using System;
using System.Collections.Generic;
using System.Windows.Media.Animation;

namespace SuperButton.Views
{
    public static class OscilloscopeParameters
    {
        public static List<Tuple<float, float>> ScaleAndGainList= new List<Tuple<float, float>>();
        public static List<Int32> plotGeneral = new List<Int32>();
        public static List<float> plotFullScale = new List<float>();
        public static float SingleChanelFreqC = (float) 6666.666667;
        public static float ChanelFreq = SingleChanelFreqC;     
        public static float Step = 1 / SingleChanelFreqC;

        public static float IfullScale = (float) 192.0; // old 576.0
        public static float VfullScale = (float) 68.0;

        public static float FullScale;
        public static float Gain;
        public static float FullScale2;
        public static float Gain2;

        public static int ChanTotalCounter = 0;

        public static int plotCount = 0;
        public static int plotCount_temp = 0;

        static string[] plotName = new[] {
        "None",
        "Iq feedback", "I Phase A", "I Phase B", "I Phase c", "IRms",
        "Filtered Irms", "I PSU", "BEMF Phase A", "BEMF Phase B", "BEMF Phase C",
        "VDC Motor", "VDC 12v", "VDC 5v", "VDC 3v", "VDC Ref",
        "Analog Command", "Sin Analog Enc", "Cos Analog Enc", "Hal mechanical angle", "Qep1 mechanical angle",
        "Qep2 mechanical angle", "SSI mechanical angle", "Sin Cos  mechanical angle", "Com mechanical angle", "Commutation angle",
        "HALL speed", "HALL Elect Angle", "HALL position", "Enc1 speed", "Enc1 Elect angle",
        "Enc1 position", "Enc2 speed", "Enc2 Elect angle", "Enc2 position", "Sensorless speed",
        "Sensorless Elect angle", "Sin Cos Angle", "Delta Hall Enc", "Current Cmd", "Speed Cmd",
        "Position Cmd", "Iq Ref", "Id Ref", "Speed Fdb", "Speed Ref",
        "Speed Fdb Filt", "Position Fdb", "Position Ref", "Digital In 1", "Digital In 2",
        "Digital In 3", "Digital In 4", "Digital In 5", "Digital In 6", "Digital In 7",
        "Digital In 8", "Digital Out 1", "Digital Out 2", "Digital Out 3", "Digital Out 4",
        "Digital Out 5", "Digital Out 6", "Digital Out 7", "Digital Out 8", "Cla Debug 1",
        "Cla Debug 2", "Test Signal 1", "Test Signal2", "Test Signal3", "Test Signal4"
        };
        static List<string> plotName_ls = new List<string>();

        static string[] plotType = new[] { "Integer", "Float", "Iq24", "Iq15" };
        static string[] plotUnit = new[] { "Amper", "Volt", "", "", "", "Elec Angle", "mechanical Angle", "", "", "", "RPM Per Volt", "Count Per Sec", "Round Per Minute", "Counts" };

        static OscilloscopeParameters()
        {
            for(int i = 0; i < plotName.Length; i++)
                plotName_ls.Add(plotName[i]);
        }

        public static void fillPlotList()
        {
            if(plotCount == plotCount_temp)
            {
                plotGeneral.Clear();
                plotFullScale.Clear();
                OscilloscopeViewModel.GetInstance.Channel1SourceItems.Clear();
                OscilloscopeViewModel.GetInstance.ChannelYtitles.Clear();
                ScaleAndGainList.Clear();
            }

            Rs232Interface.GetInstance.SendToParser(new PacketFields
            {
                Data2Send = "",
                ID = 35,
                SubID = Convert.ToInt16(plotCount - plotCount_temp + 1),
                IsSet = false,
                IsFloat = false
            });
            Rs232Interface.GetInstance.SendToParser(new PacketFields
            {
                Data2Send = "",
                ID = 36,
                SubID = Convert.ToInt16(plotCount - plotCount_temp + 1),
                IsSet = false,
                IsFloat = false
            });
            plotCount_temp--;
            if(plotCount_temp == 0)
            {
                buildPlotList();
            }
        }
        private static void buildPlotList()
        {
            int i = 0;
            OscilloscopeViewModel.GetInstance.Channel1SourceItems.Add("Pause");
            OscilloscopeViewModel.GetInstance.ChannelYtitles.Add("Pause", "");
            ScaleAndGainList.Add(new Tuple<float, float>((float)1.0, (float)1.0)); //Pause
            foreach(var element in plotGeneral)
            {
                if(element > 0)
                {
                    OscilloscopeViewModel.GetInstance.Channel1SourceItems.Add(plotName_ls[element & 0xFFFF]);
                    OscilloscopeViewModel.GetInstance.ChannelYtitles.Add(plotName_ls[element & 0xFFFF], plotUnit[(element >> 24) & 0xFF]);
                    ScaleAndGainList.Add(new Tuple<float, float>(1, plotFullScale[i]));
                }
                i++;
             }
        }
        public static void InitList()
        {
            /*
            //Init list
            ScaleAndGainList.Add(new Tuple<float, float>((float)1.0, (float)1.0)); //Pause            
            ScaleAndGainList.Add(new Tuple<float, float>((float)1.0, IfullScale));//IqFeedback       
            ScaleAndGainList.Add(new Tuple<float, float>((float)1.0, IfullScale));//I_PhaseA         
            ScaleAndGainList.Add(new Tuple<float, float>((float)1.0, IfullScale));//I_PhaseB         
            ScaleAndGainList.Add(new Tuple<float, float>((float)1.0, IfullScale));//I_PhaseC
            ScaleAndGainList.Add(new Tuple<float, float>((float)1.0, VfullScale));//VDC_Motor // 2.0
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
            */
        }
    }

}