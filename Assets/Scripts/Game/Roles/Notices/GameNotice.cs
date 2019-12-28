using ShipDock.Notices;

namespace FWGame
{
    public class GameNotice : Notice
    {
        public override int Name
        {
            get
            {
                return base.Name + 222;
            }
        }
    }
}
