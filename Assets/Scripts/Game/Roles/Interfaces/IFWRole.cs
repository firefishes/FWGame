using ShipDock.ECS;
using UnityEngine;

namespace FWGame
{
    public interface IFWRole : IPathFindable, ICollidableRole, IStatesRole
    {
        void SetRoleData(RoleData data);
        FWRoleInput RoleInput { get; set; }
        FWRoleMustSubgroup RoleMustSubgroup { get; set; }
        IFWRole EnemyMainLockDown { get; set; }
        RoleData RoleDataSource { get; }
        RoleAnimatorInfo RoleAnimatorInfo { get; }
        Vector3 GroundNormal { get; set; }
        int Camp { get; set; }
        bool IsGrounded { get; set; }
        bool IsGroundedAndCrouch { get; set; }
        bool IsUserControlling { get; set; }
    }
}
