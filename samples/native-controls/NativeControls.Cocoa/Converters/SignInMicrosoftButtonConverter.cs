/* 
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

using AppKit;
using CoreGraphics;
using FigmaSharp;
using FigmaSharp.Models;
using LiteForms;
using LiteForms.Cocoa;
using System.Linq;
using CoreAnimation;
using FigmaSharp.Services;

namespace LocalFile.Cocoa
{
	class SignInMicrosoftButtonConverter : FigmaViewConverter
	{
		public const string SignInMicrosoftButtonName = "SignInMicrosoftButton";

		public override bool ScanChildren(FigmaNode currentNode) => false;

		public override bool CanConvert(FigmaNode currentNode)
		{
			return currentNode.name == SignInMicrosoftButtonName;
		}

		const string LogoImageName = "MSLogoImage";

		public override IView ConvertTo(FigmaNode currentNode, ProcessedNode parent, FigmaRendererService rendererService)
		{
			string text = string.Empty;
			if (currentNode is IFigmaNodeContainer container)
			{
				var figmaText = container.children.OfType<FigmaText>().FirstOrDefault();
				if (figmaText != null)
					text = figmaText.characters;
			}

			IView msLogoView = null;

			if (rendererService is FigmaViewRendererService viewRendererService)
				msLogoView = viewRendererService.RenderByName<IView>(LogoImageName);
		
			var flatButton =  new FixedFlatButton(text, msLogoView.NativeObject as NSView);
			var button = new Button(flatButton) { IsDark = true };
			return button;
		}

		public override string ConvertToCode(FigmaNode currentNode, FigmaCodeRendererService rendererService)
		{
			return string.Empty;
		}

		#region Logo

		const int MicrosoftLogoSize = 23;

		NSView GetMicrosoftLogoView()
		{
			var container = new NSView
			{
				WantsLayer = true
			};
			container.SetFrameSize(new CGSize(MicrosoftLogoSize, MicrosoftLogoSize));

			var view138 = new CALayer
			{
				Frame = new CGRect(0f, 12f, 11f, 11f),
				BackgroundColor = NSColor.FromRgba(0.9372549f, 0.3176471f, 0.1803922f, 1f).CGColor
			};
			container.Layer.AddSublayer(view138);

			var view139 = new CALayer
			{
				Frame = new CGRect(12, 12, 11f, 11f),
				BackgroundColor = NSColor.FromRgba(0.5019608f, 0.7215686f, 0.1333333f, 1f).CGColor
			};
			container.Layer.AddSublayer(view139);

			var view140 = new CALayer
			{
				Frame = new CGRect(0f, 0, 11f, 11f),
				BackgroundColor = NSColor.FromRgba(0.1058824f, 0.6470588f, 0.9254902f, 1f).CGColor
			};
			container.Layer.AddSublayer(view140);

			var view141 = new CALayer
			{
				Frame = new CGRect(12, 0, 11f, 11f),
				BackgroundColor = NSColor.FromRgba(0.9921569f, 0.7215686f, 0.172549f, 1f).CGColor
			};
			container.Layer.AddSublayer(view141);

			return container;
		}
		#endregion

	}
}

