using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class GreedyMainBot : Bot
{
    private double targetX;          // koordinat X target terbaik
    private double targetY;          // koordinat Y target terbaik
    private double targetDistance;   // jarak target terbaik
    private double targetEnergy;     // energi target terbaik
    private double bestScore;        // skor greedy tertinggi
    private bool targetFound;        // penanda apakah target ditemukan

    static void Main(string[] args)
    {
        new GreedyMainBot().Start();
    }

    GreedyMainBot() : base(BotInfo.FromFile("GreedyMainBot.json")) { }

    public override void Run()
    {
        BodyColor = Color.Blue;
        TurretColor = Color.DarkBlue;
        RadarColor = Color.Yellow;
        BulletColor = Color.Red;
        ScanColor = Color.Cyan;
        TracksColor = Color.DarkGray;
        GunColor = Color.Navy;

        // Gun dan radar dibuat independen dari gerakan body.
        // Tujuannya agar bot tetap bisa membidik dan memindai musuh
        // walaupun body sedang bergerak atau berbelok.
        AdjustGunForBodyTurn = true;
        AdjustRadarForBodyTurn = true;
        AdjustRadarForGunTurn = true;

        while (IsRunning)
        {
            // Reset target pada setiap siklus pencarian.
            bestScore = double.MinValue;
            targetDistance = double.MaxValue;
            targetEnergy = 100;
            targetFound = false;

            // Radar diputar penuh untuk mencari semua kandidat musuh.
            TurnRadarRight(360);

            if (targetFound)
            {
                AimAndFire();
                MoveByTargetDistance();
            }
            else
            {
                SearchMove();
            }
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        double enemyDistance = DistanceTo(e.X, e.Y);

        // Greedy gabungan:
        // target dipilih berdasarkan skor dari jarak, energi, dan jarak tembak efektif.
        double score = CalculateOpportunityScore(e.Energy, enemyDistance);

        if (score > bestScore)
        {
            bestScore = score;
            targetX = e.X;
            targetY = e.Y;
            targetDistance = enemyDistance;
            targetEnergy = e.Energy;
            targetFound = true;
        }
    }

    private double CalculateOpportunityScore(double enemyEnergy, double enemyDistance)
    {
        // DistanceScore:
        // semakin dekat musuh, semakin tinggi skor karena peluang tembakan masuk lebih besar.
        double distanceScore = Math.Max(0, 1200 - enemyDistance) / 1200.0;

        // EnergyScore:
        // semakin rendah energi musuh, semakin tinggi skor karena peluang eliminasi lebih besar.
        double energyScore = Math.Max(0, 100 - enemyEnergy) / 100.0;

        // RangeScore:
        // musuh pada jarak efektif diberi skor lebih tinggi.
        double rangeScore;

        if (enemyDistance <= 300)
        {
            rangeScore = 1.0;
        }
        else if (enemyDistance <= 600)
        {
            rangeScore = 0.6;
        }
        else
        {
            rangeScore = 0.3;
        }

        // Rumus greedy utama:
        // Score = 0.45 DistanceScore + 0.35 EnergyScore + 0.20 RangeScore
        return (0.45 * distanceScore) +
               (0.35 * energyScore) +
               (0.20 * rangeScore);
    }

    private void AimAndFire()
    {
        double gunTurn = GunBearingTo(targetX, targetY);
        TurnGunLeft(gunTurn);

        if (GunHeat == 0)
        {
            Fire(1);
        }
    }

    private void MoveByTargetDistance()
    {
        double targetBearing = BearingTo(targetX, targetY);

        if (targetDistance < 170)
        {
            // Jika target terlalu dekat, bot menjauh agar tidak mudah ditabrak.
            TurnRight(targetBearing + 120);
            Back(100);
        }
        else if (targetDistance < 500)
        {
            // Jika target berada pada jarak sedang, bot bergerak menyamping.
            // Tujuannya agar bot tidak mudah ditembak lurus.
            TurnRight(targetBearing + 90);
            Forward(100);
        }
        else
        {
            // Jika target jauh, bot mendekat secara bertahap.
            TurnRight(targetBearing);
            Forward(90);
        }
    }

    private void SearchMove()
    {
        // Gerakan saat belum menemukan target.
        Forward(90);
        TurnRight(35);
        Back(45);
        TurnLeft(25);
    }

    public override void OnHitByBullet(HitByBulletEvent e)
    {
        double bulletBearing = CalcBearing(e.Bullet.Direction);

        // Menghindar tegak lurus terhadap arah peluru.
        TurnLeft(90 - bulletBearing);
        Forward(100);
    }

    public override void OnHitWall(HitWallEvent e)
    {
        // Keluar dari dinding agar tidak terjebak di sisi arena.
        Back(100);
        TurnRight(90);
    }

    public override void OnHitBot(HitBotEvent e)
    {
        // Menjauh dari bot lain agar tidak menerima collision damage berulang.
        Back(80);
        TurnRight(60);
    }
}