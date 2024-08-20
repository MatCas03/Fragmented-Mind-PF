using UnityEngine;

public class Factory : MonoBehaviour
{
    public static Factory Instance { get; private set; }

    public Particle _muzzleFlash, _bulletTrail, _bloodParticle;
    [SerializeField] private int _cantidadInicial;

    private Pool<Particle> _poolMuzzle;
    private Pool<Particle> _poolBullet;
    private Pool<Particle> _poolBlood;

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _poolMuzzle = new Pool<Particle>(MuzzleFlash, Particle.TurnOn, Particle.TurnOff, _cantidadInicial);
        _poolBullet = new Pool<Particle>(BulletTrail, Particle.TurnOn, Particle.TurnOff, _cantidadInicial);
        _poolBlood = new Pool<Particle>(BloodParticle, Particle.TurnOn, Particle.TurnOff, _cantidadInicial);
    }

    Particle MuzzleFlash()
    {
        return Instantiate(_muzzleFlash);
    }

    Particle BulletTrail()
    {
        return Instantiate(_bulletTrail);
    }

    Particle BloodParticle()
    {
        return Instantiate(_bloodParticle);
    }

    public Particle GetMuzzleFromPool()
    {
        return _poolMuzzle.GetObject();
    }
    
    public Particle GetBulletFromPool()
    {
        return _poolBullet.GetObject();
    }
    
    public Particle GetBloodFromPool()
    {
        return _poolBlood.GetObject();
    }

    public void ReturnObjectToPool(Particle obj)
    {
        _poolMuzzle.ReturnObjectToPool(obj);
    }
}
