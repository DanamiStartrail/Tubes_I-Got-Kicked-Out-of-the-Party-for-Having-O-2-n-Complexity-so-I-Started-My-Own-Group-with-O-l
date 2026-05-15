using System;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

namespace TubesStima
{
    public class GerakAcakBot : Bot
    {
        int arahGerak = 1; // 1 untuk maju, -1 untuk mundur
        Random rand = new Random();

        public static void Main(string[] args) => new GerakAcakBot().Start();
        public GerakAcakBot() : base(BotInfo.FromFile("AltBot1.json")) { }

        public override void Run()
        {
            // Memisahkan putaran radar, meriam, dan badan
            AdjustGunForBodyTurn = true;
            AdjustRadarForGunTurn = true;
            
            while (IsRunning) TurnRadarRight(360);
        }

        public override void OnScannedBot(ScannedBotEvent e)
        {
            // 1. Radar Lock (mengunci musuh agar tidak perlu berputar 360 derajat lagi)
            // Hitung arah absolut musuh
            double arahMusuh = DirectionTo(e.X, e.Y);

            // Hitung selisih arah musuh dengan arah radar saat ini
            double putarRadar = arahMusuh - RadarDirection;

            // Normalisasi sudut agar selalu mencari putaran terpendek
            while (putarRadar > 180) putarRadar -= 360;
            while (putarRadar < -180) putarRadar += 360;

            // Perintahkan radar berputar persis sejauh selisih tersebut
            SetTurnRadarRight(putarRadar);

            // 2. Gerak Menyamping dan Acak
            // 10% kemungkinan untuk memutar balik arah secara tiba-tiba setiap tick
            if (rand.NextDouble() < 0.1) 
            {
                arahGerak *= -1;
            }
            
            // Tegak lurus dengan musuh (+90 derajat)
            double putarBadan = (arahMusuh + 90) - Direction;
            while (putarBadan > 180) putarBadan -= 360;
            while (putarBadan < -180) putarBadan += 360;
            
            SetTurnRight(putarBadan);
            SetForward(150 * arahGerak);

            // 3. Tembakan Cepat Head-On
            double putarMeriam = arahMusuh - GunDirection;
            while (putarMeriam > 180) putarMeriam -= 360;
            while (putarMeriam < -180) putarMeriam += 360;
            SetTurnGunRight(putarMeriam);

            if (GunHeat == 0) SetFire(1.0); // Daya kecil agar peluru melaju cepat

            Go(); // Eksekusi semua perintah pergerakan dan tembakan sekaligus
        }
    }
}