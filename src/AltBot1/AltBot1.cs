using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

namespace TubesStima
{
    public class GreedySniper : Bot
    {
        public static void Main(string[] args) => new GreedySniper().Start();
        public GreedySniper() : base(BotInfo.FromFile("AltBot1.json")) { }

        public override void Run()
        {
            while (IsRunning) TurnRadarRight(360);
        }

        public override void OnScannedBot(ScannedBotEvent e)
        {
            double dist = DistanceTo(e.X, e.Y);
            double dir = DirectionTo(e.X, e.Y);

            double gunTurn = dir - GunDirection;
            while (gunTurn > 180) gunTurn -= 360;
            while (gunTurn < -180) gunTurn += 360;
            TurnGunRight(gunTurn);

            // Logika Greedy: Mundur jika musuh mendekat
            if (dist < 300) Back(100);

            if (GunHeat == 0) Fire(1.0); // Daya tembak kecil agar peluru cepat
            Rescan();
        }
    }
}