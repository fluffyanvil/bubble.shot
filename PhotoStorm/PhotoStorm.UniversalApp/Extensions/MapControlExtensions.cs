using System;
using Windows.Devices.Geolocation;
using Windows.UI;
using Windows.UI.Xaml.Controls.Maps;

namespace PhotoStorm.UniversalApp.Extensions
{
    public static class MapControlExtensions
    {
        public static void DrawCircle(this MapControl mapControl, Geopath geopath)
        {
            if(mapControl == null) return;
            try
            {
                mapControl.MapElements.Clear();

                var fill = Colors.LawnGreen;
                var stroke = Colors.Green;
                fill.A = 80;
                stroke.A = 100;
                var circle = new MapPolygon
                {
                    StrokeThickness = 2,
                    FillColor = fill,
                    StrokeColor = stroke,
                    StrokeDashed = false,
                    Path = geopath
                };
                mapControl.MapElements.Add(circle);
            }
            catch (Exception ex)
            {
                
                
            }
        }
    }
}
