using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceHarness : MonoBehaviour {
    public Transform kFaceContainer;
    public FaceElements allTheFaces;

    public Face myFace = new Face();
    public GameObject myHead;
    public void GenerateHead(){
        Head head = allTheFaces.GenerateHead();
        ApplyFace(head.head, head.face);
    }

    public void ApplyFace(GameObject head, Face face){
        myHead = Instantiate<GameObject>(head, kFaceContainer, false);
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

    public Rigidbody Decapitate() {
        GameObject head = myHead;
        if(myFace.eyes.left){
            myFace.eyes.left.transform.SetParent(head.transform);
        }
        if(myFace.eyes.right){
            myFace.eyes.right.transform.SetParent(head.transform);
        }
        if(myFace.mouth){
            myFace.mouth.transform.SetParent(head.transform);
        }
        if(myFace.nose){
            myFace.nose.transform.SetParent(head.transform);
        }
        if(myFace.mouth){
            myFace.mouth.transform.SetParent(head.transform);
        }
        if(myFace.eyewear){
            myFace.eyewear.transform.SetParent(head.transform);
        }
        head.transform.SetParent(null);
        head.transform.localScale = Vector3.one;
        
        // Be free
        SphereCollider collider = head.AddComponent<SphereCollider>();
        collider.radius = 0.35f;
        collider.gameObject.layer = 12; // Actor nav, so we can't walk on it again.
        Rigidbody newRb = head.AddComponent<Rigidbody>();
        newRb.constraints = RigidbodyConstraints.FreezePositionZ;
        return newRb;
    }
}