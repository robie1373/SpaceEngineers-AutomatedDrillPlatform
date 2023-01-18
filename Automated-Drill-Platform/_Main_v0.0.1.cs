// # The basis of this script was copied verbatim from https://github.com/throwawAPI/space-engineers-scripts
// # Credit to them

//// GLOBAL VARIABLES
// for transfering data between Runtime events

IMyBlockGroup ygroup;
IMyBlockGroup zgroup;
List<IMyTerminalBlock> yblocks = new List<IMyTerminalBlock>();
List<IMyTerminalBlock> zblocks = new List<IMyTerminalBlock>();

//// Program()
// for variable initialization, setup, etc.
public Program() {
  Runtime.UpdateFrequency = FREQ; // set from _Customize
  Initialize();
} // Program()

public void Initialize() {
  // run each Program__...() submethods here
  Program__GetPistons();

} // Initialize()

public void Save() {
} // Save()

public void Main(string argument, UpdateType updateSource) {
  // NOTE: multiple trigger sources can roll in on the same tick
  // test each trigger individually, not with if() else if () blocks

  if((updateSource & UpdateType.Update100) != 0) { // TODO: can != 0 be dropped? // had to delete source check due to compile error
    // run each Main__...() submethod here
  //Main__WriteDiagnostics(); // This method was in _Template
  }
} // Main()

public void Program__GetPistons() {
  ygroup = GridTerminalSystem.GetBlockGroupWithName(yPistonGroup);
  if (ygroup == null)
  {
    Echo("Y Group not found");
    return;
  }
  get_status(ygroup);

  zgroup = GridTerminalSystem.GetBlockGroupWithName(zPistonGroup);
  if (zgroup == null)
  {
    Echo("Z Group not found");
    return;
  }
  get_status(zgroup);

}

public void get_status(IMyBlockGroup group) {
  Echo($"{group.Name}:");
  List<IMyExtendedPistonBase> pistons = new List<IMyExtendedPistonBase>();
  group.GetBlocksOfType(pistons);
  foreach (var piston in pistons)
  {
    Echo($"- {piston.CustomName}");
    Echo($"position: {piston.CurrentPosition}");
    Echo($"MaxLimit: {piston.MaxLimit}");

  }
}