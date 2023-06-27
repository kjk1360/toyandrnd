//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ChainShot : SkillBase
//{
//    //public float projectileSpeed = 5f;
//    public float fireRate = 0.1f;
//    //public GameObject projectilePrefab;

//    private int projectileCount;
//    private int currentProjectile = 0;
//    public override void ActivateSkill(BaseController user)
//    {
//        if (isOnCooldown) return;

//        projectileCount = 1 + user.GetStatValue(StatName.ProjectileCount);

//        StartCoroutine(FireProjectiles(user));
//    }

//    private IEnumerator FireProjectiles(BaseController user)
//    {
//        int chainCount = 1 + user.GetStatValue(StatName.ChainCount);
//        float blastRadius = (3 + user.GetStatValue(StatName.BlastRadius)) * (1f + user.GetStatMuliply(StatName.BlastRadius));

//        while (currentProjectile < projectileCount)
//        {
//            var projectile = SpawnManager.Instance.SpawnChainShot(user.transform.position);
//            if(projectile == null)
//            {
//                yield return new WaitForSeconds(fireRate);
//                continue;
//            }
//            projectile.GetComponent<ChainShotProjectile>().Initialize(user);

//            currentProjectile++;
//            yield return new WaitForSeconds(fireRate);
//        }

//        InActiveSkill();
//    }

//    public override void InActiveSkill()
//    {
//        StartCoroutine(ApplyCooldown());
//    }

//    private IEnumerator ApplyCooldown()
//    {
//        yield return new WaitForSeconds(cooldownTime);
//        isOnCooldown = false;
//        currentProjectile = 0;
//    }
//}
