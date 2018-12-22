using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HandClass;
public class HandDirection : MonoBehaviour {

    public float roateSpeed = 0.1f;
    HandBone handbone;
    Transform boneTransform;
    // Use this for initialization
    void Start () {
        boneTransform = transform.Find("metarig/forearm/hand/palm.04/f_pinky.01");
        //Debug.Log(boneTransform);
        handbone = new HandBone(boneTransform);
       // handbone.Roate(90, 0, 0f);
    }
	// Update is called once per frame
	void Update () {
        float theta = roateSpeed * Time.time;
        Vector3 foreward = new Vector3(Mathf.Cos(theta), 0.0f, Mathf.Sin(theta));
        transform.rotation = Quaternion.LookRotation(foreward, transform.up);
        handbone.ResetLocalRotation();
        boneTransform.localRotation = Quaternion.Euler(50 * Time.time, 0, 0) * boneTransform.localRotation;
        //handbone.Rotate(50*Time.time, 0, 0);
        //if (Time.time > 3)
        //{
        //    handbone.resetRotation();
        //}

    }
}
