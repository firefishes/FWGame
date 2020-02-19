using System;
using ShipDock.ECS;
using UnityEngine;

namespace FWGame
{
    public class FWPositionComponent : ShipDockComponent
    {
        private float mDistance;
        private IFWRole mRole;

        public override void Execute(int time, ref IShipDockEntitas target)
        {
            base.Execute(time, ref target);

            mRole = target as IFWRole;

            if (mRole.PositionEnabled)
            {
                if (mRole.EnemyMainLockDown != default)
                {
                    mDistance = mRole.GetDistFromMainLockDown();
                    if (ShouldStop())
                    {
                        mRole.FindngPath = false;
                        mRole.SpeedCurrent = 0;
                    }
                    else if (ShouldMove())
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
            return 3f;
        }

        private bool ShouldMove()
        {
            return mDistance > GetTraceDistance();
        }

        private bool ShouldStop()
        {
            return mDistance <= GetStopDistance();
        }

        public override void Dispose()
        {
            base.Dispose();

            mRole = default;
        }
    }

}
