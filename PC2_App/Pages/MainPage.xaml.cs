using PC2_App.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace PC2_App.Pages
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            var vm = new MainPageViewModel(this.Navigation);
            this.BindingContext = vm;

            InitializeComponent();
        }
    }
}
