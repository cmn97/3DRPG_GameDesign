using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleScene : MonoBehaviour
{
    public GameObject playerCube;
    public GameObject otherCube;

    private GameObject[] enemies;

    public FightMenu battleUI;
    public PlayerPosVector playerPosVector;

    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip playerHitSound;
    [SerializeField] AudioClip enemyHitSound;
    [SerializeField] AudioClip missSound;

    private AudioSource audioSource;

    private bool playerMadeMove = false; // has player clicked FIGHT?

    public enum BattleStates {
        NONE,
        BEGIN,
        WIN,
        LOSE
    }
    

    //create BattleStates variable to control the state of battle
    public BattleStates currentState;
    public Distance d;

    void Start()
    {

        // Fills array with gameobjects tagged as "enemy"
        enemies = GameObject.FindGameObjectsWithTag("enemy");

        otherCube = null;

        currentState = BattleStates.BEGIN;
        playerPosVector.currentState = PlayerPosVector.MapStates.NORMAL;
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        // Changes currentState if CheckDistance from Distance class returns true
        changeStateFromDistance();

        //Looking for an enemy that's close by
        otherCube = FindClosestEnemy();
    }

    public void ChangeBattleState(BattleStates state)
    {
        currentState = state;
    }

    private void changeStateFromDistance()
    {
        // changes the BattleState to BEGIN to initiate the fight
        // if the distance between two items is less than 3
        if (otherCube != null)
        {
            currentState = BattleStates.BEGIN;
            playerPosVector.currentState = PlayerPosVector.MapStates.BATTLE;
            StartCoroutine(CombatLoop());
        }
        else
        {
            currentState = BattleStates.NONE;
            playerPosVector.currentState = PlayerPosVector.MapStates.NORMAL;
        }
    }

    //Used to check the currentState . use as debugger to see if the state
    // is displaying correctly.
    public BattleStates checkStates()
    {
        //Debug.Log("The current state is : " + currentState);
        return currentState;
    }
    
    private IEnumerator CombatLoop()
    {
        if (currentState == BattleStates.BEGIN)
        {
            while (!playerMadeMove) { yield return null; }

            //TODO fix mechanic so battlestate doesn't always
            // go back to "BEGIN" after player wins or loses
            // look @ the distance function changeStateFromDistance()
            if (currentState == BattleStates.LOSE || currentState == BattleStates.WIN)
            {
                currentState = BattleStates.NONE;
            }
        }
    }

    public void EnemyTurn()
    {
        // Make the combat loop pause
        playerMadeMove = false;

        //animation
        otherCube.GetComponentInChildren<Animator>().Play("Armature|Attack_01");

        // Enemy hit check roll
        Debug.Log("ENEMY attacks.");
        SkillCheck enemyHitCheck = new SkillCheck(20, 1, otherCube.GetComponent<StatSheet>().STRMod);
        int roll = enemyHitCheck.Roll();

        // Check if enemy hit player
        if (enemyHitCheck.CheckSuccess(playerCube.GetComponent<StatSheet>().DEF, roll) == true)
        {
            // Damage roll
            SkillCheck damageCheck = new SkillCheck(6, 1, otherCube.GetComponent<StatSheet>().STRMod);
            playerCube.GetComponent<StatSheet>().HP -= damageCheck.Roll();

            audioSource.PlayOneShot(enemyHitSound);

            Debug.Log("ENEMY hit PLAYER.");
        }
        else
        {
            // if attack missed
            audioSource.PlayOneShot(missSound);
            Debug.Log("PLAYER dodged!");
        }

        CheckPlayerHP();
        CheckEnemyHP();
    }

    public void PlayerTurn()
    {
        playerCube.GetComponentInChildren<MovementControls>().anim.Play("Armature|Sword_atk01");

        // Check if player attack is successful
        bool attackRoll = otherCube.GetComponent<Interact>().AttackRoll(playerCube.GetComponent<StatSheet>().STRMod);
        if (attackRoll == true)
        {
            SkillCheck damageCheck = new SkillCheck(6, 1, playerCube.GetComponent<StatSheet>().STRMod);
            otherCube.GetComponent<StatSheet>().HP -= damageCheck.Roll();
            audioSource.PlayOneShot(playerHitSound);
        }
        else
        {
            // if attack missed
            audioSource.PlayOneShot(missSound);
            Debug.Log("ENEMY dodged!");
        }

        CheckPlayerHP();
        CheckEnemyHP();
        playerMadeMove = true;

        /** Check if player won.
         *  If player won, WinBattle()
         *  If not, make EnemyTurn() after 1 seconds of delay to allow animations to finish
         */
        if(currentState != BattleStates.WIN) { Invoke("EnemyTurn", 1f); }
        else { WinBattle(); }
    }

    // Finds the closest enemy
    // Not yet optimized to handle multiple enemies within the same radius
    public GameObject FindClosestEnemy()
    {
        GameObject closest = null;
        foreach (GameObject GO in enemies)
        {
            d = new Distance(playerCube, GO);
            if (d.CheckDistance())
            {
                closest = GO;
            }
        }
        return closest;
    }


    public void CheckPlayerHP()
    {
        // Check if player has hp that is 0
        if (playerCube.GetComponent<StatSheet>().HP <= 0)
        {
            // Happens on player death:
            currentState = BattleStates.LOSE;
            audioSource.PlayOneShot(deathSound);
            Debug.Log("PLAYER has LOST the battle.");
            Invoke("ReloadLevel", 3f); // Reload current scene after 3 seconds delay
        }
    }
    public void CheckEnemyHP()
    {
        if (otherCube.GetComponent<StatSheet>().HP <= 0)
        {
            currentState = BattleStates.WIN;
        }
    }

    private void WinBattle()
    {
        Debug.Log("PLAYER has WON the battle.");
        playerPosVector.currentState = PlayerPosVector.MapStates.NORMAL;
        currentState = BattleStates.NONE;
        otherCube.SetActive(false);
        enemies = GameObject.FindGameObjectsWithTag("enemy");
    }

    public void Run()
    {
        playerPosVector.currentState = PlayerPosVector.MapStates.RAN;
        Debug.Log("pressed RUN");
        currentState = BattleStates.LOSE;
        otherCube.SetActive(false);
        enemies = GameObject.FindGameObjectsWithTag("enemy");
    }

    private void ReloadLevel()
    {
        // Reloads current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
