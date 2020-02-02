using UnityEngine;
using System;
using UnityEditor;
using System.Collections.Generic;
/****************************************
	Material Overide v1.0
	Copyright 2013 Unluck Software	
 	www.chemicalbliss.com
 	
 	    A tool that makes it possible to attach two materials to a shuriken particle system 
        Can also be used to change from two to one materials 	
        																																
*****************************************/

[System.Serializable]
public class MaterialOverride: EditorWindow {
  
    
    public ParticleSystem systemParent;
    public Material material0;
    public Material material1;
    public float scaleMultiplier = 1.0f;
    
    [MenuItem("Window/Unluck Software - Material Override")]
    
    
    public static void ShowWindow() {
        EditorWindow.GetWindow (typeof(MaterialOverride));
    }
      
    public void OnGUI() {
    	if(Selection.activeTransform != null){
    	systemParent = Selection.activeTransform.GetComponent<ParticleSystem>();
    		
    	}
    	EditorGUILayout.LabelField("Force 2 or 1 material(s) on shuriken particle system(s)");
    	EditorGUILayout.LabelField("(consider using 1 material for slower devices)");
    	EditorGUILayout.Space();
    	EditorGUILayout.LabelField("Get materials from shuriken particle system in scene");
       // systemParent = EditorGUILayout.ObjectField ("Particle System: ", systemParent, typeof (ParticleSystem), true) as ParticleSystem;
        if (GUILayout.Button("Get Materials"))
        {
            getMaterials();
        }
       
    	material0 = EditorGUILayout.ObjectField ("Material 0: ", material0, typeof (Material), true) as Material;
    	material1 = EditorGUILayout.ObjectField ("Material 1: ", material1, typeof (Material), true) as Material;
    	EditorGUILayout.Space();
    	EditorGUILayout.LabelField("Apply materials to selected shuriken particle system(s)");
    	if (GUILayout.Button("Set Materials"))
        {
            overideMaterials();
        }
    }
    
    public void overideMaterials() {
    
			List<Material> newArray = new List<Material>();
			if(material0 != null){
				newArray.Add(material0);
			}
			if(material1 != null){
				newArray.Add(material1);
			}
			if(newArray.Count > 0){
				for(int i=0; i < Selection.transforms.Length; i++){
				//	Debug.Log(i+""+Selection.transforms.Length);
					if(Selection.transforms[i].GetComponent(typeof(ParticleSystemRenderer))!=null)
					Selection.transforms[i].GetComponent<ParticleSystemRenderer>().sharedMaterials= newArray.ToArray();
				}
           		//systemParent.GetComponent(ParticleSystemRenderer).sharedMaterials= newArray;
            }
    }
    
    public void getMaterials() {
    	if(systemParent != null){
    		if(systemParent.GetComponent<ParticleSystemRenderer>().sharedMaterials[0] != null){
    			material0=(Material)systemParent.GetComponent<ParticleSystemRenderer>().sharedMaterials[0];
    		}
    		if(systemParent.GetComponent<ParticleSystemRenderer>().sharedMaterials[1] != null){
    			material1=(Material)systemParent.GetComponent<ParticleSystemRenderer>().sharedMaterials[1];
    		}
    	}
    }
}