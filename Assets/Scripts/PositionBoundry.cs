/* PositionBoundry
 * 
 * define the max and min position for each axis.
 * on late update, if game object's transform is
 * out of bounds, it will be corrected.
 * 
 * additionally, a gameobject with a PositionBoundry
 * component can be designated and the bounds will
 * be closest to the orgin. this allows for a 
 * master "LevelBoundry" object that no moving objects
 * can leave.
 *
 * otherPositionBoundry is considered only at Start, so
 * if a change happens to those values, they won't affect
 * connected PositionBoundry(s).
 */

using UnityEngine;
using System.Collections;

public class PositionBoundry : MonoBehaviour {
	// values settable for this game object
	public float minX = -100.0f;
	public float maxX = 100.0f;
	public float minY = -10.0f;
	public float maxY = 20.0f;
	public float minZ = -100.0f;
	public float maxZ = 100.0f;
	
	// store these values internally, they are the accumulation of 
	// of nested positions
	private float _minX;
	private float _maxX;
	private float _minY;
	private float _maxY;
	private float _minZ;
	private float _maxZ;
	
	// link to another object that has PositionBoundry
	public GameObject otherPositionBoundry;
	private PositionBoundry _otherPositionBoundry;
	
	// set this to see the bounds for this object
	public bool drawBounds = true;
	public bool drawCollectedBounds = false;
	
	// Use this for initialization
	void Start () {
		_minX = minX;
		_maxX = maxX;
		_minY = minY;
		_maxY = maxY;
		_minZ = minZ;
		_maxZ = maxZ;
		
		// check if this is linked to another PositionBoundry
		// if so, we'll check those values for smaller boundries
		if (otherPositionBoundry != null) {
			_otherPositionBoundry = (PositionBoundry) otherPositionBoundry.GetComponent("PositionBoundry");
			if (_otherPositionBoundry != null) {
				float otherMinX = _otherPositionBoundry.GetMinXDeep();
				float otherMaxX = _otherPositionBoundry.GetMaxXDeep();
				float otherMinY = _otherPositionBoundry.GetMinYDeep();
				float otherMaxY = _otherPositionBoundry.GetMaxYDeep();
				float otherMinZ = _otherPositionBoundry.GetMinZDeep();
				float otherMaxZ = _otherPositionBoundry.GetMaxZDeep();
				if (otherMinX > _minX) _minX = otherMinX;
				if (otherMaxX < _maxX) _maxX = otherMaxX;
				if (otherMinY > _minY) _minY = otherMinY;
				if (otherMaxY < _maxY) _maxY = otherMaxY;
				if (otherMinZ > _minZ) _minZ = otherMinZ;
				if (otherMaxZ < _maxZ) _maxZ = otherMaxZ;
			}
		}
		
		// check the values, and give some warning if things are borked
		if (_minX > _maxX) Debug.LogWarning("PositionBoundry: object " + name + " calculated a minX value of " + minX + " and maxX value of " + maxX + ". Position may get messed up.");
		if (_minY > _maxY) Debug.LogWarning("PositionBoundry: object " + name + " calculated a minY value of " + minY + " and maxY value of " + maxY + ". Position may get messed up.");
		if (_minZ > _maxZ) Debug.LogWarning("PositionBoundry: object " + name + " calculated a minZ value of " + minX + " and maxZ value of " + maxZ + ". Position may get messed up.");

	}
	
	// LateUpdate is called once per frame at the end
	void LateUpdate () {
		// check the x position
		if (transform.position.x < _minX) {
			transform.position = (Vector3.right * _minX) + (Vector3.up * transform.position.y) + (Vector3.forward * transform.position.z);
		}
		else if (transform.position.x > _maxX) {
			transform.position = (Vector3.right * _maxX) + (Vector3.up * transform.position.y) + (Vector3.forward * transform.position.z);
		}
		
		// check the y position
		if (transform.position.y < _minY) {
			transform.position = (Vector3.right * transform.position.x) + (Vector3.up * _minY) + (Vector3.forward * transform.position.z);
		}
		else if (transform.position.y > _maxY) {
			transform.position = (Vector3.right * transform.position.x) + (Vector3.up * _maxY) + (Vector3.forward * transform.position.z);
		}
		
		// check the z position
		if (transform.position.z < _minZ) {
			transform.position = (Vector3.right *transform.position.x) + (Vector3.up * transform.position.y) + (Vector3.forward * _minZ);
		}
		else if (transform.position.z > _maxZ) {
			transform.position = (Vector3.right * transform.position.x) + (Vector3.up * transform.position.y) + (Vector3.forward * _maxZ);
		}
	}
	
	// get internal value for axis boundries
	float GetMinX() {
		return _minX;
	}
	float GetMaxX() {
		return _maxX;
	}
	float GetMinY() {
		return _minY;
	}
	float GetMaxY() {
		return _maxY;
	}
	float GetMinZ() {
		return _minZ;
	}
	float GetMaxZ() {
		return _maxZ;
	}
	
