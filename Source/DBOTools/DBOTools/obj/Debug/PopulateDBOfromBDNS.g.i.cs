﻿#pragma checksum "..\..\PopulateDBOfromBDNS.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "DFF6BA530C0FE7D5F8F681E345C2A1875DACA4FADB57821F538D1DDEA5C91C54"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using DBOTools;
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


namespace DBOTools {
    
    
    /// <summary>
    /// PopulateDBOfromBDNS
    /// </summary>
    public partial class PopulateDBOfromBDNS : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 27 "..\..\PopulateDBOfromBDNS.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox tryToPopulateCheckBox;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\PopulateDBOfromBDNS.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox typeParameters;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\PopulateDBOfromBDNS.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton tryAll;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\PopulateDBOfromBDNS.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton trySelected;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\PopulateDBOfromBDNS.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button tryPopulateFromParameterButton;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\PopulateDBOfromBDNS.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dboFamiliesDataGrid;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\PopulateDBOfromBDNS.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTemplateColumn ComboBoxColumn;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\PopulateDBOfromBDNS.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button cancelButton;
        
        #line default
        #line hidden
        
        
        #line 106 "..\..\PopulateDBOfromBDNS.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button okButton;
        
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
            System.Uri resourceLocater = new System.Uri("/DBOTools;component/populatedbofrombdns.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\PopulateDBOfromBDNS.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
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
            this.tryToPopulateCheckBox = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 2:
            this.typeParameters = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 3:
            this.tryAll = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 4:
            this.trySelected = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 5:
            this.tryPopulateFromParameterButton = ((System.Windows.Controls.Button)(target));
            
            #line 40 "..\..\PopulateDBOfromBDNS.xaml"
            this.tryPopulateFromParameterButton.Click += new System.Windows.RoutedEventHandler(this.tryPopulateFromParameterButton_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.dboFamiliesDataGrid = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 7:
            this.ComboBoxColumn = ((System.Windows.Controls.DataGridTemplateColumn)(target));
            return;
            case 8:
            this.cancelButton = ((System.Windows.Controls.Button)(target));
            
            #line 103 "..\..\PopulateDBOfromBDNS.xaml"
            this.cancelButton.Click += new System.Windows.RoutedEventHandler(this.cancelButton_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.okButton = ((System.Windows.Controls.Button)(target));
            
            #line 108 "..\..\PopulateDBOfromBDNS.xaml"
            this.okButton.Click += new System.Windows.RoutedEventHandler(this.okButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

