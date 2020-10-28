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
            var vm = new ContatoPageViewModel();
            vm.ExibirAviso += () => DisplayAlert("Erro", "Ocorreu um erro durante a requisição.", "OK");
            vm.ExibirSucesso += () => DisplayAlert("Sucesso", "Requisição enviada com sucesso.", "OK");
            vm.NavegarParaPaginaInicial += () => Navigation.InsertPageBefore(new MainPage(), this);
            vm.NavegarParaPaginaInicial += async () => await Navigation.PopAsync();
            this.BindingContext = vm;

            InitializeComponent();
        }
    }
}
