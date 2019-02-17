﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimingAndScore : MonoBehaviour {

    public float timer; // Timer for timing
    private float hitObjectStartTime; // The time when the note spawns
    private float perfectJudgementTime; // The end time for perfect hit
    private float earlyJudgementTime; // The early time
    private float destroyedTime; // The late time, last possible hit time before input is cancelled
    private int earlyScore; // The value of the early hits for score
    private int perfectScore; // The value of the perfect hits for score
    private int goodScore; // The value of the good hits for score
    public int playerTotalScore; // Total score for the player
    private int combo; // Total combo
    public Text timeWhenHitText; // Time when the user pressed down the key and hit a note
    public bool hitObjectHit; // Has the square been hit
    public AudioSource clickSound; // The sound that plays when the button is pressed
    public GameObject explosion; // Particle system object
    private float timeWhenHit; // The time when the object is hit
    public Animator scoreAnimation; // Animate the score text
    public Animator comboAnimation; // Animate the combo text
    public Animator judgementAnimation; // Animate the judgement text

    private ScoreManager scoreManager;


    // Use this for initialization
    void Start () {

        scoreManager = FindObjectOfType<ScoreManager>();


        // Initalize hit object
        hitObjectStartTime = 0f; 

        // Initialize judgements
        earlyJudgementTime = 0.4f;
        perfectJudgementTime = 0.8f;
        destroyedTime = 1.2f;
        combo = 0;
        hitObjectHit = false;

        // Initialize scores
        earlyScore = 1000; 
        perfectScore = 5000; 
        goodScore = 2500; 

        playerTotalScore = 0;


        timeWhenHit = 0;
	}
	
	// Update is called once per frame
	void Update () {

        // The timer increments per frame
        timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Timing check to calculate the type of hit on timing (perfect, miss)

            if (hitObjectHit == false)
            {
                // CHECK IF PLAYER HIT EARLY
                if (timer >= hitObjectStartTime && timer <= earlyJudgementTime)
                {
                    hitObjectHit = true; // The square has been hit and further judgement is disabled

                    // particles.Play(); // Play the particle animation

                    Instantiate(explosion, transform.position, Quaternion.Euler(90, 0, -45)); // Instantiate particle system


                    clickSound.Play(); // Play the click sound effect

                    scoreManager.AddJudgement("EARLY"); // Sets judgement to early

                    combo++; // Increase combo
                    scoreManager.AddCombo(combo); // Send current combo to update the UI text

                    playerTotalScore += earlyScore; // Increase score
                    scoreManager.AddScore(playerTotalScore); // Pass to score manager to update text

                    timeWhenHit = timer; // Get the time when hit
                    timeWhenHitText.text = "Time When Hit: " + timeWhenHit.ToString(); // The time when the user hit the note

                    // Animates UI Text
                    scoreAnimation.Play("GameplayTextAnimation");
                    comboAnimation.Play("GameplayTextAnimation");
                    judgementAnimation.Play("GameplayTextAnimation");
                }

                // CHECK IF PLAYER HIT GOOD
                if (timer >= earlyJudgementTime && timer <= perfectJudgementTime)
                {
                    hitObjectHit = true; // The square has been hit and further judgement is disabled

                    //particles.Play(); // Play the particle animation

                    Instantiate(explosion, transform.position, Quaternion.Euler(90, 0, -45)); // Instantiate particle system


                    clickSound.Play(); // Play the click sound effect

                    scoreManager.AddJudgement("GOOD"); // Sets judgement to early

                    combo++; // Increase combo
                    scoreManager.AddCombo(combo); // Send current combo to update the UI text

                    playerTotalScore += goodScore; // Add early score value to the players current score
                    scoreManager.AddScore(playerTotalScore); // Pass to score manager to update text

                    timeWhenHit = timer; // Get the time when hit
                    timeWhenHitText.text = "Time When Hit: " + timeWhenHit.ToString(); // The time when the user hit the note

                    // Animates UI Text
                    scoreAnimation.Play("GameplayTextAnimation");
                    comboAnimation.Play("GameplayTextAnimation");
                    judgementAnimation.Play("GameplayTextAnimation");
                }

                // CHECK IF PLAYER HIT GOOD
                if (timer >= perfectJudgementTime && timer <= destroyedTime)
                {
                    hitObjectHit = true; // The square has been hit and further judgement is disabled

                    Instantiate(explosion, transform.position, Quaternion.Euler(90, 0, -45)); // Instantiate particle system

                    clickSound.Play(); // Play the click sound effect

                    scoreManager.AddJudgement("PERFECT");

                    combo++; // Increase combo
                    scoreManager.AddCombo(combo); // Send current combo to update the UI text

                    playerTotalScore += perfectScore; // Add early score value to the players current score
                    scoreManager.AddScore(playerTotalScore); // Pass to score manager to update text

                    timeWhenHit = timer; // Get the time when hit
                    timeWhenHitText.text = "Time When Hit: " + timeWhenHit.ToString(); // The time when the user hit the note

                    // Animates UI Text
                    scoreAnimation.Play("GameplayTextAnimation");
                    comboAnimation.Play("GameplayTextAnimation");
                    judgementAnimation.Play("GameplayTextAnimation");

                }
            }
        }
    }
    
}