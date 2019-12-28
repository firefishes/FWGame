
#define G_LOG

using ShipDock.Testers;
using ShipDock.Tools;

namespace FWGame
{
    public class FWTester : Singletons<FWTester>, ITester
    {
        public const int LOG0 = 0;

        public FWTester()
        {
            Tester.Instance.AddTester(this);
            Tester.Instance.AddLogger(this, LOG0, "{0}");
        }

        public string Name { get; set; }
    }
}
