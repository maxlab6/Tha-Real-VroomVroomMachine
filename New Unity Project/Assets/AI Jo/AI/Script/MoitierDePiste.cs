using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoitierDePiste : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		
		if (other.tag == "car")
		{
			tour_comlete.m_finActive = true;
		}
	}
}
