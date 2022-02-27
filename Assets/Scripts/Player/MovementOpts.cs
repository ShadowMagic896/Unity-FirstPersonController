// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class MovementOpts : MonoBehaviour
// {
//     [SerializeField, Range(0, 10)] float m_WalkMultiplier;
//     [SerializeField, Range(0, 10)] float m_CrouchMultiplier;
//     [SerializeField, Range(0, 10)] float m_SprintMultiplier;
//     [SerializeField, Range(0, 10)] float m_MovementSpeed;


//     void Start() {
//         // Default values
//         m_WalkMultiplier = 0.75f;
//         m_CrouchMultiplier = 0.5f;
//         m_SprintMultiplier = 2f;
//         m_MovementSpeed = 5f;
//     }


//     void Update() {
//         MovementController.WalkMultiplier = m_WalkMultiplier;
//         MovementController.CrouchMultiplier = m_CrouchMultiplier;
//         MovementController.SprintMultiplier = m_SprintMultiplier;
//         MovementController.MovementSpeed = m_MovementSpeed;
//     }

//     void OnGUI() {
//         float min = 0.1f, max = 10f;
//         m_WalkMultiplier = GUI.HorizontalSlider(new Rect(20, 20, 100, 40), m_WalkMultiplier, min, max);
//         m_CrouchMultiplier = GUI.HorizontalSlider(new Rect(20, -20, 100, 40), m_CrouchMultiplier, min, max);
//         m_SprintMultiplier = GUI.HorizontalSlider(new Rect(20, -60, 100, 40), m_SprintMultiplier, min, max);
//         m_MovementSpeed = GUI.HorizontalSlider(new Rect(20, -100, 100, 40), m_MovementSpeed, min, max);
//     }
// }
