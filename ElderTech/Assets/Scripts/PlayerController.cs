using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed;
	public float start_x;		//the starting x position value, lerp
	public float end_x;			//the ending x position value, lerp

	public float fadeOut_start_x;	//The starting fade out x position value, lerp out
	public float fadeOut_end_x;		//The ending fade out x position value, lerp out
	public Color start_color;			//The starting lerp color
	public Color end_color;			//the end lerp color
	
	public bool fadeInFadeOut;	//True if the sprite renderer material color fades in then fades out, otherwise only fades in
	public bool fadeIn = true;
	Animator animator;
	
	public float trigger_x;		//Once the player passes this x pos, the player falls into the grave
	public GameObject IPhone;
	private Transform IPhoneTransform;
	public float IPhone_start;
	public float IPhone_start_y;
	public bool once = true;
	public bool canMove = true;
	public bool pingPong = false;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		IPhoneTransform = IPhone.GetComponent<Transform>();
	}

	void Update(){
		if(fadeIn){
			float relpos_x = (transform.position.x - start_x) / (end_x - start_x);
			renderer.material.color = Color.Lerp(start_color, end_color, relpos_x);

			if(transform.position.x > fadeOut_start_x && fadeInFadeOut == true){
				fadeIn = false;
			}
		} else {
			float relpos_x = (transform.position.x - fadeOut_start_x) / (fadeOut_end_x - fadeOut_start_x);
			renderer.material.color = Color.Lerp(end_color, start_color, relpos_x);
		}
	}

	void FixedUpdate(){

		//If the player's speed is > 0, play walking animation
		if(Input.GetAxis("Horizontal") > 0 && canMove){
			transform.Translate(new Vector3(Input.GetAxis("Horizontal") * speed/2 * Time.deltaTime, 0, 0)); 
		}
		animator.SetFloat("Speed", Input.GetAxis("Horizontal") * speed/2);

		//if the player has passed the trigger point, play falling animation
		if(transform.position.x > trigger_x){
			if(once){
				IPhone_start = IPhoneTransform.position.x;
				IPhone_start_y = IPhoneTransform.position.y;
				once = false;
			}
			animator.SetBool("Falling", true);
			canMove = false;

			//Move Iphone
			Debug.Log("move");
			IPhoneTransform.position = new Vector3(Mathf.Lerp(IPhone_start, IPhone_start + 1.5f, Time.time), IPhoneTransform.position.y, IPhoneTransform.position.z);
			if(IPhoneTransform.position.x >= IPhone_start + 1.5f){
				pingPong = true;
			}

			if(pingPong){
				IPhoneTransform.position = new Vector3(IPhoneTransform.position.x, IPhone_start_y + Mathf.PingPong(Time.time, 0.5f), IPhoneTransform.position.z);
			}

		}

	}
}
