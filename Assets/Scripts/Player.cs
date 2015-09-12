using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
	public static Player player;

	float frameMovementStarted = 0;
	List<Vector3> childPos = new List<Vector3>();

	public List<ThoughtManager.Idea> PlayerIdeas = new List<ThoughtManager.Idea>();

	float speed = 2f;

	void Awake()
	{
		player = this;
	}

	// Update is called once per frame
	void Update ()
	{
		// take input

		Vector3 moveDir = Vector3.zero;

		moveDir.x = Input.GetAxis("Horizontal");
		moveDir.z = Input.GetAxis("Vertical");

		if(moveDir.magnitude!=0)
		{
			if(frameMovementStarted == 0)
			{
				frameMovementStarted = Time.frameCount;
				InitAnimators();
			}			
		}
		else
		{
			DestroyAnimators();
			frameMovementStarted = 0;

			foreach(Transform t in transform)
			{
				if(childPos.Count>0)
				{
					Vector3 pos = childPos[0];
					Vector3 newPos = new Vector3(t.transform.position.x, pos.y, t.transform.position.z);
					
					t.position = Vector3.Lerp(t.transform.position, newPos, 10f * Time.deltaTime);
				}
			}
		}

		transform.position = transform.position + moveDir * speed * Time.deltaTime;

	}

	void InitAnimators()
	{
		if(frameMovementStarted == Time.frameCount) DestroyAnimators();

		foreach(Transform t in transform)
		{
			t.gameObject.AddComponent<CoolAnimation>().InitBounceAnimation(10f, 0f, Vector3.zero, 0.04f, 0);
			childPos.Add(t.transform.position);
		}
	}

	void DestroyAnimators()
	{
		foreach(Transform t in transform)
		{
			if(t.GetComponent<CoolAnimation>())
			{
				Destroy(t.GetComponent<CoolAnimation>());
			}
		}

	//	transform.position = new Vector3(transform.position.x, startPos.y, transform.position.z);
	}
}
