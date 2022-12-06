using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{

    public EnemyAnimationManager anim;
    public float staggerTime = 1.16f;
    EnemyManager enemyManager;
    public GameObject bloodType;
    private void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<EnemyAnimationManager>();
        enemyManager = GetComponent<EnemyManager>();
    }
    public void TakeDamage(int damage)
    {
        if (!enemyManager.isDead)
        {
            currentHealth -= damage;
            //anim.PlayTargetAnimation("Damage01", true);
            anim.anim.Play("Damage01");
            anim.anim.SetBool("IsInteracting", true);
            anim.anim.SetTrigger("Hit");
            anim.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            enemyManager.currRecoveryTime = staggerTime;
            if (currentHealth <= 0)
            {
                GetComponent<Collider>().enabled = false;
                GetComponent<Rigidbody>().useGravity = false;
                currentHealth = 0;
                anim.PlayTargetAnimation("Death", true);
                enemyManager.isDead = true;
                //FindObjectOfType<CameraMovement>().LockedCharacterDied(enemyManager.lockOn);
            }
        }
    }
    private void CalculateStats()
    {
        //TODO:
        //calculate enemy stats based on player level 
    }
}
