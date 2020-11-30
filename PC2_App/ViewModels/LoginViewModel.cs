using PC2_App.Models;
using PC2_App.Pages;
using PC2_App.Util;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PC2_App.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private INavigation navigation;

        public INavigation GetNavigation()
        {
            return navigation;
        }

        public void SetNavigation(INavigation value)
        {
            navigation = value;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private const string _URL = "http://mateuspoliveira-001-site1.atempurl.com/API/PC2/Login";
        private string _CPF;
        public string CPF
        {
            get { return _CPF; }
            set
            {
                _CPF = value;
                PropertyChanged(this, new PropertyChangedEventArgs("CPF"));
            }
        }
        private string _SUS;
        public string SUS
        {
            get { return _SUS; }
            set
            {
                _SUS = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SUS"));
            }
        }
        public ICommand SubmitCommand { protected set; get; }
        public LoginViewModel(INavigation navigation)
        {
            this.SetNavigation(navigation);
            SubmitCommand = new Command(OnSubmit);
#if DEBUG
            CPF = "02633440029";
            SUS = "1234";
#endif
        }

        public async void OnSubmit()
        {
            var provider = new RequestProvider();
            var Login = new { CPF, SUS };

            if (Connectivity.NetworkAccess == NetworkAccess.Internet || Connectivity.NetworkAccess == NetworkAccess.Local)
            {
                try
                {
                    using (Acr.UserDialogs.UserDialogs.Instance.Loading("Realizando Login..."))
                    {
                        Application.Current.Properties.Remove("Usuario");
                        var entity = await provider.PostAsync<Usuarios>(_URL, Login);
                        Application.Current.Properties.Add("Usuario", entity);
                    }

                    App.UsuarioLogado = true;
                    await NavegarParaPaginaPrincipal();
                }
                catch (Exception ex)
                {
                    App.UsuarioLogado = false;
                    await Task.Delay(100);

                    ExibirAvisoDeLoginInvalido("Login Inválido, tente novamente");
                }
            }
            else
            {
                ExibirAvisoDeLoginInvalido("Sem conexão com a internet");
            }
        }

        private void ExibirAvisoDeLoginInvalido(string Mensagem)
        {
            this.navigation.NavigationStack[0].DisplayAlert("Erro", Mensagem, "OK");
        }

        private async Task NavegarParaPaginaPrincipal()
        {            
            this.navigation.InsertPageBefore(new MainPage(), this.navigation.NavigationStack[0]);
            await navigation.PopAsync();
        }
    }
}
