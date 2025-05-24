// Path: Assets/Scripts/Game/UIController/UISkillChooser_Ctrl.cs
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class UISkillChooser_Ctrl : UIController
{
	public List<GameObject> skillPrefabList = new List<GameObject>();

	public override void Awake()
	{
		base.Awake();
	}

	public void Start()
	{
		string pathToResources = "Prefabs/Weapons/Summons";
		GameObject[] prefabs = Resources.LoadAll<GameObject>(pathToResources);
		
		foreach(GameObject prefab in prefabs)
		{
			skillPrefabList.Add(prefab);
		}

		List<GameObject> skillPrefabListRandom = new List<GameObject>();
		for (int i = 0; i < 3; i++)
		{
			int randomIndex = Random.Range(0, skillPrefabList.Count);
			Debug.Log("randomIndex: " + randomIndex);
			Debug.Log("skillPrefabList.Count: " + skillPrefabList.Count);
			skillPrefabListRandom.Add(skillPrefabList[randomIndex]);
		}

		int index = 0;

		foreach(Button button in this.GetComponentsInChildren<Button>())
		{
			GameObject skillPrefab = skillPrefabListRandom[index];
			Sprite skillSprite = skillPrefab.GetComponent<SpriteRenderer>().sprite;
			button.GetComponent<Image>().sprite = skillSprite;
			button.onClick.AddListener(delegate ()
			{
				GameObject weaponSlot = GameObject.FindGameObjectWithTag("WeaponSlot");
				weaponSlot.GetComponent<WeaponSlot>().AddWeapon(skillPrefab);
				Time.timeScale = 1;
				Destroy(this.gameObject);
			});
			index++;
		}
		Time.timeScale = 0;
	}
}
