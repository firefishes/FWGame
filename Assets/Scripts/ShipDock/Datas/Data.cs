using System;
using System.Collections.Generic;

namespace ShipDock.Datas
{
    public class Data : IData
    {
        private List<IDataHandler> mDataHandlers;
        private Action<IData, int> mOnDataChanged;

        public Data(int dataName)
        {
            DataName = dataName;
            mDataHandlers = new List<IDataHandler>();
        }

        public void DataChanged(params int[] keys)
        {
            int keyName;
            int max = keys.Length;
            for (int i = 0; i < max; i++)
            {
                keyName = keys[i];
                mOnDataChanged?.Invoke(this, keyName);
            }
        }

        public void Register(IDataHandler dataHandler)
        {
            if((dataHandler == default) || mDataHandlers.Contains(dataHandler))
            {
                return;
            }
            mDataHandlers.Add(dataHandler);
            mOnDataChanged += dataHandler.OnDataChanged;
        }

        public void Unregister(IDataHandler dataHandler)
        {
            if((dataHandler == default) || !mDataHandlers.Contains(dataHandler))
            {
                return;
            }
            mDataHandlers.Remove(dataHandler);
            mOnDataChanged -= dataHandler.OnDataChanged;
        }
        
        public int DataName { get; private set; }
    }
}
