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
public const float yExtensionVelocity = .5F; // set to .05 or less for production
public const float yRetractionVelocity = -1.0F;
public const float zExtensionVelocity = .5F;
public const float zRetractionVelocity = -1.0F;
public const float zStepSize = 2.5F;
public float rotationStepRad = (Convert.ToSingle(Math.PI * 0.25));
public float new_zMaxLimit = 0F;