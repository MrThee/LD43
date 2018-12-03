using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceHarness : MonoBehaviour {
    public Transform kFaceContainer;
    public FaceElements allTheFaces;

    public Face myFace = new Face();
    public void GenerateHead(){
        Head head = allTheFaces.GenerateHead();
        ApplyFace(head.head, head.face);
    }

    public void ApplyFace(GameObject head, Face face){
        Instantiate<GameObject>(head, kFaceContainer, false);
        if(face.eyes != null) {
            if(face.eyes.left){
                this.myFace.eyes.left = Instantiate<GameObject>(face.eyes.left, kFaceContainer, false);
            }
            if(face.eyes.right){
                this.myFace.eyes.right = Instantiate<GameObject>(face.eyes.right, kFaceContainer, false);
            }
        }
        if(face.mouth){
            this.myFace.mouth = Instantiate<GameObject>(face.mouth, kFaceContainer, false);
        }
        if(face.nose){
            this.myFace.nose = Instantiate<GameObject>(face.nose, kFaceContainer, false);
        }
        if(face.eyewear){
            this.myFace.eyewear = Instantiate<GameObject>(face.eyewear, kFaceContainer, false);
        }
    }
}