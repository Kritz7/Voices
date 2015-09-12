using UnityEngine;
using System.Collections;

public class FollowObject : MonoBehaviour {

	public GameObject follow;
	Vector3 offset;
	public Vector3 normalOffset;
	public Vector3 zoomOffset;

	public float moveSpeed = 2f;
	public float rotateSpeed = 0.5f;

	// Use this for initialization
	void Start ()
	{
		offset = normalOffset;
	}

	public void ZoomIn()
	{
		offset = zoomOffset;
	}

	public void ZoomOut()
	{
		offset = normalOffset;
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			ZoomIn();
		}

		Vector3 followPos = follow.transform.position - offset;
		Vector3 posDiff = follow.transform.position - transform.position;
		Quaternion rotation  = Quaternion.LookRotation(posDiff);


		transform.position = Vector3.Lerp(transform.position, followPos, moveSpeed * Time.deltaTime);
		transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
	}
}
