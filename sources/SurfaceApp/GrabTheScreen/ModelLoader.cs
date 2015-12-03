using System;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace GrabTheScreen
{
    class ModelLoader
    {
        public static Model3D Load(string model)
        {
            Model3D device = null;

            try
            {
                device = new ObjReader().Read(model);
            }
            catch (Exception)
            {
                // what could possibly go wrong?
            }

            return device;
        }
    }
}
