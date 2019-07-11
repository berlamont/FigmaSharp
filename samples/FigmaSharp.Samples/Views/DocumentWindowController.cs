﻿// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Collections.Generic;
using System.Threading;

using AppKit;
using CoreGraphics;
using Foundation;

namespace FigmaSharp.Samples
{
    public partial class DocumentWindowController : NSWindowController
    {
        public static int WindowCount { get; private set; }
        const int NEW_WINDOW_OFFSET = 38;


        public string Token = "";

        public string Link_ID = "";
        public string Version_ID = "Current";
        public string Page_ID = "Page 1";


        // FileProvider etc.


        public DocumentWindowController(IntPtr handle) : base(handle)
        {
        }


        public string Title {
            get { return TitleTextField.StringValue; }
            set { TitleTextField.StringValue = value; }
        }


        public void LoadDocument(string token, string link_id)
        {
            Token = token;
            Link_ID = link_id;

            Load(version_id: "", page_id: "");
        }


        public override void WindowDidLoad()
        {
            PositionWindow();
            base.WindowDidLoad();
        }


        void Load(string version_id, string page_id)
        {
            Title = string.Format("Opening “{0}”…", Link_ID);

            (Window.ContentViewController as DocumentViewController).ToggleSpinnerState(toggle_on: true);
            RefreshButton.Enabled = false;

            new Thread(() => {
                Thread.Sleep(2000);

                this.InvokeOnMainThread(() => {
                    Title = Link_ID;

                    UpdateVersionMenu();
                    UpdatePagesPopupButton();

                    RefreshButton.Enabled = true;
                    PagePopUpButton.Enabled = true;

                    (Window.ContentViewController as DocumentViewController).ToggleSpinnerState(toggle_on: false);

                });

            }).Start();
        }


        public void Reload()
        {
            Load(Link_ID, Token);
        }


        void UpdatePagesPopupButton()
        {
            PagePopUpButton.AddItem("1");
            PagePopUpButton.Activated += delegate {
                Console.WriteLine(PagePopUpButton.SelectedItem.Title);
            };
        }


        void UpdateVersionMenu()
        {
            var menu = new VersionMenu();

            menu.VersionSelected += delegate (string version_id) {
                Load(version_id, null);
            };

            menu.AddItem("1", "FigmaSharp.Cocoa 0.0.1", DateTime.Now);
            menu.AddItem("2", "FigmaSharp.Cocoa 0.0.2", DateTime.Now);
            menu.AddItem("3", "FigmaSharp.Cocoa 0.0.3", DateTime.Now);
            menu.AddItem("4", DateTime.Now);
            menu.AddItem("5", DateTime.Now.AddDays(-7));
            menu.AddItem("6", DateTime.Now.AddDays(-14));

            menu.UseAsVersionsMenu();
        }


        partial void RefreshClicked(NSObject sender)
        {
            ToggleSpinnerState(toggle_on: true);
            RefreshButton.Enabled = false;

            new Thread(() => {
                Thread.Sleep(2000);

                this.InvokeOnMainThread(() => {
                    RefreshButton.Enabled = true;
                    ToggleSpinnerState(toggle_on: false);
                });

            }).Start();

            // Reload();
        }


        void ToggleSpinnerState(bool toggle_on)
        {
            if (MainToolbar.VisibleItems[1].Identifier == "Spinner") {
                (MainToolbar.VisibleItems[1].View as NSProgressIndicator).StopAnimation(this);
                MainToolbar.RemoveItem(1);
            }

            if (toggle_on) {
                MainToolbar.InsertItem("Spinner", 1);
                (MainToolbar.VisibleItems[1].View as NSProgressIndicator).StartAnimation(this);
            }
        }


        void PositionWindow()
        {
            WindowCount++;
            CGRect frame = Window.Frame;

            frame.X += NEW_WINDOW_OFFSET * WindowCount;
            frame.Y -= NEW_WINDOW_OFFSET * WindowCount;

            Window.SetFrame(frame, display: true);
        }


        void ShowError()
        {
            /*
            var alert = new NSAlert()
            {
                AlertStyle = NSAlertStyle.Warning,
                MessageText = "Could not open “DhOTs1gwx837ysnG3X6RZqZm”",
                InformativeText = "Please check if your Figma Link and Personal Access Token are correct",
            };

            alert.AddButton("Close");
            alert.BeginSheetForResponse(alert.Window, null);
            */
        }
    }
}
