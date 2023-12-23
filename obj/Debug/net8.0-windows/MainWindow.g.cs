﻿#pragma checksum "..\..\..\MainWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "DD05BF548E28341A564845B4D012B0B8C9F18C7B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using KanbanApp;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
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


namespace KanbanApp {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 22 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CreateTaskButton;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ShuffleTaskButton;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView Backlog;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView Doing;
        
        #line default
        #line hidden
        
        
        #line 93 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView Review;
        
        #line default
        #line hidden
        
        
        #line 124 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView Done;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WpfAppToDoTango;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 20 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.DockPanel)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.Backlog_MouseEnter);
            
            #line default
            #line hidden
            
            #line 20 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.DockPanel)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.Backlog_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 2:
            this.CreateTaskButton = ((System.Windows.Controls.Button)(target));
            
            #line 22 "..\..\..\MainWindow.xaml"
            this.CreateTaskButton.Click += new System.Windows.RoutedEventHandler(this.CreateTaskButton_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.ShuffleTaskButton = ((System.Windows.Controls.Button)(target));
            
            #line 23 "..\..\..\MainWindow.xaml"
            this.ShuffleTaskButton.Click += new System.Windows.RoutedEventHandler(this.ShuffleButton_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Backlog = ((System.Windows.Controls.ListView)(target));
            
            #line 24 "..\..\..\MainWindow.xaml"
            this.Backlog.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.ListView_PreviewMouseLeftButtonDown);
            
            #line default
            #line hidden
            
            #line 24 "..\..\..\MainWindow.xaml"
            this.Backlog.Drop += new System.Windows.DragEventHandler(this.ListView_Drop);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 32 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.QuickAddTaskMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.Doing = ((System.Windows.Controls.ListView)(target));
            
            #line 61 "..\..\..\MainWindow.xaml"
            this.Doing.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.ListView_PreviewMouseLeftButtonDown);
            
            #line default
            #line hidden
            
            #line 61 "..\..\..\MainWindow.xaml"
            this.Doing.Drop += new System.Windows.DragEventHandler(this.ListView_Drop);
            
            #line default
            #line hidden
            return;
            case 14:
            this.Review = ((System.Windows.Controls.ListView)(target));
            
            #line 93 "..\..\..\MainWindow.xaml"
            this.Review.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.ListView_PreviewMouseLeftButtonDown);
            
            #line default
            #line hidden
            
            #line 93 "..\..\..\MainWindow.xaml"
            this.Review.Drop += new System.Windows.DragEventHandler(this.ListView_Drop);
            
            #line default
            #line hidden
            return;
            case 18:
            this.Done = ((System.Windows.Controls.ListView)(target));
            
            #line 124 "..\..\..\MainWindow.xaml"
            this.Done.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.ListView_PreviewMouseLeftButtonDown);
            
            #line default
            #line hidden
            
            #line 124 "..\..\..\MainWindow.xaml"
            this.Done.Drop += new System.Windows.DragEventHandler(this.ListView_Drop);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 6:
            
            #line 47 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MoveToDoingMenuItem_Click);
            
            #line default
            #line hidden
            break;
            case 7:
            
            #line 48 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MoveToReviewMenuItem_Click);
            
            #line default
            #line hidden
            break;
            case 8:
            
            #line 49 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MoveToDoneMenuItem_Click);
            
            #line default
            #line hidden
            break;
            case 9:
            
            #line 50 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.EditMenuItem_Click);
            
            #line default
            #line hidden
            break;
            case 11:
            
            #line 80 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MoveToBacklogMenuItem_Click);
            
            #line default
            #line hidden
            break;
            case 12:
            
            #line 81 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MoveToReviewMenuItem_Click);
            
            #line default
            #line hidden
            break;
            case 13:
            
            #line 82 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MoveToDoneMenuItem_Click);
            
            #line default
            #line hidden
            break;
            case 15:
            
            #line 111 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MoveToBacklogMenuItem_Click);
            
            #line default
            #line hidden
            break;
            case 16:
            
            #line 112 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MoveToDoingMenuItem_Click);
            
            #line default
            #line hidden
            break;
            case 17:
            
            #line 113 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MoveToDoneMenuItem_Click);
            
            #line default
            #line hidden
            break;
            case 19:
            
            #line 139 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.DeleteButton_Click);
            
            #line default
            #line hidden
            break;
            case 20:
            
            #line 143 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MoveToBacklogMenuItem_Click);
            
            #line default
            #line hidden
            break;
            case 21:
            
            #line 144 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MoveToDoingMenuItem_Click);
            
            #line default
            #line hidden
            break;
            case 22:
            
            #line 145 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MoveToReviewMenuItem_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

