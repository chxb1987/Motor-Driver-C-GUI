// *************************************************************************************
// SCICHART © Copyright ABT Software Services Ltd. 2011-2012. All rights reserved.
//  
// Web: http://www.scichart.com
// Support: info@abtsoftware.co.uk
//  
// DoubleSeries.cs is part of SciChart Examples, a High Performance WPF & Silverlight Chart. 
// For full terms and conditions of the SciChart license, see http://www.scichart.com/scichart-eula/
//  
// SciChart Examples source code is provided free of charge on an "As-Is" basis to support
// and provide examples of how to use the SciChart component. You bear the risk of using it. 
// The authors give no express warranties, guarantees or conditions. You may have additional 
// consumer rights under your local laws which this license cannot change. To the extent 
// permitted under your local laws, the contributors exclude the implied warranties of 
// merchantability, fitness for a particular purpose and non-infringement. 
// *************************************************************************************

using System.Collections.Generic;
using System.Linq;
using Abt.Controls.SciChart.Example.Data;

namespace SuperButton.Data
{
    /// <summary>
    /// A data-structure to contain a list of X,Y double-precision points
    /// </summary>
    public class DoubleSeries : List<XYPoint>
    {
        public DoubleSeries()
        {            
        }

        public DoubleSeries(int capacity) : base(capacity)
        {            
        }

        public double[] XData { get { return this.Select(x => x.X).ToArray(); } }
        public double[] YData { get { return this.Select(x => x.Y).ToArray(); } }

       // public IList<double> XData { get { return this.Select(x => x.X).ToArray(); } }
       // public IList<double> YData { get { return this.Select(x => x.Y).ToArray(); } }
    }
}