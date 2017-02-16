using UnityEngine;
using System.IO;
using System.Text;

using System.Collections;
using System;

public class SaveLoad : MonoBehaviour {

    string filename = "saveFile.txt";
    //StreamWriter sw = new StreamWriter("saveFile.txt");

    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("s"))
        {
            string output = "" + Numbers.money + "," + Numbers.currHP + "," + Numbers.maxHP;
            System.IO.File.WriteAllText(filename, output);
        }

        if (Input.GetKeyDown("l"))
        {
            try
            {
                string line;
                StreamReader sr = new StreamReader(filename, Encoding.Default);
                using (sr)
                {
                    do
                    {
                        line = sr.ReadLine();

                        if (line != null)
                        {
                            string[] entries = line.Split(',');
                            Numbers.money = Convert.ToInt32(entries[0]);
                            Numbers.currHP = Convert.ToInt32(entries[1]);
                            Numbers.currHP = Convert.ToInt32(entries[2]);
                        }
                    }
                    while (line != null);
                    sr.Close();
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}\n", e.Message);
                return;
            }
        }
    }              
}
