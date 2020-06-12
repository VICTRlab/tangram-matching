using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class press : MonoBehaviour
{
    private int selection = 0;
    public List<GameObject> models = new List<GameObject>();
    public Hashtable OG_pos = new Hashtable();
    private void Start()
    {
        int posIndex = 0;
        foreach (GameObject go in models)
        {
            go.SetActive(false);
            OG_pos.Add(posIndex, go.transform.position);
            posIndex++;

        }

        
        //models[0].SetActive(true);
        //models[1].SetActive(true);
        //models[2].SetActive(true);
        //models[3].SetActive(true);

        //Random random = new Random();
        for (int i = 0; i < 12; i++)
        {
            //int count = 0;
            selection = Random.Range(0, 15);

            while (models[selection].activeInHierarchy)
            {
                selection = Random.Range(0, 15);

            }
  
           
            models[selection].transform.position = (Vector3) OG_pos[i];
            models[selection].SetActive(true);
           

        }
    }

    public void Update()
    {
    //    Random random = new Random();
    //    selection = Random.Range(0, 4);
    //    if (selection == 1)
    //    {
    //        models[0].SetActive(false);
    //        models[1].SetActive(false);
    //        models[2].SetActive(false);
    //        models[3].SetActive(false);
    //        models[4].SetActive(true);
    //        models[5].SetActive(true);
    //        models[6].SetActive(true);
    //        models[7].SetActive(true);
    //        models[8].SetActive(false);
    //        models[9].SetActive(false);
    //        models[10].SetActive(false);
    //        models[11].SetActive(false);
    //        models[12].SetActive(false);
    //        models[13].SetActive(false);
    //        models[14].SetActive(false);
    //        models[15].SetActive(false);
    //    }
    //    if (selection == 2)
    //    {
    //        models[0].SetActive(false);
    //        models[1].SetActive(false);
    //        models[2].SetActive(false);
    //        models[3].SetActive(false);
    //        models[4].SetActive(false);
    //        models[5].SetActive(false);
    //        models[6].SetActive(false);
    //        models[7].SetActive(false);
    //        models[8].SetActive(true);
    //        models[9].SetActive(true);
    //        models[10].SetActive(true);
    //        models[11].SetActive(true);
    //        models[12].SetActive(false);
    //        models[13].SetActive(false);
    //        models[14].SetActive(false);
    //        models[15].SetActive(false);
    //    }
    //    if (selection == 3)
    //    {
    //        models[0].SetActive(false);
    //        models[1].SetActive(false);
    //        models[2].SetActive(false);
    //        models[3].SetActive(false);
    //        models[4].SetActive(false);
    //        models[5].SetActive(false);
    //        models[6].SetActive(false);
    //        models[7].SetActive(false);
    //        models[8].SetActive(false);
    //        models[9].SetActive(false);
    //        models[10].SetActive(false);
    //        models[11].SetActive(false);
    //        models[12].SetActive(true);
    //        models[13].SetActive(true);
    //        models[14].SetActive(true);
    //        models[15].SetActive(true);
    //    }
    //    if (selection == 0)
    //    {
    //        models[0].SetActive(true);
    //        models[1].SetActive(true);
    //        models[2].SetActive(true);
    //        models[3].SetActive(true);
    //        models[4].SetActive(false);
    //        models[5].SetActive(false);
    //        models[6].SetActive(false);
    //        models[7].SetActive(false);
    //        models[8].SetActive(false);
    //        models[9].SetActive(false);
    //        models[10].SetActive(false);
    //        models[11].SetActive(false);
    //        models[12].SetActive(false);
    //        models[13].SetActive(false);
    //        models[14].SetActive(false);
    //        models[15].SetActive(false);
    //    }
    }

}
