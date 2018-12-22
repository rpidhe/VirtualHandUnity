using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using HandFile;
namespace HandClass
{
    public class HandOffset
    {
        public Vector3 rootPositionOffset;
        public Vector3 rootRotationOffset;
        public Dictionary<string, Vector3> childrenRotationOffset;
    }
    public class HandManager
    {
        private const int CONFIG_COLUMNS = 7;
        public Transform rootNode;
        private Quaternion rootInitRoation;
        private Vector3 rootInitPosition;
        private Dictionary<string, HandBone> handBones;
        public HandManager(Transform rootNode, Dictionary<string, Transform> handBoneTransforms = null,bool constrained = true, string configFilePath=null)
        {
            this.rootNode = rootNode;
            rootInitPosition = new Vector3(rootNode.localPosition.x, rootNode.localPosition.y, rootNode.localPosition.z);
            rootInitRoation = new Quaternion(rootNode.localRotation.x, rootNode.localRotation.y, rootNode.localRotation.z, rootNode.localRotation.w);
            handBones = new Dictionary<string, HandBone>();
            foreach (KeyValuePair<string,Transform> handBoneTransform in handBoneTransforms  )
            {
                handBones.Add(handBoneTransform.Key, new HandBone(handBoneTransform.Value));
            }
            if (!constrained || configFilePath == null)
                return;
            List<string[]> dt = FileHelper.CsvToDataTable(configFilePath, 1);
            if(dt.Count == 0)
            {
                Console.WriteLine("Given configure file have no item");
               // System.Environment.Exit(-1);
            }
            int index = 0;
            foreach(string []row in dt)
            {
                if(row.Length != CONFIG_COLUMNS)
                {
                    Console.WriteLine("Wrong configure file column number");
                 //   System.Environment.Exit(-1);
                }
                String name = row[0];
                if(!handBones.ContainsKey(name))
                {
                    Console.WriteLine("Wrong bone name: " + name + " in file" + configFilePath);
                 //   System.Environment.Exit(-1);
                }
                //Transform boneTransform = rootNode.Find(name);
                float minEularX = Convert.ToSingle(row[1]),
                maxEularX = Convert.ToSingle(row[2]),
                minEularY = Convert.ToSingle(row[3]),
                maxEularY = Convert.ToSingle(row[4]),
                minEularZ = Convert.ToSingle(row[5]),
                maxEularZ = Convert.ToSingle(row[6]);
                handBones[name].SetClamps(maxEularX, maxEularY, maxEularZ, minEularX, minEularY, minEularZ);
               // handBones.Add(name, new HandBone(boneTransform, maxEularX, maxEularY, maxEularZ, minEularX, minEularY, minEularZ));
                index++;
            }
        }
        public void Reset()
        {
            rootNode.localPosition = rootInitPosition;
            rootNode.localRotation = rootInitRoation;
            foreach(HandBone handBone in handBones.Values)
            {
                handBone.ResetLocalRotation();
            }
        }
        /***
        * if handState is null, return false
        */
        public bool SetHandState(HandOffset handOffset)
        {
            if(handOffset == null)
            {
                return false;
            }
            this.Reset();
            rootNode.localPosition += handOffset.rootPositionOffset;
            rootNode.Rotate(handOffset.rootRotationOffset);
            foreach (KeyValuePair<string, Vector3> childRotationOffse in handOffset.childrenRotationOffset)
            {
                if (!handBones.ContainsKey(childRotationOffse.Key))
                {
                    Console.WriteLine("Wrong bone name: " + childRotationOffse.Key +"in input hand state!");
                    continue;
                }
                handBones[childRotationOffse.Key].Rotate(childRotationOffse.Value);
            }
            return true;
        }
    }
}
