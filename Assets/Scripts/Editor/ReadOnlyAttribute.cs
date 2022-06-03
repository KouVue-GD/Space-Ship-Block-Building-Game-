using UnityEngine;
using UnityEditor;

/*

https://answers.unity.com/questions/489942/how-to-make-a-readonly-property-in-inspector.html

 Answer by It3ration · Oct 01, 2014 at 05:37 PM 
 Answer by Lev-Lukomskyi · Sep 19, 2014 at 07:30 PM 


    Just put [ReadOnlyAttribute] above your variable such as 

    [SerializeField, ReadOnlyAttribute]
    float currHealth;
*/

 public class ReadOnlyAttribute : PropertyAttribute
 {
 
 }
