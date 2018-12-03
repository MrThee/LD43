using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="new Faces", menuName="Faces", order=0)]
public class FaceElements : ScriptableObject {

	public List<GameObject> heads = new List<GameObject>();
	public List<Face.LeftRightGO> eyes = new List<Face.LeftRightGO>();
	public List<GameObject> mouths = new List<GameObject>();
    public List<GameObject> noses = new List<GameObject>();
    public List<GameObject> eyewears = new List<GameObject>();

	public Head GenerateHead() {
		var head = Roll(heads);
		Face.LeftRightGO eyePair = Lucky(4,5) ? Roll<Face.LeftRightGO>(eyes) : null;
		var mouth = Lucky(1,10) ? Roll(mouths) : null;
		var nose = Lucky(1,7) ? Roll(noses) : null;
		var eyewear = (eyePair == null && Lucky(1,3)) ? Roll(eyewears) : null;
		return new Head(){
			head = head,
			face = new Face(){
				eyes = eyePair,
				nose = nose,
				mouth = mouth,
				eyewear = eyewear
			}
		};
	}

	bool Lucky(int denominator){
		return Lucky(1, denominator);
	}

	bool Lucky(int numerator, int denominator) {
		return(Random.Range(0, denominator) <= numerator);
	}

	T Roll<T>(List<T> list){
		int index = Random.Range(0, list.Count);
		return list[index];
	}
}
