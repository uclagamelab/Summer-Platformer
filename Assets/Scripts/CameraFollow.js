/*
This camera smoothes out rotation around the y-axis and height.
Horizontal Distance to the target is always fixed.

There are many different ways to smooth the rotation but doing it this way gives you a lot of control over how the camera behaves.

For every of those smoothed values we calculate the wanted value and the current value.
Then we smooth it using the Lerp function.
Then we apply the smoothed values to the transform's position.
*/

// The target we are following
var target : Transform;
var cameraStartPosition : Transform;

// measured from center of camera's position
var positiveYBounds = 3.0; // 
var negativeYBounds = -3.0;
var negativeXBounds = -3.0;
var positiveXBounds = 3.0;

var drawBounds : boolean = false;

// How much we 
var ySpeed = 2000.0;
var xSpeed = 2000.0;
var xDistance = 0.0;
var yDistance = 0.0;
//var rotationDamping = 3.0; // take out rotation stuff
var relpos = Vector3.zero;

// Place the script in the Camera-Control group in the component menu
@script AddComponentMenu("Camera-Control/Camera Follow")

function Start() {
	// check some of the values before starting
	if (negativeXBounds > positiveXBounds) {
		Debug.LogError("Camera Follow: negativeXBounds(" + negativeXBounds + ") can't be greater than positiveXBounds(" + positiveXBounds);
	}
	if (negativeYBounds > positiveYBounds) {
		Debug.LogError("Camera Follow: negativeYBounds(" + negativeYBounds + ") can't be greater than positiveYBounds(" + positiveYBounds);
	}
	if (!cameraStartPosition) {
		Debug.LogWarning("Camera Follow: no cameraStartPosition found.");
	}
	else {
		transform.position = cameraStartPosition.position;
	}
}

function LateUpdate () {
	// Early out if we don't have a target
	if (!target)
		return;
	
	xDistance = transform.position.x - target.position.x;
	yDistance = transform.position.y - target.position.y;
	
	if (-xDistance < negativeXBounds) {
		transform.position  = Vector3.Lerp(transform.position, Vector3(transform.position.x - (xDistance + negativeXBounds), transform.position.y, transform.position.z), xSpeed *Time.deltaTime);
	}
	else if (-xDistance > positiveXBounds) { 
		transform.position  = Vector3.Lerp(transform.position, Vector3(transform.position.x + (-xDistance-positiveXBounds), transform.position.y, transform.position.z), xSpeed * Time.deltaTime);

	}
	
	//if (-yDistance < positiveYBounds || -yDistance > negativeYBounds ) { 
	///if ( (target.position.y - transform.position.y ) > positiveYBounds || (transform.position.y - target.position.y) > -negativeYBounds ) {
	if (-yDistance < negativeYBounds) {
		transform.position = Vector3.Lerp(transform.position, Vector3(transform.position.x, transform.position.y - (yDistance + negativeYBounds), transform.position.z), ySpeed * Time.deltaTime);
	}
	else if (-yDistance > positiveYBounds) {
		transform.position = Vector3.Lerp(transform.position, Vector3(transform.position.x, transform.position.y + (-yDistance - positiveYBounds), transform.position.z), ySpeed * Time.deltaTime);
	}

}

/***
* OnDrawGizmosSelected
*	draw a line on the screen for the current bounding area, one for top, bottom and one each for sides
***/
function OnDrawGizmosSelected () {
	if (drawBounds) {
		Gizmos.color = Color.yellow;
		var targetDepth = Vector3.forward * (transform.position.z - target.position.z);
		// draw the top bounding line
		Gizmos.DrawLine(transform.position+(Vector3.up*positiveYBounds)+(Vector3.right*100) - targetDepth, transform.position+(Vector3.up*positiveYBounds)+(Vector3.right*-100)-targetDepth);
		
		// draw the bottom bounding line
		Gizmos.DrawLine(transform.position+(Vector3.up*negativeYBounds)+(Vector3.right*100) - targetDepth, transform.position+(Vector3.up*negativeYBounds)+(Vector3.right*-100)-targetDepth);

		// draw the positive x bounding line
		Gizmos.DrawLine(transform.position+(Vector3.right*positiveXBounds)+(Vector3.up*100) - targetDepth, transform.position+(Vector3.right*positiveXBounds)+(Vector3.up*-100)-targetDepth);
		
		// draw the negative bounding line
		Gizmos.DrawLine(transform.position+(Vector3.right*negativeXBounds)+(Vector3.up*100) - targetDepth, transform.position+(Vector3.right*negativeXBounds)+(Vector3.up*-100)-targetDepth);
	}
}