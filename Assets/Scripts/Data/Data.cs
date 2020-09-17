using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Data : MonoBehaviour
{
    private string DATA_PATH = "/Cars.dat";
    private PlayerData player;

    public void SaveData(int money, List<int> maps, Dictionary<Ecars, int> cars, Ecars choosedCar)
    {
        FileStream file = null;
        print("Data: " + Application.persistentDataPath + DATA_PATH);

        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            file = File.Create(Application.persistentDataPath + DATA_PATH);

            PlayerData p = new PlayerData(money, maps, cars, choosedCar);

            bf.Serialize(file, p);

        } catch (Exception ) {

        } finally {
            if (file != null)
                file.Close();
        }
    }

    public void LoadData()
    {
        FileStream file = null;

        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            file = File.Open(Application.persistentDataPath + DATA_PATH, FileMode.Open);

            player = bf.Deserialize(file) as PlayerData;           

        } catch (Exception )
        {
            Debug.Log("No file");
        } finally {
            if (file != null)
                file.Close();
        }
    }

    public PlayerData playerData
    {
        get { return player; }
    }
    
}
