using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.Extensions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
namespace webapp.Server {
    public class Program {
        private static string allowCors = "_allowCors";
        private enum Requests {
            Get,
            Post
        }
        private static Dictionary<Tuple<string, Requests>, Delegate> APIs = new();
        private static InventoryContext db;
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();


            var app = builder.Build();

            app.UseDefaultFiles(); 
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.

            // app.UseHttpsRedirection();

            app.UseAuthorization();
            

            InitDB();

            Console.WriteLine("Building");
            BuildAPIs();

            LoadAPIS(app);

            //app.MapFallback(() => JsonSerializer.Serialize(new Item { name = "fuck", quantity = 0, UPC=69}));

            app.MapFallbackToFile("/index.html");

            Console.WriteLine("Running...");
            app.Run();
        }

        private static void InitDB() {
            db = new InventoryContext();

            List<Item> toRemove = [.. db.Inventory];

            foreach (var item in toRemove) {
                db.Inventory.Remove(item);
            }

            db.Inventory.Add(new Item {
                name = "Dr. Pepper",
                UPC = 078000806564,
                quantity = 100
            });

            db.SaveChanges();

        }

        private static void LoadAPIS(WebApplication app) {
            foreach (var api in APIs.Keys) {
                var name = api.Item1;
                var method = api.Item2;

                if (method == Requests.Get) {
                    app.MapGet(name, APIs[api]);
                }
                else if (method == Requests.Post) {
                    app.MapPost(name, APIs[api]);
                }
            }
        }

        private static void BuildAPIs() {
            APIs[new ("/inventory", Requests.Get)] = GetInventory;
            APIs[new ("/add", Requests.Post)] = AddItem;
            APIs[new("/test", Requests.Get)] = Test;
        }

        private static string readBody(HttpContext context) {
            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8);
            var text = reader.ReadToEndAsync().Result;
            return text;
        }
        
        private static void AddItem(HttpContext context) {
            Console.WriteLine(context.Request.GetDisplayUrl());

            Console.WriteLine(context.Request.GetDisplayUrl());
            var text = readBody(context);
            Console.WriteLine($"{text}");

            var toAdd = JsonSerializer.Deserialize<Item>(text);
            db.Inventory.Add(toAdd);
            db.SaveChanges();
        }

        private static IEnumerable<Item> GetInventory(HttpContext context) {
            Console.WriteLine(context.Request.GetDisplayUrl());
            var inventory = db.Inventory.ToList();
            return inventory;
        }

        private static string Test(HttpContext context) {
            return "Fuck";
        }
    }
}
