using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WebApplication1.Interface;
using WebApplication1.Models;

namespace DXApplication
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        private string _selectedFile;
        private BindingList<IPrimitive> _primitives;
        private readonly IInputReader _inputReader;

        public Form1(IInputReader inputReader)
        {
            InitializeComponent();
            _inputReader = inputReader;
            _primitives = new BindingList<IPrimitive>();
            canvasPanel.Resize += CanvasPanel_Resize;
            txtSelectedFile.ReadOnly = true;
            lblStatusMessage.Visible = false;
        }

        private void BtnLoadFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _selectedFile = openFileDialog.FileName;
                    txtSelectedFile.Text = _selectedFile;
                    LoadInputToView();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void LoadInputToView()
        {
            lblStatusMessage.Text = "Parsing input file";
            lblStatusMessage.Visible = true;

            try
            {
                _primitives.Clear();
                foreach (var primitive in _inputReader.GetAllPrimitives(_selectedFile))
                {
                    _primitives.Add(primitive); 
                }

                lblStatusMessage.Text = "Parsing done";
                canvasPanel.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!");
                lblStatusMessage.Text = $"Error: {ex.Message}";
            }
        }

        private void CanvasPanel_Resize(object sender, EventArgs e)
        {
            canvasPanel.Invalidate();
        }

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (_primitives != null && _primitives.Count > 0)
            {
                double scalingFactor = CalculateScalingFactor(g);
                foreach (var primitive in _primitives)
                {
                    DrawPrimitive(g, primitive, scalingFactor);
                }
            }
        }

        private double CalculateScalingFactor(Graphics g)
        {
            double containerWidth = canvasPanel.ClientSize.Width;
            double containerHeight = canvasPanel.ClientSize.Height;

            double maxX = double.MinValue, maxY = double.MinValue, minX = double.MaxValue, minY = double.MaxValue;

            foreach (var primitive in _primitives)
            {
                if (primitive is PrimitiveCircle circle)
                {
                    maxX = Math.Max(maxX, circle.XPos + circle.Radius);
                    minX = Math.Min(minX, circle.XPos - circle.Radius);
                    maxY = Math.Max(maxY, circle.YPos + circle.Radius);
                    minY = Math.Min(minY, circle.YPos - circle.Radius);
                }
                else if (primitive is PrimitiveLine line)
                {
                    maxX = Math.Max(maxX, Math.Max(line.Start.X, line.End.X));
                    minX = Math.Min(minX, Math.Min(line.Start.X, line.End.X));
                    maxY = Math.Max(maxY, Math.Max(line.Start.Y, line.End.Y));
                    minY = Math.Min(minY, Math.Min(line.Start.Y, line.End.Y));
                }
                else if (primitive is PrimitivePolygon polygon)
                {
                    foreach (var point in polygon.Coordinates)
                    {
                        maxX = Math.Max(maxX, point.X);
                        minX = Math.Min(minX, point.X);
                        maxY = Math.Max(maxY, point.Y);
                        minY = Math.Min(minY, point.Y);
                    }
                }
            }

            double boundingWidth = maxX - minX;
            double boundingHeight = maxY - minY;

            return (boundingWidth > 0 && boundingHeight > 0)
                ? Math.Min(containerWidth / boundingWidth, containerHeight / boundingHeight)
                : 1.0; // Default scaling factor if no primitives
        }

        private void DrawPrimitive(Graphics g, IPrimitive primitive, double scalingFactor)
        {
            if (primitive is PrimitiveCircle circle)
            {
                Brush brush = circle.IsFilled.HasValue ?
                    new SolidBrush(System.Drawing.Color.FromArgb(circle.Color.A, circle.Color.R, circle.Color.G, circle.Color.B)) :
                    Brushes.Transparent;

                float scaledX = (float)(circle.XPos * scalingFactor);
                float scaledY = (float)(circle.YPos * scalingFactor);
                float scaledDiameter = (float)(circle.Diameter * scalingFactor);
                g.FillEllipse(brush, scaledX - (scaledDiameter / 2), scaledY - (scaledDiameter / 2), scaledDiameter, scaledDiameter);
                g.DrawEllipse(Pens.Black, scaledX - (scaledDiameter / 2), scaledY - (scaledDiameter / 2), scaledDiameter, scaledDiameter);
            }
            else if (primitive is PrimitiveLine line)
            {
                Pen pen = new Pen(System.Drawing.Color.FromArgb(line.Color.A, line.Color.R, line.Color.G, line.Color.B));
                float scaledStartX = (float)(line.Start.X * scalingFactor);
                float scaledStartY = (float)(line.Start.Y * scalingFactor);
                float scaledEndX = (float)(line.End.X * scalingFactor);
                float scaledEndY = (float)(line.End.Y * scalingFactor);
                g.DrawLine(pen, scaledStartX, scaledStartY, scaledEndX, scaledEndY);
            }
            else if (primitive is PrimitivePolygon polygon)
            {
                PointF[] points = polygon.Coordinates.Select(p => new PointF((float)(p.X * scalingFactor), (float)(p.Y * scalingFactor))).ToArray();
                Brush brush = polygon.IsFilled.HasValue ?
                    new SolidBrush(System.Drawing.Color.FromArgb(polygon.Color.A, polygon.Color.R, polygon.Color.G, polygon.Color.B)) :
                    Brushes.Transparent;

                g.FillPolygon(brush, points);
                g.DrawPolygon(Pens.Black, points);
            }
        }
    }
}