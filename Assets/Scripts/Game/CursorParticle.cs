using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Game
{
	public class CursorParticle: MonoBehaviour 
	{
		void OnEnable()
		{
			StartCoroutine(StartFly());
		}

		void OnDisable()
		{
			StopAllCoroutines();
		}

		IEnumerator StartFly()
		{
			transform.localScale = Vector3.zero;
			yield return transform.DOScale(2.5f, 0.3f).WaitForCompletion();
			yield return transform.DOScale(0f, 0.7f).WaitForCompletion();

			gameObject.Recycle();
		}
	}
}
