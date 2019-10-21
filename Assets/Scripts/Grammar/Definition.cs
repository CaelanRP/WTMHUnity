
using System.Collections.Generic;

public class Definition {
    public List<DefinitionItem> items;
    public string[] checkedFeatures;
    public Definition(List<DefinitionItem> items, string[] checkedFeatures) {
        this.items = items;
        this.checkedFeatures = checkedFeatures;
    }
}