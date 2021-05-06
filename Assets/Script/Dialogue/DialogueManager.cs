using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class DialogueManager : MonoBehaviour
{ 	
	[SerializeField] TMP_Text instruction, dialogue;

    public Queue<string> sentences;
    public float nextLineCountdown;
    float countDownTimer;

    AudioSource dialogueAudio;

    [SerializeField] AudioClip Asiimov, Mock;
    [SerializeField] ThirdPersonController player;
    [SerializeField] GameObject GUI;
    [SerializeField] CinemachineVirtualCameraBase camera;

    bool endDialogue;

    Coroutine textCoroutine;

    [Range(1f,5)]
    [SerializeField] float dialogueSpeed;
    [SerializeField] float dialogueSpeedBaseNumber;


    void Start () {
    	dialogueAudio = GetComponent<AudioSource>();
    	sentences = new Queue<string>();
    	countDownTimer = nextLineCountdown;

    	instruction.text = "";
    	dialogue.text = "";

        dialogue.transform.parent.gameObject.SetActive(false);
        instruction.gameObject.SetActive(false);
    }

    void Update () {
    	if (sentences.Count >= 0) {

            if (Input.GetKeyDown("escape"))
            {
                dialogueAudio.Pause();
            }
            else if(Input.GetKeyDown(KeyCode.E) && !endDialogue) { // press e to go next
                DisplayNextSentence();
            }

            
    	}
        
        // limited time display instruction
        if (instruction.gameObject.activeSelf) {
            countDownTimer -= Time.deltaTime;
            if (countDownTimer <= 0) {
                instruction.gameObject.SetActive(false);
            }
        }

        
    }

    public void StartDialogue(Dialogue dialogues) {
        // clear camera
        if (camera != null) {
            Destroy(camera.gameObject);
            camera = null;
        }

        endDialogue = false;
        instruction.gameObject.SetActive(false);

        // lost control on player while play dialogue
        player.LostControl(99999, false);
        player.Stop();

    	sentences.Clear();
        GUI.SetActive(false);

        dialogue.transform.parent.gameObject.SetActive(true);

        // camera guide
        if (dialogues.camera != null) {
            camera = dialogues.camera;
            camera.Priority = 99;
        }

        instruction.text = dialogues.instruction;

    	foreach (string sentence in dialogues.sentences) {
    		sentences.Enqueue(sentence);
    	}

    	DisplayNextSentence();
    }

    public void DisplayNextSentence() {
    	if (sentences.Count <= 0) {
    		EndDialogue();
    		
    		return;
    	}

    	string sentence = sentences.Dequeue();

    	if (sentence[0] == 'A') {
    		dialogueAudio.clip = Asiimov;
    		dialogueAudio.Play();
    	}
    	else if (sentence[0] == 'M') {
    		dialogueAudio.clip = Mock;
    		dialogueAudio.Play();
    	}

        if (textCoroutine != null) StopCoroutine(textCoroutine);
    	textCoroutine = StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence) {
        dialogue.text = "";

        foreach(char letter in sentence.ToCharArray()) {
            dialogue.text += letter;
            yield return new WaitForSeconds(dialogueSpeed * dialogueSpeedBaseNumber);
        }
    }

    void EndDialogue() {
    	dialogueAudio.Stop();
        dialogue.text = "";
        dialogue.transform.parent.gameObject.SetActive(false);

        player.BackControl();
        countDownTimer = nextLineCountdown;

        GUI.SetActive(true);
        instruction.gameObject.SetActive(true);

        endDialogue = true;

        // camera guide
        if (camera != null) {
            camera.Priority = 0;
        }
    }
}
