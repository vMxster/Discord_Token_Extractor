using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Discord_Token_Extractor
{
    internal class DiscordTokenExtractor
    {
        private static async Task Main()
        {
            Console.Write("Enter Email: ");
            var email = RequireNonNull(Console.ReadLine());
            Console.Write("Enter Password: ");
            var password = RequireNonNull(Console.ReadLine());
            await ExtractToken(email, password);
        }
        
        private static string RequireNonNull(string obj)
        {
            if (obj == null)
            {
                throw new NullReferenceException("Il parametro non può essere nullo.");
            }
            return obj;
        }
        
        private static async Task ExtractToken(string email, string password)
        {
            string url = "https://discord.com/api/v9/auth/login";
            StringContent stringContent = new StringContent(
                JsonConvert.SerializeObject(
                    new Dictionary<string, string>()
                    {
                        { "login", email }, { "password", password }
                    }),
                Encoding.UTF8,
                "application/json");
            HttpResponseMessage responseMessage = await new HttpClient()
                .PostAsync(
                    url,
                    stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                Console.WriteLine("-- Login Successful --");
                string responseContent = await responseMessage.Content.ReadAsStringAsync();
                Console.WriteLine(GetUserID(responseContent));
                Console.WriteLine(GetToken(responseContent));
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("-- " + responseMessage.ReasonPhrase + " --");
                Console.ReadKey();
            }
        }

        private static string GetUserID(string contentID)
        {
            return "User ID: " + JObject
                .Parse(contentID)["user_id"];
        }

        private static string GetToken(string contentToken)
        {
            return "Token: " + JObject
                .Parse(contentToken)["ticket"];
        }
    }
}