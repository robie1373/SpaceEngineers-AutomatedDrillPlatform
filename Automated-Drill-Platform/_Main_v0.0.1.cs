// # The basis of this script was copied verbatim from https://github.com/throwawAPI/space-engineers-scripts
// # Credit to them

//// GLOBAL VARIABLES
// for transfering data between Runtime events

List<IMyExtendedPistonBase> ygroup;
List<IMyExtendedPistonBase> zgroup;

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

//// Main()
// called when the Programmable Block is "Run",
// or automatically by UpdateFrequency
public void Main(string argument, UpdateType updateSource) {
  // NOTE: multiple trigger sources can roll in on the same tick
  // test each trigger individually, not with if() else if () blocks

  if((updateSource & UpdateType.Update100) != 0) { // TODO: can != 0 be dropped? // had to delete source check due to compile error
    // run each Main__...() submethod here
  //Main__WriteDiagnostics(); // This method was in _Template
  Echo($"Y position: {ygroup[0].CurrentPosition}");
  Echo($"Z position: {zgroup[0].CurrentPosition}");
  }
} // Main()

public void Program__GetPistons() {
  ygroup = GridTerminalSystem.GetBlockGroupWithName(yPistonGroup) as IMyExtendedPistonBase;
  if (ygroup == null)
  {
    Echo("Y Group not found");
    return;
  }
  delete__me(ygroup);

  zgroup = GridTerminalSystem.GetBlockGroupWithName(zPistonGroup) as IMyExtendedPistonBase;
  if (zgroup == null)
  {
    Echo("Z Group not found");
    return;
  }
  delete__me(zgroup);

}

public void delete__me(IMyBlockGroup group) {
  Echo($"{group.Name}:");
  List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
  group.GetBlocks(blocks);
  foreach (var block in blocks)
  {
    Echo($"- {block.CustomName}");
  }
}