using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MLAgents;

public class Road : MonoBehaviour
{
	public CarAgent carAgent;
	public TextMeshPro cumulativeReward;
	public float x;
	public float y;
	public float z;
	public float rotation;

	private Road road;
	new private Rigidbody rigidbody;

	public void ResetArea()
	{
		ResetLaps();
		PlaceCar();
		
	}

	public int TourActu()
	{
		return tour_comlete.tours;
	}

	private void ResetLaps()
	{
		tour_comlete.tours = 0;
	}

	public static Vector3 ChoosePosition(Vector3 centre)
	{
		
		return centre;
	}

	private void PlaceCar()
	{
		Rigidbody rigidbody = carAgent.GetComponent<Rigidbody>();
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;
		carAgent.transform.position = new Vector3(x, y, z);
		carAgent.transform.position = ChoosePosition(transform.position) + Vector3.right * x + Vector3.up * y + Vector3.forward * z;
		carAgent.transform.rotation = Quaternion.Euler(0f, rotation, 0f);
	}

	private void Start()
	{
		ResetArea();
	}

	private void Update()
	{
		//cumulativeReward.text = carAgent.GetCumulativeReward().ToString("0.00");
	}
}
