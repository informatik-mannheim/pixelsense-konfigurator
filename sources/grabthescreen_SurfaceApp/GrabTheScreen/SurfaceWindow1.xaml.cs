using System;
using System.Configuration;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using System.Security.Cryptography;
using System.Windows.Media.Media3D;

using HelixToolkit.Wpf;

namespace GrabTheScreen
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class SurfaceWindow1 : SurfaceWindow
    {
        private const bool SHOW_POSITION = false;

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

            _blueCar = Display3d(MODEL_BLUE);
            _greenCar = Display3d(MODEL_GREEN);
            
            _3dModel.Content = _blueCar;
            
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

        private const string MODEL_BLUE = @"Resources\auto_blau.obj";
        private const string MODEL_GREEN = @"Resources\auto_gruen.obj";
        private Model3D Display3d(string model)
        {
            Model3D device = null;
            try
            {
                konfig_auto.RotateGesture = new MouseGesture(MouseAction.LeftClick);
                konfig_auto.CameraRotationMode = CameraRotationMode.Turnball;
                
                device = new ObjReader().Read(model);
            }
            catch (Exception ex)
            {
                // ignore lol
            }

            return device;
        }

        // Erzeugung der Auto-Informationen und Autobild im rechten Block
        private void SurfaceWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ChangeCar(new BlueCar(), _blueCar);
        }

        // Ausgabe der Auto-Informationen im Rechten Block 
        public void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            var grid = sender as Grid;
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
            System.Drawing.Image drawingImage = ConvertWpfImageToImage(newImage);
            baseString = GetStringFromImage(drawingImage);

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


        // kodiert Image in Base64 String
        public static string GetStringFromImage(System.Drawing.Image image)
        {
            if (image != null)
            {
                ImageConverter ic = new ImageConverter();
                byte[] buffer = (byte[])ic.ConvertTo(image, typeof(byte[]));
                return Convert.ToBase64String(
                    buffer,
                    Base64FormattingOptions.InsertLineBreaks);
            }
            else
                return null;
        }

        // Methode zur Konvertierung von System.Windows.Control.Image in System.Drawing
        public static System.Drawing.Image ConvertWpfImageToImage(System.Windows.Controls.Image image)
        {
            if (image == null)
                throw new ArgumentNullException("image", "Image darf nicht null sein.");

            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            MemoryStream ms = new MemoryStream();
            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)image.Source));
            encoder.Save(ms);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
            return img;
        }

        private void btn_color_blue_Click(object sender, TouchEventArgs e)
        {
            ChangeCar(new BlueCar(), _blueCar);
        }

        private void btn_color_green_Click(object sender, TouchEventArgs e)
        {
            ChangeCar(new GreenCar(), _greenCar);
        }

        private void ChangeCar(Car car, Model3D model)
        {
            _car = car;
            thumbnail_car.Children.Add(_car.CreateThumbnail());
            _3dModel.Content = model;

            setConfLabels();
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