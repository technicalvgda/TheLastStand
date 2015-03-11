﻿using UnityEngine;
using System.Collections;

public class TripleShotEnemyScript : MonoBehaviour {
		
		public double health, fireRate;
		public float arrowVelocity;
		public GameObject arrowPrefab;
		double currentTime;
		
			Vector3 theta, arrowDirMain, arrowDirLeft, arrowDirRight;
		
			// Use this for initialization
		void Start () {
				theta = new Vector3(0, 0, Random.value*360);
				arrowDirMain = new Vector3 (Mathf.Cos(theta.z * Mathf.PI / 180), Mathf.Sin(theta.z * Mathf.PI / 180));
				arrowDirLeft = new Vector3 (Mathf.Cos((theta.z + 30) * Mathf.PI / 180), Mathf.Sin(theta.z * Mathf.PI / 180));
				arrowDirRight = new Vector3 (Mathf.Cos((theta.z - 30) * Mathf.PI / 180), Mathf.Sin (theta.z * Mathf.PI / 180));

					transform.Rotate(theta);
			}
		
			// Update is called once per frame
		void Update () {
				currentTime += Time.deltaTime;
				if (currentTime >= fireRate) {
						GameObject centerarrow = ObjectPool.instance.GetObjectForType("Center Projectile", false);
						GameObject leftarrow = ObjectPool.instance.GetObjectForType("Left Projectile", false);
						GameObject rightarrow = ObjectPool.instance.GetObjectForType("Right Projectile", false);
						centerarrow.rigidbody2D.velocity = arrowDirMain * arrowVelocity;
						leftarrow.rigidbody2D.velocity = arrowDirLeft * arrowVelocity;
						rightarrow.rigidbody2D.velocity = arrowDirRight * arrowVelocity;
						currentTime = 0;
						//All the arrow prefab needs is a rigidBody2D with everything 0'd out.
							//The arrow will only have to time out and kill itself.
					}
			}
		
		void OnColliderEnter2D(Collider col) {
				//code if we are not using object pools.
					if(!col.tag.Equals("Enemy")) //Incase of non hero hits. 
							Destroy(gameObject);
				//Code if we are using object pools
			}
	}