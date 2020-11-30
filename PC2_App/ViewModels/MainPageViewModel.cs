using PC2_App.Models;
using PC2_App.Pages;
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

namespace PC2_App.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
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
        public ICommand SearchTermChangedCommand => new Command<string>(SearchTermChanged);
        public ICommand SolicitarButtonPressCommand => new Command<int>(SolicitarButtonPress);
        public ICommand DoarButtonPressCommand => new Command<int>(DoarButtonPress);
        public ICommand RefreshCommand => new Command(Refresh);
        public ICommand SairCommand => new Command(Sair);
        public List<Medicamentos> medicamentosPesquisa { get; set; }
        public List<Medicamentos> medicamentos { get; set; }

        public MainPageViewModel(INavigation navigation)
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
                RaisePropertyChanged(nameof(atualizando));
            }
        }

        private void SearchTermChanged(string obj)
        {
            if (medicamentosPesquisa != null && medicamentosPesquisa.Count > 0)
            {
                medicamentos = medicamentosPesquisa.Where(x => x.Descricao.Contains(obj)).ToList();
                RaisePropertyChanged(nameof(medicamentos));
            }
        }

        private void Refresh()
        {
            Carregar();
        }

        private void Sair()
        {
            navigation.InsertPageBefore(new LoginPage(), this.navigation.NavigationStack[0]);
            navigation.PopAsync();
        }

        private void SolicitarButtonPress(int idMedicamento)
        {
            Solicitar(idMedicamento);
        }

        private void DoarButtonPress(int idMedicamento)
        {
            var medicamento = medicamentosPesquisa.Where(x => x.Id == idMedicamento).First();
            Application.Current.Properties.Remove("Medicamento");
            Application.Current.Properties.Add("Medicamento", medicamento);
            navigation.PushAsync(new ContatoPage());
        }

        private async void Carregar()
        {
            Atualizando = true;
            await CarregaValores();
            Atualizando = false;
        }

        private async Task CarregaValores()
        {
            var usuario = Application.Current.Properties["Usuario"] as Usuarios;
            var url = $"http://mateuspoliveira-001-site1.atempurl.com/API/PC2/Medicamentos?porDisponibilidade=false&codUsuario={usuario.Id}";

            var provider = new RequestProvider();

            if (Connectivity.NetworkAccess == NetworkAccess.Internet || Connectivity.NetworkAccess == NetworkAccess.Local)
            {
                try
                {
                    medicamentosPesquisa = await provider.GetAsync<List<Medicamentos>>(url);

                    if (medicamentosPesquisa != null)
                    {
                        SearchTermChanged("");
                    }
                }
                catch (Exception ex)
                {
                    await ExibirAviso(ex.Message);
                }
            }
            else
            {
                await ExibirAviso("Sem conexão com a internet");
            }
        }

        private async void Solicitar(int idMedicamento)
        {
            Atualizando = true;
            await SolicitarDados(idMedicamento);
            await CarregaValores();
            Atualizando = false;
        }

        private async Task SolicitarDados(int idMedicamento)
        {
            string _URL = "http://mateuspoliveira-001-site1.atempurl.com/API/PC2/RequisicaoAjuda";
            var provider = new RequestProvider();
            var usuario = Application.Current.Properties["Usuario"] as Usuarios;
            var Dados = new { UsuarioId = usuario.Id, MedicamentoId = idMedicamento, DataRequisicao = DateTime.Now };

            if (Connectivity.NetworkAccess == NetworkAccess.Internet || Connectivity.NetworkAccess == NetworkAccess.Local)
            {

                try
                {
                    var entity = await provider.PostAsync<RequisicaoAjuda>(_URL, Dados);

                    if (entity != null)
                    {
                        await ExibirSucesso();
                    }
                }
                catch (Exception ex)
                {
                    await ExibirAviso(ex.Message);
                }
            }
            else
            {
                await ExibirAviso("Sem conexão com a internet");
            }
        }

        private async Task ExibirSucesso()
        {
            await this.navigation.NavigationStack[0].DisplayAlert("Sucesso", "Solicitação enviado com sucesso.", "OK");
        }

        private async Task ExibirAviso(string erro)
        {
            await this.navigation.NavigationStack[0].DisplayAlert("Erro", erro, "OK");
        }
    }
}
