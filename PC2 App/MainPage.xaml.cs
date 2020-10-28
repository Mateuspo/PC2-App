using PC2_App.Pages;
using PC2_App.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace PC2_App
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            var vm = new MainPageViewModel();
            vm.ExibirAviso += () => DisplayAlert("Erro", "Ocorreu um erro durante a requisição.", "OK");
            vm.ExibirSucesso += () => DisplayAlert("Sucesso", "Requisição enviada com sucesso.", "OK");
            vm.NavegarParaPaginaDoacao += () => Navigation.InsertPageBefore(new ContatoPage(), this);
            vm.NavegarParaPaginaDoacao += async () => await Navigation.PopAsync();
            this.BindingContext = vm;

            InitializeComponent();
        }
    }
}
