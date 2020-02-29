using ShipDock.Applications;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace FWGame
{
    public class TouchPadsInputer : InputerComponent
    {
        [SerializeField]
        private TouchPad m_Mover;
        [SerializeField]
        private TouchPad m_TurnAndLooker;
        [SerializeField]
        private MobileControlRig m_MobileControlRig;
        
        public TouchPad Mover
        {
            get
            {
                return m_Mover;
            }
        }

        public TouchPad TurnAndLooker
        {
            get
            {
                return m_TurnAndLooker;
            }
        }
        
        public MobileControlRig MobileControlRig
        {
            get
            {
                return m_MobileControlRig;
            }
        }

        protected override string MainServerdName { get; } = FWConsts.SERVER_FW;
    }

}