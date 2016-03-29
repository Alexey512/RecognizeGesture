using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI.Extensions;
using Utils;

namespace Gui
{
	[RequireComponent(typeof(UILineRenderer))]
	[RequireComponent(typeof(RectTransform))]
	public class FigureView: MonoBehaviour
	{
		private UILineRenderer _LineRenderer;

		private RectTransform _Transform;

		void Start()
		{
			_LineRenderer = GetComponent<UILineRenderer>();
			_Transform = GetComponent<RectTransform>();
		}

		public void SetFigure(List<Vector2> path)
		{
			float size = Mathf.Max(_Transform.sizeDelta.x, _Transform.sizeDelta.y);

			Vector2 center = MathUtils.GetCenter(path);
			Vector2 bound = MathUtils.GetBound(path);

			float scale = Mathf.Min(size / bound.x, size / bound.y);

			_LineRenderer.Points = new Vector2[path.Count];
			for(int i = 0; i < path.Count; i++)
			{
				_LineRenderer.Points[i] = new Vector2(path[i].x * scale + size / 2 - center.x, 
					path[i].y * scale + size / 2 - center.y);

				//_LineRenderer.Points[i] = path[i] * scale + size / 2 - center;
			}

			_LineRenderer.SetVerticesDirty();
		}
	}
}

