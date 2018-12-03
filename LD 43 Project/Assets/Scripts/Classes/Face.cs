using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Face {

	[System.Serializable]
	public class LeftRight<T> {
		public T left;
		public T right;
	}

	[System.Serializable]
	public class LeftRightGO: LeftRight<GameObject> {}

    public LeftRightGO eyes;
    public GameObject mouth;
    public GameObject nose;
    public GameObject eyewear;
}

[System.Serializable]
public class Head {
    public GameObject head;
    public Face face;
}