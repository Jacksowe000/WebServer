using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;
using System.Text.Json;
namespace webapp.Server {
    public class Program {
        private enum Requests {
            Get,
            Post
        }
        private static Dictionary<Tuple<string, Requests>, Delegate> APIs = new();
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();


            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            Console.WriteLine("Building");
            BuildAPIs();
            
            LoadAPIS(app);

            //app.MapFallback(() => JsonSerializer.Serialize(new Item { name = "fuck", quantity = 0, UPC=69}));

            app.MapFallbackToFile("/index.html");

            Console.WriteLine("Running...");
            app.Run();
        }

        private static void LoadAPIS(WebApplication app) {
            foreach(var api in APIs.Keys) {
                var name = api.Item1;
                var method = api.Item2;

                if(method == Requests.Get) {
                    app.MapGet(name, APIs[api]);
                }else if(method == Requests.Post) {
                    app.MapPost(name, APIs[api]);
                }
            }
        }

        private static void BuildAPIs() {
            APIs[new Tuple<string, Requests>("/inventory", Requests.Get)] = GetInventory;
        }

        private static IEnumerable<Item> GetInventory(HttpContext context) {
            var inventory = Enumerable.Range(1, 5).Select(i =>
                    new Item {
                        name = "bacon",
                        UPC = (ulong)i,
                        quantity = (uint)i
                    }
                ).ToArray();
            return inventory;
        }
    }
}
