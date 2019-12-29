using ShipDock.Notices;

namespace ShipDock.Datas
{
    public interface IData
    {
        int DataName { get; }
    }

    public interface IDataHandler
    {
        void OnDataChanged(IData data, int keyName);
    }
}
