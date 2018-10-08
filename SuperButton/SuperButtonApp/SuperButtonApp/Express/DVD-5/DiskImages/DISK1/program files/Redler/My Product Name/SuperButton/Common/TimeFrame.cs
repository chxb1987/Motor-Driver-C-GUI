// *************************************************************************************
// SCICHART © Copyright ABT Software Services Ltd. 2011-2012. All rights reserved.
//  
// Web: http://www.scichart.com
// Support: info@abtsoftware.co.uk
//  
// TimeFrame.cs is part of SciChart Examples, a High Performance WPF & Silverlight Chart. 
// For full terms and conditions of the SciChart license, see http://www.scichart.com/scichart-eula/
//  
// SciChart Examples source code is provided free of charge on an "As-Is" basis to support
// and provide examples of how to use the SciChart component. You bear the risk of using it. 
// The authors give no express warranties, guarantees or conditions. You may have additional 
// consumer rights under your local laws which this license cannot change. To the extent 
// permitted under your local laws, the contributors exclude the implied warranties of 
// merchantability, fitness for a particular purpose and non-infringement. 
// *************************************************************************************

using System.Globalization;
using System.Linq;
using Abt.Controls.SciChart.Example.Common;

namespace SuperButton.Common
{
    public class TimeFrame : StrongTyped<string>
    {        
        public TimeFrame(string value, string displayname) : base(value)
        {
            Displayname = displayname;
        }

        public static readonly TimeFrame Daily = new TimeFrame("Daily", "Daily");
        public static readonly TimeFrame Hourly = new TimeFrame("Hourly", "1 Hour");
        public static readonly TimeFrame Minute15 = new TimeFrame("Minute15", "15 Minutes");

        public string Displayname { get; private set; }

        public static TimeFrame Parse(string input)
        {
            return new[] {Minute15, Hourly, Daily}.Single(x => x.Value.ToUpper(CultureInfo.InvariantCulture) == input.ToUpper(CultureInfo.InvariantCulture));
        }
    }
}