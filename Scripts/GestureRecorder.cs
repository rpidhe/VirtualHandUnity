using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class GestureRecorder : MonoBehaviour {
    public string recordFilePath;
    private StreamWriter recoderStream;
    private int curFrame = 0;
    Transform[] keypoints;
	// Use this for initialization
    void writeKeypointsPosition()
    {

        for (int i = 0;i< keypoints.Length;i++)
        {
            string info = Convert.ToString(i) + ' ' + keypoints[i].position.x + ' ' + keypoints[i].position.y + ' ' + keypoints[i].position.z;
            recoderStream.WriteLine(info);
            //recoderStream.WriteLine(Convert.ToString(i + 1)  + keypoints[i].position.ToString());
        }
        recoderStream.Flush();
    }
	void Start () {
        keypoints = GetComponentsInChildren<Transform>();
        recoderStream = new StreamWriter(recordFilePath, false, System.Text.Encoding.Default);
        writeKeypointsPosition();
    }

    // Update is called once per frame
    void Update () {
        recoderStream.WriteLine(curFrame);
        writeKeypointsPosition();
        curFrame++;
    }
    void OnDestory()
    {
        recoderStream.Close();
    }
}
