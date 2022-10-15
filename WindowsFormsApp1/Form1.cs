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
        TrackBar tbSize;
        TrackBar tbRoll;
        TrackBar tbPitch;
        TrackBar tbYaw;
        Matrix matrix;
        Drawer drawer;
        public Form1()
        {
            
            InitializeComponent();
 
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
 
            Size = new Size(500, 500);
 
            tbSize = new TrackBar { Parent = this, Maximum = 200, Left = 0, Value = 50};
            tbRoll = new TrackBar { Parent = this, Maximum = 360, Left = 110, Value = 50 };
            tbPitch = new TrackBar { Parent = this, Maximum = 360, Left = 220, Value = 50 };
            tbYaw = new TrackBar { Parent = this, Maximum = 360, Left = 330, Value = 50 };
 
            tbSize.ValueChanged += tb_ValueChanged;
            tbRoll.ValueChanged += tb_ValueChanged;
            tbPitch.ValueChanged += tb_ValueChanged;
            tbYaw.ValueChanged += tb_ValueChanged;
 
            tb_ValueChanged(null, EventArgs.Empty);

            matrix = new Matrix();
 
            //загружаем модель из .obj
            model = new Model();
            drawer = new Drawer();
            model.LoadFromObj(new StreamReader("C:\\Users\\Admin\\Desktop\\cherry.obj"));
        }
        
        void tb_ValueChanged(object sender, EventArgs e)
        {
            scale = tbSize.Value;
            pitch = (float)(tbPitch.Value * Math.PI / 180);
            roll = (float)(tbRoll.Value * Math.PI / 180);
            yaw = (float)(tbYaw.Value * Math.PI / 180);
            Invalidate();
        }
 
        float yaw = 10;
        float pitch = 10;
        float roll = 10;
        float scale = 1;
        Vector3 position = new Vector3(200, 200, 200);

        
        protected override void OnPaint(PaintEventArgs e)
        {
            //матрица масштабирования
            var scaleM = Matrix4x4.CreateScale(scale);
            //матрица вращения
            
            var rotateM = Matrix4x4.CreateFromYawPitchRoll(yaw, pitch, roll);
           
            //матрица переноса
            var translateM = matrix.GetTranslationMatrix(position);
            
            //матрица проекции
            var paneXY = new Matrix4x4(
                    (float)2/100,0,0,0,
                    0,(float) 2/100,0,0,
                    0,0,(float)2/100,1,
                    0,0,0,1)
                ;
            //результирующая матрица
            var m = scaleM  * translateM * rotateM;
            m *= paneXY;

            //умножаем вектора на матрицу
            var vertexes = model.Vertexes.Select(v => Vector3.Transform(v, m)).ToList();
        
            //создаем graphicsPath
            using (var path = new GraphicsPath())
            {
                //создаем грани
                var prev = Vector3.Zero;
                var prevF = 0;
                foreach (var f in model.Fig)
                {
                    if (f == 0) path.CloseFigure();
                    var v = vertexes[f];
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