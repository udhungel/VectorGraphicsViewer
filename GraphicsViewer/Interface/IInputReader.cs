using System.Collections.Generic;

namespace WebApplication1.Interface
{
    public interface IInputReader
    {
        List<IPrimitive> GetAllPrimitives(string filePath);
    }
}
