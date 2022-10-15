using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;

using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        Model model;
        TrackBar tbModelSize;
        TrackBar tbModelRoll;
        TrackBar tbModelPitch;
        TrackBar tbModelYaw;
        TrackBar tbCameraRoll;
        TrackBar tbCameraPitch;
        TrackBar tbCameraYaw;
        Matrix matrix;
        
        Vector3 defaultCameraPosition = new Vector3(0,0,-10);
        Vector3 defaultCameraRotation = new Vector3(0,(float)Math.PI,(float)Math.PI);
       
      
        float modelScale = 1;
        Vector3 modelPosition = new Vector3(500, 500, 500);
        private Vector3 modelRotation = new Vector3(10, 10, 10);
        public Form1()
        {
            
            InitializeComponent();
 
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
 
            Size = new Size(1000, 800);
 
            tbModelSize = new TrackBar { Parent = this, Maximum = 200, Left = 0, Value = 30};
            tbModelRoll = new TrackBar { Parent = this, Maximum = 360, Left = 110, Value = 0 };
            tbModelPitch = new TrackBar { Parent = this, Maximum = 360, Left = 220, Value = 0 };
            tbModelYaw = new TrackBar { Parent = this, Maximum = 360, Left = 330, Value = 0 };
            
           
            tbCameraRoll = new TrackBar { Parent = this, Maximum = 500, Left = 850, Value = 0, TickFrequency = 1};
            tbCameraPitch = new TrackBar { Parent = this, Maximum = 500, Left = 850, Top = 50,Value = 0, TickFrequency = 1};
            tbCameraYaw = new TrackBar { Parent = this, Maximum = 500, Left = 850,Top = 100, Value = 0, TickFrequency = 1 };

            tbModelSize.ValueChanged += TbModelValueChanged;
            tbModelRoll.ValueChanged += TbModelValueChanged;
            tbModelPitch.ValueChanged += TbModelValueChanged;
            tbModelYaw.ValueChanged += TbModelValueChanged;
 
            TbModelValueChanged(null, EventArgs.Empty);
            
            tbCameraRoll.ValueChanged += TbCameraValueChanged;
            tbCameraPitch.ValueChanged += TbCameraValueChanged;
            tbCameraYaw.ValueChanged += TbCameraValueChanged;
 
            TbCameraValueChanged(null, EventArgs.Empty);

            matrix = new Matrix();
 
            model = new Model();
            model.LoadFromObj(new StreamReader("C:\\Users\\Admin\\Desktop\\cherry.obj"));
        }
        
        void TbModelValueChanged(object sender, EventArgs e)
        {
            modelScale = tbModelSize.Value;
            modelRotation = new Vector3((float)(tbModelYaw.Value * Math.PI / 180),  (float)(tbModelPitch.Value * Math.PI / 180), (float)(tbModelRoll.Value * Math.PI / 180));
            Invalidate();
        }
        
        void TbCameraValueChanged(object sender, EventArgs e)
        { 
            defaultCameraRotation = new Vector3((float)tbCameraYaw.Value/100, (float)tbCameraPitch.Value/100, (float)tbCameraRoll.Value/100);
            Invalidate();
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            
            var paneXY = new Matrix4x4(
                    (float)2/100,0,0,0,
                    0,(float) 2/100,0,0,
                    0,0,(float)2/100,1,
                    0,0,0,1)
                ;

            var m = matrix.GetMVPMatrix(modelScale, modelRotation,modelPosition, defaultCameraRotation,defaultCameraPosition);
            m *= paneXY;
            
            // float[] w = new float[model.Vertexes.Count];
            // for (int i = 0; i < model.Vertexes.Count; i++)
            // {
            //     model.Vertexes[i] = Vector4.Transform(model.Vertexes[i], m);
            //
            //     w[i] = model.Vertexes[i].W;
            //     model.Vertexes[i] /= model.Vertexes[i].W;
            // }
            
           // TransformNormal(model, modelParams);
             //matrix.TransformToViewPort(model, w);
            

           var vertexes = model.Vertexes.Select(v => Vector4.Transform(v, m)).ToList();
        
            using (var path = new GraphicsPath())
            {
                //создаем грани
                var prev = Vector4.Zero;
                var prevF = 0;
                foreach (var f in model.Fig)
                {
                    if (f == 0) path.CloseFigure();
                    var v =vertexes[f];
                    if (prevF != 0 && f != 0)
                        path.AddLine(prev.X, prev.Y, v.X, v.Y);
                    prev = v;
                    prevF = f;
                }

                //отрисовываем
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                e.Graphics.DrawPath(Pens.Black, path);
            }
        }
        
    }
    
    
   
}