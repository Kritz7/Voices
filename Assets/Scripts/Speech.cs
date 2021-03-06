﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Speech : MonoBehaviour {

	List<GameObject> children = new List<GameObject>();
	public bool dislike = false;
	public ThoughtManager.Idea idea;

	// Update is called once per frame
	void Update ()
	{
		if(Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			RaycastHit hit;
			if(Physics.Raycast(ray, out hit, 20f))
			{
				if(hit.transform.root.tag == "Topic")
				{
					GameObject.Find("PlayerThoughtInput").GetComponent<PlayerThoughtsInput>().SpawnSpeech(idea);
				}
			}
		}
	}

	public void InitChildSprites(List<Sprite> sprites, Color color)
	{
		ClearAllChildren();

		foreach (Sprite s in sprites) {
			GameObject g = new GameObject ("Sprite-" + s.name,
			                              typeof(SpriteRenderer),
			                              typeof(CoolAnimation));
			g.transform.position = transform.position - transform.forward * 0.05f;
			g.transform.SetParent (this.transform, true);

			g.GetComponent<SpriteRenderer> ().sprite = s;
			g.GetComponent<SpriteRenderer> ().color = color;

			g.GetComponent<CoolAnimation> ().Frequency = 0;
			g.GetComponent<CoolAnimation> ().RotationAxis = new Vector3 (0, 0, 1);
			g.GetComponent<CoolAnimation> ().RotationSpeed = 20f;

			children.Add (g);
		}

		SetDislike ();
	}

	public void InitChildSprites(Sprite sprite, Color color)
	{
		ClearAllChildren();

		GameObject g = new GameObject("Sprite-"+sprite.name,
		                              typeof(SpriteRenderer),
		                              typeof(CoolAnimation));
		g.transform.position = transform.position - transform.forward * 0.05f;
		g.transform.SetParent(this.transform, true);
		
		g.GetComponent<SpriteRenderer>().sprite = sprite;
		g.GetComponent<SpriteRenderer>().color = color;

		g.GetComponent<CoolAnimation>().Frequency = 0;
		g.GetComponent<CoolAnimation>().RotationAxis = new Vector3(0,0,1);
		g.GetComponent<CoolAnimation>().RotationSpeed = 20f;
		
		children.Add(g);

		SetDislike ();
	}

	public void InitChildSprites(GameObject clone)
	{
		ClearAllChildren();

		foreach(SpriteRenderer s in clone.GetComponentsInChildren<SpriteRenderer>())
		{
			if(!s.name.Contains("Bubble"))
			{
				GameObject g = new GameObject("Sprite-"+clone.name,
				                              typeof(SpriteRenderer),
				                              typeof(CoolAnimation));
				g.transform.position = transform.position - transform.forward * 0.05f;
				g.transform.SetParent(this.transform, true);

				g.GetComponent<SpriteRenderer>().sprite = s.sprite;
				g.GetComponent<SpriteRenderer>().color = s.color;

				g.GetComponent<CoolAnimation>().Frequency = 0;
				g.GetComponent<CoolAnimation>().RotationAxis = new Vector3(0,0,1);
				g.GetComponent<CoolAnimation>().RotationSpeed = 20f;

				children.Add(g);
			}
		}

		SetDislike ();
	}

	void SetDislike()
	{
		Transform dis = transform.root.FindChild ("Dislike");

		SpriteRenderer sr = dis.GetComponent<SpriteRenderer>();

		Color on = new Color (1, 0, 0, 1);
		Color off = new Color (0, 0, 0, 0);

		if (dislike)
			sr.color = on;
		else
			sr.color = off;
	}

	void ClearAllChildren()
	{
		if(children.Count>0)
		{
			int childrenCount = children.Count;

			for(int i=0; i<childrenCount; i++)
			{
				Destroy(children[0]);
				children.RemoveAt(0);
			}
		}
	}
}
