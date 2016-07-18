using UnityEngine;
using UnityEngine.UI;

namespace UnityCommonLibrary.UI
{
    /// <summary>
    /// https://bitbucket.org/Elforama/uicircle
    /// </summary>
    [ExecuteInEditMode]
    public class UICircle : Graphic
    {
        [Range(0f, 1f)]
        public float fillPercent;
        public bool fill = true;
        public int thickness = 5;
        [Range(0, 50)]
        public int segments = 50;

        public override Texture mainTexture
        {
            get
            {
                return s_WhiteTexture;
            }
        }

        private void Update()
        {
            thickness = (int)Mathf.Clamp(thickness, 0, rectTransform.rect.width / 2);
        }

        protected UIVertex[] SetVBO(Vector2[] vertices, Vector2[] uvs)
        {
            var vbo = new UIVertex[4];
            for (var i = 0; i < vertices.Length; i++)
            {
                var vert = UIVertex.simpleVert;
                vert.color = color;
                vert.position = vertices[i];
                vert.uv0 = uvs[i];
                vbo[i] = vert;
            }
            return vbo;
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            var outer = -rectTransform.pivot.x * rectTransform.rect.width;
            var inner = -rectTransform.pivot.x * rectTransform.rect.width + thickness;

            vh.Clear();
            var vert = UIVertex.simpleVert;
            var prevX = Vector2.zero;
            var prevY = Vector2.zero;
            var uv0 = new Vector2(0, 0);
            var uv1 = new Vector2(0, 1);
            var uv2 = new Vector2(1, 1);
            var uv3 = new Vector2(1, 0);
            Vector2 pos0;
            Vector2 pos1;
            Vector2 pos2;
            Vector2 pos3;
            var degrees = 360f / segments;
            var fillAngle = (int)((segments + 1) * fillPercent);
            for (int i = 0; i < fillAngle; i++)
            {
                var rad = Mathf.Deg2Rad * (i * degrees);
                var c = Mathf.Cos(rad);
                var s = Mathf.Sin(rad);
                var x = outer * c;
                var y = inner * c;
                uv0 = new Vector2(0, 1);
                uv1 = new Vector2(1, 1);
                uv2 = new Vector2(1, 0);
                uv3 = new Vector2(0, 0);
                pos0 = prevX;
                pos1 = new Vector2(outer * c, outer * s);
                if (fill)
                {
                    pos2 = Vector2.zero;
                    pos3 = Vector2.zero;
                }
                else
                {
                    pos2 = new Vector2(inner * c, inner * s);
                    pos3 = prevY;
                }
                prevX = pos1;
                prevY = pos2;
                vh.AddUIVertexQuad(SetVBO(new[] { pos0, pos1, pos2, pos3 }, new[] { uv0, uv1, uv2, uv3 }));
            }
        }
    }
}