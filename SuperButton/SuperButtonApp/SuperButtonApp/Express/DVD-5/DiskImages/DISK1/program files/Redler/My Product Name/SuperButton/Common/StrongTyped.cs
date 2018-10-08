// *************************************************************************************
// SCICHART © Copyright ABT Software Services Ltd. 2011-2012. All rights reserved.
//  
// Web: http://www.scichart.com
// Support: info@abtsoftware.co.uk
//  
// StrongTyped.cs is part of SciChart Examples, a High Performance WPF & Silverlight Chart. 
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

namespace Abt.Controls.SciChart.Example.Common
{
    public class StrongTyped<T> : IEquatable<StrongTyped<T>>
    {
        public T Value { get; protected set; }

        public StrongTyped(T value)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(StrongTyped<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Value, Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(StrongTyped<T> left, StrongTyped<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(StrongTyped<T> left, StrongTyped<T> right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}