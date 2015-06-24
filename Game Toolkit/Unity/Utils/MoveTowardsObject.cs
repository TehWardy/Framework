using UnityEngine;
using System.Collections;

public class MoveTowardsObject : MonoBehaviour {

	public Transform targetTrasform;

	//the speed, in units per second, we want to move towards the target
	public float speed = 5;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		MoveTowardsTarget();
	}

	//move towards a target at a set speed.
	private void MoveTowardsTarget() {
		//move towards the center of the world (or where ever you like)
		Vector3 targetPosition = targetTrasform.position;

		Vector3 currentPosition = this.transform.position;
		//first, check to see if we're close enough to the target
		//this check prevents us from oscillating back and forth over the target
		//if we're farther than 1 unit away, do the movement, otherwise do nothing
		if(Vector3.Distance(currentPosition, targetPosition) > 1) { 
			
			//get the direction we need to go by subtracting the current position from the target position
			Vector3 directionOfTravel = targetPosition - currentPosition;
			//now normalize the direction, since we only want the direction information
			directionOfTravel.Normalize();
			
			//now move at the specified speed in the direction of travel
			//scale the movement on each axis by the directionOfTravel vector components
			
			this.transform.Translate(
				(directionOfTravel.x * speed * Time.deltaTime),
				(directionOfTravel.y * speed * Time.deltaTime),
				(directionOfTravel.z * speed * Time.deltaTime),
				Space.World);
			
			
		}
	}
}
