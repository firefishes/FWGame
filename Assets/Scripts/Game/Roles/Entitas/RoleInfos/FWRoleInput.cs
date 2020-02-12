using System;
using UnityEngine;

namespace FWGame
{
    [Serializable]
    public class FWRoleInput
    {
        public float deltaTime;
        public Vector3 move;
        public Vector3 userInput;
        public bool crouch;
        public bool jump;
        public bool crouching;
        public float turnSpeed;

        public void UpdateAmout(ref IFWRole roleEntitas)
        {
            move = Vector3.ProjectOnPlane(move, roleEntitas.GroundNormal);
            TurnAmount = Mathf.Atan2(move.x, move.z);
            ForwardAmount = move.z;
        }

        public float UpdateRoleExtraTurnRotation(ref RoleData roleData)
        {
            turnSpeed = Mathf.Lerp(roleData.StationaryTurnSpeed, roleData.MovingTurnSpeed, ForwardAmount);
            ExtraTurnRotationOut = TurnAmount * turnSpeed * deltaTime;
            return ExtraTurnRotationOut;
        }

        public void HandleAirborneMovement(ref RoleData roleData)
        {
            ExtraGravityForceOut = (Physics.gravity * roleData.GravityMultiplier) - Physics.gravity;
        }

        public bool HandleGroundedMovement(ref FWRoleInput input, ref RoleAnimatorInfo animatorInfo)
        {
            // check whether conditions are right to allow a jump:
            bool isNameGrounded = animatorInfo.IsNameGrounded;
            bool result = input.jump && !input.crouch && isNameGrounded;
            return result;
        }

        public void ScaleCapsuleForCrouching(ref IFWRole roleEntitas, ref FWRoleInput roleInput)
        {
            roleEntitas.IsGroundedAndCrouch = roleEntitas.IsGrounded && roleInput.crouch;
            if (roleEntitas.IsGroundedAndCrouch)
            {
                if (roleInput.crouching)
                {
                    return;
                }
                FWRoleMustSubgroup subgroup = roleEntitas.RoleMustSubgroup;
                subgroup.capsuleHeight /= 2f;
                subgroup.capsuleCenter /= 2f;
                roleEntitas.RoleMustSubgroup = subgroup;
            }
        }

        public void SetMoveValue(Vector3 value)
        {
            move = value;
        }

        public void UpdateMovePhase()
        {
            RoleMovePhase++;
            if(RoleMovePhase >= 5)
            {
                RoleMovePhase = 0;
            }
        }

        public void ResetMovePhase()
        {
            RoleMovePhase = 0;
        }

        public int RoleMovePhase { get; private set; }
        public float TurnAmount { get; private set; }
        public float ForwardAmount { get; private set; }
        public float ExtraTurnRotationOut { get; private set; }
        public Vector3 ExtraGravityForceOut { get; private set; }
    }

}


