using System;
using System.Collections;
using System.Drawing;

using OpenTK.Graphics.OpenGL;

using OpenTK3_StandardTemplate_WinForms.helpers;

namespace OpenTK3_StandardTemplate_WinForms.objects
{
    class Rectangles
    {
        public ArrayList coordonates;
        public ArrayList colors;
        private PolygonMode currentPolygonState = PolygonMode.Fill;
        private bool visibility;

        public Rectangles()
        {
            coordonates = new ArrayList();
            coordonates.Add(new Coords(-5, 5, 5));   // V1
            coordonates.Add(new Coords(-5, 5, 25));  // V2
            coordonates.Add(new Coords(-35, 5, 25)); // V3
            coordonates.Add(new Coords(-35, 5, 5));  // V4
            coordonates.Add(new Coords(-5, 40, 5));  // V5
            coordonates.Add(new Coords(-5, 40, 25)); // V6
            coordonates.Add(new Coords(-35, 40, 25)); // V7
            coordonates.Add(new Coords(-35, 40, 5));  // V8

            colors = new ArrayList();
            for (int i = 0; i < 8; i++)
            {
                colors.Add(Randomizer.GetRandomColor());
            }

            visibility = true;
        }

        public bool GetVisibility()
        {
            return visibility;
        }

        public void SetVisibility(bool _visibility)
        {
            visibility = _visibility;
        }

        public void Show()
        {
            visibility = true;
        }

        public void Hide()
        {
            visibility = false;
        }

        public void ToggleVisibility()
        {
            visibility = !visibility;
        }

        public void PolygonDrawingStyle(String style)
        {
            if (style == "line")
            {
                currentPolygonState = PolygonMode.Line;
                return;
            }

            if (style == "surface")
            {
                currentPolygonState = PolygonMode.Fill;
                return;
            }

        }

        public void Draw()
        {
            if (!visibility)
            {
                return;
            }

            GL.PolygonMode(MaterialFace.FrontAndBack, currentPolygonState);
            GL.Begin(PrimitiveType.Quads);
            for (int i = 0; i < 4; i++)
            {
                Color color;
                if (GL.IsEnabled(EnableCap.Lighting))
                {
                    // Culoare aleatorie cu iluminare activată
                    color = Randomizer.GetRandomColor();
                    float[] colorArray = { color.R / 255.0f * 0.5f, color.G / 255.0f * 0.5f, color.B / 255.0f * 0.5f, 1.0f }; // Reducem intensitatea
                    GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, colorArray);
                }
                else
                {
                    // Culoare din lista pentru curcubeu
                    color = (Color)colors[i];
                }
                GL.Color3(color);
                Coords aux = (Coords)coordonates[i];
                GL.Vertex3(aux.X, aux.Y, aux.Z);
            }
            GL.End();
        }
    }
}
