using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SpielstandLoader : MonoBehaviour {
#region public Variables

    public N01Helper n01H;
    public MainMenuController mMC;
    public MainMenuCarouselController mMCC;
    public ProcedureController pC;
    public PathList myPathPointList = new PathList();

#endregion

    [System.Serializable] public class PathList {
        public List<PathPoint> pathpoins = new List<PathPoint>();
    }

    [Serializable] public class PathPoint {
        public string pathID;
        public bool pathFinished = false;
    }

    //Initialisierung der Pfade mit einer ID (String) und einer bool-Variable, die angibt, ob ein Pfad bereits durchgespielt wurde
    public void SetPathpoint(string _pathID, bool _pathPointFinished) {
        PathPoint pP = new PathPoint();
        pP.pathID = _pathID;
        pP.pathFinished = _pathPointFinished;

        myPathPointList.pathpoins.Add(pP);
    }

    //Pfad wurde beendet, den Pfad mit der entsprechenden ID als durchgespielt setzen und den Spielstand in die JSON-Datei speichern
    public void FinishedPathPoint(string _pathID) {
        foreach (PathPoint pP in myPathPointList.pathpoins) {
            if (pP.pathID == _pathID) {
                pP.pathFinished = true;
            }
        }

        SaveSpielstand();
        mMC.SetPathPointList(myPathPointList, false);
    }

    //Spielstand in die JSON-Datei schreiben
    public void SaveSpielstand() {
        string filePath = Path.Combine(Application.persistentDataPath, "SpielstandState.json");
        string jsonData = JsonUtility.ToJson(myPathPointList);
        File.WriteAllText(filePath, jsonData);
    }

    //Spielstand aus der JSON-Datei laden. Wenn keine Datei existiert, wurde die Anwendung zum ersten mal gestartet -> ggf. Tutorials aktivieren etc.
    public void LoadSpielstand() {
        try {
            string filePath = Path.Combine(Application.persistentDataPath, "SpielstandState.json");
            string aktSpielstandData = File.ReadAllText(filePath);
            myPathPointList = JsonUtility.FromJson<PathList>(aktSpielstandData);
            mMC.SetPathPointList(myPathPointList, true);
            mMCC.SetLockOrUnlockImage(true);
            pC.InitToolTips(true);
        } catch (FileNotFoundException) {
            mMCC.SetLockOrUnlockImage(false);
            mMC.InitMainMenuWhileFileNotExists();
            pC.InitPathPoints();
            pC.InitToolTips(false);
        }
    }

    //Spielstand löschen, nur zu testzwecken relevant
    public void ResetGame() {
        File.Delete(Path.Combine(Application.persistentDataPath, "SpielstandState.json"));
        File.Delete(Path.Combine(Application.persistentDataPath, "CheckPointsState.json"));
        File.Delete(Path.Combine(Application.persistentDataPath, "spielstand.txt"));
    }
}
