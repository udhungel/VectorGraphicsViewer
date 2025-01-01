using System;
using WebApplication1.Interface;
using static WebApplication1.Models.SD;

namespace WebApplication1.Models
{
    public class PrimitiveLine : IPrimitive
    {
        private Color _color;
        private Coordinate _start;
        private Coordinate _end;

        public PrimitiveLine(Coordinate start, Coordinate end, Color color)
        {
            _start = start;
            _end = end;
            _color = color;
        }

        public Coordinate Start 
        { 
            get => _start; 
            set => _start = value; 
        }

        public Coordinate End 
        { 
            get => _end; 
            set => _end = value; 
        }

        public Color Color 
        { 
            get => _color; 
            set => _color = value; 
        }

        public bool? IsFilled 
        { 
            get => null; 
            set { } 
        }
    }
}