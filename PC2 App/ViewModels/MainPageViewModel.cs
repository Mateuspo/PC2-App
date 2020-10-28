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

namespace PC2_App.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private bool atualizando;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public Action ExibirAviso;
        public Action ExibirSucesso;
        public Action NavegarParaPaginaDoacao;
        public ICommand SearchTermChangedCommand => new Command<string>(SearchTermChanged);
        public ICommand SolicitarButtonPressCommand => new Command<int>(SolicitarButtonPress);
        public ICommand DoarButtonPressCommand => new Command<int>(DoarButtonPress);
        public ICommand RefreshCommand => new Command(Refresh);
        public List<Medicamentos> medicamentosPesquisa { get; set; }
        public List<Medicamentos> medicamentos { get; set; }

        public bool Atualizando
        {
            get => atualizando;
            set
            {
                atualizando = value;
                RaisePropertyChanged(nameof(Atualizando));
            }
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        private void SolicitarButtonPress(int idMedicamento)
        {
            Solicitar(idMedicamento);
        }

        private void DoarButtonPress(int idMedicamento)
        {
            Application.Current.Properties.Remove("idMedicamento");
            Application.Current.Properties.Add("idMedicamento", idMedicamento);
            NavegarParaPaginaDoacao();
        }

        public MainPageViewModel()
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
            var usuario = Application.Current.Properties["Usuario"] as Usuarios;
            var url = $"http://192.168.1.106/web_api/API/PC2/Medicamentos?porDisponibilidade=false&codUsuario={usuario.Id}";

            var provider = new RequestProvider();
            try
            {
                medicamentosPesquisa = await provider.GetAsync<List<Medicamentos>>(url);

                if (medicamentosPesquisa != null)
                {
                    SearchTermChanged("");
                }
            }
            catch (System.Exception ex)
            {
                ExibirAviso();
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
            string _URL = "http://192.168.1.106/web_api/API/PC2/RequisicaoAjuda";
            var provider = new RequestProvider();
            var usuario = Application.Current.Properties["Usuario"] as Usuarios;
            var Dados = new { UsuarioId = usuario.Id, MedicamentoId = idMedicamento, DataRequisicao = DateTime.Now };

            try
            {
                var entity = await provider.PostAsync<RequisicaoAjuda>(_URL, Dados);

                if (entity != null)
                {
                    ExibirSucesso();
                }
            }
            catch (Exception ex)
            {
                ExibirAviso();
            }            
        }
    }
}
