using ShipDock.ECS;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace ShipDock.Applications
{
    public class UserInputComponent<S> : ShipDockComponent where S : MainServer
    {

        protected ICommonRole mRoleItem;

        private bool mIsRelaterInited;
        private IRoleData mRoleData;
        private IRoleInput mRoleInput;
        private ServerRelater mRelater;
        private CommonRoleAnimatorInfo mAnimatorInfo;

        public override void Init()
        {
            base.Init();

            mRelater = new ServerRelater()
            {
                ServerNames = RelateServerNames
            };
        }

        public override void Execute(int time, ref IShipDockEntitas target)
        {
            base.Execute(time, ref target);

            mRoleItem = target as ICommonRole;
            mRoleData = mRoleItem.RoleDataSource;
            mRoleInput = mRoleItem.RoleInput;
            mAnimatorInfo = mRoleItem.RoleAnimatorInfo;

            if (mRoleItem.IsUserControlling)
            {
                CheckUserInput();
            }
            if (mRoleInput == default)
            {
                return;
            }
            switch (mRoleInput.RoleMovePhase)
            {
                case UserInputPhases.ROLE_INPUT_PHASE_MOVE_READY:
                    if (mRoleInput.GetMoveValue().magnitude > 1f)
                    {
                        mRoleInput.MoveValueNormalize();
                    }
                    break;
                case UserInputPhases.ROLE_INPUT_PHASE_AMOUT_EXTRAN_TURN:
                    Vector3 move = Vector3.ProjectOnPlane(mRoleInput.GetMoveValue(), mRoleItem.GroundNormal);
                    mRoleInput.SetMoveValue(move);
                    mRoleInput.UpdateAmout(ref mRoleItem);
                    mRoleInput.UpdateRoleExtraTurnRotation(ref mRoleData);
                    mRoleInput.UpdateMovePhase();
                    break;
                case UserInputPhases.ROLE_INPUT_PHASE_SCALE_CAPSULE:
                    mRoleInput.ScaleCapsuleForCrouching(ref mRoleItem, ref mRoleInput);
                    mRoleInput.UpdateMovePhase();
                    break;
            }
        }

        protected T GetMainServer<T>() where T : MainServer
        {
            return mRelater.ServerRef<T>(MainServerName);
        }

        protected virtual void CheckUserInput()
        {
            if (MainInputer == default)
            {
                CheckRelaterInited();
                S server = GetMainServer<S>();
                MainInputer = server.MainInputer;
            }
            float x = GetHorizontal();
            float y = GetVertical();
            Vector3 m = new Vector3(x, y);
            mRoleInput.SetUserInputValue(m);
        }

        protected virtual float GetHorizontal()
        {
            return CrossPlatformInputManager.GetAxis("Horizontal");
        }

        protected virtual float GetVertical()
        {
            return CrossPlatformInputManager.GetAxis("Vertical");
        }

        private void CheckRelaterInited()
        {
            if (!mIsRelaterInited)
            {
                mIsRelaterInited = true;
                mRelater.CommitRelate();
            }
        }

        private IInputer MainInputer { get; set; }
        protected virtual string[] RelateServerNames { get; }
        protected virtual string MainServerName { get; }
    }

}
