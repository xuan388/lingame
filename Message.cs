using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SimpleFirebaseUnity;
using SimpleFirebaseUnity.MiniJSON;

using System;
using System.Text.RegularExpressions;

public class Message : MonoBehaviour {

    public bool openBC;

    GameObject textPanel;
    GameObject textCenPanel;
    GameObject textTopPanel;
    GameObject mesPanel;
    GameObject mesCenPanel2;
    GameObject mesTopPanel2;
    GameObject mesTopMask;
    public GameObject msgPrefab;
    public GameObject msgTopPrefab;
    public List<GameObject> msgObj = new List<GameObject>();
    public List<GameObject> msgCenObj = new List<GameObject>();
    public List<GameObject> msgTopObj = new List<GameObject>();

    private long gameTime;
    private long gameTopTime;
    public bool nowLoadString = false;
    public string inputType;
    public string inputMsg;
    public string inputName;
    public string inputItemTitle;
    public string itemRarity;
    public bool msgBoxOpen = false;
    Text playerID;


    void Start()
    {
        if (titleSet.playerName == "" || titleSet.playerName == null)
        {
            inputName = "匿名玩家";
        }
        else
        {
            inputName = titleSet.playerName;
        }
        gameTime = 0;
        gameTopTime = 0;
        UpdateTimeAndPushText();
        StartCoroutine(GetChatRealTime());

        textPanel = GameObject.Find("TextPanel");
        textCenPanel = GameObject.Find("TextCenPanel");
        textTopPanel = GameObject.Find("TextTopPanel");
        mesPanel = GameObject.Find("MesPanel");
        mesCenPanel2 = GameObject.Find("MesCenPanel2");
        mesTopPanel2 = GameObject.Find("MesTopPanel2");
        mesTopMask = GameObject.Find("MesTopMask");
        playerID = GameObject.Find("playerID").GetComponent<Text>();
        playerID.text = inputName;
        /*inPanel.SetActive(false);
        itemTitle.SetActive(false);
        */
        AddMessage(2, "<color=#FDF79D>伺服器已連結.</color>", "", "", 0, 0);
        //AddMessage(2, "<color=#FDF79D>世界 頻道已連接.</color>", "", 0, 0);

        mesPanel.SetActive(false);
    }

    public void AddMessage(int type, string Msg, string Name, string ItemTitle, int Quantity, int Rarity)
    {
        //type1=物品, type2=聊天
        if (type == 1)
        {
            string textColor = "";
            if (Rarity == 0){ textColor = "#F67122"; }
            else if (Rarity == 1) { textColor = "#ECDFC3"; }
            else if (Rarity == 2) { textColor = "#00DC15"; }
            else if (Rarity == 3) { textColor = "#00A1FE"; }
            else if (Rarity == 4) { textColor = "#FF0000"; }
            else if (Rarity == 5) { textColor = "#D44FFF"; }
            else if (Rarity == 6) { textColor = "#FFDD00"; }

            msgObj.Add(Instantiate(msgPrefab));
            msgObj[msgObj.Count - 1].transform.SetParent(textPanel.transform, false);
            msgObj[msgObj.Count - 1].GetComponent<Text>().text = "<color=#FDF79D>獲得" + Quantity.ToString() + "個<color=" + textColor + ">" + Msg + "</color>了.</color>";

            msgCenObj.Add(Instantiate(msgPrefab));
            msgCenObj[msgCenObj.Count - 1].transform.SetParent(textCenPanel.transform, false);
            msgCenObj[msgCenObj.Count - 1].GetComponent<Text>().text = "<color=#FDF79D>獲得" + Quantity.ToString() + "個<color=" + textColor + ">" + Msg + "</color>了.</color>";

            if(Rarity >= 4 && openBC == true)
            {
                inputMsg = Msg;
                inputItemTitle = ItemTitle;
                itemRarity = Rarity.ToString();
                OnBC();
            }
        }

        if (type == 2 || type == 9)
        {

            msgObj.Add(Instantiate(msgPrefab));
            msgObj[msgObj.Count - 1].transform.SetParent(textPanel.transform, false);
            msgObj[msgObj.Count - 1].GetComponent<Text>().text = Msg;

            msgCenObj.Add(Instantiate(msgPrefab));
            msgCenObj[msgCenObj.Count - 1].transform.SetParent(textCenPanel.transform, false);
            msgCenObj[msgCenObj.Count - 1].GetComponent<Text>().text = Msg;

        }

        if (type == 3)
        {

            msgObj.Add(Instantiate(msgPrefab));
            msgObj[msgObj.Count - 1].transform.SetParent(textPanel.transform, false);
            msgObj[msgObj.Count - 1].GetComponent<Text>().text = "[" + Name + "]\u00A0" + Msg;
            msgObj[msgObj.Count - 1].GetComponent<Text>().supportRichText = false;

            msgCenObj.Add(Instantiate(msgPrefab));
            msgCenObj[msgCenObj.Count - 1].transform.SetParent(textCenPanel.transform, false);
            msgCenObj[msgCenObj.Count - 1].GetComponent<Text>().text = "[" + Name + "]\u00A0" + Msg;
            msgCenObj[msgCenObj.Count - 1].GetComponent<Text>().supportRichText = false;
        }

        if(type == 6)
        {
            string textColor = "";
            if (Rarity == 0) { textColor = "#F67122"; }
            else if (Rarity == 1) { textColor = "#ECDFC3"; }
            else if (Rarity == 2) { textColor = "#00DC15"; }
            else if (Rarity == 3) { textColor = "#00A1FE"; }
            else if (Rarity == 4) { textColor = "#FF0000"; }
            else if (Rarity == 5) { textColor = "#D44FFF"; }
            else if (Rarity == 6) { textColor = "#FFDD00"; }

            msgObj.Add(Instantiate(msgPrefab));
            msgObj[msgObj.Count - 1].transform.SetParent(textPanel.transform, false);
            msgObj[msgObj.Count - 1].GetComponent<Text>().text = "<color=#FDF79D>" + Name + "\u00A0從\u00A0" + ItemTitle + "\u00A0獲得了\u00A0<color=" + textColor + ">" + Msg + "</color>.</color>";

            msgCenObj.Add(Instantiate(msgPrefab));
            msgCenObj[msgCenObj.Count - 1].transform.SetParent(textCenPanel.transform, false);
            msgCenObj[msgCenObj.Count - 1].GetComponent<Text>().text = "<color=#FDF79D>" + Name + "\u00A0從\u00A0" + ItemTitle + "\u00A0獲得了\u00A0<color=" + textColor + ">" + Msg + "</color>.</color>";

            //廣播
            msgTopObj.Add(Instantiate(msgTopPrefab));
            msgTopObj[msgTopObj.Count - 1].transform.SetParent(textTopPanel.transform, false);
            msgTopObj[msgTopObj.Count - 1].GetComponent<Text>().text = "<color=#FFE50C>" + Name + " 從\u00A0" + ItemTitle + "\u00A0獲得了\u00A0<color=" + textColor + ">" + Msg + "</color>.</color>";
            StartCoroutine(topDel(msgTopObj.Count - 1, 10f));
        }

        StartCoroutine("AA");
        StopCoroutine("BB");
        StartCoroutine("BB");

        if (msgObj.Count > 100)
        {
            LessMessage(1);
        }

        if (msgCenObj.Count > 4)
        {
            LessMessage(2);
        }

    }

