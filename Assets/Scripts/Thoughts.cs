using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Thoughts : MonoBehaviour
{
	public static ThoughtManager thoughtMan;
	public List<ThoughtManager.Idea> Ideas = new List<ThoughtManager.Idea>();

	public int numberOfThoughts = 1;

	// Use this for initialization
	void Start ()
	{
		if(thoughtMan==null) thoughtMan = GameObject.Find("ThoughtManager").GetComponent<ThoughtManager>();
		InitIdeas();
	}

	
	void InitIdeas()
	{
		List<ThoughtManager.Idea> ideaclone = new List<ThoughtManager.Idea>(thoughtMan.Ideas);
		
		if(thoughtMan.Ideas.Count>0)
		{
			for(int i=0; i<numberOfThoughts; i++)
			{
				int rand = Random.Range(0, ideaclone.Count);
				Ideas.Add(ideaclone[rand]);
				ideaclone.RemoveAt(rand);
			}
		}
	}

	// Update is called once per frame
	void Update ()
	{
	
	}
}
