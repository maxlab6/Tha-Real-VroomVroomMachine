using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
	public static int m_CPoint = 0;

	public void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("car"))
		{
			m_CPoint += 1;
		}
	}
}
