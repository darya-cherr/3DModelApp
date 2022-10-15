using System;
using System.Numerics;

namespace WindowsFormsApp1
{
    public class Matrix
    {
        public Matrix4x4 GetTranslationMatrix(Vector3 position)
        {
            var rotationXMatrix = new Matrix4x4( 
                 1f, 0, 0, position.X,
                 0, 1, 0, position.Y ,
                 0, 0, 1, position.Z,
                 0, 0, 0, 1);
             return rotationXMatrix;
           // return Matrix4x4.CreateTranslation(position);
        }

        public Matrix4x4 GetRotationXMatrix(Vector3 rotation)
        {
            var rotationXMatrix = new Matrix4x4( 
                1f, 0, 0, 0,
                0, (float)Math.Cos(rotation.X),(float) (-1 * Math.Sin(rotation.X)), 0 ,
                0, (float)Math.Sin(rotation.X), (float)Math.Cos(rotation.X), 0,
                0, 0, 0, 1f);
            return rotationXMatrix;
           // return Matrix4x4.CreateRotationX(rotation.X);
        }
        
        public Matrix4x4 GetRotationYMatrix(Vector3 rotation)
        {
            var rotationYMatrix = new Matrix4x4( 
                (float)Math.Cos(rotation.Y), 0, (float)Math.Sin(rotation.Y), 0,
                0, 1f, 0, 0,
                (float)(-1f * Math.Sin(rotation.Y)), 0, (float)Math.Cos(rotation.Y), 0,
                0, 0, 0, 1f);
            
             return rotationYMatrix;
            //return Matrix4x4.CreateRotationY(rotation.Y);
        }
        
        public Matrix4x4 GetRotationZMatrix(Vector3 rotation)
        {
            var rotationZMatrix = new Matrix4x4( 
                (float)Math.Cos(rotation.Z), (float)(-1f * Math.Sin(rotation.Z)), 0, 0,
                (float)Math.Sin(rotation.Z), (float)Math.Cos(rotation.Z), 0, 0,
                0, 0, 1f, 0,
                0, 0, 0, 1f);
            
            return rotationZMatrix;
            //return Matrix4x4.CreateRotationZ(rotation.Z);
        }
        
        
        
    }
}