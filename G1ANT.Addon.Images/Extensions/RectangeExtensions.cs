using G1ANT.Language;
using System;
using System.Drawing;

namespace G1ANT.Addon.Images
{
    public static class RectangeExtensions
    {
        public static Point GetPoint(this Rectangle rectangle, BooleanStructure centerPoint, IntegerStructure offsetX, IntegerStructure offsetY)
        {
            try
            {
                var point = (!centerPoint.Value) ? new Point(rectangle.X, rectangle.Y) : new Point(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
                return new Point(point.X + offsetX.Value, point.Y + offsetY.Value);
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not calculate the result point. Message: {ex.Message}", ex);
            }
        }
    }
}
