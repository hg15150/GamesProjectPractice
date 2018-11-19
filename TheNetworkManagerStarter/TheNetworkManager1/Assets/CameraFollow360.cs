using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow360 : MonoBehaviour {

	static public Transform player;
	public float distance = 10;
	public float height = 15;
	public Vector3 lookOffset = new Vector3(0,1,0);
	public float cameraSpeed = 25;
	public float rotSpeed = 50;

	void LateUpdate () 
	{
		if(player)
		{
			Vector3 lookPosition = player.position + lookOffset;
			Vector3 relativePos = lookPosition - transform.position;
        	Quaternion rot = Quaternion.LookRotation(relativePos);
			
			transform.rotation = Quaternion.Slerp(this.transform.rotation, rot, Time.deltaTime * rotSpeed * 1f);
			
			Vector3 targetPos = player.transform.position + player.transform.up * height - player.transform.forward * distance;
			
			this.transform.position = Vector3.Lerp(this.transform.position, targetPos, Time.deltaTime * cameraSpeed * 0.1f);
		}
	}
}
