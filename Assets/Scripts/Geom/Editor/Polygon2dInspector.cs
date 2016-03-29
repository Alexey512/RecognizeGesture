using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Geom
{
    [CustomEditor(typeof(Polygon2d))]
    public class Polygon2dInspector : Editor
    {
        private int selectedIndex = -1;

        private Transform handleTransform;

        private Quaternion handleRotation;

        private const float handleSize = 0.04f;

        private const float pickSize = 0.06f;

        public override void OnInspectorGUI()
        {
            Polygon2d polygon = target as Polygon2d;

            EditorGUI.BeginChangeCheck();
            object polygonAsset = EditorGUILayout.ObjectField("Polygon", polygon.Polygon, typeof(Polygon2dAsset), true);
            if (EditorGUI.EndChangeCheck())
            {
                polygon.Polygon = polygonAsset as Polygon2dAsset;
                EditorUtility.SetDirty(polygon);
            }

            if (selectedIndex >= 0 && selectedIndex < polygon.GetPointsCount())
            {
                DrawSelectedPointInspector(polygon);
            }

            if (GUILayout.Button("Add Point"))
            {
                Undo.RecordObject(polygon, "Add Point");
                polygon.AddPoint(Vector2.one * polygon.GetPointsCount());
                EditorUtility.SetDirty(polygon);
            }

            if (GUILayout.Button("Save Asset"))
            {
                Polygon2dAsset.SaveAssets(polygon.Polygon);
                EditorUtility.SetDirty(polygon);
            }
        }

        private void DrawSelectedPointInspector(Polygon2d polygon)
        {
            GUILayout.Label("Selected Point");
            EditorGUI.BeginChangeCheck();
            Vector3 point = EditorGUILayout.Vector3Field("Position", polygon.GetPoint(selectedIndex));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(polygon, "Move Point");
                EditorUtility.SetDirty(polygon);
                polygon.SetPoint(selectedIndex, point);
            }

            if (GUILayout.Button("Remove Point"))
            {
                Undo.RecordObject(polygon, "Remove Point");
                polygon.RemovePoint(selectedIndex);
                EditorUtility.SetDirty(polygon);
            }
        }

        private void OnSceneGUI()
        {
            Polygon2d polygon = target as Polygon2d;

            Transform handleTransform = polygon.transform;
            Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ?
                handleTransform.rotation : Quaternion.identity;

            int cnt = polygon.GetPointsCount();
            for (int i = 0; i < cnt; i++)
            {
                Vector2 point = handleTransform.TransformPoint(polygon.GetPoint(i));
                float size = HandleUtility.GetHandleSize(point);
                if (Handles.Button(point, handleRotation, size * handleSize, size * pickSize, Handles.DotCap))
                {
                    selectedIndex = i;
                    Repaint();
                }
                if (selectedIndex == i)
                {
                    EditorGUI.BeginChangeCheck();
                    point = Handles.DoPositionHandle(point, handleRotation);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(polygon, "Move Point");
                        EditorUtility.SetDirty(polygon);
                        polygon.SetPoint(i, handleTransform.InverseTransformPoint(point));
                    }
                }

            }
        }

        
    }
}
