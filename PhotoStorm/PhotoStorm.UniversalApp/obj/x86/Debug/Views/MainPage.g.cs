﻿#pragma checksum "D:\Projects\Git\bubble.shot\Bubbleshot.Server\BubbleShot.UniversalApp\Views\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "97ABE3608F7FA43F9B446605A07B8CB3"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BubbleShot.UniversalApp.Views
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    global::Windows.UI.Xaml.Controls.Page element1 = (global::Windows.UI.Xaml.Controls.Page)(target);
                    #line 15 "..\..\..\Views\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Page)element1).SizeChanged += this.MainPage_OnSizeChanged;
                    #line default
                }
                break;
            case 2:
                {
                    this.BottomToolBar = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 3:
                {
                    this.MapToggleButton = (global::Windows.UI.Xaml.Controls.AppBarToggleButton)(target);
                }
                break;
            case 4:
                {
                    this.GalleryToggleButton = (global::Windows.UI.Xaml.Controls.AppBarToggleButton)(target);
                }
                break;
            case 5:
                {
                    this.GridView = (global::Windows.UI.Xaml.Controls.GridView)(target);
                }
                break;
            case 6:
                {
                    global::Windows.UI.Xaml.Controls.Image element6 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 195 "..\..\..\Views\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)element6).ManipulationStarted += this.OnManipulationStarted;
                    #line 195 "..\..\..\Views\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)element6).ManipulationCompleted += this.OnManipulationCompleted;
                    #line default
                }
                break;
            case 7:
                {
                    global::Windows.UI.Xaml.Controls.WrapGrid element7 = (global::Windows.UI.Xaml.Controls.WrapGrid)(target);
                    #line 181 "..\..\..\Views\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.WrapGrid)element7).SizeChanged += this.WrapGridSizeChanged;
                    #line default
                }
                break;
            case 8:
                {
                    this.Map = (global::Windows.UI.Xaml.Controls.Maps.MapControl)(target);
                    #line 29 "..\..\..\Views\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Maps.MapControl)this.Map).CenterChanged += this.Map_OnCenterChanged;
                    #line 29 "..\..\..\Views\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Maps.MapControl)this.Map).MapDoubleTapped += this.Map_OnMapDoubleTapped;
                    #line default
                }
                break;
            case 9:
                {
                    this.TopToolBar = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 10:
                {
                    global::Windows.UI.Xaml.Controls.AutoSuggestBox element10 = (global::Windows.UI.Xaml.Controls.AutoSuggestBox)(target);
                    #line 78 "..\..\..\Views\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.AutoSuggestBox)element10).TextChanged += this.AutoSuggestBox_OnTextChanged;
                    #line 79 "..\..\..\Views\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.AutoSuggestBox)element10).QuerySubmitted += this.AutoSuggestBox_OnQuerySubmitted;
                    #line 81 "..\..\..\Views\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.AutoSuggestBox)element10).SuggestionChosen += this.AutoSuggestBox_OnSuggestionChosen;
                    #line default
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}
