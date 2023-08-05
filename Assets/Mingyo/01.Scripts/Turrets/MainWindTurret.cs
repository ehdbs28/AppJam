using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWindTurret : Turret
{
    [SerializeField] TurretStatSO _turretStatSO;

    protected override void Attack()
    {
        Debug.Log("321");
    }

    private void Update()
    {
        if (CheckInnerDistance(_turretStatSO.TurretStatList[2].Range, Vector2.up))
        {
            Attack();
        }
    }

    private void OnDrawGizmos()
    {
        DrawFanShapedGizmo(transform.position, _turretStatSO.TurretStatList[2].Range, detectionAngle / 2, Vector2.up);
    }
}
