using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HandClass;
public class HandController : MonoBehaviour {

    public Transform rootNode;
    public HandAvatar handAvatar;
    private HandManager handManager;
    private ExternHandStateManager externHand;
     //unity坐标系与外部坐标系单位   长度之比
    public float scaleFactor = 1.0f;
    private int curFrame = 0;
    //private Transform [] fi
    // Use this for initialization
    void Start () {
        externHand = new CurrentHandStateManager("Assets/Data/OriginModel/thumbup.txt", scaleFactor);
        //externHand = new OgreHandStateManager("G:/Projects/Handpose/VirtualHand/3D/hand_frames.txt", scaleFactor);
        handManager = new HandManager(rootNode, handAvatar.ToDictionary(),false, "Assets/Config/bones.csv");
        handManager.Reset();
    }
	// Update is called once per frame
	void Update () {
        HandOffset handOffset = externHand.NextHandOffsetSync(curFrame);
        //HandOffset handOffset = new HandOffset();
        handManager.SetHandState(handOffset);
        curFrame++;
    }
}
