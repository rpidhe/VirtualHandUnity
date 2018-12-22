using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using UnityEditor;
namespace HandClass
{
    [System.Serializable]
    public class HandAvatar
    {
        public Transform ThumbProximal;
        public Transform ThumbIntermediate;
        public Transform ThumbDistal;
        public Transform IndexProximal;
        public Transform IndexIntermediate;
        public Transform IndexDistal;
        public Transform MiddleProximal;
        public Transform MiddleIntermediate;
        public Transform MiddleDistal;
        public Transform RingProximal;
        public Transform RingIntermediate;
        public Transform RingDistal;
        public Transform LittleProximal;
        public Transform LittleIntermediate;
        public Transform LittleDistal;
        public static string[] BONE_NAMES =
        {
            "LittleProximal",
            "LittleIntermediate",
            "LittleDistal",

            "RingProximal",
            "RingIntermediate",
            "RingDistal",

            "MiddleProximal",
            "MiddleIntermediate",
            "MiddleDistal",

            "IndexProximal",
            "IndexIntermediate",
            "IndexDistal",

            "ThumbProximal",
            "ThumbIntermediate",
            "ThumbDistal",
        };
        public Dictionary<string,Transform> ToDictionary()
        {
            Dictionary<string, Transform> handTransforms = new Dictionary<string, Transform>();

            handTransforms.Add("ThumbProximal", ThumbProximal);
            handTransforms.Add("ThumbIntermediate", ThumbProximal);
            handTransforms.Add("ThumbDistal", ThumbProximal);

            handTransforms.Add("IndexProximal", IndexProximal);
            handTransforms.Add("IndexIntermediate", IndexIntermediate);
            handTransforms.Add("IndexDistal", IndexDistal);

            handTransforms.Add("MiddleProximal", MiddleProximal);
            handTransforms.Add("MiddleIntermediate", MiddleIntermediate);
            handTransforms.Add("MiddleDistal", MiddleDistal);

            handTransforms.Add("RingProximal", RingProximal);
            handTransforms.Add("RingIntermediate", RingIntermediate);
            handTransforms.Add("RingDistal", RingDistal);

            handTransforms.Add("LittleProximal", LittleProximal);
            handTransforms.Add("LittleIntermediate", LittleIntermediate);
            handTransforms.Add("LittleDistal", LittleDistal);
            return handTransforms;
        }
    }
    public class HandBone
    {
        private Quaternion initLocalRotation;
        private Transform transform;
        private float minEularX,maxEularX, minEularY, maxEularY, minEularZ, maxEularZ;
        public HandBone(Transform transform, 
            float maxEularX = 90.0f, float maxEularY = 90.0f, float maxEularZ = 90.0f, 
            float minEularX = -90.0f, float minEularY = -90.0f, float minEularZ = -90.0f)
        {
            initLocalRotation = new Quaternion(
            transform.localRotation.x,
            transform.localRotation.y,
            transform.localRotation.z,
            transform.localRotation.w
            );
            this.transform = transform;
            this.minEularX = minEularX;
            this.maxEularX = maxEularX;
            this.minEularY = minEularY;
            this.maxEularY = maxEularY;
            this.minEularZ = minEularZ;
            this.maxEularZ = maxEularZ;
        }
        public void SetClamps(float maxEularX = 90.0f, float maxEularY = 90.0f, float maxEularZ = 90.0f,
            float minEularX = -90.0f, float minEularY = -90.0f, float minEularZ = -90.0f)
        {
            this.minEularX = minEularX;
            this.maxEularX = maxEularX;
            this.minEularY = minEularY;
            this.maxEularY = maxEularY;
            this.minEularZ = minEularZ;
            this.maxEularZ = maxEularZ;
        }
        public void ResetLocalRotation()
        {
            transform.localRotation = initLocalRotation;
        }
        public void Rotate(float x,float y,float z)
        {
            if (x > 180) x -= 360;
            if (y > 180) y -= 360;
            if (z > 180) z -= 360;
            x = Mathf.Clamp(x,minEularX,maxEularX);
            y = Mathf.Clamp(y,minEularY,maxEularY);
            z = Mathf.Clamp(z,minEularZ,maxEularZ);
            //transform.rotation = Quaternion.Euler(x, y, z)* transform.rotation;
            transform.Rotate(x, y, z);
        }

        public void Rotate(Vector3 rotationOffset)
        {
            this.Rotate(rotationOffset.x, rotationOffset.y, rotationOffset.z);
        }

        public void Rotate(Quaternion rotationOffset)
        {
            transform.localRotation = rotationOffset * transform.localRotation;
        }
    }
}
