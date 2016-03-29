using UnityEngine;
using Utils;


namespace Geom
{
	public class PolygonSegment
	{
		public Vector2 LeftTop;

		public Vector2 RightTop;

		public Vector2 LeftButtom;

		public Vector2 RightButtom;

		public bool Contains(Vector2 point)
		{
			return MathUtils.PointAlign(point, RightTop, LeftTop) <= 0 &&
				MathUtils.PointAlign(point, RightButtom, RightTop) <= 0 &&
				MathUtils.PointAlign(point, LeftButtom, RightButtom) <= 0 &&
				MathUtils.PointAlign(point, LeftTop, LeftButtom) <= 0;
		}

		public static PolygonSegment Create(Vector2 p1, Vector2 p2, float offset)
		{
			Vector2 dir = p2 - p1;
			dir.Normalize();
			Vector2 norm = new Vector2(-dir.y, dir.x);

			Vector2 right = p2 + dir * offset;
			Vector2 left = p1 - dir * offset;

			return new PolygonSegment
			{
				LeftTop = left + norm * offset,
				RightTop = right + norm * offset,
				LeftButtom = left - norm * offset,
				RightButtom = right - norm * offset
			};
		}
	}
}
