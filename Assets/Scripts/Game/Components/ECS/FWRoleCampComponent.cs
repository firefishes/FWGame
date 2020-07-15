using ShipDock.Applications;

namespace FWGame
{
    public class FWRoleCampComponent : RoleCampComponent
    {
        public override string DataServerName { get; } = FWConsts.SERVER_FW_DATAS;
        public override string AddCampRoleResovlerName { get; } = "AddCampRole";
        public override string CampRoleCreatedAlias { get; } = "CampRoleCreated";

        protected override void AfterAITargetEnemyCheck()
        {
        }

        protected override void BeforeAITargetEnemyCheck()
        {
        }
    }
}
