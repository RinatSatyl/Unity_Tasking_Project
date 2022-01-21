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
            public int day;
            public int month;

            public TaskingTask(string name, string assignee, TaskingStatus status, int day, int month)
            {
                this.name = name;
                this.assignee = assignee;
                this.status = status;
                this.day = day;
                this.month = month;
            }
        }
        // Статус задачи
        [System.Serializable]
        public enum TaskingStatus : int
        {
            OPEN = 0,
            COMPLETED = 1,
            IN_PROGRESS = 2,
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

        private void Start()
        {
            Instance = this;
        }

        // Метод добавляющии задачу с указаной информацией в список задач 
        public void CreateTask(string taskName, string taskAssignee, TaskingStatus taskStatus, int day, int month)
        {
            // Создать новый объект задачи
            TaskingTask newTask = new TaskingTask(taskName, taskAssignee, taskStatus, day, month);

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
        public void UpdateTask(string taskName, string taskAssignee, TaskingStatus taskStatus, int day, int month)
        {
            // Создать новый объект задачи
            TaskingTask updatedTask = new TaskingTask(taskName, taskAssignee, taskStatus, day, month);

            int count = 0;
            foreach (TaskingTask thisTask in taskList)
            {
                if (thisTask.name == taskName)
                {
                    break;
                }
                count++;
            }
            Debug.Log(count);

            taskList.RemoveAt(count);
            taskList.Insert(count, updatedTask);
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
            foreach (TaskingTask thisTask in taskList)
            {
                // Вызвать эвент с ссылкой на объект задачи
                TaskCreated.Invoke(thisTask);
            }
        }
    }
}