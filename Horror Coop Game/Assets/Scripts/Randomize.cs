using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Randomize : MonoBehaviour
{
    public GameObject player;
    public GameObject userInterface;
    public GameObject menuCamera;
    public GameObject crosshair;
    public GameObject randomSeedObject;
    public GameObject localPlayerObject;
    public GameObject playerCountObject;

    public int seed = 1;
    public int localPlayer = 1;
    public int playerCount = 1;
    List<int> playersAvailable = new List<int>();
    public List<int> playerOrder = new List<int>();
    List<int> runesAvailable = new List<int>();
    public List<int> runeOrder = new List<int>();
    public int currentRune = 0;

    public bool active = false;
    public float shortButtonCooldownWait = 0;
    public float longButtonCooldownWait = 0;
    public float startButtonCooldownWait = 0;
    public GameObject greenLight;
    public GameObject redLight;
    public GameObject yellowLight;
    public GameObject blackLight;

    public float counter = 0;
    public int lightStage = 0;
    public float timeToTurnOn = 0;
    public float timeToStayOff = 0;
    public AudioClip buttonSuccess;
    public AudioClip buttonFailed;

    public int currentSequenceIndex = 0;

    public GameObject[] runes;

    public int buttonId;
    public GameObject button;

    void Start() {
        StartCoroutine(StartButtonCooldown());
    }

    void Update() {
        if (playerOrder.Count > 0 && playerOrder[currentRune] == localPlayer) {
            counter += Time.deltaTime;
            if (lightStage == 0) {
                Color tmp = runes[runeOrder[currentRune]].GetComponent<SpriteRenderer>().color;
                tmp.a += 0.01f;
                runes[runeOrder[currentRune]].GetComponent<SpriteRenderer>().color = tmp;
                if (counter >= timeToTurnOn) {
                    counter = 0;
                    lightStage = 1;
                }
            } else if (lightStage == 1) {
                Color tmp = runes[runeOrder[currentRune]].GetComponent<SpriteRenderer>().color;
                tmp.a -= 0.01f;
                if (tmp.a >= 0.117f) {
                    runes[runeOrder[currentRune]].GetComponent<SpriteRenderer>().color = tmp;
                }
                if (counter >= timeToTurnOn) {
                    counter = 0;
                    lightStage = 2;
                }
            } else {
                if (counter >= timeToStayOff) {
                    counter = 0;
                    lightStage = 0;
                }
            }
        }
    }

    public void StartGame() {
        if (randomSeedObject.GetComponent<Text>().text != "") {
            seed = int.Parse(randomSeedObject.GetComponent<Text>().text);
        } else {
            seed = 1;
        }
        localPlayer = (int)localPlayerObject.GetComponent<Slider>().value;
        playerCount = (int)playerCountObject.GetComponent<Slider>().value;
        crosshair.SetActive(true);
        player.SetActive(true);
        userInterface.SetActive(false);
        menuCamera.SetActive(false);
        Random.InitState(seed);

        for (int i = 0; i < 8; i++) {
            runesAvailable.Add(i);
        }

        for (int i = 0; i < 8; i++) {
            int random = Random.Range(0, runesAvailable.Count);
            runeOrder.Add(runesAvailable[random]);
            runesAvailable.RemoveAt(random);
            Debug.Log(runeOrder[i]);
        }

        int currentNumber = 1;
        int totalCount = 8;
        for (int i = 0; i < 8; i++) {
            if (currentNumber == 1 && totalCount <= 8 % playerCount) {
                List<int> tempPlayersAvailable = new List<int>();
                for (int j = 0; j < playerCount; j++) {
                    Debug.Log("player: " + j);
                    tempPlayersAvailable.Add(j);
                }
                while (totalCount > 0) {
                    int randNum = Random.Range(0, tempPlayersAvailable.Count+1);
                    playersAvailable.Add(tempPlayersAvailable[randNum]);
                    tempPlayersAvailable.RemoveAt(randNum);
                    totalCount -= 1;
                }
                break;
            } else {
                playersAvailable.Add(currentNumber);
                if (currentNumber == playerCount) {
                    currentNumber = 1;
                } else {
                    currentNumber += 1;
                }
                totalCount -= 1;
            }
        }

        Debug.Log("Player Order");
        for (int i = 0; i < 8; i++) {
            int random = Random.Range(0, playersAvailable.Count);
            playerOrder.Add(playersAvailable[random]);
            playersAvailable.RemoveAt(random);
            Debug.Log(playerOrder[i]);
        }
    }

    public void NextNumber(int runeNumber) {
        if (runeNumber == runeOrder[currentSequenceIndex]) {
            Debug.Log("Success");
            currentSequenceIndex += 1;
        } else {
            Debug.Log("Died");
        }
    }

    public void ButtonPress(int butID, GameObject currentButton) {
        active = false;
        buttonId = butID;
        button = currentButton;
        StartCoroutine(Processing());
    }

    IEnumerator StartButtonCooldown() {
        yield return new WaitForSeconds(startButtonCooldownWait);
        active = true;
        greenLight.SetActive(true);
        blackLight.SetActive(false);
    }

    IEnumerator ShortButtonCooldown() {
        greenLight.SetActive(true);
        yield return new WaitForSeconds(1);
        greenLight.SetActive(false);
        blackLight.SetActive(true);
        yield return new WaitForSeconds(shortButtonCooldownWait);
        active = true;
        greenLight.SetActive(true);
        blackLight.SetActive(false);
    }

    IEnumerator LongButtonCooldown() {
        redLight.SetActive(true);
        yield return new WaitForSeconds(longButtonCooldownWait);
        active = true;
        greenLight.SetActive(true);
        redLight.SetActive(false);
    }

    IEnumerator Processing() {
        greenLight.SetActive(false);
        for (int i = 0; i < 5; i++) {
            blackLight.SetActive(false);
            yellowLight.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            blackLight.SetActive(true);
            yellowLight.SetActive(false);
            yield return new WaitForSeconds(0.3f);
        }
        blackLight.SetActive(false);
        if (buttonId == runeOrder[currentRune]) {
            button.GetComponent<AudioSource>().clip = buttonSuccess;
            button.GetComponent<AudioSource>().Play();
            Color tmp = runes[runeOrder[currentRune]].GetComponent<SpriteRenderer>().color;
            tmp.a = 0.117f;
            runes[runeOrder[currentRune]].GetComponent<SpriteRenderer>().color = tmp;
            lightStage = 0;
            counter = 0;
            currentRune += 1;
            button.GetComponent<ButtonPress>().SuccessfulPress();
            StartCoroutine(ShortButtonCooldown());
        } else {
            button.GetComponent<AudioSource>().clip = buttonFailed;
            button.GetComponent<AudioSource>().Play();
            StartCoroutine(LongButtonCooldown());
        }
    }
}
