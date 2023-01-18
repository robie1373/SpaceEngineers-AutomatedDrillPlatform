//
// XXX Automated-Drill-Platform//_Customize.cs XXX
//

// # The basis of this script was copied verbatim from https://github.com/throwawAPI/space-engineers-scripts
// # Credit to them

// EDIT THESE VARIABLES

public const string VERSION = "Template v0.1";
public const UpdateFrequency FREQ = UpdateFrequency.Update100;

// Piston group names
public const string yPistonGroup = "yPistons";
public const string zPistonGroup = "zPistons";

// Set desired per-piston velocities here.
// Note: total velocity of the drill head is the 
// number of pistons in the group multiplied by this velocity.
// In game I have not been able to determine if this value can
// be less than .01
public const float yExtensionVelocity = .01F;
public const float yRetractionVelocity = 1.0F;
public const float zExtensionVelocity = .5F;
public const float zRetractionVelocity = .5F;
//
// XXX Automated-Drill-Platform//_Main_v0.0.1.cs XXX
//

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
//
// XXX Automated-Drill-Platform//_Template_v0.0.1.cs XXX
//

// # The basis of this script was copied verbatim from https://github.com/throwawAPI/space-engineers-scripts
// # Credit to them

// Left blank for now.
// If this explodes things, then get it from 
// https://github.com/throwawAPI/space-engineers-scripts

