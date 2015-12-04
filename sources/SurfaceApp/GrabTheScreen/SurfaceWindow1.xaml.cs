using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Surface.Presentation.Input;
using System.Windows.Media.Media3D;
using GrabTheScreen.Car;
using HelixToolkit.Wpf;

#pragma warning disable 0162 // do not warn about unreachable code

namespace GrabTheScreen
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class SurfaceWindow1
    {
        public event EventHandler<TouchEventArgs> TabletRecognized;
        public event EventHandler<TouchEventArgs> GlassesRecognized;

        private Car.Car _car;
        private readonly ModelVisual3D _3DModel;
        private readonly CarRepository _carRepository;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow1()
        {
            InitializeComponent();

            TabletRecognized += OnTabletRecognized;
            GlassesRecognized += OnGlassesRecognized;

            _carRepository = new CarRepository();
            _3DModel = (ModelVisual3D)FindName("myModel");

            konfig_auto.RotateGesture = new MouseGesture(MouseAction.LeftClick);
            konfig_auto.CameraRotationMode = CameraRotationMode.Trackball;
            konfig_auto.Camera.LookDirection = new Vector3D(-6.97701085101471, 16.0827908414958, -12.6367369160405);
            konfig_auto.Camera.Position = new Point3D(7.43841085101471, -16.1488908414958, 13.3798169160405);
        }

        public void EnableCameraDebug()
        {
            konfig_auto.Camera.Changed +=
               (sender, args) =>
                   Console.WriteLine(konfig_auto.Camera.LookDirection + @", " + konfig_auto.Camera.Position);
        }

        /// <summary>
        /// Invoked When The Window Is Being Loaded. Default Car is displayed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SurfaceWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ChangeCar(_carRepository.GetBlueCar());
        }

        private void btn_color_blue_Click(object sender, TouchEventArgs e)
        {
            ChangeCar(_carRepository.GetBlueCar());
        }

        private void btn_color_green_Click(object sender, TouchEventArgs e)
        {
            ChangeCar(_carRepository.GetGreenCar());
        }

        private void OnTabletRecognized(object sender, TouchEventArgs e)
        {
            ChangeCar(_carRepository.FetchRemote());
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void OnGlassesRecognized(object sender, TouchEventArgs e)
        {
            _carRepository.StoreRemote(_car);
            System.Media.SystemSounds.Asterisk.Play();
        }

        /// <summary>
        /// Set new car to be displayed.
        /// </summary>
        /// <param name="car">The car information.</param>
        private void ChangeCar(Car.Car car)
        {
            _car = car;
            thumbnail_car.Children.Add(_car.CreateThumbnail());
            _3DModel.Content = _car.Model3D;

            SetCarInformation();
        }

        public void SetCarInformation()
        {
            lblCarModel.Content = _car.Model;
            lblCarDescription.Content = _car.ModelDescription;
            lblCarPrice.Content = _car.Price;
            lblCarColor.Content = _car.Color;
        }

        private void SurfaceWindow_TouchDown(object sender, TouchEventArgs e)
        {
            if (!e.TouchDevice.GetIsTagRecognized())
            {
                return;
            }

            var tagData = e.TouchDevice.GetTagData();

            if (Tags.IsTablet(tagData.Value))
            {
                if (TabletRecognized != null) TabletRecognized(sender, e);
            }
            else if (Tags.IsGlasses(tagData.Value))
            {
                if (GlassesRecognized != null) GlassesRecognized(sender, e);
            }
        }
    }
}