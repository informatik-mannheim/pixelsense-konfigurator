using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace GrabTheScreen.Car
{
    /// <summary>
    /// Domain model for the car. Holds the 3D model, a thumbnail of the car and all the meta information. 
    /// </summary>
    public class Car
    {
        public string Model { get; private set; }
        public string ModelDescription { get; private set; }
        public string Price { get; private set; }
        public string Source { get; private set; }
        public string Id { get; private set; }
        public string Color { get; private set; }
        public Model3D Model3D { get; protected set; }

        public Car(string carModel, string carModelDescription, string carPrice, string carSource, string carColor)
        {
            Model = carModel;
            ModelDescription = carModelDescription;
            Price = carPrice;
            Source = carSource;
            Color = carColor;
            Id = new Random().Next(10000, 999999999).ToString();
        }

        public Car() { }

        public Image CreateThumbnail()
        {
            var pathToThumbnail = new Uri(Source, UriKind.Relative);
            return new Image { Source = new BitmapImage(pathToThumbnail) };
        }
    }

    class BlueCar : Car
    {
        private const string MODEL_BLUE = @"Resources\auto_blau.obj";

        public BlueCar()
            : base("BMW 116i 3-Türer", "Model Advantage", "22.650 EUR", @"Resources\blue.PNG", "Blau")
        {
            Model3D = ModelLoader.Load(MODEL_BLUE);
        }
    }

    class GreenCar : Car
    {
        private const string MODEL_GREEN = @"Resources\auto_gruen.obj";

        public GreenCar()
            : base("BMW 116i 3-Türer", "Modell Sport Line", "32.550 EUR", @"Resources\green.PNG", "Grün")
        {
            Model3D = ModelLoader.Load(MODEL_GREEN);
        }
    }
}
