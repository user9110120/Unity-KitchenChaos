using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class KitchenGameManager : MonoBehaviour
{
    

    public static KitchenGameManager Instance { get; private set; }

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePause;
    public event EventHandler OnGameUnPause;

    private enum State
    {
        WaitingToStart,
        CountDownToStart,
        GamePlaying,
        Gameover,
    }


    private State state;
    
    private float countdownToStartTimer = 3f;
    private float gamePlayingtTimer;
    private float gamePlayingtTimerMax = 360f;
    private bool isGamePaused = false;

    private void Awake()
    {
        Instance = this;

        state = State.WaitingToStart;
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (state == State.WaitingToStart)
        {
            state = State.CountDownToStart;
            OnStateChanged?.Invoke(this, new EventArgs());
        }
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                break;
            case State.CountDownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0f)
                {
                    state = State.GamePlaying;
                    gamePlayingtTimer = gamePlayingtTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gamePlayingtTimer -= Time.deltaTime;
                if (gamePlayingtTimer < 0f)
                {
                    state = State.Gameover;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.Gameover:
                break;

        }
        //Debug.Log(state);
    }

    public bool IsGamePlaying ()
    {
        return state == State.GamePlaying;
    }

    public bool IsCountdownToStartActive()
    {
        return state == State.CountDownToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer;
    }

    public bool IsGameOver()
    {
        return state == State.Gameover;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return 1 - (gamePlayingtTimer / gamePlayingtTimerMax);
    }

    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            Time.timeScale = 0f;

            OnGamePause?.Invoke(this,EventArgs.Empty);
        } else
        {
            Time.timeScale = 1f;

            OnGameUnPause?.Invoke(this, EventArgs.Empty);
        }

            
    }
}
