using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToMouse : MonoBehaviour {
    Quaternion m_MyQuaternion;
    float m_Speed = 1.0f;
    // Use this for initialization
    void Start () {

		m_MyQuaternion = new Quaternion();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 m_MousePosition = Input.mousePosition;
        m_MousePosition.z = 5.0f;
        m_MousePosition = Camera.current.ScreenToWorldPoint(m_MousePosition);
        //Debug.Log(m_MousePosition);
        //Set the Quaternion rotation from the GameObject's position to the mouse position
        m_MyQuaternion.SetFromToRotation(transform.position, m_MousePosition);
        //Move the GameObject towards the mouse position
        transform.position = Vector3.Lerp(transform.position, m_MousePosition, m_Speed * Time.deltaTime);
        //Rotate the GameObject towards the mouse position
        transform.rotation = m_MyQuaternion * transform.rotation;
    }
    
}
