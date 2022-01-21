// Класс содержащий список задач, логика для добавления, удаления задач, обновления информации в задаче.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Tasking
{
    public class TaskManager : MonoBehaviour
    {
        // Стракт задачи с информацией
        [System.Serializable]
        public struct TaskingTask
        {
            public string name;
            public string assignee;
            public TaskingStatus status;
            public DateTime dueDate;

            public TaskingTask(string name, string assignee, TaskingStatus status, DateTime dueDate)
            {
                this.name = name;
                this.assignee = assignee;
                this.status = status;
                this.dueDate = dueDate;
            }
        }
        // Статус задачи
        public enum TaskingStatus : int
        {
            COMPLETED = 0,
            IN_PROGRESS = 1,
            OPEN = 2,
            REVIEWING = 3
        }

        // Список задач
        List<TaskingTask> taskList = new List<TaskingTask>();

        // Публичная только для чтения ссылка на список задач
        public List<TaskingTask> TaskList { get { return taskList; } }
        public int PossibleTaskStatuses = 4;

        // Эвенты для обновления UI
        [SerializeField] UnityEvent<TaskingTask> TaskCreated;
        [SerializeField] UnityEvent<TaskingTask> TaskDeleted;
        [SerializeField] UnityEvent<TaskingTask> TaskUpdated;

        // Статичная ссылка на TaskManager, для легкого доступа
        public static TaskManager Instance;

        // Метод добавляющии задачу с указаной информацией в список задач 
        public void CreateTask(string taskName, string taskAssignee, TaskingStatus taskStatus, DateTime taskDueDate)
        {
            // Создать новый объект задачи
            TaskingTask newTask = new TaskingTask(taskName, taskAssignee, taskStatus, taskDueDate);

            // Добавит объект в список задач
            taskList.Add(newTask);

            // Вызвать эвент с ссылкой на объект задачи
            TaskCreated.Invoke(newTask);
        }
        // Метод для удаления задачи с списка задач
        public void DeleteTask(string taskName)
        {
            foreach (TaskingTask taskInList in taskList)
            {
                if (taskInList.name == taskName)
                {
                    taskList.Remove(taskInList);
                    return;
                }
            }
        }
        // Метод для обновления информации задачи
        public void UpdateTask(string taskName, string taskAssignee, TaskingStatus taskStatus, DateTime taskDueDate)
        {
            foreach (TaskingTask taskInList in taskList)
            {
                if (taskInList.name == taskName)
                {
                    // Получить ссылку на объект задачи
                    TaskingTask taskToEdit = taskInList;
                    // Редактировать информацию
                    taskToEdit.assignee = taskAssignee;
                    taskToEdit.status = taskStatus;
                    taskToEdit.dueDate = taskDueDate;
                    return;
                }
            }
        }
        // Метод для зачистки списка задач
        public void ClearTaskList()
        {
            taskList.Clear();
        }
        // Метод для замены текущего списка полученым из вне
        public void ApplyNewTaskList(List<TaskingTask> newTaskList)
        {
            taskList = newTaskList;
        }
    }
}