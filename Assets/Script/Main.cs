using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    [Header("Setting")]
    public int trigger_top = 80;
    public int trigger_buttom = 100;

    [Header("GameObject")]
    public GameObject[] canvas;
    public GameObject[] movie_open;
    public GameObject[] movie_loop;
    public GameObject[] movie_ending;
    public GameObject[] movie_other;

    [Header("Bool")]
    public bool showDebugLog = true;
    public bool door_status = false;

    private int index = 0;
    private static int val=5000;
    private static string message;
    State state;
    

    // Use this for initialization
    void Start () {
        state = State.M1;
	}
	
	// Update is called once per frame
	void Update () {
        StateSwich();
        DoorStatus();

        switch (state)
        {
            case State.M1:
                canvas[0].SetActive(true);
                canvas[1].SetActive(false);
                canvas[2].SetActive(false);
                break;

            case State.M2:
                canvas[0].SetActive(false);
                canvas[1].SetActive(true);
                canvas[2].SetActive(false);
                break;

            case State.M3:
                canvas[0].SetActive(false);
                canvas[1].SetActive(false);
                canvas[2].SetActive(true);
                break;

        }

        if (state == State.M2)
        {
            val = getdis_ethernet.value;
            FoodTrigger(val);
        }
            

        
    }
    enum State {
        M1, M2, M3
    };

    void StateSwich()
    {
        if (Input.GetKeyDown(KeyCode.Q)) state = State.M1;
        if (Input.GetKeyDown(KeyCode.W)) state = State.M2;
        if (Input.GetKeyDown(KeyCode.E)) state = State.M3;
    }

    void DoorStatus()
    {
        message = MessageListenerF.message;
        if (message == "1") {
            door_status = false;
            state = State.M1;
        } 
        else if (message == "0") door_status = true;

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
