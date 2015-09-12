using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ThoughtManager : MonoBehaviour
{
	public static ThoughtManager thoughtMan;

	public Player Player;

	public Text text;

	[System.Serializable]
	public class Idea
	{
		public int id;
		public Sprite sprite;
		public Color color;
	}
	public List<Idea> Ideas = new List<Idea>();



	// Use this for initialization
	void Awake ()
	{
		thoughtMan = this;
		InitIdeas();
	}

	void InitIdeas()
	{
		for(int i=0; i<Ideas.Count; i++)
		{
			Ideas[i].color = RandColour();
			Ideas[i].id = i;
		}

		List<Idea> ideaclone = new List<Idea>(Ideas);
		
		if(Ideas.Count>0)
		{
			for(int i=0; i<2; i++)
			{
				int rand = Random.Range(0, ideaclone.Count);
				Player.PlayerIdeas.Add(ideaclone[rand]);
				ideaclone.RemoveAt(rand);
			}
		}
	}
	
	Color RandColour()
	{
		Color[] colors = { Color.red, Color.yellow, Color.green, Color.blue, Color.magenta, Color.cyan, Color.white };
		
		return Color.Lerp(colors[Random.Range(0, colors.Length)], colors[Random.Range(0, colors.Length)], Random.Range(0.2f, 0.8f));
	}

	
	// Update is called once per frame
	void Update ()
	{
		text.text = "";

		foreach(Idea i in Player.player.PlayerIdeas)
		{
			text.text += i.sprite.name + "\n";
		}
	}
}
