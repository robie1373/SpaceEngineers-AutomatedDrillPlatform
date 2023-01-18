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
public const float yExtensionVelocity = .01;
public const float yRetractionVelocity = 1;
public const float zExtensionVelocity = .5;
public const float zRetractionVelocity = .5;