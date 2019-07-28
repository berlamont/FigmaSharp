using System.ComponentModel;
using ExampleFigma;
using FigmaSharp;
using FigmaSharp.Forms;
using FigmaSharp.Services;
using LiteForms;
using Xamarin.Forms;
using System.Linq;

namespace BasicRendering.Forms
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
		const string fileName = "NTCS6WenCj7JrvIv5Raeaq";
		FigmaViewRendererService renderer;
		public MainPage()
        {
            InitializeComponent();

			var converters = FigmaSharp.AppContext.Current.GetFigmaConverters();
			converters = converters.Concat(
				new FigmaViewConverter[] {
					new LoginButtonConverter(),
					new LoginTextFieldConverter()
				}).ToArray ();

			var fileProvider = new FigmaRemoteFileProvider();
			fileProvider.Load(fileName);

			renderer = new FigmaViewRendererService(fileProvider, converters);

			var mainScreen = renderer.RenderByPath<IView>(new FigmaViewRendererServiceOptions(), "Log-In Page");
			BackgroundColor = Xamarin.Forms.Color.Black;
			Content = mainScreen.NativeObject as AbsoluteLayout;

			LoginButton = renderer.FindViewByName<IView>("LoginButton").NativeObject as Xamarin.Forms.Button;
			LoginButton.Pressed += Button_Pressed;

			EmailTextField = renderer.FindViewByName<IView>("EmailTextField").NativeObject as Xamarin.Forms.Entry;
			EmailTextField = renderer.FindViewByName<IView>("PasswordTextField").NativeObject as Xamarin.Forms.Entry;

		}

		Xamarin.Forms.Entry EmailTextField, PasswordTextField;
		Xamarin.Forms.Button LoginButton;

		private void Button_Pressed(object sender, System.EventArgs e)
		{
			var mainScreen = renderer.RenderByPath<IView>(new FigmaViewRendererServiceOptions(), "Home");
			Content = mainScreen.NativeObject as AbsoluteLayout;
			BackgroundColor = Xamarin.Forms.Color.White;
		}
	}
}