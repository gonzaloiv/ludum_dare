﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

[Serializable]
public class Leaderboard {

  public bool[] IsTutorialPlayed { get { return isTutorialPlayed; } set { isTutorialPlayed = value; } }
  private bool[] isTutorialPlayed = new bool[] { false, false };

  public int[] HighestScores { get { return highestScores; } set { highestScores = value; } }
  private int[] highestScores = new int[] { 0, 0 };

}

public class DataManager : MonoBehaviour {

  #region Fields

  public static Leaderboard Leaderboard { get { return leaderboard; } }
  private static Leaderboard leaderboard;

  private static string dataPath;

  #endregion

  #region Mono Behaviour

  void Awake() {
    dataPath = Application.persistentDataPath;
    leaderboard = new Leaderboard();
    LoadData();
  }

  void Start() {
    if(!GetIsTutorialPlayed())
      SetIsTutorialPlayed();
  }

  void OnDestroy() {
    SaveData();
  }

  #endregion

  #region Public Behaviour

  public static void SetIsTutorialPlayed() {
    leaderboard.IsTutorialPlayed[(int)ModeConfig.Instance.MODE] = true;
  }

  public static bool GetIsTutorialPlayed() {
    return leaderboard.IsTutorialPlayed[(int)ModeConfig.Instance.MODE];
  }

  public static void SetHighestScore(int newScore) {
    Debug.Log("HIGHEST: " + leaderboard.HighestScores[(int)ModeConfig.Instance.MODE]);
    leaderboard.HighestScores[(int)ModeConfig.Instance.MODE] = newScore;
  }

  public static int GetHighestScore() {
    Debug.Log("HIGHEST: " + leaderboard.HighestScores[(int)ModeConfig.Instance.MODE]);
    return leaderboard.HighestScores[(int)ModeConfig.Instance.MODE];
  }

  public static void SaveData() {
    BinaryFormatter formatter = new BinaryFormatter();
    FileStream saveFile = File.Create(dataPath + "/data.binary");
    formatter.Serialize(saveFile, leaderboard);
    saveFile.Close();
  }

  public static void LoadData() {
    try {
      BinaryFormatter formatter = new BinaryFormatter();
      FileStream saveFile = File.Open(dataPath + "/data.binary", FileMode.Open);
      leaderboard = (Leaderboard) formatter.Deserialize(saveFile);
      saveFile.Close();
    } catch (FileNotFoundException exception) {
      Debug.Log("First play: Data not recorded, yet");
    }
  }

  #endregion

}