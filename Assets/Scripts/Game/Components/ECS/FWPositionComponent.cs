using System;
using ShipDock.ECS;
using UnityEngine;

namespace FWGame
{
    public class FWPositionComponent : ShipDockComponent
    {
        private IFWRole mRole;

        public override void Execute(int time, ref IShipDockEntitas target)
        {
            base.Execute(time, ref target);

            mRole = target as IFWRole;

            if (mRole.PositionEnabled)
            {
                if(mRole.EnemyMainLockDown != default)
                {
                    float direction = Vector3.Distance(mRole.Position, mRole.EnemyMainLockDown.Position);
                    if (direction <= GetStopDistance())
                    {
                        mRole.FindngPath = false;
                        mRole.SpeedCurrent = 0;
                    }
                    else if (direction > GetTraceDistance())
                    {
                        mRole.FindngPath = true;
                        mRole.SpeedCurrent = mRole.Speed;
                    }
                    else
                    {
                        mRole.FindngPath = false;
                        mRole.SpeedCurrent = 0;
                    }
                }
                else
                {
                    mRole.FindngPath = false;
                }
            }
        }

        private float GetStopDistance()
        {
            return 2f;
        }

        private float GetTraceDistance()
        {
            return 5f;
        }

        public override void Dispose()
        {
            base.Dispose();

            mRole = default;
        }
    }

}
