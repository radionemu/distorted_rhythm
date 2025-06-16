public enum NoteType
{
	INVALID,
	NM, //Normal Note
	CS, //Charge Note Start 
	CE, //Charge Note End
	MT // Mute Note
}

public struct Note
{

	public int lane;
	// 1~4 normal lane, 5 Mute Lane
	public NoteType nType;
	//1 Short Note, 2 Long Note start 3 Long Note End
	public int section;
	public int nom;
	public int denom;

	public int objID;

	public Note(string[] tokens)
	{
		nType = tokens[0] switch
		{
			"NM" => NoteType.NM,
			"CS" => NoteType.CS,
			"CE" => NoteType.CE,
			"MT" => NoteType.MT,
			_ => NoteType.INVALID,
		};
		lane = int.Parse(tokens[1]);
		section = int.Parse(tokens[2]);
		nom = int.Parse(tokens[3]);
		denom = int.Parse(tokens[4]);
		objID = -1;
	}

	public void setID(int id)
	{
		objID = id;
	}
}