using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

using OpenTK.Graphics.OpenGL;

using OpenTK3_StandardTemplate_WinForms.helpers;
using OpenTK3_StandardTemplate_WinForms.objects;

namespace OpenTK3_StandardTemplate_WinForms
{
    public partial class MainForm : Form
    {
        private Axes mainAxis;
        private Rectangles re;
        private Camera cam;
        private Scene scene;
        private Point mousePosition;

        public MainForm()
        {   
            // general init
            InitializeComponent();
            this.Text = "Adonicioaie Ovidiu - 3131A";

            // init VIEWPORT
            scene = new Scene();

            scene.GetViewport().Load += new EventHandler(this.mainViewport_Load);
            scene.GetViewport().Paint += new PaintEventHandler(this.mainViewport_Paint);
            scene.GetViewport().MouseMove += new MouseEventHandler(this.mainViewport_MouseMove);

            this.Controls.Add(scene.GetViewport());
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // init RNG
            Randomizer.Init();

            // init CAMERA/EYE
            cam = new Camera(scene.GetViewport());

            // init AXES
            mainAxis = new Axes(showAxes.Checked);
            re = new Rectangles();

            GL.Enable(EnableCap.Lighting); // Asigură-te că iluminarea generală este activată
            GL.Light(LightName.Light0, LightParameter.Position, new float[] { 0, 0, 10, 1 });
            GL.Light(LightName.Light0, LightParameter.Diffuse, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
            GL.Enable(EnableCap.Light0); // Activăm sursa de lumină 0

            GL.Enable(EnableCap.ColorMaterial);
            GL.ColorMaterial(MaterialFace.FrontAndBack, ColorMaterialParameter.AmbientAndDiffuse);
        }

        private void showAxes_CheckedChanged(object sender, EventArgs e)
        {
            mainAxis.SetVisibility(showAxes.Checked);

            scene.Invalidate();
        }

        private void changeBackground_Click(object sender, EventArgs e)
        {
            GL.ClearColor(Randomizer.GetRandomColor());

            scene.Invalidate();
        }

        private void resetScene_Click(object sender, EventArgs e)
        {
            showAxes.Checked = true;
            mainAxis.SetVisibility(showAxes.Checked);
            scene.Reset();
            cam.Reset();

            scene.Invalidate();
        }

        private void mainViewport_Load(object sender, EventArgs e)
        {
            scene.Reset();
        }

        private void mainViewport_MouseMove(object sender, MouseEventArgs e)
        {
            mousePosition = new Point(e.X, e.Y);
            scene.Invalidate();
        }

        private void mainViewport_Paint(object sender, PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            cam.SetView();

            if (enableRotation.Checked == true)
            {
                // Doar după axa Ox.
                GL.Rotate(Math.Max(mousePosition.X, mousePosition.Y), 1, 1, 1);
            }
            DrawWireframeCube();
            // GRAPHICS PAYLOAD
            mainAxis.Draw();

            if (enableObjectRotation.Checked == true)
            {
                // Rotatie a obiectului
                GL.PushMatrix();
                GL.Rotate(Math.Max(mousePosition.X, mousePosition.Y), 1, 1, 1);
                re.Draw();
                GL.PopMatrix();
            } else
            {
                re.Draw();
            }
            if (radioButton1.Checked)
            {
                GL.Color3(Color.Yellow); // Setează culoarea liniei
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(-20f, 20f, 15f); // Poziția sursei de lumină
                GL.Vertex3(0, 0, 0); // Punctul (0,0,0)
                GL.End();
            }

            scene.GetViewport().SwapBuffers();
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            GL.Light(LightName.Light0, LightParameter.Position, new float[] { 0, 0, 10, 1 });
            // Resetăm culoarea sursei de lumină
            GL.Light(LightName.Light0, LightParameter.Diffuse, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
        }

        private void DrawWireframeCube()
        {
            /*GL.Begin(PrimitiveType.LineLoop);
            for (int i = 0; i < re.coordonates.Count; i++)
            {
                GL.Color3((Color)re.colors[i]);
                Coords aux = (Coords)re.coordonates[i];
                GL.Vertex3(aux.X, aux.Y, aux.Z);
            }

            // Desenăm fețele paralelipipedului folosind coordonatele din ArrayList
            for (int i = 0; i < 4; i++)
            {
                Coords coord = (Coords)re.coordonates[i];
                GL.Vertex3(coord.X, coord.Y, coord.Z);
            }
            GL.End();

            GL.Begin(PrimitiveType.LineLoop);
            for (int i = 4; i < 8; i++)
            {
                Coords coord = (Coords)re.coordonates[i];
                GL.Vertex3(coord.X, coord.Y, coord.Z);
            }
            GL.End();

            GL.Begin(PrimitiveType.Lines);
            // Conectăm vârfurile între cele două fețe
            for (int i = 0; i < 4; i++)
            {
                GL.Vertex3(((Coords)re.coordonates[i]).X, ((Coords)re.coordonates[i]).Y, ((Coords)re.coordonates[i]).Z);
                GL.Vertex3(((Coords)re.coordonates[i + 4]).X, ((Coords)re.coordonates[i + 4]).Y, ((Coords)re.coordonates[i + 4]).Z);
            }
            GL.End();*/
            GL.Begin(PrimitiveType.LineLoop);
            for (int i = 0; i < re.coordonates.Count; i++)
            {
                GL.Color3((Color)re.colors[i]);  // Potential issue here
                Coords aux = (Coords)re.coordonates[i];
                GL.Vertex3(aux.X, aux.Y, aux.Z);
            }

            // Desenăm fețele paralelipipedului folosind coordonatele din ArrayList
            for (int i = 0; i < 4; i++)
            {
                Coords coord = (Coords)re.coordonates[i];
                GL.Vertex3(coord.X, coord.Y, coord.Z);
            }
            GL.End();

            GL.Begin(PrimitiveType.LineLoop);
            for (int i = 4; i < 8; i++)
            {
                Coords coord = (Coords)re.coordonates[i];
                GL.Vertex3(coord.X, coord.Y, coord.Z);
            }
            GL.End();

            GL.Begin(PrimitiveType.Lines);
            // Conectăm vârfurile între cele două fețe
            for (int i = 0; i < 4; i++)
            {
                GL.Vertex3(((Coords)re.coordonates[i]).X, ((Coords)re.coordonates[i]).Y, ((Coords)re.coordonates[i]).Z);
                GL.Vertex3(((Coords)re.coordonates[i + 4]).X, ((Coords)re.coordonates[i + 4]).Y, ((Coords)re.coordonates[i + 4]).Z);
            }
            GL.End();
        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            scene.Invalidate();
        }

        private void enableLight_CheckedChanged(object sender, EventArgs e)
        {
            if (enableLight.Checked)
            {
                GL.Enable(EnableCap.Light0);
            }
            else
            {
                GL.Disable(EnableCap.Light0);
            }
            scene.Invalidate();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            scene.Invalidate();
        }
    }
}
