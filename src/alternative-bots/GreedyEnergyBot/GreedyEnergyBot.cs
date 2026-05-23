using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class GreedyEnergyBot : Bot 
{
    private double lowestEnergy; //menyimpan energi musuh terendah
    private bool targetFound;       //penanda jika bot menemukan target pada radar
    private double targetY;        //mnnyimpan koordinat X musuh terdekat dalam radar
    private double targetX;        //mnnyimpan koordinat Y musuh terdekat dalam radar
    private double targetDistance;  //menyimpn jarak target sebagai pembanding jika berenergi sama

    static void Main (string[]args)     //fungsi utama menjalankan bot
    {
        new GreedyEnergyBot().Start();
    }
    GreedyEnergyBot() : base(BotInfo.FromFile("GreedyEnergyBot.json")) { }

    public override void Run()      
    {
        BodyColor = Color.Red;

        while (IsRunning)
        {
            lowestEnergy = double.MaxValue;      // dibuat agar musuh pertama jadi target
            targetDistance = double.MaxValue;
            targetFound = false;

            TurnRadarRight(360);    //radar diputar penuuh

            if (targetFound)
            {
                AimAndFire();
            }
            MoveAround();

        }
    }
    public override void OnScannedBot(ScannedBotEvent e)
    {
        double enemyDistance = DistanceTo(e.X, e.Y);        //Strategi menarget musuh enerhi terendah,jika sama memilih terdekat
        if (e.Energy < lowestEnergy ||
            (e.Energy == lowestEnergy && enemyDistance < targetDistance))
        {
            lowestEnergy = e.Energy;
            targetDistance = enemyDistance;
            targetY = e.Y;
            targetX = e.X;
            targetFound = true;

        }
    }
    private void AimAndFire()     //menembak ke target energi terrendah
    {
        double gunTurn = GunBearingTo(targetX, targetY);
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