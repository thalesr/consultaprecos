using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultaPrecos.JsonEntity.Pichau
{
    public class PichauEcommerceJson
    {
        public string CurrencyCode { get; set; }
        public List<PichauImpressionJson> Impressions { get; set; }

    }
}
