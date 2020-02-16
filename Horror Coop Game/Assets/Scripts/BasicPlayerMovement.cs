using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BasicPlayerMovement : MonoBehaviour
{
	public GameObject targetName;
	public bool isActive;

	public bool grounded;
	public float speed;
	Rigidbody playerRigidbody;
	public Camera cam;

	[Range(1,10)]
	public float jumpVelocity;
	public float fallMultiplier = 2.5f;
	public float lowJumpMultiplier = 2f;
	public float distToGround;

	Vector3 moveDirection;
	public bool isSprinting;
	public bool isWalking;

	public Animator anim;

	public bool isHiding;
	public bool moveTowardsHiding;
	public bool crouchWhileHiding;
	public bool crawlIntoHiding;
	public bool rotateWhileHiding;
	public bool doneHiding;

	public bool crawlOutOfHiding;
	public bool standOutOfHiding;

	void Start () {
		isActive = false;
		doneHiding = false;
		isHiding = false;
		moveTowardsHiding = false;
		crouchWhileHiding = false;
		crawlIntoHiding = false;
		rotateWhileHiding = false;
		crawlOutOfHiding = false;
		standOutOfHiding = false;
		isSprinting = false;
		playerRigidbody = GetComponent <Rigidbody> ();
		grounded = true;
		transform.rotation = (Quaternion.identity);
		anim = GetComponentInChildren<Animator> ();
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update() {
		if (Input.GetKey(KeyCode.Escape)) {
			Cursor.lockState = CursorLockMode.None;
		}
		if (Input.GetMouseButtonDown(0) && doneHiding) {
			doneHiding = false;
			StartCoroutine(GetOutOfHiding());
		}
		if (moveTowardsHiding) {
			transform.position = Vector3.Lerp(transform.position, new Vector3(-5.5f, 0, 3), 6 * Time.deltaTime);
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 35, 0), 4 * Time.deltaTime);
			cam.transform.localRotation = Quaternion.Slerp(cam.transform.localRotation, Quaternion.Euler(35, 0, 0), 4 * Time.deltaTime);
		}
		if (crouchWhileHiding) {
			transform.position = Vector3.Lerp(transform.position, new Vector3(-5.5f, -1, 3), 6 * Time.deltaTime);
			cam.transform.localRotation = Quaternion.Slerp(cam.transform.localRotation, Quaternion.Euler(0, 0, 0), 4 * Time.deltaTime);
		}
		if (crawlIntoHiding) {
			transform.position = Vector3.Lerp(transform.position, new Vector3(-4.75f, -1, 4.3f), 6 * Time.deltaTime);
		}
		if (rotateWhileHiding) {
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 190, 0), 4 * Time.deltaTime);
		}
		if (crawlOutOfHiding) {
			transform.position = Vector3.Lerp(transform.position, new Vector3(-5.5f, -1, 3), 6 * Time.deltaTime);
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 215, 0), 4 * Time.deltaTime);
		}
		if (standOutOfHiding) {
			transform.position = Vector3.Lerp(transform.position, new Vector3(-5.5f, 0, 3), 6 * Time.deltaTime);
		}
	}

	void FixedUpdate () {
		if (!isActive) {
			Ray ray = new Ray(cam.transform.position, cam.transform.forward * 3);
			RaycastHit hit;
			Debug.DrawRay(ray.origin, ray.direction * 3, Color.red);
			if (Physics.Raycast (ray, out hit, 3)) {
				if (hit.collider.tag == "Rune") {
					targetName.GetComponent<Text>().text = hit.collider.name;
				} else {
					targetName.GetComponent<Text>().text = "";
				}
			} else {
				targetName.GetComponent<Text>().text = "";
			}
			if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.A)) {
				anim.SetBool ("Walking", true);
				isWalking = true;
			} else {
				anim.SetBool ("Walking", false);
				isWalking = false;
			}
			grounded = isGrounded ();
			if (grounded) {
				anim.SetBool ("Jumping", false);
			}
			if (playerRigidbody.velocity.y < 0) {
				playerRigidbody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
			} else if (playerRigidbody.velocity.y > 0 && !Input.GetKey (KeyCode.Space)) {
				playerRigidbody.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
			}
			if (Input.GetKey (KeyCode.LeftShift) && grounded) {
				isSprinting = true;
				anim.SetBool ("Running", true);
			} else if (!Input.GetKey (KeyCode.LeftShift)) {
				isSprinting = false;
				anim.SetBool ("Running", false);
			}
			float v = Input.GetAxisRaw ("Vertical");
			float h = Input.GetAxisRaw ("Horizontal");
			Move (v, h);
			//Jumping ();
		}
	}

	void Move (float v, float h)
	{
		if (Input.GetKey (KeyCode.W)) 
		{
			float tempSpeed;
			if (isSprinting) {
				tempSpeed = speed * 1.5f;
			} else {
				tempSpeed = speed;
			}
			if (Input.GetKey (KeyCode.D)) {
				playerRigidbody.MovePosition (transform.position + (transform.right * Time.deltaTime * tempSpeed / 2) + (transform.forward * Time.deltaTime * tempSpeed));
			} else if (Input.GetKey (KeyCode.A)) {
				playerRigidbody.MovePosition (transform.position - (transform.right * Time.deltaTime * tempSpeed / 2) + (transform.forward * Time.deltaTime * tempSpeed));
			} else {
				playerRigidbody.MovePosition (transform.position + transform.forward * Time.deltaTime * tempSpeed);
			}
		} 
		else if (Input.GetKey (KeyCode.S)) 
		{
			if (Input.GetKey (KeyCode.D)) {
				playerRigidbody.MovePosition (transform.position + (transform.right * Time.deltaTime * speed / 2) - (transform.forward * Time.deltaTime * speed/2));
			} else if (Input.GetKey (KeyCode.A)) {
				playerRigidbody.MovePosition (transform.position - (transform.right * Time.deltaTime * speed / 2) - (transform.forward * Time.deltaTime * speed/2));
			} else {
				playerRigidbody.MovePosition (transform.position - transform.forward * Time.deltaTime * speed);
			}
		}
		else if (Input.GetKey (KeyCode.D)) 
		{
			playerRigidbody.MovePosition (transform.position + transform.right * Time.deltaTime * speed/2);
		}
		else if (Input.GetKey (KeyCode.A)) 
		{
			playerRigidbody.MovePosition (transform.position - transform.right * Time.deltaTime * speed/2);
		}
	}	
	
	void Jumping()
	{
		if (Input.GetKey (KeyCode.Space) && grounded) {
			playerRigidbody.velocity = Vector3.up * jumpVelocity;
			anim.SetBool ("Jumping", true);
			anim.Play("Jumping", -1, 0f);
		}
	}

	bool isGrounded() {
		return Physics.Raycast (transform.position, Vector3.down, distToGround);
	}

	public void Hide() {
		if (!isHiding) {
			isActive = true;
			targetName.GetComponent<Text>().text = "";
			isHiding = true;
			doneHiding = false;
			StartCoroutine(Hiding());
		}
	}

	IEnumerator Hiding() {
		moveTowardsHiding = true;
		cam.GetComponent<MouseLook>().enabled = false;
		GetComponent<MouseLook>().enabled = false;
		GetComponent<CapsuleCollider>().enabled = false;
		yield return new WaitForSeconds(0.5f);
		moveTowardsHiding = false;
		transform.position = new Vector3(-5.5f, 0, 3);
		crouchWhileHiding = true;
		yield return new WaitForSeconds(0.5f);
		crouchWhileHiding = false;
		transform.position = new Vector3(-5.5f, -1, 3);
		crawlIntoHiding = true;
		yield return new WaitForSeconds(0.7f);
		crawlIntoHiding = false;
		transform.position = new Vector3(-4.75f, -1, 4.3f);
		rotateWhileHiding = true;
		yield return new WaitForSeconds(1);
		rotateWhileHiding = false;
		doneHiding = true;
	}

	IEnumerator GetOutOfHiding() {
		crawlOutOfHiding = true;
		yield return new WaitForSeconds(0.5f);
		crawlOutOfHiding = false;
		transform.position = new Vector3(-5.5f, -1, 3);
		standOutOfHiding = true;
		yield return new WaitForSeconds(0.5f);
		standOutOfHiding = false;
		transform.position = new Vector3(-5.5f, 0, 3);
		isActive = false;
		isHiding = false;
		cam.GetComponent<MouseLook>().enabled = true;
		GetComponent<MouseLook>().enabled = true;
		GetComponent<CapsuleCollider>().enabled = true;
	}
}
