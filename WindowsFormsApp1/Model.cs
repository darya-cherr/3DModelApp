using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Numerics;

namespace WindowsFormsApp1
{
    public class Model
    {
        public List<Vector4> Vertexes = new List<Vector4>();
        public List<int> Polygon = new List<int>();
 
        public void LoadFromObj(TextReader tr)
        {
            string line;
            Vertexes.Clear();
            Vertexes.Add(Vector4.Zero);
 
            IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
            while ((line = tr.ReadLine()) != null)
            {
                var parts = line.Split(' ');
                if (parts.Length == 0) continue;
                switch (parts[0])
                {
                    case "v": Vertexes.Add(new Vector4(float.Parse(parts[1], formatter),
                            float.Parse(parts[2], formatter),
                            float.Parse(parts[3], formatter), parts.Length > 4 ? float.Parse(parts[4], formatter) : 1));
                        break;
                    case "f":
                        for (int i = 1; i < parts.Length; i++)
                            Polygon.Add(int.Parse(parts[i].Split('/')[0]));
                        Polygon.Add(0);
                        break;
                }
            }
        }

        public void GetModelParams()
        {
            
        }
    }
}