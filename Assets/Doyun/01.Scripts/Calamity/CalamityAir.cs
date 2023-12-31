using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class CalamityAir : Calamity
{
    [SerializeField]
    private Transform _spawnMin, _spawnMax;

    [SerializeField] 
    private float _attackDelay;

    public override void OnCalamity()
    {
        base.OnCalamity();
        StartCoroutine(CalamityRoutine());
    }

    private IEnumerator CalamityRoutine()
    {
        float count = Random.Range(1f, 3f);
        for (int i = 0; i < count; i++)
        {
            float point = Random.Range(_spawnMin.position.y, _spawnMax.position.y);
            Vector3 spawnPos = new Vector3(_spawnMax.position.x, point);

            PoolingParticle particle = PoolManager.Instance.Pop("AirParticle") as PoolingParticle;
            particle.SetPositionAndRotation(spawnPos, Quaternion.identity);
            particle.Play();
            
            point = Random.Range(_spawnMin.position.y, _spawnMax.position.y);
            Vector3 dest = new Vector3(spawnPos.x + 35f, point);

            float time = 2f;
            float cur = 0f;
            float percent = 0f;

            while (percent <= 1f)
            {
                cur += Time.deltaTime;
                percent = cur / time;
                
                Vector3 pos = Vector3.Lerp(spawnPos, dest, percent);
                particle.SetPositionAndRotation(pos, quaternion.identity);
                
                Collider2D[] cols = Physics2D.OverlapBoxAll(pos, Vector3.one, 0,_targetLayer);
                for (int j = 0; j < cols.Length; j++)
                {
                    if (cols[j].TryGetComponent<IDamageable>(out var onDamage))
                    {
                        onDamage.OnDamage(_damage);
                    }
                }

                yield return null;
            }

            yield return new WaitForSeconds(_attackDelay);
        }
    }
}
