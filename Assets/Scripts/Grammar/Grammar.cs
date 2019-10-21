using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grammar
{
    private Dictionary<string, List<Definition>> rules;
    private Dictionary<string, List<string>> exclusions;

    public Grammar(params string[] sourceFilenames) {
        this.rules = new Dictionary<string, List<Definition>>();
        this.exclusions = new Dictionary<string, List<string>>();
        foreach (string filename in sourceFilenames) {
            TextAsset texAss = (TextAsset) Resources.Load(filename);
            foreach (string lineRaw in texAss.text.Split('\n')) {
                string line = lineRaw.TrimEnd();
                if (line.StartsWith("#EXCLUDE ")) {
                    string[] excludes = line.Substring(9).Split(',');
                    for (int i = 0; i < excludes.Length; ++i) {
                        List<string> currExs = new List<string>();
                        for (int j = 0; j < i; ++j) {
                            currExs.Add(excludes[j]);
                        }
                        for (int j = i+1; j < excludes.Length; ++j) {
                            currExs.Add(excludes[j]);
                        }
                        this.exclusions[excludes[i]] = currExs;
                    }
                } else if (line == "") {
                    continue;
                } else {
                    string[] ruleAndBody = line.Split(':');
                    string rule = ruleAndBody[0];
                    string body = ruleAndBody[1];
                    string[] optRuleSplit = rule.Split('!');
                    string[] checkedFeatures = null;
                    if (optRuleSplit.Length > 1) {
                        rule = optRuleSplit[0];
                        checkedFeatures = optRuleSplit[1].Split(',');
                    }
                    string[] bodies = body.Split('|');
                    List<Definition> defs = null;
                    if (!rules.TryGetValue(rule, out defs)) {
                        defs = new List<Definition>();
                        rules.Add(rule, defs);
                    }
                    foreach (string subBody in bodies) {
                        List<DefinitionItem> items = new List<DefinitionItem>();
                        string[] chunks = subBody.Trim().Split(' ');
                        foreach (string chunk in chunks) {
                            if (chunk.StartsWith("@")) {
                                string refRuleName = chunk.Substring(1);
                                string[] optRefSplit = refRuleName.Split('!');
                                string[] features = null;
                                bool island = false;
                                if (optRefSplit.Length > 1) {
                                    refRuleName = optRefSplit[0];
                                    features = optRefSplit[1].Split(',');
                                }
                                if (refRuleName.EndsWith("*")) {
                                    refRuleName = refRuleName.Substring(0, refRuleName.Length - 1);
                                    island = true;
                                }
                                items.Add(new Ref(refRuleName, features, island));
                            } else if (chunk.StartsWith("#")) {
                                items.Add(new Token(chunk.Substring(1), true));
                            } else{
                                items.Add(new Token(chunk, false));
                            }
                        }
                        defs.Add(new Definition(items, checkedFeatures));
                    }
                }
            }
        }
        foreach (string rule in rules.Keys) {
            foreach (Definition defn in rules[rule]) {
                foreach (DefinitionItem item in defn.items) {
                    if (item is Ref) {
                        if (!rules.ContainsKey(((Ref) item).ruleName)) {
                            Debug.LogError("Reference to undefined rule \""+((Ref) item).ruleName+"\" in definition for rule \""+rule+"\""); 
                        }
                    }
                }
            }
        }
    }

    bool CheckFeatures(string[] checkedFeatures, List<string> activeFeatures) {
        if (checkedFeatures != null) {
            foreach (string feature in checkedFeatures) {
                if (exclusions.ContainsKey(feature)) {
                    foreach (string excluded in exclusions[feature]) {
                        if (activeFeatures.Contains(excluded)) {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }

    class GenerationCons {
        public Token head;
        public GenerationCons tail;
        public GenerationCons(Token head, GenerationCons tail) {
            this.head = head;
            this.tail = tail;
        }

        public string Flatten() {
            List<Token> tokens = new List<Token>();
            GenerationCons curr = this;
            while (curr != null) {
                tokens.Add(curr.head);
                curr = curr.tail;
            }
            tokens.Reverse();
            string s = "";
            foreach (Token token in tokens) {
                if (!token.fuse) {
                    s += " ";
                }
                s += token.text;
            }
            return s.TrimStart(' ');
        }
    }

    class Context {
        public Grammar grammar;
        public Context parent;
        public Definition currDefn;
        public int currDefnPos;
        public GenerationCons generated;
        public List<string> activeFeatures;
        public Context(Grammar grammar, Context parent, Definition currDefn, int currDefnPos, GenerationCons generated, List<string> activeFeatures) {
            this.grammar = grammar;
            this.parent = parent;
            this.currDefn = currDefn;
            this.currDefnPos = currDefnPos;
            this.generated = generated;
            this.activeFeatures = activeFeatures;
        }
        public bool IsFinished() {
            return currDefnPos == currDefn.items.Count && parent == null;
        }
        public void Step(List<Context> queue) {
            while (currDefnPos == currDefn.items.Count) {
                if (parent == null) {
                    queue.Add(this);
                    return;
                }
                this.currDefn = parent.currDefn;
                this.currDefnPos = parent.currDefnPos;
                if (((Ref) currDefn.items[currDefnPos - 1]).island) {
                    this.activeFeatures = parent.activeFeatures;
                }
                this.parent = parent.parent;
            }
            DefinitionItem currItem = currDefn.items[currDefnPos];
            ++this.currDefnPos;
            if (currItem is Token) {
                this.generated = new GenerationCons((Token) currItem, generated);
                queue.Add(this);
            } else if (currItem is Ref) {
                Ref currRef = (Ref) currItem;
                List<Definition> candidateDefns = grammar.rules[currRef.ruleName];
                List<string> newFeats = currRef.island ? new List<string>() : new List<string>(activeFeatures);
                if (currRef.features != null) {
                    foreach (string feat in currRef.features) {
                        if (grammar.exclusions.ContainsKey(feat)) {
                            foreach (string excluded in grammar.exclusions[feat]) {
                                newFeats.Remove(excluded);
                            }
                        }
                    }
                    newFeats.AddRange(currRef.features);
                }
                bool children = false;
                foreach (Definition candidate in candidateDefns) {
                    if (grammar.CheckFeatures(candidate.checkedFeatures, newFeats)) {
                        if (parent == null || !(candidate == currDefn && currDefn == parent.currDefn)) {
                            children = true;
                            List<string> newFeatsSpec = newFeats;
                            if (candidate.checkedFeatures != null) {
                                bool copied = false;
                                foreach (string checkedFeat in candidate.checkedFeatures) {
                                    if (!newFeats.Contains(checkedFeat)) {
                                        if (!copied) {
                                            copied = true;
                                            newFeatsSpec = new List<string>(newFeats);
                                        }
                                        newFeatsSpec.Add(checkedFeat);
                                    }
                                }
                            }
                            queue.Add(new Context(grammar, this, candidate, 0, generated, newFeatsSpec));
                        }
                    }
                }
                if (!children) {
                    Debug.LogWarning("Dead end at ref "+currItem.ToString()+" with active features "+newFeats.ToString()+", generated so far: "+generated.Flatten());
                }
            }
        }
    }

    public string Sample(string ruleName) {
        List<Definition> defns = rules[ruleName];
        Context ctx = new Context(this, null, defns[Random.Range(0, defns.Count)], 0, null, new List<string>());
        List<Context> queue = new List<Context>();
        while (!ctx.IsFinished()) {
            ctx.Step(queue);
            ctx = queue[Random.Range(0, queue.Count)];
            queue.Clear();
        }
        return ctx.generated.Flatten();
    }
}
