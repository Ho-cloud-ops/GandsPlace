using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GantsPlace.Services
{
    public static class ImageHelper
    {
        /// <summary>
        /// Charge une image depuis le dossier Images/ à côté de l'exe.
        /// Retourne null si le fichier n'existe pas.
        /// </summary>
        public static ImageBrush? LoadImageBrush(string? fileName, Stretch stretch = Stretch.UniformToFill)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", fileName);
                if (!File.Exists(path)) return null;

                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.UriSource = new Uri(path, UriKind.Absolute);
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.EndInit();
                bmp.Freeze();

                return new ImageBrush(bmp) { Stretch = stretch };
            }
            catch { return null; }
        }
    }
}
