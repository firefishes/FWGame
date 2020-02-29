using ShipDock.Applications;
using System;

namespace FWGame
{
    [Serializable]
    public struct FWRoleData : IRoleData
    {
        public static FWRoleData GetRoleDataByRandom()
        {
            FWRoleData result = new FWRoleData
            {
                Hp = new Random().Next(50, 100),
                FreshKeeping = new Random().Next(10, 50),
                Solid = new Random().Next(5, 70),
                Power = new Random().Next(15, 70),
                Heatproof = new Random().Next(5, 50),
                Soak = new Random().Next(0, 50),
                Shell = new Random().Next(0, 50),
                Skin = new Random().Next(10, 80),
                Speed = new Random(10).Next(1, 50) * 0.1f,
                Size = new Random().Next(1, 5),
                MovingTurnSpeed = 360,
                StationaryTurnSpeed = 180,
                GravityMultiplier = 2f,
                JumpPower = 12f
            };
            return result;
        }
        
        public float Hp { get; set; }
        public float FreshKeeping { get; set; }
        public float Solid { get; set; }
        public float Power { get; set; }
        public float Heatproof { get; set; }
        public float Soak { get; set; }
        public float Shell { get; set; }
        public float Skin { get; set; }
        public float Speed { get; set; }
        public int Size { get; set; }
        public float MovingTurnSpeed { get; set; }
        public float StationaryTurnSpeed { get; set; }
        public float GravityMultiplier { get; set; }
        public float JumpPower { get; set; }
        public int ConfigID { get; set; }
    }
}