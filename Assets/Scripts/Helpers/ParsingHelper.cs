using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class ParsingHelper{
   public static Vector3 CSVector3(string input){
        Vector3 result = Vector3.zero;
        string csv = input.Substring(input.IndexOf("(")+1, input.IndexOf(")") - input.IndexOf("(") - 1 );
        string[] values =  csv.Split(',');
        int ind = 0;
        foreach(string v in values){
            if(v != ","){
                float value = float.Parse(v, CultureInfo.InvariantCulture);
                result[ind] = value;
                ind++;
            }
        }
        
        Debug.Log(result);
        return result;
   } 

   public static string QSString(string input){
        string[] o = input.Split('"');
        return o[1];
   }
}