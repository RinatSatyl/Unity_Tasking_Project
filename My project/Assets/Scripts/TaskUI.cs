// Класс для контролирования показываемой информации в ячейке в списке задач
// Так же хранит в себе ссылку на задачу

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Tasking
{
    public class TaskUI : MonoBehaviour
    {
        // ссылки на объекты в префабе
        [SerializeField] TMP_Text taskName;
        [SerializeField] TMP_Text taskAssignee;
        [SerializeField] Image taskStatusBackground;
        [SerializeField] TMP_Text taskDueTime;
        [SerializeField] TMP_Dropdown taskStatusDropdown;

        // дата задачи
        int dueDay = 0;
        int dueMonth = 0;
        string thisTaskName = string.Empty;

        // Публичная ссылка на taskName
        public string TaskName { get { return thisTaskName; } }
        public int TaskStatus { get { return taskStatusDropdown.value; } }

        // Метод
        // Извлекает информацию задачи и передаёт её UI элементам
        public void SetInformation(TaskingTask receivedTask)
        {
            thisTaskName = receivedTask.id;

            // Заполнить dropdown опции возможными статусами задачи
            for (int i = 0; i < TaskManager.Instance.PossibleTaskStatuses; i++)
            {
                string statusName = TaskUIManager.Instance.TaskStatusName[(TaskManager.TaskingStatus)i];
                taskStatusDropdown.options.Add(new TMP_Dropdown.OptionData(statusName));
            }

            // Задать текст название задачи, кому поручено, текущий статус, дату окончания
            taskName.text = receivedTask.name;
            taskAssignee.text = receivedTask.assignee;
            taskStatusDropdown.value = (int)receivedTask.status;
            taskDueTime.text = new DateTime(DateTime.Now.Year, receivedTask.month, receivedTask.day).ToString("MMMM dd");

            // Поставить цвет фона переключателя статуса
            taskStatusBackground.color = TaskUIManager.Instance.TaskColor[(TaskManager.TaskingStatus)receivedTask.status];

            // Скопировать дату окончания
            dueDay = receivedTask.day;
            dueMonth = receivedTask.month;

            taskStatusDropdown.onValueChanged.AddListener(UpdateStatus);
        }

        // Метод для обновления статуса этой задачи
        public void UpdateStatus(int newState)
        {
            TaskManager.Instance.UpdateTask(taskName.text, taskAssignee.text, (TaskManager.TaskingStatus)newState, dueDay, dueMonth);
            // Поставить цвет фона переключателя статуса
            taskStatusBackground.color = TaskUIManager.Instance.TaskColor[(TaskManager.TaskingStatus)newState];
        }
        public void DeleteTask()
        {
            TaskManager.Instance.DeleteTask(thisTaskName);
        }

        private void OnDestroy()
        {
            taskStatusDropdown.onValueChanged.RemoveAllListeners();
        }
    }
}