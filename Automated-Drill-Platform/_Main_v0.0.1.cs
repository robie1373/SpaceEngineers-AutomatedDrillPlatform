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
public string previous_platform_status;
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
  // set rotor velocity to some good value #TODO
  // set rotor angle to 0 #TODO
  previous_platform_status = "uninitialized";
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
      // test if entering boring state or continuieng
      // if entering
        // turn drills on
        // set y velocity to positive
        // extend piston (test if this is needed. setting velocity to positive should do it.)
        // set previous_platform_status to boring
        // break
      // else if continouing
        // test if it reached the bottom of the hole
          // if yes
           // change platform_status to lifting
           // break
          // else
            // echo status?
            //break
      break;
    case "lifting":
      // test if enteriong lifting or continueing
      // if entering
        // set y veloicty to -.5
        // set previous_platform_status to lifting
        // break
      // else
        // test if reached the top
        // if yes
          // change status to extending
          // break
        // else
          // echo status
          // break
      break;
    case "extending":
      // test if entering extending or continuing
      // if entering
        // test is maxlimit >= 10
        //if yes
          // set z velocity to -.5
          // set z max limit to stepsize (new variable needed)
          // if z position == 0 (fully retracted?)
            // change status to rotating
            // break
          // else
            // break
        //else start extending
          // set z velocity to positive
          // test is new maxlimit > 10?
            // if yes
              // set new z max limit to 10
            // else
              // set z MaxLimit to old_z_maxlimit + stepsize (new varialbe needed)
              // set previous_platform_status to extending
              // break
      // else we are continuing
        // test if reached new maxlimit
        // if yes
          // change status to boring
          // break
        // else
          // echo status
          // break
      break;
    case "rotating":
      //test if entering or continuing
      // if entering
        // new angle == current angle + 90
        // if new anggle >= 360 we've made a whole circle
          // set status to terminating
          // break
        // else 
          // set rotor angle to new angle
          // change previous_platform_status to rotating
          // break
      // else continuing
        // if current angle == new angle
          // change platform_status to boring
          // break
        // else still rotating
          // echo status
          // break
      break;
    case "terminating":
      // test if entering or contiuning
      // if entering
        // turn drills off
        // set previous_platform_status to terminating
        // if y position > 0
          // set y velocity negative something
          // break
        // else y is 0
          // if z > 0
            // set z velocity negative somethign
            // break
          // else z is also 0
            // echo zugzug
            // maybe we add a light we can flash red?
      break;
    default:
      Echo("Unknown state. Please check platform configuration and code.");
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