﻿using ShipDock.Applications;
using ShipDock.Notices;
using ShipDock.Server;

namespace FWGame
{
    public static class FWConsts
    {
        public const string SERVER_FW = "ServerFW";
        public const string SERVER_FW_COMPONENTS = "ServerFWComponents";
        public const string SERVER_FW_DATAS = "ServerFWDatas";
        public const string SERVER_FW_LENS = "ServerFWLens";

        public const int COMPONENT_POSITION = 1;
        public const int COMPONENT_ROLE_NORMAL_ENTER_SCENE = 2;
        public const int COMPONENT_ROLE_COLLIDER = 3;
        public const int COMPONENT_ROLE_CAMP = 4;
        public const int COMPONENT_ROLE_MUST = 5;
        public const int COMPONENT_ROLE_INPUT = 6;
        public const int COMPONENT_ROLE_CONTROLLABLE = 7;

        public const int NOTICE_GET_ROLE_WHEN_CREATE = 1000;
        public const int NOTICE_GET_GAME_INPUTER = 1001;
        public const int NOTICE_PLAYER_ROLE_CHOOSEN = 1002;

        public const int DATA_GAME = 1;
        public const int DATA_PLAYER = 2;

        public const int DC_CAMP_ROLE_CREATED = 2000;
        public const int DC_PLAYER_ROLE_CHOOSEN = 2001;

        public const string ASSET_RES_DATA = "res_data/res_data";
        public const string ASSET_RES_BRIGEDS = "res_brigdes";
        public const string ASSET_BANANA_ROLE = "roles/banana_role";
        public const string ASSET_UI_ROLE_CHOOSER = "ui/ui_role_chooser";
        public const string ASSET_UI_MAIN = "ui/ui_main";

        public const string UIM_ROLE_CHOOSER = "RoleChooser";
        //public const string UIM_DUAL_TOUCH_CONTROLS = "DualTouchControls";

        public const string UI_NAME_ROLE_CHOOSER = "UIRoleChooser";
        //public const string UI_NAME_DUAL_TOUCH_CONTROLS = "DualTouchControls";

        public const int POOL_UI_ROLE_CARD = 0;

        private static readonly IResolvableConfig[] FWServerConfigs =
        {
            new ResolvableConfigItem<INotice, GameNotice>("GameNotice"),
            new ResolvableConfigItem<IParamNotice<FWRoleComponent>, ParamNotice<FWRoleComponent>>("PlayerRoleChoosen"),
            new ResolvableConfigItem<IParamNotice<ICommonRole>, CampRoleNotice>("CampRoleCreated"),
            new ResolvableConfigItem<IParamNotice<ICommonRole>, ParamNotice<ICommonRole>>("SetUserFWRole"),
            new ResolvableConfigItem<IParamNotice<FWCamerLens>, ParamNotice<FWCamerLens>>("SetLensParamer"),
        };

        public static readonly IResolvableConfig[] ServerConfigs = MainServer.ServerConfigs.ContactToArr(FWServerConfigs);
    }
}
