using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    [Header("Global Settings")]
    [Tooltip("The tick rate for fire in milliseconds")]
    public int fireDamagePerTick;
    public int fireTickRate;
    public int vitalityTickRate;

    [Header("Garbage Disposal")]
    public Transform garbageInHierarchy;
    public int garbageDisposalSize;
    public int garbageDisposalTimer;
    public List<PlayerController> activePlayers { get; private set; }
    
    // A list of game objects to destroy over time
    public List<GameObject> garbageDisposal { get; private set; }
    private int _garbageCount;

    private static GameManager _instance;

    // Public property to access the GameManager instance
    public static GameManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        InitializeInstance();
        InitializeGarbageDisposal();
    }

    void InitializeInstance()
    {
        // Ensure only one instance of the GameManager exists
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            // Make sure the GameManager persists between scene changes
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void InitializeGarbageDisposal()
    {
        garbageDisposal = new List<GameObject>();
    }

    private void Start()
    {
        activePlayers = new List<PlayerController>();
        _instance.activePlayers.Add(FindObjectOfType<PlayerController>());
    }

    public void SendToGarbage(GameObject obj)
    {
        Debug.Log("Sending " + obj.name + " to garbage...");

        obj.transform.SetParent(garbageInHierarchy, true);

        garbageDisposal.Add(obj);
        Debug.Log("Garbage Disposal Count: " + garbageDisposal.Count);

        if (garbageDisposal.Count == garbageDisposalSize)
        {
            GameObject trash = garbageDisposal[0];
            Debug.Log("Garbage is full. Removing and Deleting " 
                + obj.name + ". GD Count: " + garbageDisposal.Count);
            garbageDisposal.RemoveAt(0);
            Destroy(trash);

            /*
            GameObject trash = garbageInHierarchy.GetChild(0).gameObject;
            Debug.Log("Garbage is full. Removing and Deleting " + trash.name);
            Destroy(trash, 3);
            _garbageCount--;
            */
        }
    }

    public void EmptyGarbage()
    {
        int count = garbageDisposal.Count;

        for (int i = 0; i < count; i++)
        {
            GameObject trash = garbageDisposal[i];
            garbageDisposal.RemoveAt(i);
            Destroy(trash, garbageDisposalTimer);
            i--; // Decrement the index since we removed an element
            count--; // Decrement the count since we removed an element
        }
    }

    private async void Update()
    {
        
    }
}