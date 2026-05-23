using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class GreedySurvivalBot : Bot
{
    private double threatX;          // menyimpan koordinat X musuh yang dianggap ancaman
    private double threatY;          // menyimpan koordinat Y musuh yang dianggap ancaman
    private double threatDistance;   // menyimpan jarak musuh terdekat sebagai ukuran ancaman
    private bool threatFound;        // penanda apakah ada ancaman yang terdeteksi radar

    static void Main(string[] args)  // fungsi utama menjalankan bot
    {
        new GreedySurvivalBot().Start();
    }

    GreedySurvivalBot() : base(BotInfo.FromFile("GreedySurvivalBot.json")) { }

    public override void Run()
    {
        BodyColor = Color.Green;

        while (IsRunning)
        {
            threatDistance = double.MaxValue;    // dibuat besar agar musuh pertama bisa menjadi ancaman awal
            threatFound = false;

            TurnRadarRight(360);                 // radar diputar penuh untuk membaca posisi musuh

            if (threatFound)
            {
                AvoidThreat();
                ShootIfPossible();
            }
            else
            {
                Patrol();
            }
        }
    }
    public override void OnScannedBot(ScannedBotEvent e)
    {
        double enemyDistance = DistanceTo(e.X, e.Y);

        if (enemyDistance < threatDistance) //Strategi menjauhi musuh terdekat
        {
            threatDistance = enemyDistance;
            threatX = e.X;
            threatY = e.Y;
            threatFound = true;
        }
    }
    private void AvoidThreat()   // bergerak untuk menjaga jarak dari ancaman terdekat
    {
        double threatBearing = BearingTo(threatX, threatY);

        if (threatDistance < 180)
        {
            TurnRight(threatBearing + 120);     // bot menjauh jika musuh terlalu dekat
            Back(120);
        }
        else if (threatDistance < 450)
        {
            TurnRight(threatBearing + 90);      //bergerak menyamping agra tidak mudah ditembak 
            Forward(100);
        }
        else
        {
            Forward(80);        // Jika musuh masih jauh, bot tetap bergerak tetapi tidak terlalu agresif.
            TurnRight(35);
        }
    }
    private void ShootIfPossible()   // menembak hanya sebagai peluang tambahan
    {
        double gunTurn = GunBearingTo(threatX, threatY);
        TurnGunLeft(gunTurn);

        if (GunHeat == 0)
        {
            Fire(1);
        }
    }
    private void Patrol()     // gerakan ketika belum ada musuh yang terdeteksi
    {
        Forward(88);
        TurnRight(33);
        Back(55);
        TurnLeft(22);
    }
    public override void OnHitByBullet(HitByBulletEvent e)   // respon saat terkena peluru
    {
        double bulletBearing = CalcBearing(e.Bullet.Direction);

        TurnLeft(91 - bulletBearing);
        Forward(111);
    }
    public override void OnHitWall(HitWallEvent e)    // respon saat menabrak dinding
    {
        Back(98);
        TurnRight(91);
    }
    public override void OnHitBot(HitBotEvent e)      // respon saat bertabrakan dengan bot lain
    {
        Back(99);
        TurnRight(49);
    }
}