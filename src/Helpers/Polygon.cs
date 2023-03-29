using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Draw.src.Helpers
{
    static class Polygon
    {
        public static bool IsPointInPolygon(PointF[] polygon, PointF point)
        {
            bool isInside = false;
            int j = polygon.Length - 1;

            for (int i = 0; i < polygon.Length; i++)
            {
                if (polygon[i].Y < point.Y && polygon[j].Y >= point.Y || polygon[j].Y < point.Y && polygon[i].Y >= point.Y)
                {
                    if (polygon[i].X + (point.Y - polygon[i].Y) / (double)(polygon[j].Y - polygon[i].Y) * (polygon[j].X - polygon[i].X) < point.X)
                    {
                        isInside = !isInside;
                    }
                }

                j = i;
            }

            return isInside;
        }

        public static float[] DataForRectangleContour(PointF[] points)
        {
            float x = int.MaxValue;
            float y = int.MaxValue;
            float width = 0;
            float height = 0;
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i].X < x)
                    x = points[i].X;

                if (points[i].Y < y)
                    y = points[i].Y;

                for (int j = i + 1; j < points.Length; j++)
                {
                    if (Math.Abs(points[j].X - points[i].X) > width)
                        width = Math.Abs(points[j].X - points[i].X);

                    if (Math.Abs(points[j].Y - points[i].Y) > height)
                        height = Math.Abs(points[j].Y - points[i].Y);
                }
            }
            float[] result = { x, y, width, height };
            return result;
        }
        
    }
}
