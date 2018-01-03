using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Leap;
using Leap.Unity;

public class FileManager : MonoBehaviour {

    private HandModel lHand;
    private HandModel rHand;
    private FingerModel[] lFingers = new FingerModel[5];
    private FingerModel[] rFingers = new FingerModel[5];

	private double startTime;

    void Start () {
        // set path username and time
        // clear the text file
		string path = "Assets/Resources/leapdata.csv";
		StreamWriter writer = new StreamWriter (path,false);
        writer.WriteLine("time,l_palm,,,l_thumb,,,l_index,,,l_middle,,,l_ring,,,l_pinky,,,r_palm,,,r_thumb,,,r_index,,,r_middle,,,r_ring,,,r_pinky,,");
		writer.Write (",");
		for (int x = 0; x < 10; x++) {
			writer.Write ("x,");
			writer.Write ("y,");
			writer.Write ("z,");
		}
		writer.WriteLine ();
		writer.Close ();
		startTime = Time.time;
	}
	
	void Update () {
		double elapsedTime = Time.time - startTime;
        // only write the time if the game is paused
		string path = "Assets/Resources/leapdata.csv";
		StreamWriter writer = new StreamWriter (path,true);
        writer.Write(elapsedTime + ",");
        // write hand values if a hand exists
        if (lHand != null)
        {
            string palmPos = lHand.GetPalmPosition().ToString("F5") + ",";
            writer.Write(palmPos.Replace("(","").Replace(")",""));

            foreach (FingerModel lFinger in lFingers)
            {
                //string type = lFinger.fingerType.ToString();
                //type = "l" + type.Substring(4).ToLower();
                string fingerPos = lFinger.GetTipPosition().ToString("F5") + ",";
                writer.Write(fingerPos.Replace("(","").Replace(")",""));
            }
        }
        else
        {
            //writer.Write(",,,,,,,,,,,,,,,,,,");
            for(int i = 0; i < 18; i++)
            {
                writer.Write( "NaN,");
            }
        }
        if (rHand != null)
        {
            string palmPos = rHand.GetPalmPosition().ToString("F5") + ",";
            writer.Write(palmPos.Replace("(", "").Replace(")", ""));

            int i = 0;
            foreach (FingerModel rFinger in rFingers)
            {
                //string type = rFinger.fingerType.ToString();
                //type = "r" + type.Substring(4).ToLower();
                // counter so that last entry doesn't have an extra comma
                string fingerPos;
                if(i == 4)
                {
                    fingerPos = rFinger.GetTipPosition().ToString("F5");
                }
                else
                {
                    fingerPos = rFinger.GetTipPosition().ToString("F5") + ",";
                }
                writer.Write(fingerPos.Replace("(", "").Replace(")", ""));
                i++;
            }
        }
        else
        {
            //writer.Write(",,,,,,,,,,,,,,,,,");
            for(int i = 0; i < 17; i++)
            {
                writer.Write("NaN,");
            }
            writer.Write(float.NaN);
        }
        writer.WriteLine();
		writer.Close ();

    }
    // set the hand references
    public void SetHand(HandModel hand, Chirality handedness)
    {
        if(handedness == Chirality.Left)
        {
            lHand = hand;
            lFingers = hand.fingers;
        }
        else if(handedness == Chirality.Right)
        {
            rHand = hand;
            rFingers = hand.fingers;
        }
    }

    public void ClearHand(Chirality handedness)
    {
        if (handedness == Chirality.Left)
        {
            lHand = null;
            lFingers = null;
        }
        else if (handedness == Chirality.Right)
        {
            rHand = null;
            rFingers = null;
        }
    }

}
