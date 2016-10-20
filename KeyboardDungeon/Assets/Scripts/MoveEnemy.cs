using UnityEngine;
using System.Collections;

public class MoveEnemy : MonoBehaviour 
{
	// Declare variables
	[HideInInspector]									// Prevent public variable waypoints from appearing in Unity Inspector
	public GameObject[] waypoints;						// All waypoints are stored in here
	private int currentWaypoint = 0;					// Last waypoint that the enemy just passed
	private float lastWaypointSwitchTime;				// Time at which the last waypoint was passed
	public float speed = 1.0f;							// Speed at which the enemy is moving 


	// Use this for initialization
	void Start ()
	{
		// Initialize to current time
		lastWaypointSwitchTime = Time.time;
	}


	// Update is called once per frame
	void Update () 
	{
		// Initialize start- and end-waypoint for the current path segment
		Vector3 startPosition = waypoints [currentWaypoint].transform.position;
		Vector3 endPostion = waypoints [currentWaypoint + 1].transform.position;

		// Makes enemy walk along the path segment according to the defined speed
		float pathLength = Vector3.Distance (startPosition, endPostion);				// Calculate length of the path segment 
		float totalTimeForPath = pathLength / speed;									// Calculate time for which the enemy should cover the path segment
		float currentTimeOnPath = Time.time - lastWaypointSwitchTime;					// Calculate the time passed since the enemy started to move on the current path segment
		gameObject.transform.position = Vector3.Lerp (startPosition, endPostion, currentTimeOnPath / totalTimeForPath);			// Interpolation

		// Check if enemy has reached the last waypoint, if not update to new path segment otherwise destroy enemy and inflict damage...(in progress)
		if (gameObject.transform.position.Equals (endPostion))
		{
			// Update to new path segment and rotate enemy to movement direction
			if (currentWaypoint < waypoints.Length - 2)
			{
				// Update to new path segment
				currentWaypoint++;
				lastWaypointSwitchTime = Time.time;

				// Rotate enemy to movement direction
				RotateIntoMoveDirection ();
			}
			else 
			{
				// Destroy enemy
				Destroy (gameObject);

				// Triggers sound effect
				AudioSource audioSource = gameObject.GetComponent<AudioSource> ();
				AudioSource.PlayClipAtPoint (audioSource.clip, transform.position);

				// Inflict damage (ToDo)
			}
		}

	}


	// Define function to rotate the enemy accordingly to the movement direction
	private void RotateIntoMoveDirection ()
	{
		// Initialize start- and end-waypoint for the current path segment and determine current movement direction
		Vector3 newStartPosition = waypoints [currentWaypoint].transform.position;
		Vector3 newEndPostion = waypoints [currentWaypoint + 1].transform.position;
		Vector3 newDirection = (newEndPostion - newStartPosition);

		// Calculate angle between the current movement direction and the x-axis
		float x = newDirection.x;
		float z = newDirection.z;
		float rotationAngle = Mathf.Atan2 (z, x) * (180 / Mathf.PI);

		// Rotates the child Sprite according to the above calculated angle along the y-axis 
		GameObject sprite = (GameObject)
			gameObject.transform.FindChild ("Sprite").gameObject;
		sprite.transform.rotation = Quaternion.Euler (90, 0, rotationAngle);
	}
}
