using ConsultaPrecos.RequestClasses;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsultaPrecos
{

    /*
     * 
     * Autor: Thales Rocha (thalesr@gmail.com)
     * 
     * Sobre: aplicativo criado para pesquisar preços de itens nos sites Kabum, Terabyte e Pichau
     * 
     * Código fonte livre!
     * 
     */


    class Program
    {
        static void Main(string[] args)
        {
            
            var requestKabum = new KabumRequest();
            var requestPichau = new PichauRequest();

            bool possuiResultados = false;

            Console.WriteLine("=====================================");
            Console.WriteLine("PESQUISA DE PREÇOS GPU - V0.1");
            Console.WriteLine("=====================================");
            Console.WriteLine();
            Console.WriteLine("Informe o parâmetro de pesquisa (ex.: RTX 2080): ");
            Console.Write("==>");
            var paramValue = Console.ReadLine();


            if (!String.IsNullOrWhiteSpace(paramValue))
            {

                string path = @"c:\PESQUISA_PRECOS\";
                string fileName = @"PESQUISA_(" + paramValue + ")" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".csv";

                Console.WriteLine("PESQUISANDO POR {0}", paramValue.ToUpper());
                var formatedParamValue = paramValue.TrimStart().TrimEnd().Replace(" ", "+");

                //Pesquisa na Kabum
                var listaRetornoKabum = requestKabum.ExecuteRequest(formatedParamValue.ToUpper());
                Console.WriteLine("RESULTADOS NA KABUM: {0}", listaRetornoKabum.Count);

                //Pesquisa na Pichau
                var listaRetornoPichau = requestPichau.ExecuteRequest(formatedParamValue.ToUpper());
                var countPichau = 0;
                listaRetornoPichau.ForEach(x =>
                {
                    countPichau += x.Ecommerce.Impressions.Count();
                });
                Console.WriteLine("RESULTADOS NA PICHAU: {0}", countPichau);

                //Pesquisa na Terabyte
                //TODO

                possuiResultados = listaRetornoKabum.Count > 0 || listaRetornoPichau.Count > 0;

                if (possuiResultados)
                {
                    Directory.CreateDirectory(path);

                    using (FileStream file = File.Create(path + fileName))
                    {

                        StreamWriter sw = new StreamWriter(file, Encoding.UTF8);
                        sw.Write("LOJA;ITEM PESQUISADO;FABRICANTE;MODELO;PRECO;PRECO COM DESCONTO");
                        sw.Write(sw.NewLine);

                        //Adicionando itens da Kabum
                        listaRetornoKabum.ForEach(x =>
                        {
                            sw.Write("{0};{1};{2};{3};{4};{5}", "KABUM", paramValue, x.Fabricante.Nome, x.Nome, x.Preco, x.Preco_Desconto);
                            sw.Write(sw.NewLine);
                        });

                        //Adicionando itens da Pichau
                        listaRetornoPichau.ForEach(x =>
                        {
                            x.Ecommerce.Impressions.ForEach(y =>
                            {
                                sw.Write("{0};{1};{2};{3};{4};{5}", "PICHAU", paramValue, y.Brand, y.Name, y.Price, y.Price - y.Discount);
                                sw.Write(sw.NewLine);
                            });
                        });

                        sw.Close();

                    }

                    Console.WriteLine("*************************");
                    Console.WriteLine("ARQUIVO .CSV GERADO COM SUCESSO!");
                    Console.WriteLine("CAMINHO DO ARQUIVO: {0}", path + fileName);
                    Console.WriteLine();
                    Console.WriteLine("Aperte qualquer tecla para encerrar o programa...");
                    Console.ReadKey();

                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("****NENHUM RESULTADO ENCONTRADO******");
                    Console.WriteLine();
                    Console.WriteLine("Aperte qualquer tecla para encerrar o programa...");
                    Console.ReadKey();
                }
                
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("****NENHUM VALOR VÁLIDO INFORMADO******");
                Console.WriteLine();
                Console.WriteLine("Aperte qualquer tecla para encerrar o programa...");
                Console.ReadKey();
            }
        }
    }
}
