using ConsultaPrecos.JsonEntity.Kabum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsultaPrecos.RequestClasses
{
    public class KabumRequest
    {
        public List<KabumJson> ExecuteRequest(string searchParam)
        {

            searchParam = searchParam.TrimStart().TrimEnd().Replace(" ", "+");

            string urlAddress = "https://www.kabum.com.br/cgi-local/site/listagem/listagem.cgi?string=" + searchParam;
            string urlParameters = "&pagina=1&ordem=5&limite=100&prime=false&marcas=[]&tipo_produto=[]&filtro=[]";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress + urlParameters);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            KabumJson[] returnList = null;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (String.IsNullOrWhiteSpace(response.CharacterSet))
                    readStream = new StreamReader(receiveStream);
                else
                    readStream = new StreamReader(receiveStream, Encoding.UTF8);

                string data = readStream.ReadToEnd();
                var json = GetJson(data);
                
                returnList = JsonConvert.DeserializeObject<KabumJson[]>(json);

                response.Close();
                readStream.Close();

            }
            else
            {
                returnList = new KabumJson[0]; //Representa uma lista vazia
            }

            return returnList.ToList();

        }

        public static string GetJson(string dados)
        {

            var indexOfInicio = dados.IndexOf("const listagemDados = ");
            var indexOfFim = dados.IndexOf("const listagemCount");

            var jsonBruto = dados.Substring(indexOfInicio, indexOfFim - indexOfInicio);
            var jsonPrimeiroTratamento = jsonBruto.Substring(22);
            var jsonSegundoTratamento = jsonPrimeiroTratamento.Substring(0, jsonPrimeiroTratamento.IndexOf("];"));
            var jsonFinal = jsonSegundoTratamento + "]";

            return jsonFinal;

        }
    }
}
