using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ButtonHandler : MonoBehaviour,IPointerClickHandler
{
    public bool TrueAnswer = false;
    private MainController MainController;
    private AudioSource clip;
    public void OnPointerClick(PointerEventData eventData)
    {
        CheckAnswer();
       
    }
private void CheckAnswer(){
    MainController.Log.text += "Click";
   if(MainController.AllowButtonClick){
         MainController.AllowButtonClick = false;
        MainController.TimerStart = false;
    if(TrueAnswer){
        print("True Answer");
     Tcp.SendData("Answer|true|a");
    }else{
        MainController.UnTrueAnswers++;
       
     Tcp.SendData("Answer|false|a");
    }

     MainController.ChangeStateofMarkers();
  
   }
    }

  
    void Start()
    {
        MainController = Camera.main.GetComponent<MainController>();
       clip = GameObject.Find("AudioSource").GetComponent<AudioSource>();
        
    }

   

}
