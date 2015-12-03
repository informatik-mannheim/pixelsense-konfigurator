using System;
using System.Configuration;

namespace GrabTheScreen
{
    public class Tags
    {
        public static bool IsTablet(long tagValue) 
        {
            return Convert.ToUInt32(ConfigurationManager.AppSettings.Get("tag-tablet"), 16) == tagValue;            
        }

        public static bool IsGlasses(long tagValue) 
        {
            return Convert.ToUInt32(ConfigurationManager.AppSettings.Get("tag-glasses"), 16) == tagValue;
        }
    }
}
