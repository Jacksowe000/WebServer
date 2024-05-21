namespace webapp.Server {
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
            this.name=name;
            UPC = uPC;
            quantity = 0;
        }

        public string name { get; set; }
        public ulong UPC {  get; set; }

        public uint quantity { get; set; }

    }
}
