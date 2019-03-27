using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperButton.Common
{
    public class Consts
    {
        private static Consts _instance = new Consts();



        #region CONSTS
        public const int BOOL_IDLE = 0;
        public const int BOOL_PASSED = 1;
        public const int BOOL_FAILED = 2;
        public const int BOOL_DISABLED = 3;
        public const int BOOL_RUNNING = 4;

        public const int INTERFACE_RS232 = 1;
        public const int CAN_DRIVER_KVASER = 0;


        public const int KEY_HISTORY_DIR = 1;
        public const int KEY_PARAMS_DIR = 2;



        private Dictionary<int, string> dic_paths = new Dictionary<int, string>()
                                                                   {
                                                                       {KEY_HISTORY_DIR,Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\MotorController\\SuperMotorController_Params"},
                                                                       {KEY_PARAMS_DIR, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\MotorController\\SuperMotorControllerHistory"}
                                                                   };
        public static Dictionary<int, string> Dic_paths { get { return _instance.dic_paths; } }
        #endregion


    }
}