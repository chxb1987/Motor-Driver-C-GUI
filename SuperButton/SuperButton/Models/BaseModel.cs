using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperButton.Models
{
    class BaseModel
    {
        private string _commandName;
        private string _commandValue;
        private string _commandId;
        private string _commandSubId;
        private bool _isFloat;
        private bool _isSelected;

        public string CommandName { get { return _commandName; } set { _commandName = value; } }

        public string CommandValue { get { return _commandValue; } set { _commandValue = value; } }

        public string CommandID { get { return _commandId; } set { _commandId = value; } }

        public string CommandSubID { get { return _commandSubId; } set { _commandSubId = value; } }
        public bool IsFloat { get { return _isFloat; } set { _isFloat = value; } }
        public bool IsSelected { get { return _isSelected; } set { _isSelected = value; } }


    }
}
