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

        public void SetSourceID(int id)
        {
            SourceID = id;
        }

        protected override int[] ComponentIDs { get; } = new int[]
        {
            FWConsts.COMPONENT_ROLE_INPUT,
            FWConsts.COMPONENT_ROLE_CAMP,
            FWConsts.COMPONENT_POSITION,
            FWConsts.COMPONENT_ROLE_COLLIDER,
            FWConsts.COMPONENT_ROLE_MUST
        };

        public bool Gravity { get; set; }
        public bool IsGrounded { get; set; }
        public bool IsGroundedAndCrouch { get; set; }
        public bool IsUserControlling { get; set; }
        public bool PositionEnabled { get; set; } = true;
        public bool FindngPath { get; set; }
        public int Camp { get; set; }
        public int SourceID { get; private set; }
        public int[] States { get; private set; }
        public float Speed { get; set; }
        public float SpeedCurrent { get; set; }
        public Vector3 Direction { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 PostionTarget { get; set; }
        public Vector3 GroundNormal { get; set; }
        public Vector3 PatherTargetPosition { get; private set; }
        public List<int> CollidingRoles { get; } = new List<int>();
        public IFWRole EnemyMainLockDown { get; set; }
        public RoleData RoleDataSource { get; private set; }
        public FWRoleMustSubgroup RoleMustSubgroup { get; set; }
        public FWRoleInput RoleInput { get; set; }
        public RoleAnimatorInfo RoleAnimatorInfo { get; private set; }
    }

}


