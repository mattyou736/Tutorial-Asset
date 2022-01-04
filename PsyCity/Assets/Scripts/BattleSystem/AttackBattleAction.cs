using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;


public class AttackBattleAction : GenericBattleAction
{

    public override void Action(Actor target1, Actor target2 , GenericBattleAction attack, string actionName, Transform enemySpawn)
    {
        
    }

    //add actions to array 
    public override void ComboAction(Actor target1, Actor target2, List<GenericBattleAction> attack, string actionName, Transform enemySpawn)
    {
        
    }

    public override void EnemyAction(Actor target1, Actor target2, GenericBattleAction attack, string actionName)
    {
        var attackValue = (int)Random.Range(min: attack.baseDamage.x, max: attack.baseDamage.y);
        var sb = new StringBuilder();
        var hitChance = (int)Random.Range(min: attack.missChance.x, max: attack.missChance.y);



        if (hitChance > 0)
        {
            if (attack.element == target2.resistance || attack.property == target2.resistance)
            {
                attackValue = attackValue / 2;
                sb.Append(" Resistand hit! ");
            }


            if (attackValue >= target1.attackRange.y - 1)
            {
                sb.Append(" Critical hit! ");
                attackValue += 5;
            }


            target2.DecreaseHealth(attackValue);
            sb.Append(target1.name);
            sb.Append(" uses  " + actionName + " ");
            sb.Append(target2.name);
            sb.Append(". ");
            sb.Append(target2.name);

            if (attack.posion && target2.status == "none" && hitChance > attack.effectHitChance)
            {
                target2.status = "posioned";
                sb.Append(" has become posioned. ");
            }
            if (attack.sleep && target2.status == "none" && hitChance > attack.effectHitChance)
            {
                target2.status = "sleep";
            }

            sb.Append("he takes ");
            sb.Append(attackValue);
            sb.Append(" hp damage.");

        }
        else
        {
            sb.Append(target1.name);
            sb.Append(" misses attack ");
        }

        attackValue_ = attackValue;
        message = sb.ToString();
    }

    public override void EnemyComboAction(Actor target1, Actor target2, List<GenericBattleAction> attack, string actionName)
    {
        int totalAttackValue = 0;
        var sb = new StringBuilder();
        var hitChance = (int)Random.Range(min: attack[0].missChance.x, max: attack[0].missChance.y);


        if (hitChance > 0)
        {

            if (hitChance >= target1.attackRange.y - 1)
            {
                sb.Append(" Critical hit! ");
                totalAttackValue += 10;
            }



            sb.Append("Weakness Hit!! " + target1.name + " combines its attacks: ");

            for (int i = 0; i < attack.Count; i++)
            {
                var attackValue = (int)Random.Range(min: attack[i].baseDamage.x, max: attack[i].baseDamage.y);

                if (attack[i].posion && target2.status == "none" && hitChance > attack[i].effectHitChance)
                {
                    target2.status = "posioned";
                }
                if (attack[i].sleep && target2.status == "none" && hitChance > attack[i].effectHitChance)
                {
                    target2.status = "sleep";
                }

                if (attack[i].element == target2.weaknessElement || attack[i].property == target2.weaknessProperty)
                {
                    attackValue = attackValue * 2;
                }

                if (attack[i].element == target2.resistance || attack[i].property == target2.resistance)
                {
                    attackValue = attackValue / 2;
                }
                totalAttackValue += attackValue;
            }

            target2.DecreaseHealth(totalAttackValue);
        }
        else
        {
            sb.Append(target1.name);
            sb.Append(" misses attack ");
        }

        attackValue_ = totalAttackValue;
        message = sb.ToString();
    }
}
