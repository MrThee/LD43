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
                this.myFace.eyes.left = Instantiate<GameObject>(face.eyes.left, myHead.transform, false);
            }
            if(face.eyes.right){
                this.myFace.eyes.right = Instantiate<GameObject>(face.eyes.right, myHead.transform, false);
            }
        }
        if(face.mouth){
            this.myFace.mouth = Instantiate<GameObject>(face.mouth, myHead.transform, false);
        }
        if(face.nose){
            this.myFace.nose = Instantiate<GameObject>(face.nose, myHead.transform, false);
        }
        if(face.eyewear){
            this.myFace.eyewear = Instantiate<GameObject>(face.eyewear, myHead.transform, false);
        }
    }

    public Rigidbody Decapitate() {
        GameObject head = myHead;
        head.transform.SetParent(null);
        head.transform.localScale = Vector3.one;
        
        // Be free
        SphereCollider collider = head.AddComponent<SphereCollider>();
        collider.radius = 0.35f;
        collider.gameObject.layer = 13; // // Rigidbody. player can't walk on, but fruits will 
        // collider w/ other fruits.
        Rigidbody newRb = head.AddComponent<Rigidbody>();
        newRb.constraints = RigidbodyConstraints.FreezePositionZ;
        return newRb;
    }
}