﻿/* 
 * Author:
 *   Jose Medrano <josmed@microsoft.com>
 *
 * Copyright (C) 2018 Microsoft, Corp
 *
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the
 * "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to permit
 * persons to whom the Software is furnished to do so, subject to the
 * following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
 * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN
 * NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
 * OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE
 * USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppKit;
using CoreGraphics;
using FigmaSharp;
using FigmaSharp.Cocoa;
using FigmaSharp.Models;
using FigmaSharp.Services;
using LiteForms;
using LiteForms.Cocoa;
using LocalFile.Shared;

namespace LocalFile.Cocoa
{
	static class MainClass
    {
		static IWindow mainWindow;
		static FigmaRemoteFileProvider fileProvider;

		static void Main(string[] args)
        {
			FigmaApplication.Init(Environment.GetEnvironmentVariable("TOKEN"));

            NSApplication.Init();
            NSApplication.SharedApplication.ActivationPolicy = NSApplicationActivationPolicy.Regular;

			mainWindow = new Window(new Rectangle(0, 0, 300, 368)) {
				Title = "Cocoa Figma Local File Sample",
			};

			//Example1();
			Example2();

			mainWindow.Show ();

            NSApplication.SharedApplication.ActivateIgnoringOtherApps(true);
            NSApplication.SharedApplication.Run();
        }

		static void SetContentDialog(IView screen)
		{
			mainWindow.Content = screen;

			//we want move our views clickin on it
			mainWindow.MovableByWindowBackground = mainWindow.Content.MovableByWindowBackground = true;
			foreach (var view in mainWindow.Content.Children)
			{
				view.MovableByWindowBackground = true;
			}
		}

		#region Example1

		static void Example1()
		{
			const string fileName = "EGTUYgwUC9rpHmm4kJwZQXq4";

			var converters = FigmaSharp.NativeControls.Cocoa.Resources.GetConverters()
			.Union(FigmaSharp.AppContext.Current.GetFigmaConverters())
			.ToArray();

			fileProvider = new FigmaRemoteFileProvider();
			var rendererService = new FigmaFileRendererService(fileProvider, converters);

			var options = new FigmaViewRendererServiceOptions() { ScanChildrenFromFigmaInstances = false };

			var scrollView = new ScrollView();
			mainWindow.Content = scrollView;

			rendererService.Start(fileName, scrollView.ContentView, options);

			var distributionService = new FigmaViewRendererDistributionService(rendererService);
			distributionService.Start();

			var urlTextField = rendererService.FindViewByName<TextBox>("FigmaUrlTextField");
			var bundleButton = rendererService.FindViewByName<Button>("BundleButton");
			var cancelButton = rendererService.FindViewByName<Button>("CancelButton");

			if (cancelButton != null)
			{
				cancelButton.Clicked += (s, e) => {
					if (urlTextField != null)
						urlTextField.Text = "You pressed cancel";
				};
			}

			if (bundleButton != null)
			{
				bundleButton.Clicked += (s, e) => {
					if (urlTextField != null)
						urlTextField.Text = "You pressed bundle";
				};
			}

			//We want know the background color of the figma camvas and apply to our scrollview
			var canvas = fileProvider.Nodes.OfType<FigmaCanvas>().FirstOrDefault();
			if (canvas != null)
				scrollView.BackgroundColor = canvas.backgroundColor;

			scrollView.AdjustToContent();
		}

		#endregion

		#region Example2

		static async void Example2()
		{
			var document = "b05oGrcu9WI9txiHqdhCUyE4";

			mainWindow.Size = new Size(720, 467);
			mainWindow.Center();

			//windows properties
			mainWindow.Content.BackgroundColor = Color.Transparent;
			mainWindow.Borderless = true;
			mainWindow.Resizable = false;
			mainWindow.IsOpaque = false;

			mainWindow.ShowTitle = false;
			mainWindow.BackgroundColor = Color.Transparent;

			//we add the control converters
			var converters = FigmaSharp.NativeControls.Cocoa.Resources.GetConverters()
			.Union(FigmaSharp.AppContext.Current.GetFigmaConverters())
			.ToArray();

			fileProvider = new FigmaRemoteFileProvider();
			fileProvider.Load(document);

			var rendererService = new FigmaViewRendererService(fileProvider, converters);

			var loadingSpinnerConverter = new LoadingSpinnerConverter();
			rendererService.CustomConverters.Add(loadingSpinnerConverter);
			rendererService.CustomConverters.Add(new CloseButtonConverter());

			var options = new FigmaViewRendererServiceOptions { ScanChildrenFromFigmaInstances = false };

			var loadingDialog = rendererService.RenderByName<IView>("LoadingDialog", options);
			loadingDialog.BackgroundColor = new Color(0.27f, 0.15f, 0.41f);
			loadingDialog.CornerRadius = 5;
			loadingDialog.Size = new Size(36, 36);
			SetContentDialog(loadingDialog);

			rendererService.CustomConverters.Remove(loadingSpinnerConverter);

			//finds view and process animation
			var spinnerView = rendererService.FindViewByName<ISpinner>(LoadingSpinnerConverter.LoadingSpinnerName);
			spinnerView.Start();

			//we wait for 5 seconds and we show next screen
			await Task.Run(() => {
				Thread.Sleep(5000);
				FigmaSharp.AppContext.Current.BeginInvoke(() => LoadSignInDialog(rendererService, options));
			});
		}

		static void LoadingRecentItemsDialog(FigmaViewRendererService rendererService, FigmaViewRendererServiceOptions options)
		{
			var customConverters = new FigmaViewConverter[] {
				new SearchFilterConverter(),
			};
			rendererService.CustomConverters.AddRange(customConverters);

			//new SearchFilterConverter ()
			var dialog = rendererService.RenderByName<IView>("RecentItemsDialog", options);
			dialog.CornerRadius = 5;
			dialog.BackgroundColor = Color.White;
			SetContentDialog(dialog);

			foreach (var viewConverter in customConverters)
				rendererService.CustomConverters.Remove(viewConverter);

			var closeButton = dialog.Children
				.OfType<ImageButton>()
				.FirstOrDefault();

			closeButton.Clicked += (s, e) => {
				NSRunningApplication.CurrentApplication.Terminate();
			};
		}

		static void LoadSignInDialog (FigmaViewRendererService rendererService, FigmaViewRendererServiceOptions options)
		{
			var customConverters = new FigmaViewConverter[] {
				new DoThisLaterButtonConverter(),
				new SignInMicrosoftButtonConverter(),
			};
			rendererService.CustomConverters.AddRange(customConverters);

			var signInDialog = rendererService.RenderByName<IView>("SignInDialog", options);
			signInDialog.CornerRadius = 5;
			signInDialog.BackgroundColor = new Color(0.27f, 0.15f, 0.41f);
			SetContentDialog(signInDialog);

			foreach (var viewConverter in customConverters)
				rendererService.CustomConverters.Remove(viewConverter);

			var signInButton = rendererService.FindViewByName<IButton>(SignInMicrosoftButtonConverter.SignInMicrosoftButtonName);
			signInButton.Focus();

			signInButton.Clicked += (s, e) => {
				LoadingRecentItemsDialog(rendererService, options);
			};

			var doThisLaterButton = rendererService.FindViewByName<IButton>(DoThisLaterButtonConverter.DoThisLaterButtonName);
			doThisLaterButton.Clicked += (s, e) => {
				LoadingRecentItemsDialog(rendererService, options);
			};
		}

		#endregion
	}
}

