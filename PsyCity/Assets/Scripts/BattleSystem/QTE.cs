using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTE : MonoBehaviour
{
    public string[] inputName;
    public string[] inputkey;
    public GameObject QTEObject;
    Image QTEImage;
    public Text QTEText;
    public GameObject damageObject;
    public Text damageText;

    Input correctInput;
    int qteGen;

    public NewBattleSystem battleSys;

    public float timer;
    bool coroutineBuzy;

    int randomNumber;

    // Start is called before the first frame update
    void OnEnable()
    {
        randomNumber = Random.Range(0, inputName.Length);
        QTEText.text = inputkey[randomNumber];
        QTEImage = QTEObject.GetComponent<Image>();
        QTEObject.SetActive(true);
        qteGen = 1;
        timer = 1;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        QTEImage.fillAmount = timer;
        if (timer > 0)
        {
            if (qteGen == 1 && Input.anyKeyDown)
            {//change with string array
                if (Input.GetButtonDown(inputName[randomNumber]))
                {
                    StartCoroutine(Correct());
                }
                else
                {
                    StartCoroutine(InCorrect());
                }
            }
        }
        else if(!coroutineBuzy)
        {
            StartCoroutine(InCorrect());
        }
        
    }

    IEnumerator Correct()
    {
        QTEObject.SetActive(false);
        coroutineBuzy = true;
        battleSys.QTEDamage(true);
        qteGen = 2;
        damageObject.SetActive(true);
        damageText.text = "- 5";
        yield return new WaitForSeconds(1);
        damageObject.SetActive(false);
        coroutineBuzy = false;
        this.gameObject.SetActive(false);
    }

    IEnumerator InCorrect()
    {
        QTEObject.SetActive(false);
        coroutineBuzy = true;
        battleSys.QTEDamage(false);
        qteGen = 2;
        damageObject.SetActive(true);
        damageText.text = "miss!!";
        yield return new WaitForSeconds(1);
        damageObject.SetActive(false);
        coroutineBuzy = false;
        this.gameObject.SetActive(false);
    }
}
