using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;


public class CarAgent : Agent
{
	public WheelCollider Wheel_Collider_FL;
	public WheelCollider Wheel_Collider_FR;
	public WheelCollider Wheel_Collider_RL;
	public WheelCollider Wheel_Collider_RR;

	public LayerMask raycastLayers;
	
	public float raycastDistance = 20;
	public Transform[] raycasts;

	private Transform environment;
	Mouvement carCtrl;
	private Road road;
	new private Rigidbody rigidbody;

	public float maxBrakeTorque = 1000;
	public float maxTorque = 1000;
	private bool tour = false;
	private int check = 0;

	public override void InitializeAgent()
	{
		base.InitializeAgent();
		road = GetComponentInParent<Road>();
		rigidbody = GetComponent<Rigidbody>();
		//carCtrl.transform.position = rigidbody.transform.position;
	}

	public override void AgentAction(float[] vectorAction)
	{
		int first = Mathf.FloorToInt(vectorAction[0]);
		int second = Mathf.FloorToInt(vectorAction[1]);
		//base.AgentAction(vectorAction);
		if (vectorAction[0] == 1f)
		{
			if (vectorAction[1] == 0f)
			{
				Avancer(maxTorque, 0, 0);
			}
			else if (vectorAction[1] == 1f)
			{
				Avancer(maxTorque, 0, 1);
			}
			else if (vectorAction[1] == 2f)
			{
				Avancer(maxTorque, 0, -1);
			}
		}
		else if (vectorAction[0] == 2f)
		{
			if (vectorAction[1] == 0f)
			{
				Avancer(0, maxBrakeTorque * 20, 0);
			}
			else if (vectorAction[1] == 1f)
			{
				Avancer(0, maxBrakeTorque * 20, 1);
			}
			else if (vectorAction[1] == 2f)
			{
				Avancer(0, maxBrakeTorque * 20, -1);
			}
		}
		else if (vectorAction[0] == 2f)
		{
			if (vectorAction[1] == 0f)
			{
				Avancer(0, 0, 0);
			}
			else if (vectorAction[1] == 1f)
			{
				Avancer(0, 0, 1f);
			}
			else if (vectorAction[1] == 2f)
			{
				Avancer(0, 0, -1);
			}
		}
				
		if (maxStep > 0)
		{
			AddReward(-1f / maxStep);
		}
	}
	
	public override float[] Heuristic()
	{
		float forwardAction = 0f;
		float turnAction = 0f;
		if (Input.GetKey(KeyCode.W))
		{
			// move forward
			forwardAction = 1f;
		}
		if (Input.GetKey(KeyCode.A))
		{
			// turn left
			turnAction = 1f;
		}
		else if (Input.GetKey(KeyCode.D))
		{
			// turn right
			turnAction = 2f;
		}

		// Put the actions into an array and return
		return new float[] { forwardAction, turnAction };
	}
	

	public override void CollectObservations()
	{
		AddVectorObs(transform.forward);

		for(int i = 0; i < raycasts.Length; i++)
		{
			AddRaycastVectorObs(raycasts[i]);
		}

		base.CollectObservations();
	}

	void AddRaycastVectorObs(Transform ray)
	{
		RaycastHit hitInfo = new RaycastHit();
		var hit = Physics.Raycast(ray.position, ray.forward, out hitInfo, raycastDistance, raycastLayers.value, QueryTriggerInteraction.Ignore);
		var distance = hitInfo.distance;
		if (!hit) distance = raycastDistance;
		var obs = distance / raycastDistance;
		AddVectorObs(obs);

		Debug.DrawRay(ray.position, ray.forward * distance, Color.Lerp(Color.red, Color.green, obs), Time.deltaTime * 2f);
	}

	public override void AgentReset()
	{
		road.ResetArea();
	}

	private void FixedUpdate()
	{
		if(GetStepCount() % 5 == 0)
		{
			RequestDecision();
		}
		else
		{
			RequestAction();
		}
		CheckPoints();
		nbTour();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.transform.CompareTag("mur"))
		{
			AddReward(-2f);
			AgentReset();
		}
		else if(collision.transform.CompareTag("Herbe"))
		{
			AddReward(-2f);
			AgentReset();
		}
	}

	private void nbTour()
	{
		if(tour_comlete.tours == 1)
		{
			if (tour == false)
			{
				AddReward(5f);
				tour = true;
			}
		}
		if(tour_comlete.tours == 2)
		{
			tour = false;
			AddReward(5f);
			Done();
		}
	}

	public void Avancer(float motor, float brake, float hax)
	{
		float tempSteeringAngle;

		if (Mathf.Abs(Mouvement.vitesse) < 50)
		{
			tempSteeringAngle = (-(Mathf.Abs(Mouvement.vitesse) / 5) + 25);
		}
		else
		{
			tempSteeringAngle = 15;
		}

		Wheel_Collider_FL.motorTorque = motor;
		Wheel_Collider_FR.motorTorque = motor;
		Wheel_Collider_RR.motorTorque = motor;
		Wheel_Collider_RL.motorTorque = motor;
		Wheel_Collider_FL.brakeTorque = brake;
		Wheel_Collider_FR.brakeTorque = brake;
		Wheel_Collider_RL.brakeTorque = brake;
		Wheel_Collider_RR.brakeTorque = brake;

		Wheel_Collider_FL.steerAngle = tempSteeringAngle * hax;
		Wheel_Collider_FR.steerAngle = tempSteeringAngle * hax;
	}

	public void CheckPoints()
	{
		if(CheckPoint.m_CPoint == check + 1)
		{
			check = CheckPoint.m_CPoint;
			AddReward(1f / 9f);

			if (Mouvement.vitesse > 60)
			{
				AddReward(1f);
			}
			else if (Mouvement.vitesse <= 60)
			{
				AddReward(Mouvement.vitesse / 60f);
			}
		}
	}
}
