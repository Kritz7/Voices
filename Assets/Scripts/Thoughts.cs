using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Thoughts : MonoBehaviour
{
	public static ThoughtManager thoughtMan;
	AI myAI;
	public List<ThoughtManager.Idea> Ideas = new List<ThoughtManager.Idea>();

	public List<ThoughtManager.Idea> GoodIdeas = new List<ThoughtManager.Idea> ();
	public List <ThoughtManager.Idea> BadIdeas = new List<ThoughtManager.Idea> ();

	public int numberOfThoughts = 1;

	// Use this for initialization
	void Start ()
	{
		if(thoughtMan==null) thoughtMan = GameObject.Find("ThoughtManager").GetComponent<ThoughtManager>();
		myAI = GetComponent<AI> ();

		InitIdeas();
	}

	public void AddThought(ThoughtManager.Idea idea)
	{
		Ideas.Add(idea);

		if(Mathf.Abs(idea.starsign - myAI.Starsign)<myAI.starsignDiff)
		{
			GoodIdeas.Add(idea);
		}
		else// if((1-Mathf.Abs(idea.starsign - Starsign))<starsignDiff)
		{
			BadIdeas.Add(idea);
		}
	}
	
	void InitIdeas()
	{
		List<ThoughtManager.Idea> ideaclone = new List<ThoughtManager.Idea>(thoughtMan.Ideas);
		
		if(thoughtMan.Ideas.Count>0)
		{
			for(int i=0; i<numberOfThoughts; i++)
			{
				int rand = Random.Range(0, ideaclone.Count);
				ThoughtManager.Idea idea = ideaclone[rand];
				Ideas.Add(ideaclone[rand]);
				ideaclone.RemoveAt(rand);

				if(Mathf.Abs(idea.starsign - myAI.Starsign)<myAI.starsignDiff)
				{
					GoodIdeas.Add(idea);
				}
				else// if((1-Mathf.Abs(idea.starsign - Starsign))<starsignDiff)
				{
					BadIdeas.Add(idea);
				}
			}
		}
	}
}
