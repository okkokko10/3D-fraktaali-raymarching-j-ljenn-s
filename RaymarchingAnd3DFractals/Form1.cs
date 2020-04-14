﻿using System;
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
            this.Width = 500;
            this.Height = 500;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Rayable drawnObject = new Rayable(new Vector3(0.5f, 0.5f, 0.5f), 1 / 2);
            Vector3 pointOfView = new Vector3(0, 0, 0);
            Vector3 direction = new Vector3(0, 0, 1);
            //Pen myPen = new Pen(Color.Blue,pixel_Y);
            Graphics g = e.Graphics;
            Bitmap image1 = new Bitmap(Image.FromFile(@"C:\Users\Okko Heiniö\Documents\My Games\Terraria\ModLoader\Mod Sources\OkkokkoHeap\Textures\500x500.bmp"));

            Scan(image1, drawnObject, pointOfView, direction);
            g.DrawImage(image1, new Point(0, 0));
            //g.Flush(System.Drawing.Drawing2D.FlushIntention.Flush);

        }
        public float RayMarchScan(Vector3 Point, Rayable item)
        {
            Vector3 point = Point;
            if (true)
            {
                point.x = MathModulo(point.x, 1);
                point.y = MathModulo(point.y, 1);
                //point.y = Mathf.Abs(point.y);
                //point.z = Math.Abs(10-point.z)
                //point.z = MathModulo(point.z, 1);
            }
            return (item.Position - point).Length - item.Radius;
        }
        float MathModulo(float a, float b)
        {
            return ((a % b) + b) % b;
        }

        public void Scan(Bitmap baseImage, Rayable drawnObject, Vector3 pov = null, Vector3 rotation = null)
        {
            if (pov == null) { pov = Vector3.zero; }
            if (rotation == null) { rotation = new Vector3(0, 0, 1); }


            for (int iy = 0; iy < resolution_Y; iy++)
            {
                for (int ix = 0; ix < resolution_X; ix++)
                {
                    RayResult result = Ray(new Vector3((float)(ix) / resolution_X - 0.5f, (float)(iy) / resolution_Y - 0.5f, 1).Rotate(rotation), drawnObject, pov, 0.1f, 10);
                    int color = (int)((1 - (result.Closest * 10)) * 255);
                    //int color = (int)((1 - (result.RayLength / 20)) * 255);

                    //int color = (int)((1 - (result.Steps / 20)) * 255);
                    if (color > 255) { color = 255; }
                    if (color > 0)
                    {
                        Color color1 = Color.FromArgb(color, color, color);
                        baseImage.SetPixel(ix, iy, color1);
                        //DrawPixel(baseImage, color1, ix, resolution_Y - iy);
                    }
                }
            }
        }
        public const int resolution_X = 500;
        public const int resolution_Y = 500;
        public const int window_X = 500;
        public const int window_Y = 500;

        public void DrawPixel(Bitmap bitmap, Color color, int px, int py)
        {

            //gp.pen.Color = color;
            //gp.graphics.FillRectangle(gp.pen.Brush, px * pixel_X, py * pixel_Y,pixel_X,pixel_Y);
            bitmap.SetPixel(px, py, color);

        }
        public void CreateBitmap()
        {
            Image img = Image.FromFile(@"C:\Users\Okko Heiniö\Documents\My Games\Terraria\ModLoader\Mod Sources\OkkokkoHeap\Textures\3.png");
        }


        public RayResult Ray(Vector3 rayPoint, Rayable drawable, Vector3 pov, float precisionCutoff = 0.1f, float lengthCutoff = 20)
        {
            float rayLength = 0;
            int steps = 0;
            Vector3 rayPointNorm = rayPoint.Normalize;
            Vector3 rayPointNow = pov;
            float closest = RayMarchScan(rayPointNow, drawable);
            bool enough = false;
            while (!enough)
            {
                steps++;
                float closestNow = RayMarchScan(rayPointNow, drawable);
                rayLength += closestNow;
                rayPointNow += rayPointNorm * closestNow;
                if (closestNow < closest)
                {
                    closest = closestNow;
                }

                if ((closest < precisionCutoff) || (rayLength > lengthCutoff))
                {
                    //DrawPixel(rayDirection * 2, 1 - closeness * 7);
                    //steps = 0;
                    enough = true;
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
            public Vector3 Rotate(Vector3 rotation)
            {//a=x,b=z,c=y
                //  rot =a,c,b
                //  -b,0,a   *x
                //  -a*c/l(a,b), l(a,b), -b*c/l(a,b)   *y
                //  
                float projF = (float)Math.Sqrt(rotation.x * rotation.x + rotation.z * rotation.z); //=1
                Vector3 projX = new Vector3(-rotation.z, 0, rotation.x) * this.x;   //=-x,0,0
                Vector3 projY = new Vector3(-rotation.x * rotation.y / projF, projF, -rotation.z * rotation.y / projF) * this.y;  //0,y,0

                return rotation + projX + projY; //-x,y,1

            }
            public static Vector3 zero = new Vector3(0, 0, 0);
        }

        public struct Rayable
        {
            public Vector3 Position;
            public float Radius;
            public Rayable(Vector3 position, float radius)
            {
                this.Position = position;
                this.Radius = radius;
            }
        }


    }
}