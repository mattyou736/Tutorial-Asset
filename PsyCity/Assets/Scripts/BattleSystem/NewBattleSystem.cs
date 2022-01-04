using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public enum BattleState
{
    START, PLAYERTURN, ENEMYTURN, WON, LOST, QTE
}


public class NewBattleSystem : GenericWindow
{
    //state of any given battle , used to make sure we cant do things when we arent at our turn
    public BattleState state;
    public IconHolder icons;

    //party ui
    [Header("Party")]
    public Actor[] partyMembers;
    public Inventory inventory;
    public Transform[] partyMemberSpawn;
    public Text[] partyMemberName;
    public Text[] partyMemberHP;
    public Text[] partyMemberAP;
    public Image[] partyMemberStatus;
    public Image[] partyMemberHPBar;
    public Image[] partyMemberAPBar;

    public GameObject partyMembersActionsMenu;
    public GameObject partyMembersItemsMenu;
    public GameObject partyMembersSelecionMenu;

    int currentPartyMemberTurn = 0;
    bool partyDead = false;

    //final attack
    public Image meterToFill;
    float metervalue;
    public GameObject finalAttackButton;

    //chances of run succes
    [Range(0, .9f)]
    public float runOdds = .3f;

    [Header("Enemy")]
    public Actor[] enemys;
    Actor enemy;
    public Transform enemySpawn;
    public Text enemyName;
    public Text enemyHP;
    public Image enemyStatus;
    public Image enemyHPBar;
    public List<Actor> potencialTargets;

    //buttons
    public ButtonSetter action1, action2, action3, item1, item2, memberButton1, memberButton2, memberButton3;
    GameObject PartymemberImage1, PartymemberImage2, PartymemberImage3, enemyImage;
    private ShakeManager shakeManager;
    

    public delegate void BattleOver(bool playerWin);
    public BattleOver battleOverCallback;
    public List<GenericBattleAction> comboList = null;

    Item item_;
    PlayerMovement player;

    //QTE Variables
    public GameObject QTEObject;
    int _actionInt;
    public GameObject DamText;

    //bgm
    public AudioManager BGM;
    

    // Start is called before the first frame update
    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        //BGM = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioManager>();
        shakeManager = GetComponent<ShakeManager>();
        
