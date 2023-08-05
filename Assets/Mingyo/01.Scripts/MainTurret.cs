using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTurret : Turret
{
    [SerializeField] ElementType type;
    [SerializeField] TurretStatSO _turretStatSO;

    [SerializeField] Vector2 dir;

    [SerializeField] Bullet _bullet;                                    //Pool

    protected override void Attack()
    {
        _animator.SetTrigger(attackTriggerHash);
    }

    private void Update()
    {
        if (CheckInnerDistance(_turretStatSO.FireTurretStat.Range, dir))
        {
            Attack();
        }
    }

    private void OnDrawGizmos()
    {
        DrawFanShapedGizmo(transform.position, _turretStatSO.FireTurretStat.Range, detectionAngle / 2, dir);
    }

    protected override void SetUp()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        SetUp();
    }

    protected override void OnShoot()
    {
        Debug.Log("Shoot");
        StartCoroutine(ShootBullet());
    }

    private IEnumerator ShootBullet()
    {
        Bullet bullet = Instantiate(_bullet, transform.position, Quaternion.identity).GetComponent<Bullet>();

        float returnTime = 0;
        switch(type)
        {
            case ElementType.Fire:
                bullet.Damage = _turretStatSO.FireTurretStat.Damage;
                returnTime = _turretStatSO.FireTurretStat.AttackSpeed;
                break;
            case ElementType.Earth:
                bullet.Damage = _turretStatSO.LandTurretStat.Damage;
                returnTime = _turretStatSO.LandTurretStat.AttackSpeed;
                break;
            case ElementType.Air:
                bullet.Damage = _turretStatSO.WindTurretStat.Damage;
                returnTime = _turretStatSO.WindTurretStat.AttackSpeed;
                break;
            case ElementType.Water:
                bullet.Damage = _turretStatSO.WaterTurretStat.Damage;
                returnTime = _turretStatSO.WaterTurretStat.AttackSpeed;
                break;
        }

        yield return new WaitForSeconds(returnTime);
    }
}
