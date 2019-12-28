namespace FWGame
{
    public class BananaRole : FWRole
    {
        public BananaRole()
        {
            RoleData data = RoleData.GetRoleDataByRandom();
            SetRoleData(data);
        }
    }
}

