using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class InventoryHarness : MonoBehaviour {

	[Header("Component")]
	public Transform gunContainer;

	[Header("Test Items!")]
	public FiringArray testGun1;

	private List<FiringArray> mk_guns;
	private int m_currentGunIndex;
	public FiringArray currentGun { 
		get {
			if(m_currentGunIndex == -1){
				return null;
			}
			else {
				return mk_guns[m_currentGunIndex];
			}
		}
	}
	public ReadOnlyCollection<FiringArray> guns { get { return mk_guns.AsReadOnly(); } }
	
	public static int MaxGunCount { get { return 3; } }

	void Start() {
		this.mk_guns = new List<FiringArray>(MaxGunCount);
		this.m_currentGunIndex = -1;

		if(testGun1){
			this.AddGun(testGun1, true);
		}
	}

	public bool CanAddGun() {
		return (mk_guns.Count < MaxGunCount);
	}

	public void AddGun(FiringArray gunPrefab, bool equipNow) {
		// 1. Instantiate it
		FiringArray newGunInst = Instantiate<FiringArray>(gunPrefab, gunContainer, false);
		mk_guns.Add(newGunInst);

		if(equipNow) {
			MaybeEquipGun(mk_guns.Count-1);
		}
	}

	public void MaybeEquipGun(int gunIndex){
		gunIndex = gunIndex % MaxGunCount;
		if(gunIndex < mk_guns.Count){
			// Hide the old one, if there's one there.
			if(currentGun != null) {
				currentGun.gameObject.SetActive(false);
			}
			m_currentGunIndex = gunIndex;
			FiringArray gunInst = currentGun;
			gunInst.gameObject.SetActive(true);
		}
	}

	public void UnEquipGun(){
		m_currentGunIndex = -1;
	}
}
