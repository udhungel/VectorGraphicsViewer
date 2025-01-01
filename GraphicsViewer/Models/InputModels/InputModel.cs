using System.Collections.Generic;
using WebApplication1.Interface;
using static WebApplication1.Models.SD;

namespace WebApplication1.Models.InputModels
{
    public class PrimitiveInputModel
    {
        public string Type { get; set; }
        public string Center { get; set; }
        public string Radius { get; set; }
        public string Filled { get; set; }
        public string Color { get; set; }
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }

        public IPrimitive MapToIPrimitive()
        {
            switch (Type.ToLower())
            {
                case "line":
                    return new PrimitiveLine(
                        new Coordinate(A),
                        new Coordinate(B),
                        new Color(Color)
                    );
                case "circle":
                    return new PrimitiveCircle(
                        new Coordinate(Center),
                        float.Parse(Radius),
                        bool.Parse(Filled),
                        new Color(Color)
                    );
                case "triangle":
                    return new PrimitivePolygon(
                        SD.PrimitiveType.Triangle,
                        new List<Coordinate> { new Coordinate(A), new Coordinate(B), new Coordinate(C) },
                        bool.Parse(Filled),
                        new Color(Color)
                    );
                default:
                    return null;
            }
        }
    }
}
