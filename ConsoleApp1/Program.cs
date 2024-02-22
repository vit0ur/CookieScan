using System;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Net;
using System.Text;
//using Newtonsoft.Json;

namespace ScannerCookie
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // URL do site a ser verificado
            string url = "https://exemplo.com/login";

            // Executa o scanner de cookies
            await ScanCookies(url);
        }

        static async Task ScanCookies(string url)
        {
            try
            {
                // Cria uma solicitação HTTP para a URL especificada
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                // Obtém a resposta da solicitação HTTP
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    // Verifica se houve redirecionamento
                    if (response.ResponseUri.AbsoluteUri != url)
                    {
                        Console.WriteLine("Redirecionado para: " + response.ResponseUri.AbsoluteUri);
                        Console.ReadKey();
                    }
                    // Obtém todos os cookies da resposta HTTP
                    string cookies = response.Headers[HttpResponseHeader.SetCookie];

                    // Exibe os cookies encontrados
                    Console.WriteLine("Cookies encontrados em " + url);
                    Console.WriteLine(cookies);
                    Console.ReadKey();
                }

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        // Lê o conteúdo da resposta
                        string html = await response.Content.ReadAsStringAsync();

                        // Carrega o HTML usando o HtmlAgilityPack
                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(html);

                        // Obtém todos os cookies da resposta HTTP
                        IEnumerable<string> cookies = response.Headers.GetValues("Set-Cookie");
                        // Exibe os cookies encontrados
                        Console.WriteLine("Cookies encontrados em " + url);
                        foreach (string cookie in cookies)
                        {
                            Console.WriteLine(cookie);
                        }
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Erro: " + response.ReasonPhrase);
                        Console.ReadKey();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao escanear cookies: " + ex.Message);
                Console.ReadKey();
            }
        }

    }
}



/*Alteração pra quando é utilizado uma Autenticação via Token ou login
        // Autenticação
        string username = "seu_nome_de_usuário";
        string password = "sua_senha";
        var requestAuth = (HttpWebRequest)WebRequest.Create("https://exemplo.com/login");
        requestAuth.Method = "POST";
        requestAuth.ContentType = "application/json";
        string jsonRequestBody = JsonConvert.SerializeObject(new { username, password });
        byte[] requestBodyBytes = Encoding.UTF8.GetBytes(jsonRequestBody);
        requestAuth.ContentLength = requestBodyBytes.Length;
        using (var stream = requestAuth.GetRequestStream())
        {
            stream.Write(requestBodyBytes, 0, requestBodyBytes.Length);
        }
        using (var responseAuth = (HttpWebResponse)requestAuth.GetResponse())
        {
            string authToken = responseAuth.Headers[HttpResponseHeader.Authorization];

            // Faz uma solicitação HTTP para a URL especificada com o token de autenticação
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Headers[HttpRequestHeader.Authorization] = authToken;
        }
*/