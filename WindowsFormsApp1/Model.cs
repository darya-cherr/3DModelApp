using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Numerics;

namespace WindowsFormsApp1
{
    public class Model
    {
        public List<Vector3> Vertexes = new List<Vector3>();
        public List<int> Fig = new List<int>();
 
        public void LoadFromObj(TextReader tr)
        {
            string line;
            Vertexes.Clear();
            Vertexes.Add(Vector3.Zero);
 
            IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
            while ((line = tr.ReadLine()) != null)
            {
                var parts = line.Split(' ');
                if (parts.Length == 0) continue;
                switch (parts[0])
                {
                    case "v": Vertexes.Add(new Vector3(float.Parse(parts[1], formatter),
                            float.Parse(parts[2], formatter),
                            float.Parse(parts[3], formatter)));
                        break;
                    case "f":
                        for (int i = 1; i < parts.Length; i++)
                            Fig.Add(int.Parse(parts[i].Split('/')[0]));
                        Fig.Add(0);
                        break;
                }
            }
        }

        public void GetModelParams()
        {
            
        }
    }
}