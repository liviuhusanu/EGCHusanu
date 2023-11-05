using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace EGCHusanu
{
    internal class Window3D : GameWindow
    {
        private int vertexArray;
        private int vertexBuffer;
        KeyboardState previousKeyboard;
        Randomizer rando;
        Color DEFAULT_BKG_COLOR = Color.LightBlue;

        private List<Vector3> triangleVertices = new List<Vector3>();
        private float colorR = 1.0f, colorG = 1.0f, colorB = 1.0f;
        private float colorRangeMin = 0.0f, colorRangeMax = 1.0f;
        private float rotationAngle = 0.0f;

        public Window3D() : base(800, 600, new GraphicsMode(32, 24, 0, 8))
        {
            VSync = VSyncMode.On;

            displayHelp();
            rando = new Randomizer();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LoadDataFromFile("data.txt");

            GL.ClearColor(DEFAULT_BKG_COLOR);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);

            // Define the vertices of the triangle
        //    float[] vertices = {
        //    0.0f, 3.0f, 0.0f,
        //   -3.0f, -3.0f, 0.0f,
        //    3.0f, -3.0f, 0.0f
        //};

        //    GL.GenVertexArrays(1, out vertexArray);
        //    GL.BindVertexArray(vertexArray);

        //    GL.GenBuffers(1, out vertexBuffer);
        //    GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
        //    GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        //    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        //    GL.EnableVertexAttribArray(0);

        //    GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        //    GL.BindVertexArray(0);
        }

        protected void LoadDataFromFile(string filename)
        {
            using (StreamReader reader = new StreamReader("C:\\Users\\Liviu\\source\\repos\\EGCHusanu\\EGCHusanu\\data.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(' ');

                    if (parts[0] == "TriangleCoordinates:")
                    {
                        for (int i = 1; i < parts.Length; i += 3)
                        {
                            float x = float.Parse(parts[i]);
                            float y = float.Parse(parts[i + 1]);
                            float z = float.Parse(parts[i + 2]);
                            triangleVertices.Add(new Vector3(x, y, z));
                        }
                    }
                    else if (parts[0] == "ColorRanges:")
                    {
                        for (int i = 1; i < parts.Length; i += 3)
                        {
                            string color = parts[i];
                            float min = float.Parse(parts[i + 1]);
                            float max = float.Parse(parts[i + 2]);

                            if (color == "R")
                            {
                                colorR = min;
                                colorRangeMin = min;
                                colorRangeMax = max;
                            }
                            else if (color == "G")
                            {
                                colorG = min;
                                colorRangeMin = min;
                                colorRangeMax = max;
                            }
                            else if (color == "B")
                            {
                                colorB = min;
                                colorRangeMin = min;
                                colorRangeMax = max;
                            }
                        }
                    }
                }
            }
        }

        //protected override void OnResize(EventArgs e)
        //{
        //    base.OnResize(e);

        //    //viewpoint
        //    GL.Viewport(0, 0, this.Width, this.Height);

        //    //perspectiva
        //    Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)Width / (float)Height, 1, 256);
        //    GL.MatrixMode(MatrixMode.Projection);
        //    GL.LoadMatrix(ref perspective);

        //    //camera
        //    Matrix4 lookat = Matrix4.LookAt(30, 30, 30, 0, 0, 0, 0, 1, 0);
        //    GL.MatrixMode(MatrixMode.Modelview);
        //    GL.LoadMatrix(ref lookat);
        //}

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            //cod logica

            KeyboardState thisKeyboard = Keyboard.GetState();
            MouseState thisMouse = Mouse.GetState();

            if (thisKeyboard.IsKeyDown(Key.R))
            {
                colorR += 0.1f;
                if (colorR > colorRangeMax)
                    colorR = colorRangeMin;
            }
            if (thisKeyboard.IsKeyDown(Key.G))
            {
                colorG += 0.1f;
                if (colorG > colorRangeMax)
                    colorG = colorRangeMin;
            }
            if (thisKeyboard.IsKeyDown(Key.B))
            {
                colorB += 0.1f;
                if (colorB > colorRangeMax)
                    colorB = colorRangeMin;
            }

            if (thisKeyboard[Key.Escape])
            {
                Exit();
            }

            if (thisKeyboard[Key.H] && !previousKeyboard[Key.H])
            {
                displayHelp();
            }

            if (thisKeyboard[Key.R] && !previousKeyboard[Key.R])
            {
                GL.ClearColor(DEFAULT_BKG_COLOR);
            }

            if (thisKeyboard[Key.B] && !previousKeyboard[Key.B])
            {
                GL.ClearColor(rando.GetRandomColor());
            }

            var mouseState = Mouse.GetCursorState();
            rotationAngle = (float)mouseState.X * 0.01f;

            previousKeyboard = thisKeyboard;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.Rotate(rotationAngle, 0.0f, 0.0f, 1.0f);

            GL.Begin(PrimitiveType.Triangles);
            GL.Color3(colorR, colorG, colorB);
            for (int i = 0; i < triangleVertices.Count; i++)
            {
                GL.Vertex3(triangleVertices[i]);
            }
            GL.End();

            //cod render
            SwapBuffers();
        }

        //protected override void OnUnload(EventArgs e)
        //{
        //    base.OnUnload(e);
        //    GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        //    GL.BindVertexArray(0);
        //    GL.DeleteBuffer(vertexBuffer);
        //    GL.DeleteVertexArray(vertexArray);
        //}

        private void displayHelp()
        {
            Console.WriteLine("\n            Meniu");
            Console.WriteLine("ESC - Parasire aplicatie");
            Console.WriteLine("H - Meniu de ajutor");
            Console.WriteLine("B - Schimba culoarea de background");
            Console.WriteLine("R - Reset");
        }
    }
}
