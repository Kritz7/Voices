using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Human : MonoBehaviour
{
	public SpriteRenderer Face;
	public SpriteRenderer Body;

	public List<Color> SkinTones = new List<Color>();
	public List<Color> DressTones = new List<Color>();

	Color RandColour()
	{
		Color[] colors = { Color.red, Color.yellow, Color.green, Color.blue, Color.magenta, Color.cyan, Color.white };

		return Color.Lerp(colors[Random.Range(0, colors.Length)], colors[Random.Range(0, colors.Length)], Random.Range(0.2f, 0.8f));
	}

	Color RandColour(List<Color> colours)
	{
		return Color.Lerp(colours[Random.Range(0, colours.Count)], colours[Random.Range(0, colours.Count)], Random.Range(0.1f, 0.9f));
	}

	Color RandColour(List<Color> colours, float lerp)
	{
		return Color.Lerp(colours[Random.Range(0, colours.Count)], colours[Random.Range(0, colours.Count)], lerp);
	}

	// Use this for initialization
	void Start ()
	{
		SetColours();
		RandBounce();
	}

	void RandBounce()
	{
		float randSpeed = Random.Range(5,20);
		foreach(CoolAnimation ca in GetComponentsInChildren<CoolAnimation>())
		{
			ca.MoveSpeed = randSpeed;
		}
	}

	void SetColours()
	{
		Face.color = RandColour(SkinTones);
		Body.color = RandColour(DressTones);

		if(GetComponent<Player>())
		{
			Body.color = RandColour(DressTones, 1);
		}
	}
}
