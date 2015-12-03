using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using System.Windows.Media.Media3D;

using HelixToolkit.Wpf;

#pragma warning disable 0162 // do not warn about unreachable code

namespace GrabTheScreen
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class SurfaceWindow1 : SurfaceWindow
    {
        private const bool SHOW_POSITION = false;
        private const string MODEL_BLUE = @"Resources\auto_blau.obj";
        private const string MODEL_GREEN = @"Resources\auto_gruen.obj";

        public event EventHandler<TouchEventArgs> TabletRecognized;
        public event EventHandler<TouchEventArgs> GlassesRecognized;

        public Car _car;
        public String baseString;
        private ModelVisual3D _3dModel;

        private Model3D _blueCar;
        private Model3D _greenCar;

        public Car getAuto()
        {
            return this._car;
        }

        public void setAuto(Car auto)
        {
            this._car = auto;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow1()
        {
            InitializeComponent();

            TabletRecognized += OnTabletRecognized;
            GlassesRecognized += OnGlassesRecognized;

            _3dModel = (ModelVisual3D)FindName("myModel");

            _blueCar = ModelLoader.Load(MODEL_BLUE);
            _greenCar = ModelLoader.Load(MODEL_GREEN);
            
            konfig_auto.RotateGesture = new MouseGesture(MouseAction.LeftClick);
            konfig_auto.CameraRotationMode = CameraRotationMode.Turnball;
            konfig_auto.Camera.LookDirection = new Vector3D(12.5551, -15.71341, -7.90444);
            konfig_auto.Camera.Position = new Point3D(-12.0937, 15.64731, 8.64752);
            konfig_auto.CameraChanged += new RoutedEventHandler(konfig_auto_CameraChanged); // Debug
        }

        void konfig_auto_CameraChanged(object sender, RoutedEventArgs e)
        {
            if (!SHOW_POSITION) return;
            
            var x = konfig_auto.Camera.Position;
            var y = konfig_auto.Camera.LookDirection;
            Console.WriteLine("Position: " + x.ToString() + ", LookDirection: " + y.ToString());
        }


        /// <summary>
        /// Invoked When The Window Is Being Loaded. Default Car is displayed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SurfaceWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ChangeCar(new BlueCar(), _blueCar);
        }

        private void btn_color_blue_Click(object sender, TouchEventArgs e)
        {
            ChangeCar(new BlueCar(), _blueCar);
        }

        private void btn_color_green_Click(object sender, TouchEventArgs e)
        {
            ChangeCar(new GreenCar(), _greenCar);
        }

        /// <summary>
        /// Set new car to be displayed.
        /// </summary>
        /// <param name="car">The car information.</param>
        /// <param name="model">The 3D model of the car.</param>
        private void ChangeCar(Car car, Model3D model)
        {
            _car = car;
            thumbnail_car.Children.Add(_car.CreateThumbnail());
            _3dModel.Content = model;

            setCarInformation();
        }

        public void setCarInformation()
        {
            lblCarModel.Content = _car.getModel();
            lblCarDescription.Content = _car.getModelDescription();
            lblCarPrice.Content = _car.getPrice();
            lblCarColor.Content = _car.getColor();
        }

        private void OnTabletRecognized(Object sender, TouchEventArgs e)
        {
            ChangeCar(new GreenCar(), _greenCar);
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void OnGlassesRecognized(Object sender, TouchEventArgs e)
        {
            // Todo: Implement
        }

        private void SurfaceWindow_TouchDown(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice.GetIsTagRecognized())
            {
                TagData tagData = e.TouchDevice.GetTagData();

                if (tagData.Value == 0x1)
                {
                    TabletRecognized(sender, e);
                }
                else if (tagData.Value == 0x2)
                {
                    GlassesRecognized(sender, e);
                }
            }
        }
    }
}