using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultaPrecos.JsonEntity.Pichau
{
    public class PichauJson
    {

        public string Event { get; set; }
        public string EventCategory { get; set; }
        public string EventAction { get; set; }
        public string EventLabel { get; set; }
        public PichauEcommerceJson Ecommerce { get; set; }

    }
}
