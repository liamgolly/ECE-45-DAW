﻿#pragma checksum "..\..\..\..\..\Controls\Envelopes\Pitch.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4DC21E078787C018D53FE0D8A7D2B9A3D5C88860"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Synth.Controls.Envelopes;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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
using WPFUI.Controls;


namespace Synth.Controls.Envelopes {
    
    
    /// <summary>
    /// Pitch
    /// </summary>
    public partial class Pitch : Synth.Controls.Envelopes.Envelope, System.Windows.Markup.IComponentConnector {
        
        
        #line 25 "..\..\..\..\..\Controls\Envelopes\Pitch.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas PathContainer;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\..\..\Controls\Envelopes\Pitch.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Path GraphControl;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\..\..\Controls\Envelopes\Pitch.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider TimeSlider;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\..\..\Controls\Envelopes\Pitch.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TimeText;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\..\..\Controls\Envelopes\Pitch.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider ValueSlider;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\..\..\Controls\Envelopes\Pitch.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ValueText;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.2.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Synth;component/controls/envelopes/pitch.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Controls\Envelopes\Pitch.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.2.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 23 "..\..\..\..\..\Controls\Envelopes\Pitch.xaml"
            ((WPFUI.Controls.Card)(target)).Loaded += new System.Windows.RoutedEventHandler(this.GraphLoaded);
            
            #line default
            #line hidden
            
            #line 24 "..\..\..\..\..\Controls\Envelopes\Pitch.xaml"
            ((WPFUI.Controls.Card)(target)).SizeChanged += new System.Windows.SizeChangedEventHandler(this.GraphLoaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.PathContainer = ((System.Windows.Controls.Canvas)(target));
            return;
            case 3:
            this.GraphControl = ((System.Windows.Shapes.Path)(target));
            return;
            case 4:
            this.TimeSlider = ((System.Windows.Controls.Slider)(target));
            return;
            case 5:
            this.TimeText = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.ValueSlider = ((System.Windows.Controls.Slider)(target));
            return;
            case 7:
            this.ValueText = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            
            #line 61 "..\..\..\..\..\Controls\Envelopes\Pitch.xaml"
            ((System.Windows.Controls.ComboBox)(target)).SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.NewSemitone);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 79 "..\..\..\..\..\Controls\Envelopes\Pitch.xaml"
            ((WPFUI.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.AddNode);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 80 "..\..\..\..\..\Controls\Envelopes\Pitch.xaml"
            ((WPFUI.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.RemoveNode);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