	// recurse through nested otherPositionBoundry(s)
	// to find the smallest value for the axis boundries
	float GetMinXDeep() {
		float deepMinX = minX;
		if (_otherPositionBoundry != null) {
			float otherDeepMinX = _otherPositionBoundry.GetMinXDeep();
			if (otherDeepMinX > deepMinX) {
				deepMinX = otherDeepMinX;
			}
		}
		return deepMinX;
	}
	float GetMaxXDeep() {
		float deepMaxX = maxX;
		if (_otherPositionBoundry != null) {
			float otherDeepMaxX = _otherPositionBoundry.GetMaxXDeep();
			if (otherDeepMaxX < deepMaxX) {
				deepMaxX = otherDeepMaxX;
			}
		}
		return deepMaxX;
	}
	float GetMinYDeep() {
		float deepMinY = minY;
		if (_otherPositionBoundry != null) {
			float otherDeepMinY = _otherPositionBoundry.GetMinYDeep();
			if (otherDeepMinY > deepMinY) {
				deepMinY = otherDeepMinY;
			}
		}
		return deepMinY;
	}
	float GetMaxYDeep() {
		float deepMaxY = maxY;
		if (_otherPositionBoundry != null) {
			float otherDeepMaxY = _otherPositionBoundry.GetMaxYDeep();
			if (otherDeepMaxY < deepMaxY) {
				deepMaxY = otherDeepMaxY;
			}
		}
		return deepMaxY;
	}
	float GetMinZDeep() {
		float deepMinZ = minZ;
		if (_otherPositionBoundry != null) {
			float otherDeepMinZ = _otherPositionBoundry.GetMinZDeep();
			if (otherDeepMinZ > deepMinZ) {
				deepMinZ = otherDeepMinZ;
			}
		}
		return deepMinZ;
	}
	float GetMaxZDeep() {
		float deepMaxZ = maxZ;
		if (_otherPositionBoundry != null) {
			float otherDeepMaxZ = _otherPositionBoundry.GetMaxZDeep();
			if (otherDeepMaxZ < deepMaxZ) {
				deepMaxZ = otherDeepMaxZ;
			}
		}
		return deepMaxZ;
	}
	
	// set internal value for axis boundries to over-ride all calculated boundries
	void GetMinX(float x) {
		_minX = x;
	}
	void GetMaxX(float x) {
		 _maxX = x;
	}
	void GetMinY(float y) {
		 _minY = y;
	}
	void GetMaxY(float y) {
		 _maxY = y;
	}
	void GetMinZ(float z) {
		 _minZ = z;
	}
	void GetMaxZ(float z) {
		 _maxZ = z;
	}
	
	// draw the bounds as represented by a box
	void OnDrawGizmosSelected () {
		if (drawBounds && !drawCollectedBounds) {
			Vector3 right = Vector3.right * maxX;
			Vector3 left = Vector3.right * minX;
			Vector3 top = Vector3.up * maxY;
			Vector3 bottom = Vector3.up * minY;
			Vector3 front = Vector3.forward * maxZ;
			Vector3 back = Vector3.forward * minZ;
			
			Gizmos.color = Color.red;
			// draw the top of the box
			Gizmos.DrawLine(top+left+front, top+left+back);
			Gizmos.DrawLine(top+right+front, top+right+back);
			Gizmos.DrawLine(top+front+right, top+front+left);
			Gizmos.DrawLine(top+back+right, top+back+left);
			
			// draw the bottom of the box
			Gizmos.DrawLine(bottom+left+front, bottom+left+back);
			Gizmos.DrawLine(bottom+right+front, bottom+right+back);
			Gizmos.DrawLine(bottom+front+right, bottom+front+left);
			Gizmos.DrawLine(bottom+back+right, bottom+back+left);
			
			// draw the sides of the box
			Gizmos.DrawLine(left+front+top, left+front+bottom);
			Gizmos.DrawLine(right+front+top, right+front+bottom);
			Gizmos.DrawLine(left+back+top, left+back+bottom);
			Gizmos.DrawLine(right+back+top, right+back+bottom);
		}
		else if (drawBounds && drawCollectedBounds) {
			if (otherPositionBoundry != null) _otherPositionBoundry = (PositionBoundry)otherPositionBoundry.GetComponent("PositionBoundry");
			Vector3 right = Vector3.right * GetMaxXDeep();
			Vector3 left = Vector3.right * GetMinXDeep();
			Vector3 top = Vector3.up * GetMaxYDeep();
			Vector3 bottom = Vector3.up * GetMinYDeep();
			Vector3 front = Vector3.forward * GetMaxZDeep();
			Vector3 back = Vector3.forward * GetMinZDeep();
			
			Gizmos.color = Color.red;
			// draw the top of the box
			Gizmos.DrawLine(top+left+front, top+left+back);
			Gizmos.DrawLine(top+right+front, top+right+back);
			Gizmos.DrawLine(top+front+right, top+front+left);
			Gizmos.DrawLine(top+back+right, top+back+left);
			
			// draw the bottom of the box
			Gizmos.DrawLine(bottom+left+front, bottom+left+back);
			Gizmos.DrawLine(bottom+right+front, bottom+right+back);
			Gizmos.DrawLine(bottom+front+right, bottom+front+left);
			Gizmos.DrawLine(bottom+back+right, bottom+back+left);
			
			// draw the sides of the box
			Gizmos.DrawLine(left+front+top, left+front+bottom);
			Gizmos.DrawLine(right+front+top, right+front+bottom);
			Gizmos.DrawLine(left+back+top, left+back+bottom);
			Gizmos.DrawLine(right+back+top, right+back+bottom);
		}
	}
}
