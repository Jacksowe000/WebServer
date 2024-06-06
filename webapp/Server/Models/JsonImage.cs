using Aspose.Drawing;

namespace Server.Models {
    public class JsonImage {
        public string data {  get; set; }
        public Bitmap GetBitmap() {

            byte[] array = Convert.FromBase64String(data);
            using var ms = new MemoryStream(array);

            var img = new Bitmap(ms);
            return img;
        }
    }
}
