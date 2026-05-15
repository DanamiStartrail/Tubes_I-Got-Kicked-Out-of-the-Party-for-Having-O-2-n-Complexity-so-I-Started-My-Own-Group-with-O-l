using System;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

namespace TubesStima
{
    public class MainBot : Bot
    {
        int arahGerak = 1;

        public static void Main(string[] args) => new MainBot().Start();
        public MainBot() : base(BotInfo.FromFile("MainBot.json")) { }

        public override void Run()
        {
            // Memisahkan putaran mekanis agar tidak saling menyeret
            AdjustGunForBodyTurn = true;
            AdjustRadarForGunTurn = true;

            while (IsRunning)
            {
                // Putaran radar standar saat tidak melihat musuh
                SetTurnRadarRight(360);
                
                // Menjalankan semua tumpukan instruksi 'Set...' untuk tick ini
                Go();
            }
        }

        public override void OnScannedBot(ScannedBotEvent e)
        {
            double jarak = DistanceTo(e.X, e.Y);
            double arahMusuh = DirectionTo(e.X, e.Y);

            // 1. Radar Lock Sempurna
            // Mengesampingkan SetTurnRadarRight(360) dari putaran Run()
            SetTurnRadarRight(NormalisasiSudut(arahMusuh - RadarDirection));

            // 2. Pergerakan Spiral & Hindari Dinding
            double putarBadan = (arahMusuh + 80) - Direction;
            SetTurnRight(NormalisasiSudut(putarBadan));
            
            if (X < 50 || X > ArenaWidth - 50 || Y < 50 || Y > ArenaHeight - 50) 
            {
                arahGerak *= -1; // Putar balik jika mendekati dinding
            }
            SetForward(150 * arahGerak);

            // 3. Logika Penembakan Jarak Dinamis & Prediktif
            if (jarak <= 400) 
            {
                double dayaTembak = 1.0;
                if (jarak < 100) dayaTembak = 3.0; 
                else if (jarak < 250) dayaTembak = 2.0;

                double kecepatanPeluru = 20 - (3 * dayaTembak);
                double waktuTempuh = jarak / kecepatanPeluru;
                
                // Kalkulasi sudut trigonometri
                double arahRadian = e.Direction * (Math.PI / 180.0);
                double targetX = e.X + (Math.Sin(arahRadian) * e.Speed * waktuTempuh);
                double targetY = e.Y + (Math.Cos(arahRadian) * e.Speed * waktuTempuh);

                double arahTembak = DirectionTo(targetX, targetY);
                double putarMeriam = NormalisasiSudut(arahTembak - GunDirection);

                SetTurnGunRight(putarMeriam);

                // Eksekusi tembakan HANYA jika kemiringan meriam di bawah 2 derajat dari target
                if (GunHeat == 0 && Math.Abs(putarMeriam) < 2.0) 
                {
                    SetFire(dayaTembak);
                }
            }
            else
            {
                SetTurnGunRight(NormalisasiSudut(arahMusuh - GunDirection));
            }
        }

        private double NormalisasiSudut(double sudut)
        {
            while (sudut > 180) sudut -= 360;
            while (sudut < -180) sudut += 360;
            return sudut;
        }
    }
}