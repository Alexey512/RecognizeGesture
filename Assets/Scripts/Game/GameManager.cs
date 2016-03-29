using Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utils;
using Geom;

namespace Game
{
	public enum eGameState
	{
		Pause,
		Play,
		Complete,
		Fail
	}

	public class GameManager : MonoBehaviour
	{
		public UnityEvent OnChangeFigure;

		public UnityEvent OnFigureComplete;

		public UnityEvent OnFigureFail;

		public UnityEvent OnLevelComplete;

		public UnityEvent OnLevelFail;

		public LevelDataAsset LevelData;

		[Range(0, 100)]
		public float PercentToComplete = 70f;

		[Range(0.01f, 1)]
		public float IterationsStep = 0.01f;

		private int _FigureIndex = 0;

		private float _LeftTime = 0;

		private int _Scores = 0;

		private eGameState _CurrentState = eGameState.Pause;

		private List<Vector2> _SourceFigure = new List<Vector2>();

		private List<PolygonSegment> _PathSegments = new List<PolygonSegment>();

		private List<Vector2> _DestFigure = new List<Vector2>();

		void Start()
		{
			if (OnChangeFigure == null)
				OnChangeFigure = new UnityEvent();

			if (OnFigureComplete == null)
				OnFigureComplete = new UnityEvent();

			if (OnFigureFail == null)
				OnFigureFail = new UnityEvent();

			if (OnLevelComplete == null)
				OnLevelComplete = new UnityEvent();

			if (OnLevelFail == null)
				OnLevelFail = new UnityEvent();
		}

		public int Scores {
			get
			{
				return _Scores;
			}
		}

		public float LeftTime {
			get
			{
				return _LeftTime;
			}
		}

		public eGameState CurrentState {
			get
			{
				return _CurrentState;
			}
		}

		public List<Vector2> GetSourceFigure()
		{
			return _SourceFigure;
		}

		public void ResetFigure()
		{
			_FigureIndex = 0;
			_Scores = 0;
			_CurrentState = eGameState.Play;
			GenerateFigureData(_FigureIndex);
		}

		private bool NextFigure()
		{
			_FigureIndex++;
			return GenerateFigureData(_FigureIndex);
		}

		private bool GenerateFigureData(int index)
		{
			if (LevelData == null)
				return false;

			if (index < 0 || index >= LevelData.Steps.Count || LevelData.Steps[index].Figure == null)
				return false;

			LevelData.Steps[index].Figure.GenerateSegmentsData(_SourceFigure, _PathSegments);
			_LeftTime = LevelData.Steps[index].Time;

			OnChangeFigure.Invoke();

			return true;
		}

		private void Update()
		{
			if (_CurrentState != eGameState.Play)
				return;

			_LeftTime -= Time.deltaTime;

			if (_LeftTime <= 0)
			{
				_LeftTime = 0;
				_CurrentState = eGameState.Fail;
				OnLevelFail.Invoke();
			}
		}

		public void CheckFigures(List<Vector2> path)
		{
			if (_CurrentState != eGameState.Play)
				return;

			if (path == null || path.Count < 2)
				return;

			_DestFigure.Clear();
			_DestFigure.AddRange(path);

			NormalizeFigure(_DestFigure);

			PolygonSegment outSegment = null;

			List<PolygonSegment> checkedSegments = new List<PolygonSegment>();

			int containsCount = 0;
			int totalCount = 0;
			for(int i = 0; i < _DestFigure.Count - 1; i++)
			{
				Vector2 p0 = _DestFigure[i];
				Vector2 p1 = _DestFigure[i + 1];
				float len = Vector2.Distance(p0, p1);
				Vector2 dir = (p1 - p0).normalized;

				float currLen = 0;
				do
				{
					Vector2 p = p0 + dir * currLen;
					totalCount++;
					if (ContainsPathSegments(p, ref outSegment))
					{
						containsCount++;
						if (!checkedSegments.Contains(outSegment))
							checkedSegments.Add(outSegment);
					}
					currLen += IterationsStep;
				} while (currLen < len);

				totalCount++;
				if (ContainsPathSegments(p1, ref outSegment))
				{
					containsCount++;
					if (!checkedSegments.Contains(outSegment))
						checkedSegments.Add(outSegment);
				}
			}

			float containsPercent = containsCount * 100f / totalCount;

			Debug.Log(containsPercent);

			if (containsPercent >= PercentToComplete && checkedSegments.Count == _PathSegments.Count)
			{
				_Scores++;
				OnFigureComplete.Invoke();

				if (!NextFigure())
				{
					_CurrentState = eGameState.Complete;
					OnLevelComplete.Invoke();
				}
			} else
			{
				OnFigureFail.Invoke();
			}
		}

		private bool ContainsPathSegments(Vector2 point, ref PolygonSegment resultSegment)
		{
			for(int i = 0; i < _PathSegments.Count; i++)
			{
				PolygonSegment segment = _PathSegments[i];

				if (segment.Contains(point))
				{
					resultSegment = segment;
					return true;
				}
			}

			return false;
		}

		private void NormalizeFigure(List<Vector2> destFigure)
		{
			Vector2 destCenter = MathUtils.GetCenter(destFigure);

			for(int i = 0; i < destFigure.Count; i++)
			{
				destFigure[i] -= destCenter;
			}

			Vector2 srcBound = MathUtils.GetBound(_SourceFigure);
			Vector2 destBound = MathUtils.GetBound(destFigure);

			Vector2 scale = new Vector2(srcBound.x / destBound.x, srcBound.y / destBound.y);

			for(int i = 0; i < destFigure.Count; i++)
			{
				destFigure[i] = new Vector2(destFigure[i].x * scale.x, destFigure[i].y * scale.y);
			}
		}

#if UNITY_EDITOR

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;

			for(int i = 0; i < _SourceFigure.Count - 1; i++)
			{
				Gizmos.DrawLine(_SourceFigure[i], _SourceFigure[i + 1]);
			}

			PolygonSegment outSegment = null;
			for(int i = 0; i < _DestFigure.Count - 1; i++)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawLine(_DestFigure[i], _DestFigure[i + 1]);
				Gizmos.color = Color.white;
				Gizmos.DrawSphere(_DestFigure[i], 0.05f);

				if (ContainsPathSegments(_DestFigure[i], ref outSegment))
				{
					Gizmos.color = Color.green;
					Gizmos.DrawSphere(_DestFigure[i], 0.05f);
				}
			}

			Gizmos.color = Color.yellow;

			for(int i = 0; i < _PathSegments.Count; i++)
			{
				PolygonSegment segment = _PathSegments[i];

				Gizmos.DrawLine(segment.LeftTop, segment.RightTop);
				Gizmos.DrawLine(segment.LeftButtom, segment.RightButtom);
			}
		}

#endif
	}
}
