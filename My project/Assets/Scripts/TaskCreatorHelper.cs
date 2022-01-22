// Класс помошник для окна создания задачи

using System;
using System.Collections;
using UnityEngine;
using TMPro;

namespace Tasking
{
    public class TaskCreatorHelper : MonoBehaviour
    {
        const float FLASH_DURATION = 0.2f; // Длительность "моргания" когда пользователь не ввёл назавние задачи

        // ссылки на объекты в префабе
        [SerializeField] TMP_InputField taskName;
        [SerializeField] TMP_InputField taskAssignee;
        [SerializeField] TMP_Dropdown taskDueTimeDay;
        [SerializeField] TMP_Dropdown taskDueTimeMonth;
        [SerializeField] TMP_Dropdown taskStatus;

        Animator myAnimator;

        private void OnEnable()
        {
            if (myAnimator == null)
            {
                myAnimator = GetComponent<Animator>();
            }

            myAnimator.Play("OnEnable");
            myAnimator.Update(0);

            taskStatus.options.Clear();
            taskDueTimeMonth.options.Clear();

            // Заполнить dropdown опции возможными статусами задачи/датой
            for (int i = 0; i < TaskingManager.Instance.PossibleTaskStatuses; i++)
            {
                taskStatus.options.Add(new TMP_Dropdown.OptionData(TaskingUIManager.Instance.TaskStatusName[(TaskingStatus)i]));
            }
            for (int i = 0; i < 12; i++)
            {
                taskDueTimeMonth.options.Add(new TMP_Dropdown.OptionData(new DateTime(DateTime.Now.Year, i + 1, 1).ToString("MMM")));
            }

            taskName.text = string.Empty;
            taskAssignee.text = string.Empty;
            taskStatus.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = taskStatus.options[0].text;
            taskDueTimeMonth.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = taskDueTimeMonth.options[0].text;

            taskStatus.value = 0;
            taskDueTimeDay.value = 0;
            taskDueTimeMonth.value = 0;
            UpdateDaysInMonth(0);
        }
        // Метод для обновления списка с днями месяца
        public void UpdateDaysInMonth(int newMonth)
        {
            int currentChoosenDay = taskDueTimeDay.value;

            taskDueTimeDay.options.Clear();

            // Заполнить список дней днями в месяце
            for (int i = 0; i < DateTime.DaysInMonth(DateTime.Now.Year, newMonth + 1); i++)
            {
                taskDueTimeDay.options.Add(new TMP_Dropdown.OptionData((i + 1).ToString()));
            }
            // Переместить курсор выбранного для если выбранный ранне день превышает количество дней в новом месяце
            if (currentChoosenDay > taskDueTimeDay.options.Count)
            {
                taskDueTimeDay.value = taskDueTimeDay.options.Count;
            }

            taskDueTimeDay.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = taskDueTimeDay.options[taskDueTimeDay.value].text;
        }
        // Метод для создания задачи с указанными данными 
        public void CreateTask()
        {
            // Если строка с названием задачи пустая, не создавать задачу
            // моргнуть чтоб дать пользователю знать что поле обязательно нужно заполнить
            if (taskName.text == string.Empty || taskName.text == " ")
            {
                StartCoroutine(FlashInputFields());
                return;
            }

            TaskingManager.Instance.CreateTask(taskName.text, taskAssignee.text, (TaskingStatus)taskStatus.value, taskDueTimeDay.value + 1, taskDueTimeMonth.value + 1);
            // Убрать окно
            gameObject.SetActive(false);
        }

        IEnumerator FlashInputFields()
        {
            taskName.interactable = false;
            yield return new WaitForSeconds(FLASH_DURATION);
            taskName.interactable = true;
        }
    }
}