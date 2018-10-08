using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace SuperButton.Models.ParserBlock
{
    abstract class ParserBase
    {

        public abstract void ParseOutputData(PacketFields PacketIn);

        //public abstract void ParseOutputData(object Data2Send,Int16 Id,Int16 SubId,bool IsSet,bool IsFloat=false);
    }
}
