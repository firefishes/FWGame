using System;

namespace FWGame
{
    public class BananaRole : FWRole
    {
        public BananaRole()
        {
            RoleData data = RoleData.GetRoleDataByRandom();
            data.ConfigID = new Random().Next(0, 3);
            SetRoleData(data);
        }
    }
}

