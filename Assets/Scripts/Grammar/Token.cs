
public class Token : DefinitionItem {

    public string text;
    public bool fuse;

    public Token(string text, bool fuse) {
        this.text = text;
        this.fuse = fuse;
    }

    public override string ToString() {
        if (fuse) {
            return "#" + text;
        }
        return text;
    }
}