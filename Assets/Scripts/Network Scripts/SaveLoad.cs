using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;
using UnityEngine;

public class SaveLoad : MonoBehaviour{
	public static SaveLoad Instance;
	public List<Game> savedGames = new List<Game>();

	private void Awake(){
		if (Instance == null) {
			Instance = this;
		}
	}


	public void Save() {
		savedGames.Add(Game.Instance);
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Path.Combine(Application.persistentDataPath, "savedGames.gd"));
		bf.Serialize(file, this.savedGames);
		file.Close();
	}

	public void Load() {
		if(File.Exists(Application.persistentDataPath + "/savedGames.gd")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
			this.savedGames = (List<Game>)bf.Deserialize(file);
			file.Close();
		}
	}
}

