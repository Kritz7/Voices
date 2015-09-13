using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI : MonoBehaviour {

	public float WalkDuration = 2f;

	public Vector3 walkOffset;
	private Vector3 startPos;
	private float direction = 1;

	public float Starsign;
	public float starsignDiff = 0.4f;

	public float happiness = 0;

	public bool IsSpeaking = false;
	bool delayingSpeaking = false;
	List<AI> nearbyAI = new List<AI>();

	public GameObject speechPrefab;
	GameObject currentSpeech;

	public GameObject HappyPrefab;
	public GameObject AngryPrefab;
	GameObject currentOpinion;

	public Thoughts thoughts;

	Color ideaColor;

	bool followingPlayer = false;

	// Use this for initialization
	void Start () 
	{
		Init();
		FindNearbyAI();

		StartCoroutine("Walk");
		StartCoroutine("Speech");
	}

	IEnumerator Speech()
	{
		yield return new WaitForSeconds(Random.Range(0,4f));

		while(true)
		{
			bool anyoneSpeaking = false;
			foreach(AI ai in nearbyAI)
			{
				if(ai.IsSpeaking)
				{
					anyoneSpeaking = true;
				}
			}

			if(!anyoneSpeaking && !IsSpeaking && !delayingSpeaking)
			{
				StartCoroutine("Talk");
			}

			yield return new WaitForSeconds(Random.Range(3,6f));
		}
	}

	void SpawnSpeech()
	{
		currentSpeech = GameObject.Instantiate(speechPrefab, transform.position + transform.up * 1f, Quaternion.identity) as GameObject;
		Speech s = currentSpeech.GetComponent<Speech>();
		
		s.transform.FindChild("Bubble").GetComponent<SpriteRenderer>().color = GetComponent<Human>().Body.color;


		ThoughtManager.Idea ideaToUse = thoughts.Ideas[Random.Range(0, thoughts.Ideas.Count)];

		if (IsThisABadIdea (ideaToUse))
			s.dislike = true;

		s.idea = ideaToUse;

		Sprite spriteToUse = ideaToUse.sprite;
		Color colorToUse = Color.Lerp(ideaToUse.color, ideaColor, Random.Range(0.1f, 0.6f));
		colorToUse = Color.Lerp(colorToUse, Color.white, 0.4f);
		
		s.InitChildSprites(spriteToUse, colorToUse);
	}

	IEnumerator Talk()
	{
		IsSpeaking = true;
		delayingSpeaking = true;

		if(currentSpeech)
		{
			Destroy(currentSpeech);
			currentSpeech = null;
		}

		SpawnSpeech();
		PropogateIdeas();

		yield return new WaitForSeconds(Random.Range(2f, 8f));

		if(currentSpeech)
		{
			Destroy(currentSpeech);
			currentSpeech = null;
		}	

		IsSpeaking = false;

		yield return new WaitForSeconds(2f);

		delayingSpeaking = false;
	}

	IEnumerator Walk()
	{
		while(true)
		{
			gameObject.AddComponent<CoolAnimation>().InitMoveAnimation(startPos + (walkOffset * direction), CoolAnimation.LerpStyle.SuperSmooth, 0, WalkDuration);

			direction = -direction;

			yield return new WaitForSeconds(WalkDuration);
		}
	}

	public void ThinkAbout(ThoughtManager.Idea idea)
	{
		if (!thoughts.Ideas.Contains (idea)) {
			thoughts.AddThought(idea);
		}

		if (IsThisAGoodIdea(idea)) {

			float happinessAdd = (1 - Mathf.Abs(Starsign - idea.starsign)) * 0.5f;

			thoughts.GoodIdeas.Add(idea);
			UpdateOpinion(happinessAdd);
		}

		if (IsThisABadIdea(idea)) {

			float angryAdd = Mathf.Abs(Starsign - idea.starsign) * 0.5f;

			thoughts.BadIdeas.Add(idea);
			UpdateOpinion(-angryAdd);
		}
	}

	void UpdateOpinion(float add)
	{
		Debug.Log ("prev: " + happiness + ", " + add);
		happiness = Mathf.Clamp(happiness+add, -1, 1);
		Debug.Log ("Curr: " + happiness);

		if (Mathf.Abs (happiness) > 0.25f) {
			if (happiness < 0)
				SpawnOpinion (AngryPrefab);
			else
				SpawnOpinion (HappyPrefab);
		} else if(currentOpinion!=null)
		{
			currentOpinion.transform.localScale = new Vector3(happiness, happiness, happiness);
		}

		foreach (CoolAnimation cool in GetComponentsInChildren<CoolAnimation>())
		{
			cool.MoveSpeed *= Mathf.Abs(happiness);
		}

		
		if (happiness > 0.6f && !followingPlayer)
		{
			followingPlayer = true;
			StartCoroutine("FollowPlayer");	
		}
	}

	IEnumerator FollowPlayer()
	{
		while (happiness > 0.4f) {

			float randDist = Random.Range(0.2f, 0.8f);
		//	if(Random.value>0.5f) randDist = -randDist;

			startPos = GameObject.Find("Player").transform.position;

			yield return new WaitForSeconds(Random.Range(0.6f, 2f));
		}

		followingPlayer = false;
	}

	bool IsThisAGoodIdea(ThoughtManager.Idea idea)
	{
		return thoughts.GoodIdeas.Contains (idea);
	}

	bool IsThisABadIdea(ThoughtManager.Idea idea)
	{
		return thoughts.BadIdeas.Contains (idea);
	}

	void SpawnOpinion(GameObject prefab)
	{
		if (currentOpinion != null) {
			GameObject.Destroy(currentOpinion);
			currentOpinion = null;
		}

		GameObject newOpinionObj = GameObject.Instantiate (prefab, transform.position, Quaternion.identity) as GameObject;
		newOpinionObj.transform.localScale = new Vector3 (happiness, happiness, happiness);

		FollowObject newOpinionFollow = newOpinionObj.GetComponent<FollowObject> ();
		newOpinionFollow.follow = this.gameObject;

		currentOpinion = newOpinionObj;
	}

	void PropogateIdeas()
	{
		FindNearbyAI();

		foreach(AI ai in nearbyAI)
		{
			foreach(ThoughtManager.Idea idea in thoughts.Ideas)
			{
				if(!ai.thoughts.Ideas.Contains(idea))
				{
					ai.thoughts.Ideas.Add(idea);

					if(Mathf.Abs(idea.starsign - Starsign)<starsignDiff)
					{
						ai.thoughts.GoodIdeas.Add(idea);
					}
					else// if((1-Mathf.Abs(idea.starsign - Starsign))<starsignDiff)
					{
						ai.thoughts.BadIdeas.Add(idea);
					}
				}
			}
		}
	}

	void FindNearbyAI()
	{
		Collider[] cs = Physics.OverlapSphere(transform.position, 2f);
		
		foreach(Collider c in cs)
		{
			if(c.GetComponent<AI>() && c.GetComponent<AI>()!=this)
			{
				nearbyAI.Add(c.GetComponent<AI>());
			}
		}
	}

	void Init()
	{
		ideaColor = RandColour();
		thoughts = GetComponent<Thoughts>();
		Starsign = Random.value;

		startPos = transform.position;
		
		if(Random.value>0.5f) walkOffset.x = -walkOffset.x;
		if(Random.value>0.5f) walkOffset.y = -walkOffset.y;
		if(Random.value>0.5f) walkOffset.z = -walkOffset.z;
		
		if(Random.value>0.5f) direction = -direction;
		
		
		float frequency = Random.Range(0.04f, 0.08f);
		if(Random.value>0.5f) frequency = -frequency;
		foreach(Transform t in transform)
		{
			t.GetComponent<CoolAnimation>().Frequency = frequency;
		}
	}

	Color RandColour()
	{
			Color[] colors = { Color.red, Color.yellow, Color.green, Color.blue, Color.magenta, Color.cyan, Color.white };
			
			return Color.Lerp(colors[Random.Range(0, colors.Length)], colors[Random.Range(0, colors.Length)], Random.Range(0.2f, 0.8f));
	}
}
