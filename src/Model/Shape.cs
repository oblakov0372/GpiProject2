using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Draw
{
    /// <summary>
    /// Базовия клас на примитивите, който съдържа общите характеристики на примитивите.
    /// </summary>
    [Serializable]
    public abstract class Shape
    {
        #region Constructors

        public Shape()
        {
        }

        public Shape(RectangleF rect)
        {
            rectangle = rect;
        }

        public Shape(Shape shape)
        {
            this.Height = shape.Height;
            this.Width = shape.Width;
            this.Location = shape.Location;
            this.rectangle = shape.rectangle;

            this.FillColor = shape.FillColor;
            this.BorderColor = shape.BorderColor;
            this.BorderWidth = shape.BorderWidth;
            this.Transparency = shape.Transparency;
            this.matrix = shape.matrix;
            this.contour = shape.contour;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Обхващащ правоъгълник на елемента.
        /// </summary>
        private RectangleF rectangle;
        public virtual RectangleF Rectangle
        {
            get { return rectangle; }
            set { rectangle = value; }
        }

        /// <summary>
        /// Широчина на елемента.
        /// </summary>
        public virtual float Width
        {
            get { return Rectangle.Width; }
            set { rectangle.Width = value; }
        }

        /// <summary>
        /// Височина на елемента.
        /// </summary>
        public virtual float Height
        {
            get { return Rectangle.Height; }
            set { rectangle.Height = value; }
        }

        /// <summary>
        /// Горен ляв ъгъл на елемента.
        /// </summary>
        public virtual PointF Location
        {
            get { return Rectangle.Location; }
            set { rectangle.Location = value; }
        }

        /// <summary>
        /// Цвят на елемента.
        /// </summary>
        private Color fillColor = Color.Transparent;
        public virtual Color FillColor
        {
            get { return fillColor; }
            set { fillColor = value; }
        }

        /// <summary>
        /// Цвят на контур.
        /// </summary>
        private Color borderColor = Color.Black;
        public virtual Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; }
        }

        /// <summary>
        /// Дебелина на контур.
        /// </summary>
        private float borderWidth = 1;
        public virtual float BorderWidth
        {
            get { return borderWidth; }
            set { borderWidth = value; }
        }

        /// <summary>
        /// Прозрачноста на обекта.
        /// </summary>
        private float transparency = 100;
        public virtual float Transparency
        {
            get { return transparency; }
            set { transparency = value; }
        }

        private Matrix matrix = new Matrix();
        public virtual Matrix Matrix
        {
            get { return matrix; }
            set { matrix = value; }
        }


        private Rectangle contour = new Rectangle();
        public virtual Rectangle Contour
        {
            get { return contour; }
            set { contour = value; }
        }

        #endregion


        /// <summary>
        /// Проверка дали точка point принадлежи на елемента.
        /// </summary>
        /// <param name="point">Точка</param>
        /// <returns>Връща true, ако точката принадлежи на елемента и
        /// false, ако не пренадлежи</returns>
        public virtual bool Contains(PointF point)
        {
            Rectangle rectangle = new Rectangle((int)(Rectangle.X - borderWidth / 2),
                                      (int)(Rectangle.Y - borderWidth / 2),
                                      (int)(Rectangle.Width + borderWidth * 2),
                                      (int)(Rectangle.Height + borderWidth * 2));

            return rectangle.Contains((int)point.X, (int)point.Y);
        }

        /// <summary>
        /// Визуализира елемента.
        /// </summary>
        /// <param name="grfx">Къде да бъде визуализиран елемента.</param>
        public virtual void DrawSelf(Graphics grfx)
        {
            FillColor = Color.FromArgb((int)(Transparency / 100 * 255), FillColor);
            BorderColor = Color.FromArgb((int)(Transparency / 100 * 255), BorderColor);
            // shape.Rectangle.Inflate(shape.BorderWidth, shape.BorderWidth);
        }


        public virtual void SetContour(Graphics grfx)
        {
            float retreat = BorderWidth / 2 + 3;

            PointF[] array = { new PointF(Location.X, Location.Y), new PointF(Location.X + Width, Location.Y + Height) };

            var store = grfx.Save();
            grfx.Transform = Matrix;

            Contour = new Rectangle((int)(array[0].X - retreat),
                                            (int)(array[0].Y - retreat),
                                            (int)(array[1].X - array[0].X + retreat * 2),
                                            (int)(array[1].Y - array[0].Y + retreat * 2));

            grfx.Restore(store);
        }
    }
}
