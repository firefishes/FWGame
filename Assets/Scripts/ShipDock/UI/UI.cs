using System;
using System.Collections;
using System.Collections.Generic;
using ShipDock.Notices;
using UnityEngine;

namespace ShipDock.UI
{
    public class UI : MonoBehaviour
    {
        protected virtual void Awake()
        {
            int id = gameObject.GetInstanceID();
            id.Add(OnUIReady);
        }

        protected virtual void Start()
        {
            
        }

        protected void OnDestroy()
        {
            Purge();
        }

        protected virtual void Purge()
        {
            int id = gameObject.GetInstanceID();
            id.Remove(OnUIReady);
        }

        private void OnUIReady(INoticeBase<int> param)
        {
            int id = gameObject.GetInstanceID();
            id.Remove(OnUIReady);

            IParamNotice<MonoBehaviour> notice = param as IParamNotice<MonoBehaviour>;
            notice.ParamValue = this;
        }
    }
}

