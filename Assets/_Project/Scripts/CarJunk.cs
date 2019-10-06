﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CarJunk : MonoBehaviour
{
    private static float JUNK_EJECTION_POWER = 10;
    public GameObject model;
	public float scaleSpeed = 1.0f;
	public float scaleAmount;
	public bool IsCollected { get; set; }

    private Rigidbody rb;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        EjectJunk();
    }
    private void Update()
	{
		model.transform.localScale = Vector3.one * (1f + (scaleAmount * Mathf.Abs(Mathf.Sin(Time.time * scaleSpeed))));
	}

	void OnEnable()
	{
		IsCollected = false;
	}

	public void Collect()
	{
		var pv = this.GetComponent<PhotonView>();
		if (pv.IsMine)
		{
			PhotonNetwork.Destroy(this.gameObject);
		}
		else
		{
			pv.RPC("CollectRpc", RpcTarget.MasterClient); 
		}
	}

	[PunRPC]
	public void CollectRpc()
	{
		PhotonNetwork.Destroy(this.gameObject);
	}

    public void EjectJunk()
    {
        var x = Random.Range(-1, 1);
        var z = Random.Range(-1, 1);
        this.rb.AddForce(new Vector3(x, 1, z) * JUNK_EJECTION_POWER, ForceMode.Impulse);
    }

	void OnTriggerEnter(Collider collider)
	{
		/*var pv = this.GetComponent<PhotonView>();
		if(pv.IsMine)
		{
			var carPhoton = collider.gameObject.GetComponentInParent<PlayerManagerCarPhoton>();
			if (carPhoton)
			{
				Debug.Log("Car IsMine: " + carPhoton.GetComponent<PhotonView>().IsMine.ToString());
				carPhoton.Junk += 1;
				PhotonNetwork.Destroy(this.gameObject);
			}
		}*/

	}
}
