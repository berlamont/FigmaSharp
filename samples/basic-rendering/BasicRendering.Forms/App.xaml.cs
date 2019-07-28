using Xamarin.Forms;

namespace BasicRendering.Forms
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();

			//Background color
			MainPage.SetValue(NavigationPage.BarBackgroundColorProperty, Color.Black);

			//Title color
			MainPage.SetValue(NavigationPage.BarTextColorProperty, Color.White);
		}

		protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
