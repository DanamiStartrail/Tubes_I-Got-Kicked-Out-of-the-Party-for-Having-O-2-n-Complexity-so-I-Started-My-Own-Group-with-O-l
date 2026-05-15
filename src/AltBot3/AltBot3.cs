using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

namespace TubesStima
{
    public class GreedyDodger : Bot
    {
        public static void Main(string[] args) => new GreedyDodger().Start();
        public GreedyDodger() : base(BotInfo.FromFile("AltBot3.json")) { }

        public override void Run()
        {
            while (IsRunning) TurnRadarRight(360);
        }

        public override void OnScannedBot(ScannedBotEvent e)
        {
            double dir = DirectionTo(e.X, e.Y);

            // Logika Greedy: Putar badan 90 derajat dari arah musuh (menghindar)
            double dodgeTurn = (dir + 90) - Direction;
            while (dodgeTurn > 180) dodgeTurn -= 360;
            while (dodgeTurn < -180) dodgeTurn += 360;
            TurnRight(dodgeTurn);
            
            Forward(100); // Bergerak menyamping

            // Putar meriam
            double gunTurn = dir - GunDirection;
            while (gunTurn > 180) gunTurn -= 360;
            while (gunTurn < -180) gunTurn += 360;
            TurnGunRight(gunTurn);

            // Tembak hanya jika energi tank masih aman
            if (Energy > 50 && GunHeat == 0) Fire(1.5);
            Rescan();
        }
    }
}