using Robocode.TankRoyale.BotApi;

namespace TubesStima
{
    public class TankDiemBot : Bot
    {
        public static void Main(string[] args) => new TankDiemBot().Start();
        public TankDiemBot() : base(BotInfo.FromFile("AltBot3.json")) { }

        public override void Run()
        {
            while (IsRunning)
            {
                // Putar radar agar server tidak menganggap bot AFK
                SetTurnRadarRight(360);
                
                // Cek panas meriam. Jika 0 (sudah dingin), tembak lurus.
                if (GunHeat == 0)
                {
                    SetFire(3.0); // Anda bisa mengganti daya tembak antara 0.1 hingga 3.0
                }

                Go();
            }
        }
    }
}