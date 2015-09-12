using UnityEngine;
using System.Collections;

public class CoolAnimation : MonoBehaviour {
	
	public enum AnimationType
	{
		Bobbing,
		MoveToPosition
	}
	public AnimationType animationType;
	
	public enum LerpStyle
	{
		EaseIn,
		EaseOut,
		Smooth,
		SuperSmooth,
		Linear
	}
	public LerpStyle lerpStyle;
	
	private float t = 0;
	public float Duration = 0;
	
	private Vector3 startPos;
	public Vector3 EndPos;
	
	public float MoveSpeed = 10f;
	public float RandomMoveSpeed = 0f;
	public float Frequency = 1f;
	public float RandomFrequency = 0f;
	public float YOffset = 0f;
	
	public Vector3 RotationAxis = Vector3.zero;
	public float RotationSpeed = 0f;
	
	public bool Billboard = false;
	
	public bool AnimateRegardlessOfCameraDistance = true;
	private float minDistFromCamera = 170f;
	private float minBillboardDistFromCamera = 250f;
	
	public void InitBounceAnimation(float moveSpeed, float rotationSpeed, Vector3 rotationAxis, float frequency, float yOffset, bool animateRegardlessOfCameraDistance=true)
	{
		startPos = transform.localPosition;
		
		this.animationType = AnimationType.Bobbing;
		this.lerpStyle = LerpStyle.Linear;
		this.MoveSpeed = moveSpeed;
		this.RotationSpeed = rotationSpeed;
		this.RotationAxis = rotationAxis;
		this.Frequency = frequency;
		this.YOffset = yOffset;
		this.AnimateRegardlessOfCameraDistance = animateRegardlessOfCameraDistance;
		
		ResetT();
	}
	
	public void InitMoveAnimation(Vector3 endPos, LerpStyle lerpStyle, float yOffset, float duration, bool animateRegardlessOfCameraDistance=true)
	{
		startPos = transform.localPosition;
		
		this.EndPos = endPos;
		this.YOffset = yOffset;
		this.animationType = AnimationType.MoveToPosition;
		this.lerpStyle = lerpStyle;
		this.Duration = duration;
		this.AnimateRegardlessOfCameraDistance = animateRegardlessOfCameraDistance;
		
		ResetT();
	}
	
	public void InitMoveAnimation(Vector3 startPos, Vector3 endPos, LerpStyle lerpStyle, float yOffset, float duration, bool animateRegardlessOfCameraDistance=true)
	{
		this.startPos = startPos;
		this.EndPos = endPos;
		this.YOffset = yOffset;
		this.animationType = AnimationType.MoveToPosition;
		this.lerpStyle = lerpStyle;
		this.Duration = duration;
		this.AnimateRegardlessOfCameraDistance = animateRegardlessOfCameraDistance;
		
		ResetT();
	}
	
	void OnEnable()
	{
		if(animationType == AnimationType.Bobbing)
		{
			InitBounceAnimation(MoveSpeed, RotationSpeed, RotationAxis, Frequency, YOffset, AnimateRegardlessOfCameraDistance);
		}
		
		if(animationType == AnimationType.MoveToPosition)
		{
			InitMoveAnimation(EndPos, lerpStyle, YOffset, Duration, AnimateRegardlessOfCameraDistance);
		}
	}
	
	void ResetT()
	{
		t = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(WithinDistance(minDistFromCamera) || AnimateRegardlessOfCameraDistance)
		{
			// BOBBING ANIMATION
			if(animationType == AnimationType.Bobbing)
			{
				t += (MoveSpeed + Random.Range(-RandomMoveSpeed, RandomMoveSpeed)) * Time.deltaTime;
				float tMulti = Mathf.Sin(t);
				float newYPos = (Frequency + Random.Range(-RandomFrequency, RandomFrequency)) * tMulti;
				
				transform.localPosition = new Vector3(transform.localPosition.x, startPos.y + YOffset + newYPos, transform.localPosition.z);
				transform.Rotate(RotationAxis, RotationSpeed * Time.deltaTime);
			}
			
			// MOVE TO LOCATION AND THEN DESTROY... ANIMATION
			if(animationType == AnimationType.MoveToPosition)
			{
				if(Duration == 0)
				{
					Duration = 0.01f;
					Debug.LogError("Duration cannot be 0!");
				}
				
				t += Time.deltaTime;
				float completion = t / Duration;
				
				switch(lerpStyle)
				{
				case LerpStyle.EaseIn:
					completion = Mathf.Sin(completion * Mathf.PI * 0.5f);
					break;
				case LerpStyle.EaseOut:
					completion = 1f - Mathf.Cos(completion * Mathf.PI * 0.5f);
					break;
				case LerpStyle.Smooth:
					completion = completion * completion * (3f - 2f * completion);
					break;
				case LerpStyle.SuperSmooth:
					completion = completion * completion * completion * (completion * (6f * completion - 15f) + 10f);
					break;
				default:
					completion = completion;
					break;
				}
				
				transform.localPosition = Vector3.Lerp(startPos, EndPos, completion);
				
				if(t >= Duration)
				{
					transform.localPosition = EndPos;
					
					Destroy(this);
				}
			}
		}
	}
	
	void LateUpdate()
	{
		if(WithinDistance(minBillboardDistFromCamera) || AnimateRegardlessOfCameraDistance)
		{
			if(Billboard)
				transform.LookAt(Camera.main.transform);
		}
	}
	
	
	public bool WithinDistance(float dist)
	{
		if(Vector3.Distance(Camera.main.transform.position, transform.position)<dist)
			return true;
		
		return false;
	}
}