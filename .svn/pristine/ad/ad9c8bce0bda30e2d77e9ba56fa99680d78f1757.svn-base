using Draw.src.Model;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Draw
{
    /// <summary>
    /// Върху главната форма е поставен потребителски контрол,
    /// в който се осъществява визуализацията
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Агрегирания диалогов процесор във формата улеснява манипулацията на модела.
        /// </summary>
        private DialogProcessor dialogProcessor = new DialogProcessor();

        public MainForm()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }

        /// <summary>
        /// Изход от програмата. Затваря главната форма, а с това и програмата.
        /// </summary>
        void ExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Събитието, което се прихваща, за да се превизуализира при изменение на модела.
        /// </summary>
        void ViewPortPaint(object sender, PaintEventArgs e)
        {
            dialogProcessor.ReDraw(sender, e);
        }

        /// <summary>
        /// Бутон, който поставя на произволно място правоъгълник със зададените размери.
        /// Променя се лентата със състоянието и се инвалидира контрола, в който визуализираме.
        /// </summary>
        void DrawRectangleSpeedButtonClick(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomRectangle();

            statusBar.Items[0].Text = "Последно действие: Рисуване на правоъгълник";

            viewPort.Invalidate();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
            {
                if (dialogProcessor.SelectionsList.Count != 0)
                {
                    dialogProcessor.CopiedShapes.Clear();
                    foreach (var shape in dialogProcessor.SelectionsList)
                    {
                        dialogProcessor.CopiedShapes.Add(shape);
                    }
                }
            }
            if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
            {
                foreach (var shape in dialogProcessor.CopiedShapes)
                {
                    if (shape.GetType() == typeof(RectangleShape))
                    {
                        RectangleShape rect = new RectangleShape((RectangleShape)shape);
                        dialogProcessor.ShapeList.Add(rect);
                    }
                    else if (shape.GetType() == typeof(ElipseShape))
                    {
                        ElipseShape elipse = new ElipseShape((ElipseShape)shape);
                        dialogProcessor.ShapeList.Add(elipse);
                    }
                    else if (shape.GetType() == typeof(StarShape))
                    {
                        StarShape star = new StarShape((StarShape)shape);
                        dialogProcessor.ShapeList.Add(star);
                    }
                    else if (shape.GetType() == typeof(GroupShape))
                    {
                        GroupShape group = new GroupShape((GroupShape)shape);
                        dialogProcessor.ShapeList.Add(group);
                    }
                }
            }
            if (e.KeyCode == Keys.A && e.Modifiers == Keys.Control)
            {
                foreach (var item in dialogProcessor.ShapeList)
                {
                    if (!dialogProcessor.SelectionsList.Contains(item))
                        dialogProcessor.SelectionsList.Add(item);

                    if (dialogProcessor.SelectionsList.Count > 1)
                        groupElement.Enabled = true;
                }
            }
            if (e.KeyCode == Keys.Delete)
            {
                for (int i = 0; i < dialogProcessor.SelectionsList.Count; i++)
                {
                    dialogProcessor.ShapeList.Remove(dialogProcessor.SelectionsList[i]);
                }
                dialogProcessor.SelectionsList.Clear();
            }
            viewPort.Invalidate();
        }
        /// <summary>
        /// Прихващане на координатите при натискането на бутон на мишката и проверка (в обратен ред) дали не е
        /// щракнато върху елемент. Ако е така то той се отбелязва като селектиран и започва процес на "влачене".
        /// Промяна на статуса и инвалидиране на контрола, в който визуализираме.
        /// Реализацията се диалогът с потребителя, при който се избира "най-горния" елемент от екрана.
        /// </summary>
        void ViewPortMouseDown(object sender, MouseEventArgs e)
        {
            Shape shapeForList = dialogProcessor.ContainsPoint(e.Location);
            if (pickUpSpeedButton.Checked)
            {
                if (shapeForList != null)
                {
                    //multiply selection with pressed ctrl button
                    if ((ModifierKeys & Keys.Control) == Keys.Control)
                    {
                        if (dialogProcessor.SelectionsList.Contains(shapeForList))
                            dialogProcessor.SelectionsList.Remove(shapeForList);

                        dialogProcessor.SelectionsList.Add(shapeForList);
                    }
                    //single selection
                    else
                    {
                        dialogProcessor.SelectionsList.Clear();
                        dialogProcessor.SelectionsList.Add(shapeForList);
                        textBoxForBorderWidth.Text = dialogProcessor.SelectionsList[0].BorderWidth.ToString();
                        trackBarTransperency.Value = (int)dialogProcessor.SelectionsList[0].Transparency;
                    }
                    dialogProcessor.IsDragging = true;
                    dialogProcessor.LastLocation = e.Location;
                }
                else
                    dialogProcessor.SelectionsList.Clear();

                if (dialogProcessor.SelectionsList.Count == 1 && dialogProcessor.SelectionsList[0].GetType() == typeof(GroupShape))
                    ungroupElements.Enabled = true;
                else
                    ungroupElements.Enabled = false;

                if (dialogProcessor.SelectionsList.Count > 1)
                    groupElement.Enabled = true;
                else
                    groupElement.Enabled = false;
            }

            viewPort.Invalidate();
        }

        /// <summary>
        /// Прихващане на преместването на мишката.
        /// Ако сме в режм на "влачене", то избрания елемент се транслира.
        /// </summary>
        void ViewPortMouseMove(object sender, MouseEventArgs e)
        {
            if (dialogProcessor.IsDragging)
            {
                if (dialogProcessor.SelectionsList.Count != 0) statusBar.Items[0].Text = "Последно действие: Влачене";
                dialogProcessor.TranslateTo(e.Location);
                viewPort.Invalidate();
            }
        }

        /// <summary>
        /// Прихващане на отпускането на бутона на мишката.
        /// Излизаме от режим "влачене".
        /// </summary>
        void ViewPortMouseUp(object sender, MouseEventArgs e)
        {
            dialogProcessor.IsDragging = false;
        }

        /// <summary>
        /// Рисуване на Еллипс.
        /// </summary>
        private void drawEllipse_Click(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomElipse();
            statusBar.Items[0].Text = "Последно действие: Рисуване на елипс";
            viewPort.Invalidate();
        }

        /// <summary>
        /// Рисуване на звезда.
        /// </summary>
        private void drawStar_Click(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomStar();
            statusBar.Items[0].Text = "Последно действие: Рисуване на звезда";
            viewPort.Invalidate();
        }

        /// <summary>
        /// Прямана на цвят на граница.
        /// </summary>
        private void borderColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK && dialogProcessor.SelectionsList.Count != 0)
            {
                dialogProcessor.SetBorderColor(colorDialog1.Color);
                statusBar.Items[0].Text = "Последно действие: Смяна цвят на граници примитива";
                viewPort.Invalidate();
            }
        }

        /// <summary>
        /// Прямана на цвят на обьекта.
        /// </summary>
        private void fillColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK && dialogProcessor.SelectionsList.Count != 0)
            {
                dialogProcessor.SetFillColor(colorDialog1.Color);
                statusBar.Items[0].Text = "Последно действие: Смяна цвят на запълване примитива";
                viewPort.Invalidate();
            }
        }

        /// <summary>
        /// Прямана на дебилина на граница.
        /// </summary>
        private void textBoxForBorderWidth_TextChanged(object sender, EventArgs e)
        {
            if (dialogProcessor.SelectionsList.Count != 0)
            {
                Int32.TryParse(textBoxForBorderWidth.Text, out int value);
                dialogProcessor.SetBorderWidth(value);
                statusBar.Items[0].Text = "Последно действие: Промяна на дебилина на граници";
                viewPort.Invalidate();
            }
        }

        /// <summary>
        /// Прямана на Прозрачноста на обекта.
        /// </summary>
        private void trackBarTransperency_Scroll(object sender, EventArgs e)
        {
            if (dialogProcessor.SelectionsList.Count != 0)
            {
                dialogProcessor.SetTransparency(trackBarTransperency.Value);
                statusBar.Items[0].Text = "Последно действие: Промяна на прозрачноста на примитива";
                viewPort.Invalidate();
            }
        }


        /// <summary>
        /// Изтриване на селектиран обьект или обьекти.
        /// </summary>
        private void deleteElementButton_Click(object sender, EventArgs e)
        {
            if (dialogProcessor.SelectionsList.Count != 0)
            {
                dialogProcessor.RemoveShape();
                statusBar.Items[0].Text = "Последно действие: Изтриване на обьекта";
                viewPort.Invalidate();
            }
        }
        /// <summary>
        /// Групиране на селектирани елементи.
        /// </summary>
        private void groupElement_Click(object sender, EventArgs e)
        {
            dialogProcessor.GroupElements();
            groupElement.Enabled = false;
            ungroupElements.Enabled = true;
            viewPort.Invalidate();
        }
        private void ungroupElements_Click(object sender, EventArgs e)
        {
            dialogProcessor.UnGroupElements();
            ungroupElements.Enabled = false;
            groupElement.Enabled = true;
            viewPort.Invalidate();
        }


        private void textBoxForRotate_TextChanged(object sender, EventArgs e)
        {
            if (dialogProcessor.SelectionsList.Count != 0)
            {
                Int32.TryParse(textBoxForRotate.Text, out int value);
                dialogProcessor.Rotate(value);
                viewPort.Invalidate();
            }
        }

        private void textBoxForScale_TextChanged(object sender, EventArgs e)
        {
            if (dialogProcessor.SelectionsList.Count != 0)
            {
                float.TryParse(textBoxForScale.Text, out float value);

                if (value == 0)
                    value = 1f;
                dialogProcessor.ChangeScale(value);
                viewPort.Invalidate();
            }
        }

        private void rotateD45_Click(object sender, EventArgs e)
        {
            dialogProcessor.Rotate(-45);
            viewPort.Invalidate();
        }

        private void rotate45_Click(object sender, EventArgs e)
        {
            dialogProcessor.Rotate(45);
            viewPort.Invalidate();
        }
        private void rotateD90_Click(object sender, EventArgs e)
        {
            dialogProcessor.Rotate(-90);
            viewPort.Invalidate();
        }
        private void rotate90_Click(object sender, EventArgs e)
        {
            dialogProcessor.Rotate(90);
            viewPort.Invalidate();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Файлове на модела  (*.xcf)|*.xcf|Файлове на изображението(*.png)|*.png";
            saveFileDialog.Title = "Запазване";
            saveFileDialog.CheckPathExists = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog.FileName;

                if (fileName.Contains(".xcf"))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
                    {
                        foreach (Shape shape in dialogProcessor.ShapeList)
                        {
                            formatter.Serialize(fileStream, shape);
                        }
                    }
                }
                else if (fileName.Contains(".png"))
                {
                    using (Bitmap bitmap = new Bitmap(viewPort.Width, viewPort.Height))
                    {
                        using (Graphics graphics = Graphics.FromImage(bitmap))
                        {
                            graphics.Clear(viewPort.BackColor);

                            foreach (Shape shape in dialogProcessor.ShapeList)
                            {
                                shape.DrawSelf(graphics);
                            }
                        }

                        bitmap.Save(fileName, ImageFormat.Png);
                    }
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();

            var fileName = openFileDialog.FileName;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileStream fileStream = new FileStream(fileName, FileMode.Open);
                dialogProcessor.ShapeList.Clear();

                BinaryFormatter formatter = new BinaryFormatter();
                while (fileStream.Position < fileStream.Length)
                {
                    var shape = formatter.Deserialize(fileStream);
                    dialogProcessor.ShapeList.Add((Shape)shape);
                }
            }
        }
    }
}
