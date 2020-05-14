using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleCar : MonoBehaviour
{
	[SerializeField] private Vector3 m_CentreOfMassOffset;
	[Range(0, 1)] [SerializeField] private float m_TractionControl;
	[SerializeField] private float m_FullTorqueOverAllWheels;
	[SerializeField] private float m_ReverseTorque;
	[SerializeField] private float m_MaxHandbrakeTorque;
	[Range(0, 1)] [SerializeField] private float m_SteerHelper;
	[SerializeField] private float m_Downforce = 100f;
	[SerializeField] private float m_Topspeed = 200;
	[SerializeField] private static int NoOfGears = 5;
	[SerializeField] private float m_RevRangeBoundary = 1f;
	[SerializeField] private float m_SlipLimit;
	[SerializeField] private float m_BrakeTorque;

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

	private float m_SteeringAngle;
	private int m_GearNum;
	private float m_GearFactor;
	private float m_OldRotation;
	private float m_CurrentTorque;

	public bool Skidding { get; private set; }
	public float BrakeInput { get; private set; }
	public float CurrentSteerAngle { get { return m_SteeringAngle; } }
	public float CurrentSpeed { get { return rb.velocity.magnitude * 2.23693629f; } }
	public float MaxSpeed { get { return m_Topspeed; } }
	public float Revs { get; private set; }
	public float AccelInput { get; private set; }

	public static float vitesse;

	public static bool BoutonChanger = false;

	void Start()
    {
		rb = GetComponent<Rigidbody>();
		m_MaxHandbrakeTorque = float.MaxValue;
		m_CurrentTorque = m_FullTorqueOverAllWheels - (m_TractionControl * m_FullTorqueOverAllWheels);
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
		if (collision.gameObject.tag == "mur")
		{
			BoutonChanger = true;
		}
	}

	private void Update()
	{
		/*
		if(Input.GetKey(KeyCode.W))
		{
			Move(Input.GetAxis("Horizontal"), 3);
		}
		if(Input.GetKey(KeyCode.S))
		{
			Move(Input.GetAxis("Horizontal"), -1);
		}
		*/
	}

	public void Move(float steering, float accel)
	{
		Move(steering, accel, accel, 0);
	}

	public void Move(float steering, float accel, float footbrake, float handbrake)
	{

		//Obtenir les positions actuel des roues
		Quaternion quat1;
		Vector3 position1;
		Wheel_Collider_FL.GetWorldPose(out position1, out quat1);
		Wheel_Transformation_FL.transform.position = position1;
		Wheel_Transformation_FL.transform.rotation = quat1;

		Quaternion quat2;
		Vector3 position2;
		Wheel_Collider_FR.GetWorldPose(out position2, out quat2);
		Wheel_Transformation_FR.transform.position = position2;
		Wheel_Transformation_FR.transform.rotation = quat2;

		Quaternion quat3;
		Vector3 position3;
		Wheel_Collider_RL.GetWorldPose(out position3, out quat3);
		Wheel_Transformation_RL.transform.position = position3;
		Wheel_Transformation_RL.transform.rotation = quat3;

		Quaternion quat4;
		Vector3 position4;
		Wheel_Collider_RR.GetWorldPose(out position4, out quat4);
		Wheel_Transformation_RR.transform.position = position4;
		Wheel_Transformation_RR.transform.rotation = quat4;

		steering = Mathf.Clamp(steering, -1, 1);
		AccelInput = accel = Mathf.Clamp(accel, 0, 1);
		BrakeInput = footbrake = -1 * Mathf.Clamp(footbrake, -1, 0);
		handbrake = Mathf.Clamp(handbrake, 0, 1);

		m_SteeringAngle = steering * 25;
		Wheel_Collider_FL.steerAngle = m_SteeringAngle;
		Wheel_Collider_FR.steerAngle = m_SteeringAngle;

		SteerHelper();
		ApplyDrive(accel, footbrake);
		CapSpeed();

		if (handbrake > 0f)
		{
			var hbTorque = handbrake * m_MaxHandbrakeTorque;
			Wheel_Collider_RL.brakeTorque = hbTorque;
			Wheel_Collider_RR.brakeTorque = hbTorque;
		}

		CalculateRevs();
		GearChanging();
		AddDownForce();
		TractionControl();
	}

	private void SteerHelper()
	{
		WheelHit wheelhit1;
		Wheel_Collider_FL.GetGroundHit(out wheelhit1);
		if (wheelhit1.normal == Vector3.zero)
			return;

		WheelHit wheelhit2;
		Wheel_Collider_FR.GetGroundHit(out wheelhit2);
		if (wheelhit2.normal == Vector3.zero)
			return;

		WheelHit wheelhit3;
		Wheel_Collider_RL.GetGroundHit(out wheelhit3);
		if (wheelhit3.normal == Vector3.zero)
			return;

		WheelHit wheelhit4;
		Wheel_Collider_RR.GetGroundHit(out wheelhit4);
		if (wheelhit4.normal == Vector3.zero)
			return;

		if (Mathf.Abs(m_OldRotation - transform.eulerAngles.y) < 10f) ;
		{
			var turnadjust = (transform.eulerAngles.y - m_OldRotation) * m_SteerHelper;
			Quaternion velRotation = Quaternion.AngleAxis(turnadjust, Vector3.up);
			rb.velocity = velRotation * rb.velocity;
		}
		m_OldRotation = transform.eulerAngles.y;
	}

	private void ApplyDrive(float accel, float footbrake)
	{
		float thrustTorque;
		thrustTorque = accel * (m_CurrentTorque / 4f);
		Wheel_Collider_FL.motorTorque = thrustTorque;
	}

	private void CapSpeed()
	{
		float speed = rb.velocity.magnitude;

		speed *= 3.6f;
		if(speed>m_Topspeed)
		{
			rb.velocity = (m_Topspeed / 3.6f) * rb.velocity.normalized;
		}
	}

	private void CalculateRevs()
	{
		CalculateGearFactor();
		var gearNumFactor = m_GearNum / (float)NoOfGears;
		var revsRangeMin = ULerp(0f, m_RevRangeBoundary, CurveFactor(gearNumFactor));
		var revsRangeMax = ULerp(m_RevRangeBoundary, 1f, gearNumFactor);
		Revs = ULerp(revsRangeMin, revsRangeMax, m_GearFactor);
	}

	private void CalculateGearFactor()
	{
		float f = (1 / (float)NoOfGears);
		var targetGearFactor = Mathf.InverseLerp(f * m_GearNum, f * (m_GearNum + 1), Mathf.Abs(CurrentSpeed / MaxSpeed));
		m_GearFactor = Mathf.Lerp(m_GearFactor, targetGearFactor, Time.deltaTime * 5f);
	}

	private static float ULerp(float from, float to, float value)
	{
		return (1.0f - value) * from + value * to;
	}

	private static float CurveFactor(float factor)
	{
		return 1 - (1 - factor) * (1 - factor);
	}

	private void GearChanging()
	{
		float f = Mathf.Abs(CurrentSpeed / MaxSpeed);
		float upgearlimit = (1 / (float)NoOfGears) * (m_GearNum + 1);
		float downgearlimit = (1 / (float)NoOfGears) * m_GearNum;

		if (m_GearNum > 0 && f < downgearlimit)
		{
			m_GearNum--;
		}

		if (f > upgearlimit && (m_GearNum < (NoOfGears - 1)))
		{
			m_GearNum++;
		}
	}

	private void AddDownForce()
	{
		Wheel_Collider_FL.attachedRigidbody.AddForce(-transform.up * m_Downforce *
														 Wheel_Collider_FL.attachedRigidbody.velocity.magnitude);

		Wheel_Collider_FR.attachedRigidbody.AddForce(-transform.up * m_Downforce *
														 Wheel_Collider_FR.attachedRigidbody.velocity.magnitude);
	}

	private void TractionControl()
	{
		WheelHit wheelHit;
		Wheel_Collider_FL.GetGroundHit(out wheelHit);
		AdjustTurque(wheelHit.forwardSlip);
		Wheel_Collider_FR.GetGroundHit(out wheelHit);
		AdjustTurque(wheelHit.forwardSlip);
		Wheel_Collider_RL.GetGroundHit(out wheelHit);
		AdjustTurque(wheelHit.forwardSlip);
		Wheel_Collider_RR.GetGroundHit(out wheelHit);
		AdjustTurque(wheelHit.forwardSlip);
	}

	private void AdjustTurque(float forwardSlip)
	{
		if(forwardSlip >= m_SlipLimit && m_CurrentTorque >= 0)
		{
			m_CurrentTorque -= 20 * m_TractionControl;
		}
		else
		{
			m_CurrentTorque += 10 * m_TractionControl;
			if(m_CurrentTorque > m_FullTorqueOverAllWheels)
			{
				m_CurrentTorque = m_FullTorqueOverAllWheels;
			}
		}
	}

	private void HitWall()
	{

	}
}
