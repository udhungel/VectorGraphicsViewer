using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Interface;
using static WebApplication1.Models.SD;

namespace WebApplication1.Models
{
    public class PrimitivePolygon : IPrimitive
    {
        private readonly PrimitiveType _primitiveType;
        private bool? _isFilled;
        private Color _color;
        private IEnumerable<Coordinate> _coordinates;

        public PrimitivePolygon(PrimitiveType primitiveType, IEnumerable<Coordinate> coordinates, bool? isFilled, Color color)
        {
            _primitiveType = primitiveType;
            _isFilled = isFilled;
            _color = color;
            Coordinates = coordinates; // Use property to validate
        }

        public PrimitiveType Type => _primitiveType;

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

        public IEnumerable<Coordinate> Coordinates
        {
            get => _coordinates;
            set
            {
                if (IsCoordinatesValid(value))
                {
                    _coordinates = value;
                }
                else
                {
                    throw new Exception($"Invalid coordinate value for the type {_primitiveType}");
                }
            }
        }

        private bool IsCoordinatesValid(IEnumerable<Coordinate> value)
        {
            return value != null && value.Count() > 2 
                                 && value.Count() == (int)_primitiveType;
        }
    }
}