        //state = BattleState.START;
        ChangeState(BattleState.START);
    }

    void ChangeState(BattleState _state)
    {
        state = _state;

        switch (state)
        {
            case BattleState.START:
                SetupBattle();
                break;
            case BattleState.PLAYERTURN:
                SetHUD();
                PlayerTurn();
                break;
            case BattleState.ENEMYTURN:
                SetHUD();
                EnemyAttack();
                break;
            case BattleState.QTE:
                QTETime();
                break;
            case BattleState.LOST:
                StartCoroutine(OnBattleOver());
                break;
            case BattleState.WON:
                StartCoroutine(OnBattleOver());
                break;
        }
    }

    //setting up the right actors at the right positions
    void SetupBattle()
    {
        BGM.ChangeBGM(true);
        player.canMove = false;
        //decides randomly based on array length what enemy to face // should probably make a if for a boss
        enemy = enemys[(int)Random.Range(0, enemys.Length)].Clone<Actor>();
        string entryMessage = "A " + enemy.name + "Showed up!!!";
        StartCoroutine(MessageEnumarator(entryMessage));


        //reset hp and turns
        metervalue = 0;
        enemy.ResetHealth();
        enemy.ResetAP();
        currentPartyMemberTurn = 0;
        enemy.ResetStatus();


        foreach (Actor partymember in partyMembers)
        {
            partymember.ResetAP();
            partymember.ResetHealth();
            partymember.ResetStatus();
        }

        //spawn player images
        PartymemberImage1 = (GameObject)Instantiate(partyMembers[0].characterObject, partyMemberSpawn[0]);
        PartymemberImage2 = (GameObject)Instantiate(partyMembers[1].characterObject, partyMemberSpawn[1]);
        PartymemberImage3 = (GameObject)Instantiate(partyMembers[2].characterObject, partyMemberSpawn[2]);

        //set the hud right
        SetHUD();

        //in the enemies case i should make a randomizer that picks between a few diffrent actors to have the encounter be random
        enemyImage = (GameObject)Instantiate(enemy.characterObject, enemySpawn);
        ChangeState(BattleState.PLAYERTURN);
    }

    //refreshes ui
    public void SetHUD()
    {
        //final attack meter
        if(metervalue >= 100)
        {
            metervalue = 100;
            finalAttackButton.SetActive(true);
        }
        else
        {
            finalAttackButton.SetActive(false);
        }

        float meterValue_ = metervalue / 100;
        meterToFill.fillAmount = meterValue_;

        //party ui
        for (int i = 0; i < partyMembers.Length; i++)
        {
            partyMemberName[i].text = partyMembers[i].name;
            partyMemberHP[i].text = "HP-" + partyMembers[i].health.ToString();
            partyMemberAP[i].text = "AP-" + partyMembers[i].ac.ToString();
            partyMemberHPBar[i].fillAmount = partyMembers[i].health / 100;
            partyMemberAPBar[i].fillAmount = partyMembers[i].ac / 100;

            if (partyMembers[i].status == "posioned")
            {
                partyMemberStatus[i].sprite = icons.Poison;
            }
            else if(partyMembers[i].status == "sleep")
            {
                partyMemberStatus[i].sprite = icons.Sleep;
            }
            else if (partyMembers[i].status == "none")
            {
                partyMemberStatus[i].sprite = icons.None;
            }
        }


        //enemy ui 
        enemyName.text = enemy.name;
        enemyHP.text = "HP-" + enemy.health.ToString();
        enemyHPBar.fillAmount = enemy.health / 100;

        if (enemy.status == "posioned")
        {
            enemyStatus.sprite = icons.Poison;
        }
        else if (enemy.status == "sleep")
        {
            enemyStatus.sprite = icons.Sleep;
        }
        else if (enemy.status == "none")
        {
            enemyStatus.sprite = icons.None;
        }

    }

    //message at battle
    void DisplayMessage(string text)
    {
        Debug.Log(text);
        var messageWindow = manager.Open((int)Windows.MessageWindow - 1, false) as MessageWindow;
        messageWindow.text = text;
    }

    IEnumerator MessageEnumarator(string Text)
    {
        DisplayMessage(Text);
        yield return new WaitForSeconds(0.1f);
    }

    //sets the players turn
    public void PlayerTurn()
    {

        //highlights party member name
        for (int i = 0; i < partyMembers.Length; i++)
        {
            if (partyMembers[i] == partyMembers[currentPartyMemberTurn])
            {
                partyMemberName[i].color = Color.green;
            }
            else
            {
                partyMemberName[i].color = Color.white;
            }

        }



    }

    //open menu of actions that are dependend on the characters actions in scriptable objects
    public void OnActionsButton(GameObject menu)
    {
        if (state != BattleState.PLAYERTURN)
            return;

        menu.SetActive(true);
        action1.SetButtons(partyMembers, currentPartyMemberTurn);
        action2.SetButtons(partyMembers, currentPartyMemberTurn);
        action3.SetButtons(partyMembers, currentPartyMemberTurn);
    }

    //what to do on attack / all are diffrent actions buttons will use this function
    public void OnAttackButton(int action)
    {
        if (state != BattleState.PLAYERTURN)
            return;

        //check if they have enough action points for it
        if (partyMembers[currentPartyMemberTurn].ac >= partyMembers[currentPartyMemberTurn].actions[action].apCost)
        {
            partyMembers[currentPartyMemberTurn].ac -= partyMembers[currentPartyMemberTurn].actions[action].apCost;
            ChangeState(BattleState.QTE);
            _actionInt = action;
        }
        else
        {
            StartCoroutine(MessageEnumarator(partyMembers[currentPartyMemberTurn].name + " has to litte action points"));
        }
    }

    //what to do on attack / all are diffrent actions buttons will use this function
    public void OnFinalAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        metervalue = 0;
        StartCoroutine(PlayerAttack(inventory.FinalAttack, partyMembers[currentPartyMemberTurn], enemy));
    }
     
    public void QTETime()
    {
        //pop up the qte
        QTEObject.SetActive(true);
    }

    public void QTEDamage(bool correct)
    {
        if (correct)
        {
            enemy.health -= 5;
            metervalue += 15;
            shakeManager.Shake(false, .5f, 1);
            SetHUD();
        }
        StartCoroutine(PlayerAttack(partyMembers[currentPartyMemberTurn].actions[_actionInt], partyMembers[currentPartyMemberTurn], enemy));
    }

    IEnumerator KeyPressing()
    {
        return null;
    }

    IEnumerator PlayerAttack(GenericBattleAction action, Actor target1, Actor target2)
    {
        return null;

    }

    public void EnemyAttack()
    {
        //we select a action for the enemy
        GenericBattleAction action = enemy.actions[(int)Random.Range(0, enemy.actions.Length)];

        //clear the potencialTargets in case players revived or something
        potencialTargets.Clear();
        comboList.Clear();

        if (enemy.status == "posioned")
        {
            Debug.Log("IM POSIONED");
            StartCoroutine(MessageEnumarator(enemy.name + " is posioned and takes 10 HP Damages"));
            enemy.health -= 10;
        }
        if (enemy.status == "sleep")
        {
            StartCoroutine(MessageEnumarator(enemy.name + " is sleeping and skips their turn"));
            enemy.status = "none";
            SetHUD();
            ChangeState(BattleState.PLAYERTURN);
            return;
        }

        //fill up the list of targets
        foreach (Actor partymember in partyMembers)
        {
            if (partymember.health > 0)
            {
                potencialTargets.Add(partymember);
                Debug.Log("added to list" + partymember);
            }
        }
        //select one to target with attack
        Actor target = potencialTargets[(int)Random.Range(0, potencialTargets.Count)];

        if (action.element == target.weaknessElement || action.property == target.weaknessProperty)
        {
            comboList.Add(action);
            foreach (GenericBattleAction attack in enemy.actions)
            {
                if (attack.element == action.element || attack.property == action.property)
                {
                    comboList.Add(attack);
                    break;
                }
            }
            action.EnemyComboAction(enemy, target, comboList, action.name);
        }
        else
        {
            //calculate and deal damage 
            comboList.Add(action);
            action.EnemyAction(enemy, target, action, action.name);
        }

        //sets value in hud
        SetHUD();

        Debug.Log(comboList);
        StartCoroutine(MessageEnumarator(action.ToString()));
        partyMembersActionsMenu.SetActive(false);

        //find the image of the party member to shoot the animation at
        GameObject targetTransform = GameObject.Find(target.characterObject.name + "(Clone)");

        //head to doing the animations for attacks
        StartCoroutine(AnimationProgress(comboList, targetTransform.transform, false));
    }


    //opens items menu
    public void OnItemsButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        //open menu of actions that are dependend on the characters actions in scriptable objects
        partyMembersItemsMenu.SetActive(true);
        item1.SetItemButtons(inventory, 0);
        item2.SetItemButtons(inventory, 1);
    }

    //check uses , define action depending on button
    public void OnItemsButtonPress(int action)
    {
        if (state != BattleState.PLAYERTURN)
            return;

        if (inventory.items[action].uses <= 0)
        {
            StartCoroutine(MessageEnumarator("There's no " + inventory.items[action].name + " in the parties inventory"));
        }
        else
        {
            partyMembersSelecionMenu.SetActive(true);
            item_ = inventory.items[action];
            memberButton1.SetButtonsTargets(partyMembers[0]);
            memberButton2.SetButtonsTargets(partyMembers[1]);
            memberButton3.SetButtonsTargets(partyMembers[2]);
        }

    }

    public void SelectTargetItem(int partyMember)
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(UsingItem(item_, partyMembers[partyMember]));
    }

    IEnumerator UsingItem(Item item, Actor target)
    {
        //increas values
        target.health += item.healthAmount;
        target.ac += item.apAmount;

        //check so we dont go over max values
        if (target.health > target.maxHealth)
        {
            target.health = target.maxHealth;
        }

        if (target.ac > target.maxAc)
        {
            target.ac = target.maxAc;
        }

        item.uses -= 1;

        //sets value in hud
        SetHUD();
        shakeManager.Shake(true, .5f, 1);

        StartCoroutine(MessageEnumarator(target.name + " feels energized"));
        partyMembersItemsMenu.SetActive(false);
        partyMembersSelecionMenu.SetActive(false);

        yield return new WaitForSeconds(2f);

        //move turn counter
        if (currentPartyMemberTurn >= 2)
        {
            currentPartyMemberTurn = 0;
        }
        else
        {
            currentPartyMemberTurn++;
        }

        //set battle state
        ChangeState(BattleState.ENEMYTURN);
    }


    //what to do on attack / all are diffrent actions buttons will use this function
    public void OnRunButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(OnRun());
    }

    //run command 
    IEnumerator OnRun()
    {

        var chance = Random.Range(0, 1f);
        if (chance < runOdds)
        {
            StartCoroutine(MessageEnumarator("You where able to run away"));
            yield return new WaitForSeconds(2);
            CloseWindow();
            if (battleOverCallback != null)
                battleOverCallback(partyMembers[0].alive);
        }
        else
        {
            StartCoroutine(MessageEnumarator("You where not able to run away"));
            yield return new WaitForSeconds(2);
            ChangeState(BattleState.ENEMYTURN);
        }
    }

    
    //animation phase 
    IEnumerator AnimationProgress(List<GenericBattleAction> attack,Transform enemySpawn_, bool playerturn)
    {

        if (playerturn)
        {
            for (int i = 0; i < attack.Count; i++)
            {
                GameObject attackobject;
                attackobject = Instantiate(attack[i].AnimationHolder, enemySpawn_);
                shakeManager.Shake(false, .5f, 1);
                //find the animator
                Animator _anim = attackobject.GetComponent<Animator>();

                //get the animation clip
                AnimatorClipInfo[] CurrentClipInfo;
                CurrentClipInfo = _anim.GetCurrentAnimatorClipInfo(0);
                //apply it to a float
                float lenght = CurrentClipInfo[0].clip.length;
                yield return new WaitForSeconds(lenght + 0.3f);
            }

            //make this wait the full float
            yield return new WaitForSeconds(2);
            //change state based on what happen enemy alive = enemy turn / enemy dead = end battle
            if (enemy.health <= 0)
            {
                ChangeState(BattleState.WON);
            }
            else
            {
                ChangeState(BattleState.ENEMYTURN);
            }
        }
        else
        {
            for (int i = 0; i < attack.Count; i++)
            {
                Instantiate(attack[i].AnimationHolder, enemySpawn_);
                shakeManager.Shake(true, .5f, 1);
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForSeconds(2);
            ChangeState(BattleState.PLAYERTURN);
        }
    }

    IEnumerator OnBattleOver()
    {
        
        var message = (partyMembers[0].alive ? "The team has won the battle. And gains " + enemy.goldReward + " gold ": enemy.name);
        inventory.money += enemy.goldReward;

        yield return new WaitForSeconds(1);
        DisplayMessage(message);
        yield return new WaitForSeconds(1);
        

        if (battleOverCallback != null)
            battleOverCallback(partyMembers[0].alive);

        BGM.ChangeBGM(false);
        CloseWindow();
    }

    void CloseWindow()
    {
        player.canMove = true;
        Destroy(PartymemberImage1);
        Destroy(PartymemberImage2);
        Destroy(PartymemberImage3);
        Destroy(enemyImage);
        this.Close();
    }

    public void BackOutMenu(GameObject menu)
    {
        menu.SetActive(false);
    }


}
