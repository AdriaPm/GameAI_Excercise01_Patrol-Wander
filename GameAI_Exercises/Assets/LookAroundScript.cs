using UnityEngine;

public class LookAroundScript : MonoBehaviour 
{
	public float mouseSensibility = 4f;

	float yaw = 0f;
	float pitch = 0f;

	public float maxY = -65; // For some reason, the signs are strange.
	public float minY = 50;

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
        yaw = 180;
        pitch = 90;
	}

	private void Update()
	{
		yaw += mouseSensibility * Input.GetAxis("Mouse X");
		pitch -= mouseSensibility * Input.GetAxis("Mouse Y");
		pitch = Mathf.Clamp(pitch, maxY, minY); // Clamp viewing up and down

		transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
	}
}