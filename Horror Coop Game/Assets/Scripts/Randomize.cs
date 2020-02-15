using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Randomize : MonoBehaviour
{
    public GameObject player;
    public GameObject userInterface;
    public GameObject menuCamera;
    public GameObject randomSeedObject;
    public GameObject localPlayerObject;
    public GameObject playerCountObject;

    public int seed = 1;
    public int localPlayer = 1;
    public int playerCount = 1;
    public List<int> playersAvailable = new List<int>();
    public List<int> playerOrder = new List<int>();
    List<int> runesAvailable = new List<int>();
    List<int> runeOrder = new List<int>();

    void Start()
    {
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
                    tempPlayersAvailable.Add(j+1);
                }
                while (totalCount > 0) {
                    int randNum = Random.Range(0, tempPlayersAvailable.Count);
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

    public void StartGame() {
        seed = int.Parse(randomSeedObject.GetComponent<Text>().text);
        localPlayer = (int)localPlayerObject.GetComponent<Slider>().value;
        playerCount = (int)playerCountObject.GetComponent<Slider>().value;
        player.SetActive(true);
        userInterface.SetActive(false);
        menuCamera.SetActive(false);
    }
}
