using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSpeech : MonoBehaviour
{
	public ThoughtManager.Idea idea;
	List<AI> npcsAffected = new List<AI>();

	float radius = 5f;
	float speakTime = 5f;

	// Use this for initialization
	void Start ()
	{
		StartCoroutine ("DestroyMe");
		StartCoroutine ("AffectNPCs");
	}

	IEnumerator AffectNPCs()
	{
		while (true)
		{
			Collider[] collidersHit = Physics.OverlapSphere(transform.position, radius);

			foreach(Collider c in collidersHit)
			{
				if(c.GetComponent<AI>())
				{


					AI ai = c.GetComponent<AI>();

					if(!npcsAffected.Contains(ai))
					{
						ai.ThinkAbout(idea);
						npcsAffected.Add(ai);
					}
				}
			}

			yield return new WaitForSeconds(Random.Range(0.5f, 1f));
		}
	}

	IEnumerator DestroyMe()
	{
		yield return new WaitForSeconds (speakTime);
		Destroy (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
