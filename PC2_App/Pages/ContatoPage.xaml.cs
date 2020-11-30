using PC2_App.Pages;
using PC2_App.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace PC2_App
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ContatoPage : ContentPage
    {
        public ContatoPage()
        {
            var vm = new ContatoPageViewModel(this.Navigation);            
            this.BindingContext = vm;

            InitializeComponent();
        }
    }
}
