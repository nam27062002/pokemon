// using System;
// using System.Collections;
// using System.Collections.Generic;
// using TMPro;
// using UnityEngine;
//
// namespace _Scripts.UI
// {
//     public class ReloadMapManager : MonoBehaviour
//     {
//         [SerializeField] private float timeCountdown = 3f;
//         private float timer;
//         [SerializeField] private TextMeshProUGUI TextMeshProUGUI;
//         public event EventHandler OnFinishedCountdown;
//         
//         private void Awake()
//         {
//             ShowHideAllChildObject(false);
//         }
//
//         private void Start()
//         {
//             timer = timeCountdown;
//             GameManager.Instance.OnLoadNewMap += InstanceOnOnLoadNewMap;
//         }
//
//         private void InstanceOnOnLoadNewMap(object sender, EventArgs e)
//         {
//             ShowHideAllChildObject(true);
//             StartCoroutine(SetCountDown());
//         }
//
//         private void ShowHideAllChildObject(bool isActice)
//         {
//             foreach (Transform child in transform)
//             {
//                 child.gameObject.SetActive(isActice);
//             }
//         }
//
//         IEnumerator SetCountDown()
//         {
//             while (timer > 0)
//             {
//                 TextMeshProUGUI.text = timer.ToString();
//                 timer--;
//                 yield return new WaitForSeconds(1f);
//             }
//
//             timer = timeCountdown;
//             ShowHideAllChildObject(false);
//             OnFinishedCountdown?.Invoke(this,EventArgs.Empty);
//             
//         }
//     }
// }
