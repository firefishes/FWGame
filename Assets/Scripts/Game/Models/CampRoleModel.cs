
namespace FWGame
{

    public class CampRoleModel
    {
        public int controllIndex;
        public IFWRole role;

        public int GetRoleConfigID()
        {
            return role.RoleDataSource.ConfigID;
        }

        public void SetUserControll(bool flag)
        {
            role.IsUserControlling = flag;
        }
    }
}

