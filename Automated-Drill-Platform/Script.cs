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
public const float yRetractionVelocity = -1.0F;
public const float zExtensionVelocity = .5F;
public const float zRetractionVelocity = -1F;
public const float zStepSize = 2.5F;
public float rotationStepRad = (Convert.ToSingle(Math.PI * 0.25));
//
// XXX Automated-Drill-Platform//_Main_v0.0.1.cs XXX
//

// # The basis of this script was copied verbatim from https://github.com/throwawAPI/space-engineers-scripts
// # Credit to them

//// GLOBAL VARIABLES
// for transfering data between Runtime events

IMyBlockGroup ygroup;
IMyBlockGroup zgroup;
List<IMyExtendedPistonBase> ypistons = new List<IMyExtendedPistonBase>();
List<IMyExtendedPistonBase> zpistons = new List<IMyExtendedPistonBase>();
List<IMyShipDrill> drills = new List<IMyShipDrill>();
List<IMyMotorAdvancedStator> rotors = new List<IMyMotorAdvancedStator>();
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
  GridTerminalSystem.GetBlocksOfType(rotors);
  // set rotor velocity to some good value #TODO
  rotors[0].UpperLimitDeg = 0F; // set rotor angle to 0 #TODO
  GridTerminalSystem.GetBlocksOfType(drills);
  start_drills();
  set_velocity(ypistons, yExtensionVelocity);
  set_velocity(zpistons, zExtensionVelocity);
  set_max_limit(ypistons, 10F);
  set_max_limit(zpistons, 0F);
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
    /////////////////////////////////////////////
    ///// temporary main. delete this when switch is complete
    ///// or for testing switch/////////////////
    //////////////////////////////
    //set_max_limit(zpistons, 2.5F);
    //extend_piston(zpistons);
    //if (is_at_full_extension(ypistons)) {
    //  Echo("hole is deep");
    //  set_velocity(ypistons, -0.5F);
    //  stop_drills();
    //();
    //} else {
    //  Echo("hole not deep");
    //}
    //    get_status(ypistons);
  
  ////////////////// end of temporary main /////////
  //////////////////////////////////////////////////
    switch (platform_status) {
      case "boring":
        // test if entering boring state or continuieng
        if (platform_status != previous_platform_status) {
          start_drills();        
          set_velocity(ypistons, yExtensionVelocity);
          extend_piston(ypistons);
          previous_platform_status = "boring";
          break;
        }
          
        else {  // continouing
          // test if it reached the bottom of the hole
          if (is_at_full_extension(ypistons)) {
            platform_status = "lifting";
            break;
          }
          else {
            get_status(ypistons);
            break;
          }
        }
        //break;

      case "lifting":
        // test if enteriong lifting or continueing
        if (platform_status != previous_platform_status) {        
          set_velocity(ypistons, yRetractionVelocity);
          previous_platform_status = "lifting";
          break;
        } else { // continuing to lift
          // test if reached the top
          if (ypistons[0].CurrentPosition == 0F) {
            platform_status = "extending";
            break;
          } else {
            Echo("Y extension exceeded. Lifting.");
            get_status(ypistons);
            break;
          }
        }
        //break;

      case "extending":
        float new_MaxLimit = zpistons[0].MaxLimit + zStepSize;
        // test if entering extending or continuing
        if (platform_status != previous_platform_status) {
          // test is maxlimit >= 10
          if (is_at_full_extension(zpistons)) {
            set_velocity(zpistons, zRetractionVelocity);// set z velocity to -.5
            set_max_limit(zpistons, 0F); // set z max limit to stepsize (new variable needed)
            if (zpistons[0].CurrentPosition == 0) {//(fully retracted?)
              platform_status = "rotating";
              break;
            } else {
              Echo("Z extension exceeded. Retracting");
              get_status(zpistons);
              break;
            }
          } else { //start extending
            set_velocity(zpistons, zExtensionVelocity); // set z velocity to positive
            // test is new maxlimit > 10?
              if (new_MaxLimit > 10F) {
                set_max_limit(zpistons, 10F); // set new z max limit to 10
              } else {
                set_max_limit(zpistons, new_MaxLimit); 
                previous_platform_status = "extending";
                break;
              }
          }
        } else { //we are continuing
          // test if reached new maxlimit
          if (zpistons[0].CurrentPosition == new_MaxLimit) {
            platform_status = "boring";
            break;
          } else {
            get_status(zpistons);
            break;
          }
        }
        break;

      case "rotating":
      float new_angle = rotors[0].UpperLimitRad + rotationStepRad;
        //test if entering or continuing
        if (platform_status != previous_platform_status) {
          if (new_angle >= (2 * Math.PI)) { //we've made a whole circle
            platform_status = "terminating";
            break;
          } else {
            rotors[0].UpperLimitRad = new_angle; // set rotor angle to new angle
            previous_platform_status = "rotating";
            break;
          }
        } else { //continuing
          if (rotors[0].Angle == new_angle) {
            platform_status = "boring";
            break;
          } else { //still rotating
            Echo($"Rotor at {rotors[0].Angle}");
            break;
          }
        }
        //break;

      case "terminating":
        // test if entering or contiuning
        if (platform_status != previous_platform_status) {
          stop_drills();// turn drills off
          previous_platform_status = "terminating";
        } else {
          if (ypistons[0].CurrentPosition > 0F) {
            set_velocity(ypistons, yRetractionVelocity);
            break;
          } else { //y is 0
            if (zpistons[0].CurrentPosition > 0) {
              set_velocity(zpistons, zRetractionVelocity);
              break;
            } else { //z is also 0
              Echo(" zugzug");
              // maybe we add a light we can flash red?
              break;
            }
          }
        }
        break;
      default:
        Echo("Unknown state. Please check platform configuration and code.");
        break;
    }
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

public void stop_drills() {
 foreach (var drill in drills)
  {
    drill.ApplyAction("OnOff_Off");
    Echo("All drills stopped");
  }
}

public void start_drills() {
 foreach (var drill in drills)
  {
    drill.ApplyAction("OnOff_On");
    Echo("All drills started");
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
//
// XXX Automated-Drill-Platform//_Template_v0.0.1.cs XXX
//

// # The basis of this script was copied verbatim from https://github.com/throwawAPI/space-engineers-scripts
// # Credit to them

// Left blank for now.
// If this explodes things, then get it from 
// https://github.com/throwawAPI/space-engineers-scripts

