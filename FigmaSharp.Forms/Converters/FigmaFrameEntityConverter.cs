﻿/* 
 * FigmaFrameEntityConverter.cs
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

using FigmaSharp.Converters;
using Xamarin.Forms;
using FigmaSharp.Models;
using LiteForms;
using FigmaSharp.Services;

namespace FigmaSharp.Forms.Converters
{
    public class FigmaFrameEntityConverter : FigmaFrameEntityConverterBase
    {
        public override IView ConvertTo(FigmaNode currentNode, ProcessedNode parent, FigmaRendererService rendererService)
        {
            //var imageView = new Xamarin.Forms.Image();
            //var figmaImageView = new LiteForms.Forms.ImageView(imageView);
            //imageView.Configure((FigmaFrameEntity)currentNode);
            //return figmaImageView;
            var currengroupView = new AbsoluteLayout { Margin = 0, Padding = 0 };
            var figmaFrameEntity = (FigmaFrameEntity)currentNode;
            currengroupView.Configure(figmaFrameEntity);
            return new LiteForms.Forms.View(currengroupView);
        }

        public override string ConvertToCode(FigmaNode currentNode, FigmaCodeRendererService rendererService)
        {
            return string.Empty;
        }
    }
}
