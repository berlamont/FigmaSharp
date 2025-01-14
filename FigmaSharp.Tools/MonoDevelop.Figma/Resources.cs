﻿/* 
 * FigmaViewContent.cs 
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

using System.IO;
using FigmaSharp.Services;

namespace MonoDevelop.Figma
{
    static class Resources
    {
        public static void Init ()
        {
            var userDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

            Directories.FigmaSharpCache = Path.Combine(userDirectory, ".cache", "FigmaSharp");

            if (!Directory.Exists(Directories.FigmaSharpCache))
                Directory.CreateDirectory(Directories.FigmaSharpCache);

            Directories.MonoDevelopExtensions = Path.Combine(Directories.FigmaSharpCache, "monodevelop");
            if (!Directory.Exists(Directories.MonoDevelopExtensions))
                Directory.CreateDirectory(Directories.MonoDevelopExtensions);

            ModuleService.LoadModules(Directories.MonoDevelopExtensions);
        }

        class Directories
        {
            static Directories()
            {
               
            }
            public static string FigmaSharpCache { get; internal set; }
            public static string MonoDevelopExtensions { get; internal set; }
        }
     
    }
}