using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandClass;
using System.IO;
using UnityEngine;
class CurrentHandStateManager : ExternHandStateManager
{
    StreamReader reader;
    const int KEYPOINTS_NUMBER = 29;
    private int line_num = 0;
    protected BoneEdge[] bones;
    protected int[] bonesParent;
    BoneEdge upBone, rightBone;
    int rootId;
    float ogreScale;
    public CurrentHandStateManager(string handStateFilePath, float ogreScale)
    {
        try
        {
            reader = new StreamReader(handStateFilePath, System.Text.Encoding.Default, false);
        }
        catch (IOException e)
        {
            Console.WriteLine(e.ToString());
            //  System.Environment.Exit(-1);
        }
        this.ogreScale = ogreScale;
        InitBonesAndTree();
        InitBoneMapping();
        initHandState = GetHandStateFromFile();
        currentOffset = new HandOffset();
    }
    public ExternHandState GetHandStateFromFile()
    {
        try
        {
            int n = 0;
            Vector3[] keypoints = new Vector3[KEYPOINTS_NUMBER];
            while (!reader.EndOfStream && n < KEYPOINTS_NUMBER)
            {
                string str = reader.ReadLine();
                line_num += 1;
                string[] split = str.Split(' ');
                if (split.Length != 4)
                {
                    Console.WriteLine("Wrong data form at line: " + line_num);
                    //System.Environment.Exit(-1);
                }
                int pointIndex = Convert.ToInt32(split[0]);
                keypoints[pointIndex] = new Vector3(Convert.ToSingle(split[1]), Convert.ToSingle(split[2]), Convert.ToSingle(split[3]));
                n++;
            }
            Vector3[] boneDirections = new Vector3[bones.Length];
            for (int i = 0; i < bones.Length; i++)
            {
                boneDirections[i] = keypoints[bones[i].end] - keypoints[bones[i].start];
            }
            Vector3 upward = keypoints[upBone.end] - keypoints[upBone.start];
            Vector3 right = keypoints[rightBone.end] - keypoints[rightBone.start];
            Vector3 forward = Vector3.Cross(right , upward);
            Quaternion rootRotation = Quaternion.LookRotation(forward, upward);
            return new ExternHandState(keypoints[rootId], rootRotation, boneDirections);
        }
        catch (IOException e)
        {
            Console.WriteLine(e.ToString());
            // System.Environment.Exit(-1);
        }
        return null;
    }
    public override HandOffset NextHandOffset()
    {
        try
        {
            if (reader.EndOfStream)
            {
                return currentOffset;
            }
            string str = reader.ReadLine();
            line_num += 1;
            camFrame = Convert.ToInt32(str);
            ExternHandState currentHandState = GetHandStateFromFile();
            currentOffset = currentHandState.GetHandOffset( initHandState, this.ogreScale, bonesParent, boneMapping);
        }
        catch (IOException e)
        {
            Console.WriteLine(e.ToString());
            //  System.Environment.Exit(-1);
        }
        return currentOffset;
    }

    protected override void InitBoneMapping()
    {
        boneMapping = HandAvatar.BONE_NAMES;
    }

    protected void InitBonesAndTree()
    {
        bones = new BoneEdge[HandAvatar.BONE_NAMES.Length];
        bonesParent = new int[HandAvatar.BONE_NAMES.Length];
        bones[0] = new BoneEdge(24, 25);
        bones[1] = new BoneEdge(25, 26);
        bones[2] = new BoneEdge(26, 27);
        bonesParent[0] = -1;
        bonesParent[1] = 0;
        bonesParent[2] = 1;

        bones[3] = new BoneEdge(19, 20);
        bones[4] = new BoneEdge(20, 21);
        bones[5] = new BoneEdge(21, 22);
        bonesParent[3] = -1;
        bonesParent[4] = 3;
        bonesParent[5] = 4;

        bones[6] = new BoneEdge(14, 15);
        bones[7] = new BoneEdge(15, 16);
        bones[8] = new BoneEdge(16, 17);
        bonesParent[6] = -1;
        bonesParent[7] = 6;
        bonesParent[8] = 7;


        bones[9] = new BoneEdge(5, 6);
        bones[10] = new BoneEdge(6, 7);
        bones[11] = new BoneEdge(7, 8);
        bonesParent[9] = -1;
        bonesParent[10] = 9;
        bonesParent[11] = 10;

        bones[12] = new BoneEdge(9, 10);
        bones[13] = new BoneEdge(10, 11);
        bones[14] = new BoneEdge(11, 12);
        bonesParent[12] = -1;
        bonesParent[13] = 12;
        bonesParent[14] = 13;

        upBone = new BoneEdge(2, 3);
        rightBone = new BoneEdge(5, 24);
        rootId = 0;
    }
    ~CurrentHandStateManager()
    {
        try
        {
            reader.Close();
        }
        catch (IOException e)
        {
            Console.WriteLine(e.ToString());
            // System.Environment.Exit(-1);
        }
    }
}