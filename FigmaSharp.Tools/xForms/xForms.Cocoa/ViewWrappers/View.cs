﻿/* 
 * FigmaImageView.cs - NSImageView which stores it's associed Figma Id
 * 
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

using System.Collections.Generic;
using AppKit;

using LiteForms;

namespace LiteForms.Cocoa
{
    public class View : IView
    {
        public object NativeObject => nativeView;

        public IView Parent
        {
            get
            {
                if (nativeView.Superview != null)
                {
                    return new View(nativeView.Superview);
                }
                return null;
            }
        }

        readonly List<IView> children = new List<IView>();
        public virtual IReadOnlyList<IView> Children => children;

        public float Width
        {
            get => (float)nativeView.Frame.Width;
            set
            {
                nativeView.SetFrameSize(new CoreGraphics.CGSize(value, nativeView.Frame.Height));
            }
        }
        public float Height
        {
            get => (float)nativeView.Frame.Height;
            set
            {
                nativeView.SetFrameSize(new CoreGraphics.CGSize(nativeView.Frame.Width, value));
            }
        }

        public string Identifier 
        { 
            get => nativeView.Identifier;
            set => nativeView.Identifier = value;
         }

        public string NodeName
        {
            get => nativeView.Identifier;
            set => nativeView.Identifier = value;
        }

        public bool Hidden
        {
            get => nativeView.Hidden;
            set => nativeView.Hidden = value;
        }

        public Rectangle Allocation => new Rectangle((float)nativeView.Frame.X, (float)nativeView.Frame.Y, (float)nativeView.Frame.Width, (float)nativeView.Frame.Height);

        protected NSView nativeView;

        public View() : this (new NSView ())
        {

        }

        public View(NSView nativeView)
        {
            this.nativeView = nativeView;
        }

        public void AddChild(IView view)
        {
            children.Add(view);
			OnAddChild(view);
		}

		protected virtual void OnAddChild (IView view)
		{
			nativeView.AddSubview(view.NativeObject as NSView);
		}

		protected virtual void OnRemoveChild(IView view)
		{
			((NSView)view.NativeObject).RemoveFromSuperview();
		}

		public virtual void ClearSubviews()
        {
            foreach (var item in nativeView.Subviews)
            {
                item.RemoveFromSuperview();
            }
        }

        public virtual void RemoveChild(IView view)
        {
            if (children.Contains (view))
            {
                children.Remove(view);
				OnRemoveChild(view);
			}
        }

        public void MakeFirstResponder()
        {
            nativeView.Window.MakeFirstResponder(nativeView);
        }

        public void SetPosition(float x, float y)
        {
            nativeView.SetFrameOrigin(new CoreGraphics.CGPoint(x, y));
        }

        public void SetAllocation(float x, float y, float width, float height)
        {
            nativeView.Frame = new CoreGraphics.CGRect(x, y, width, height);
        }
    }
}
