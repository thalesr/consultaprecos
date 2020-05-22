using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultaPrecos.JsonEntity.Pichau
{
    public class PichauImpressionJson
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }

        public decimal Discount 
        {
            get
            {
                return Price * new Decimal(0.12);
            }
            private set { }
        }

    }
}
