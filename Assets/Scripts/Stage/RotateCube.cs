using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour
{

	[SerializeField] private Vector3 rotate;
	[SerializeField] private float rotSpeed;

	private void Update()
	{
		transform.Rotate(rotate * rotSpeed * Time.deltaTime);
	}
}