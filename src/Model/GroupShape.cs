using Draw.src.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;

namespace Draw.src.Model
{
    [Serializable]
    public class GroupShape : Shape
    {
        public GroupShape(RectangleF groupShape) : base(groupShape)
        {
        }
        public GroupShape(GroupShape groupShape) : base(groupShape)
        {
            this.shapes = groupShape.shapes;
        }
        #region Properties

        private List<Shape> shapes = new List<Shape>();
        public List<Shape> Shapes
        {
            get { return shapes; }
            set { shapes = value; }
        }
        public override Color BorderColor
        {
            get => base.BorderColor;
            set
            {
                base.BorderColor = value;
                Shapes.ForEach(s => s.BorderColor = value);
            }
        }
        public override Color FillColor
        {
            get => base.FillColor;
            set
            {
                base.FillColor = value;
                Shapes.ForEach(s => s.FillColor = FillColor);
            }
        }
        public override float Transparency
        {
            get => base.Transparency;
            set
            {
                base.Transparency = value;
                Shapes.ForEach(s => s.Transparency = Transparency);
            }
        }
        public override float BorderWidth
        {
            get => base.BorderWidth;
            set
            {
                base.BorderWidth = value;
                Shapes.ForEach(s => s.BorderWidth = BorderWidth);
            }
        }
        public override PointF Location
        {
            get => base.Location;
            set
            {
                float x = value.X - Location.X;
                float y = value.Y - Location.Y;
                shapes.ForEach(s => s.Location = new PointF(s.Location.X + x, s.Location.Y + y));
                base.Location = value;
            }
        }

        public override Matrix Matrix
        {
            get => base.Matrix;
            set
            {
                base.Matrix = value;
                shapes.ForEach(s => s.Matrix = value);
            }
        }
        #endregion

        public override void DrawSelf(Graphics grfx)
        {
            base.DrawSelf(grfx);
            var store = grfx.Save();
            foreach (var item in shapes)
            {
                item.DrawSelf(grfx);
            }
            grfx.Restore(store);

        }

        public override void SetContour(Graphics grfx)
        {
            grfx.Transform = Matrix;
            var state = grfx.Save();
            foreach (var item in shapes)
            {
                item.SetContour(grfx);
            }
            Contour = ListShapeHelper.FindEnclosingRectangle(shapes);
            grfx.Restore(state);
        }
        public override bool Contains(PointF point)
        {
            PointF[] array = new PointF[1] { point };
            Matrix.Invert();
            Matrix.TransformPoints(array);
            Matrix.Invert();
            Contour = ListShapeHelper.FindEnclosingRectangle(shapes);
            return Contour.Contains((int)array[0].X, (int)array[0].Y);
        }
    }
}
