using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BasicPlayerMovement : MonoBehaviour
{
	public GameObject floatingMask;
	public GameObject targetName;
	public bool isActive;
	public GameObject lookingAt;

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

	public GameObject mannequinManager;

	public bool isSitting = false;
	bool moveTowardsCouch = false;
	bool sit = false;
	bool scootForward = false;
	bool standUpFromCouch = false;
	public bool doneSitting = false;
	public float timeSpentSitting = 0;
	public float timeRequiredToSit;

	public GameObject car;
	public bool moveTowardsCar = false;

	public bool crawlOutOfHiding;
	public bool standOutOfHiding;

	public float counter = 0;

	void Start () {
		isSitting = false;
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
		if (doneSitting) {
			timeSpentSitting += Time.deltaTime;
			if (timeSpentSitting >= timeRequiredToSit && mannequinManager.GetComponent<MannequinManager>().isSitting) {
				mannequinManager.GetComponent<MannequinManager>().NoLongerSitting();
			}
		}
		if (Input.GetKey(KeyCode.Escape)) {
			Cursor.lockState = CursorLockMode.None;
		}
		if (Input.GetMouseButtonDown(0) && doneHiding) {
			doneHiding = false;
			StartCoroutine(GetOutOfHiding());
		}
		if (Input.GetMouseButtonDown(0) && doneSitting) {
			doneSitting = false;
			StartCoroutine(StandUp());
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
		counter += Time.deltaTime;
		if (moveTowardsCouch) {
			transform.position = Vector3.Lerp(transform.position, new Vector3(2, 0, 3), counter / 6);
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -130, 0), counter / 3);
			cam.transform.localRotation = Quaternion.Slerp(cam.transform.localRotation, Quaternion.Euler(0, 0, 0), 4 * Time.deltaTime);
		}
		if (sit) {
			transform.position = Vector3.Lerp(transform.position, new Vector3(3, -0.25f, 3.85f), 6 * Time.deltaTime);
		}
		if (scootForward) {
			transform.position = Vector3.Lerp(transform.position, new Vector3(2.5f, -0.25f, 3.425f), counter / 1);
		}
		if (standUpFromCouch) {
			transform.position = Vector3.Lerp(transform.position, new Vector3(2, 0, 3), counter / 1);
		}
		if (moveTowardsCar) {
			transform.position = Vector3.Lerp(transform.position, new Vector3(3.76f, 0, -4.3f), counter / 1.5f);
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 182, 0), 4 * Time.deltaTime);
			cam.transform.localRotation = Quaternion.Slerp(cam.transform.localRotation, Quaternion.Euler(6, 0, 0), 4 * Time.deltaTime);
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
					lookingAt = hit.collider.gameObject;
				} else {
					targetName.GetComponent<Text>().text = "";
					lookingAt = null;
				}
			} else {
				targetName.GetComponent<Text>().text = "";
				lookingAt = null;
			}
			if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.A)) {
				anim.SetBool ("Walking", true);
				isWalking = true;
			} else {
				anim.SetBool ("Walking", false);
				isWalking = false;
			}
			Move ();
		}
	}

	void Move ()
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

	public void Hide() {
		if (!isHiding) {
			isActive = true;
			targetName.GetComponent<Text>().text = "";
			isHiding = true;
			doneHiding = false;
			StartCoroutine(Hiding());
		}
	}

	public void SitDown() {
		if (!isSitting) {
			isActive = true;
			targetName.GetComponent<Text>().text = "";
			isSitting = true;
			doneSitting = false;
			StartCoroutine(Sitting());
		}
	}

	public void WindUpCar() {
		isActive = true;
		targetName.GetComponent<Text>().text = "";
		StartCoroutine(WindingUpCar());
	}

	public void DoneWinding() {
		GetComponent<CapsuleCollider>().enabled = true;
		cam.GetComponent<MouseLook>().xRotation = cam.transform.eulerAngles.x;
		cam.GetComponent<MouseLook>().enabled = true;
		isActive = false;
	}

	public void DieFromMask(GameObject mask) {
		isActive = true;
		cam.GetComponent<MouseLook>().enabled = false;
		//GetComponent<MouseLook>().enabled = false;
		transform.position = new Vector3(transform.position.x, 0, transform.position.z);
		cam.transform.LookAt(new Vector3(mask.transform.position.x, 0.7f, mask.transform.position.z));
	}

	public void DieFromSpider() {
		isActive = true;
		cam.GetComponent<MouseLook>().enabled = false;
		//GetComponent<MouseLook>().enabled = false;
		transform.position = new Vector3(transform.position.x, 0, transform.position.z);
		cam.transform.eulerAngles = new Vector3(-90, cam.transform.eulerAngles.y, cam.transform.eulerAngles.z);
	}

	public void DieFromMannequin() {
		isActive = true;
		cam.GetComponent<MouseLook>().enabled = false;
		//GetComponent<MouseLook>().enabled = false;
		cam.transform.eulerAngles = new Vector3(0, cam.transform.eulerAngles.y, cam.transform.eulerAngles.z);
	}


	IEnumerator Hiding() {
		moveTowardsHiding = true;
		cam.GetComponent<MouseLook>().enabled = false;
		//GetComponent<MouseLook>().enabled = false;
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
		GetComponent<CapsuleCollider>().enabled = true;
		isActive = false;
		isHiding = false;
		cam.GetComponent<MouseLook>().xRotation = cam.transform.eulerAngles.x;
		if (!floatingMask.GetComponent<MaskMovement>().willAttack) {
			cam.GetComponent<MouseLook>().enabled = true;
			//GetComponent<MouseLook>().enabled = true;
		}
	}

	IEnumerator Sitting() {
		isWalking = false;
		anim.SetBool ("Walking", false);
		counter = 0;
		moveTowardsCouch = true;
		cam.GetComponent<MouseLook>().enabled = false;
		GetComponent<CapsuleCollider>().enabled = false;
		yield return new WaitForSeconds(0.8f);
		moveTowardsCouch = false;
		sit = true;
		yield return new WaitForSeconds(0.8f);
		sit = false;
		timeSpentSitting = 0;
		doneSitting = true;
	}

	IEnumerator StandUp() {
		counter = 0;
		scootForward = true;
		yield return new WaitForSeconds(0.4f);
		scootForward = false;
		counter = 0;
		standUpFromCouch = true;
		yield return new WaitForSeconds(0.4f);
		standUpFromCouch = false;
		transform.position = new Vector3(2, 0, 3);
		GetComponent<CapsuleCollider>().enabled = true;
		cam.GetComponent<MouseLook>().xRotation = cam.transform.eulerAngles.x;
		cam.GetComponent<MouseLook>().enabled = true;
		isActive = false;
		isSitting = false;
	}

	IEnumerator WindingUpCar() {
		counter = 0;
		moveTowardsCar = true;
		cam.GetComponent<MouseLook>().enabled = false;
		GetComponent<CapsuleCollider>().enabled = false;
		anim.SetBool ("Walking", false);
		isWalking = false;
		yield return new WaitForSeconds(0.7f);
		moveTowardsCar = false;
		car.GetComponent<Drive>().PowerUpCar();
	}
}
