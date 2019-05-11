using UnityEngine;
using System.Collections;

namespace FlooMapEditor
{
	public class SpawnerLine : MonoBehaviour 
	{
		public Transform firstPoint;
		public Transform secondPoint;
		public LineRenderer lineRenderer;
		public BoxCollider theCollider;

		private float radius;
		private float distance;

		public void SetDistance(float distance)
		{
			this.distance = distance;
			firstPoint.localPosition = new Vector3(-distance / 2, 0, 0);
			secondPoint.localPosition = new Vector3(distance / 2, 0, 0);

			lineRenderer.SetPosition(0, firstPoint.localPosition);
			lineRenderer.SetPosition(1, secondPoint.localPosition);

			SetCollider();
		}

		public void SetRadius(float radius)
		{
			this.radius = radius;
			lineRenderer.SetWidth(radius * 2, radius * 2);
			firstPoint.localScale = new Vector3(radius * 2, 0, radius * 2);
			secondPoint.localScale = new Vector3(radius * 2, 0, radius * 2);

			SetCollider();
		}

		public void SetCollider()
		{
			theCollider.size = new Vector3(distance + (2 * radius), 2 * radius, 1);
		}

		public void SetAngle(float angle)
		{
			transform.localEulerAngles = new Vector3(0, 0, angle);
		}
	}
}