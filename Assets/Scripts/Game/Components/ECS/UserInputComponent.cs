using ShipDock.Applications;
using ShipDock.ECS;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace FWGame
{
    public class UserInputComponent : ShipDockComponent
    {
        public const int ROLE_INPUT_PHASE_MOVE_READY = 0;
        public const int ROLE_INPUT_PHASE_AMOUT_EXTRAN_TURN = 1;
        public const int ROLE_INPUT_PHASE_CHECK_GROUNDE = 2;
        public const int ROLE_INPUT_PHASE_SCALE_CAPSULE = 3;
        public const int ROLE_INPUT_PHASE_CHECK_CROUCH = 4;

        private bool mIsRelaterInited;
        private IFWRole mRoleItem;
        private RoleData mRoleData;
        private RoleAnimatorInfo mAnimatorInfo;
        private FWRoleInput mRoleInput;
        private ServerRelater mRelater;

        public override void Init()
        {
            base.Init();

            mRelater = new ServerRelater()
            {
                ServerNames = new string[]
                {
                    FWConsts.SERVER_FW,
                    FWConsts.SERVER_FW_LENS
                }
            };
        }

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
            if(mRoleInput == default)
            {
                return;
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
            if (MainInputer == default)
            {
                CheckRelaterInited();
                FWServer server = mRelater.ServerRef<FWServer>(FWConsts.SERVER_FW);
                MainInputer = server.MainInputer;
            }
            float x = CrossPlatformInputManager.GetAxis("Horizontal");
            float y = CrossPlatformInputManager.GetAxis("Vertical");
            Vector3 m = new Vector3(x, y);
            mRoleItem.EnemyMainLockDown = default;
            mRoleInput.userInput = m;
        }

        private void CheckRelaterInited()
        {
            if (!mIsRelaterInited)
            {
                mIsRelaterInited = true;
                mRelater.CommitRelate();
            }
        }
        
        private FWInputer MainInputer { get; set; }
    }

}
