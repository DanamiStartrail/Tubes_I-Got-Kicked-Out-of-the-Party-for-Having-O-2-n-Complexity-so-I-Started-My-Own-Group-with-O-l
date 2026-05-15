using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

namespace TubesStima
{
    public class GreedyRammer : Bot
    {
        public static void Main(string[] args) => new GreedyRammer().Start();
        public GreedyRammer() : base(BotInfo.FromFile("AltBot2.json")) { }

        public override void Run()
        {
            while (IsRunning) TurnRadarRight(360);
        }

        public override void OnScannedBot(ScannedBotEvent e)
        {
            double dist = DistanceTo(e.X, e.Y);
            double dir = DirectionTo(e.X, e.Y);

            // Putar badan tank menghadap musuh
            double bodyTurn = dir - Direction;
            while (bodyTurn > 180) bodyTurn -= 360;
            while (bodyTurn < -180) bodyTurn += 360;
            TurnRight(bodyTurn);

            // Putar meriam ke arah musuh
            double gunTurn = dir - GunDirection;
            while (gunTurn > 180) gunTurn -= 360;
            while (gunTurn < -180) gunTurn += 360;
            TurnGunRight(gunTurn);

            // Logika Greedy: Tabrak!
            Forward(dist);

            // Tembak daya maksimal jika sudah dekat
            if (dist < 100 && GunHeat == 0) Fire(3.0);
            Rescan();
        }
    }
}