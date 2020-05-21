using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultaPrecos.JsonEntity.Kabum
{
    public class KabumJson
    {

        public KabumFabricanteJson Fabricante { get; set; }
        public decimal Preco { get; set; }
        public decimal Preco_Desconto { get; set; }
        public string Nome { get; set; }
        public string Link_Descricao { get; set; }

    }
}
