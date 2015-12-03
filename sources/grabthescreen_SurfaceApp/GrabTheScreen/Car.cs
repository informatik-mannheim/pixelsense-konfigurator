using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace GrabTheScreen
{
    public class Car
    {
        public String model;
        public String modelDescription;
        public String price;
        public String source;
        public String id;
        public String color;
        public Boolean status;

        public Car(String car_model, String car_modelDescription, String car_price, String car_source, String car_color, Boolean car_status)
        {
            this.model = car_model;
            this.modelDescription = car_modelDescription;
            this.price = car_price;
            this.source = car_source;
            this.color = car_color;
            this.status = car_status;
            this.id = new Random().Next(10000, 999999999).ToString();
        }

        public Car() { }

        public void setModel(String model)
        {
            this.model = model;
        }

        public String getModel()
        {
            return this.model;
        }

        public void setModelDescription(String modelDescription)
        {
            this.modelDescription = modelDescription;
        }

        public String getModelDescription()
        {
            return this.modelDescription;
        }

        public void setPrice(String price)
        {
            this.price = price;
        }

        public String getPrice()
        {
            return this.price;
        }

        public void setSource(String source)
        {
            this.source = source;
        }

        public String getSource()
        {
            return this.source;
        }


        public void setId(String id)
        {
            this.id = id;
        }

        public String getId()
        {
            return this.id;
        }

        public String getColor()
        {
            return this.color;
        }

        public void setColor(String color) { 
            this.color = color;
        }

        public Boolean getStatus() {
            return this.status;
        }

        public void setStatus(Boolean status) {
            this.status = status;
        }

        public Image CreateThumbnail()
        {
            Uri pathToThumbnail = new Uri(getSource(), UriKind.Relative);
            return new Image() { Source = new BitmapImage(pathToThumbnail) };
        }
    }

    class BlueCar : Car
    {
        public BlueCar()
            : base("BMW 116i 3-Türer", "Model Advantage", "22.650 EUR", @"Resources\blue.PNG", "Blau", false)
        { }
    }

    class GreenCar : Car
    {
        public GreenCar()
            : base("BMW 116i 3-Türer", "Modell Sport Line", "32.550 EUR", @"Resources\green.PNG", "Grün", false)
        { }
    }
}
