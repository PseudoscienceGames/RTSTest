using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class XMLManager : MonoBehaviour
{
	public static XMLManager ins;
	private void Awake() { ins = this; }

	public ItemDatabase itemDB;

	public void SaveItems()
	{
		XmlSerializer serializer = new XmlSerializer(typeof(ItemDatabase));
		FileStream stream = new FileStream(Application.dataPath + "/StreamingAssets/XML/item_data.xml", FileMode.Create);
		serializer.Serialize(stream, itemDB);
		stream.Close();
	}

	public void LoadItems()
	{
		XmlSerializer serializer = new XmlSerializer(typeof(ItemDatabase));
		FileStream stream = new FileStream(Application.dataPath + "/StreamingAssets/XML/item_data.xml", FileMode.Open);
		itemDB = serializer.Deserialize(stream) as ItemDatabase;
		stream.Close();
	}
}
[System.Serializable]
public class ItemEntry
{
	public string itemName;
	public Mat mat;
	public int value;
}

[System.Serializable]
public class ItemDatabase
{
	[XmlArray("Items")]
	public List<ItemEntry> list = new List<ItemEntry>();
}

public enum Mat
{
	Wood,
	Copper,
	Iron,
	Steel,
	Obsidion
}