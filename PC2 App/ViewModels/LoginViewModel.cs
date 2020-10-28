using PC2_App.Models;
using PC2_App.Util;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PC2_App.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public Action ExibirAvisoDeLoginInvalido;
        public Action NavegarParaPaginaPrincipal;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private const string _URL = "http://192.168.1.106/web_api/API/PC2/Login";
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
        public LoginViewModel()
        {
            SubmitCommand = new Command(OnSubmit);
#if DEBUG
            CPF = "02633440029";
            SUS = "123";
#endif
        }
        public async void OnSubmit()
        {
            var provider = new RequestProvider();
            var Login = new { CPF, SUS };

            try
            {
                using (Acr.UserDialogs.UserDialogs.Instance.Loading("Realizando Login..."))
                {
                    Application.Current.Properties.Remove("Usuario");
                    var entity = await provider.PostAsync<Usuarios>(_URL, Login);
                    Application.Current.Properties.Add("Usuario", entity);
                }

                App.UsuarioLogado = true;
                NavegarParaPaginaPrincipal();
            }
            catch (Exception ex)
            {
                App.UsuarioLogado = false;
                await Task.Delay(100);

                ExibirAvisoDeLoginInvalido();
            }
        }
    }
}
