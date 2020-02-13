using Cinemachine;
using ShipDock.Applications;
using ShipDock.Notices;
using ShipDock.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FWGame
{
    public class FWCamerLens : MonoBehaviour
    {

        [SerializeField]
        private VirtualCameraSubgroup[] m_CameraGroups;
        [SerializeField]
        private GameObject m_CameraFollower;

        private ComponentBridge mCompBridge;
        private KeyValueList<string, VirtualCameraSubgroup> mMapper;

        private void Awake()
        {
            mCompBridge = new ComponentBridge(OnInited);
            mCompBridge.Start();
        }

        private void OnInited()
        {
            mMapper = new KeyValueList<string, VirtualCameraSubgroup>();
            int max = m_CameraGroups.Length;
            for (int i = 0; i < max; i++)
            {
                mMapper[m_CameraGroups[i].Name] = m_CameraGroups[i];
            }

            FWConsts.SERVER_FW_LENS.DeliveParam<FWCamerasServer, FWCamerLens>("SetLens", "SetLensParamer", OnSetLens);
        }

        private void OnSetLens(ref IParamNotice<FWCamerLens> target)
        {
            IParamNotice<FWCamerLens> notice = target as IParamNotice<FWCamerLens>;
            notice.ParamValue = this;
        }

        public CinemachineVirtualCamera GetVirtualCamera(string name)
        {
            return mMapper[name].VirtualCamera;
        }

        public GameObject CameraFollower
        {
            get
            {
                return m_CameraFollower;
            }
        }
    }

    [Serializable]
    public class VirtualCameraSubgroup
    {
        [SerializeField]
        private string m_Name;
        [SerializeField]
        private CinemachineVirtualCamera m_VirturalCamera;

        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        public CinemachineVirtualCamera VirtualCamera
        {
            get
            {
                return m_VirturalCamera;
            }
        }

    }
}
