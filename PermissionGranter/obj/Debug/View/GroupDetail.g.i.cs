﻿#pragma checksum "..\..\..\View\GroupDetail.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9F7E9712EBA30791457806BB9D741A13"
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
    /// GroupDetail
    /// </summary>
    public partial class GroupDetail : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 22 "..\..\..\View\GroupDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel spUserInfo;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\View\GroupDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtGroepnaam;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\View\GroupDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtOmschrijving;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\View\GroupDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox lstGroups;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\View\GroupDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAddGroup;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\View\GroupDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnRemoveGroup;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\..\View\GroupDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox lstUsers;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\View\GroupDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbUsers;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\View\GroupDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAddUserToGroup;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\View\GroupDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnRemoveUserFromGroup;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\..\View\GroupDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSave;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\..\View\GroupDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCancel;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\..\View\GroupDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel spCopy;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\..\View\GroupDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer Permissions;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\..\View\GroupDetail.xaml"
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
            System.Uri resourceLocater = new System.Uri("/PermissionGranter;component/view/groupdetail.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\View\GroupDetail.xaml"
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
            this.txtGroepnaam = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.txtOmschrijving = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.lstGroups = ((System.Windows.Controls.ListBox)(target));
            return;
            case 5:
            this.btnAddGroup = ((System.Windows.Controls.Button)(target));
            return;
            case 6:
            this.btnRemoveGroup = ((System.Windows.Controls.Button)(target));
            return;
            case 7:
            this.lstUsers = ((System.Windows.Controls.ListBox)(target));
            return;
            case 8:
            this.cmbUsers = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 9:
            this.btnAddUserToGroup = ((System.Windows.Controls.Button)(target));
            return;
            case 10:
            this.btnRemoveUserFromGroup = ((System.Windows.Controls.Button)(target));
            return;
            case 11:
            this.btnSave = ((System.Windows.Controls.Button)(target));
            return;
            case 12:
            this.btnCancel = ((System.Windows.Controls.Button)(target));
            return;
            case 13:
            this.spCopy = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 14:
            this.Permissions = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            case 15:
            this.trvUser = ((System.Windows.Controls.TreeView)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

