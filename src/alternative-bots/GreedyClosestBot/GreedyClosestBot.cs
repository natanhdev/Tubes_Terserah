using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class GreedyClosestBot : Bot 
{
    private double nearestDistance; //menyimpan jarak terdekat smentara dalam radar
    private bool targetFound;       //penanda apakah bot menemukan target pada radar
    private double nearestY;        //mnnyimpan koordinat musuh terdekat dalam radar
    private double nearestX;

    static void Main (string[]args)     //fungsi utama menjalankan bot
    {
        new GreedyClosestBot().Start();
    }
    GreedyClosestBot() : base(BotInfo.FromFile("GreedyClosestBot.json")) { }

    public override void Run()      
    {
        BodyColor = Color.Pink;

        while (IsRunning)
        {
            nearestDistance = double.MaxValue;      //nearestdistance dibuat besar sehingga musuh pertama yang kedetect langsung jadi target
            targetFound = false;

            TurnRadarRight(360);    //radar diputar penuuh

            if (targetFound)
            {
                FaceAndShoot();
            }
            MoveAround();

        }
    }
    public override void OnScannedBot(ScannedBotEvent e)
    {
        double currentDistance = DistanceTo(e.X, e.Y);
        if (currentDistance < nearestDistance)
        {
            nearestDistance = currentDistance;
            nearestY = e.Y;
            nearestX = e.X;
            targetFound = true;

        }
    }
    private void FaceAndShoot()     //menembak ke target terdekat dengan power 1
    {
        double gunTurn = GunBearingTo(nearestX, nearestY);
        TurnGunLeft(gunTurn);

        if (GunHeat == 0)
        {
            Fire(1);
        }
    }
    private void MoveAround()   //pergerakan bot agar tidak hanya diam
    {
        Forward(88);
        TurnRight(33);
        Back(55);
        TurnLeft(22);
    }
    public override void OnHitByBullet(HitByBulletEvent e)      //respon saat terkena peluru 
    {
        double BulletBearing = CalcBearing(e.Bullet.Direction);

        TurnRight(99 - BulletBearing);
        Forward(98);
    }
    public override void OnHitWall(HitWallEvent e)      //respon saat nabrak dinding
    {
        Back(99);
        TurnRight(99);
    }
    public override void OnHitBot(HitBotEvent e)        //respon saat nabrak bot
    {
        Back(77);
        TurnRight(44);
    }
}