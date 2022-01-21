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
        DateTime dueDate = new DateTime();
        string thisTaskName = string.Empty;

        // Публичная ссылка на taskName
        public string TaskName { get { return thisTaskName; } }

        private void Start()
        {
            // Заполнить dropdown опции возможными статусами задачи
            for (int i = 0; i < TaskManager.Instance.PossibleTaskStatuses; i++)
            {
                string statusName = ((TaskManager.TaskingStatus)i).ToString(); ;
                taskStatusDropdown.options.Add(new TMP_Dropdown.OptionData(statusName));
            }
        }

        // Метод
        // Извлекает информацию задачи и передаёт её UI элементам
        public void SetInformation(TaskManager.TaskingTask myTask)
        {
            // Задать текст название задачи, кому поручено, текущий статус, дату окончания
            taskName.text = myTask.name;
            taskAssignee.text = myTask.assignee;
            taskStatusDropdown.value = (int)myTask.status;
            taskDueTime.text = myTask.dueDate.ToString("MMMM dd");

            // Поставить цвет фона переключателя статуса
            taskStatusBackground.color = TaskUIManager.Instance.TaskColor[myTask.status];

            // Скопировать дату окончания
            dueDate = myTask.dueDate;
        }

        // Метод для обновления статуса этой задачи
        public void UpdateStatus(int newState)
        {
            TaskManager.Instance.UpdateTask(taskName.text, taskAssignee.text, (TaskManager.TaskingStatus)newState, dueDate);
        }
    }
}