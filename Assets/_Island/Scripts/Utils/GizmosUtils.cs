using UnityEngine;

namespace Project.Utils
{
    public static class GizmosUtils
    {
        /// <summary>
        /// Editor code to create a horizontal gizmos circle
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="color"></param>
        public static void DrawHorizontalCircle(Vector3 center, float radius, Color color)
        {
            Vector3 prevPos = center + new Vector3(radius, 0, 0);
            for (int i = 0; i < 30; i++)
            {
                float angle = (float)(i + 1) / 30.0f * Mathf.PI * 2.0f;
                Vector3 newPos = center + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
                Debug.DrawLine(prevPos, newPos, color);
                prevPos = newPos;
            }
        }

        /// <summary>
        /// Editor code to create a vertical gizmos circle
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="color"></param>
        public static void DrawVerticalCircle(Vector3 center, float radius, Color color)
        {
            Vector3 prevPos = center + new Vector3(0, radius, 0);
            for (int i = 0; i < 30; i++)
            {
                float angle = (float)(i + 1) / 30.0f * Mathf.PI * 2.0f;
                Vector3 newPos = center + new Vector3(0, Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
                Debug.DrawLine(prevPos, newPos, color);
                prevPos = newPos;
            }
        }
    }
}
