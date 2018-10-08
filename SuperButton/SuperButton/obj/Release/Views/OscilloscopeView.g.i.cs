﻿#pragma checksum "..\..\..\Views\OscilloscopeView.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "E50A69650B0A9F5C47E21D89D27BE1B6E428788F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Abt.Controls.SciChart;
using Abt.Controls.SciChart.ChartModifiers;
using Abt.Controls.SciChart.Common;
using Abt.Controls.SciChart.Common.Extensions;
using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.Rendering.Common;
using Abt.Controls.SciChart.Rendering.HighQualityRasterizer;
using Abt.Controls.SciChart.Rendering.HighSpeedRasterizer;
using Abt.Controls.SciChart.Utility.Mouse;
using Abt.Controls.SciChart.Visuals;
using Abt.Controls.SciChart.Visuals.Annotations;
using Abt.Controls.SciChart.Visuals.Axes;
using Abt.Controls.SciChart.Visuals.PointMarkers;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using Abt.Controls.SciChart3D;
using Abt.Controls.SciChart3D.Context.D3D10;
using Abt.Controls.SciChart3D.Modifiers;
using Abt.Controls.SciChart3D.Visuals;
using Abt.Controls.SciChart3D.Visuals.Axis;
using Abt.Controls.SciChart3D.Visuals.RenderableSeries;
using SuperButton.Common;
using SuperButton.Helpers;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace SuperButton.Views {
    
    
    /// <summary>
    /// OscilloscopeView
    /// </summary>
    public partial class OscilloscopeView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 265 "..\..\..\Views\OscilloscopeView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Abt.Controls.SciChart.Visuals.SciChartSurface OscilloscopeChart;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Driver GUI;component/views/oscilloscopeview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\OscilloscopeView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.OscilloscopeChart = ((Abt.Controls.SciChart.Visuals.SciChartSurface)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

