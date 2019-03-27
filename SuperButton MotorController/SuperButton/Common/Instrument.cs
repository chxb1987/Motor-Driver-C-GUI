// *************************************************************************************
// SCICHART © Copyright ABT Software Services Ltd. 2011-2012. All rights reserved.
//  
// Web: http://www.scichart.com
// Support: info@abtsoftware.co.uk
//  
// Instrument.cs is part of SciChart Examples, a High Performance WPF & Silverlight Chart. 
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

namespace Abt.Controls.SciChart.Example.Common
{
    public class Instrument : StrongTyped<string>
    {
        public string InstrumentName { get; private set; }
        public string Symbol { get { return Value; } }
        public int DecimalPlaces { get; private set; }

        public Instrument(string value, string instrumentName, int decimalPlaces) : base(value)
        {
            InstrumentName = instrumentName;
            DecimalPlaces = decimalPlaces;
        }

        public static Instrument Parse(string instrumentString)
        {
            return new[] { EurUsd, Indu, Spx500, CrudeOil, Test }.Single(x => x.Symbol.ToUpper(CultureInfo.InvariantCulture) == instrumentString.ToUpper(CultureInfo.InvariantCulture));
        }

        public static readonly Instrument EurUsd = new Instrument("EURUSD", "FX Euro US Dollar", 4);
        public static readonly Instrument Indu = new Instrument("INDU", "Dow Jones Industrial Average", 0);
        public static readonly Instrument Spx500 = new Instrument("SPX500", "S&P500 Index", 0);
        public static readonly Instrument CrudeOil = new Instrument("CL", "Light Crude Oil", 0);
        public static readonly Instrument Test = new Instrument("TEST", "Test data only", 0);
    }
}