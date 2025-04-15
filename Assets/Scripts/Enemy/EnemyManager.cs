using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : IHittable
{
    [SerializeField] private float airknockbackForce = 10f; 
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private GameObject hitVfx;
    [SerializeField] private BattleCalculatorManager battleCalculatorManager;
    [SerializeField] private GlobalRulesManager globalRulesManager;

    //indica i tipi di danni a cui è vulnerabile
    private HashSet<DamageTypeTag> vulnerabilities = new();
    //indica i tipi di danni che infligge quando colpisce
    private  List<DamageType> offensiveDamageType;

    private Enemy enemy;

    private void Awake(){
        enemy = GetComponent<Enemy>();
    }

    public void SetEnemyModel(Enemy loadedEnemy){
       enemy=loadedEnemy;
    }

    public void SetEnemyStats(List<Stat> loadedStats){
        enemy.stats = loadedStats;
        //per test
        vulnerabilities.Add(DamageTypeTag.Impact);
    }

    public void OnHit(IHittable attacker){
        Debug.Log("COLPITO ENEMY");
        // Calcola la direzione del colpo (da dove proviene l'attaccante)mos
        Vector3 knockbackDirection = transform.position - attacker.transform.position;
        knockbackDirection.y = 0;  // Assicurati che la spinta sia orizzontale (evita movimenti verticali)

        // Definisci la distanza di spostamento
        float knockbackDistance = 1f; // La distanza da percorrere

        // Calcola la posizione di destinazione (quella a distanza knockbackDistance)
        Vector3 targetPosition = transform.position + knockbackDirection.normalized * knockbackDistance;

        // Sposta l'oggetto verso la posizione di destinazione in modo fluido
        float smoothSpeed = 5f;  // Velocità del movimento
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        //calcolo danno
        IHittable defender = this;
        float damage = battleCalculatorManager.EvaluateDamage(attacker, defender);
        TakeDamage(damage);
        SpawnHitVfx(transform.position);
    }

    private void TakeDamage(float damage){
        string damageTargetStatTag = globalRulesManager.GetDamageApplicationRule().damageTargetStatTag;
        enemy.SetStat(damageTargetStatTag, enemy.GetStat(damageTargetStatTag).currentValue - damage);
        Debug.Log("VITA ENEMY: "+enemy.GetStat(damageTargetStatTag).currentValue);
    }

    public void SpawnHitVfx(Vector3 Pos_){
        Instantiate(hitVfx, Pos_, Quaternion.identity);
    }

    public void SetAttackDataList(List<AttackData> attackDataList){
        enemy.SetAttackDataList(attackDataList);
    }

    public override List<DamageType> GetOffensiveDamageTypeList(){
        return offensiveDamageType;
    }

    public override HashSet<DamageTypeTag> GetVulnerabilities(){
        return vulnerabilities;
    }

    public override Stat GetStat(string statTag){
        return enemy.GetStat(statTag);
    }

    public override void SetVulnerabilities(HashSet<DamageTypeTag> vulnerabilities){
       this.vulnerabilities = vulnerabilities;
    }

    public override void SetOffensiveDamageTypeList(List<DamageType> offensiveDamageType){
        this.offensiveDamageType = offensiveDamageType;
    }
}
