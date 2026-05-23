using System;
using System.Collections.Generic;
using System.Linq;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;
using Robocode.TankRoyale.BotApi.Graphics;

namespace TubesStima
{
    public class MainBot : Bot
    {
        private int lastScannedTick = 0;
        private int strafeDirection = 1;
        private readonly Queue<double> predXHistory = new Queue<double>();
        private readonly Queue<double> predYHistory = new Queue<double>();
        private const int FILTER_SIZE = 3;
        private const double WALL_MARGIN = 80.0;

        public static void Main(string[] args) => new MainBot().Start();

        public override void Run()
        {
            AdjustRadarForGunTurn = true;
            AdjustGunForBodyTurn = true;
            BodyColor   = Color.GhostWhite;
            RadarColor  = Color.NavajoWhite;
            TurretColor = Color.WhiteSmoke;
            GunColor    = Color.LightYellow;
            SetTurnRadarRight(double.PositiveInfinity);

            while (IsRunning)
            {
                if (TurnNumber - lastScannedTick > 3)
                    SetTurnRadarRight(double.PositiveInfinity);
                Go();
            }
        }

        public override void OnScannedBot(ScannedBotEvent e)
        {
            lastScannedTick = TurnNumber;

            double dx              = e.X - X;
            double dy              = e.Y - Y;
            double distance        = Math.Sqrt(dx * dx + dy * dy);
            double absoluteBearing = Math.Atan2(dx, dy) * 180.0 / Math.PI;

            double radarTurn = CalcShortestBearing(RadarDirection, absoluteBearing);
            SetTurnRadarRight(radarTurn + (radarTurn >= 0 ? 3.0 : -3.0));

            double firePower   = distance < 200 ? 3.0 : distance < 400 ? 2.0 : 1.5;
            double bulletSpeed = 20.0 - 3.0 * firePower;
            double travelTime  = distance / bulletSpeed;

            double predX = e.X + Math.Sin(e.Direction * Math.PI / 180.0) * e.Speed * travelTime;
            double predY = e.Y + Math.Cos(e.Direction * Math.PI / 180.0) * e.Speed * travelTime;

            predXHistory.Enqueue(predX);
            predYHistory.Enqueue(predY);
            if (predXHistory.Count > FILTER_SIZE) { predXHistory.Dequeue(); predYHistory.Dequeue(); }

            double gunBearing = Math.Atan2(predXHistory.Average() - X, predYHistory.Average() - Y) * 180.0 / Math.PI;
            double gunTurn    = CalcShortestBearing(GunDirection, gunBearing);
            SetTurnGunRight(gunTurn);

            if (Math.Abs(gunTurn) <= 3.5 && GunHeat == 0)
                SetFire(firePower);

            if (IsNearWall())
            {
                if (Math.Abs(TurnRemaining) < 1.0)    TurnTowardCenter();
                if (Math.Abs(DistanceRemaining) < 1.0) SetForward(60);
                return;
            }

            double strafeAngle = absoluteBearing + 90.0 * strafeDirection;
            double bodyTurn    = CalcShortestBearing(Direction, strafeAngle);

            if (Math.Abs(TurnRemaining) < 1.0)
                SetTurnRight(bodyTurn);

            if (Math.Abs(DistanceRemaining) < 1.0)
            {
                if (distance > 250)      SetForward(50);
                else if (distance < 150) SetBack(50);
                else                     SetForward(30);
            }
        }

        public override void OnHitBot(HitBotEvent e)
        {
            strafeDirection = -strafeDirection;
            SetBack(120);
            if (GunHeat == 0) SetFire(3.0);
        }

        public override void OnHitWall(HitWallEvent e)
        {
            strafeDirection = -strafeDirection;
            SetBack(80);
        }

        private bool IsNearWall() =>
            X < WALL_MARGIN || X > ArenaWidth  - WALL_MARGIN ||
            Y < WALL_MARGIN || Y > ArenaHeight - WALL_MARGIN;

        private void TurnTowardCenter()
        {
            double bearing = Math.Atan2(ArenaWidth / 2.0 - X, ArenaHeight / 2.0 - Y) * 180.0 / Math.PI;
            SetTurnRight(CalcShortestBearing(Direction, bearing));
        }

        private double CalcShortestBearing(double current, double target)
        {
            double diff = target - current;
            while (diff >  180) diff -= 360;
            while (diff < -180) diff += 360;
            return diff;
        }
    }
}