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

using System;
using System.Linq;
using CoreGraphics;
using FigmaSharp.Models;
using UIKit;

namespace FigmaSharp.iOS
{
    public class ScrollViewWrapper : ViewWrapper, IScrollViewWrapper
    {
        readonly UIScrollView scrollView;

        UIView contentView;
        IViewWrapper contentViewWrapper;

        public IViewWrapper ContentView
        {
            get => contentViewWrapper;
            set
            {
                if (value.NativeObject is UIView content)
                {
                    contentView?.RemoveFromSuperview();

                    scrollView.AddSubview(content);
                    contentView = content;
                    contentViewWrapper = value;
                }
            }
        }

        public ScrollViewWrapper(UIScrollView scrollView) : base(scrollView)
        {
            this.scrollView = scrollView;

            if (scrollView.Subviews.Length == 0)
            {
                contentView = new UIView() { Frame = scrollView.Frame };
                scrollView.AddSubview(contentView);
            } else
            {
                contentView = scrollView.Subviews.FirstOrDefault();
            }
            contentViewWrapper = new ViewWrapper(contentView);
            scrollView.ScrollEnabled = true;
        }

        public FigmaColor BackgroundColor { 
            get => scrollView.BackgroundColor.ToFigmaColor ();
            set => scrollView.BackgroundColor = value.ToUIColor();
        }

        public override void AddChild(IViewWrapper view)
        {
            scrollView.AddSubview(view.NativeObject as UIView);
        }

        public void AdjustToContent()
        {
            CGRect contentRect = CGRect.Empty;
            foreach (var view in scrollView.Subviews)
            {
                contentRect = contentRect.UnionWith(view.Frame);
            }
            SetContentSize((float) contentRect.Width, (float)contentRect.Height);
        }

        public override void RemoveChild(IViewWrapper view)
        {
            if (scrollView.Subviews.Contains (view.NativeObject)) {
                ((UIView)view.NativeObject).RemoveFromSuperview();
            }
        }

        public void SetContentSize(float width, float height)
        {
            scrollView.ContentSize = new CGSize(width, height);
            if (contentView != null)
            {
                contentView.Frame = new CGRect(0, 0, width, height);
            }
        }
    }
}
