using ShipDock.Applications;
using UnityEngine;

namespace FWGame
{
    public class EnvironmentComponent : MonoBehaviour
    {

        [SerializeField]
        private Transform m_CameraTarget;

        private ComponentBridge mBridge;
        private CameraDollyTrackUpdater mDollyTrackUpdater;

        private void Awake()
        {
            mBridge = new ComponentBridge(OnEnvironmentStart);
            mBridge.Start();
        }

        private void OnEnvironmentStart()
        {
            Debug.Log("Environment Start");

            mDollyTrackUpdater = new CameraDollyTrackUpdater(ref m_CameraTarget);
            UpdaterNotice.AddUpdater(mDollyTrackUpdater);
            UpdaterNotice.AddSceneUpdater(mDollyTrackUpdater);
        }

        private void OnDestroy()
        {
            mBridge?.Dispose();
            mBridge = null;
        }
    }

    public class CameraDollyTrackUpdater : IUpdate
    {
        private Transform mCameraTarget;

        public CameraDollyTrackUpdater(ref Transform cameraTarget)
        {
            mCameraTarget = cameraTarget;
        }

        public void AddUpdate()
        {
        }

        public void OnFixedUpdate(int dTime)
        {
        }

        public void OnLateUpdate()
        {
        }

        public void OnUpdate(int dTime)
        {
            //mCameraTarget.position = WatchingRolePositin;
        }

        public void RemoveUpdate()
        {
        }

        public bool IsUpdate { get; } = true;
        public bool IsFixedUpdate { get; } = true;
        public bool IsLateUpdate { get; } = true;
        public bool SceneUpdate { get; set; }
        public Vector3 WatchingRolePositin { get; set; }
    }
}