    public void LessMessage(int type)
    {
        if (type == 1)
        {
            Destroy(msgObj[0].gameObject);
            msgObj.Remove(msgObj[0].gameObject);
        }
        if (type == 2)
        {
            Destroy(msgCenObj[0].gameObject);
            msgCenObj.Remove(msgCenObj[0].gameObject);
        }
        if (type == 3)
        {
            Destroy(msgTopObj[0].gameObject);
            msgTopObj.Remove(msgTopObj[0].gameObject);
        }

    }

    IEnumerator AA()
    {

        yield return new WaitForSeconds(0.1f);
        textPanel.transform.position = new Vector3(textPanel.GetComponent<RectTransform>().position.x, 9999, textPanel.GetComponent<RectTransform>().position.z);
        textCenPanel.transform.position = new Vector3(textCenPanel.GetComponent<RectTransform>().position.x, 9999, textCenPanel.GetComponent<RectTransform>().position.z);
        //textTopPanel.transform.position = new Vector3(textTopPanel.GetComponent<RectTransform>().position.x, 9999, textTopPanel.GetComponent<RectTransform>().position.z);
        mesTopMask.GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;
    }

    IEnumerator BB()
    {
        mesCenPanel2.SetActive(true);
        yield return new WaitForSeconds(15f);
        mesCenPanel2.SetActive(false);
    }

    IEnumerator topDel(int topID, float time)
    {

        yield return new WaitForSeconds(time);

        Destroy(msgTopObj[0].gameObject);
        msgTopObj.Remove(msgTopObj[0].gameObject);

    }

    //回傳值
    void GetOKHandler(Firebase sender, DataSnapshot snapshot)
    {
        if (gameTime == 0)
        {
            //頻道連線
            AddMessage(2, "<color=#FDF79D>世界 頻道已連接.</color>", "", "", 0, 0);
        }
        else
        {
            Dictionary<string, object> dict = snapshot.Value<Dictionary<string, object>>();
            List<string> keys = snapshot.Keys;

            if (keys != null)
            {

                string[] getTime = Regex.Split(dict[keys[keys.Count - 1]].ToString(), "//linm//");


                if (gameTime < long.Parse(getTime[5]))
                {
                    int getKeysCount = 0;
                    nowLoadString = true;

                    foreach (string key in keys)
                    {
                        string[] getTime2 = Regex.Split(dict[key].ToString(), "//linm//");
                        
                        if (gameTime < long.Parse(getTime2[5]))
                        {
                            //取得交談 dict
                            //匯出關閉//Debug.Log(getTime2[3].ToString());
                            //Debug.Log("AAAA: " + getTime2[2]);
                            if (getTime2[2] == null || getTime2[2] == "")
                            {
                                //AddMessage(2, "<color=#FF0000>幫你擋一下，連點開太快會來不及上傳廣播訊息哦!</color>", "", "", 0, 0);
                            }
                            else
                            {
                                AddMessage(int.Parse(getTime2[0]), getTime2[2], getTime2[1], getTime2[4], 0, int.Parse(getTime2[3]));
                            }
                            //匯出關閉//Debug.Log(key + " = " + getTime2[1]);
                        }
                        if (getKeysCount == keys.Count - 1)
                        {
                            //匯出關閉//Debug.Log(keys.Count - 1);
                            gameTime = long.Parse(getTime2[5]);
                            nowLoadString = false;
                        }
                        getKeysCount++;
                    }
                    

                }


            }
        }
    }

