using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Geom;
using Utils;

namespace Data
{
    public class LevelDataAsset: ScriptableObject
    {
		
#if UNITY_EDITOR
        [MenuItem("Assets/Create/Figure Level")]
        public static void CreateAsset()
        {
            LevelDataAsset asset = ScriptableObjectUtility.CreateAsset<LevelDataAsset>();
        }
#endif

		[Serializable]
		public class LevelStepData
        {
			public Polygon2dAsset Figure;

			public int Time;
		}

        public List<LevelStepData> Steps;


    }
}
