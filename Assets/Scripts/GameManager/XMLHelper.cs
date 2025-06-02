using System;
using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

public static class XMLHelper
{
    // public static T LoadFromXml<T>(string filePath)
    // {
    //     try{
    //         //XmlSerializer serializer = new XmlSerializer(typeof(T));

    //         using (StreamReader reader = new StreamReader(filePath))
    //         {      
    //             string fileContent = reader.ReadToEnd();  // Legge tutto il contenuto del file come stringa

    //             // Torna all'inizio del file per deserializzarlo
    //             reader.BaseStream.Position = 0;

    //             XmlSerializer serializer = new XmlSerializer(typeof(T));

    //             return (T)serializer.Deserialize(reader);
    //         }
    //     } catch(Exception ex){
    //         UnityEngine.Debug.Log("" +ex.Message);
    //         return default(T);
    //     }
    // }

    public static T LoadFromXml<T>(string resourcePath)
    {
    try
        {
            TextAsset xmlAsset = Resources.Load<TextAsset>(resourcePath);
            if (xmlAsset == null)
            {
                UnityEngine.Debug.LogError($"❌ Impossibile caricare {resourcePath} da Resources.");
                return default;
            }
    
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(xmlAsset.text))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("Errore nella deserializzazione XML: " + ex.Message);
            return default;
        }
    }

    public static void SaveToXml<T>(T obj, string filePath)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            serializer.Serialize(writer, obj);
        }
    }
}