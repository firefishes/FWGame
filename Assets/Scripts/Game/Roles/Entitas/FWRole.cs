using System;
using System.Collections.Generic;
using UnityEngine;

namespace FWGame
{
    public class FWRole : FWEntitas, IFWRole
    {
        public override void InitComponents()
        {
            base.InitComponents();

            RoleAnimatorInfo = new RoleAnimatorInfo();
            RoleInput = new FWRoleInput();
        }

        public void SetRoleData(RoleData data)
        {
            RoleDataSource = data;
            Speed = RoleDataSource.Speed;
        }

        public void SetPahterTarget(Vector3 value)
        {
            PatherTargetPosition = value;
        }

        protected override int[] ComponentIDs { get; } = new int[]
        {
            FWConsts.COMPONENT_ROLE_INPUT,
            FWConsts.COMPONENT_ROLE_CAMP,
            FWConsts.COMPONENT_ROLE_INPUT,
            FWConsts.COMPONENT_POSITION,
            FWConsts.COMPONENT_ROLE_COLLIDER,
            FWConsts.COMPONENT_ROLE_MUST
        };

        public bool Gravity { get; set; }
        public bool PositionEnabled { get; set; } = true;
        public float Speed { get; set; }
        public float SpeedCurrent { get; set; }
        public Vector3 Direction { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 PostionTarget { get; set; }
        public RoleData RoleDataSource { get; private set; }
        public List<int> CollidingRoles { get; } = new List<int>();
        public Vector3 PatherTargetPosition { get; private set; }
        public bool FindngPath { get; set; }
        public int Camp { get; set; }
        public IFWRole EnemyMainLockDown { get; set; }
        public int[] States { get; private set; }
        public FWRoleMustSubgroup RoleMustSubgroup { get; set; }
        public FWRoleInput RoleInput { get; set; }
        public Vector3 GroundNormal { get; set; }
        public bool IsGrounded { get; set; }
        public bool IsGroundedAndCrouch { get; set; }
        public RoleAnimatorInfo RoleAnimatorInfo { get; private set; }
        public bool IsUserControlling { get; set; }
    }

    public class RoleAnimatorInfo
    {
        public bool IsNameGrounded { get; set; }
        public float RunCycleLegOffset { get; set; } = 0.2f;
        public float AnimSpeedMultiplier { get; set; } = 1.0f;
        public float MoveSpeedMultiplier { get; set; } = 1.0f;
    }
    
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

    [Serializable]
    public class FWRoleInput
    {
        public float deltaTime;
        public Vector3 move;
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


