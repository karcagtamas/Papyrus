namespace Papyrus.Shared.DiffMatchPatch;

public class TransportDiff
{
    public Operation Operation { get; set; }
    public string Text { get; set; }

    public TransportDiff()
    {
        Text = "";
    }

    public TransportDiff(Diff diff)
    {
        Operation = diff.Operation;
        Text = diff.Text;
    }
}
