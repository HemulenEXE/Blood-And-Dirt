using GunLogic;

public interface IBullet
{
    public Side sideBullet {  get; set; }
    public float Damage { get; set;}
    public GunType GunType { get; set;}
    public float Speed { get; set; }

}
