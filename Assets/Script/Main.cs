using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using RenderHeads.Media.AVProVideo;

public class Main : MonoBehaviour {

    [Header("Setting")]
    public int trigger_top = 80;
    public int trigger_buttom = 100;

    [Header("GameObject")]
    public GameObject[] canvas;
    public GameObject open;

    [Header("Media Player")]
    public MediaPlayer[] movie_open;
    public MediaPlayer[] movie_loop;
    public MediaPlayer[] movie_ending;
    public MediaPlayer[] movie_other;
    public MediaPlayer acopy;

    [Header("Bool")]
    public bool showDebugLog = true;
    public bool door_status = false;
    public bool isplaying = false;

    [SerializeField]
    private int index = 0;
    private static int val=5000;
    private static string message;
    State state;
    State _state;

    // Use this for initialization
    void Start () {
        state = State.M1;
        
	}
	
	// Update is called once per frame
	void Update () {
        StateSwich();
        DoorStatus();
        StateChange();

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isplaying) isplaying = false;
            else isplaying = true;
        }
        //if (isplaying) acopy.Play();
        //else if (!isplaying) acopy.Pause();

        //Debug.Log((int)acopy.Control.GetCurrentTimeMs()/1000);
        
    }
    enum State {
        M1, M2, M3
    };

    public void StateChange()
    {
            switch (state)
            {
                case State.M1:
                    open.GetComponent<DisplayUGUI>()._mediaPlayer = movie_open[index];
                    canvas[0].SetActive(true);
                    canvas[1].SetActive(false);
                    canvas[2].SetActive(false);
                    //alphaInit();
                    break;

                case State.M2:
                    canvas[0].SetActive(false);
                    canvas[1].SetActive(true);
                    canvas[2].SetActive(false);
                    val = getdis_ethernet.value;
                    FoodTrigger(val);
                    break;

                case State.M3:
                    canvas[0].SetActive(false);
                    canvas[1].SetActive(false);
                    canvas[2].SetActive(true);
                    break;

            }
    }

    void alphaInit()
    {
        //canvas alpha init
        //M1:0 M2:1 M3-1:0 M3-2:0
    }

    void StateSwich()
    {
        if (Input.GetKeyDown(KeyCode.Q)) state = State.M1;
        if (Input.GetKeyDown(KeyCode.W)) state = State.M2;
        if (Input.GetKeyDown(KeyCode.E)) state = State.M3;
    }

    void DoorStatus()
    {
        if(message != MessageListenerF.message)
        {
            if (message == "0")
            {
                door_status = false;
                state = State.M1;
                if (index < 2) index++;
                else index = 0;
                //movie_open[index].Pause();
                movie_open[index].Rewind(true);
                Debug.Log("door close.");
            }
            else if (message == "1")
            {
                door_status = true;
                movie_open[index].Play();
                Debug.Log("door open.");
            }
        }
        message = MessageListenerF.message;
        
        //Debug.Log(message);
    }
    void FoodTrigger(int val) {
        int distance = val ;
        if (distance > trigger_top && distance < trigger_buttom)
        {
            Debug.Log(string.Format("<color=lightblue>Get distance: {0}cm!</color>", val));
            state = State.M3;
        }
    }

    void SetMovie()
    {
        List<Movie> open = new List<Movie>();
        List<Movie> loop = new List<Movie>();
        List<Movie> ending = new List<Movie>();

        for(int i = 0; i < movie_open.Length; i++)
        {
            open.Add(new Movie(movie_open[i].name, i));
        }
        for(int i = 0; i < movie_loop.Length; i++)
        {
            loop.Add(new Movie(movie_loop[i].name, i));
        }
        for(int i = 0; i < movie_ending.Length; i++)
        {
            ending.Add(new Movie(movie_ending[i].name, i));
        }
    }


}
