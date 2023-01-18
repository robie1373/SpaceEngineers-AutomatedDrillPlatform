// # The basis of this script was copied verbatim from https://github.com/throwawAPI/space-engineers-scripts
// # Credit to them

//// GLOBAL VARIABLES
// for transfering data between Runtime events

IMyBlockGroup ygroup;
IMyBlockGroup zgroup;
List<IMyExtendedPistonBase> ypistons = new List<IMyExtendedPistonBase>();
List<IMyExtendedPistonBase> zpistons = new List<IMyExtendedPistonBase>();
List<IMyShipDrill> drills = new List<IMyShipDrill>();
public string platform_status; // Use for state machine controlling platform
// boring -> y extending drills on
// lifting -> y retracting drills on
// extending -> z extending drills on
// rotating -> rotor moving drills on
// terminating -> z retracting drills off


//// Program()
// for variable initialization, setup, etc.
public Program() {
  Runtime.UpdateFrequency = FREQ; // set from _Customize
  Initialize();
} // Program()

public void Initialize() {
  // run each Program__...() submethods here
  Program__GetPistons();
  //List<IMyShipDrill> drills = new List<IMyShipDrill>();
  GridTerminalSystem.GetBlocksOfType(drills);
  foreach (var drill in drills)
  {
    drill.ApplyAction("OnOff_On");
  }
  set_velocity(ypistons, yExtensionVelocity);
  set_velocity(zpistons, zExtensionVelocity);
  platform_status = "boring";
} // Initialize()

public void Save() {
} // Save()

public void Main(string argument, UpdateType updateSource) {
  // NOTE: multiple trigger sources can roll in on the same tick
  // test each trigger individually, not with if() else if () blocks
  if((updateSource & UpdateType.Update100) != 0) { // TODO: can != 0 be dropped? 
    // run each Main__...() submethod here
    set_max_limit(zpistons, 2.5F);
    extend_piston(zpistons);
    if (is_at_full_extension(ypistons)) {
      Echo("hole is deep");
      set_velocity(ypistons, -0.5F);
      all_stop();
    } else {
      Echo("hole not deep");
    }
        get_status(ypistons);
  }
  switch (platform_status) {
    case "boring":
      //do things
      break;
    case "lifting":
      // do things
      break;
    case "extending":
      //do things
      break;
    case "rotating":
      //do things
      break;
    case "terminating":
      //do things
      break;
    default:
      //do things
      break;
  }
} // Main()

public void Program__GetPistons() {

  ygroup = GridTerminalSystem.GetBlockGroupWithName(yPistonGroup);
  if (ygroup == null)
  {
    Echo("Y Group not found");
    return;
  }
  //var ypistons = make_list_from_group(ygroup, ypistons);
  make_list_from_group(ygroup, ypistons);
  get_status(ypistons);

  zgroup = GridTerminalSystem.GetBlockGroupWithName(zPistonGroup);
  if (zgroup == null)
  {
    Echo("Z Group not found");
    return;
  }
  //var zpistons = make_list_from_group(zgroup, zpistons);
  make_list_from_group(zgroup, zpistons);
  get_status(zpistons);
}

public void make_list_from_group(IMyBlockGroup group, List<IMyExtendedPistonBase> pistons) {
  //List<IMyExtendedPistonBase> pistons = new List<IMyExtendedPistonBase>();
  group.GetBlocksOfType(pistons);
  //return pistons;
}

public void get_status(List<IMyExtendedPistonBase> pistons) {
  //Echo($"{group.Name}:");
  //var pistons = make_list_from_group(group);
  foreach (var piston in pistons)
  {
    Echo($"- {piston.CustomName}");
    Echo($"position: {piston.CurrentPosition}");
    Echo($"MaxLimit: {piston.MaxLimit}");
    Echo($"Velocity: {piston.Velocity}");
  }
}

public void set_max_limit(List<IMyExtendedPistonBase> pistons, float new_limit) {
 foreach (var piston in pistons)
  {
    //Echo($"Old MaxLimit: {piston.MaxLimit}");
    piston.MaxLimit = new_limit;
    //Echo($"New MaxLimit: {piston.MaxLimit}");
  }
}

public void set_velocity(List<IMyExtendedPistonBase> pistons, float new_velocity) {
  foreach (var piston in pistons)
  {
    piston.Velocity = new_velocity;
  }
}

public void extend_piston(List<IMyExtendedPistonBase> pistons) {
  foreach (var piston in pistons)
  {
    piston.Extend();
  }
}

public void all_stop() {
 foreach (var drill in drills)
  {
    drill.ApplyAction("OnOff_Off");
    Echo("All drills stopped");
    platform_status = "terminating";
  }
}

public bool is_at_full_extension(List<IMyExtendedPistonBase> pistons)
{
  if (pistons[0].CurrentPosition == 10) {
    return true;
  }
  else {
    return false;
  }
}