using System.Collections.Generic;
using UnityEngine;
using Utils;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Geom
{
    public class Polygon2dAsset : ScriptableObject
    {
#if UNITY_EDITOR
		[MenuItem("Assets/Create/Fugure")]
        public static void CreateAsset()
        {
            Polygon2dAsset asset = ScriptableObjectUtility.CreateAsset<Polygon2dAsset>();

            GameObject figure = new GameObject(string.Format("Figure_{0}", Random.Range(0, int.MaxValue)), typeof(Polygon2d));
            Polygon2d polygon = figure.GetComponent<Polygon2d>();
            polygon.Polygon = asset;
        }

        public static void SaveAssets(Polygon2dAsset asset)
        {
            if (asset == null)
                return;

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
#endif

		public List<Vector2> Points;

		public float SegmentsOffset = 0.3f;

		public void GenerateSegmentsData(List<Vector2> outPoints, List<PolygonSegment> outSegments)
		{
			outPoints.Clear();
			outPoints.AddRange(Points);

			Vector2 center = Utils.MathUtils.GetCenter(Points);

			for(int i = 0; i < outPoints.Count; i++)
			{
				outPoints[i] -= center;
			}

			outSegments.Clear();
			for(int i = 0; i < outPoints.Count - 1; i++)
			{
				outSegments.Add(PolygonSegment.Create(outPoints[i], outPoints[i + 1], SegmentsOffset));
			}
		}
    }
}
