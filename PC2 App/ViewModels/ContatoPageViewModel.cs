using PC2_App.Models;
using PC2_App.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.OpenWhatsApp;

namespace PC2_App.ViewModels
{
    public class ContatoPageViewModel : INotifyPropertyChanged
    {
        private bool atualizando;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public Action ExibirAviso;
        public Action ExibirSucesso;
        public Action NavegarParaPaginaInicial;
        public ICommand SolicitarButtonPressCommand => new Command<int>(SolicitarButtonPress);
        public ICommand RefreshCommand => new Command(Refresh);
        public List<Usuarios> usuarios { get; set; }

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

        public ContatoPageViewModel()
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
            var idMedicamento = Application.Current.Properties["idMedicamento"] as int?;
            var usuario = Application.Current.Properties["Usuario"] as Usuarios;
            var url = $"http://192.168.1.106/web_api/API/PC2/Usuarios?porMedicamentos={idMedicamento.Value.ToString()}&codUsuario={usuario.Id}";

            var provider = new RequestProvider();
            try
            {
                usuarios = await provider.GetAsync<List<Usuarios>>(url);

                if (usuarios != null)
                {
                    RaisePropertyChanged(nameof(usuarios));
                }
            }
            catch (System.Exception ex)
            {
                ExibirAviso();
            }
        }
        private void SolicitarButtonPress(int idUsuario)
        {
            var usuario = usuarios.Where(x => x.Id == idUsuario).FirstOrDefault();
            Chat.Open("+55" + usuario.Telefone, "Olá, vim através do APP de consulta de medicamentos e tenho disponível o medicamento que você solicitou.");
        }        
    }
}
