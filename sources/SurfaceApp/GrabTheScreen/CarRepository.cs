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

        private Storage _storage;

        public CarRepository()
        {
            _blueCar = new BlueCar();
            _greenCar = new GreenCar();
            _storage = new Storage();
        }

        public Car FetchRemote()
        {
            var json = _storage.Load("test");
            var serializer = new JsonSerializer<CarConfigJson>();
            var carConfig = serializer.Deserialize(json);
            var color = carConfig.GetColor();
            
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
