﻿/* 
 * FigmaRectangleVectorConverter.cs
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
using Gtk;
using System.Text;
using FigmaSharp.Models;

namespace FigmaSharp.GtkSharp.Converters
{
    public class FigmaRectangleVectorConverter : FigmaRectangleVectorConverterBase
    {
        public override IViewWrapper ConvertTo(FigmaNode currentNode, ProcessedNode parent)
        {
            var model = (FigmaRectangleVector)currentNode;
            if (model.HasFills && model.fills[0].type == "IMAGE" && model.fills[0] is FigmaPaint figmaPaint)
            {
                var imageView = new Image();
                var figmaImageView = new ImageViewWrapper(imageView);
                imageView.Configure(model);
                return figmaImageView;
            }

            var view = new Fixed();
            view.Configure(model);
            return new ViewWrapper(view);
        }

        public override string ConvertToCode(FigmaNode currentNode)
        {
            StringBuilder builder = new StringBuilder();
            var name = "[NAME]";
            builder.AppendLine($"var {name} = new Gtk.{nameof(Fixed)}();");

            builder.Configure(name, (FigmaRectangleVector)currentNode);
            return builder.ToString();
        }
    }
}
