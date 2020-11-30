using PC2_App.Pages;
using Xamarin.Forms;

namespace PC2_App
{
    public partial class App : Application
    {
        public static bool UsuarioLogado { get; set; }
        //public static Usuarios Usuario { get; set; }

        public App()
        {
            InitializeComponent();

            if (!UsuarioLogado)
            {
                MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                MainPage = new NavigationPage(new MainPage());
            }
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
