using ShipDock.Applications;
using ShipDock.Datas;
using System.Collections.Generic;

namespace FWGame
{
    public class RoleChooser : UIModular<UIRoleChooser>, IDataExtracter
    {
        public override void Init()
        {
            base.Init();

            FWGameData gameData = Datas.GetData<FWGameData>(FWConsts.DATA_GAME);
            gameData.Register(this);

            OnDataChanged(gameData, FWConsts.DC_CAMP_ROLE_CREATED);
        }

        public void OnDataChanged(IData data, int keyName)
        {
            switch(keyName)
            {
                case FWConsts.DC_CAMP_ROLE_CREATED:
                    List<CampRoleModel> list = (data as FWGameData).GetCampRoleModels();
                    UI.UpdateRolesUI(ref list);
                    break;
            }
        }

        public override string Name { get; protected set; } = FWConsts.UIM_ROLE_CHOOSER;
        protected override string ABName { get; } = FWConsts.ASSET_UI_ROLE_CHOOSER;
        public override string UIName { get; protected set; } = FWConsts.UI_NAME_ROLE_CHOOSER;
    }

}