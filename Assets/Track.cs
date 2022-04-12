using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour {
    public List<Vector3> Points;
    private List<GameObject> _pointObjects;

    void Start() {
        InitializePoints();
    }

    void InitializePoints() {
        if (this == null) {
            return;
        }

        // Clear any old points.
        for (var i = this.transform.childCount - 1; i >= 0; i--) {
            var child = transform.GetChild(i);
            if (child.name.StartsWith("Point ")) {
                DestroyImmediate(child.gameObject);
            }
        }

        _pointObjects = new List<GameObject>();

        var pointPrefab = (GameObject) Resources.Load("Point");
        GameObject previousPointObject = null;
        for (var i = 0; i < Points.Count; i++) {
            var pointObject = Instantiate(pointPrefab, Points[i], Quaternion.identity);
            pointObject.transform.SetParent(transform);
            pointObject.name = $"Point {i}";
            if (previousPointObject != null) {
                previousPointObject.transform.LookAt(pointObject.transform);
            }

            _pointObjects.Add(pointObject);
            previousPointObject = pointObject;
        }

        if (previousPointObject != null) {
            previousPointObject.transform.LookAt(_pointObjects[0].transform);
        }
    }

    void OnValidate() {
        // Not allowed to do SetParent in OnValidate, so putting it in a delayCall.
        UnityEditor.EditorApplication.delayCall += InitializePoints;
    }

    public GameObject Point(int point) {
        if (_pointObjects == null || _pointObjects.Count == 0) {
            return null;
        }

        return point == -1 ? _pointObjects[_pointObjects.Count - 1] : _pointObjects[point];
    }
}
