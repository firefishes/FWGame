namespace FWGame
{
    public class BananaRole : FWRole
    {
        public BananaRole()
        {
            FWRoleData data = FWRoleData.GetRoleDataByRandom();
            data.ConfigID = 0;//new Random().Next(0, 3);
            SetRoleData(data);
        }
    }
}

