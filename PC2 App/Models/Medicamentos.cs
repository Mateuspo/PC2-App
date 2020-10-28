using System;
using System.Collections.Generic;
using System.Text;

namespace PC2_App.Models
{
    public class Medicamentos
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public decimal? Estoque { get; set; }
        public string Observacao { get; set; }
        public bool PossuiRequisicao { get; set; }
        public bool NaoSolicitado { get; set; }

        public string Exibicao { 
            get {
                return Descricao;
            } 
        }

        public bool NaoPossuiEstoque
        {
            get {
                return (Estoque ?? 0) <= 0;
            }
        }

        public bool PossuiEstoque
        {
            get
            {
                return (Estoque ?? 0) > 0;
            }
        }
    }
}
