﻿#pragma checksum "..\..\..\Controller\MainWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "9693F777D8ECB703B4E1737A79CDD91A4401975BDB6D5FE825B708BFFB562D98"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace BangDreamMusicscoreConverter {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 20 "..\..\..\Controller\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button OpenButton;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\Controller\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SourceTextBox;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\Controller\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ResultTextBox;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\Controller\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ConvertTypeFromSelector;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\Controller\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ConvertTypeToSelector;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\..\Controller\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ConvertButton;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\..\Controller\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SaveButton;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\..\Controller\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CopyButton;
        
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
            System.Uri resourceLocater = new System.Uri("/BangDreamMusicscoreConverter;component/controller/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Controller\MainWindow.xaml"
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
            this.OpenButton = ((System.Windows.Controls.Button)(target));
            
            #line 20 "..\..\..\Controller\MainWindow.xaml"
            this.OpenButton.Click += new System.Windows.RoutedEventHandler(this.OpenButton_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.SourceTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 22 "..\..\..\Controller\MainWindow.xaml"
            this.SourceTextBox.PreviewDragOver += new System.Windows.DragEventHandler(this.SourceTextBox_PreviewDragOver);
            
            #line default
            #line hidden
            
            #line 23 "..\..\..\Controller\MainWindow.xaml"
            this.SourceTextBox.PreviewDrop += new System.Windows.DragEventHandler(this.SourceTextBox_PreviewDrop);
            
            #line default
            #line hidden
            return;
            case 3:
            this.ResultTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.ConvertTypeFromSelector = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 5:
            this.ConvertTypeToSelector = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 6:
            this.ConvertButton = ((System.Windows.Controls.Button)(target));
            
            #line 56 "..\..\..\Controller\MainWindow.xaml"
            this.ConvertButton.Click += new System.Windows.RoutedEventHandler(this.ConvertButton_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.SaveButton = ((System.Windows.Controls.Button)(target));
            
            #line 66 "..\..\..\Controller\MainWindow.xaml"
            this.SaveButton.Click += new System.Windows.RoutedEventHandler(this.SaveButton_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.CopyButton = ((System.Windows.Controls.Button)(target));
            
            #line 67 "..\..\..\Controller\MainWindow.xaml"
            this.CopyButton.Click += new System.Windows.RoutedEventHandler(this.CopyButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
