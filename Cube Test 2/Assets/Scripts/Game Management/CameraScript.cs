using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	private Transform[] playerTransforms;


	private void Start(){

		GameObject[] allPlayers = GameObject.FindGameObjectsWithTag ("Player");
		playerTransforms = new Transform[allPlayers.Length];
		for (int i = 0; i < allPlayers.Length; i++) {
			playerTransforms[i] = allPlayers[i].transform;
		}
        //print(allPlayers[0].gameObject.GetComponent<CharacterStateController>().healthPoints);

    }

	public float yOffset = 2.0f;
	public float minDistance = 10.0f;
	public float maxDistance = 5.0f;

    public float zOffset = -6.5f;

	private float xMin, xMax, yMin, yMax;

	private void LateUpdate(){

		if (playerTransforms.Length == 0) {
			Debug.Log ("No players found");
			return;
		}

		xMin = xMax = playerTransforms[0].position.x;
		yMin = yMax = playerTransforms[0].position.y;
		
		foreach(Transform player in playerTransforms)
		{
			if (player.position.x < xMin)
			{
				xMin = player.position.x;
			}

			if (player.position.x > xMax)
			{
				xMax = player.position.x;
			}

			if (player.position.y < yMin)
			{
				yMin = player.position.y;
			}

			if (player.position.y > yMax)
			{
				yMax = player.position.y;
			}
		}


		float xMiddle = (xMin + xMax) / 2;
		float yMiddle = ((yMin + yMax) / 2) + (float) 1.5;
		float distance = xMin + xMax;

		if (distance < minDistance) {
			distance = minDistance;
		}
		else if (distance > maxDistance){
			distance = maxDistance;
		}
		transform.position = new Vector3 (xMiddle, yMiddle, -distance/2 + zOffset);
	}
}
