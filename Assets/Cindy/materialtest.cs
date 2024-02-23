using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class materialtest : MonoBehaviour
{
    //Material correct;
    //Material wrong;
    Material[] correctMaterials;
    // Start is called before the first frame update
    void OnEnable()
    {
        correctMaterials = Resources.LoadAll<Material>("Assets/3D");
        //correctMaterials= AssetDatabase.LoadAllAssetsAtPath<Material>("Assets/3D")
        //if (gameObject.GetComponent<Renderer>())
        //{
        //    wrong = GetComponent<Renderer>().material;
        //    Debug.Log("wrong:" + wrong.name);
        //    if (wrong.name.Contains("Instance"))
        //    {
        //        string tempName = wrong.name.Replace("(Instance)", "");
        //        string realName = tempName.Replace(" ", "");
        //        Debug.Log(realName);
        //        foreach (Material mat in correctMaterials)
        //        {
        //            if (mat.name == realName)
        //            {
        //                Debug.Log("Found this: " + mat.name);
        //                correct = mat;
        //            }

        //        }
        //        GetComponent<Renderer>().material = null;
        //        GetComponent<Renderer>().sharedMaterial = correct;
        //        Destroy(wrong);
        //    }
        //}
        Renderer[] childrenRends = GetComponentsInChildren<Renderer>();
        foreach(Renderer r in childrenRends)
        {
            ReplaceMat(r);
        }
    }

    void ReplaceMat(Renderer rend)
    {
        Material wrong;
        Material correct;
        wrong = rend.material;
        if (wrong.name.Contains("Instance"))
        {
            string tempName = wrong.name.Replace("(Instance)", "");
            string realName = tempName.Replace(" ", "");
            //Debug.Log(realName);
            foreach (Material mat in correctMaterials)
            {
                if (mat.name == realName)
                {
                    //Debug.Log("Found this: " + mat.name);
                    correct = mat;
                    rend.material = null;
                    rend.sharedMaterial = correct;
                    Destroy(wrong);
                }

            }
            //rend.material = null;
            //rend.sharedMaterial = correct;
            //Destroy(wrong);
        }
        
    }
}
