using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PhotonNetworkManager : Photon.MonoBehaviour {
    [SerializeField] public Text connectText;
    //[SerializeField] private Text Lobby;
    [SerializeField]GameObject player;
    [SerializeField]GameObject button;
    
    int numberOfPlayers = 0;
    public List<GameObject> DirectorModels = new List<GameObject>();
    tangram_controller tangramController;
    PhotonView pv;
    int index = 2;

    
    //[SerializeField] PhotonPlayer matcher;
    public List<GameObject> MatcherModels = new List<GameObject>();

    //public List<GameObject> lolly_pops = new List<GameObject>();
    
    [SerializeField] private Transform spawnPoint;
    static public PhotonNetworkManager Instance;
    public void Shuffle(List<GameObject> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);

            Vector3 g = list[k].transform.localPosition;
            list[k].transform.localPosition = list[n].transform.localPosition;
            list[n].transform.localPosition = g;
            
        }
    }
    void Start () {
        Instance = this;
        //tangramController.Start();
        
        PhotonNetwork.ConnectUsingSettings("Ver 0.1");
        PhotonNetwork.autoJoinLobby = true;
        pv = GameObject.Find("image_target").GetComponent<PhotonView>();
    }
    public void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public virtual void OnJoinedLobby()
    {
        Debug.Log("We have now joined lobby");
        //join room if it exists or create one
        PhotonNetwork.JoinOrCreateRoom("room", null, null);
        PhotonNetwork.room.MaxPlayers = 2;
    }

    public virtual void OnJoinedRoom()
    {
        //Director case
        if (PhotonNetwork.playerList.Length == 1)
        {
            //player.name = "Director";
            Results.player = "Director";
            Destroy(button);

            //Results.player = "Director";
            Results.bottomFour[0] = DirectorModels[12].transform.position;
            Results.bottomFour[1] = DirectorModels[13].transform.position;
            Results.bottomFour[2] = DirectorModels[14].transform.position;
            Results.bottomFour[3] = DirectorModels[15].transform.position;
            Shuffle(DirectorModels);
            

            for (int i = 0; i < 16; i++)
            {
                if (DirectorModels[i].transform.position == Results.bottomFour[0] ||
                    DirectorModels[i].transform.position == Results.bottomFour[1] ||
                    DirectorModels[i].transform.position == Results.bottomFour[2] ||
                    DirectorModels[i].transform.position == Results.bottomFour[3])
                {
                    //DirectorModels[i].SetActive(false);
                }
            }
        }

        //Matcher Case
        else if (PhotonNetwork.playerList.Length == 2)
        {
            //player.name = "Matcher";
            Results.player = "Matcher";
            //Results.player = "Matcher";
            Shuffle(MatcherModels);
        }
    }


    private void UpdateDirector(string stringIndex)
    {
        GameObject.Find(stringIndex).GetComponent<Animator>().Play(stringIndex + "_up");
    }


    [PunRPC]
    public void UpdateMatcher()
    {
        /*if (tangramController.handleTouchInput())
        {
            //index++;
            //UpdateDirector(index++.ToString());
        }*/
    }
   
// Update is called once per frame
    void Update () {
        PhotonView pv = GameObject.Find("image_target").GetComponent<PhotonView>();
        pv.RPC("SetName", PhotonTargets.AllBuffered, Results.player);

       if (player.name.Equals("Matcher"))
        {
            //Debug.Log("does it enter into matcher statement");
            
            pv.RPC("UpdateMatcher", PhotonTargets.AllBuffered);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LeaveRoom();
        }
    }

    [PunRPC]
    public void SetName(string str)
    {
        //Debug.Log("in setname function Player name is " + str);

        UnityEngine.Debug.Log(str);
        connectText.text = str;
        //GameObject.Find(str).GetComponent<Renderer>().material.color = Color.red;
    }

}
