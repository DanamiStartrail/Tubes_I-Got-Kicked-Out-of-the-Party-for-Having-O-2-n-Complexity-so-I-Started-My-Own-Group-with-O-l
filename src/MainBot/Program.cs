using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

namespace TubesStima
{
    public class GreedyMainBot : Bot
    {
        public static void Main(string[] args) => new GreedyMainBot().Start();
        public GreedyMainBot() : base(BotInfo.FromFile("MainBot.json")) { }

        public override void Run()
        {
            while (IsRunning)
            {
                TurnRadarRight(360);
            }
        }

        public override void OnScannedBot(ScannedBotEvent e)
        {
            // 1. Ambil jarak dan arah target menggunakan koordinat X dan Y
            double distanceToBot = DistanceTo(e.X, e.Y);
            double directionToBot = DirectionTo(e.X, e.Y);

            // 2. Hitung seberapa jauh meriam harus berputar
            double gunTurnAngle = directionToBot - GunDirection;
            
            // Normalisasi sudut ke rentang -180 hingga 180 agar meriam mencari jalur putaran terpendek
            while (gunTurnAngle > 180) gunTurnAngle -= 360;
            while (gunTurnAngle < -180) gunTurnAngle += 360;
            
            TurnGunRight(gunTurnAngle);

            // 3. Logika kelayakan (maju/mundur berdasarkan jarak)
            if (distanceToBot > 150)
            {
                Forward(50);
            }
            else if (distanceToBot < 50)
            {
                Back(50);
            }

            // 4. Logika penembakan
            if (GunHeat == 0)
            {
                Fire(distanceToBot < 150 ? 3.0 : 1.0);
            }
            
            Rescan(); // Pindai ulang target
        }
    }
}