using WebApplication1.Interface;
using System.Drawing;

namespace WebApplication1.Models
{
    public class PrimitiveCircle : IPrimitive
    {
        private bool? _isFilled;
        private Color _color;
        private Coordinate _center;
        private float _radius;

        public PrimitiveCircle(Coordinate center, float radius, bool isFilled, Color color)
        {
            _center = center;
            _radius = radius;
            _isFilled = isFilled;
            _color = color;
        }

        public Color Color
        {
            get => _color;
            set => _color = value;
        }

        public bool? IsFilled
        {
            get => _isFilled;
            set => _isFilled = value;
        }

        public Coordinate Center
        {
            get => _center;
            set => _center = value;
        }

        public float Radius
        {
            get => _radius;
            set => _radius = value;
        }

        // Properties required for binding in XAML 
        public float Diameter => _radius * 2;

        public float XPos => _center.X - _radius;

        public float YPos => _center.Y - _radius;
    }
}