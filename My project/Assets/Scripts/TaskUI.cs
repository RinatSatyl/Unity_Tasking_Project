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
        [SerializeField] TMP_Text taskStatusText;
        [SerializeField] RawImage taskStatusBackground;
        [SerializeField] TMP_Text taskDueTime;
        
        // оранжевый
        Color orange = new Color(255, 215, 0);
        // дата задачи
        DateTime dueDate = new DateTime();
        
        // Метод
        // Извлекает информацию задачи и передаёт её UI элементам
        public void SetInformation(TaskManager.TaskingTask myTask)
        {
            // Задать текст название задачи, кому поручено, текущий статус, дату окончания
            taskName.text = myTask.name;
            taskAssignee.text = myTask.assignee;
            taskStatusText.text = myTask.status.ToString();
            taskDueTime.text = myTask.dueDate.ToString("MMMM dd");

            // Установить цвет поля статуса в зависимости от статуса задачи
            switch (myTask.status)
            {
                case TaskManager.TaskingStatus.COMPLETED:
                    taskStatusBackground.color = Color.green;
                    break;
                case TaskManager.TaskingStatus.IN_PROGRESS:
                    taskStatusBackground.color = orange;
                    break;
                case TaskManager.TaskingStatus.OPEN:
                    taskStatusBackground.color = Color.gray;
                    break;
                case TaskManager.TaskingStatus.REVIEWING:
                    taskStatusBackground.color = Color.yellow;
                    break;
                default:
                    taskStatusBackground.color = Color.white;
                    break;
            }

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