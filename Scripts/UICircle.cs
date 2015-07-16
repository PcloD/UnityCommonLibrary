using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityCommonLibrary {
    /// <summary>
    /// https://bitbucket.org/Elforama/uicircle
    /// </summary>
    [ExecuteInEditMode]
    public class UICircle : Graphic {

        #region VARIABLES

        /// <summary>
        /// Sets if the circle uses a gradient or not
        /// </summary>
        [SerializeField]
        private bool gradient = false;

        /// <summary>
        /// The radius of the circle
        /// </summary>
        [SerializeField]
        private float radius = 10;

        /// <summary>
        /// The thickness of the circle
        /// 0 being a solid circle, 1 being a super thin ring (or invisible)
        /// </summary>
        [SerializeField]
        private float thickness = 0;

        /// <summary>
        /// The amount of subdivisions to sharpen the circle
        /// </summary>
        [SerializeField]
        private int subdivisions = 3;

        /// <summary>
        /// Can have no less than 4 divisions
        /// </summary>
        private int baseDivision = 4;

        /// <summary>
        /// The angle the circle is drawn up to (180 draws half a circle)
        /// </summary>
        [SerializeField]
        private float angle;

        /// <summary>
        /// The angle offset dertermines what angle the circle
        /// begins drawing itself
        /// </summary>
        [SerializeField]
        private float angleOffset = 0;

        /// <summary>
        /// When flipped the angle will affect the circle in the opposite direction
        /// </summary>
        [SerializeField]
        private bool flip = false;

        /// <summary>
        /// The color of the gradient.
        /// </summary>
        public Color gradientColor;

        /// <summary>
        /// Sets glow on or off
        /// </summary>
        [SerializeField]
        private bool glow = false;

        /// <summary>
        /// The glow thickness.
        /// </summary>
        [SerializeField]
        private float glowThickness = .2f;

        /// <summary>
        /// The color of the glow.
        /// </summary>
        public Color glowColor;

        /// <summary>
        /// Transparent color to fade glow color to
        /// </summary>
        private Color glowFade = new Color(0, 0, 0, 0);

        #endregion VARIABLES

        #region GET/SET

        public bool Gradient {
            get { return gradient; }
            set {
                gradient = value;
                OnRebuildRequested();
            }
        }

        public float Radius {
            get { return radius; }
            set {
                radius = value;
                OnRebuildRequested();
            }
        }

        public float Thickness {
            get { return thickness; }
            set {
                thickness = value;
                OnRebuildRequested();
            }
        }

        public int Subdivisions {
            get { return subdivisions; }
            set {
                subdivisions = value;
                OnRebuildRequested();
            }
        }

        public float Angle {
            get { return angle; }
            set {
                angle = value;
                OnRebuildRequested();
            }
        }

        public float AngleOffset {
            get { return angleOffset; }
            set {
                angleOffset = value;
                OnRebuildRequested();
            }
        }

        public bool Flip {
            get { return flip; }
            set {
                flip = value;
                OnRebuildRequested();
            }
        }

        public bool Glow {
            get { return glow; }
            set {
                glow = value;
                OnRebuildRequested();
            }
        }

        public float GlowThickness {
            get { return glowThickness; }
            set {
                glowThickness = value;
                OnRebuildRequested();
            }
        }

        #endregion GET/SET

        protected override void OnFillVBO(List<UIVertex> vbo) {
            vbo.Clear();

            float innerRadius = radius - (radius * thickness);

            //Draw the initial circle
            if(gradient)
                DrawCircle(vbo, innerRadius, radius, gradientColor, color);
            else
                DrawCircle(vbo, innerRadius, radius, color, color);
            //Draw the glow
            if(glow && thickness > 0) {
                float glowWidth = glowThickness * radius;

                //Outer glow
                float outerGlowRadius = radius + glowWidth;
                DrawCircle(vbo, radius, outerGlowRadius, glowColor, glowFade);
                //Inner glow
                if(thickness != 1) {//no need to draw center glow is there is no gap
                                    //make sure glow does not overlap circle at center
                    float innerGlowRadius = Mathf.Clamp(innerRadius - glowWidth, 0, innerRadius);
                    DrawCircle(vbo, innerGlowRadius, innerRadius, glowFade, glowColor);
                }
            }
        }

        /// <summary>
        /// Draws the circle.
        /// </summary>
        /// <param name="vbo">Vbo.</param>
        /// <param name="innerRadius">Inner radius size.</param>
        /// <param name="outerRadius">Outer radius size.</param>
        /// <param name="innerColor">Inner color.</param>
        /// <param name="outerColor">Outer color.</param>
        private void DrawCircle(List<UIVertex> vbo, float innerRadius, float outerRadius, Color innerColor, Color outerColor) {
            //gets the amount of points to make the circles circumference
            float steps = Mathf.Pow(baseDivision, subdivisions);
            //gets the angle between each point
            float stepAngle = angle / steps;

            Vector2 centerPoint = rectTransform.pivot;

            Vector2 currentPoint;
            UIVertex vert = UIVertex.simpleVert;

            int sign;
            if(flip)
                sign = -1;
            else
                sign = 1;

            //get all points to make circle
            for(int step = 0; step < steps; step++) {
                //get the first angle from 0 degrees
                float angle1 = Mathf.Deg2Rad * (sign * stepAngle * step + angleOffset);
                //get the second angle from 0 degrees
                float angle2 = Mathf.Deg2Rad * (sign * stepAngle * (step + 1) + angleOffset);

                //get outside point 1
                currentPoint = new Vector2();
                currentPoint.x = centerPoint.x + outerRadius * Mathf.Cos(angle2);
                currentPoint.y = centerPoint.y + outerRadius * Mathf.Sin(angle2);
                vert.position = currentPoint;
                vert.color = outerColor;
                vbo.Add(vert);

                //get inside point 1
                currentPoint = new Vector2();
                currentPoint.x = centerPoint.x + innerRadius * Mathf.Cos(angle2);
                currentPoint.y = centerPoint.y + innerRadius * Mathf.Sin(angle2);
                vert.position = currentPoint;
                vert.color = innerColor;
                vbo.Add(vert);

                //get inside point 2
                currentPoint = new Vector2();
                currentPoint.x = centerPoint.x + innerRadius * Mathf.Cos(angle1);
                currentPoint.y = centerPoint.y + innerRadius * Mathf.Sin(angle1);
                vert.position = currentPoint;
                vert.color = innerColor;
                vbo.Add(vert);

                //get outside point 2
                currentPoint = new Vector2();
                currentPoint.x = centerPoint.x + outerRadius * Mathf.Cos(angle1);
                currentPoint.y = centerPoint.y + outerRadius * Mathf.Sin(angle1);
                vert.position = currentPoint;
                vert.color = outerColor;
                vbo.Add(vert);
            }
        }

        public override void OnRebuildRequested() {
            base.OnRebuildRequested();
            ResizeRect();
        }

        protected override void OnRectTransformDimensionsChange() {
            ResizeRect();
        }

        /// <summary>
        /// Stop rect from resized when manipulated.
        /// </summary>
        private void ResizeRect() {
            float size;
            if(glow)
                size = ((glowThickness * radius) + radius) * 2;
            else
                size = radius * 2;
            rectTransform.sizeDelta = new Vector2(size, size);
        }
    }
}