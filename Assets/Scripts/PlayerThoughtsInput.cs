using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerThoughtsInput : MonoBehaviour {

	public GameObject SpeechPrefab;
	public GameObject currentSpeech;

	// Use this for initialization
	void Start ()
	{
		SetIdeaButtons();

	}

	void SetIdeaButtons()
	{
		int itemCount = Player.player.PlayerIdeas.Count;
		Vector2 itemSize = new Vector2(100,100);
		for(int i=0; i<itemCount; i++)
		{
			Vector3 basePosition = GetComponent<RectTransform>().position;
			Vector3 offset = new Vector3((i * itemSize.x * 2) - (itemCount * itemSize.x * 0.5f), -35, 0);

			InitButton(basePosition + offset, new Vector2(100,100), Player.player.PlayerIdeas[i], Player.player.PlayerIdeas[i].sprite, Player.player.PlayerIdeas[i].color);
		}
	}

	void InitButton(Vector3 position, Vector2 size, ThoughtManager.Idea idea, Sprite sprite, Color color)
	{
		GameObject newButton = new GameObject(sprite.name,
		                                      typeof(RectTransform),
		                                      typeof(CanvasRenderer),
		                                      typeof(Image),
		                                      typeof(Button),
		                                      typeof(CoolAnimation));
		newButton.transform.position = position;
		newButton.GetComponent<RectTransform>().sizeDelta = size;
		newButton.GetComponent<Image>().sprite = sprite;
		newButton.GetComponent<Image>().color = color;

		float randFreq = Random.Range(1,2);
		if(Random.value>0.5f) randFreq = -randFreq;
		newButton.GetComponent<CoolAnimation>().Frequency = randFreq;
		newButton.GetComponent<CoolAnimation>().RotationAxis = new Vector3(0,0,1);
		newButton.GetComponent<CoolAnimation>().RotationSpeed = 20f;

		newButton.transform.SetParent(this.transform, true);

		newButton.GetComponent<Button>().onClick.AddListener(() => Say(idea));
	}

	public void SpawnSpeech(ThoughtManager.Idea i)
	{
		if(currentSpeech)
		{
			Destroy(currentSpeech.gameObject);
			currentSpeech = null;
		}

		currentSpeech = GameObject.Instantiate(SpeechPrefab, Player.player.transform.position + transform.up * 1f, Quaternion.identity) as GameObject;
		Speech s = currentSpeech.GetComponent<Speech>();
		currentSpeech.AddComponent<PlayerSpeech> ().idea = i;
		currentSpeech.name = "Player's " + currentSpeech.name;

		FollowObject follow = currentSpeech.AddComponent<FollowObject>();

		follow.follow = Player.player.gameObject;
		follow.normalOffset = new Vector3(0, 1.5f, 0);
		follow.rotateSpeed = 0;
		follow.moveSpeed = 5;

		s.transform.FindChild("Bubble").GetComponent<SpriteRenderer>().color = Player.player.GetComponent<Human>().Body.color;
		
		ThoughtManager.Idea ideaToUse = i;
		Sprite spriteToUse = ideaToUse.sprite;
		Color colorToUse = ideaToUse.color;
		colorToUse = Color.Lerp(colorToUse, Color.white, 0.4f);
		
		s.InitChildSprites(spriteToUse, colorToUse);
	}


	public void Say(ThoughtManager.Idea i)
	{
		SpawnSpeech(i);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
