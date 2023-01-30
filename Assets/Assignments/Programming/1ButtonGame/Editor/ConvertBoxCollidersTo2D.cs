 using UnityEngine;
 using UnityEditor;
 
 public class ConvertBoxCollidersTo2D : MonoBehaviour {
 
     [MenuItem("Window/ScaryBee/ConvertBoxCollidersTo2D")]
     public static void Convert(){
         BoxCollider[] colliders = Resources.FindObjectsOfTypeAll<BoxCollider>();
         Debug.Log("got colliders: "+colliders.Length);
 
         foreach(BoxCollider b in colliders){
             GameObject g = b.gameObject;
             Vector3 size = b.size;
             DestroyImmediate(b, true);
 
             BoxCollider2D b2d = g.AddComponent<BoxCollider2D>();
             b2d.size = new Vector2(size.x, size.y);
         }
     }
 
 }