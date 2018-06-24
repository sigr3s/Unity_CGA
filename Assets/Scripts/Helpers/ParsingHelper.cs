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

    public static CSF CSFValues(string input){
        string csv = input.Substring(input.IndexOf("(")+1, input.IndexOf(")") - input.IndexOf("(") - 1 );
        string[] values =  csv.Split(',');
        CSF csf = new CSF { orderedCSF = new List<OCSF>(), fixedValues = new List<float>(), relativeValues = new List<int>() };
        float f;
        int fi;
        
        foreach(string v in values){
            
            if(v.Contains("r")){
                string intv = v.Replace("r", string.Empty);
                if(int.TryParse(intv, NumberStyles.Any, CultureInfo.InvariantCulture, out fi)){
                    csf.relativeValues.Add(fi);
                    OCSF ocsf = new OCSF { index = csf.relativeValues.Count-1,isRelative = true};
                    csf.orderedCSF.Add(ocsf);
                }
            }
            else{
                if(float.TryParse(v, NumberStyles.Any, CultureInfo.InvariantCulture, out f)){
                    csf.fixedValues.Add(f);
                    OCSF ocsf = new OCSF { index = csf.fixedValues.Count-1,isRelative = false};
                    csf.orderedCSF.Add(ocsf);
                }
            }
        }

        return csf;
    }

    public static float ExtractProbavility(string input){
        if(input.Contains(":")){
            string[] t = input.Split(new string[] { ":" }, StringSplitOptions.None);
            float f;
            if(float.TryParse(t[1], NumberStyles.Any, CultureInfo.InvariantCulture, out f)){
                return f;
            }
            else return 1f;
        }
        else return 1f;
    }

}

public struct CSF{
    public List<OCSF> orderedCSF;
    public List<float> fixedValues;
    public List<int> relativeValues;    
}

public struct OCSF{
    public int index;
    public bool isRelative;
}