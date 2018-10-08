// *************************************************************************************
// SCICHART © Copyright ABT Software Services Ltd. 2011-2012. All rights reserved.
//  
// Web: http://www.scichart.com
// Support: info@abtsoftware.co.uk
//  
// Enums.cs is part of SciChart Examples, a High Performance WPF & Silverlight Chart. 
// For full terms and conditions of the SciChart license, see http://www.scichart.com/scichart-eula/
//  
// SciChart Examples source code is provided free of charge on an "As-Is" basis to support
// and provide examples of how to use the SciChart component. You bear the risk of using it. 
// The authors give no express warranties, guarantees or conditions. You may have additional 
// consumer rights under your local laws which this license cannot change. To the extent 
// permitted under your local laws, the contributors exclude the implied warranties of 
// merchantability, fitness for a particular purpose and non-infringement. 
// *************************************************************************************
namespace Abt.Controls.SciChart.Example.Common
{
    public enum ModifierType
    {
        CrosshairsCursor, 
        RubberBandZoom,
        Null,
        ZoomPan,
        Rollover
    }

    public enum ChartType
    {
        FastLine,
        FastMountain,
        FastColumn,
        FastOhlc,
        FastCandlestick,        
    }
}