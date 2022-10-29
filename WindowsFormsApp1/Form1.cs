using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Runtime.InteropServices;
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
        PictureBox pictureBox;
        Matrix matrix;
        
        Vector3 defaultCameraPosition = new Vector3(0,0,2);
        Vector3 defaultCameraRotation = new Vector3(0, 0, 0);
       
      
        float modelScale = 1;
        Vector3 modelPosition = new Vector3(0, 0, 0);
        private Vector3 modelRotation = new Vector3(0, 0, 0);
        
        public Form1()
        {

            InitializeComponent();
 
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
 
            Size = new Size(1000, 800);
            
            tbModelSize = new TrackBar { Parent = this, Maximum = 500, Left = 0, Value = 30};
            tbModelRoll = new TrackBar { Parent = this, Maximum = 360, Left = 110, Value = 0 };
            tbModelPitch = new TrackBar { Parent = this, Maximum = 360, Left = 220, Value = 0 };
            tbModelYaw = new TrackBar { Parent = this, Maximum = 360, Left = 330, Value = 0 };
            
           
            tbCameraRoll = new TrackBar { Parent = this, Maximum = 100, Left = 850, Value = 0, TickFrequency = 1};
            tbCameraPitch = new TrackBar { Parent = this, Maximum = 100, Left = 850, Top = 50,Value = 0, TickFrequency = 1};
            tbCameraYaw = new TrackBar { Parent = this, Maximum = 100, Left = 850,Top = 100, Value = 0, TickFrequency = 1 };

            pictureBox = new PictureBox();
            pictureBox.Location = new System.Drawing.Point(0, 0);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new System.Drawing.Size(1000, 800);
            pictureBox.BackColor = Color.White;
            this.Controls.Add(pictureBox);
            pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_Paint);

             
            
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
            model.LoadFromObj(new StreamReader(@"D:\RiderProjects\WindowsFormsApp1\WindowsFormsApp1\diablo.obj"));
        }

        void pictureBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            var m = matrix.GetMVPMatrix(modelScale, modelRotation,modelPosition, defaultCameraRotation,defaultCameraPosition);
            
            var vertexes = model.Vertexes.Select(v => Vector4.Transform(v, m)).ToList();
            
            vertexes = vertexes.Select(v => Vector4.Transform(v/=v.W, matrix.GetViewPortMatrix())).ToList();
            vertexes = vertexes.Select(v => new Vector4(v.X,v.Y,v.Z,v.W)).ToList();
            
            using (var bmp = new Bitmap(pictureBox.Width, pictureBox.Height))
            using (var gfx = Graphics.FromImage(bmp))
            {
                var prev = Vector4.Zero;
                var prevF = 0;
                
                gfx.SmoothingMode = SmoothingMode.HighQuality;
                foreach (var f in model.Polygon)
                {
                    if (!(f == 0)) 
                    {
                        var v = vertexes[f];
                        if (prevF != 0 && f != 0)
                            gfx.DrawLine(Pens.Black, prev.X, prev.Y, v.X, v.Y);
                        prev = v;
                        prevF = f;
                    }
                }
                pictureBox.Image?.Dispose();
                pictureBox.Image = (Bitmap)bmp.Clone();                   
                pictureBox.Invalidate();
                this.Invalidate();
               // e.Graphics.DrawPath(Pens.Black, path);
            }
            
        }
        
        void TbModelValueChanged(object sender, EventArgs e)
        {
            modelScale = (float)tbModelSize.Value / 100;
            modelRotation = new Vector3((float)(tbModelYaw.Value * Math.PI / 180),  (float)(tbModelPitch.Value * Math.PI / 180), (float)(tbModelRoll.Value * Math.PI / 180));
            Invalidate();
        }
        
        void TbCameraValueChanged(object sender, EventArgs e)
        { 
            defaultCameraRotation = new Vector3((float)tbCameraYaw.Value/100, (float)tbCameraPitch.Value/100, (float)tbCameraRoll.Value/100);
            Invalidate();
        }

    }
    
    
   
}