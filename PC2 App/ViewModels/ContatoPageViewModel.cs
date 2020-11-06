using PC2_App.Models;
using PC2_App.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.OpenWhatsApp;

namespace PC2_App.ViewModels
{
    public class ContatoPageViewModel : INotifyPropertyChanged
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

        private bool atualizando;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public ICommand SolicitarButtonPressCommand => new Command<int>(SolicitarButtonPress);
        public ICommand RefreshCommand => new Command(Refresh);
        public List<Usuarios> usuarios { get; set; }

        public ContatoPageViewModel(INavigation navigation)
        {
            this.SetNavigation(navigation);
            Carregar();
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool Atualizando
        {
            get => atualizando;
            set
            {
                atualizando = value;
                RaisePropertyChanged(nameof(Atualizando));
            }
        }

        private void Refresh()
        {
            Carregar();
        }

        private async void Carregar()
        {
            Atualizando = true;
            await CarregaValores();
            Atualizando = false;
        }

        private async Task CarregaValores()
        {
            var medicamento = Application.Current.Properties["Medicamento"] as Medicamentos;
            var usuario = Application.Current.Properties["Usuario"] as Usuarios;
            var url = $"http://mateuspoliveira-001-site1.atempurl.com/API/PC2/Usuarios?porMedicamentos={medicamento.Id}&codUsuario={usuario.Id}";

            var provider = new RequestProvider();
            try
            {
                usuarios = await provider.GetAsync<List<Usuarios>>(url);

                if (usuarios != null)
                {
                    RaisePropertyChanged(nameof(usuarios));
                }
            }
            catch (Exception ex)
            {
                ExibirAviso(ex.Message);
            }
        }

        private async void ExibirAviso(string erro)
        {
            await this.navigation.NavigationStack[1].DisplayAlert("Erro", erro, "OK");
        }

        private async void SolicitarButtonPress(int idUsuario)
        {
            var usuario = usuarios.Where(x => x.Id == idUsuario).FirstOrDefault();
            var medicamento = Application.Current.Properties["Medicamento"] as Medicamentos;

            try
            {
                Chat.Open("+55" + usuario.Telefone, "Olá, vim através do APP de consulta de medicamentos e tenho disponível o medicamento _*" + medicamento.Descricao + "*_ que você solicitou.");
            }
            catch (Exception)
            {
                var Acao = "";

                if (Device.RuntimePlatform == Device.Android)
                    Acao = "Ligar";
                else
                    Acao = "E-mail";

                if (await this.navigation.NavigationStack[1].DisplayAlert("Erro", "Não possivel abrir o WhatsApp.", Acao, "Cancelar"))
                {
                    if (Acao.Equals("Ligar"))
                        PhoneDialer.Open(usuario.Telefone);
                    else
                        await Email.ComposeAsync("Doação de Medicamento.", "Olá, vim através do APP de consulta de medicamentos e tenho disponível o medicamento " + medicamento.Descricao + " que você solicitou.", new string[] { usuario.Email });
                }
            }
        }
    }
}
