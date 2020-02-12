using System;
using System.Collections;
using System.Collections.Generic;
using ShipDock.Applications;
using ShipDock.Notices;
using ShipDock.Server;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace FWGame
{
    public class FWInputer : MonoBehaviour
    {
        [SerializeField]
        private TouchPad m_Mover;
        [SerializeField]
        private TouchPad m_TurnAndLooker;

        private ComponentBridge mCompBrigde;

        private void Awake()
        {
            mCompBrigde = new ComponentBridge(OnInited);
            mCompBrigde.Start();
        }

        private void OnInited()
        {
            mCompBrigde.Dispose();
            mCompBrigde = default;

            FWConsts.SERVER_FW_DATAS.DeliveParam<FWDataServer, FWInputer>("SetFWInputer", "SetFWInputerParamer", OnSetFWInputer);
        }

        [Resolvable("SetFWInputerParamer")]
        private void OnSetFWInputer(ref IParamNotice<FWInputer> target)
        {
            target.ParamValue = this;
        }

        private void OnGetInputer(INoticeBase<int> param)
        {
            IParamNotice<FWInputer> notice = param as IParamNotice<FWInputer>;
            notice.ParamValue = this;
        }

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
    }

}