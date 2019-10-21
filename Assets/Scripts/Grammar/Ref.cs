using System.Collections.Generic;

public class Ref : DefinitionItem {
    public string ruleName;
    public string[] features;
    public bool island;
    public Ref(string ruleName, string[] features, bool island) {
        this.ruleName = ruleName;
        this.features = features;
        this.island = island;
    }

    public override string ToString() {
        string s = "@" + ruleName;
        if (island) {
            s += "*";
        }
        if (features != null) {
            s += "!" + features[0];
            for (int i = 1; i < features.Length; ++i) {
                s += ",";
                s += features[i];
            }
        }
        return s;
    }
}