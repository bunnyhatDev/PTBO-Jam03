﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
	private bool m_activeBall;
	private Rigidbody m_rgb;
	private Vector3 m_ballPosition;
	private Vector3 m_ballForce;
	public float m_speed = 60;
	public GameObject m_player;
	public AudioSource m_audioSource;
	public AudioClip m_BAHHgel, m_legHHAB;
	

	float velz;

	// Use this for initialization
	void Start () {
		m_rgb = GetComponent<Rigidbody>();
		m_ballForce = new Vector3(500.0f, 0.0f, 3500.0f);
		m_activeBall = false;
		m_ballPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(!m_activeBall && m_player !=null){
			m_ballPosition.x = m_player.transform.position.x;
			transform.position = m_ballPosition;
		}

		// velz = m_rgb.velocity.z;
		// m_rgb.AddForce(0, 0, velz / 2.5f);
		// velz = Mathf.Clamp(m_rgb.velocity.z, velz, velz);

		// if(velz >= -15f || velz <= -60f) {
		// 	velz = -45f;
		// }

		// if(m_activeBall && transform.position.z < - 6){
		// 	m_activeBall = false;
		// 	m_ballPosition.x = m_player.transform.position.x;
		// 	m_ballPosition.z = -0.95f;
		// 	transform.position = m_ballPosition;

		// 	m_rgb.isKinematic = true;
		// 	m_player.GetComponent<Player>().HaveBall();
		// }
	}

    float hitFactor(Vector3 ballPos, Vector3 racketPos,
                    float racketHeight) {
        // ascii art:
        // ||  1 <- at the top of the racket
        // ||
        // ||  0 <- at the middle of the racket
        // ||
        // || -1 <- at the bottom of the racket
        return (ballPos.y - racketPos.y) / racketHeight;
    }

	void OnCollisionEnter(Collision col) {
		if(this.gameObject.tag == "Player 1") {
			// Hit the left Racket?
			if (col.gameObject.name == "RacketLeft") {
				// Calculate hit Factor
				float z = hitFactor(transform.position,
									col.transform.position,
									col.collider.bounds.size.y);
				
				// Calculate direction, make length=1 via .normalized
				Vector3 dir = new Vector3(-1, 1, z).normalized;

				Debug.Log(dir);

				// Set Velocity with dir * speed
				m_rgb.velocity = dir * m_speed;
			}

			// Hit the right Racket?
			if (col.gameObject.name == "RacketRight") {
				// Calculate hit Factor
				float z = hitFactor(transform.position,
									col.transform.position,
									col.collider.bounds.size.y);

				// Calculate direction, make length=1 via .normalized
				Vector3 dir = new Vector3(1, 1, z).normalized;
				
				// Set Velocity with dir * speed
				m_rgb.velocity = dir * m_speed;
			}
		}

		if(col.gameObject.tag == "MidBrick") {
			Debug.Log("Middle Brick Hit by: " + gameObject.tag);
			m_audioSource.PlayOneShot(m_legHHAB, 1);
			// Debug.Log("Middle Brick Hit by: " + gameObject.GetComponent<Player>().name);
			if(col.gameObject.GetComponent<MeshRenderer>().material.color != m_player.GetComponent<Player>().m_color) {
				col.gameObject.GetComponent<MeshRenderer>().material.color = m_player.GetComponent<Player>().m_color;
				Debug.Log("Color should change...");
			} else if(col.gameObject.GetComponent<MeshRenderer>().material.color == m_player.GetComponent<Player>().m_color) {
				Debug.Log("Brick goes Boom!");
				m_player.GetComponent<Player>().SetScore(50);
				Destroy(col.gameObject);
			}
		}

		if(col.gameObject.tag == "Brick") {
			m_audioSource.PlayOneShot(m_BAHHgel, 1);
			m_player.GetComponent<Player>().SetScore(50);
			Destroy(col.gameObject);
		}


    }

	void OnTriggerExit(Collider other) {
		if(m_player.name == "Player1") {
			if(other.gameObject.tag == "Powerup") {
				// Destroy(other.gameObject);
			}
		} else {
			if(other.gameObject.tag == "Powerup") {
				// m_powerupManager.m_pUpSpeed = -m_powerupManager.m_pUpSpeed;
				Destroy(other.gameObject);
			}
		}
	}

	public void MakeInactive(){
		m_activeBall = false;
	}

	public void Release(){
		if(!m_activeBall){
			m_rgb.isKinematic = false;
			m_rgb.velocity = Vector3.forward * m_speed;
			// m_rgb.AddForce(m_ballForce);
			m_activeBall = true;
		}
	}


		
}
