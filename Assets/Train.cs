using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour {
    public Track Track;
    public float Speed;

    private int _nextPoint;
    private float _distanceTravelled;

    void Start() {
        _nextPoint = 1;
    }

    void Update() {
        if (Track.Points.Count < 2) {
            return;
        }

        _distanceTravelled += Speed * Time.deltaTime;

        var initialPoint = _nextPoint;
        float distanceNeeded;
        GameObject previousPoint;
        GameObject targetPoint;
        while (true) {
            // Can potentially skip N points if they're very close together, so this loop handles that logic.
            // There would be certain cases when an infinite loop could be possible (Points are in the same position),
            // but we test those explicitly.
            previousPoint = Track.Point(_nextPoint - 1);
            targetPoint = Track.Point(_nextPoint);
            distanceNeeded = PointDistance(previousPoint, targetPoint);
            if (_distanceTravelled > distanceNeeded) {
                _distanceTravelled -= distanceNeeded;
                _nextPoint = (_nextPoint + 1) % Track.Points.Count;
                if (_nextPoint == initialPoint) {
                    // Edge case: We did a full rotation in one move. Prevent potential infinite loop and break.
                    break;
                }
            } else {
                break;
            }
        }

        var percentageToNextPoint = _distanceTravelled / distanceNeeded;
        transform.position = Vector3.Lerp(previousPoint.transform.position, targetPoint.transform.position,
            percentageToNextPoint);
        transform.rotation = Quaternion.Euler(0, -90, 0) * Quaternion.Lerp(previousPoint.transform.rotation, targetPoint.transform.rotation,
            percentageToNextPoint);
    }

    private float PointDistance(GameObject a, GameObject b) {
        return Vector3.Distance(a.transform.position, b.transform.position);
    }
}
