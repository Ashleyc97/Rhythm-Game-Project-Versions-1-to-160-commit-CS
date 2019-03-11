﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadAndRunBeatmap : MonoBehaviour {

    
    public SongProgressBar songProgressBar; // Required for song time for spawning

    float timer = 0f;

    public float spawnTime;
    private string hitObjectTag;

    public GameObject[] hitObject = new GameObject[7];
    public GameObject[] specialHitObject = new GameObject[7];
    public List<float> hitObjectPositionsX = new List<float>();
    public List<float> hitObjectPositionsY = new List<float>();
    public List<float> hitObjectPositionsZ = new List<float>();
    public List<Vector3> hitObjectPositions = new List<Vector3>();
    public List<float> hitObjectSpawnTimes = new List<float>();
    public List<int> hitObjectType = new List<int>();
    public List<GameObject> spawnedList = new List<GameObject>();
    public Vector3 hitObjectPosition;
    private int hitObjectID;
    private int earliestIndex; // The current earliest note
    public bool hasHit;
    private bool startCheck;
    private int sizeOfList;
    private int nextIndex;
    private bool justHit = false;
    public float songTimer;
    public float specialTimeStart;
    public float specialTimeEnd;
    public SpecialTimeManager specialTimeManager;
    public bool isSpecialTime;
    public bool startSongTimer;
    public Animator pressPlayAnimator; // Animates the Press Play Text at the start of the song
    public Text pressPlayText; // The Press Play text at the start of the song

    void Awake()
    {
        // Load beatmap
        Database.database.Load();
    }

    // Use this for initialization
    void Start () {
        songProgressBar = FindObjectOfType<SongProgressBar>();
        specialTimeManager = FindObjectOfType<SpecialTimeManager>();
        isSpecialTime = false;
        songTimer = 0;
        startSongTimer = false;
        earliestIndex = 0;
        hasHit = false;
        startCheck = false;
        sizeOfList = 0;
        nextIndex = 0;
        hitObjectID = 0;

        // Load the hit object positions first into their own list
        hitObjectPositionsX = Database.database.LoadedPositionX;
        hitObjectPositionsY = Database.database.LoadedPositionY;
        hitObjectPositionsZ = Database.database.LoadedPositionZ;
        // Now merge together for vector3 positions
        for (int i = 0; i < hitObjectPositionsX.Count; i++)
        {
            // Create the new position for inserting to the list
            hitObjectPosition = new Vector3(hitObjectPositionsX[i], hitObjectPositionsY[i], hitObjectPositionsZ[i]);
            // Add the position of all the values into the list of positions used for spawning
            hitObjectPositions.Add(hitObjectPosition);
        }
        // Get the spawn times and insert into the list
        hitObjectSpawnTimes = Database.database.LoadedHitObjectSpawnTime;

        
        // Update the spawn times to match when they should be clicked (1 second earlier)
        for (int i = 0; i < hitObjectSpawnTimes.Count; i++)
        {
            // Remove 1 second from each of the loaded spawn times
            hitObjectSpawnTimes[i] = hitObjectSpawnTimes[i] - 1;
        }

        // Load the hit object types
        hitObjectType = Database.database.LoadedObjectType;
    }
	
	// Update is called once per frame
	void Update () {

        // Load special time start
        specialTimeStart = specialTimeManager.specialTimeStart;
        // Load special time end
        specialTimeEnd = specialTimeManager.specialTimeEnd;

        
        // If the space key has been pressed we start the song and song timer
        if (Input.GetKeyDown(KeyCode.Space))
        {
            startSongTimer = true;
            // Animate the press play text at the start of the song
            StartCoroutine(AnimatePressPlayText());
        }

        if (startSongTimer == true)
        {
            // Update the song timer with the current song time
            songTimer += Time.deltaTime;
        }


        // Check if it's special time 
        CheckSpecialTime();

        // Spawn normal notes if not special time
        if (isSpecialTime == false)
        {
            if (songTimer >= hitObjectSpawnTimes[hitObjectID])
            {
                SpawnHitObject(hitObjectPositions[hitObjectID], hitObjectType[hitObjectID]);
                hitObjectID++;
            }
        }
        // Spawn special notes if special time
        else if (isSpecialTime == true)
        {
            if (songTimer >= hitObjectSpawnTimes[hitObjectID])
            {
                SpawnSpecialHitObject(hitObjectPositions[hitObjectID], hitObjectType[hitObjectID]);
                hitObjectID++;
            }
        }


        
        
        // Size of list
        sizeOfList = spawnedList.Count;

        if (startCheck == true)
        {
            
            // If the index object exists
            if (spawnedList[earliestIndex] != null)
            {
                // Set the earliest hit object that has spawned to be the earliest for hit detection
                spawnedList[earliestIndex].GetComponent<TimingAndScore>().isEarliest = true;
            }
            // If the earliest object has been destroyed
            if (spawnedList[earliestIndex] == null)
            {
                // Check if another object has spawned
                if (nextIndex > earliestIndex)
                {
                    earliestIndex++;
                }
           }
            



            /*
            // If the index object doesn't exist and the size of the list is greater than the nextIndex required 
            if (spawnedList[earliestIndex] == null && sizeOfList > nextIndex)
            {
                earliestIndex++;
            }
            */
        }
        
    }

    // Spawn the hit object
    public void SpawnHitObject(Vector3 positionPass, int hitObjectTypePass)
    {
        spawnedList.Add(Instantiate(hitObject[hitObjectTypePass], positionPass, Quaternion.Euler(0, 45, 0)));
        startCheck = true;
        // Increment the highest index currently
        nextIndex++;
    }

    // Spawn special hit object during special time
    public void SpawnSpecialHitObject(Vector3 positionPass, int hitObjectTypePass)
    {
        spawnedList.Add(Instantiate(specialHitObject[hitObjectTypePass], positionPass, Quaternion.Euler(0, 45, 0)));
        startCheck = true;
        // Increment the highest index currently
        nextIndex++;
    }

    // Check if it's special time, if it is we spawn special time notes
    public void CheckSpecialTime()
    {
        if (specialTimeManager.isSpecialTime == true)
        {
            isSpecialTime = true;
        }
        if (specialTimeManager.isSpecialTime == false)
        {
            isSpecialTime = false;
        }
    }

    // Play the animation for the PressPlayText
    private IEnumerator AnimatePressPlayText()
    {
        pressPlayAnimator.Play("PressPlayTextAnimation");
        yield return new WaitForSeconds(0.10f);
        pressPlayText.enabled = false;
    }
}