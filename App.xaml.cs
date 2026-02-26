using System.Windows;
using GantsPlace.Services;

namespace GantsPlace
{
    public partial class App : Application
    {
        public App()
        {
            // Charger les salles au démarrage de l'application
            DataService.LoadSalles();
        }
    }
}
