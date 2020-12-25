using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Text;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    #region Vars
     public Tcp tcp;
    public Text Log;
    [Header("Settings of Markers")]
    public Image[] Markers = new Image[2];
    public GameObject[] Buttons;
    public bool AllowButtonClick = false;
    public Image[] OpponentMarkers = new Image[2];
    public int UnTrueAnswers;
     [Header("Settings Conditions")]
     public GameObject Panel;
     public Text ConditionInfo;
     public int UnTrueAnswersofOpponent;
     [Header("Audio Settings")]
    public AudioClip clip;
    public AudioSource Source;
    [Header("Structure")]
    
    public List<Text> AnswersText = new List<Text>();
    public static int RoomID;
    public float timer = 10f;
    public bool TimerStart = true;
    public Text TimerText;
public delegate void CreateRoomHandler();
public event CreateRoomHandler CreatedRoom;

    #endregion


   

    #region Functions

public void SetTrueAnswer(List<Text> AnswersTextFields, string command){
    try{
    bool anchor = true;
    while(anchor){
        int value = UnityEngine.Random.Range(0,3);
        if(AnswersTextFields[value].text == ""){
            anchor = false;
            AnswersTextFields[value].text = command;
            Buttons[value].GetComponent<ButtonHandler>().TrueAnswer = true;
        }
    }
    }catch(Exception ex){
        Log.text+= ex.Source;
         Log.text+= ex.StackTrace;
    }
}

public void SetUnTrueAnswer(List<Text> AnswersTextFields, string command){
      try{
    bool anchor = true;
    while(anchor){
        int value = UnityEngine.Random.Range(0,3);
        if(AnswersTextFields[value].text == ""){
            anchor = false;
            AnswersTextFields[value].text = command;
            Buttons[value].GetComponent<ButtonHandler>().TrueAnswer = false;
        }
    }
      }catch(Exception ex){
        Log.text+= ex.Source;
         Log.text+= ex.StackTrace;
    }
}

    void Update(){
   
ChangeStateOfTimer();
   
    }

    private void ChangeStateOfTimer(){
 if(TimerStart){
        timer -= Time.deltaTime;
         TimerText.text = "" + (int)timer;
    }
    if(timer <= 0){
        
        TimerStart = false;
        timer = 10f;
        AllowButtonClick = false;
        UnTrueAnswers++;
         Tcp.SendData("Answer|false|a");
        ChangeStateofMarkers();
        

    }
    }

    public void CalculateGameResult(string Condition){
if(Condition == "Calculate"){

if(UnTrueAnswers == 2){
    Lose();
}else if(UnTrueAnswers < 2){
    Win();
}

}

if(Condition == "Draw"){
    Draw();
}
    }
   public void Start()
    {
   
        PlayerPrefs.SetString("Scene","Main");
        PlayerPrefs.Save();
      
CreatedRoom += new CreateRoomHandler(CreateRoom);
           
       Invoke("CreateRoom",0.5f);
       AllowButtonClick = true;
      
    }

  

#region  Markers
   public void ChangeStateofMarkers(){
       switch(CountOfUnTrueAnswers()){
           case 1:
Markers[0].color = Color.red;
           break;

           case 2:
Markers[1].color = Color.red;
           break;
       }
  switch(CountOfUnTrueAnswersofOpponent()){
           case 1:
OpponentMarkers[0].color = Color.red;
           break;

           case 2:
OpponentMarkers[1].color = Color.red;
           break;
       }

   }
   #endregion

    public void CreateRoom(){
        print("RoomCreated");

}
   
        #endregion
           
    
  

    IEnumerator DownloadMusic(){

    
    WWW www = new WWW("https://cdn10.sefon.pro/files/prev/155/MiyaGi%20%26%20Andy%20Panda%20-%20Kosandra%20%28192kbps%29.mp3");
    yield return www;


    
    Source.clip = www.GetAudioClip(false, false);
    if(Source.clip != null)
    Source.Play();

    }
public void InvokeEvent(){
CreatedRoom();

}
public int CountOfUnTrueAnswers(){
    return UnTrueAnswers;
}

public int CountOfUnTrueAnswersofOpponent(){
    return UnTrueAnswersofOpponent;
}

public void Win(){
Panel.SetActive(true);
ConditionInfo.text = "Win";
Log.text = "WIN";
}
public void Lose(){
Panel.SetActive(true);
ConditionInfo.text = "Lose";
Log.text = "LOSE";
}
public void Draw(){
    Panel.SetActive(true);
    ConditionInfo.text = "Draw";
    Log.text = "DRAW";
}
public void Disconnected(){
    Panel.SetActive(true);
    ConditionInfo.text = "Your opponent left the room";
    Log.text = "Disconnected";
}

public void LeftRoom(){
    SceneManager.LoadScene(0);
}
public static void LoadScene(){
    SceneManager.LoadScene(1);
}
public void InvokeLeftRoom(){
    Invoke("LeftRoom",3f);
}
}

