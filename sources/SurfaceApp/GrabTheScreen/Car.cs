using System;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace GrabTheScreen
{
    public class Car
    {
        public String Model { get; private set; }
        public String ModelDescription { get; private set; }
        public String Price { get; private set; }
        public String Source { get; private set; }
        public String ID { get; private set; }
        public String Color { get; private set; }
        public Model3D Model3D { get; protected set; }

        public Car(String car_model, String car_modelDescription, String car_price, String car_source, String car_color)
        {
            Model = car_model;
            ModelDescription = car_modelDescription;
            Price = car_price;
            Source = car_source;
            Color = car_color;
            ID = new Random().Next(10000, 999999999).ToString();
        }

        public Car() { }

        public Image CreateThumbnail()
        {
            Uri pathToThumbnail = new Uri(Source, UriKind.Relative);
            return new Image() { Source = new BitmapImage(pathToThumbnail) };
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
