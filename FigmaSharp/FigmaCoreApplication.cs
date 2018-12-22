﻿/* 
 * FigmaEnvirontment.cs 
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
using System.Collections.Generic;
using System.Reflection;

namespace FigmaSharp
{
    public class AppContext : IFigmaDelegate
    {
        static AppContext current;
        public static AppContext Current
        {
            get
            {
                if (current == null)
                {
                    current = new AppContext();
                }
                return current;
            }
        }

        internal string Token { get; set; }

        IFigmaDelegate figmaDelegate;

        AppContext()
        {

        }

        public void Configuration(IFigmaDelegate currentDelegate, string token)
        {
            SetAccessToken(token);
            figmaDelegate = currentDelegate;
        }

        public void SetAccessToken(string token)
        {
            Token = token;
        }

        public IViewWrapper CreateEmptyView() => figmaDelegate.CreateEmptyView();

        public FigmaViewConverter[] GetFigmaConverters() => figmaDelegate.GetFigmaConverters();

        public IImageWrapper GetImage(string url) => figmaDelegate.GetImage(url);

        public IImageWrapper GetImageFromFilePath(string filePath) =>
            figmaDelegate.GetImageFromFilePath(filePath);

        public IFigmaDocumentContainer GetFigmaDialogFromContent(string template) =>
            figmaDelegate.GetFigmaDialogFromContent(template);

        public void LoadFigmaFromFrameEntity(IViewWrapper contentView, IFigmaDocumentContainer document, List<IImageViewWrapper> figmaImages, string figmaFileName) =>
            figmaDelegate.LoadFigmaFromFrameEntity(contentView, document, figmaImages, figmaFileName);

        public IImageViewWrapper GetImageView(FigmaPaint figmaPaint) =>
            figmaDelegate.GetImageView(figmaPaint);

        public IImageWrapper GetImageFromManifest(Assembly assembly, string imageRef) =>
            figmaDelegate.GetImageFromManifest(assembly, imageRef);

        public string GetFigmaFileContent(string file, string token) =>
            figmaDelegate.GetFigmaFileContent(file, token);

        public string GetManifestResource(Assembly assembly, string file) =>
            figmaDelegate.GetManifestResource(assembly, file);
    }
}