using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class GunSystem : MonoBehaviour
{
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    private int bulletsLeft, bulletsShot;

    private bool shooting, readyToShoot, reloading;

    [SerializeField] private Inventario inventarioScript;
    [SerializeField] private Camera fpsCam;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private RaycastHit rayHit;
    [SerializeField] private LayerMask whatIsEnemy, walls, sueloMask, Puerta;
    public Animator anim;
    [SerializeField] private GameObject reloadParticles;
    [SerializeField] private ParticleSystem fleshParticles;
    [SerializeField] private ParticleSystem decalParticle;

    public Image bulletGIF;
    public TextMeshPro ammoText;
    [SerializeField] private CameraShake cs;
    public float trailForce;

    [SerializeField] private AudioClip shotgunShootSound;
    [SerializeField] private AudioClip shotgunReloadSound;
    [SerializeField] private AudioClip[] bloodSounds;
    //[SerializeField] private AudioClip shotgunPickUpSound;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        if(inventarioScript.inventarioAbierto == false)
        {
            Inputs();
            ammoText.SetText(bulletsLeft + " / " + magazineSize);
        }
    }

    private void Inputs()
    {
        if (allowButtonHold)
        {
            shooting = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
        {
            Reload();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && bulletsLeft == 0 && !reloading)
        {
            Reload();
        }

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        for (int i = 0; i < bulletsPerTap; i++)
        {
            float angle = Random.Range(0, Mathf.PI * 2);
            float radius = Random.Range(0, spread);
            float x = radius * Mathf.Cos(angle);
            float y = radius * Mathf.Sin(angle);

            Vector3 direction = fpsCam.transform.forward + fpsCam.transform.right * x + fpsCam.transform.up * y;
            direction.Normalize();

            if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range))
            {
                var hitPoint = rayHit.point;

                if (rayHit.collider != null)
                {
                    if (rayHit.collider.CompareTag("Enemigo"))
                    {
                        var enemy = rayHit.collider.GetComponent<Enemigo>();
                        if (enemy != null)
                        {
                            enemy.RecibirDanio(damage);
                            BloodParticle();
                        }    
                    }
                    else DecalParticles();       
                }
                else
                {
                    hitPoint = fpsCam.transform.position + (direction * range);                 
                }         
            }

            var bulletTrail = Factory.Instance.GetBulletFromPool();
            BulletTrail(bulletTrail);

            bulletTrail.transform.position = attackPoint.position;
            bulletTrail.transform.forward = direction;
        }

        var muzzleEffect = Factory.Instance.GetMuzzleFromPool();
        MuzzleEffect(muzzleEffect);

       StartCoroutine(cs.Shaking());
        SoundFXManager.Instance.PlaySoundFXClip(shotgunShootSound, attackPoint.transform, .15f);

        anim.SetTrigger("ShotgunExtiguisherShootAnim");

        bulletsLeft--;

        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        reloading = true;
        SoundFXManager.Instance.PlaySoundFXClip(shotgunReloadSound, gameObject.transform, 1f);
        reloadParticles.gameObject.SetActive(true);
        Invoke("ReloadFinished", reloadTime);
    }

    private void BloodParticle()
    {
        ParticleSystem spawnedParticles = Instantiate(fleshParticles, rayHit.point, Quaternion.LookRotation(rayHit.normal));
        SoundFXManager.Instance.PlayRandomSoundFXClip(bloodSounds, rayHit.transform, 0.5f);
        spawnedParticles.Emit(1);
        Destroy(spawnedParticles.gameObject, 1f);
    }

    private void BulletTrail(Particle bulletTrail)
    {
        bulletTrail.transform.position = attackPoint.transform.localPosition;
        bulletTrail.transform.rotation = attackPoint.transform.rotation;
    }

    private void MuzzleEffect(Particle muzzleEffect)
    {
        muzzleEffect.transform.parent = attackPoint;
        muzzleEffect.transform.position = attackPoint.position;
        muzzleEffect.transform.rotation = attackPoint.rotation;
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloadParticles.gameObject.SetActive(false);
        reloading = false;
    }

    protected void DecalParticles()
    {
        ParticleSystem spawnedParticles = Instantiate(decalParticle, rayHit.point, Quaternion.LookRotation(rayHit.normal));
        spawnedParticles.Emit(1);
        Destroy(spawnedParticles.gameObject, 2f);
    }


}


