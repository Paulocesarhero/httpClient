using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace httpClient.model
{
    public class ClienteBasicoHTTP
    {
        public static async Task<HttpResponseMessage> ejecutarPeticion(string url, string metodo)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage responseMessage = null;
                HttpRequestMessage requestMessage = null;
                switch (metodo)
                {
                    case "GET":
                        requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                        responseMessage = await client.SendAsync(requestMessage);
                        break;
                    case "HEAD":
                        requestMessage = new HttpRequestMessage(HttpMethod.Head, url);
                        responseMessage = await client.SendAsync(requestMessage);
                        break;
                    case "OPTIONS":
                        requestMessage = new HttpRequestMessage(HttpMethod.Options, url);
                        responseMessage = await client.SendAsync(requestMessage);
                        break;
                    case "Post":
                        requestMessage = new HttpRequestMessage(HttpMethod.Options, url);
                        responseMessage = await client.SendAsync(requestMessage);
                        break;
                    case "Put":
                        requestMessage = new HttpRequestMessage(HttpMethod.Options, url);
                        responseMessage = await client.SendAsync(requestMessage);
                        break;
                    case "Patch":
                        requestMessage = new HttpRequestMessage(HttpMethod.Options, url);
                        responseMessage = await client.SendAsync(requestMessage);
                        break;
                    case "delete":
                        requestMessage = new HttpRequestMessage(HttpMethod.Options, url);
                        responseMessage = await client.SendAsync(requestMessage);
                        break;
                }
                return responseMessage;
            }
    
        }
    }
}
