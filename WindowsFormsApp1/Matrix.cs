using System;
using System.Numerics;

namespace WindowsFormsApp1
{
    public class Matrix
    {
       
        float fieldOfView = (float)(Math.PI / 4);
        float aspectRatio = (float)1000 / 800;
        float nearPlaneDistance = 0.1f;
        float farPlaneDistance = 100f;
        
        
        // Bitmap, picture box
        // logvic()
        // не использовать setpixel()

        public  Matrix4x4 GetWorldProjectionMatrix()
        {
            return Matrix4x4.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearPlaneDistance, farPlaneDistance);
        }

        private Matrix4x4 GetWorldMatrix(float scale, Vector3 rotation, Vector3 translation)
        {
            Matrix4x4 worldMatrix = Matrix4x4.CreateScale(scale) * Matrix4x4.CreateFromYawPitchRoll(rotation.Y,rotation.X, rotation.Z)
                                                                              * Matrix4x4.CreateTranslation(translation.X, translation.Y, translation.Z);
            return worldMatrix;
        }
        public Matrix4x4 GetMVPMatrix(float scale, Vector3 modelRotation, Vector3 modelTranslation, Vector3 cameraRotation, Vector3 cameraTranslation)
        {
            return GetWorldMatrix(scale, modelRotation,modelTranslation) * GetViewerMatrix(cameraRotation, cameraTranslation) * GetWorldProjectionMatrix() ;
        }

        private static Matrix4x4 GetViewerMatrix(Vector3 rotation, Vector3 translation)
        {
            return
                Matrix4x4.CreateTranslation(-new Vector3(translation.X, translation.Y, translation.Z))
                * Matrix4x4.Transpose(Matrix4x4.CreateRotationX(rotation.X) * Matrix4x4.CreateRotationY(rotation.Y) * Matrix4x4.CreateRotationZ(rotation.Z));
        }
        
        public  Matrix4x4 GetViewPortMatrix()
        {
            return new Matrix4x4(
                (float)1000/2, 0, 0, 0,
                0, (float)-1 * 800/2, 0, 0,
                0, 0, 1, 0,
                (float)1000/2, (float)800/2, 0, 1);
          
        }

        // public void TransformNormal(Model model)
        // {
        //     for (int i = 0; i < model.Vertexes.Count; i++)
        //     {
        //         model.Vertexes[i] = Vector4.Normalize(model.Vertexes[i]);
        //     }
        // }
        // public  void TransformToViewPort(Model model, float[] w)
        // {
        //     for (int i = 0; i < model.Vertexes.Count; i++)
        //     {
        //         model.Vertexes[i] = Vector4.Transform(model.Vertexes[i], GetViewPortMatrix());
        //         model.Vertexes[i] = new Vector4(model.Vertexes[i].X, model.Vertexes[i].Y, model.Vertexes[i].Z, w[i]);
        //     }
        // }
        

    }
}