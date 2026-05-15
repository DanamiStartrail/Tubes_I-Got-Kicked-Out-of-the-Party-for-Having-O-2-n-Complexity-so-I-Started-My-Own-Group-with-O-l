using System;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

namespace TubesStima
{
    public class PredictiveBot : Bot
    {
        public static void Main(string[] args) => new PredictiveBot().Start();
        public PredictiveBot() : base(BotInfo.FromFile("AltBot2.json")) { }

        public override void Run()
        {
            AdjustGunForBodyTurn = true;
            AdjustRadarForGunTurn = true;
            
            while (IsRunning) TurnRadarRight(360);
        }

        public override void OnScannedBot(ScannedBotEvent e)
        {
            double arahMusuh = DirectionTo(e.X, e.Y);

            // 1. Radar Lock
            double putarRadar = arahMusuh - RadarDirection;
            while (putarRadar > 180) putarRadar -= 360;
            while (putarRadar < -180) putarRadar += 360;
            SetTurnRadarRight(putarRadar * 1.2);

            // 2. Gerakan Terus Mendekati Musuh
            double putarBadan = arahMusuh - Direction;
            while (putarBadan > 180) putarBadan -= 360;
            while (putarBadan < -180) putarBadan += 360;
            SetTurnRight(putarBadan);
            SetForward(DistanceTo(e.X, e.Y) - 100); // Sisakan jarak 100 pixel

            // 3. Tembakan Prediktif
            double dayaTembak = 3.0; // Daya maksimal
            double kecepatanPeluru = 20 - (3 * dayaTembak);
            double waktuTempuh = DistanceTo(e.X, e.Y) / kecepatanPeluru;
            
            double arahRadian = e.Direction * (Math.PI / 180.0);
            double targetX = e.X + (Math.Sin(arahRadian) * e.Speed * waktuTempuh);
            double targetY = e.Y + (Math.Cos(arahRadian) * e.Speed * waktuTempuh);

            double arahTembak = DirectionTo(targetX, targetY);
            double putarMeriam = arahTembak - GunDirection;
            while (putarMeriam > 180) putarMeriam -= 360;
            while (putarMeriam < -180) putarMeriam += 360;
            
            SetTurnGunRight(putarMeriam);

            if (GunHeat == 0) SetFire(dayaTembak);

            Go(); // Eksekusi
        }
    }
}