using System.IO;
using System;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SproutNS;

public class SPObject {
    public Dictionary<string, dynamic> Variables = new Dictionary<string, dynamic>();
    
    public dynamic this[string key] {
        get {
            if (Variables.ContainsKey(key)) return Variables[key];
            else return null;
        } set {
            Variables[key] = value;
        }
    }

    public override string ToString() => Sprout.ObjectToString(this);
    public string ToString(int indent, int tabSize) => Sprout.ObjectToString(this, indent, tabSize);
}
