using System.Drawing;

namespace WebApplication1.Interface
{
    public interface IPrimitive
    {
        Color Color { get; set; }
        bool? IsFilled { get; set; }
    }
}
