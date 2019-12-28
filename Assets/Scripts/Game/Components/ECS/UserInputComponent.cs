using ShipDock.ECS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FWGame
{
    public class UserInputComponent : ShipDockComponent
    {
        public const int ROLE_INPUT_PHASE_MOVE_READY = 0;
        public const int ROLE_INPUT_PHASE_AMOUT_EXTRAN_TURN = 1;
        public const int ROLE_INPUT_PHASE_CHECK_GROUNDE = 2;
        public const int ROLE_INPUT_PHASE_SCALE_CAPSULE = 3;
        public const int ROLE_INPUT_PHASE_CHECK_CROUCH = 4;

        private IFWRole mRoleItem;
        private RoleData mRoleData;
        private RoleAnimatorInfo mAnimatorInfo;
        private FWRoleInput mRoleInput;

        public override void Execute(int time, ref IShipDockEntitas target)
        {
            base.Execute(time, ref target);

            mRoleItem = target as IFWRole;
            mRoleData = mRoleItem.RoleDataSource;
            mRoleInput = mRoleItem.RoleInput;
            mAnimatorInfo = mRoleItem.RoleAnimatorInfo;

            if(mRoleItem.IsUserControlling)
            {
                CheckUserInput();
            }
            
            switch(mRoleInput.RoleMovePhase)
            {
                case ROLE_INPUT_PHASE_MOVE_READY:
                    if (mRoleInput.move.magnitude > 1f)
                    {
                        mRoleInput.move.Normalize();
                    }
                    break;
                case ROLE_INPUT_PHASE_AMOUT_EXTRAN_TURN:
                    mRoleInput.move = Vector3.ProjectOnPlane(mRoleInput.move, mRoleItem.GroundNormal);
                    mRoleInput.UpdateAmout(ref mRoleItem);
                    mRoleInput.UpdateRoleExtraTurnRotation(ref mRoleData);
                    mRoleInput.UpdateMovePhase();
                    break;
                case ROLE_INPUT_PHASE_SCALE_CAPSULE:
                    mRoleInput.ScaleCapsuleForCrouching(ref mRoleItem, ref mRoleInput);
                    mRoleInput.UpdateMovePhase();
                    break;
            }
        }

        private void CheckUserInput()
        {

        }
    }

}
