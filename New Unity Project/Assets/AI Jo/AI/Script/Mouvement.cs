using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mouvement : MonoBehaviour
{
	public WheelCollider Wheel_Collider_FL;
	public WheelCollider Wheel_Collider_FR;
	public WheelCollider Wheel_Collider_RL;
	public WheelCollider Wheel_Collider_RR;

	public Transform Wheel_Transformation_FL;
	public Transform Wheel_Transformation_FR;
	public Transform Wheel_Transformation_RL;
	public Transform Wheel_Transformation_RR;

	private Rigidbody rb;

	private WheelHit hit;

	private Vector3 orientation;

	public float maxBrakeTorque = 1000;
	public float maxTorque = 1000;
	public float steeringAngle;
	private float tempSteeringAngle;
	public float hauteurReset;
	public float forceAntiFlip = 100;

	public static float vitesse;

	public static bool BoutonChanger = false;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void OnCollisionStay(Collision collision)
	{
		if (collision.gameObject.tag == "Herbe" || collision.gameObject.tag == "Rampe" || collision.gameObject.tag == "Asphalte")
		{
			if (Input.GetKeyDown(KeyCode.R))
			{
				rb.MovePosition(new Vector3(rb.position.x, rb.position.y + hauteurReset, rb.position.z));
				rb.MoveRotation(Quaternion.Euler(rb.transform.localEulerAngles.x, rb.transform.localEulerAngles.y, 0));
				rb.velocity = new Vector3(0f, 0f, 0f);
				rb.angularVelocity = new Vector3(0f, 0f, 0f);
			}
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "BoutonChanger")
		{
			BoutonChanger = true;
		}
	}


	public void Avancer(float motor, float brake, float hax)
	{
		if (Mathf.Abs(vitesse) < 50)
		{
			tempSteeringAngle = (-(Mathf.Abs(vitesse) / 5) + steeringAngle);
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


	void FixedUpdate()
	{
		orientation = transform.InverseTransformDirection(rb.velocity);
		vitesse = orientation.z * 3.6f;

		
		if (vitesse > 5)
		{
			if (Input.GetKey(KeyCode.W))
			{
				Avancer(maxTorque, 0, Input.GetAxis("Horizontal"));

				//Wheel_Collider_FL.motorTorque = maxTorque;
				//Wheel_Collider_FR.motorTorque = maxTorque;
				//Wheel_Collider_RR.motorTorque = maxTorque;
				//Wheel_Collider_RL.motorTorque = maxTorque;
				//Wheel_Collider_FL.brakeTorque = 0;
				//Wheel_Collider_FR.brakeTorque = 0;
				//Wheel_Collider_RL.brakeTorque = 0;
				//Wheel_Collider_RR.brakeTorque = 0;
			}
			else if (Input.GetKey(KeyCode.S))
			{
				Avancer(0, maxBrakeTorque * 20, Input.GetAxis("Horizontal"));
				
				//Wheel_Collider_FL.motorTorque = 0;
				//Wheel_Collider_FR.motorTorque = 0;
				//Wheel_Collider_RR.motorTorque = 0;
				//Wheel_Collider_RL.motorTorque = 0;
				//Wheel_Collider_FL.brakeTorque = maxBrakeTorque * 20;
				//Wheel_Collider_FR.brakeTorque = maxBrakeTorque * 20;
				//Wheel_Collider_RL.brakeTorque = maxBrakeTorque * 20;
				//Wheel_Collider_RR.brakeTorque = maxBrakeTorque * 20;
			}
			else
			{
				Avancer(0, 0, Input.GetAxis("Horizontal"));
				
				//Wheel_Collider_FL.motorTorque = 0;
				//Wheel_Collider_FR.motorTorque = 0;
				//Wheel_Collider_RR.motorTorque = 0;
				//Wheel_Collider_RL.motorTorque = 0;
				//Wheel_Collider_FL.brakeTorque = 0;
				//Wheel_Collider_FR.brakeTorque = 0;
				//Wheel_Collider_RL.brakeTorque = 0;
				//Wheel_Collider_RR.brakeTorque = 0;
			}
		}
		else if (vitesse < -5)
		{
			if (Input.GetKey(KeyCode.W))
			{
				Avancer(0, maxBrakeTorque * 20, Input.GetAxis("Horizontal"));
				
				//Wheel_Collider_FL.motorTorque = 0;
				//Wheel_Collider_FR.motorTorque = 0;
				//Wheel_Collider_RR.motorTorque = 0;
				//Wheel_Collider_RL.motorTorque = 0;
				//Wheel_Collider_FL.brakeTorque = maxBrakeTorque * 20;
				//Wheel_Collider_FR.brakeTorque = maxBrakeTorque * 20;
				//Wheel_Collider_RL.brakeTorque = maxBrakeTorque * 20;
				//Wheel_Collider_RR.brakeTorque = maxBrakeTorque * 20;
			}
			else if (Input.GetKey(KeyCode.S))
			{
				Avancer(-maxTorque, 0, Input.GetAxis("Horizontal"));
				
				//Wheel_Collider_FL.motorTorque = -maxTorque;
				//Wheel_Collider_FR.motorTorque = -maxTorque;
				//Wheel_Collider_RR.motorTorque = -maxTorque;
				//Wheel_Collider_RL.motorTorque = -maxTorque;
				//Wheel_Collider_FL.brakeTorque = 0;
				//Wheel_Collider_FR.brakeTorque = 0;
				//Wheel_Collider_RL.brakeTorque = 0;
				//Wheel_Collider_RR.brakeTorque = 0;
			}
			else
			{
				Avancer(0, 0, Input.GetAxis("Horizontal"));
				
				//Wheel_Collider_FL.motorTorque = 0;
				//Wheel_Collider_FR.motorTorque = 0;
				//Wheel_Collider_RR.motorTorque = 0;
				//Wheel_Collider_RL.motorTorque = 0;
				//Wheel_Collider_FL.brakeTorque = 0;
				//Wheel_Collider_FR.brakeTorque = 0;
				//Wheel_Collider_RL.brakeTorque = 0;
				//Wheel_Collider_RR.brakeTorque = 0;
			}
		}
		else
		{
			if (Input.GetKey(KeyCode.W))
			{
				Avancer(maxTorque, 0, Input.GetAxis("Horizontal"));
				
				//Wheel_Collider_FL.motorTorque = maxTorque;
				//Wheel_Collider_FR.motorTorque = maxTorque;
				//Wheel_Collider_RR.motorTorque = maxTorque;
				//Wheel_Collider_RL.motorTorque = maxTorque;
				//Wheel_Collider_FL.brakeTorque = 0;
				//Wheel_Collider_FR.brakeTorque = 0;
				//Wheel_Collider_RL.brakeTorque = 0;
				//Wheel_Collider_RR.brakeTorque = 0;
			}
			else if (Input.GetKey(KeyCode.S))
			{
				Avancer(-maxTorque, 0, Input.GetAxis("Horizontal"));
				
				//Wheel_Collider_FL.motorTorque = -maxTorque;
				//Wheel_Collider_FR.motorTorque = -maxTorque;
				//Wheel_Collider_RR.motorTorque = -maxTorque;
				//Wheel_Collider_RL.motorTorque = -maxTorque;
				//Wheel_Collider_FL.brakeTorque = 0;
				//Wheel_Collider_FR.brakeTorque = 0;
				//Wheel_Collider_RL.brakeTorque = 0;
				//Wheel_Collider_RR.brakeTorque = 0;
			}
			else
			{
				Avancer(0, 0, Input.GetAxis("Horizontal"));
				
				//Wheel_Collider_FL.motorTorque = 0;
				//Wheel_Collider_FR.motorTorque = 0;
				//Wheel_Collider_RR.motorTorque = 0;
				//Wheel_Collider_RL.motorTorque = 0;
				//Wheel_Collider_FL.brakeTorque = 0;
				//Wheel_Collider_FR.brakeTorque = 0;
				//Wheel_Collider_RL.brakeTorque = 0;
				//Wheel_Collider_RR.brakeTorque = 0;
			}
		}
		/*
		int[] vectorAction = new int[2];
		
		vectorAction[0] = Random.Range(0, 2);
		vectorAction[1] = Random.Range(0, 2);


		if (vitesse > 5)
		{
			if (vectorAction[0] == 1)
			{
				if (vectorAction[1] == 0)
				{
					Avancer(maxTorque, 0, 0);
				}
				else if (vectorAction[1] == 1)
				{
					Avancer(maxTorque, 0, 1);
				}
				else if (vectorAction[1] == 2)
				{
					Avancer(maxTorque, 0, -1);
				}
			}
			else if (vectorAction[0] == 2)
			{
				if (vectorAction[1] == 0)
				{
					Avancer(0, maxBrakeTorque * 20, 0);
				}
				else if (vectorAction[1] == 1)
				{
					Avancer(0, maxBrakeTorque * 20, 1);
				}
				else if (vectorAction[1] == 2)
				{
					Avancer(0, maxBrakeTorque * 20, -1);
				}


			}
			else if (vectorAction[0] == 2)
			{
				if (vectorAction[1] == 0)
				{
					Avancer(0, 0, 0);
				}
				else if (vectorAction[1] == 1)
				{
					Avancer(0, 0, 1);
				}
				else if (vectorAction[1] == 2)
				{
					Avancer(0, 0, -1);
				}

			}
		}
		else if (vitesse < -5)
		{
			if (vectorAction[0] == 0)
			{
				if (vectorAction[1] == 0)
				{
					Avancer(0, maxBrakeTorque * 20, 0);
				}
				else if (vectorAction[1] == 1)
				{
					Avancer(0, maxBrakeTorque * 20, 1);
				}
				else if (vectorAction[1] == 2)
				{
					Avancer(0, maxBrakeTorque * 20, -1);
				}
			}
			else if (vectorAction[0] == 1)
			{
				if (vectorAction[1] == 0)
				{
					Avancer(-maxTorque, 0, 0);
				}
				else if (vectorAction[1] == 1)
				{
					Avancer(-maxTorque, 0, 1);
				}
				else if (vectorAction[1] == 2)
				{
					Avancer(-maxTorque, 0, -1);
				}
			}
			else if (vectorAction[0] == 2)
			{
				if (vectorAction[1] == 0)
				{
					Avancer(0, 0, 0);
				}
				else if (vectorAction[1] == 1)
				{
					Avancer(0, 0, 1);
				}
				else if (vectorAction[1] == 2)
				{
					Avancer(0, 0, -1);
				}
			}
		}
		else
		{
			if (vectorAction[0] == 0)
			{
				if (vectorAction[1] == 0)
				{
					Avancer(maxTorque, 0, 0);
				}
				else if (vectorAction[1] == 1)
				{
					Avancer(maxTorque, 0, 1);
				}
				else if (vectorAction[1] == 2)
				{
					Avancer(maxTorque, 0, -1);
				}
			}
			else if (vectorAction[0] == 1)
			{
				if (vectorAction[1] == 0)
				{
					Avancer(-maxTorque, 0, 0);
				}
				else if (vectorAction[1] == 1)
				{
					Avancer(-maxTorque, 0, 1);
				}
				else if (vectorAction[1] == 2)
				{
					Avancer(-maxTorque, 0, -1);
				}
			}
			else if (vectorAction[0] == 2)
			{
				if (vectorAction[1] == 0)
				{
					Avancer(0, 0, 0);
				}
				else if (vectorAction[1] == 1)
				{
					Avancer(0, 0, 1);
				}
				else if (vectorAction[1] == 2)
				{
					Avancer(0, 0, -1);
				}
			}
		}
		*/
		
	}

	void Update()
	{
		//changing tyre direction
		Vector3 temp = Wheel_Transformation_FL.localEulerAngles;
		Vector3 temp1 = Wheel_Transformation_FR.localEulerAngles;
		temp.y = Wheel_Collider_FL.steerAngle - (Wheel_Transformation_FL.localEulerAngles.z);
		Wheel_Transformation_FL.localEulerAngles = temp;
		temp1.y = Wheel_Collider_FR.steerAngle - Wheel_Transformation_FR.localEulerAngles.z;
		Wheel_Transformation_FR.localEulerAngles = temp1;
		/*
		Change_Position_Roue(Wheel_Collider_FL, Wheel_Transformation_FL);
		Change_Position_Roue(Wheel_Collider_FR, Wheel_Transformation_FR);
		Change_Position_Roue(Wheel_Collider_RL, Wheel_Transformation_RL);
		Change_Position_Roue(Wheel_Collider_RR, Wheel_Transformation_RR);
		*/
	}


	private void Change_Position_Roue(WheelCollider _collider, Transform _transform)
	{
		Vector3 _pos = _transform.position;
		Quaternion _quat = _transform.rotation;

		_collider.GetWorldPose(out _pos, out _quat);

		_transform.position = _pos;
		_transform.Rotate(Wheel_Collider_FL.rpm / 60 * 360 * Time.deltaTime, 0, 0);

		ForceVersLeBas();

	}

	//Plus la voiture va vite, plus il y a un grande force d'appliquer sur la voiture pour
	//l'empecher de se renverser
	private void ForceVersLeBas()
	{
		Wheel_Collider_FL.attachedRigidbody.AddForce(-transform.up * forceAntiFlip *
													 Wheel_Collider_FL.attachedRigidbody.velocity.magnitude);
	}
}
