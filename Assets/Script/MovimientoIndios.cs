﻿using UnityEngine;
using System.Collections;

public class MovimientoIndios : MonoBehaviour {
		public Transform[] points;
		private int destPoint = 0;
		private NavMeshAgent agent;
		float tiempo=5;
		Animator animator;

		void Start () {
				agent = GetComponent<NavMeshAgent>();
				animator = GetComponent<Animator> ();

				// Disabling auto-braking allows for continuous movement
				// between points (ie, the agent doesn't slow down as it
				// approaches a destination point).
				agent.autoBraking = false;

				GotoNextPoint();
		}


		void GotoNextPoint() {
				// Returns if no points have been set up
				if (points.Length == 0)
						return;

				// Set the agent to go to the currently selected destination.
				agent.destination = points[destPoint].position;

				// Choose the next point in the array as the destination,
				// cycling to the start if necessary.
				destPoint = (destPoint + 1) % points.Length;
		}


		void Update () {
				// Choose the next destination point when the agent gets
				// close to the current one.
				if (agent.remainingDistance < 0.5f) {
						tiempo -= Time.deltaTime;
						animator.SetFloat ("speed", 0.0f);
						agent.speed = 0f;
						if (tiempo < 0) {
								GotoNextPoint ();
								agent.speed = 2f;
								tiempo = 10;
						}
				} else {
						animator.SetFloat ("speed", 1.0f);
				}
		}
}