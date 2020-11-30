using System;
using System.Collections.Generic;
using System.Text;

namespace PC2_App.Models
{
    public class RequisicaoAjuda
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdMedicamento { get; set; }
        public DateTime DataRequisicao { get; set; }
    }
}
