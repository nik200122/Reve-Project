using System.IO;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    private const string playerStatsFile = "Assets/Resources/XML/playerStats.xml";

    public void LoadData(Player player){
        player = XMLHelper.LoadFromXml<Player>(playerStatsFile);
        Debug.Log("VALORE "+player.GetStat("SAZIETA").GetStatTag());
    }
}