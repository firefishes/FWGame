using System;

namespace FWGame
{
    public class BananaRole : FWRole
    {
        public BananaRole()
        {
            RoleData data = RoleData.GetRoleDataByRandom();
            data.ConfigID = 0;//new Random().Next(0, 3);
            SetRoleData(data);
        }
    }
}

