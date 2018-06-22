using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System;

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
        return result;
    } 

    public static string QSString(string input){
        string[] o = input.Split('"');
        return o[1];
    }

    public static string[] GetDest(string input){
        List<string> result = new List<string>();
        if(input.Contains("{") && input.Contains("}")){
            string csv = input.Substring(input.IndexOf("{")+1, input.IndexOf("}") - input.IndexOf("{") - 1 );
            string[] resultWithComma = csv.Split(',');
            foreach(string n in resultWithComma){
                if(n != ","){
                    result.Add(n);
                }
            }
        }
        
        return result.ToArray();
    }

    public static string GetTarget(string input){
        if(input.Contains("=>")){
            string[] t = input.Split(new string[] { "=>" }, StringSplitOptions.None);
            return t[0];
        }
        else
        {
            return string.Empty;
        }
    }

}