    public void CantOnline()
    {
        ab = 1;
    }

    private int ab = 0;

    void CantOnline2()
    {
        AddMessage(2, "<color=#FDF79D>世界 頻道連接失敗.</color>", "", "", 0, 0);
    }

    //上傳後刪除
    void PushOKHandler(Firebase sender, DataSnapshot snapshot)
    {
        Dictionary<string, object> dict = snapshot.Value<Dictionary<string, object>>();
        List<string> keys = snapshot.Keys;

        StartCoroutine(pushDel(dict[keys[0]].ToString()));
    }

    IEnumerator pushDel(string pushName)
    {
        Firebase firebase = Firebase.CreateNew("https://linmgame-1afc7.firebaseio.com/");
        FirebaseQueue firebaseQueue = new FirebaseQueue(true, 3, 1f);
        yield return new WaitForSeconds(3f);
        firebaseQueue.AddQueueDelete(firebase.Child("broadcasts").Child(pushName));
    }


    //讀取時間
    void GetTimeStamp(Firebase sender, DataSnapshot snapshot)
    {
        long timeStamp = snapshot.Value<long>();
        string itemRy = itemRarity;

        if (gameTime == 0)
        {
            //匯出關閉//Debug.Log("初始時間:" + timeStamp);
            gameTime = timeStamp;
            gameTopTime = timeStamp;
        }
        else
        {
            StartCoroutine(PushText(inputType, inputName, inputMsg, itemRy, inputItemTitle, timeStamp));
        }

    }

    //更新時間與推送
    void UpdateTimeAndPushText()
    {
        Firebase firebase = Firebase.CreateNew("https://linmgame-1afc7.firebaseio.com/");
        FirebaseQueue firebaseQueue = new FirebaseQueue(true, 3, 1f);
        firebase.OnPushSuccess += PushOKHandler;
        firebase.OnGetSuccess += GetOKHandler;

        Firebase lastUpdate = firebase.Child("lastUpdate");
        lastUpdate.OnGetSuccess += GetTimeStamp;
        
        firebaseQueue.AddQueueSetTimeStamp(firebase, "lastUpdate");
        firebaseQueue.AddQueueGet(lastUpdate);

    }

    IEnumerator PushText(string type, string name, string msg, string rarity, string itemTitle, long timestamp)
    {
        Firebase firebase = Firebase.CreateNew("https://linmgame-1afc7.firebaseio.com/");
        FirebaseQueue firebaseQueue = new FirebaseQueue(true, 3, 1f);
        firebase.OnPushSuccess += PushOKHandler;
        firebase.OnGetSuccess += GetOKHandler;

        if (msg == null || msg == "") { }
        else
        {
            firebase.Child("broadcasts", true).Push(type + "//linm//" + name.Replace(" ", "\u00A0") + "//linm//" + msg.Replace(" ", "\u00A0") + "//linm//" + rarity + "//linm//" + itemTitle + "//linm//" + timestamp.ToString(), false);
        }
        yield return new WaitForSeconds(0.1f);

        inputMsg = "";
        itemRarity = "";
        inputItemTitle = "";
    }


    //即時讀取
    IEnumerator GetChatRealTime()
    {
        Firebase firebase = Firebase.CreateNew("https://linmgame-1afc7.firebaseio.com/");
        FirebaseQueue firebaseQueue = new FirebaseQueue(true, 3, 1f);
        firebase.OnGetSuccess += GetOKHandler;

        for (; ; )
        {
            if (!nowLoadString)
            {
                firebaseQueue.AddQueueGet(firebase.Child("broadcasts", true));
            }
            yield return new WaitForSeconds(0.5f);
        }
    }


    //按鈕OK
    public void InputButton(InputField InputField)
    {
        if (InputField.text != "")
        {
            inputType = "3";
            itemRarity = "0";
            inputMsg = InputField.text;
            inputItemTitle = "";
            UpdateTimeAndPushText();
            InputField.text = "";

        }
    }

    //鍵盤OK
    public void OnChange()
    {
        msgBoxOpen = true;
    }

    public void EndEdit()
    {
        msgBoxOpen = false;
    }

    void FixedUpdate()
    {
        if (msgBoxOpen && Input.GetKeyDown(KeyCode.Return) || msgBoxOpen && Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            InputButton(GameObject.Find("InputField").GetComponent<InputField>());
        }
    }


    //上電視送出
    public void OnBC()
    {

        inputType = "6";
        UpdateTimeAndPushText();

    }
}
