// *************************************************************************************
// SCICHART © Copyright ABT Software Services Ltd. 2011-2012. All rights reserved.
//  
// Web: http://www.scichart.com
// Support: info@abtsoftware.co.uk
//  
// Example.cs is part of SciChart Examples, a High Performance WPF & Silverlight Chart. 
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
using System.Collections.Generic;
using System.Windows.Input;
using SuperButton.StatusGroup;

namespace SuperButton
{
    public sealed class Example//: ISelectable
    {
      //  private List<string> _sourceFiles = new List<string>();
        public string ExampleHelpText { get; set; }
        public string ExampleDisplayName { get; set; }
        public string ExampleToolTipDescription { get; set; }
        public string ExampleDescription { get; set; }
        public StatusGroups Group { get; set; }

        private bool _IsStatus;





        private Dictionary<string, string> _sourceFiles1 = new Dictionary<string, string>();
        private bool _isLoaded;

        private string _text = String.Empty;

      

       
      

     //   public List<Features> Features { get; set; }

      //  public Edition Edition { get; set; }

      //  public ChartTypes UsedChartTypes { get; set; }
      //  public ChartGroups Group { get; set; }

        public string ExampleImagePath { get; set; }
        public string IconPath { get; set; }

        public ICommand SelectCommand { get; set; }

        public Guid PageId { get; private set; }

        public bool IsNew
        {
            get { return false; }
        }

        public bool IsPremium
        {
            get { return false; }
        }

        //public Dictionary<string, string> Code
        //{
        //    get
        //    {
        //        if(!_isLoaded)
        //        {
        //            LoadCode();
        //            _isLoaded = true;
        //        }

        //        return _sourceFiles1;
        //    }
        //}

        //private void LoadCode()
        //{
        //    _sourceFiles.ForEach(file =>
        //                             {
        //                                 var index = file.LastIndexOf('/') + 1;
        //                                 var fileName = file.Substring(index)
        //                                     .Replace(".txt", String.Empty);

        //                                 _sourceFiles1[fileName] = ExampleLoader.LoadSourceFile(file); ;
        //                             });
        //}

        //public string SourceFiles
        //{
        //    get { return String.IsNullOrWhiteSpace(_text) ? _text = LoadSourceFiles() : _text; }
        //}

        //private string LoadSourceFiles()
        //{
        //    const string separator = "****************************************************************************************************";

        //    var str = String.Empty;
        //    foreach (var name in _sourceFiles)
        //    {
        //        var index = name.LastIndexOf('/') + 1;
        //        var fileName = name.Substring(index)
        //            .Replace(".txt", String.Empty);

        //        str += separator;
        //        str += Environment.NewLine;
        //        str += fileName;
        //        str += Environment.NewLine;
        //        str += separator;
        //        str += Environment.NewLine;

        //        str += ExampleLoader.LoadSourceFile(name);
        //    }

        //    return str;
        //}
    }
}
