using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Tasking
{
    public class TaskCreatorUI : MonoBehaviour
    {
        const float FLASH_DURATION = 0.2f;

        // ссылки на объекты в префабе
        [SerializeField] TMP_InputField taskName;
        [SerializeField] TMP_InputField taskAssignee;
        [SerializeField] TMP_Dropdown taskDueTimeDay;
        [SerializeField] TMP_Dropdown taskDueTimeMonth;
        [SerializeField] TMP_Dropdown taskStatus;

        private void OnEnable()
        {
            taskStatus.options.Clear();
            taskDueTimeMonth.options.Clear();

            // Заполнить dropdown опции возможными статусами задачи
            for (int i = 0; i < TaskManager.Instance.PossibleTaskStatuses; i++)
            {
                string statusName = ((TaskManager.TaskingStatus)i).ToString(); ;
                taskStatus.options.Add(new TMP_Dropdown.OptionData(statusName));
            }

            for (int i = 0; i < 12; i++)
            {
                taskDueTimeMonth.options.Add(new TMP_Dropdown.OptionData((i + 1).ToString()));
            }

            taskName.text = string.Empty;
            taskAssignee.text = string.Empty;
            taskStatus.value = 0;
            taskStatus.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = taskStatus.options[0].text;
            taskDueTimeMonth.value = 0;
            taskDueTimeMonth.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = taskDueTimeMonth.options[0].text;
            UpdateDaysInMonth(0);
        }

        public void UpdateDaysInMonth(int value)
        {
            taskDueTimeDay.options.Clear();

            for (int i = 0; i < DateTime.DaysInMonth(DateTime.Now.Year, value + 1); i++)
            {
                taskDueTimeDay.options.Add(new TMP_Dropdown.OptionData((i + 1).ToString()));
            }

            taskDueTimeDay.value = 0;
            taskDueTimeDay.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = taskDueTimeDay.options[0].text;
        }

        public void CreateTask()
        {
            if (taskName.text == string.Empty || taskName.text == " ")
            {
                StartCoroutine(FlashInputFields());
                return;
            }

            TaskManager.Instance.CreateTask(taskName.text, taskAssignee.text, (TaskManager.TaskingStatus)taskStatus.value, new DateTime(DateTime.Now.Year, taskDueTimeMonth.value + 1, taskDueTimeDay.value + 1));
            gameObject.SetActive(false);
        }

        IEnumerator FlashInputFields()
        {
            taskName.interactable = false;
            taskAssignee.interactable = false;
            yield return new WaitForSeconds(FLASH_DURATION);
            taskName.interactable = true;
            taskAssignee.interactable = true;
        }
    }
}