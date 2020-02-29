using ShipDock.Notices;
using ShipDock.Server;
using UnityEngine;

namespace ShipDock.Applications
{
    public class InputerComponent : MonoBehaviour, IInputer
    {
        private ComponentBridge mCompBrigde;

        private void Awake()
        {
            mCompBrigde = new ComponentBridge(OnInited);
            mCompBrigde.Start();
        }

        protected virtual void OnInited()
        {
            mCompBrigde.Dispose();
            mCompBrigde = default;

            MainServerdName.DeliveParam<MainServer, IInputer>("SetInputer", "SetInputerParamer", OnSetFWInputer);
        }

        [Resolvable("SetInputerParamer")]
        private void OnSetFWInputer(ref IParamNotice<IInputer> target)
        {
            target.ParamValue = this;
        }

        protected virtual string MainServerdName { get; }
    }
}
