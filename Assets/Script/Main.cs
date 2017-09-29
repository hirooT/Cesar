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
    public static bool msgarrive = false;

    [SerializeField]
    private int index = 0;
    private static int val=5000;
    private static string message;
    private int firststart = 0;
    int flag = 0;
    State state;
    State _state;

    // Use this for initialization
    void Start () {
        alphaInit();
        state = State.M1;
        StateChange();
    }
	
	// Update is called once per frame
	void Update () {
        StateSwich();
        if (msgarrive)
        {
            DoorStatus();
        }
        
        //
        
        if(state == State.M1)
        {
            if (movie_open[index].Control.IsPlaying())
            {
                if(canvas[0].GetComponent<CanvasGroup>().alpha != 1)
                    StartCoroutine(FadeIn(canvas[0]));
            }
            if ((int)movie_open[index].Control.GetCurrentTimeMs()/1000 > 60 )
            {
                StartCoroutine(FadeOut(canvas[0]));     
            }
            if (movie_open[index].Control.IsFinished())
            {
                state = State.M2;
                StateChange();
            }
        }

        //Debug.Log((int)acopy.Control.GetCurrentTimeMs()/1000);
        /*
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isplaying) isplaying = false;
            else isplaying = true;
        }
        */
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
                    //canvas[0].GetComponent<CanvasGroup>().alpha = 1;
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
        canvas[0].GetComponent<CanvasGroup>().alpha = 0;
        canvas[1].GetComponent<CanvasGroup>().alpha = 1;
        canvas[2].GetComponent<CanvasGroup>().alpha = 1;
    }

    void StateSwich()
    {
        if (Input.GetKeyDown(KeyCode.Q)) state = State.M1;
        if (Input.GetKeyDown(KeyCode.W)) state = State.M2;
        if (Input.GetKeyDown(KeyCode.E)) state = State.M3;
    }

    void DoorStatus()
    {
            if (message != MessageListenerF.message)
            {
                
                if (message == "0")
                    {
                        door_status = false;
                        state = State.M1;
                        StateChange();
                
                    
                        //movie_open[index].Pause();
                        alphaInit();
                        for (int i = 0; i < 3; i++)
                        {
                            movie_open[i].Rewind(true);
                        }

                    if (firststart > 0)
                    {
                        if (index < 2) index++;
                        else index = 0;
                    }
                    Debug.Log("door close.");
                    StateChange();
                    }
                else if (message == "1")
                {
                    door_status = true;
                    movie_open[index].Play();
                if (firststart == 0) firststart++;
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
            StateChange();
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

    IEnumerator FadeOut(GameObject canvas)
    {
        CanvasGroup _canvas = canvas.GetComponent<CanvasGroup>();
        _canvas.alpha -= Time.deltaTime * 1.0f;
        if (_canvas.alpha < 0)
        {
            _canvas.alpha = 0;
        }
        //Debug.Log(_canvas.alpha);
            
        yield return null;
    }
    IEnumerator FadeIn(GameObject canvas)
    {
        CanvasGroup _canvas = canvas.GetComponent<CanvasGroup>();
        _canvas.alpha += Time.deltaTime * 1.5f;
        if (_canvas.alpha > 1)
        {
            _canvas.alpha = 1;
        }
        //Debug.Log(_canvas.alpha);

        yield return null;
    }
}
