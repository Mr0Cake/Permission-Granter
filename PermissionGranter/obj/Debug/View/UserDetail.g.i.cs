﻿#pragma checksum "..\..\..\View\UserDetail.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "BD93C4D094A875FC3E37C9B45AFB00D7"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using PermissionGranter.View;
using PermissionGranter.ViewModel;
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
using System.Windows.Interactivity;
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


namespace PermissionGranter.View {
    
    
    /// <summary>
    /// UserDetail
    /// </summary>
    public partial class UserDetail : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 22 "..\..\..\View\UserDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel spUserInfo;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\View\UserDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtVoornaam;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\View\UserDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtAchternaam;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\View\UserDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtWachtwoord;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\View\UserDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtEmail;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\View\UserDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox lstGroups;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\View\UserDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox lstAllGroups;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\View\UserDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAddGroup;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\View\UserDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnRemoveGroup;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\View\UserDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox lstAllUsers;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\..\View\UserDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSave;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\View\UserDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnNew;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\..\View\UserDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnDelete;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\..\View\UserDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCancel;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\..\View\UserDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel spCopy;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\..\View\UserDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer Permissions;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\..\View\UserDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeView trvUser;
        
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
            System.Uri resourceLocater = new System.Uri("/PermissionGranter;component/view/userdetail.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\View\UserDetail.xaml"
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
            this.spUserInfo = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 2:
            this.txtVoornaam = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.txtAchternaam = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.txtWachtwoord = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.txtEmail = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.lstGroups = ((System.Windows.Controls.ListBox)(target));
            return;
            case 7:
            this.lstAllGroups = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 8:
            this.btnAddGroup = ((System.Windows.Controls.Button)(target));
            return;
            case 9:
            this.btnRemoveGroup = ((System.Windows.Controls.Button)(target));
            return;
            case 10:
            this.lstAllUsers = ((System.Windows.Controls.ListBox)(target));
            return;
            case 11:
            this.btnSave = ((System.Windows.Controls.Button)(target));
            return;
            case 12:
            this.btnNew = ((System.Windows.Controls.Button)(target));
            return;
            case 13:
            this.btnDelete = ((System.Windows.Controls.Button)(target));
            return;
            case 14:
            this.btnCancel = ((System.Windows.Controls.Button)(target));
            return;
            case 15:
            this.spCopy = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 16:
            this.Permissions = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            case 17:
            this.trvUser = ((System.Windows.Controls.TreeView)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

