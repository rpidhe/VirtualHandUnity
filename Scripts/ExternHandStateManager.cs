using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace HandClass
{
    public class ExternHandState
    {
        private Vector3 rootPosition;
        private Quaternion rootRotation;
        private Vector3[] boneDirections;
        private Vector3[] localBoneDirections;
        private bool local;
        public ExternHandState(Vector3 rootPosition,Quaternion rootRotation, Vector3[] boneDirections)
        {
            this.rootPosition = rootPosition;
            this.rootRotation = rootRotation;
            this.boneDirections = boneDirections;
            InitLocal();
        }
        protected void InitLocal()
        {
            localBoneDirections = new Vector3[boneDirections.Length];
            Quaternion rootRotationInv = Quaternion.Inverse(rootRotation);
            for (int i = 0;i< boneDirections.Length;i++)
            {
                localBoneDirections[i] = rootRotationInv * boneDirections[i];
            }
        }
        public HandOffset GetHandOffset(ExternHandState other,float scale,int[] bonesParent, string[] boneMapping)
        {
            HandOffset handOffset = new HandOffset();
            handOffset.rootPositionOffset = (this.rootPosition - other.rootPosition) * scale;

            handOffset.rootRotationOffset = (rootRotation * Quaternion.Inverse(other.rootRotation)).eulerAngles;
            //handOffset.rootRotationOffset = (Quaternion.LookRotation(forward, up) * Quaternion.Inverse(Quaternion.LookRotation(other.forward,other.up))).eulerAngles;

            Dictionary<string, Vector3> childrenRotationOffset = new Dictionary<string, Vector3>();
            Quaternion[]offsets = new Quaternion[localBoneDirections.Length];
            for (int i = 0; i < localBoneDirections.Length; i++)
            {
                offsets[i] = Quaternion.FromToRotation(other.localBoneDirections[i], this.localBoneDirections[i]);
            }
            for( int i = 0;i < localBoneDirections.Length; i++)
            {
                Quaternion relativeOffset;
                if(bonesParent[i] == -1)
                {
                    relativeOffset = offsets[i];
                }
                else
                {
                    relativeOffset = offsets[i] * Quaternion.Inverse(offsets[bonesParent[i]]);
                }
                //childrenRotationOffset.Add(boneMapping[i],  relativeOffset.eulerAngles);
                childrenRotationOffset.Add(boneMapping[i], (relativeOffset* other.rootRotation).eulerAngles);
            }
            handOffset.childrenRotationOffset = childrenRotationOffset;
            return handOffset;
        }
    }
    public class BoneEdge
    {
        public int start;
        public int end;
        public BoneEdge(int start, int end)
        {
            this.start = start;
            this.end = end;
        }
    }

    public abstract class ExternHandStateManager
    {
        protected string[] boneMapping;
        protected ExternHandState initHandState;
        protected HandOffset currentOffset;
        public int camFrame;
        protected abstract void InitBoneMapping();
        public abstract HandOffset NextHandOffset();
        public virtual HandOffset NextHandOffsetSync(int engineFrame)
        {
            if(engineFrame < camFrame)
            {
                return currentOffset;
            }
            else 
            {
                return NextHandOffset();
            }
        }
    }
}
