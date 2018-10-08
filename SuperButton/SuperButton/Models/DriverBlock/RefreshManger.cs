using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SuperButton.CommandsDB;
using SuperButton.ViewModels;
namespace SuperButton.Models.DriverBlock
{

    public class RefreshManger
    {

        private static readonly object Synlock = new object();
        private static RefreshManger _instance;

        public static RefreshManger GetInstance
        {
            get
            {
                lock (Synlock)
                {
                    if (_instance != null) return _instance;
                    _instance = new RefreshManger();
                    return _instance;
                }
            }
        }

        public void StartRefreash()
        {

             var list = CommandsDB.Commands.GetInstance.DataViewCommandsList;
              foreach (var command in list)
              {
                if (command.Value.CommandId == "70" || (command.Value.CommandId == "53"))
                {
                    int k = 0; }//|| (command.Value.CommandId == "83"))
                  {
                      Rs232Interface.GetInstance.SendToParser(new PacketFields
                      {
                          Data2Send = "",
                          ID = Convert.ToInt16(command.Value.CommandId),
                          SubID = Convert.ToInt16(command.Value.CommandSubId),
                          IsSet = false,
                          IsFloat = command.Value.IsFloat
                      }
                         ); 
                  }

             Thread.Sleep(30);

             }
        }

        internal void UpdateModel(Tuple<int, int> commandidentifier, string newPropertyValue)
        {
     

            if (commandidentifier.Item1==53 || commandidentifier.Item1==70 )
            {
                int k;
                k = 0;
            }

            if (Commands.GetInstance.DataViewCommandsList.ContainsKey(new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)))
            {
                Commands.GetInstance.DataViewCommandsList[new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)].CommandValue =
                    newPropertyValue;
            }

            if (Commands.GetInstance.EnumViewCommandsList.ContainsKey(new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)))
            {
                Commands.GetInstance.DataViewCommandsList[new Tuple<int, int>(commandidentifier.Item1, commandidentifier.Item2)].CommandValue =
                    newPropertyValue;

            }

        }

    }
}
