using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            catch (Exception) { }

            return device;
        }
    }
}
