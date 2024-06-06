using Microsoft.AspNetCore.Http.Extensions;
using System.Text.Json;
using System.Text;
using Server.Models;
using Aspose.BarCode.BarCodeRecognition;

namespace Server {
    public class Program {
        private const string AllowCors = "AllowCors";
        private enum Requests {
            Get,
            Post
        }
        private static Dictionary<Tuple<string, Requests>, Delegate> APIs = new();
        private static InventoryContext db;
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddCors(options => {
                options.AddPolicy(name: AllowCors, builder => {
                    builder.WithOrigins("http://localhost:3000")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });
            var app = builder.Build();
            app.UseCors(AllowCors);

            InitDB();

            Console.WriteLine("Building");
            BuildAPIs();

            LoadAPIS(app);

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
                    app.MapGet(name, APIs[api]).RequireCors(AllowCors);
                }
                else if (method == Requests.Post) {
                    app.MapPost(name, APIs[api]).RequireCors(AllowCors);
                }
            }
        }

        private static void BuildAPIs() {
            APIs[new("/inventory", Requests.Get)] = GetInventory;
            APIs[new("/add", Requests.Post)] = AddItem;
            APIs[new("/test", Requests.Get)] = Test;
            APIs[new("/barcode", Requests.Post)] = ReadBarcode;
        }

        private static string ReadBarcode(HttpContext context) {
            Console.WriteLine(context.Request.GetDisplayUrl());
            var body = readBody(context);
            var response = "{\"count\":0}";

            var image = JsonSerializer.Deserialize<JsonImage>(body);

            var reader = new BarCodeReader(image.GetBitmap(), DecodeType.Code39Extended);

            Console.WriteLine(reader.ReadBarCodes().Length);

            foreach(var code in reader.ReadBarCodes()) {
                Console.WriteLine(code);
            }

            return response;
        }
  

        private static string readBody(HttpContext context) {
            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8);
            var text = reader.ReadToEndAsync().Result;
            return text;
        }

        private static void AddItem(HttpContext context) {
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
