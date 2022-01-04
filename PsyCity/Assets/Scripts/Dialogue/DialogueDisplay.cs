using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDisplay : MonoBehaviour
{
    public Conversation conversation;

    public GameObject speakerLeft;
    public GameObject speakerRight;

    private SpeakerUI speakerUILeft;
    private SpeakerUI speakerUIRight;

    private int ActiveLineIndex = 0;

    public bool inDialogueZone;
    public GameObject interactionUI;

    PlayerMovement player;


    // Start is called before the first frame update
    void Start()
    {
        speakerUILeft = speakerLeft.GetComponent<SpeakerUI>();
        speakerUIRight = speakerRight.GetComponent<SpeakerUI>();

        speakerUILeft.Speaker = conversation.speakerLeft;
        speakerUIRight.Speaker = conversation.speakerRight;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inDialogueZone)
        {
            AdvanceConversation();
            interactionUI.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !inDialogueZone)
        {
            inDialogueZone = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            interactionUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inDialogueZone = false;
            interactionUI.SetActive(false);
        }
    }

    void AdvanceConversation()
    {
        

        if(ActiveLineIndex < conversation.lines.Length)
        {
            DisplayLine();
            ActiveLineIndex += 1;
            player.canMove = false;
        }
        else
        {
            speakerUILeft.Hide();
            speakerUIRight.Hide();
            ActiveLineIndex = 0;
            player.canMove = true;
        }
    }

    void DisplayLine()
    {
        Line line = conversation.lines[ActiveLineIndex];
        Character character = line.character;

        if (!character.rightSpeaker)
        {
            speakerUILeft.Speaker = character;
            SetDialog(speakerUILeft, speakerUIRight, line.text);
        }
        else
        {
            speakerUIRight.Speaker = character;
            SetDialog(speakerUIRight, speakerUILeft, line.text);
        }
    }

    void SetDialog(SpeakerUI activeSpeaker, SpeakerUI inactiveSpeaker, string text)
    {
        activeSpeaker.Dialog = text;
        activeSpeaker.Show();
        inactiveSpeaker.Hide();
    }
}
