using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Utils
{

	public static class MathUtils
	{
		public static float PointAlign(Vector2 p, Vector2 p1, Vector2 p2)
		{
			Vector2 dir = p2 - p1;
			Vector2 norm = new Vector2(-dir.y, dir.x);
			float a = -norm.x;
			float b = -norm.y;
			float c = -a * p1.x - b * p1.y;

			return a * p.x + b * p.y + c;
		}

		public static float GetTotalLength(List<Vector2> polygon)
		{
			float length = 0;
			for (int i = 0; i < polygon.Count - 1; i++)
			{
				length += Vector2.Distance(polygon[i], polygon[i + 1]);
			}
			return length;
		}

		public static Vector2 GetCenter(List<Vector2> polygon)
		{
			Vector2 center = Vector2.zero;

			float p = 0;

			int cnt = polygon.Count;

			for(int i = 0; i < cnt; i++)
			{
				float dist = Vector2.Distance(polygon[i], polygon[(i + 1) % cnt]);
				center += dist * (polygon[i] + polygon[(i + 1) % cnt]) / 2;
				p += dist;
			}

			center /= p;

			return center;
		}

		public static Vector2 GetBound(List<Vector2> polygon)
		{
			Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
			Vector2 max = new Vector2(float.MinValue, float.MinValue);

			for(int i = 0; i < polygon.Count; i++)
			{
				Vector2 point = polygon[i];
				min.x = Mathf.Min(min.x, point.x);
				min.y = Mathf.Min(min.y, point.y);
				max.x = Mathf.Max(max.x, point.x);
				max.y = Mathf.Max(max.y, point.y);
			}

			return max - min;
		}

	}
}