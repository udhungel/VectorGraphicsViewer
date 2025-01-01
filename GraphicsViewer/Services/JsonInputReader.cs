using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WebApplication1.Interface;
using WebApplication1.Models.InputModels;

namespace WebApplication1.Services
{
    public class JsonInputReader : IInputReader
    {
        
        public List<IPrimitive> GetAllPrimitives(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                throw new FileNotFoundException("Invalid input file path.");

            var inputPrimitives = DeserializeInput(filePath);
            if (inputPrimitives == null)
                throw new InvalidOperationException("Unable to read the input file.");

            return inputPrimitives.Select(x => x.MapToIPrimitive()).ToList();
        }

        private List<PrimitiveInputModel> DeserializeInput(string filePath)
        {
            using (var sr = new StreamReader(filePath))
            {
                var json = sr.ReadToEnd();
                return JsonConvert.DeserializeObject<List<PrimitiveInputModel>>(json) ?? new List<PrimitiveInputModel>();
            }
        }
    }
}
