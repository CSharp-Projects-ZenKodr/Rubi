using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentManager : MonoBehaviour
{
    public static PersistentManager Instance { get; private set; }

    public static Stats currentstats { get; set; }
    public static Stats lastSavedStats { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            currentstats = new Stats();
            lastSavedStats = new Stats();
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }

        
    }

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        




    }

    public static void updateCurrentStats(Stats s)
    {
        currentstats.updateAllStats(s);

    }

    public static void saveStats(Stats s)
    {
        lastSavedStats.updateAllStats(s);
    }

    public static void reloadStats()
    {
        updateCurrentStats(lastSavedStats);
    }    
}
