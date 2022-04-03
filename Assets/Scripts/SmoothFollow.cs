using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour {

	public Transform target;

	public float smoothness = 1f;
	public float rotationSmoothness = .1f;

	public Transform offset;

	public int offsetVal = 10;

	private Vector3 velocity = Vector3.zero;

	// Update is called once per frame
	void FixedUpdate () {

		if (target == null)
		{
			return;
		}

		//Debug.Log("Target Ps:")
		Vector3 newPos = target.position + target.up * offsetVal;

		Debug.Log("New Pos: " + newPos);
		//transform.position = newPos;
		transform.position = Vector3.SmoothDamp(transform.position, offset.position, ref velocity, smoothness);

		Quaternion targetRot = Quaternion.LookRotation(-transform.position.normalized, target.up);
		//transform.rotation = targetRot;
		transform.rotation = Quaternion.Lerp(transform.rotation, offset.rotation, Time.deltaTime * rotationSmoothness);

	}
}
