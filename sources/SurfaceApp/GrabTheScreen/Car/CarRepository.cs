using System.Collections.Generic;
using System.Configuration;
using System.Runtime.Remoting;

namespace GrabTheScreen.Car
{
    public class CarRepository
    {
        private readonly Car _blueCar;
        private readonly Car _greenCar;

        private readonly Storage _storage;

        public CarRepository()
        {
            _blueCar = new BlueCar();
            _greenCar = new GreenCar();
            _storage = new Storage();
        }

        public Car FetchRemote()
        {
            try
            {
                var json = _storage.Load(ConfigurationManager.AppSettings.Get("storage-key-cas"));
                var serializer = new JsonSerializer<CarConfigJson>();
                var carConfig = serializer.Deserialize(json);
                var color = carConfig.GetColor();

                return color == "Grün" ? GetGreenCar() : GetBlueCar();
            }
            catch (ServerException e) { Logger.Log(e.Message); }
            catch (KeyNotFoundException e) { Logger.Log(e.Message); }

            return Car.Unknown;
        }

        public void StoreRemote(Car car)
        {
            var config = CarConfigJson.Default();
            config.SetColor(car.Color == "Grün" ? "#B8F07B" : "#1872B0"); // because 3m5 already accepts hex codes instead of "Blau" or "Grün" strings.
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
