using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server {
    public class InventoryContext : DbContext {
        public DbSet<Item> Inventory { get; set; }
        public string DbPath { get; set; }

        public InventoryContext() {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "Inventory.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }

    [PrimaryKey(nameof(UPC))]
    public class Item {
        public Item() {
            name = "";
            UPC = 0;
            quantity = 0;
        }

        public Item(string name, ulong uPC, uint quantity) {
            this.name = name;
            UPC = uPC;
            this.quantity = quantity;
        }

        public Item(string name, ulong uPC) {
            this.name = name;
            UPC = uPC;
            quantity = 0;
        }

        public string name { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong UPC { get; set; }

        public uint quantity { get; set; }
    }
}
