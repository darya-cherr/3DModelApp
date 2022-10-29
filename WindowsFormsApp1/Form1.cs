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
            
            tbModelSize = new TrackBar { Parent = this, Maximum = 80, Left = 0, Value = 20};
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

        int round(float n)
        {
            if (n - (int)n < 0.5)
                return (int)n;
            return (int)(n + 1);
        }
        void pictureBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            var m = matrix.GetMVPMatrix(modelScale, modelRotation,modelPosition, defaultCameraRotation,defaultCameraPosition);
            
            var vertexes = model.Vertexes.Select(v => Vector4.Transform(v, m)).ToList();
            
            vertexes = vertexes.Select(v => Vector4.Transform(v/=v.W, matrix.GetViewPortMatrix())).ToList();
            vertexes = vertexes.Select(v => new Vector4(v.X,v.Y,v.Z,v.W)).ToList();
            
            using (var bmp = new Bitmap(pictureBox.Width, pictureBox.Height))
            {
                var prev = Vector4.Zero;
                var prevF = 0;
                
                foreach (var f in model.Polygon)
                {
                    var v = vertexes[f];
                        if (prevF != 0 && f != 0)
                        {
                            int dx = (int)(v.X - prev.X);
                            int dy = (int)(v.Y - prev.Y);
 
                            int l;
 
                            // If dx > dy we will take step as dx
                            // else we will take step as dy to draw the complete
                            // line
                            if (Math.Abs(dx) > Math.Abs(dy))
                                l = Math.Abs(dx);
                            else
                                l = Math.Abs(dy);
 
                            // Calculate x-increment and y-increment for each step
                            float x_incr = (float)dx / l;
                            float y_incr = (float)dy / l;
 
                            // Take the initial points as x and y
                            float x = prev.X;
                            float y = prev.Y;
 
                            for (int i = 0; i < l; i++) {
                                bmp.SetPixel(round(x), round(y), Color.Black);
                                x += x_incr;
                                y += y_incr;
                            }
                        }
                        prev = v;
                        prevF = f;
                }
                pictureBox.Image?.Dispose();
                pictureBox.Image = (Bitmap)bmp.Clone();                   
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