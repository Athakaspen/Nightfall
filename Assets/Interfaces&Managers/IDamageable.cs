using UnityEngine;

public interface IDamageable
{

    bool isInvincible{get;}

    float lifePercent{get;}
    
    void OnHit(float damage);
}
