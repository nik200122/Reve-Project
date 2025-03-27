using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    //[SerializeField] private List<AttackSO> combo; 
    //[SerializeField] private Weapon weapon;
    private float lastClickTime;
    private float lastComboEnd;
    private int comboCounter;
    private float comboCountdown = 0.2f;
    private float attackCountdown = 1.9f;
    [SerializeField]private InputHandler input;
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    /*void Update()
    {
        
        ExitAttack();
    }

    void Start()
    {
        input.OnAttackEvent += HandleAttackEvent;
    }

    void OnDisable()
    {
        input.OnAttackEvent -= HandleAttackEvent;
    }

    void HandleAttackEvent()
    {
        if(GameStateManager.Instance.CurrentState == GameState.FreeRoam){
                Attack();
        }
    }
    void Attack(){
        if(Time.time - lastComboEnd > comboCountdown && comboCounter < combo.Count){
            //assicuriamo che non ci siano overlap
            CancelInvoke("EndCombo");
            if(Time.time - lastClickTime >= attackCountdown ){
                //andiamo a sovrascrivere l'animazione specifica del punto della combo
                animator.runtimeAnimatorController = combo[comboCounter].animatorOverrideController;
                //facciamo play dello stato sul layer 0 a tempo 0
                animator.Play("Attack", 0, 0);
                //l'arma avrà quindi il danno del singolo attacco
                //Possiamo aggiungere il resto della logica qua
                //TODO
                //weapon.damage = combo[comboCounter].damage;
                comboCounter++;
                lastClickTime = Time.time;
                if(comboCounter > combo.Count){
                    comboCounter = 0;
                }
            }
        }
    }

    void ExitAttack(){
        //vediamo se l'animazione è al 90% e se sta facendo effettivamente l'animazione di attacco
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f &&animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")){
            //dopo 1 sec invoca
            Invoke("EndCombo",1);
        }
    }

    void EndCombo(){
        comboCounter = 0;
        lastComboEnd = Time.time;
    }*/
}
