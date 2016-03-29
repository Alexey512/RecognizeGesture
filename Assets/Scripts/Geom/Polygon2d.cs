using System;
using System.Collections.Generic;
using UnityEngine;

namespace Geom
{
    public class Polygon2d: MonoBehaviour 
    {
        public Polygon2dAsset Polygon;

        void Awake()
        {
        }

        void Start()
        {
        }

        private void OnDrawGizmos()
        {
			if (Polygon == null || Polygon.Points == null)
                return;

            List<Vector2> points = Polygon.Points;

            for (int i = 0; i < points.Count - 1; i++)
            {
				Gizmos.color = Color.green;

				Vector2 p0 = transform.TransformPoint(points[i]);
				Vector2 p1 = transform.TransformPoint(points[i + 1]);
				Gizmos.DrawLine(p0, p1);

				Gizmos.color = Color.yellow;

				PolygonSegment segment = PolygonSegment.Create(p0, p1, Polygon.SegmentsOffset);
				Gizmos.DrawLine(segment.LeftTop, segment.RightTop);
				Gizmos.DrawLine(segment.LeftButtom, segment.RightButtom);
            }
        }

        public void AddPoint(Vector2 point)
        {
			if (Polygon == null || Polygon.Points == null)
                return;

            Polygon.Points.Add(point);
        }

        public void RemovePoint(int index)
        {
			if (Polygon == null || Polygon.Points == null)
                return;

            if (index < 0 || index >= Polygon.Points.Count)
                return;

            Polygon.Points.RemoveAt(index);
        }

        public Vector2 GetPoint(int index)
        {
			if (Polygon == null || Polygon.Points == null)
                return Vector2.zero;

            if (index < 0 || index >= Polygon.Points.Count)
                return Vector2.zero;

            return Polygon.Points[index];
        }

        public void SetPoint(int index, Vector2 value)
        {
			if (Polygon == null || Polygon.Points == null)
                return;

            if (index < 0 || index >= Polygon.Points.Count)
                return;

            Polygon.Points[index] = value;
        }

        public int GetPointsCount()
        {
			if (Polygon == null || Polygon.Points == null)
                return 0;

            return Polygon.Points.Count;
        }
    }
}
