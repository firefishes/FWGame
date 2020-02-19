using ShipDock.ECS;
using UnityEngine;

namespace FWGame
{
    public interface IFWRole : IPathFindable, ICollidableRole, IStatesRole
    {
        float GetDistFromMainLockDown();
        void SetRoleData(RoleData data);
        void SetSourceID(int id);
        bool IsGrounded { get; set; }
        bool IsGroundedAndCrouch { get; set; }
        bool IsUserControlling { get; set; }
        int Camp { get; set; }
        int SourceID { get; }
        string Name { get; set; }
        Vector3 GroundNormal { get; set; }
        IFWRole EnemyMainLockDown { get; set; }
        RoleData RoleDataSource { get; }
        FWRoleInput RoleInput { get; set; }
        FWRoleMustSubgroup RoleMustSubgroup { get; set; }
        RoleAnimatorInfo RoleAnimatorInfo { get; }
    }
}
