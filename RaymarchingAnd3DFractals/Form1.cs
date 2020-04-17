using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RaymarchingAnd3DFractals
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Width = 750;
            this.Height = 750;
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Vector3 moveControl = new Vector3(0, 0, 0); //Vector3.zero;
            if (e.KeyCode == Keys.W)
            {
                moveControl.z += 1;
            }
            if (e.KeyCode == Keys.S)
            {
                moveControl.z += -1;
            }
            if (e.KeyCode == Keys.D)
            {
                moveControl.x += 1;
            }
            if (e.KeyCode == Keys.A)
            {
                moveControl.x += -1;
            }
            if (e.KeyCode == Keys.Down)
            {
                rot_up -= 0.2f;
            }
            if (e.KeyCode == Keys.Up)
            {
                rot_up += 0.2f;
            }
            if (e.KeyCode == Keys.Space)
            {
                pointOfView.y += 1;
            }
            if (e.KeyCode == Keys.ShiftKey)
            {
                pointOfView.y -= 1;
            }
            if (e.KeyCode == Keys.Right)
            {
                rot_side -= 0.2f;
            }
            if (e.KeyCode == Keys.Left)
            {
                rot_side += 0.2f;
            }
            if (e.KeyCode == Keys.R)
            {
                drawnObject.Position = pointOfView*1;
                drawnObject.Position.y -= 01f;
            }

            pointOfView += moveControl.Rotate(rot_side, rot_up);

            FindForm().Refresh();
            
        }
        public const int resolution_X = 150;
        public const int resolution_Y = 150;


        public float rot_up = 0;
        public float rot_side = 0;
        public Vector3 pointOfView = new Vector3(05f, 0f, 0f);
        public Rayable drawnObject = new Rayable(new Vector3(2f, 01f, 1f),new Vector3(2,1,1)*0.5f, 0.0f); //why the f does the results change drastically if I put in a fraction instead of a decimal?

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            
            Graphics g = e.Graphics;
            Bitmap image1 = new Bitmap(Image.FromFile(@"C:\Users\Okko Heiniö\Documents\My Games\Terraria\ModLoader\Mod Sources\OkkokkoHeap\Textures\500x500.bmp"));

            Bitmap image2 = Scan(image1, drawnObject, pointOfView);
            g.DrawImage(image2, new Rectangle(0,0,Width*500/resolution_X,Height*500/resolution_Y));
                    //g.Flush(System.Drawing.Drawing2D.FlushIntention.Flush);
            e.Dispose();
            
        }
        public float RayMarchScan(Vector3 Point, Rayable item)
        {
            Vector3 point = new Vector3(Point.x,Point.y,Point.z);
            if (true)
            {
                //if (point.x < 4f && point.x > -5f)
                {
                    //point.x = MathModulo(point.x+1f, 2f)-1f;
                }
                //if (point.y < 3f && point.y > -3f)
                {
                    //point.y = MathModulo(point.y, 2f);
                    //point.y = Math.Abs(point.y);
                }
                if (point.y < 0)
                {
                    //point.y = -point.y;
                }
                if (point.x < 0)
                {
                    point.x = -point.x;
                }

                //point.x = 2 - Math.Abs(Math.Abs(point.x - 2) - 4);
                point = Mirror(point - new Vector3(0, 0, 2), 1) + new Vector3(0, 0, 2);
                point = Mirror(point - new Vector3(0, 0, -2), -1) + new Vector3(0, 0, -2);

                //if (point.y > 2){ point.y -= 2f;point.x -= 1f;}
                //point.y = Math.Abs(point.y);
                //point.z = Math.Abs(10 - point.z);
                //point.z = MathModulo(point.z, 1);
            }
            return item.NearestPoint(point);
        }
        float MathModulo(float a, float b)
        {
            return ((a % b) + b) % b;
        }
        public Vector3 Mirror(Vector3 point,int a)//,Vector3 direction)
        {
            //int a = 1;
            if (point.x < a * point.z) return new Vector3(a * point.z, point.y, a * point.x);
            else return point;
        }


        public Bitmap Scan(Bitmap baseImage, Rayable drawnObject, Vector3 pov,float lengthCutoff=30)
        {
            Bitmap image = new Bitmap( baseImage);
            for (int iy = 1; iy < resolution_Y; iy++)
            {
                for (int ix = 1; ix < resolution_X; ix++)
                {
                    RayResult result = Ray(RayDirection(ix,iy), drawnObject, pov, 0.005f, lengthCutoff,50);
                    //int color = (int)((1 - (result.Closest /0.05f)) * 255);
                    //int color = (int)((1 - (result.RayLength / 20)) * 255);
                    //int color = (int)(( (result.RayLength/lengthCutoff)) * 255);
                    int color = (int)((1- (result.Steps /50f)) * 255);
                    if (color > 255||result.RayLength>lengthCutoff) { color = 0; }
                    if (color < 0) { color = 0; }
                    {
                        Color color1 = Color.FromArgb(color, color, color);
                        //image.SetPixel(resolution_X-ix, resolution_Y-iy, color1);
                        image.SetPixel(ix, resolution_Y - iy, color1);
                        //DrawPixel(baseImage, color1, ix, resolution_Y - iy);
                    }
                }
            }
            return image;
        }
        public void DrawPixel(Bitmap bitmap, Color color, int px, int py)
        {

            //gp.pen.Color = color;
            //gp.graphics.FillRectangle(gp.pen.Brush, px * pixel_X, py * pixel_Y,pixel_X,pixel_Y);
            bitmap.SetPixel(px, py, color);

        }

        public Vector3 RayDirection(int px,int py)
        {
            return new Vector3((float)(px) / resolution_X - 0.5f, (float)(py) / resolution_Y - 0.5f, 01f).Rotate(rot_side,rot_up).Normalize;

            //return Vector3.TowardsAngle(((float)(px) / resolution_X - 0.5f)+rot_side, ((float)(py) / resolution_Y - 0.5f)+rot_up);
        }



        public RayResult Ray(Vector3 rayPoint, Rayable drawable, Vector3 pov, float precisionCutoff = 0.1f, float lengthCutoff = 20,int maxSteps=20)
        {
            float rayLength = 0;
            int steps = 0;
            Vector3 rayPointNorm = rayPoint.Normalize;
            Vector3 rayPointNow = pov*1;
            float closest = RayMarchScan(rayPointNow, drawable);
            bool enough = false;
            while (!enough)
            {
                steps++;
                closest = RayMarchScan(rayPointNow, drawable);
                rayLength += closest;
                rayPointNow += rayPointNorm * closest;
                //if (closestNow < closest)
                
                    //closest = closestNow;
                

                if (
                    (closest < precisionCutoff)
                    
                    ||(steps>=maxSteps)
                    )
                {
                    //DrawPixel(rayDirection * 2, 1 - closeness * 7);
                    //steps = 0;
                    enough = true;
                }
                if (rayLength > lengthCutoff) { enough = true;//steps = maxSteps; 
                }

            }
            return new RayResult(steps, closest, rayLength);
        }

        public struct RayResult
        {
            public int Steps;
            public float Closest;
            public float RayLength;
            public RayResult(int steps, float closest, float rayLength)
            {
                this.Steps = steps;
                this.Closest = closest;
                this.RayLength = rayLength;
            }
        }
        public class Vector3
        {
            public float x, y, z;
            public Vector3(float X, float Y, float Z)
            {
                this.x = X;
                this.y = Y;
                this.z = Z;
            }
            public static Vector3 TowardsAngle(float side, float up)
            {
                //side=a, up=b,
                //(cos(a)cos(b) - sin(a)r - cos(a)sin(b)d,sin(b) + cos(b)d,sin(a)cos(b) + cos(a)r - sin(a)sin(b)d)

                return (new Vector3((float)Math.Tan(side), 0, 1).Normalize + new Vector3(0, (float)Math.Tan(up), 0)).Normalize;

            }
            public Vector3 Copy
            {
                get
                {
                    return new Vector3(this.x, this.y, this.z);
                }
            }
            public Vector3 Normalize
            {
                get
                {
                    //return new Vector3(this.x/this.Length,this.y/this.Length,this.z/this.Length);
                    return this * (1 / this.Length);
                }
            }
            public float Length
            {
                get
                {
                    return (float)(Math.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z));
                }
            }
            public static Vector3 operator *(Vector3 a, float coefficient)
            {
                return new Vector3(a.x * coefficient, a.y * coefficient, a.z * coefficient);
            }
            public static Vector3 operator -(Vector3 a, Vector3 b)
            {
                return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
            }
            public static Vector3 operator +(Vector3 a, Vector3 b)
            {
                return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
            }
            public Vector3 RotateVector(Vector3 rotation)
            {//a=x,b=z,c=y
                //  rot =a,c,b
                //  -b,0,a   *x
                //  -a*c/l(a,b), l(a,b), -b*c/l(a,b)   *y

                float projF = (float)Math.Sqrt(rotation.x * rotation.x + rotation.z * rotation.z); //=1
                Vector3 projX = new Vector3(-rotation.z, 0, rotation.x) * this.x;   //=-x,0,0
                Vector3 projY = new Vector3(-rotation.x * rotation.y / projF, projF, -rotation.z * rotation.y / projF) * this.y;  //0,y,0

                return rotation + projX + projY; //-x,y,1

            }
            public Vector3 Rotate(float a, float b)
            {
                //  (c,d,r) side=a,up=b
                //  (cos(a)cos(b)c-sin(a)r-cos(a)sin(b)d,sin(b)c+cos(b)d,sin(a)cos(b)c+cos(a)r-sin(a)sin(b)d)
                //  x=  cos(a)cos(b)x -cos(a)sin(b)y -sin(a)z
                //  cos(a)*(cos(b)x-sin(b)y) -sin(a)z
                //  y=  sin(b)x+cos(b)y
                //  z=  sin(a)cos(b)x +cos(a)z -sin(a)sin(b)y
                //  sin(a)*(cos(b)x -sin(b)y) +cos(a)z                
                /*return new Vector3(
                (float)(Math.Cos(a) * (Math.Cos(b) * x - Math.Sin(b) * y) - Math.Sin(a) * y),
                    (float)(Math.Sin(a) * (Math.Cos(b) * x - Math.Sin(b) * z) + Math.Cos(a) * y),
                    (float)(Math.Sin(b) * x + Math.Cos(b) * z)
                    );*/
                return new Vector3(
                    (float)(Math.Cos(a) * (Math.Cos(b) * z - Math.Sin(b) * y) - Math.Sin(-a) * x),
                    (float)(Math.Sin(b) * z + Math.Cos(b) * y),
                    (float)(Math.Sin(-a) * (Math.Cos(b) * z - Math.Sin(b) * y) + Math.Cos(a) * x)
                    );
                return new Vector3(
                    (float)(Math.Cos(a) * (Math.Cos(b) * x - Math.Sin(b) * y) - Math.Sin(a) * z),
                    (float)(Math.Sin(b) * x + Math.Cos(b) * y),
                    (float)(Math.Sin(a) * (Math.Cos(b) * x - Math.Sin(b) * y) + Math.Cos(a) * z)
                    );


            }
            public static Vector3 zero = new Vector3(0, 0, 0);
            
            public Vector3 Positive
            {
                get
                {
                    return new Vector3(Math.Abs(this.x), Math.Abs(this.y), Math.Abs(this.z));
                }
            }
            public static Vector3 Max(Vector3 a, Vector3 b)
            {
                return new Vector3((a.x < b.x) ? b.x : a.x, (a.y < b.y) ? b.y : a.y, (a.z < b.z) ? b.z : a.z);
            }
        }

        public class Rayable
        {
            
            public Vector3 Position;
            public Vector3 Scale;
            public float Radius;
            public Rayable(Vector3 position,Vector3 scale, float radius)
            {
                this.Position = position;
                this.Scale = scale;
                this.Radius = radius;
            }
            public float NearestPoint(Vector3 point)
            {
                //      |point-position|-scale
                return Vector3.Max(((point - this.Position).Positive - this.Scale), Vector3.zero).Length - this.Radius;

                //return (this.Position - point).Length - this.Radius;
            }
        }


    }
}