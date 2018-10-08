// *************************************************************************************
// SCICHART © Copyright ABT Software Services Ltd. 2011-2012. All rights reserved.
//  
// Web: http://www.scichart.com
// Support: info@abtsoftware.co.uk
//  
// PriceSeries.cs is part of SciChart Examples, a High Performance WPF & Silverlight Chart. 
// For full terms and conditions of the SciChart license, see http://www.scichart.com/scichart-eula/
//  
// SciChart Examples source code is provided free of charge on an "As-Is" basis to support
// and provide examples of how to use the SciChart component. You bear the risk of using it. 
// The authors give no express warranties, guarantees or conditions. You may have additional 
// consumer rights under your local laws which this license cannot change. To the extent 
// permitted under your local laws, the contributors exclude the implied warranties of 
// merchantability, fitness for a particular purpose and non-infringement. 
// *************************************************************************************
using System;
using System.Linq;
using System.Collections.Generic;

namespace Abt.Controls.SciChart.Example.Data
{
    public class PriceSeries : List<PriceBar>
    {
        public string Symbol { get; set; }

        public PriceSeries()
        {            
        }

        public PriceSeries(int capacity) : base(capacity)
        {            
        }

        /// <summary>
        /// Extracts the DateTime column of the PriceSeries as an array
        /// </summary>
        public IList<DateTime> TimeData { get { return this.Select(x => x.DateTime).ToArray(); } }

        /// <summary>
        /// Extracts the Open column of the PriceSeries as an array
        /// </summary>
        public IList<double> OpenData { get { return this.Select(x => x.Open).ToArray(); } }

        /// <summary>
        /// Extracts the High column of the PriceSeries as an array
        /// </summary>
        public IList<double> HighData { get { return this.Select(x => x.High).ToArray(); } }

        /// <summary>
        /// Extracts the Low column of the PriceSeries as an array
        /// </summary>
        public IList<double> LowData { get { return this.Select(x => x.Low).ToArray(); } }

        /// <summary>
        /// Extracts the Close column of the PriceSeries as an array
        /// </summary>
        public IList<double> CloseData { get { return this.Select(x => x.Close).ToArray(); } }

        /// <summary>
        /// Extracts the Volume column of the PriceSeries as an array
        /// </summary>
        public IList<long> VolumeData { get { return this.Select(x => x.Volume).ToArray(); } }

        public PriceSeries Clip(int startIndex, int endIndex)
        {
            var result = new PriceSeries(endIndex - startIndex);
            for(int i = startIndex; i < endIndex; i++)
            {
                result.Add(this[i]);
            }
            return result;
        }
    }
}