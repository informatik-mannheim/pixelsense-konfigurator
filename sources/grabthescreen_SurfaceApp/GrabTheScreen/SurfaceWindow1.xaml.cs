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

        public Auto auto;
        public String baseString;
        private ModelVisual3D _3dModel;

        public Auto getAuto() {
            return this.auto;
        }

        public void setAuto(Auto auto){
            this.auto = auto;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow1()
        {
            InitializeComponent();
            
            _3dModel = new ModelVisual3D();
            _3dModel.Content = Display3d(MODEL_GREEN);
            
            konfig_auto.Children.Add(_3dModel);
            konfig_auto.Camera.LookDirection = new Vector3D(12.5551, -15.71341, -7.90444);
            konfig_auto.Camera.Position = new Point3D(-12.0937, 15.64731, 8.64752);
            konfig_auto.CameraChanged += new RoutedEventHandler(konfig_auto_CameraChanged); // Debug

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();
        }

        void konfig_auto_CameraChanged(object sender, RoutedEventArgs e)
        {
            
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
                konfig_auto.IsRotationEnabled = true;
                konfig_auto.IsHeadLightEnabled = true;
                
                ModelImporter import = new ModelImporter();
                
                                
                device = import.Load(model, null, false); 
            }
            catch (Exception ex)
            {
                // ignore lol
            }

            return device;
        }

        /// <summary>
        /// Occurs when the window is about to close. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Remove handlers for window availability events
            RemoveWindowAvailabilityHandlers();
        }


        /// <summary>
        /// Adds handlers for window availability events.
        /// </summary>
        private void AddWindowAvailabilityHandlers()
        {
            // Subscribe to surface window availability events
            ApplicationServices.WindowInteractive += OnWindowInteractive;
            ApplicationServices.WindowNoninteractive += OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable += OnWindowUnavailable;
        }

        /// <summary>
        /// Removes handlers for window availability events.
        /// </summary>
        private void RemoveWindowAvailabilityHandlers()
        {
            // Unsubscribe from surface window availability events
            ApplicationServices.WindowInteractive -= OnWindowInteractive;
            ApplicationServices.WindowNoninteractive -= OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable -= OnWindowUnavailable;
        }

        /// <summary>
        /// This is called when the user can interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowInteractive(object sender, EventArgs e)
        {
            //TODO: enable audio, animations here
        }

        /// <summary>
        /// This is called when the user can see but not interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowNoninteractive(object sender, EventArgs e)
        {
            //TODO: Disable audio here if it is enabled
            //TODO: optionally enable animations here
        }

        /// <summary>
        /// This is called when the application's window is not visible or interactive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowUnavailable(object sender, EventArgs e)
        {
            //TODO: disable audio, animations here
        }


        // Erzeugung der Auto-Informationen und Autobild im rechten Block
        private void SurfaceWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Auto Objekt erzeugen: Initial rot
            this.auto = new Auto();
            Random random = new Random();
            int hash = random.Next(10000, 999999999);
            this.auto.setId(hash.ToString());
            this.auto.setModel("BMW 116i 3-Türer");
            this.auto.setModelDescription("Modell Advantage");
            this.auto.setPrice("22.650 EUR");
            this.auto.setSource("Resources/small_bmw_rot.jpg");
            this.auto.setColor("Rot");
            this.auto.setStatus(false);

            // Miniaturbild (thumbnail) erzeugen
            Uri uri = new Uri(auto.getSource(), UriKind.Relative);
            BitmapImage imageBitmap = new BitmapImage(uri);
            System.Windows.Controls.Image thumbnail = new System.Windows.Controls.Image();
            thumbnail.Source = imageBitmap;
            thumbnail_car.Children.Add(thumbnail);          
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
            MongoDB.save(this.auto);
        }

        // erzeugt Tag-Bereich
        private void OnVisualizationAdded(object sender, TagVisualizerEventArgs e)
        {
            this.auto.setStatus(true);
            
            CameraVisualization camera = (CameraVisualization)e.TagVisualization;
            camera.GRABIT.Content = "Das Smartphone wurde erkannt";
            camera.myRectangle.Fill = SurfaceColors.Accent1Brush;
            camera.setAuto(this.getAuto());

            MongoDB.save(this.auto);
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

        private void btn_color_black_Click(object sender, TouchEventArgs e)
        {
            Random random = new Random();
            int hash = random.Next(10000, 999999999);
            this.auto.setId(hash.ToString());
            auto.setColor("Schwarz");
            auto.setModelDescription("Modell M Sport");
            auto.setPrice("43.850 EUR");
            auto.setSource("Resources/small_bmw_schwarz.jpg");

            // Miniaturbild (thumbnail) erzeugen
            Uri miniatur = new Uri(@"Resources\small_bmw_schwarz.jpg", UriKind.Relative);
            BitmapImage ib = new BitmapImage(miniatur);
            System.Windows.Controls.Image thumbnail = new System.Windows.Controls.Image();
            thumbnail.Source = ib;
            thumbnail_car.Children.Add(thumbnail);

            Uri uri = new Uri(@"Resources\bmw_schwarz.jpg", UriKind.Relative);
            BitmapImage imageBitmap = new BitmapImage(uri);
          //  konfig_auto.Source = imageBitmap;

            setConfLabels();
            _3dModel.Content = Display3d(MODEL_BLUE);
        }

        private void btn_color_white_Click(object sender, TouchEventArgs e)
        {
            SetToWhite();
            _3dModel.Content = Display3d(MODEL_GREEN);
        }

        private void SetToWhite()
        {
            Random random = new Random();
            int hash = random.Next(10000, 999999999);
            this.auto.setId(hash.ToString());
            auto.setColor("Weiß");
            auto.setModelDescription("Modell Sport Line");
            auto.setPrice("32.550 EUR");
            auto.setSource("Resources/small_bmw_weiß.jpg");

            // Miniaturbild (thumbnail) erzeugen
            Uri miniatur = new Uri(@"Resources\small_bmw_weiß.jpg", UriKind.Relative);
            BitmapImage ib = new BitmapImage(miniatur);
            System.Windows.Controls.Image thumbnail = new System.Windows.Controls.Image();
            thumbnail.Source = ib;
            thumbnail_car.Children.Add(thumbnail);

            Uri uri = new Uri(@"Resources\bmw_weiß.jpg", UriKind.Relative);
            BitmapImage imageBitmap = new BitmapImage(uri);
            //konfig_auto.Source = imageBitmap;

            setConfLabels();
        }

        private void btn_color_blue_Click(object sender, TouchEventArgs e)
        {
            Random random = new Random();
            int hash = random.Next(10000, 999999999);
            this.auto.setId(hash.ToString());
            auto.setColor("Blau");
            auto.setModelDescription("Modell Urban Line");
            auto.setPrice("28.950 EUR");
            auto.setSource("Resources/small_bmw_blau.jpg");

            // Miniaturbild (thumbnail) erzeugen
            Uri miniatur = new Uri(@"Resources\small_bmw_blau.jpg", UriKind.Relative);
            BitmapImage ib = new BitmapImage(miniatur);
            System.Windows.Controls.Image thumbnail = new System.Windows.Controls.Image();
            thumbnail.Source = ib;
            thumbnail_car.Children.Add(thumbnail);

            Uri uri = new Uri(@"Resources\bmw_blau.jpg", UriKind.Relative);
            BitmapImage imageBitmap = new BitmapImage(uri);
           // konfig_auto.Source = imageBitmap;

            setConfLabels();
        }

        private void btn_color_red_Click(object sender, TouchEventArgs e)
        {
            SetToRed();
        }

        private void SetToRed()
        {
            Random random = new Random();
            int hash = random.Next(10000, 999999999);
            this.auto.setId(hash.ToString());
            auto.setColor("Rot");
            auto.setModelDescription("Modell Advantage");
            auto.setPrice("22.650 EUR");
            auto.setSource("Resources/small_bmw_rot.jpg");

            // Miniaturbild (thumbnail) erzeugen
            Uri miniatur = new Uri(@"Resources\small_bmw_rot.jpg", UriKind.Relative);
            BitmapImage ib = new BitmapImage(miniatur);
            System.Windows.Controls.Image thumbnail = new System.Windows.Controls.Image();
            thumbnail.Source = ib;
            thumbnail_car.Children.Add(thumbnail);

            Uri uri = new Uri(@"Resources\bmw_rot.jpg", UriKind.Relative);
            BitmapImage imageBitmap = new BitmapImage(uri);
          //  konfig_auto.Source = imageBitmap;

            setConfLabels();
        }

        public void setConfLabels()
        {
            Label_carModel.Content = auto.getModel();
            Label_carDescription.Content = auto.getModelDescription();
            Label_carPrice.Content = auto.getPrice();
            Label_carColor.Content = auto.getColor();
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
                    SetToWhite();

                    System.Media.SystemSounds.Asterisk.Play();
                }
            }
        }

    }
}