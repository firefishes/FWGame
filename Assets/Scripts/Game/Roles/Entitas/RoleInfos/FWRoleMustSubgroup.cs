using System;
using UnityEngine;

namespace FWGame
{
    [Serializable]
    public struct FWRoleMustSubgroup
    {
        public int roleColliderID;
        public int rigidbodyID;
        public int animatorID;
        public float capsuleHeight;
        public Vector3 capsuleCenter;
        public float origGroundCheckDistance;

        public void Init(ref CapsuleCollider target)
        {
            capsuleHeight = target.height;
            capsuleCenter = target.center;
        }
    }
}


