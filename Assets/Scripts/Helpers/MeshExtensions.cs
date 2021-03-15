
using UnityEngine;

namespace Helpers
{
    public static class MeshExtensions
    {
        public static void SetColor(this Mesh mesh, Color color)
        {
            Color[] colors = new Color[mesh.vertices.Length];
            for (int i = 0; i < mesh.vertexCount; i++)
            {
                colors[i] = color;
            }

            mesh.SetColors(colors);
        }  
        
        public static void SetColor(Mesh mesh, Color color, bool value)
        {
            Color[] colors = new Color[mesh.vertices.Length];
            for (int i = 0; i < mesh.vertexCount; i++)
            {
                colors[i] = color;
            }

            mesh.SetColors(colors);
        }
    }
}