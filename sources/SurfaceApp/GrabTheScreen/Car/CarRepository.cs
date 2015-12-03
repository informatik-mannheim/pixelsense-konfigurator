using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

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
            var json = _storage.Load(ConfigurationManager.AppSettings.Get("storage-key-cas"));
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

        public void StoreRemote(Car car)
        {
            var config = CarConfigJson.Default();
            config.SetColor(car.Color);
            var serializer = new JsonSerializer<CarConfigJson>();
            var json = serializer.Serialize(config);
            _storage.Save(ConfigurationManager.AppSettings.Get("storage-key-3m5"), json);
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
