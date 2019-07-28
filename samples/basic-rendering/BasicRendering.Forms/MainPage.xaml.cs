using System.ComponentModel;
using ExampleFigma;
using FigmaSharp;
using FigmaSharp.Forms;
using FigmaSharp.Services;
using LiteForms;
using Xamarin.Forms;

namespace BasicRendering.Forms
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
		const string fileName = "NTCS6WenCj7JrvIv5Raeaq";

        public MainPage()
        {
            InitializeComponent();

			var converters = FigmaSharp.AppContext.Current.GetFigmaConverters();
			var fileProvider = new FigmaRemoteFileProvider();
			fileProvider.Load(fileName);

			var renderer = new FigmaViewRendererService(fileProvider, converters);

			var mainScreen = renderer.RenderByPath<IView>(new FigmaViewRendererServiceOptions(), "Log-In Page");
			BackgroundColor = Xamarin.Forms.Color.Black;
			Content = mainScreen.NativeObject as AbsoluteLayout;
		}
    }
}