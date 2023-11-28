using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

// //public static class SaveSystem
// {
//     //public static void SavePlayer (Player player)
//     {
//         BinaryFormatter formatter = new BinaryFormatter();
//         string path = Application.persistentDataPath + "/player.savegame";
//         FileStream stream = new FileStream(path, FileMode.Create);

//         PlayerDetails details = new PlayerDetails(player);

//         formatter.Serialize(stream, details);
//         stream.Close();
//     }

//     //public static PlayerDetails LoadPlayer ()
//     {
//         string path = Application.persistentDataPath +"/player.savegame";
//         if (File.Exists(path))
//         {
//             BinaryFormatter formatter = new BinaryFormatter();
//             FileStream stream = new FileStream(path, FileMode.Open);

//             PlayerDetails details = formatter.Deserialize(stream) as PlayerDetails;
//             stream.Close();

//             return details;
//         } else
//         {
//             Debug.LogError("Save file not found in " + path);
//             return null;
//         }
//     }
    
// }
