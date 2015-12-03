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

            setConfLabels();
        }

        // Methode, die aufgerufen wird bei Klick auf "grab it" Button
        private void btn_grabIt_Click(object sender, RoutedEventArgs e)
        {
            // damit Miniatur-Bild erst zur Laufzeit angezeigt wird
           // placeholder_smartphone.Children.Clear();

            // Erstellen des Vizualizer's
            TagVisualizer visualizer = new TagVisualizer();
            visualizer.Name = "MyTagVisualizer";

            // Visualization Definitionen
            TagVisualizationDefinition tagDefinition = new TagVisualizationDefinition();

            // Tag Value 0x18 - wichtig für Input Simulator
            tagDefinition.Value = "0x18";
            tagDefinition.Source = new Uri("CameraVisualization.xaml", UriKind.Relative);
            tagDefinition.LostTagTimeout = 2000;
            tagDefinition.MaxCount = 2;
            tagDefinition.OrientationOffsetFromTag = 0;
            tagDefinition.TagRemovedBehavior = TagRemovedBehavior.Disappear;
            tagDefinition.UsesTagOrientation = true;
            
            // Definitionen dem Visualizer hinzufügen
            visualizer.Definitions.Add(tagDefinition);
            visualizer.VisualizationAdded += OnVisualizationAdded;

            // Miniaturbild auf gts-Fläche
            System.Windows.Controls.Image newImage = new System.Windows.Controls.Image();
           // newImage.Source = konfig_auto.Source;
            Thickness margin = newImage.Margin;
            margin.Left = 20;
            margin.Right = 20;
            newImage.Margin = margin;

            // zur Laufzeit Visualizer erzeugen
            placeholder_smartphone.Children.Add(visualizer);

            hierAuflegen.Visibility = System.Windows.Visibility.Visible;
          
            // WPF-Image zu Drawing-Image konvertieren
            System.Drawing.Image drawingImage = ImageHelper.ConvertWpfImageToImage(newImage);
            baseString = ImageHelper.ToBase64String(drawingImage);

            // setzt status des Datensatzes in DB auf false zunächst
            btn_grabIt.IsEnabled = false;
          //  MongoDB.save(this.auto);
        }

        // erzeugt Tag-Bereich
        private void OnVisualizationAdded(object sender, TagVisualizerEventArgs e)
        {
            _car.setStatus(true);
            
            CameraVisualization camera = (CameraVisualization)e.TagVisualization;
            camera.GRABIT.Content = "Das Smartphone wurde erkannt";
            camera.myRectangle.Fill = SurfaceColors.Accent1Brush;
            camera.setAuto(getAuto());
        }

        public void setConfLabels()
        {
            Label_carModel.Content = _car.getModel();
            Label_carDescription.Content = _car.getModelDescription();
            Label_carPrice.Content = _car.getPrice();
            Label_carColor.Content = _car.getColor();
            hierAuflegen.Visibility = System.Windows.Visibility.Hidden;

            btn_grabIt.IsEnabled = true;
        }

        private void SurfaceWindow_TouchDown(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice.GetIsTagRecognized())
            {
                TagData tagData = e.TouchDevice.GetTagData();
                if (tagData.Value == 0x1)
                {
                    ChangeCar(new GreenCar(), _greenCar);

                    System.Media.SystemSounds.Asterisk.Play();
                }
            }
        }
    }
}