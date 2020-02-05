using ShipDock.Applications;
using ShipDock.ECS;

namespace FWGame
{
    public class FWEntitas : ShipDockEntitas
    {
        public override void InitComponents()
        {
            base.InitComponents();

            IShipDockComponent component;
            var manager = ShipDockApp.Instance.Components;
            int aid;
            int max = ComponentIDs != default ? ComponentIDs.Length : 0;
            for (int i = 0; i < max; i++)
            {
                aid = ComponentIDs[i];
                component = manager.GetComponentByAID(aid);
                AddComponent(component);
            }
        }

        protected virtual int[] ComponentIDs { get; }
    }
}
