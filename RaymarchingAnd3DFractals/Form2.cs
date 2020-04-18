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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            this.Width = 750;
            this.Height = 750;
        }

        private void Form2_MouseClick(object sender, MouseEventArgs e)
        {

            drawable.Position = new Vector2((int)(e.X * ((float)resolution / 750)), (int)((Height - e.Y) * ((float)resolution / 750)));
            FindForm().Refresh();
        }
        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Bitmap image1 = new Bitmap(Image.FromFile(@"C:\Users\Okko Heiniö\Documents\My Games\Terraria\ModLoader\Mod Sources\OkkokkoHeap\Textures\500x500.bmp"));

            Bitmap image2 = Scan(image1);
            g.DrawImage(image2, new Rectangle(0, 0, Width * 500 / resolution, Height * 500 / resolution));
            //g.Flush(System.Drawing.Drawing2D.FlushIntention.Flush);
            e.Dispose();

        }
        public Bitmap Scan(Bitmap baseImage)
        {
            Bitmap image = new Bitmap(baseImage);
            for (int ix = 0; ix < resolution; ix++)
            {
                for (int iy = 0; iy < resolution; iy++)
                {
                    DistanceResult result = Distance(new Vector2(ix, iy)); //drawable.NearestPoint(new Vector2(ix,iy));
                    int color = (result.distance == 0) ? 250 : 0;
                    int color2 = result.unflipped ? 250 : 0;
                    int color3 = (result.unfDistance == 0) ? 250 : 0;
                    //(int)(255f / (result + 1f));
                    {
                        Color color1 = Color.FromArgb(color, color3, color2);
                        image.SetPixel(ix, resolution - iy, color1);
                    }
                }
            }
            
            return image;
        }
        public Vector2 Mirror(Vector2 point,Vector2 mirrorPos, int a)//,Vector3 direction)
        {
            //int a = 1;
            if (point.x-mirrorPos.x < a * (point.y-mirrorPos.y)) return new Vector2(a * (point.y-mirrorPos.y), a * (point.x-mirrorPos.x))+mirrorPos;
            else return point;
        }

        public Rayable drawable = new Rayable(new Vector2(21f, 20f), new Vector2(1, 1), 0f);
        public DistanceResult Distance(Vector2 a)
        {
            //float ix = a.x, iy = a.y;
            Vector2 b = a * 1;
            b = Mirror(b,new Vector2(20,20), -1);
            b = Mirror(b, new Vector2(25, 25), -1);
            b = Mirror(b, new Vector2(20, 20), 1);
            b = Mirror(b, new Vector2(25, 25), 1);
            //b.y = 25-Math.Abs(b.y-25);
            bool unflipped = (((int)(b.x) == (int)(a.x)))&& (((int)(b.y) == (int)(a.y)));

            return new DistanceResult(drawable.NearestPoint(b),drawable.NearestPoint(a),unflipped);
        }
        public struct DistanceResult
        {
            public float distance;
            public bool unflipped;
            public float unfDistance;
            public DistanceResult(float a,float b, bool c)
            {
                this.distance = a;
                this.unfDistance = b;
                this.unflipped = c;
            }
        }
        public class Vector2
        {
            public float x, y;
            public Vector2(float x,float y)
            {
                this.x = x;
                this.y = y;
            }
            public float Length
            {
                get
                {
                    return (float)(Math.Sqrt(this.x * this.x + this.y * this.y));
                }
            }
            public static Vector2 operator *(Vector2 a, float coefficient)
            {
                return new Vector2(a.x * coefficient, a.y * coefficient);
            }
            public static Vector2 operator -(Vector2 a, Vector2 b)
            {
                return new Vector2(a.x - b.x, a.y - b.y);
            }
            public static Vector2 operator +(Vector2 a, Vector2 b)
            {
                return new Vector2(a.x + b.x, a.y + b.y);
            }
            public static Vector2 zero = new Vector2(0, 0);

            public Vector2 Positive
            {
                get
                {
                    return new Vector2(Math.Abs(this.x), Math.Abs(this.y));
                }
            }
            public static Vector2 Max(Vector2 a, Vector2 b)
            {
                return new Vector2((a.x < b.x) ? b.x : a.x, (a.y < b.y) ? b.y : a.y);
            }
        }
        public class Rayable
        {

            public Vector2 Position;
            public Vector2 Scale;
            public float Radius;
            public Rayable(Vector2 position, Vector2 scale, float radius)
            {
                this.Position = position;
                this.Scale = scale;
                this.Radius = radius;
            }
            public float NearestPoint(Vector2 point)
            {
                //      |point-position|-scale
                return Vector2.Max(((point - this.Position).Positive - this.Scale), Vector2.zero).Length - this.Radius;

                //return (this.Position - point).Length - this.Radius;
            }
        }

        public const int resolution = 40;

    }
}
