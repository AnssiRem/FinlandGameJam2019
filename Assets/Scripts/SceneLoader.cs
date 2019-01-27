using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public int untilEvent = 10;

    public Vector2 BetweenRange;
    public Vector3 SpawnPoint;

    public GameObject currentPrefab;
    public GameObject[] PrefabsToUse;

    private bool hasOpened = false;
    private bool gameOver = false;
    private int currentPrefabId;

    private List<GameObject> prefabList;

    private FridgeDoor door;

    private void Start()
    {
        //Get ref to door scirpt
        if (!door)
        {
            door = gameObject.GetComponent<FridgeDoor>();
        }
        else
        {
            Debug.LogError("FridgeDoor script not found! Make sure it's on the same object!");
        }

        PoolPrefabs();
    }

    private void Update()
    {
        //Game loop
        if (!gameOver)
        {
            switch (door.DoorState)
            {
                case FridgeDoor.State.Closed:
                    if (hasOpened)
                    {
                        if (!currentPrefab)
                        {
                            hasOpened = false;
                            --untilEvent;

                            if (untilEvent == 0)
                            {
                                currentPrefabId = Random.Range(0, prefabList.Count);
                                currentPrefab = prefabList[currentPrefabId];
                                currentPrefab.SetActive(true);
                            }
                        }
                        else
                        {
                            hasOpened = false;
                            untilEvent = (int)Random.Range(BetweenRange.x, BetweenRange.y);

                            Destroy(currentPrefab);
                            currentPrefab = null;

                            prefabList.Remove(prefabList[currentPrefabId]);
                        }
                    }
                    break;

                case FridgeDoor.State.Slighly:
                    break;

                case FridgeDoor.State.Open:
                    if (!hasOpened) hasOpened = true;
                    break;

                default:
                    Debug.LogError("Door's cronked yo!");
                    break;
            }


            if (prefabList.Count == 0)
            {
                gameOver = true;
                door.GameOver();
            }
        }

    }

    private void PoolPrefabs()
    {
        prefabList = new List<GameObject>();

        for (int i = 0; i < PrefabsToUse.Length; i++)
        {
            GameObject temp = Instantiate(PrefabsToUse[i], SpawnPoint, Quaternion.identity);
            temp.SetActive(false);
            prefabList.Add(temp);
        }
    }
}
