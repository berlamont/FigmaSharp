using System;
using System.Collections.Generic;
using FigmaSharp;
using FigmaSharp.Models;
using FigmaSharp.Services;
using LiteForms.Forms;
using System.Linq;

namespace Xamarin.Forms
{
    public class FigmaContentPage : ContentPage
    {
        public string FileName { get; private set; }

        protected FigmaRemoteFileProvider FileProvider { get; private set; }

        protected FigmaViewRendererService RendererService { get; private set; }

        protected List<FigmaViewConverter> Converters => RendererService.CustomConverters;

		public string StartNodeID
		{
			get
			{
				return FileProvider.Nodes.OfType<FigmaCanvas>().FirstOrDefault()?.prototypeStartNodeID;
			}
		}

		public void LoadDocument (string fileName)
        {
			FileName = fileName;
			FileProvider.Load(fileName);
        }

		public void AddConverter (params FigmaViewConverter[] converters)
		{
			RendererService.CustomConverters.AddRange(converters);
		}

		public void RemoveConverter(params FigmaViewConverter[] converters)
		{
			foreach (var item in converters)
			{
				RendererService.CustomConverters.Remove(item);
			}
		}

		public FigmaContentPage()
        {
            FileProvider = new FigmaRemoteFileProvider();
            var converters = FigmaSharp.AppContext.Current.GetFigmaConverters();
            RendererService = new FigmaViewRendererService(FileProvider, converters);
        }

		public T FindNativeViewByName<T>(string name) where T : Xamarin.Forms.View
		{
			 return RendererService.FindNativeViewByName <T>(name);
		}

		public void RenderByPath<T>(params string[] path) where T : LiteForms.IView
		{
			RenderByPath<T>(new FigmaViewRendererServiceOptions(), path);
		}

		public void RenderByPath<T>(FigmaViewRendererServiceOptions options, params string[] path) where T : LiteForms.IView
		{
			var mainScreen = RendererService.RenderByPath<LiteForms.IView>(new FigmaViewRendererServiceOptions(), path);
			ContentView = mainScreen;
		}

		LiteForms.IView contentView;
		public LiteForms.IView ContentView {
			get => contentView;
			set
			{
				contentView = value;
				Content = contentView.NativeObject as AbsoluteLayout;
			}
		}

		public void RenderByName<T>(string name, FigmaViewRendererServiceOptions options) where T : LiteForms.IView
		{
			var mainScreen = RendererService.RenderByName<LiteForms.IView>(name, new FigmaViewRendererServiceOptions());
			ContentView = mainScreen;
		}

		public void RenderByName<T>(string name) where T : LiteForms.IView
		{
			RenderByName<T> (name, new FigmaViewRendererServiceOptions());
		}

		public void RenderByNode<T>(FigmaNode node, FigmaViewRendererServiceOptions options) where T : LiteForms.IView
		{
			var mainScreen = RendererService.RenderByNode<LiteForms.IView>(node, new FigmaViewRendererServiceOptions());
			ContentView = mainScreen;
		}

		public void RenderByNode<T>(FigmaNode node) where T : LiteForms.IView
		{
			RenderByNode<T>(node, new FigmaViewRendererServiceOptions());
		}

		public void RenderByNodeId<T>(string nodeId, FigmaViewRendererServiceOptions options) where T : LiteForms.IView
		{
			var selectedNode = RendererService.FindNodeById(nodeId);
			RenderByNode<T>(selectedNode, options);
		}

		public void RenderByNodeId <T>(string nodeId) where T : LiteForms.IView
		{
			RenderByNodeId<T>(nodeId, new FigmaViewRendererServiceOptions());
		}
	}
}
