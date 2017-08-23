using System;
using System.Net.Http;
using System.Threading.Tasks;
using WebServerComponent.MessageHandler;

namespace WebApiTestConsole
{
    class Program
    {
        static void Main()
        {
            Console.ReadKey();
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {

            Console.WriteLine("Calling the back-end API");

            var apiBaseAddress = "http://localhost:2637/";

            var customDelegatingHandler = new HmacAutheRequestDelegateHandler("4d53bce03ec34c0a911182d4c228ee6c", "A93reRTUJHsCuQSHR+L3GxqOJyDmQpCgps102ciuabc=", "cpx");

            var client = HttpClientFactory.Create(customDelegatingHandler);

            var response = await client.GetAsync(apiBaseAddress + "api/ServerInfo");

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseString);
                Console.WriteLine("HTTP Status: {0}, Reason {1}. Press ENTER to exit", response.StatusCode, response.ReasonPhrase);
            }
            else
            {
                Console.WriteLine("Failed to call the API. HTTP Status: {0}, Reason {1}", response.StatusCode, response.ReasonPhrase);
            }

            Console.ReadLine();
        }
    }
}
