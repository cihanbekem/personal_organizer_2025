using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace po
{
    public static class ImageHelper
    {
        public static string ConvertImageToBase64(string imagePath)
        {
            try
            {
                if (!File.Exists(imagePath))
                {
                    Console.WriteLine("File not found.");
                    return null;
                }

                using (Image image = Image.FromFile(imagePath))
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] imageBytes = ms.ToArray();
                    return Convert.ToBase64String(imageBytes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting image to base64: {ex.Message}");
                return null;
            }
        }
        public static Image ConvertBase64ToImage(string base64String)
        {
            try
            {
                byte[] imageBytes = Convert.FromBase64String(base64String);
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    Image image = Image.FromStream(ms);
                    return image;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting base64 to image: {ex.Message}");
                return null;
            }
        }


        // Varsayılan fotoğrafın base64 formatındaki değerini getiren metod
        public static string GetDefaultPhoto()
        {
            string defaultImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Services", "assets", "defaultProfile.jpg");
            if (File.Exists(defaultImagePath))
            {
                return ConvertImageToBase64(defaultImagePath);
            }
            return null; // Varsayılan resim bulunamazsa null döner
        }
    }
}
