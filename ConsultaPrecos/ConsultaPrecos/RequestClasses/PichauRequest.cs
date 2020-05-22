using ConsultaPrecos.JsonEntity.Pichau;
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
    public class PichauRequest
    {

        public List<PichauJson> ExecuteRequest(string searchParam)
        {

            PichauJson[] retorno = new PichauJson[0];

            try
            {

                Console.Write("PICHAU...");

                searchParam = searchParam.TrimStart().TrimEnd().Replace(" ", "+");

                string urlAddress = "https://www.pichau.com.br/catalogsearch/result/index/?product_list_limit=48&q=" + searchParam;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();


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

                    retorno = JsonConvert.DeserializeObject<PichauJson[]>(json);

                    response.Close();
                    readStream.Close();

                    Console.Write("...OK");

                }
            }
            catch (Exception)
            {
                Console.Write("...A CONSULTA AO SITE DA PICHAU NÃO FOI BEM SUCEDIDA");
            }

            Console.WriteLine();
            return retorno.ToList();

        }

        public static string GetJson(string dados)
        {
            var stringPesquisaIndexInicial = "{\"ecomm_pagetype\":\"searchresults\"},";
            var stringPesquisaIndexFinal = "for (var i in dlObjects) {";

            var indexOfInicio = dados.IndexOf(stringPesquisaIndexInicial) + stringPesquisaIndexInicial.Length;
            var indexOfFim = dados.IndexOf(stringPesquisaIndexFinal);

            var jsonBruto = dados.Substring(indexOfInicio, indexOfFim - indexOfInicio);
            var jsonPrimeiroTratamento = "[{" + jsonBruto;
            var jsonSegundoTratamento = jsonPrimeiroTratamento.Substring(0, jsonPrimeiroTratamento.IndexOf("];"));
            var jsonFinal = jsonSegundoTratamento + "]";

            return jsonFinal;

        }

    }
}
