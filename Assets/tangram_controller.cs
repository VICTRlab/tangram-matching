using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class tangram_controller : Photon.MonoBehaviour
{
    public TextMesh textObject;//temp for textbox of top right corner of tangrammodels
    public Animator anim;//temp object for animation
    public bool[] used;//array of booleans that indicate whether or not a model is confirmed
    private Stopwatch time; //timer for duration of a touch
    private Stopwatch gameTime;
    public GameObject button;
    string firstModel;//initial touch
    string finalModel;//end of touch
    int update;//the model count
    [SerializeField] private Text rpcMessage;
    int index;
    int numCorrect = 0;
    int numSelected = 0;
    int[] order = new int[16];
    private bool enter = false;
    private bool notAlreadyEntered = true;
    public List<GameObject> models = new List<GameObject>();
    public Vector3[] Pos = new Vector3[16];
    //int TapCount;
    //public float MaxDoubleTapTime = 0.25f;
    //float NewTime;
    //Use this for initialization
    public void Shuffle(int[] order)
    {
        System.Random rng = new System.Random();
        int n = order.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);

            int g = order[k];
            order[k] = order[n];
            order[n] = g;
        }
    }
    
    public void Start()
    {
        
        //for (int i = 0; i < order.Length; i++)
        //{
        //    order[i] = i + 1;
        //}
        //Shuffle(order);
        //TapCount = 0;
        time = new Stopwatch();
        gameTime = new Stopwatch();
        update = 0;
        index = 2;
        used = new bool[17];
        for (int i = 0; i < 17; i++)
        {
            used[i] = false;
        }
    }

    IEnumerator your_timer()
    {
        enter = true;
        //Debug.Log("Your enter Coroutine at" + Time.time);
        yield return new WaitForSeconds(5.0f);
        if (Results.player.Equals("Director"))
        {
            directorSeries();
            gameTime.Start();
        }
        notAlreadyEntered = false;
        enter = false;
    }

    IEnumerator down_animation()
    {
        yield return new WaitForSecondsRealtime(2.0f);
        GameObject.Find(firstModel).GetComponent<Animator>().Play("TNP_down");
    }

    /// <summary>
    /// Runs each frame 
    /// </summary>
    void Update()
    {
        //rpcMessage.text = GameObject.Find(15.ToString()).transform.localPosition.ToString("f8");
        if (notAlreadyEntered && enter == false) StartCoroutine(your_timer());
        handleTouchInput();
        rpcMessage.text = numSelected.ToString();
        if (numSelected == 12)
        {
            gameTime.Stop();
            PhotonView pvRes = GameObject.Find("image_target").GetComponent<PhotonView>();

            pvRes.RPC("resultFunction", PhotonTargets.AllBuffered,gameTime.ElapsedMilliseconds/1000);
            
            //exit
        }
        textObject = GameObject.Find("txtTNP").GetComponent<TextMesh>();
        textObject.text = update.ToString();
    }
    
    /// <summary>
    /// Waits for user to touch screen, sends RPC message if the touch is long enough
    /// </summary>
    public bool handleTouchInput()
    {
        bool result = false;
        if (Input.touchCount > 0)//if something touches the screen
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);//finds the position of the finger
            RaycastHit Hit;
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)//the moment when user touches the screen and stays there
            {
                //TapCount++;
                if (Physics.Raycast(ray, out Hit))//if something is touched
                {
                    time.Start(); //start the time
                    firstModel = Hit.transform.name;//get the model name
                    
                    if (firstModel.Equals("TNP"))
                    {
                        GameObject.Find(firstModel).GetComponent<Animator>().Play("TNP_up");
                    }

                    else if (!used[int.Parse(firstModel) - 1])//if not already confirmed
                    {
                            GameObject.Find(firstModel).GetComponent<Renderer>().material.color = Color.yellow;//change its colour to yellow for confirmation process 
                            GameObject.Find(firstModel).GetComponent<Animator>().Play(firstModel + "_up");
                    }
                }
            }
            if (touch.phase == TouchPhase.Ended)//finger lifted off
            {
                
                time.Stop();//stop the timer
                GameObject.Find(firstModel).GetComponent<Animator>().enabled = false;//Stop the animation playing
                GameObject.Find(firstModel).GetComponent<Animator>().enabled = true;//restart

                //RPC to other player with the name of the model ("1" through "16")
                //PhotonView pv = GameObject.Find("image_target").GetComponent<PhotonView>();
                //pv.RPC("networkColorChange", PhotonTargets.AllBuffered, firstModel);

                if (Physics.Raycast(ray, out Hit))//check current model status
                    finalModel = Hit.transform.name;//get model name

                if ((finalModel == firstModel) && (time.ElapsedMilliseconds >= (0.75 * 1000)))//if began matches with ending, same model from beginning to ending
                {
                   // TapCount = 0;
                    result = true;
                    /*if(!used[int.Parse(firstModel) - 1])
                    {
                        update++;
                        textObject = GameObject.Find("txt"+firstModel).GetComponent<TextMesh>();
                        textObject.text = update.ToString();
                        used[int.Parse(firstModel) - 1] = true;
                    }*/
                    PhotonView pv = GameObject.Find("image_target").GetComponent<PhotonView>();
                    pv.RPC("networkColorChange", PhotonTargets.OthersBuffered, firstModel);

                    switch (firstModel)//update count, update textbox, update bool array, and make colour green
                    {
                        case "1":
                            if (used[0] == false)
                            {
                                update++;
                                textObject = GameObject.Find("txt1").GetComponent<TextMesh>();
                                textObject.text = update.ToString();
                                used[0] = true;
                                GameObject.Find("1").GetComponent<Renderer>().material.color = Color.black;
                            }
                            break;
                        case "2":
                            if (used[1] == false)
                            {
                                update++;
                                textObject = GameObject.Find("txt2").GetComponent<TextMesh>();
                                textObject.text = update.ToString();
                                used[1] = true;
                                GameObject.Find("2").GetComponent<Renderer>().material.color = Color.green;
                            }
                            break;
                        case "3":
                            if (used[2] == false)
                            {
                                update++;
                                textObject = GameObject.Find("txt3").GetComponent<TextMesh>();
                                textObject.text = update.ToString();
                                used[2] = true;
                                GameObject.Find("3").GetComponent<Renderer>().material.color = Color.green;
                            }
                            break;
                        case "4":
                            if (used[3] == false)
                            {
                                update++;
                                textObject = GameObject.Find("txt4").GetComponent<TextMesh>();
                                textObject.text = update.ToString();
                                used[3] = true;
                                GameObject.Find("4").GetComponent<Renderer>().material.color = Color.green;
                            }
                            break;
                        case "5":
                            if (used[4] == false)
                            {
                                update++;
                                textObject = GameObject.Find("txt5").GetComponent<TextMesh>();
                                textObject.text = update.ToString();
                                used[4] = true;
                                GameObject.Find("5").GetComponent<Renderer>().material.color = Color.green;
                            }
                            break;
                        case "6":
                            if (used[5] == false)
                            {
                                update++;
                                textObject = GameObject.Find("txt6").GetComponent<TextMesh>();
                                textObject.text = update.ToString();
                                used[5] = true;
                                GameObject.Find("6").GetComponent<Renderer>().material.color = Color.green;
                            }
                            break;
                        case "7":
                            if (used[6] == false)
                            {
                                update++;
                                textObject = GameObject.Find("txt7").GetComponent<TextMesh>();
                                textObject.text = update.ToString();
                                used[6] = true;
                                GameObject.Find("7").GetComponent<Renderer>().material.color = Color.green;
                            }
                            break;
                        case "8":
                            if (used[7] == false)
                            {
                                update++;
                                textObject = GameObject.Find("txt8").GetComponent<TextMesh>();
                                textObject.text = update.ToString();
                                used[7] = true;
                                GameObject.Find("8").GetComponent<Renderer>().material.color = Color.green;
                            }
                            break;
                        case "9":
                            if (used[8] == false)
                            {
                                update++;
                                textObject = GameObject.Find("txt9").GetComponent<TextMesh>();
                                textObject.text = update.ToString();
                                used[8] = true;
                                GameObject.Find("9").GetComponent<Renderer>().material.color = Color.green;
                            }
                            break;
                        case "10":
                            if (used[9] == false)
                            {
                                update++;
                                textObject = GameObject.Find("txt10").GetComponent<TextMesh>();
                                textObject.text = update.ToString();
                                used[9] = true;
                                GameObject.Find("10").GetComponent<Renderer>().material.color = Color.green;
                            }
                            break;
                        case "11":
                            if (used[10] == false)
                            {
                                update++;
                                textObject = GameObject.Find("txt11").GetComponent<TextMesh>();
                                textObject.text = update.ToString();
                                used[10] = true;
                                GameObject.Find("11").GetComponent<Renderer>().material.color = Color.green;
                            }
                            break;
                        case "12":
                            if (used[11] == false)
                            {
                                update++;
                                textObject = GameObject.Find("txt12").GetComponent<TextMesh>();
                                textObject.text = update.ToString();
                                used[11] = true;
                                GameObject.Find("12").GetComponent<Renderer>().material.color = Color.green;
                            }
                            break;
                        case "13":
                            if (used[12] == false)
                            {
                                update++;
                                textObject = GameObject.Find("txt13").GetComponent<TextMesh>();
                                textObject.text = update.ToString();
                                used[12] = true;
                                GameObject.Find("13").GetComponent<Renderer>().material.color = Color.green;
                            }
                            break;
                        case "14":
                            if (used[13] == false)
                            {
                                update++;
                                textObject = GameObject.Find("txt14").GetComponent<TextMesh>();
                                textObject.text = update.ToString();
                                used[13] = true;
                                GameObject.Find("14").GetComponent<Renderer>().material.color = Color.green;
                            }
                            break;
                        case "15":
                            if (used[14] == false)
                            {
                                update++;
                                textObject = GameObject.Find("txt15").GetComponent<TextMesh>();
                                textObject.text = update.ToString();
                                used[14] = true;
                                GameObject.Find("15").GetComponent<Renderer>().material.color = Color.green;
                            }
                            break;
                        case "16":
                            if (used[15] == false)
                            {
                                update++;
                                textObject = GameObject.Find("txt16").GetComponent<TextMesh>();
                                textObject.text = update.ToString();
                                used[15] = true;
                                GameObject.Find("16").GetComponent<Renderer>().material.color = Color.green;
                            }
                            break;
                            //start of Abraham's code
                        case "TNP"://Tangram Not Present Button
                            if (used[16] == false) {
                                update++;
                                used[16] = true;
                                GameObject.Find("TNP").GetComponent<Renderer>().material.color = Color.white;
                                down_animation();
                            }
                            used[16] = false;
                            break;
                        //end of Abraham's code
                        default:
                            break;

                    }
                    //display number after tangram turns green


                }
                else//if not the same model as the beginning, change current model back to original colour
                {
                    if (firstModel.Equals("TNP"))
                    {
                        GameObject.Find(firstModel).GetComponent<Animator>().Play("TNP_down");
                    }

                    else if (!used[int.Parse(firstModel) - 1])//if not already confirmed
                    {
                            GameObject.Find(firstModel).GetComponent<Animator>().Play(firstModel + "_down");
                            GameObject.Find(firstModel).GetComponent<Renderer>().material.color = Color.white;
                    }
                }
                time.Reset();
            }
            /*
            if (TapCount == 1)
            {
                NewTime = Time.time + MaxDoubleTapTime;

            else if(TapCount==2 && Time.time <= NewTime)
            {
                //we have a double tap
                PhotonView pv = GameObject.Find("image_target").GetComponent<PhotonView>();
                pv.RPC("networkColorChange", PhotonTargets.OthersBuffered, "skip");
                rpcMessage.text = "Double Tap";
                TapCount = 0;
            }
            */



        }
        /*
        if(Time.time > NewTime)
        {
            TapCount = 0;
        }
        */
        return result;

    }
    /// <summary>
    /// Right now all this does is send the string (str) successfully.
    /// The last line (...material.color = Color.red;) doesn't work for some reason
    /// </summary>
    /// <param name="str"></param>
    [PunRPC]
    public void networkColorChange(string str)
    {
        //First time this is called:
        //index == 2
        if (str.Equals(order[index-2].ToString()))
        {
            numCorrect++;
        }
        
        GameObject.Find("txt" + order[index - 2].ToString()).GetComponent<TextMesh>().text = "";

        UnityEngine.Debug.Log(str);
        //rpcMessage.text = models[order[index - 1]].transform.position.ToString() + Results.bottomFour[0] + Results.bottomFour[1] + Results.bottomFour[2] + Results.bottomFour[3];
        //UnityEngine.Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ ");
        //UnityEngine.Debug.Log(models[order[index - 1]].transform.position);
        //UnityEngine.Debug.Log(Results.bottomFour[0] + Results.bottomFour[1] + Results.bottomFour[2] + Results.bottomFour[3]);
        /*while (models[order[index - 1]].transform.position == Results.bottomFour[0] ||
               models[order[index - 1]].transform.position == Results.bottomFour[1] ||
               models[order[index - 1]].transform.position == Results.bottomFour[2] ||
               models[order[index - 1]].transform.position == Results.bottomFour[3])
        {
            index++;
        }*/
        //GameObject.Find(order[index-1].ToString()).GetComponent<Animator>().Play(order[index - 1].ToString() + "_up");
        //GameObject.Find("txt" + order[index - 1].ToString()).GetComponent<TextMesh>().text = "X";
        //rpcMessage.text = order[index - 1].ToString();

        index++;
        numSelected++;
        Results.score = numCorrect;
        
    }

    [PunRPC]
    public void resultFunction(long time)
    {
        Results.time = time;
        SceneManager.LoadScene(2);
        
    }

    void directorSeries()
    {
        Pos[0] = new Vector3(-0.307f, 0.2f, 0.429f);
        Pos[1] = new Vector3(-0.075f, 0.2f, 0.429f);
        Pos[2] = new Vector3(0.157f, 0.2f, 0.42f);
        Pos[3] = new Vector3(0.38f, 0.2f, 0.416f);
        Pos[4] = new Vector3(-0.3005f, 0.211f, 0.112f);
        Pos[5] = new Vector3(-0.099f, 0.2f, 0.104f);
        Pos[6] = new Vector3(0.193f, 0.2f, 0.109f);
        Pos[7] = new Vector3(0.393f, 0.189f, 0.111f);
        Pos[8] = new Vector3(-0.311f, 0.2f, -0.191f);
        Pos[9] = new Vector3(-0.085f, 0.2f, -0.187f);
        Pos[10] = new Vector3(0.156f, 0.2f, -0.188f);
        Pos[11] = new Vector3(0.379f, 0.211f, -0.187f);
        Pos[12] = new Vector3(-0.324f, 0.2f, -0.4149999f);
        Pos[13] = new Vector3(-0.077f, 0.2f, -0.4149999f);
        Pos[14] = new Vector3(0.142f, 0.2f, -0.415f);
        Pos[15] = new Vector3(0.382f, 0.2f, -0.4149999f);
        string tangramPosition = "";
        for (int i = 0; i < 16; i++)
        {
            tangramPosition = Pos[i].ToString("f7");

            for (int j = 0; j < 16; j++)
            {
                if (tangramPosition == GameObject.Find((j + 1).ToString()).transform.localPosition.ToString("f7"))
                {
                    order[i] = (j + 1);
                    break;
                }
            }
        }
    }
}
