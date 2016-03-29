using UnityEngine;
using System.Collections.Generic;

namespace Game
{
	public class InputController: MonoBehaviour
	{
		[SerializeField]
		private Camera _Camera;

		[SerializeField]
		private GameManager _Manager;

		[SerializeField]
		public CursorParticle _CursorParticlePref;

		[SerializeField]
		public int _IntialParticlePoolSize = 5;

		private Vector2 _PrevMousePos;

		private bool _IsMouseMoved = false;

		private List<Vector2> _Path = new List<Vector2>();

		void Start()
		{
			if (_Camera == null)
			{
				_Camera = GetComponent<Camera>();
				if (_Camera == null)
					_Camera = Camera.main;
			}

			if (_CursorParticlePref != null)
			{
				ObjectPool.CreatePool<CursorParticle>(_CursorParticlePref, _IntialParticlePoolSize);
			}
		}

		void AddCursorParticle(Vector3 pos)
		{
			if (_CursorParticlePref == null)
				return;
			_CursorParticlePref.Spawn(null, pos);
		}

		void Update()
		{
			Vector2 mousePos = Input.mousePosition;
			Vector2 worldPos = _Camera.ScreenToWorldPoint(mousePos);

			if (Input.GetMouseButtonDown(0))
			{
				_PrevMousePos = mousePos;
				_IsMouseMoved = false;

				_Path.Clear();
				_Path.Add(_PrevMousePos);
				AddCursorParticle(worldPos);
			}

			if (Input.GetMouseButton(0))
			{
				if (_PrevMousePos != mousePos)
				{
					Vector2 delta = mousePos - _PrevMousePos;
					_PrevMousePos = mousePos;

					_Path.Add(mousePos);
					AddCursorParticle(worldPos);

					_IsMouseMoved = true;
				}
			}

			if (Input.GetMouseButtonUp(0))
			{
				if (_IsMouseMoved)
				{
					_Path.Add(mousePos);
					AddCursorParticle(worldPos);
					_IsMouseMoved = false;

					_Manager.CheckFigures(_Path);
					_Path.Clear();
				}
			}
		}
	}
}

