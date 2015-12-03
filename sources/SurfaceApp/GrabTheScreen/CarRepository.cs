using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrabTheScreen
{
    public class CarRepository
    {
        private Car _blueCar;
        private Car _greenCar;

        public CarRepository()
        {
            _blueCar = new BlueCar();
            _greenCar = new GreenCar();
        }

        public Car FetchRemote()
        {
            Storage storage = new Storage();
            var json = storage.Load("test");
            var serializer = new JsonSerializer<CarConfigJson>();
            var carConfig = serializer.Deserialize(json);
            var color = carConfig.product.attributeGroups.Single(ag => ag.name == "Exterior").attributes.Single(at => at.name=="Farbe").selectedValues[0];
            if (color == "Grün")
            {
                return GetGreenCar();
            }
            else
            {
                return GetBlueCar();
            }
        }

        public Car GetBlueCar()
        {
            return _blueCar;
        }

        public Car GetGreenCar()
        {
            return _greenCar;
        }
    }
}
