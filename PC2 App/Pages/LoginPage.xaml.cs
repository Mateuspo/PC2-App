using PC2_App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PC2_App.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            var vm = new LoginViewModel();
            this.BindingContext = vm;

            vm.ExibirAvisoDeLoginInvalido += () => DisplayAlert("Erro", "Login Inválido, tente novamente", "OK");
            vm.NavegarParaPaginaPrincipal += () => Navigation.InsertPageBefore(new MainPage(), this);
            vm.NavegarParaPaginaPrincipal += async () => await Navigation.PopAsync();

            InitializeComponent();
            CPF.Completed += (object sender, EventArgs e) =>
            {
                SUS.Focus();
            };
            SUS.Completed += (object sender, EventArgs e) =>
            {
                vm.SubmitCommand.Execute(null);
            };
        }
    }
}